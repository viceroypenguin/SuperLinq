namespace Test;

[Obsolete("References `CountBy` which is obsolete in net9+")]
public sealed class CountByTest
{
	[Fact]
	public void CountBySimpleTest()
	{
		using var xs = TestingSequence.Of(1, 2, 3, 4, 5, 6, 1, 2, 3, 1, 1, 2);

		SuperEnumerable
			.CountBy(xs, SuperEnumerable.Identity)
			.AssertSequenceEqual(
				CreatePair(1, 4),
				CreatePair(2, 3),
				CreatePair(3, 2),
				CreatePair(4, 1),
				CreatePair(5, 1),
				CreatePair(6, 1));
	}

	[Fact]
	public void CountByWithSecondOccurenceImmediatelyAfterFirst()
	{
		using var xs = "jaffer".AsTestingSequence();

		SuperEnumerable
			.CountBy(xs, SuperEnumerable.Identity)
			.AssertSequenceEqual(
				CreatePair('j', 1),
				CreatePair('a', 1),
				CreatePair('f', 2),
				CreatePair('e', 1),
				CreatePair('r', 1));
	}

	[Fact]
	public void CountByEvenOddTest()
	{
		using var xs = Enumerable.Range(1, 100)
			.AsTestingSequence();
		SuperEnumerable
			.CountBy(xs, c => c % 2)
			.AssertSequenceEqual(
				CreatePair(1, 50),
				CreatePair(0, 50));
	}

	[Fact]
	public void CountByWithEqualityComparer()
	{
		using var xs = TestingSequence.Of("a", "B", "c", "A", "b", "A");

		SuperEnumerable
			.CountBy(xs, SuperEnumerable.Identity, StringComparer.OrdinalIgnoreCase)
			.AssertSequenceEqual(
				CreatePair("a", 3),
				CreatePair("B", 2),
				CreatePair("c", 1));
	}

	[Fact]
	public void CountByHasKeysOrderedLikeGroupBy()
	{
		var randomSequence = SuperEnumerable.Random(0, 100).Take(100).ToArray();

		using var xs = randomSequence.AsTestingSequence();
		var countByKeys = SuperEnumerable.CountBy(xs, SuperEnumerable.Identity).Select(x => x.Key);
		var groupByKeys = randomSequence.GroupBy(SuperEnumerable.Identity).Select(x => x.Key);

		countByKeys.AssertSequenceEqual(groupByKeys);
	}

	[Fact]
	public void CountByIsLazy()
	{
		_ = SuperEnumerable.CountBy(new BreakingSequence<string>(), BreakingFunc.Of<string, int>());
	}

	[Fact]
	public void CountByWithSomeNullKeys()
	{
		using var xs = TestingSequence.Of("foo", null, "bar", "baz", null, null, "baz", "bar", null, "foo");
		SuperEnumerable
			.CountBy(xs, SuperEnumerable.Identity)
			.AssertSequenceEqual(
				CreatePair((string?)"foo", 2),
				CreatePair((string?)null, 4),
				CreatePair((string?)"bar", 2),
				CreatePair((string?)"baz", 2));
	}

	private static KeyValuePair<TKey, TValue> CreatePair<TKey, TValue>(TKey key, TValue value) => new(key, value);
}
