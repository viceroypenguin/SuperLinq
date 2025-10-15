namespace SuperLinq.Async.Tests;

public sealed class SkipUntilTest
{
	[Fact]
	public async Task SkipUntilPredicateNeverFalse()
	{
		await using var sequence = AsyncEnumerable.Range(0, 5)
			.AsTestingSequence();
		await sequence
			.SkipUntil(x => x != 100)
			.AssertSequenceEqual(1, 2, 3, 4);
	}

	[Fact]
	public async Task SkipUntilPredicateNeverTrue()
	{
		await using var sequence = AsyncEnumerable.Range(0, 5)
			.AsTestingSequence();
		Assert.Empty(await sequence.SkipUntil(x => x == 100).ToListAsync());
	}

	[Fact]
	public async Task SkipUntilPredicateBecomesTrueHalfWay()
	{
		await using var sequence = AsyncEnumerable.Range(0, 5)
			.AsTestingSequence();
		await sequence
			.SkipUntil(x => x == 2)
			.AssertSequenceEqual(3, 4);
	}

	[Fact]
	public void SkipUntilEvaluatesSourceLazily()
	{
		_ = new AsyncBreakingSequence<string>().SkipUntil(x => x.Length == 0);
	}

	[Fact]
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

	public static IEnumerable<object[]> TestData { get; } =
		[
			[Array.Empty<int>(), 0, Array.Empty<int>()], // empty sequence
			[new[] { 0 }, 0, Array.Empty<int>()], // one-item sequence, predicate succeed
			[new[] { 0 }, 1, Array.Empty<int>()], // one-item sequence, predicate don't succeed
			[new[] { 1, 2, 3 }, 0, new[] { 2, 3 }],         // predicate succeed on first item
			[new[] { 1, 2, 3 }, 1, new[] { 2, 3 }],
			[new[] { 1, 2, 3 }, 2, new[] { 3 }],
			[new[] { 1, 2, 3 }, 3, Array.Empty<int>()], // predicate succeed on last item
			[new[] { 1, 2, 3 }, 4, Array.Empty<int>()], // predicate never succeed
		];

	[Theory, MemberData(nameof(TestData))]
	public async Task TestSkipUntil(int[] source, int min, int[] expected)
	{
		await using var xs = source
			.ToAsyncEnumerable()
			.AsTestingSequence();
		Assert.Equal(expected, await xs
			.SkipUntil(v => v >= min)
			.ToListAsync());
	}
}
