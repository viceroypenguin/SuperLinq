using System.Runtime.ExceptionServices;

namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Creates a buffer with a shared view over the source sequence, causing each enumerator to fetch the next element
	/// from the source sequence.
	/// </summary>
	/// <typeparam name="TSource">Source sequence element type.</typeparam>
	/// <param name="source">Source sequence.</param>
	/// <returns>Buffer enabling each enumerator to retrieve elements from the shared source sequence.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/></exception>
	public static IAsyncBuffer<TSource> Share<TSource>(this IAsyncEnumerable<TSource> source)
	{
		Guard.IsNotNull(source);

		return new SharedBuffer<TSource>(source);
	}

	/// <summary>
	/// Shares the source sequence within a selector function where each enumerator can fetch the next element from the
	/// source sequence.
	/// </summary>
	/// <typeparam name="TSource">Source sequence element type.</typeparam>
	/// <typeparam name="TResult">Result sequence element type.</typeparam>
	/// <param name="source">Source sequence.</param>
	/// <param name="selector">Selector function with shared access to the source sequence for each enumerator.</param>
	/// <returns>Sequence resulting from applying the selector function to the shared view over the source
	/// sequence.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is <see
	/// langword="null"/>.</exception>
	public static IAsyncEnumerable<TResult> Share<TSource, TResult>(
		this IAsyncEnumerable<TSource> source,
		Func<IAsyncEnumerable<TSource>, IAsyncEnumerable<TResult>> selector)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(selector);

		return Core(source, selector);

		static async IAsyncEnumerable<TResult> Core(
			IAsyncEnumerable<TSource> source,
			Func<IAsyncEnumerable<TSource>, IAsyncEnumerable<TResult>> selector,
			[EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			await using var buffer = source.Share();
			await foreach (var i in selector(buffer).WithCancellation(cancellationToken).ConfigureAwait(false))
				yield return i;
		}
	}

	private sealed class SharedBuffer<T> : IAsyncBuffer<T>
	{
		private readonly SemaphoreSlim _lock = new(initialCount: 1);

		private IAsyncEnumerable<T>? _source;

		private IAsyncEnumerator<T>? _enumerator;
		private bool _initialized;
		private int _version;

		private ExceptionDispatchInfo? _exception;

		private bool _disposed;

		public SharedBuffer(IAsyncEnumerable<T> source)
		{
			_source = source;
		}

		public int Count => 0;

		public async ValueTask Reset(CancellationToken cancellationToken = default)
		{
			if (_disposed)
				ThrowHelper.ThrowObjectDisposedException(nameof(IAsyncBuffer<T>));

			await _lock.WaitAsync(cancellationToken).ConfigureAwait(false);
			try
			{
				if (_disposed)
					ThrowHelper.ThrowObjectDisposedException(nameof(IAsyncBuffer<T>));

				_initialized = false;
				_version++;

				if (_enumerator != null)
					await _enumerator.DisposeAsync();
				_enumerator = null;
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

				_exception?.Throw();

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
			var version = _version;
			while (true)
			{
				T? element;

				if (_disposed)
					ThrowHelper.ThrowObjectDisposedException(nameof(IAsyncBuffer<T>));

				await _lock.WaitAsync(cancellationToken);
				try
				{
					if (_disposed)
						ThrowHelper.ThrowObjectDisposedException(nameof(IBuffer<T>));
					if (!_initialized
						|| version != _version)
					{
						ThrowHelper.ThrowInvalidOperationException("Buffer reset during iteration.");
					}

					_exception?.Throw();

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

					element = _enumerator.Current;
				}
				finally
				{
					_ = _lock.Release();
				}

				yield return element;
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
