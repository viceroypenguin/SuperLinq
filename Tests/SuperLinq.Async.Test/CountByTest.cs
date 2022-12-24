namespace Test.Async;

public class CountByTest
{
	[Fact]
	public async Task CountBySimpleTest()
	{
		await using var xs = AsyncSeq(1, 2, 3, 4, 5, 6, 1, 2, 3, 1, 1, 2)
			.AsTestingSequence();

		await xs.CountBy(c => c)
			.AssertSequenceEqual(
				(1, 4),
				(2, 3),
				(3, 2),
				(4, 1),
				(5, 1),
				(6, 1));
	}

	[Fact]
	public async Task CountByWithSecondOccurenceImmediatelyAfterFirst()
	{
		await using var xs = "jaffer".ToAsyncEnumerable()
			.AsTestingSequence();

		await xs.CountBy(c => c)
			.AssertSequenceEqual(
				('j', 1),
				('a', 1),
				('f', 2),
				('e', 1),
				('r', 1));
	}

	[Fact]
	public async Task CountByEvenOddTest()
	{
		await using var xs = AsyncEnumerable.Range(1, 100)
			.AsTestingSequence();

		await xs
			.CountBy(c => c % 2)
			.AssertSequenceEqual(
				(1, 50),
				(0, 50));
	}

	[Fact]
	public async Task CountByWithEqualityComparer()
	{
		await using var xs = AsyncSeq("a", "B", "c", "A", "b", "A")
			.AsTestingSequence();

		await xs
			.CountBy(c => c, StringComparer.OrdinalIgnoreCase)
			.AssertSequenceEqual(
				("a", 3),
				("B", 2),
				("c", 1));
	}

	[Fact]
	public async Task CountByHasKeysOrderedLikeGroupBy()
	{
		var randomSequence = SuperLinq.SuperEnumerable.Random(0, 100).Take(100).ToArray();

		await using var xs = randomSequence.ToAsyncEnumerable()
			.AsTestingSequence();
		var countByKeys = xs.CountBy(x => x).Select(x => x.key);
		var groupByKeys = randomSequence.GroupBy(x => x).Select(x => x.Key);

		await countByKeys.AssertSequenceEqual(groupByKeys);
	}

	[Fact]
	public void CountByIsLazy()
	{
		new AsyncBreakingSequence<string>()
			.CountBy(AsyncBreakingFunc.Of<string, int>());
	}

	[Fact]
	public async Task CountByWithSomeNullKeys()
	{
		await using var ss = AsyncSeq("foo", null, "bar", "baz", null, null, "baz", "bar", null, "foo")
			.AsTestingSequence();

		await ss.CountBy(s => s)
			.AssertSequenceEqual(
				("foo", 2),
				(default, 4),
				("bar", 2),
				("baz", 2));
	}
}
