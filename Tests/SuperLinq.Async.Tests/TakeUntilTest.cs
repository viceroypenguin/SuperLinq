namespace SuperLinq.Async.Tests;

public sealed class TakeUntilTest
{
	[Fact]
	public void TakeUntilEvaluatesSourceLazily()
	{
		_ = new AsyncBreakingSequence<string>().TakeUntil(x => x.Length == 0);
	}

	[Fact]
	public async Task TakeUntilPredicateNeverFalse()
	{
		await using var sequence = Enumerable.Range(0, 5).AsTestingSequence();
		await sequence
			.TakeUntil(x => x != 100)
			.AssertSequenceEqual(0);
	}

	[Fact]
	public async Task TakeUntilPredicateNeverTrue()
	{
		await using var sequence = Enumerable.Range(0, 5).AsTestingSequence();
		await sequence
			.TakeUntil(x => x == 100)
			.AssertSequenceEqual(Enumerable.Range(0, 5));
	}

	[Fact]
	public async Task TakeUntilPredicateBecomesTrueHalfWay()
	{
		await using var sequence = Enumerable.Range(0, 5).AsTestingSequence();
		await sequence
			.TakeUntil(x => x == 2)
			.AssertSequenceEqual(0, 1, 2);
	}

	[Fact]
	public async Task TakeUntilEvaluatesPredicateLazily()
	{
		// Predicate would explode at x == 0, but we never need to evaluate it due to the Take call.
		await using var sequence = Enumerable.Range(-2, 5).AsTestingSequence();

		await sequence
			.TakeUntil(x => 1 / x == 1)
			.Take(3)
			.AssertSequenceEqual(-2, -1, 0);
	}
}
