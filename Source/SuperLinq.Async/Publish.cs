﻿using System.Collections;
using System.Runtime.ExceptionServices;

namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Creates a buffer with a view over the source sequence, causing each enumerator to obtain access to the remainder
	/// of the sequence from the current index in the buffer.
	/// </summary>
	/// <typeparam name="TSource">Source sequence element type.</typeparam>
	/// <param name="source">Source sequence.</param>
	/// <returns>
	/// Buffer enabling each enumerator to retrieve elements from the shared source sequence, starting from the index at
	/// the point of obtaining the enumerator.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	public static IAsyncBuffer<TSource> Publish<TSource>(this IAsyncEnumerable<TSource> source)
	{
		Guard.IsNotNull(source);

		return new PublishBuffer<TSource>(source);
	}

	/// <summary>
	/// Publishes the source sequence within a selector function where each enumerator can obtain a view over a tail of
	/// the source sequence.
	/// </summary>
	/// <typeparam name="TSource">Source sequence element type.</typeparam>
	/// <typeparam name="TResult">Result sequence element type.</typeparam>
	/// <param name="source">Source sequence.</param>
	/// <param name="selector">Selector function with published access to the source sequence for each
	/// enumerator.</param>
	/// <returns>Sequence resulting from applying the selector function to the published view over the source
	/// sequence.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is <see
	/// langword="null"/>.</exception>
	public static IAsyncEnumerable<TResult> Publish<TSource, TResult>(
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
			await using var buffer = source.Publish();
			await foreach (var i in selector(buffer).WithCancellation(cancellationToken).ConfigureAwait(false))
				yield return i;
		}
	}

	private sealed class PublishBuffer<T> : IAsyncBuffer<T>
	{
		private readonly SemaphoreSlim _lock = new(initialCount: 1);

		private IAsyncEnumerable<T>? _source;

		private IAsyncEnumerator<T>? _enumerator;
		private List<Queue<T>>? _buffers;
		private bool _initialized;
		private int _version;

		private ExceptionDispatchInfo? _exception;
		private bool? _exceptionOnGetEnumerator;

		private bool _disposed;

		public PublishBuffer(IAsyncEnumerable<T> source)
		{
			_source = source;
		}

		public int Count => _buffers?.Count > 0 ? _buffers.Max(x => x.Count) : 0;

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

				_buffers = null;

				if (_enumerator != null)
					await _enumerator.DisposeAsync();
				_enumerator = null;
				_exception = null;
				_exceptionOnGetEnumerator = null;
			}
			finally
			{
				_ = _lock.Release();
			}
		}

		public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
		{
			var (buffer, version) = InitializeEnumerator(cancellationToken);
			return GetEnumeratorImpl(buffer, version, cancellationToken);
		}

		private (Queue<T> buffer, int version) InitializeEnumerator(CancellationToken cancellationToken)
		{
			if (_disposed)
				ThrowHelper.ThrowObjectDisposedException(nameof(IAsyncBuffer<T>));

			_lock.Wait(cancellationToken);
			try
			{
				if (_disposed)
					ThrowHelper.ThrowObjectDisposedException(nameof(IAsyncBuffer<T>));

				Assert.NotNull(_source);

				if (_exceptionOnGetEnumerator == true)
				{
					Assert.NotNull(_exception);
					_exception.Throw();
				}

				if (!_initialized)
				{
					try
					{
						_enumerator = _source.GetAsyncEnumerator(cancellationToken);
						_buffers = new();
						_initialized = true;
					}
					catch (Exception ex)
					{
						_exception = ExceptionDispatchInfo.Capture(ex);
						_exceptionOnGetEnumerator = true;
						throw;
					}
				}

				Assert.NotNull(_buffers);

				var queue = new Queue<T>();
				_buffers.Add(queue);
				return (queue, _version);
			}
			finally
			{
				_ = _lock.Release();
			}
		}

		private async IAsyncEnumerator<T> GetEnumeratorImpl(Queue<T> buffer, int version, CancellationToken cancellationToken)
		{
			try
			{
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

						if (buffer.Count == 0)
						{
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
								_exceptionOnGetEnumerator = false;
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

							Assert.NotNull(_buffers);

							var current = _enumerator.Current;
							foreach (var q in _buffers)
								q.Enqueue(current);
						}

						element = buffer.Dequeue();
					}
					finally
					{
						_ = _lock.Release();
					}

					yield return element;
				}
			}
			finally
			{
				_ = _buffers?.Remove(buffer);
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

				_buffers = null;

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
