namespace Test;

public class TakeEveryTest
{
	[Fact]
	public void TakeEveryNegativeSkip()
	{
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			Array.Empty<object>().TakeEvery(-1));
	}

	[Fact]
	public void TakeEveryOutOfRangeZeroStep()
	{
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			Array.Empty<object>().TakeEvery(0));
	}

	[Fact]
	public void TakeEveryEmptySequence()
	{
		Assert.Empty(Array.Empty<object>().TakeEvery(1));
	}

	[Fact]
	public void TakeEveryNonEmptySequence()
	{
		var result = new[] { 1, 2, 3, 4, 5 }.TakeEvery(1);
		result.AssertSequenceEqual(1, 2, 3, 4, 5);
	}

	[Fact]
	public void TakeEveryOtherOnNonEmptySequence()
	{
		var result = new[] { 1, 2, 3, 4, 5 }.TakeEvery(2);
		result.AssertSequenceEqual(1, 3, 5);
	}

	[Fact]
	public void TakeEveryThirdOnNonEmptySequence()
	{
		var result = new[] { 1, 2, 3, 4, 5 }.TakeEvery(3);
		result.AssertSequenceEqual(1, 4);
	}

	[Fact]
	public void TakeEveryIsLazy()
	{
		new BreakingSequence<object>().TakeEvery(1);
	}
}
