namespace SuperLinq;

/// <summary>
/// Provides a set of static methods for querying objects that
/// implement <see cref="IEnumerable{T}" />.
/// </summary>
public static partial class SuperEnumerable
{
	internal static int? TryGetCollectionCount<T>(this IEnumerable<T> source) =>
#if NET6_0_OR_GREATER
		source.TryGetNonEnumeratedCount(out var count) ? count : default(int?);
#else
		source switch
		{
			null => ThrowHelper.ThrowArgumentNullException<int?>(nameof(source)),
			ICollection<T> collection => collection.Count,
			System.Collections.ICollection collection => collection.Count,
			_ => null
		};
#endif
}
