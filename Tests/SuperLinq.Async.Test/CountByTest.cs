namespace Test.Async;

public class CountByTest
{
	[Fact]
	public ValueTask CountBySimpleTest()
	{
		var result = AsyncSeq(1, 2, 3, 4, 5, 6, 1, 2, 3, 1, 1, 2)
			.CountBy(c => c);

		return result.AssertSequenceEqual(
			(1, 4),
			(2, 3),
			(3, 2),
			(4, 1),
			(5, 1),
			(6, 1));
	}

	[Fact]
	public ValueTask CountByWithSecondOccurenceImmediatelyAfterFirst()
	{
		var result = "jaffer".ToAsyncEnumerable()
			.CountBy(c => c);

		return result.AssertSequenceEqual(
			('j', 1),
			('a', 1),
			('f', 2),
			('e', 1),
			('r', 1));
	}

	[Fact]
	public ValueTask CountByEvenOddTest()
	{
		var result = AsyncEnumerable.Range(1, 100)
			.CountBy(c => c % 2);

		return result.AssertSequenceEqual(
			(1, 50),
			(0, 50));
	}

	[Fact]
	public ValueTask CountByWithEqualityComparer()
	{
		var result = AsyncSeq("a", "B", "c", "A", "b", "A")
			.CountBy(c => c, StringComparer.OrdinalIgnoreCase);

		return result.AssertSequenceEqual(
			("a", 3),
			("B", 2),
			("c", 1));
	}

	[Fact]
	public ValueTask CountByHasKeysOrderedLikeGroupBy()
	{
		var randomSequence = SuperEnumerable.Random(0, 100).Take(100).ToArray();

		var countByKeys = randomSequence.ToAsyncEnumerable().CountBy(x => x).Select(x => x.key);
		var groupByKeys = randomSequence.GroupBy(x => x).Select(x => x.Key);

		return countByKeys.AssertSequenceEqual(groupByKeys);
	}

	[Fact]
	public void CountByIsLazy()
	{
		new AsyncBreakingSequence<string>().CountBy(AsyncBreakingFunc.Of<string, int>());
	}

	[Fact]
	public ValueTask CountByWithSomeNullKeys()
	{
		var ss = AsyncSeq("foo", null, "bar", "baz", null, null, "baz", "bar", null, "foo");
		var result = ss.CountBy(s => s);

		return result.AssertSequenceEqual(
			("foo", 2),
			(default, 4),
			("bar", 2),
			("baz", 2));
	}
}
