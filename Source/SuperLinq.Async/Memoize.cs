using System.Runtime.ExceptionServices;

namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Creates a sequence that lazily caches the source as it is iterated for the first time, reusing the cache
	/// thereafter for future re-iterations. By default, all sequences are cached, whether they are instantiated or
	/// lazy.
	/// </summary>
	/// <typeparam name="TSource">
	/// Type of elements in <paramref name="source"/>.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <returns>
	/// Returns a sequence that corresponds to a cached version of the input sequence.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	/// <paramref name="source"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	/// The returned <see cref="IAsyncBuffer{T}"/> will cache items from <paramref name="source"/> in a thread-safe
	/// manner. The sequence supplied in <paramref name="source"/> is not expected to be thread-safe but it is required
	/// to be thread-agnostic.
	/// </para>
	/// </remarks>
	public static IAsyncBuffer<TSource> Memoize<TSource>(this IAsyncEnumerable<TSource> source)
	{
		ArgumentNullException.ThrowIfNull(source);

		return new EnumerableMemoizedBuffer<TSource>(source);
	}

	private sealed class EnumerableMemoizedBuffer<T> : IAsyncBuffer<T>
	{
		private readonly SemaphoreSlim _lock = new(initialCount: 1);

		private IAsyncEnumerable<T>? _source;

		private IAsyncEnumerator<T>? _enumerator;
		private List<T> _buffer = new();
		private bool _initialized;

		private ExceptionDispatchInfo? _exception;
		private int? _exceptionIndex;

		private bool _disposed;

		public EnumerableMemoizedBuffer(IAsyncEnumerable<T> source)
		{
			_source = source;
		}

		public int Count => _buffer.Count;

		public async ValueTask Reset(CancellationToken cancellationToken = default)
		{
			if (_disposed)
				ThrowHelper.ThrowObjectDisposedException(nameof(IAsyncBuffer<T>));

			await _lock.WaitAsync(cancellationToken);
			try
			{
				if (_disposed)
					ThrowHelper.ThrowObjectDisposedException(nameof(IAsyncBuffer<T>));

				_buffer = new();
				_initialized = false;
				if (_enumerator != null)
					await _enumerator.DisposeAsync().ConfigureAwait(false);
				_enumerator = null;
				_exceptionIndex = null;
				_exception = null;
			}
			finally
			{
				_ = _lock.Release();
			}
		}

		public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
		{
			InitializeEnumerator(cancellationToken);
			return GetEnumeratorImpl(cancellationToken);
		}

		private void InitializeEnumerator(CancellationToken cancellationToken)
		{
			if (_disposed)
				ThrowHelper.ThrowObjectDisposedException(nameof(IAsyncBuffer<T>));

			_lock.Wait(cancellationToken);
			try
			{
				if (_disposed)
					ThrowHelper.ThrowObjectDisposedException(nameof(IAsyncBuffer<T>));

				Assert.NotNull(_source);

				if (_exceptionIndex == -1)
				{
					ArgumentNullException.ThrowIfNull(_exception);
					_exception.Throw();
				}

				if (_initialized)
					return;

				try
				{
					_enumerator = _source.GetAsyncEnumerator(cancellationToken);
					_initialized = true;
				}
				catch (Exception ex)
				{
					_exception = ExceptionDispatchInfo.Capture(ex);
					_exceptionIndex = -1;
					throw;
				}
			}
			finally
			{
				_ = _lock.Release();
			}
		}

		private async IAsyncEnumerator<T> GetEnumeratorImpl(CancellationToken cancellationToken)
		{
			var buffer = _buffer;
			var index = 0;
			while (true)
			{
				T? element;

				if (_disposed)
					ThrowHelper.ThrowObjectDisposedException(nameof(IAsyncBuffer<T>));

				await _lock.WaitAsync(cancellationToken).ConfigureAwait(false);
				try
				{
					if (_disposed)
						ThrowHelper.ThrowObjectDisposedException(nameof(IAsyncBuffer<T>));
					if (!_initialized
						|| buffer != _buffer)
					{
						ThrowHelper.ThrowInvalidOperationException("Buffer reset during iteration.");
					}

					if (index >= _buffer.Count)
					{
						if (index == _exceptionIndex)
						{
							Assert.NotNull(_exception);
							_exception.Throw();
						}

						if (_enumerator == null)
							break;

						var moved = false;
						try
						{
							moved = await _enumerator.MoveNextAsync(cancellationToken).ConfigureAwait(false);
						}
						catch (Exception ex)
						{
							_exception = ExceptionDispatchInfo.Capture(ex);
							_exceptionIndex = index;
							await _enumerator.DisposeAsync().ConfigureAwait(false);
							_enumerator = null;
							throw;
						}

						if (!moved)
						{
							await _enumerator.DisposeAsync().ConfigureAwait(false);
							_enumerator = null;
							break;
						}

						_buffer.Add(_enumerator.Current);

						Assert.True(index < _buffer.Count);
					}

					element = _buffer[index];
				}
				finally
				{
					_ = _lock.Release();
				}

				yield return element;
				index++;
			}
		}

		public async ValueTask DisposeAsync()
		{
			if (_disposed)
				return;

			await _lock.WaitAsync().ConfigureAwait(false);
			try
			{
				_disposed = true;
				_buffer.Clear();
				if (_enumerator != null)
					await _enumerator.DisposeAsync().ConfigureAwait(false);
				_enumerator = null;
				_source = null;
			}
			finally
			{
				_ = _lock.Release();
				_lock.Dispose();
			}
		}
	}
}
