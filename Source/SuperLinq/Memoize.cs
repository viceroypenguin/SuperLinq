using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.ExceptionServices;

namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Creates a sequence that lazily caches the source as it is iterated for the first time, reusing the cache
	///     thereafter for future re-iterations. By default, all sequences are cached, whether they are instantiated or
	///     lazy.
	/// </summary>
	/// <typeparam name="TSource">
	///	    Type of elements in <paramref name="source"/>.
	/// </typeparam>
	/// <param name="source">
	///	    The source sequence.
	/// </param>
	/// <param name="forceCache">
	///	    Force caching of <see cref="ICollection{T}"/>s.
	/// </param>
	/// <returns>
	///	    Returns a sequence that corresponds to a cached version of the input sequence.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    The returned <see cref="IEnumerable{T}"/> will cache items from <paramref name="source"/> in a thread-safe
	///     manner. The sequence supplied in <paramref name="source"/> is not expected to be thread-safe but it is
	///     required to be thread-agnostic. The iterator returned by <see cref="IEnumerable{T}.GetEnumerator"/> is not
	///     thread-safe, and access should be limited to a single thread/task or controlled via external locks.
	/// </para>
	/// <para>
	///	    By default, <see cref="Memoize"/> will choose the safe option and cache all <see cref="IEnumerable{T}"/>s.
	///     <see cref="ICollection{T}"/> will use an optimized form using <see cref="ICollection{T}.CopyTo(T[], int)"/>,
	///     while other <see cref="IEnumerable{T}"/>s will cache iteratively as each element is requested.
	/// </para>
	/// <para>
	///	    However, if <paramref name="forceCache"/> is set to <see langword="false"/>, then data in an <see
	///     cref="ICollection{T}"/> will be returned directly and not cached. In most cases, this is safe, but if the
	///     collection is modified in between uses, then different data may be returned for each iteration.
	/// </para>
	/// </remarks>
	public static IBuffer<TSource> Memoize<TSource>(this IEnumerable<TSource> source, bool forceCache = true)
	{
		ArgumentNullException.ThrowIfNull(source);

		return (source, forceCache) switch
		{
			(ICollection<TSource> coll, true) => new CollectionMemoizedBuffer<TSource>(coll),
			(ICollection<TSource> coll, false) => new CollectionProxyBuffer<TSource>(coll),
			_ => new EnumerableMemoizedBuffer<TSource>(source),
		};
	}

	private sealed class EnumerableMemoizedBuffer<T> : IBuffer<T>
	{
		private readonly object _lock = new();

		private IEnumerable<T>? _source;

		private IEnumerator<T>? _enumerator;
		private List<T> _buffer = new();
		private bool _initialized;

		private ExceptionDispatchInfo? _exception;
		private int? _exceptionIndex;

		private bool _disposed;

		public EnumerableMemoizedBuffer(IEnumerable<T> source)
		{
			_source = source;
		}

		public int Count => _buffer.Count;

		public void Reset()
		{
			lock (_lock)
			{
				if (_disposed)
					ThrowHelper.ThrowObjectDisposedException<IBuffer<T>>();

				_buffer = new();
				_initialized = false;
				_enumerator?.Dispose();
				_enumerator = null;
				_exceptionIndex = null;
				_exception = null;
			}
		}

		public IEnumerator<T> GetEnumerator()
		{
			InitializeEnumerator();
			return GetEnumeratorImpl();
		}

		private void InitializeEnumerator()
		{
			lock (_lock)
			{
				if (_disposed)
					ThrowHelper.ThrowObjectDisposedException<IBuffer<T>>();

				Assert.NotNull(_source);

				if (_exceptionIndex == -1)
				{
					ArgumentNullException.ThrowIfNull(_exception);
					_exception.Throw();
				}

				if (_initialized)
					return;

#if NET6_0_OR_GREATER
				if (_source.TryGetCollectionCount() is int n)
					_buffer.EnsureCapacity(n);
#endif

				try
				{
					_enumerator = _source.GetEnumerator();
					_initialized = true;
				}
				catch (Exception ex)
				{
					_exception = ExceptionDispatchInfo.Capture(ex);
					_exceptionIndex = -1;
					throw;
				}
			}
		}

		private IEnumerator<T> GetEnumeratorImpl()
		{
			var buffer = _buffer;
			var index = 0;
			while (true)
			{
				T? element;
				lock (_lock)
				{
					if (_disposed)
						ThrowHelper.ThrowObjectDisposedException<IBuffer<T>>();
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
							moved = _enumerator.MoveNext();
						}
						catch (Exception ex)
						{
							_exception = ExceptionDispatchInfo.Capture(ex);
							_exceptionIndex = index;
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

						_buffer.Add(_enumerator.Current);

						Assert.True(index < _buffer.Count);
					}

					element = _buffer[index];
				}

				yield return element;
				index++;
			}
		}

		[ExcludeFromCodeCoverage]
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public void Dispose()
		{
			lock (_lock)
			{
				_disposed = true;
				_buffer.Clear();
				_enumerator?.Dispose();
				_enumerator = null;
				_source = null;
			}
		}
	}

	private sealed class CollectionMemoizedBuffer<T> : IBuffer<T>
	{
		private enum State
		{
			Uninitialized,
			Initializing,
			Initialized,
			Disposed,
			Error,
		}

		private sealed record CmbHelper(State State, T[]? Buffer = null, ExceptionDispatchInfo? Exception = null);

		private ICollection<T>? _source;

		private volatile CmbHelper _state;

		public CollectionMemoizedBuffer(ICollection<T> source)
		{
			_source = source;
			_state = new(State.Uninitialized, null);
		}

		public int Count
		{
			get
			{
				var h = _state;
				if (h.State != State.Initialized)
					return 0;
				return h.Buffer!.Length;
			}
		}

		public void Reset()
		{
			while (true)
			{
				var h = _state;
				switch (h.State)
				{
					case State.Uninitialized:
						return;

					case State.Initializing:
						break;

					case State.Initialized:
					case State.Error:
					{
						var res = Interlocked.CompareExchange(ref _state, new(State.Uninitialized), h);
						if (res != h)
							continue;
						return;
					}

					case State.Disposed:
						ThrowHelper.ThrowObjectDisposedException<IBuffer<T>>();
						break;

					default:
						ThrowHelper.ThrowUnreachableException();
						return;
				}
			}
		}

		public IEnumerator<T> GetEnumerator()
		{
			var buffer = InitializeBuffer();
			return GetEnumeratorImpl(buffer);
		}

		private T[] InitializeBuffer()
		{
			var source = _source;

			while (true)
			{
				var h = _state;
				switch (h.State)
				{
					case State.Uninitialized:
					{
						var tmp = new CmbHelper(State.Initializing);
						// are we first?
						var res = Interlocked.CompareExchange(ref _state, tmp, h);
						if (res != h) continue;
						h = tmp;

						// we should be only ones inside here
						Assert.NotNull(source);

						T[]? array;
						try
						{
							// try to get the array
							array = source.ToArray();
						}
						catch (Exception ex)
						{
							var edi = ExceptionDispatchInfo.Capture(ex);
							// if it doesn't get set, then we're disposed, so don't worry about it
							_ = Interlocked.CompareExchange(ref _state, new(State.Error, Exception: edi), h);
							throw;
						}

						// we've got an array, set it
						res = Interlocked.CompareExchange(ref _state, new(State.Initialized, Buffer: array), tmp);
						// only way this could happen is dispose, so loop again to trigger dispose handling
						if (res != tmp) continue;

						// we've got the array, hand it out.
						return array;
					}

					case State.Initializing:
						continue;

					case State.Initialized:
						Assert.NotNull(h.Buffer);
						return h.Buffer;

					case State.Disposed:
						ThrowHelper.ThrowObjectDisposedException<IBuffer<T>>();
						break;

					case State.Error:
						h.Exception!.Throw();
						break;

					default:
						ThrowHelper.ThrowUnreachableException();
						return default!;
				}
			}
		}

		private IEnumerator<T> GetEnumeratorImpl(T[] buffer)
		{
			foreach (var i in buffer)
			{
				if (_state.State == State.Disposed)
					ThrowHelper.ThrowObjectDisposedException<IBuffer<T>>();

				if (_state.State != State.Initialized
					|| _state.Buffer != buffer)
				{
					ThrowHelper.ThrowInvalidOperationException("Buffer reset during iteration.");
				}

				yield return i;
			}
		}

		[ExcludeFromCodeCoverage]
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public void Dispose()
		{
			// don't care what current state is, force everyone out
			_ = Interlocked.Exchange(ref _state, new(State.Disposed));
			_source = null;
		}
	}

	private sealed class CollectionProxyBuffer<T> : IBuffer<T>
	{
		public CollectionProxyBuffer(ICollection<T> source)
		{
			_source = source;
		}

		private ICollection<T>? _source;
		private ICollection<T> Source
		{
			get
			{
				if (_source == null)
					ThrowHelper.ThrowObjectDisposedException<IBuffer<T>>();
				return _source;
			}
		}

		public void Reset()
		{
			if (_source == null)
				ThrowHelper.ThrowObjectDisposedException<IBuffer<T>>();
		}

		public int Count => Source.Count;

		public IEnumerator<T> GetEnumerator() => Source.GetEnumerator();

		[ExcludeFromCodeCoverage]
		IEnumerator IEnumerable.GetEnumerator() => Source.GetEnumerator();

		public void Dispose() => _source = null;
	}
}
