namespace Test.Async;

public class AtMostTest
{
	[Fact]
	public async ValueTask AtMostWithNegativeCount()
	{
		await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
			await AsyncSeq(1).AtMost(-1));
	}

	[Fact]
	public async ValueTask AtMostWithEmptySequenceHasAtMostZeroElements()
	{
		Assert.True(await AsyncEnumerable.Empty<int>().AtMost(0));
	}

	[Fact]
	public async ValueTask AtMostWithEmptySequenceHasAtMostOneElement()
	{
		Assert.True(await AsyncEnumerable.Empty<int>().AtMost(1));
	}

	[Fact]
	public async ValueTask AtMostWithSingleElementHasAtMostZeroElements()
	{
		Assert.False(await AsyncSeq(1).AtMost(0));
	}

	[Fact]
	public async ValueTask AtMostWithSingleElementHasAtMostOneElement()
	{
		Assert.True(await AsyncSeq(1).AtMost(1));
	}

	[Fact]
	public async ValueTask AtMostWithSingleElementHasAtMostManyElements()
	{
		Assert.True(await AsyncSeq(1).AtMost(2));
	}

	[Fact]
	public async ValueTask AtMostWithManyElementsHasAtMostOneElements()
	{
		Assert.False(await AsyncSeq(1, 2, 3).AtMost(1));
	}

	[Fact]
	public async ValueTask AtMostDoesNotIterateUnnecessaryElements()
	{
		Assert.False(await AsyncSeqExceptionAt(4).AtMost(2));
	}
}
