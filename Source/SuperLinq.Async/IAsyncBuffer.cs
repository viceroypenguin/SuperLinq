namespace SuperLinq.Async;

/// <summary>
/// Represents a cached sequence that can be re-enumerated multiple times.
/// </summary>
/// <typeparam name="T">The type of the items in the cached sequence.</typeparam>
public interface IAsyncBuffer<out T> : IAsyncEnumerable<T>, IAsyncDisposable
{
	/// <summary>
	/// Clears the current buffer and restarts the enumeration from the beginning.
	/// </summary>
	/// <remarks>
	/// Any active iterators of this buffer may receive an <see cref="InvalidOperationException"/> when they next <see
	/// cref="IAsyncEnumerator{T}.MoveNextAsync"/> due to the invalid state of iteration.
	/// </remarks>
	ValueTask Reset(CancellationToken cancellationToken = default);

	/// <summary>
	/// The number of elements currently cached.
	/// </summary>
	int Count { get; }
}
