using System.Collections;
using System.Runtime.ExceptionServices;

namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Creates a buffer with a view over the source sequence, causing each enumerator to obtain access to the
	///     remainder of the sequence from the current index in the buffer.
	/// </summary>
	/// <typeparam name="TSource">
	///	    Source sequence element type.
	/// </typeparam>
	/// <param name="source">
	///	    Source sequence.
	/// </param>
	/// <returns>
	///	    Buffer enabling each enumerator to retrieve elements from the shared source sequence, starting from the
	///     index at the point of obtaining the enumerator.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    A separate buffer will be maintained for each <see cref="IEnumerator{T}"/> created from the returned <see
	///	    cref="IEnumerable{T}"/>. This buffer will be maintained until the enumerator is disposed, and will contain
	///	    all elements returned by <paramref name="source"/> from the time that the <see cref="IEnumerator{T}"/> is
	///	    created.
	/// </para>
	/// <para>
	///	    This operator uses deferred execution and streams its result.
	///	</para>
	///	</remarks>
	public static IBuffer<TSource> Publish<TSource>(this IEnumerable<TSource> source)
	{
		ArgumentNullException.ThrowIfNull(source);

		return new PublishBuffer<TSource>(source);
	}

	private sealed class PublishBuffer<T>(
		IEnumerable<T> source
	) : IBuffer<T>
	{
		private readonly object _lock = new();

		private IEnumerable<T>? _source = source;

		private IEnumerator<T>? _enumerator;
		private List<Queue<T>>? _buffers;
		private bool _initialized;
		private int _version;

		private ExceptionDispatchInfo? _exception;
		private bool? _exceptionOnGetEnumerator;

		private bool _disposed;

		public int Count => _buffers?.Count > 0 ? _buffers.Max(x => x.Count) : 0;

		public void Reset()
		{
			lock (_lock)
			{
				if (_disposed)
					ThrowHelper.ThrowObjectDisposedException<IBuffer<T>>();

				_initialized = false;
				_version++;

				_buffers = null;

				_enumerator?.Dispose();
				_enumerator = null;
				_exception = null;
				_exceptionOnGetEnumerator = null;
			}
		}

		public IEnumerator<T> GetEnumerator()
		{
			var (buffer, version) = InitializeEnumerator();
			return GetEnumeratorImpl(buffer, version);
		}

		private (Queue<T> buffer, int version) InitializeEnumerator()
		{
			lock (_lock)
			{
				if (_disposed)
					ThrowHelper.ThrowObjectDisposedException<IBuffer<T>>();

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
						_enumerator = _source.GetEnumerator();
						_buffers = [];
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
		}
		private IEnumerator<T> GetEnumeratorImpl(Queue<T> buffer, int version)
		{
			try
			{
				while (true)
				{
					T? element;
					lock (_lock)
					{
						if (_disposed)
							ThrowHelper.ThrowObjectDisposedException<IBuffer<T>>();
						if (!_initialized
							|| version != _version)
						{
							ThrowHelper.ThrowInvalidOperationException("Buffer reset during iteration.");
						}

						if (buffer.Count == 0)
						{
							_exception?.Throw();

							if (_enumerator is null)
								break;

							var moved = false;
							try
							{
								moved = _enumerator.MoveNext();
							}
							catch (Exception ex)
							{
								_exception = ExceptionDispatchInfo.Capture(ex);
								_exceptionOnGetEnumerator = false;
								_enumerator.Dispose();
								_enumerator = null;
								throw;
							}

							if (!moved)
							{
								_enumerator.Dispose();
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

					yield return element;
				}
			}
			finally
			{
				_ = _buffers?.Remove(buffer);
			}
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public void Dispose()
		{
			lock (_lock)
			{
				_disposed = true;

				_buffers = null;

				_enumerator?.Dispose();
				_enumerator = null;
				_source = null;
			}
		}
	}
}
