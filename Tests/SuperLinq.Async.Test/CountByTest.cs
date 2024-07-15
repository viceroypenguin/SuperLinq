namespace Test.Async;

public sealed class CountByTest
{
	[Fact]
	public async Task CountBySimpleTest()
	{
		await using var xs = TestingSequence.Of(1, 2, 3, 4, 5, 6, 1, 2, 3, 1, 1, 2);

		await xs.CountBy(SuperEnumerable.Identity)
			.AssertSequenceEqual(
				CreatePair(1, 4),
				CreatePair(2, 3),
				CreatePair(3, 2),
				CreatePair(4, 1),
				CreatePair(5, 1),
				CreatePair(6, 1));
	}

	[Fact]
	public async Task CountByWithSecondOccurenceImmediatelyAfterFirst()
	{
		await using var xs = "jaffer".AsTestingSequence();

		await xs.CountBy(SuperEnumerable.Identity)
			.AssertSequenceEqual(
				CreatePair('j', 1),
				CreatePair('a', 1),
				CreatePair('f', 2),
				CreatePair('e', 1),
				CreatePair('r', 1));
	}

	[Fact]
	public async Task CountByEvenOddTest()
	{
		await using var xs = AsyncEnumerable.Range(1, 100)
			.AsTestingSequence();

		await xs
			.CountBy(c => c % 2)
			.AssertSequenceEqual(
				CreatePair(1, 50),
				CreatePair(0, 50));
	}

	[Fact]
	public async Task CountByWithEqualityComparer()
	{
		await using var xs = TestingSequence.Of("a", "B", "c", "A", "b", "A");

		await xs
			.CountBy(SuperEnumerable.Identity, StringComparer.OrdinalIgnoreCase)
			.AssertSequenceEqual(
				CreatePair("a", 3),
				CreatePair("B", 2),
				CreatePair("c", 1));
	}

	[Fact]
	public async Task CountByHasKeysOrderedLikeGroupBy()
	{
		var randomSequence = SuperLinq.SuperEnumerable.Random(0, 100).Take(100).ToArray();

		await using var xs = randomSequence.AsTestingSequence();
		var countByKeys = xs.CountBy(SuperEnumerable.Identity).Select(x => x.Key);
		var groupByKeys = randomSequence.GroupBy(SuperEnumerable.Identity).Select(x => x.Key);

		await countByKeys.AssertSequenceEqual(groupByKeys);
	}

	[Fact]
	public void CountByIsLazy()
	{
		_ = new AsyncBreakingSequence<string>()
			.CountBy(AsyncBreakingFunc.Of<string, int>());
	}

	[Fact]
	public async Task CountByWithSomeNullKeys()
	{
		await using var ss = TestingSequence.Of("foo", null, "bar", "baz", null, null, "baz", "bar", null, "foo");

		await ss.CountBy(SuperEnumerable.Identity)
			.AssertSequenceEqual(
				CreatePair((string?)"foo", 2),
				CreatePair((string?)null, 4),
				CreatePair((string?)"bar", 2),
				CreatePair((string?)"baz", 2));
	}

	private static KeyValuePair<TKey, TValue> CreatePair<TKey, TValue>(TKey key, TValue value) => new(key, value);
}
