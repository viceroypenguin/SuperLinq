namespace Test;

public class CountByTest
{
	[Fact]
	public void CountBySimpleTest()
	{
		using var xs = TestingSequence.Of(1, 2, 3, 4, 5, 6, 1, 2, 3, 1, 1, 2);

		xs.CountBy(SuperEnumerable.Identity)
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

		xs.CountBy(SuperEnumerable.Identity)
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
		xs.CountBy(c => c % 2)
			.AssertSequenceEqual(
				KeyValuePair.Create(1, 50),
				KeyValuePair.Create(0, 50));
	}

	[Fact]
	public void CountByWithEqualityComparer()
	{
		using var xs = TestingSequence.Of("a", "B", "c", "A", "b", "A");

		xs.CountBy(SuperEnumerable.Identity, StringComparer.OrdinalIgnoreCase)
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
		var countByKeys = xs.CountBy(SuperEnumerable.Identity).Select(x => x.Key);
		var groupByKeys = randomSequence.GroupBy(SuperEnumerable.Identity).Select(x => x.Key);

		countByKeys.AssertSequenceEqual(groupByKeys);
	}

	[Fact]
	public void CountByIsLazy()
	{
		_ = new BreakingSequence<string>().CountBy(BreakingFunc.Of<string, int>());
	}

	[Fact]
	public void CountByWithSomeNullKeys()
	{
		using var ss = TestingSequence.Of("foo", null, "bar", "baz", null, null, "baz", "bar", null, "foo");
		ss.CountBy(SuperEnumerable.Identity)
			.AssertSequenceEqual(
				KeyValuePair.Create((string?)"foo", 2),
				KeyValuePair.Create((string?)null, 4),
				KeyValuePair.Create((string?)"bar", 2),
				KeyValuePair.Create((string?)"baz", 2));
	}
}
