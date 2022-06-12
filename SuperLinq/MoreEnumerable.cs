namespace SuperLinq;

/// <summary>
/// Provides a set of static methods for querying objects that
/// implement <see cref="IEnumerable{T}" />.
/// </summary>
public static partial class SuperEnumerable
{
	internal static int? TryGetCollectionCount<T>(this IEnumerable<T> source) =>
		source switch
		{
			null => throw new ArgumentNullException(nameof(source)),
			ICollection<T> collection => collection.Count,
			IReadOnlyCollection<T> collection => collection.Count,
			_ => null
		};

	static int CountUpTo<T>(this IEnumerable<T> source, int max)
	{
		if (source == null) throw new ArgumentNullException(nameof(source));
		if (max < 0) throw new ArgumentOutOfRangeException(nameof(max), "The maximum count argument cannot be negative.");

		var count = 0;

		using (var e = source.GetEnumerator())
		{
			while (count < max && e.MoveNext())
			{
				count++;
			}
		}

		return count;
	}

	// See https://github.com/atifaziz/Optuple

	static (bool HasValue, T Value) Some<T>(T value) => (true, value);
}
