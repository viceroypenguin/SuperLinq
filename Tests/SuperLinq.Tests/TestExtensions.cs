namespace SuperLinq.Tests;

public enum SourceKind
{
	Sequence,
	BreakingCollection,
}

public static partial class TestExtensions
{
	#region Sequences
	internal static IEnumerable<T> Seq<T>(params T[] values) => values;

	public static IEnumerable<int> SeqExceptionAt(int index) =>
		SuperEnumerable.From(
			Enumerable.Range(1, index - 1)
				.Select(i => Func(() => i))
				.Concat([BreakingFunc.Of<int>()])
				.ToArray());

	public static IEnumerable<IDisposableEnumerable<int>> GetEmptySequences() =>
		Enumerable.Empty<int>()
			.GetBreakingCollectionSequences();

	public static IEnumerable<IDisposableEnumerable<int>> GetSingleElementSequences() =>
		Seq(1)
			.GetBreakingCollectionSequences();

	public static IEnumerable<IDisposableEnumerable<int>> GetThreeElementSequences() =>
		Seq(1, 2, 3)
			.GetBreakingCollectionSequences();
	#endregion

	#region Sequence Content Validation
	internal static void AssertSequenceEqual<T>(this IEnumerable<T> actual, IEnumerable<T> expected, bool testCollectionEnumerable = false) =>
		actual.AssertSequenceEqual(expected as IList<T> ?? [.. expected], testCollectionEnumerable);

	internal static void AssertSequenceEqual<T>(this IEnumerable<T> actual, params T[] expected) =>
		actual.AssertSequenceEqual((IList<T>)expected);

	internal static void AssertSequenceEqual<T>(this IEnumerable<T> actual, IList<T> expected, bool testCollectionEnumerable = false)
	{
		if (actual is ICollection<T>)
		{
			var arr = new T[expected.Count];
			var cnt = SuperEnumerable.CopyTo(actual, arr);
			Assert.Equal(expected.Count, cnt);
			Assert.Equal(expected, arr);

			if (testCollectionEnumerable)
				Assert.Equal(expected, actual);
		}
		else
		{
			Assert.Equal(expected, actual);
		}
	}

	internal static void AssertSequenceEqual<T>(this IEnumerable<T> actual, Func<T, T, bool> comparer, params T[] expected) =>
		Assert.Equal(expected, actual, EqualityComparer.Create(comparer));

	internal static void AssertCollectionEqual<T>(this IEnumerable<T> actual, params T[] expected) =>
		Assert.True(actual.CollectionEqual(expected, comparer: default));

	internal static void AssertCollectionEqual<T>(this IEnumerable<T> actual, IEnumerable<T> expected) =>
		Assert.True(actual.CollectionEqual(expected, comparer: default));

	internal static void AssertCollectionEqual<T>(this IEnumerable<T> actual, IEnumerable<T> expected, IEqualityComparer<T>? comparer) =>
		Assert.True(actual.CollectionEqual(expected, comparer));
	#endregion

	#region Collection Error Checking
	internal static void AssertCollectionErrorChecking<T>(this IEnumerable<T> result, int length)
	{
		var coll = result as ICollection<T>;
		Assert.NotNull(coll);

		Assert.Equal(length, result.Count());
		Assert.Equal(length, coll.Count);

		_ = Assert.Throws<ArgumentNullException>(
			"array",
			() => coll.CopyTo(null!, 0));

		var array = new T[length * 2];
		_ = Assert.Throws<ArgumentOutOfRangeException>(
			"arrayIndex",
			() => coll.CopyTo(array, -1));
		_ = Assert.Throws<ArgumentOutOfRangeException>(
			"arrayIndex",
			() => coll.CopyTo(array, length + 1));

		Assert.True(array.All(x => EqualityComparer<T>.Default.Equals(x, default!)));
	}

	internal static void AssertListElementChecking<T>(this IEnumerable<T> result, int length)
	{
		var list = result as IList<T>;
		Assert.NotNull(list);

		if (length > 0)
		{
			_ = result.ElementAt(0);
			_ = result.ElementAt(length - 1);

			_ = Assert.Throws<ArgumentOutOfRangeException>(
				"index",
				() => result.ElementAt(-1));

			_ = Assert.Throws<ArgumentOutOfRangeException>(
				"index",
				() => result.ElementAt(length));
		}
		else
		{
			_ = Assert.Throws<ArgumentOutOfRangeException>(
				"index",
				() => result.ElementAt(0));
		}
	}
	#endregion

	#region Testing Sequence Generation
	public static IEnumerable<IDisposableEnumerable<T>> GetTestingSequence<T>(
		this IEnumerable<T> input,
		int maxEnumerations = 1
	)
	{
		// UI will consume one enumeration
		yield return input.AsTestingSequence(maxEnumerations: maxEnumerations);
	}

	public static IEnumerable<IDisposableEnumerable<T>> GetCollectionSequences<T>(this IEnumerable<T> input)
	{
		// UI will consume one enumeration
		yield return input.AsTestingSequence();
		yield return input.AsTestingCollection();
	}

	public static IEnumerable<IDisposableEnumerable<T>> GetBreakingCollectionSequences<T>(this IEnumerable<T> input)
	{
		// UI will consume one enumeration
		yield return input.AsTestingSequence();
		yield return input.AsBreakingCollection();
	}

	public static IEnumerable<IDisposableEnumerable<T>> GetListSequences<T>(this IEnumerable<T> input)
	{
		// UI will consume one enumeration
		yield return input.AsTestingSequence();
		yield return new BreakingList<T>(input);
	}

	public static IEnumerable<IDisposableEnumerable<T>> GetAllSequences<T>(this IEnumerable<T> input)
	{
		// UI will consume one enumeration
		yield return input.AsTestingSequence();
		yield return input.AsTestingCollection();
		yield return input.AsBreakingList();
	}
	#endregion
}
