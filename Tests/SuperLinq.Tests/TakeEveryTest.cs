namespace SuperLinq.Tests;

public sealed class TakeEveryTest
{
	[Test]
	public void TakeEveryIsLazy()
	{
		_ = new BreakingSequence<object>().TakeEvery(1);
	}

	[Test]
	public void TakeEveryNegativeSkip()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new BreakingSequence<int>().TakeEvery(-1));
	}

	[Test]
	public void TakeEveryOutOfRangeZeroStep()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new BreakingSequence<int>().TakeEvery(0));
	}

	[Test]
	public void TakeEveryEmptySequence()
	{
		using var sequence = TestingSequence.Of<object>();

		var result = sequence.TakeEvery(1);
		result.AssertSequenceEqual();
	}

	[Test]
	public void TakeEveryNonEmptySequence()
	{
		using var sequence = Enumerable.Range(1, 5).AsTestingSequence();

		var result = sequence.TakeEvery(1);
		result.AssertSequenceEqual(1, 2, 3, 4, 5);
	}

	[Test]
	public void TakeEveryOtherOnNonEmptySequence()
	{
		using var sequence = Enumerable.Range(1, 5).AsTestingSequence();

		var result = sequence.TakeEvery(2);
		result.AssertSequenceEqual(1, 3, 5);
	}

	[Test]
	public void TakeEveryThirdOnNonEmptySequence()
	{
		using var sequence = Enumerable.Range(1, 5).AsTestingSequence();

		var result = sequence.TakeEvery(3);
		result.AssertSequenceEqual(1, 4);
	}
}
