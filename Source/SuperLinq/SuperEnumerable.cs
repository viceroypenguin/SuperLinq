using System.Diagnostics.CodeAnalysis;

namespace SuperLinq;

/// <summary>
/// Provides a set of static methods for querying objects that
/// implement <see cref="IEnumerable{T}" />.
/// </summary>
public static partial class SuperEnumerable
{
	[ExcludeFromCodeCoverage]
	internal static int? TryGetCollectionCount<T>(this IEnumerable<T> source) =>
#if NET6_0_OR_GREATER
		source.TryGetNonEnumeratedCount(out var count) ? count : default(int?);
#else
		source switch
		{
			null => ThrowHelper.ThrowArgumentNullException<int?>(nameof(source)),
			ICollection<T> collection => collection.Count,
			System.Collections.ICollection collection => collection.Count,
			_ => null,
		};
#endif

	[ExcludeFromCodeCoverage]
	internal static int GetCollectionCount<T>(this IEnumerable<T> source) =>
#if NET6_0_OR_GREATER
		source.TryGetNonEnumeratedCount(out var count)
			? count
			: ThrowHelper.ThrowInvalidOperationException<int>("Expected valid non-enumerated count.");
#else
		source switch
		{
			null => ThrowHelper.ThrowArgumentNullException<int>(nameof(source)),
			ICollection<T> collection => collection.Count,
			System.Collections.ICollection collection => collection.Count,
			_ => ThrowHelper.ThrowInvalidOperationException<int>("Expected valid non-enumerated count."),
		};
#endif

	private static int Max(int val1, int val2) => Math.Max(val1, val2);
	private static int Max(int val1, int val2, int val3) => Math.Max(val1, Math.Max(val2, val3));
	private static int Max(int val1, int val2, int val3, int val4) => Math.Max(Math.Max(val1, val2), Math.Max(val3, val4));

	private static int Min(int val1, int val2) => Math.Min(val1, val2);
	private static int Min(int val1, int val2, int val3) => Math.Min(val1, Math.Min(val2, val3));
	private static int Min(int val1, int val2, int val3, int val4) => Math.Min(Math.Min(val1, val2), Math.Min(val3, val4));
}
