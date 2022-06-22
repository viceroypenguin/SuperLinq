namespace SuperLinq.Async;

/// <summary>
/// Provides a set of static methods for querying objects that
/// implement <see cref="IAsyncEnumerable{T}" />.
/// </summary>
public static partial class AsyncSuperEnumerable
{
	internal static ConfiguredCancelableAsyncEnumerable<T>.Enumerator GetConfiguredAsyncEnumerator<T>(this IAsyncEnumerable<T> enumerable, CancellationToken cancellationToken) =>
		enumerable.ConfigureAwait(false).WithCancellation(cancellationToken).GetAsyncEnumerator();

	private static (bool HasValue, T Value) Some<T>(T value) => (true, value);
}
