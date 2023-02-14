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

	internal static bool TryGetCollectionCount<T>(this IEnumerable<T> source, out int count)
	{
		Guard.IsNotNull(source);
#if NET6_0_OR_GREATER
		return source.TryGetNonEnumeratedCount(out count);
#else
		switch (source)
		{
			case ICollection<T> collection:
				count = collection.Count;
				return true;
			case System.Collections.ICollection collection:
				count = collection.Count;
				return true;
			default:
				count = default;
				return false;
		}
#endif
	}

	internal static (bool HasValue, T Value) Some<T>(T value) => (true, value);
}
