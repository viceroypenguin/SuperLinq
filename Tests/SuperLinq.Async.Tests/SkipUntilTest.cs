namespace SuperLinq.Async.Tests;

public sealed class SkipUntilTest
{
	[Test]
	public async Task SkipUntilPredicateNeverFalse()
	{
		await using var sequence = AsyncEnumerable.Range(0, 5)
			.AsTestingSequence();
		await sequence
			.SkipUntil(x => x != 100)
			.AssertSequenceEqual(1, 2, 3, 4);
	}

	[Test]
	public async Task SkipUntilPredicateNeverTrue()
	{
		await using var sequence = AsyncEnumerable.Range(0, 5)
			.AsTestingSequence();
		Assert.Empty(await sequence.SkipUntil(x => x == 100).ToListAsync());
	}

	[Test]
	public async Task SkipUntilPredicateBecomesTrueHalfWay()
	{
		await using var sequence = AsyncEnumerable.Range(0, 5)
			.AsTestingSequence();
		await sequence
			.SkipUntil(x => x == 2)
			.AssertSequenceEqual(3, 4);
	}

	[Test]
	public void SkipUntilEvaluatesSourceLazily()
	{
		_ = new AsyncBreakingSequence<string>().SkipUntil(x => x.Length == 0);
	}

	[Test]
	public async Task SkipUntilEvaluatesPredicateLazily()
	{
		await using var sequence = AsyncEnumerable.Range(-2, 5)
			.AsTestingSequence();

		// Predicate would explode at x == 0, but we never need to evaluate it as we've
		// started returning items after -1.
		await sequence
			.SkipUntil(x => 1 / x == -1)
			.AssertSequenceEqual(0, 1, 2);
	}

	public static IEnumerable<(int[] source, int min, int[] expected)> TestData() =>
		[
			([], 0, []),
			([0], 0, []),
			([0], 1, []),
			([1, 2, 3], 0, [2, 3]),
			([1, 2, 3], 1, [2, 3]),
			([1, 2, 3], 2, [3]),
			([1, 2, 3], 3, []),
			([1, 2, 3], 4, []),
		];

	[Test]
	[MethodDataSource(nameof(TestData))]
	public async Task TestSkipUntil(int[] source, int min, int[] expected)
	{
		await using var xs = source
			.ToAsyncEnumerable()
			.AsTestingSequence();

		Assert.Equal(
			expected,
			await xs
				.SkipUntil(v => v >= min)
				.ToListAsync()
		);
	}
}
