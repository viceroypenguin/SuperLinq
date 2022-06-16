using System.Runtime.CompilerServices;

namespace SuperLinq.Async;

/// <summary>
/// Provides a set of static methods for querying objects that
/// implement <see cref="IAsyncEnumerable{T}" />.
/// </summary>
public static partial class AsyncSuperEnumerable
{
	internal static ConfiguredCancelableAsyncEnumerable<T>.Enumerator GetConfiguredAsyncEnumerator<T>(this IAsyncEnumerable<T> enumerable, bool continueOnCapturedContext, CancellationToken cancellationToken) =>
		enumerable.ConfigureAwait(continueOnCapturedContext).WithCancellation(cancellationToken).GetAsyncEnumerator();
}
