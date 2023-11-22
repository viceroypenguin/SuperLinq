using System.Collections;

namespace SuperLinq;

/// <summary>
///	    Represents a cached sequence that can be re-enumerated multiple times.
/// </summary>
/// <typeparam name="T">
///	    The type of the items in the cached sequence.
/// </typeparam>
public interface IBuffer<out T> : IEnumerable<T>, IDisposable
{
	/// <summary>
	///	    Clears the current buffer and restarts the enumeration from the beginning.
	/// </summary>
	/// <remarks>
	///	    Any active iterators of this buffer may receive an <see cref="InvalidOperationException"/> when they next
	///     <see cref="IEnumerator.MoveNext"/> due to the invalid state of iteration.
	/// </remarks>
	void Reset();

	/// <summary>
	///		The number of elements currently cached.
	/// </summary>
	int Count { get; }
}
