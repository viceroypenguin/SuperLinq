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

#if NETCOREAPP
	/// <summary>
	///		Configures how awaits on the tasks returned from an async disposable are performed.
	/// </summary>
	/// <param name="continueOnCapturedContext">
	///		<see langword="true" /> to capture and marshal back to the current context; otherwise, <see langword="false" />.
	/// </param>
	/// <returns>
	///		The configured async disposable.
	/// </returns>
	public ConfiguredAsyncDisposable ConfigureAwait(bool continueOnCapturedContext) =>
		((IAsyncDisposable)this).ConfigureAwait(continueOnCapturedContext);
#endif
}
