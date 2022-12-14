using System.Diagnostics.Contracts;

namespace SuperLinq;

/// <summary>
/// Provides a set of static methods for querying objects that
/// implement <see cref="IEnumerable{T}" />.
/// </summary>
public static partial class SuperEnumerable
{
	[Pure]
	internal static int? TryGetCollectionCount<T>(this IEnumerable<T> source) =>
		source switch
		{
			null => throw new ArgumentNullException(nameof(source)),
			ICollection<T> collection => collection.Count,
			IReadOnlyCollection<T> collection => collection.Count,
			_ => null
		};

	[Pure]
	internal static bool TryGetCollectionCount<T>(this IEnumerable<T> source, out int count)
	{
		Guard.IsNotNull(source);
		switch (source)
		{
			case ICollection<T> collection:
				count = collection.Count;
				return true;
			case IReadOnlyCollection<T> collection:
				count = collection.Count;
				return true;
			default:
				count = default;
				return false;
		}
	}

	internal static int CountUpTo<T>(this IEnumerable<T> source, int max)
	{
		Guard.IsNotNull(source);
		Guard.IsGreaterThanOrEqualTo(max, 0);

		var count = 0;

		using var e = source.GetEnumerator();
		while (count < max && e.MoveNext())
			count++;

		return count;
	}

	// See https://github.com/atifaziz/Optuple

	internal static (bool HasValue, T Value) Some<T>(T value) => (true, value);
}
