namespace Test;

public enum SourceKind
{
	Sequence,
	BreakingList,
	BreakingReadOnlyList,
	BreakingCollection,
	BreakingReadOnlyCollection
}

static partial class TestExtensions
{
	/// <summary>
	/// Just to make our testing easier so we can chain the assertion call.
	/// </summary>

	internal static void AssertSequenceEqual<T>(this IEnumerable<T> actual, IEnumerable<T> expected) =>
		Assert.Equal(expected, actual);

	/// <summary>
	/// Make testing even easier - a params array makes for readable tests :)
	/// The sequence should be evaluated exactly once.
	/// </summary>

	internal static void AssertSequenceEqual<T>(this IEnumerable<T> actual, params T[] expected) =>
		Assert.Equal(expected, actual);

	internal static IEnumerable<string> GenerateSplits(this string str, params char[] separators)
	{
		foreach (var split in str.Split(separators))
			yield return split;
	}

	internal static IEnumerable<IEnumerable<T?>> ArrangeCollectionInlineDatas<T>(this IEnumerable<T> input)
	{
		yield return input.ToSourceKind(SourceKind.Sequence);
		yield return input.ToSourceKind(SourceKind.BreakingReadOnlyCollection);
		yield return input.ToSourceKind(SourceKind.BreakingCollection);
	}

	internal static IEnumerable<T?> ToSourceKind<T>(this IEnumerable<T?> input, SourceKind sourceKind)
	{
		switch (sourceKind)
		{
			case SourceKind.Sequence:
				return input.Select(x => x);
			case SourceKind.BreakingList:
				return new BreakingList<T?>(input.ToList());
			case SourceKind.BreakingReadOnlyList:
				return new BreakingReadOnlyList<T?>(input.ToList());
			case SourceKind.BreakingCollection:
				return new BreakingCollection<T?>(input.ToList());
			case SourceKind.BreakingReadOnlyCollection:
				return new BreakingReadOnlyCollection<T?>(input.ToList());
			default:
				throw new ArgumentException(null, nameof(sourceKind));
		}
	}
}
