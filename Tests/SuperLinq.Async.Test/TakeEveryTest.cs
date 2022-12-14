namespace Test.Async;

public class TakeEveryTest
{
	[Fact]
	public void TakeEveryNegativeSkip()
	{
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			AsyncEnumerable.Empty<object>().TakeEvery(-1));
	}

	[Fact]
	public void TakeEveryOutOfRangeZeroStep()
	{
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			AsyncEnumerable.Empty<object>().TakeEvery(0));
	}

	[Fact]
	public Task TakeEveryEmptySequence()
	{
		return AsyncEnumerable.Empty<object>().TakeEvery(1).AssertEmpty();
	}

	[Fact]
	public Task TakeEveryNonEmptySequence()
	{
		var result = AsyncSeq(1, 2, 3, 4, 5).TakeEvery(1);
		return result.AssertSequenceEqual(1, 2, 3, 4, 5);
	}

	[Fact]
	public Task TakeEveryOtherOnNonEmptySequence()
	{
		var result = AsyncSeq(1, 2, 3, 4, 5).TakeEvery(2);
		return result.AssertSequenceEqual(1, 3, 5);
	}

	[Fact]
	public Task TakeEveryThirdOnNonEmptySequence()
	{
		var result = AsyncSeq(1, 2, 3, 4, 5).TakeEvery(3);
		return result.AssertSequenceEqual(1, 4);
	}

	[Fact]
	public void TakeEveryIsLazy()
	{
		new AsyncBreakingSequence<object>().TakeEvery(1);
	}
}
