using CommunityToolkit.Diagnostics;

namespace Test;

public enum SourceKind
{
	Sequence,
	BreakingList,
	BreakingCollection,
}

public interface IDisposableEnumerable<T> : IEnumerable<T>, IDisposable { }

internal static partial class TestExtensions
{
	internal static IEnumerable<T> Seq<T>(params T[] values) => values;

	public static IEnumerable<int> SeqExceptionAt(int index) =>
		SuperEnumerable.From(
			Enumerable.Range(1, index)
				.Select(i => Func(() => i))
				.Append(BreakingFunc.Of<int>())
				.ToArray());

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

	internal static void AssertSequenceEqual<T>(this IEnumerable<T> actual, Func<T, T, bool> comparer, params T[] expected) =>
		Assert.Equal(expected, actual, EqualityComparer.Create(comparer));

	internal static IEnumerable<IDisposableEnumerable<T>> ArrangeCollectionInlineDatas<T>(this IEnumerable<T> input)
	{
		yield return input.ToSourceKind(SourceKind.Sequence);
		yield return input.ToSourceKind(SourceKind.BreakingCollection);
	}

	internal static IDisposableEnumerable<T> ToSourceKind<T>(this IEnumerable<T> input, SourceKind sourceKind) =>
		sourceKind switch
		{
			SourceKind.Sequence => input.AsTestingSequence(),
			SourceKind.BreakingList => new BreakingList<T>(input),
			SourceKind.BreakingCollection => new BreakingCollection<T>(input),
			_ => ThrowHelper.ThrowArgumentException<IDisposableEnumerable<T>>(nameof(sourceKind)),
		};

	internal static void AssertCollectionEqual<T>(this IEnumerable<T> actual, params T[] expected) =>
		Assert.True(actual.CollectionEqual(expected, comparer: default));

	internal static void AssertCollectionEqual<T>(this IEnumerable<T> actual, IEnumerable<T> expected) =>
		Assert.True(actual.CollectionEqual(expected, comparer: default));

	internal static void AssertCollectionEqual<T>(this IEnumerable<T> actual, IEnumerable<T> expected, IEqualityComparer<T>? comparer) =>
		Assert.True(actual.CollectionEqual(expected, comparer));
}
