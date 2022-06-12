using System.Collections;
using System.Runtime.ExceptionServices;

namespace SuperLinq.Experimental;

static partial class ExperimentalEnumerable
{
	/// <summary>
	/// Creates a sequence that lazily caches the source as it is iterated
	/// for the first time, reusing the cache thereafter for future
	/// re-iterations. If the source is already cached or buffered then it
	/// is returned verbatim.
	/// </summary>
	/// <typeparam name="T">
	/// Type of elements in <paramref name="source"/>.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <returns>
	/// Returns a sequence that corresponds to a cached version of the
	/// input sequence.</returns>
	/// <remarks>
	/// The returned <see cref="IEnumerable{T}"/> will cache items from
	/// <paramref name="source"/> in a thread-safe manner. Each thread can
	/// call its <see cref="IEnumerable{T}.GetEnumerator"/> to acquire an
	/// iterator  but the same iterator should not be used simultanesouly
	/// from multiple threads. The sequence supplied in
	/// <paramref name="source"/> is not expected to be thread-safe but it
	/// is required to be thread-agnostic because different threads
	/// (though never simultaneously) may iterate over the sequence.
	/// </remarks>

	public static IEnumerable<T> Memoize<T>(this IEnumerable<T> source)
	{
		switch (source)
		{
			case null: throw new ArgumentNullException(nameof(source));
			case ICollection<T>: // ...
			case IReadOnlyCollection<T>: // ...
			case MemoizedEnumerable<T>: return source;
			default: return new MemoizedEnumerable<T>(source);
		}
	}
}

sealed class MemoizedEnumerable<T> : IEnumerable<T>, IDisposable
{
	List<T>? _cache;
	readonly object _locker;
	readonly IEnumerable<T> _source;
	IEnumerator<T>? _sourceEnumerator;
	int? _errorIndex;
	ExceptionDispatchInfo? _error;

	public MemoizedEnumerable(IEnumerable<T> sequence)
	{
		_source = sequence ?? throw new ArgumentNullException(nameof(sequence));
		_locker = new object();
	}

	public IEnumerator<T> GetEnumerator()
	{
		if (_cache == null)
		{
			lock (_locker)
			{
				if (_cache == null)
				{
					_error?.Throw();

					try
					{
						var cache = new List<T>(); // for exception safety, allocate then...
						_sourceEnumerator = _source.GetEnumerator(); // (because this can fail)
						_cache = cache; // ...commit to state
					}
					catch (Exception ex)
					{
						_error = ExceptionDispatchInfo.Capture(ex);
						throw;
					}
				}
			}
		}

		return _(); IEnumerator<T> _()
		{
			var index = 0;

			while (true)
			{
				T current;
				lock (_locker)
				{
					if (_cache == null) // Cache disposed during iteration?
						throw new ObjectDisposedException(nameof(MemoizedEnumerable<T>));

					if (index >= _cache.Count)
					{
						if (index == _errorIndex)
						{
							_error!.Throw();
						}

						if (_sourceEnumerator == null)
							break;

						bool moved;
						try
						{
							moved = _sourceEnumerator.MoveNext();
						}
						catch (Exception ex)
						{
							_error = ExceptionDispatchInfo.Capture(ex);
							_errorIndex = index;
							_sourceEnumerator.Dispose();
							_sourceEnumerator = null;
							throw;
						}

						if (moved)
						{
							_cache.Add(_sourceEnumerator.Current);
						}
						else
						{
							_sourceEnumerator.Dispose();
							_sourceEnumerator = null;
							break;
						}
					}

					current = _cache[index];
				}

				yield return current;
				index++;
			}
		}
	}

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	public void Dispose()
	{
		lock (_locker)
		{
			_error = null;
			_cache = null;
			_errorIndex = null;
			_sourceEnumerator?.Dispose();
			_sourceEnumerator = null;
		}
	}
}
