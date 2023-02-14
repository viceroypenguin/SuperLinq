namespace Test.Async;

public class TakeEveryTest
{
	[Fact]
	public void TakeEveryIsLazy()
	{
		_ = new AsyncBreakingSequence<object>().TakeEvery(1);
	}

	[Fact]
	public void TakeEveryNegativeSkip()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new AsyncBreakingSequence<int>().TakeEvery(-1));
	}

	[Fact]
	public void TakeEveryOutOfRangeZeroStep()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new AsyncBreakingSequence<int>().TakeEvery(0));
	}

	[Fact]
	public async Task TakeEveryEmptySequence()
	{
		await using var sequence = TestingSequence.Of<object>();

		var result = sequence.TakeEvery(1);
		await result.AssertSequenceEqual();
	}

	[Fact]
	public async Task TakeEveryNonEmptySequence()
	{
		await using var sequence = Enumerable.Range(1, 5).AsTestingSequence();

		var result = sequence.TakeEvery(1);
		await result.AssertSequenceEqual(1, 2, 3, 4, 5);
	}

	[Fact]
	public async Task TakeEveryOtherOnNonEmptySequence()
	{
		await using var sequence = Enumerable.Range(1, 5).AsTestingSequence();

		var result = sequence.TakeEvery(2);
		await result.AssertSequenceEqual(1, 3, 5);
	}

	[Fact]
	public async Task TakeEveryThirdOnNonEmptySequence()
	{
		await using var sequence = Enumerable.Range(1, 5).AsTestingSequence();

		var result = sequence.TakeEvery(3);
		await result.AssertSequenceEqual(1, 4);
	}
}
