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
				KeyValuePair.Create(1, 4),
				KeyValuePair.Create(2, 3),
				KeyValuePair.Create(3, 2),
				KeyValuePair.Create(4, 1),
				KeyValuePair.Create(5, 1),
				KeyValuePair.Create(6, 1));
	}

	[Fact]
	public void CountByWithSecondOccurenceImmediatelyAfterFirst()
	{
		using var xs = "jaffer".AsTestingSequence();

		SuperEnumerable
			.CountBy(xs, SuperEnumerable.Identity)
			.AssertSequenceEqual(
				KeyValuePair.Create('j', 1),
				KeyValuePair.Create('a', 1),
				KeyValuePair.Create('f', 2),
				KeyValuePair.Create('e', 1),
				KeyValuePair.Create('r', 1));
	}

	[Fact]
	public void CountByEvenOddTest()
	{
		using var xs = Enumerable.Range(1, 100)
			.AsTestingSequence();
		SuperEnumerable
			.CountBy(xs, c => c % 2)
			.AssertSequenceEqual(
				KeyValuePair.Create(1, 50),
				KeyValuePair.Create(0, 50));
	}

	[Fact]
	public void CountByWithEqualityComparer()
	{
		using var xs = TestingSequence.Of("a", "B", "c", "A", "b", "A");

		SuperEnumerable
			.CountBy(xs, SuperEnumerable.Identity, StringComparer.OrdinalIgnoreCase)
			.AssertSequenceEqual(
				KeyValuePair.Create("a", 3),
				KeyValuePair.Create("B", 2),
				KeyValuePair.Create("c", 1));
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
				KeyValuePair.Create((string?)"foo", 2),
				KeyValuePair.Create((string?)null, 4),
				KeyValuePair.Create((string?)"bar", 2),
				KeyValuePair.Create((string?)"baz", 2));
	}
}
