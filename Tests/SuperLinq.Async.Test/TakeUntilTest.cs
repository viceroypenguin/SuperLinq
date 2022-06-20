namespace Test.Async;

public class TakeUntilTest
{
	[Fact]
	public Task TakeUntilPredicateNeverFalse()
	{
		var sequence = AsyncEnumerable.Range(0, 5).TakeUntil(x => x != 100);
		return sequence.AssertSequenceEqual(0);
	}

	[Fact]
	public Task TakeUntilPredicateNeverTrue()
	{
		var sequence = AsyncEnumerable.Range(0, 5).TakeUntil(x => x == 100);
		return sequence.AssertSequenceEqual(0, 1, 2, 3, 4);
	}

	[Fact]
	public Task TakeUntilPredicateBecomesTrueHalfWay()
	{
		var sequence = AsyncEnumerable.Range(0, 5).TakeUntil(x => x == 2);
		return sequence.AssertSequenceEqual(0, 1, 2);
	}

	[Fact]
	public void TakeUntilEvaluatesSourceLazily()
	{
		new AsyncBreakingSequence<string>().TakeUntil(x => x.Length == 0);
	}

	[Fact]
	public Task TakeUntilEvaluatesPredicateLazily()
	{
		// Predicate would explode at x == 0, but we never need to evaluate it due to the Take call.
		var sequence = AsyncEnumerable.Range(-2, 5).TakeUntil(x => 1 / x == 1).Take(3);
		return sequence.AssertSequenceEqual(-2, -1, 0);
	}
}
