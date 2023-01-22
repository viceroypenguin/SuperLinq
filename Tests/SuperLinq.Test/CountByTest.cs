namespace Test;

public class CountByTest
{
	[Fact]
	public void CountBySimpleTest()
	{
		using var xs = TestingSequence.Of(1, 2, 3, 4, 5, 6, 1, 2, 3, 1, 1, 2);

		xs.CountBy(c => c)
			.AssertSequenceEqual(
				(1, 4),
				(2, 3),
				(3, 2),
				(4, 1),
				(5, 1),
				(6, 1));
	}

	[Fact]
	public void CountByWithSecondOccurenceImmediatelyAfterFirst()
	{
		using var xs = "jaffer".AsTestingSequence();

		xs.CountBy(c => c)
			.AssertSequenceEqual(
				('j', 1),
				('a', 1),
				('f', 2),
				('e', 1),
				('r', 1));
	}

	[Fact]
	public void CountByEvenOddTest()
	{
		using var xs = Enumerable.Range(1, 100)
			.AsTestingSequence();
		xs.CountBy(c => c % 2)
			.AssertSequenceEqual(
				(1, 50),
				(0, 50));
	}

	[Fact]
	public void CountByWithEqualityComparer()
	{
		using var xs = TestingSequence.Of("a", "B", "c", "A", "b", "A");

		xs.CountBy(c => c, StringComparer.OrdinalIgnoreCase)
			.AssertSequenceEqual(
				("a", 3),
				("B", 2),
				("c", 1));
	}

	[Fact]
	public void CountByHasKeysOrderedLikeGroupBy()
	{
		var randomSequence = SuperEnumerable.Random(0, 100).Take(100).ToArray();

		using var xs = randomSequence.AsTestingSequence();
		var countByKeys = xs.CountBy(x => x).Select(x => x.key);
		var groupByKeys = randomSequence.GroupBy(x => x).Select(x => x.Key);

		countByKeys.AssertSequenceEqual(groupByKeys);
	}

	[Fact]
	public void CountByIsLazy()
	{
		new BreakingSequence<string>().CountBy(BreakingFunc.Of<string, int>());
	}

	[Fact]
	public void CountByWithSomeNullKeys()
	{
		using var ss = TestingSequence.Of("foo", null, "bar", "baz", null, null, "baz", "bar", null, "foo");
		ss.CountBy(s => s)
			.AssertSequenceEqual(
				("foo", 2),
				(null, 4),
				("bar", 2),
				("baz", 2));
	}
}
