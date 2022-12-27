using System.Collections;

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
			ICollection collection => collection.Count,
			_ => null
		};

	internal static bool TryGetCollectionCount<T>(this IEnumerable<T> source, out int count)
	{
		Guard.IsNotNull(source);
		switch (source)
		{
			case ICollection<T> collection:
				count = collection.Count;
				return true;
			case ICollection collection:
				count = collection.Count;
				return true;
			default:
				count = default;
				return false;
		}
	}

	internal static (bool HasValue, T Value) Some<T>(T value) => (true, value);
}
