namespace Test.Async;

public class AtMostTest
{
	[Fact]
	public async Task AtMostWithNegativeCount()
	{
		await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
			await AsyncSeq(1).AtMost(-1));
	}

	[Fact]
	public async Task AtMostWithEmptySequenceHasAtMostZeroElements()
	{
		Assert.True(await AsyncEnumerable.Empty<int>().AtMost(0));
	}

	[Fact]
	public async Task AtMostWithEmptySequenceHasAtMostOneElement()
	{
		Assert.True(await AsyncEnumerable.Empty<int>().AtMost(1));
	}

	[Fact]
	public async Task AtMostWithSingleElementHasAtMostZeroElements()
	{
		Assert.False(await AsyncSeq(1).AtMost(0));
	}

	[Fact]
	public async Task AtMostWithSingleElementHasAtMostOneElement()
	{
		Assert.True(await AsyncSeq(1).AtMost(1));
	}

	[Fact]
	public async Task AtMostWithSingleElementHasAtMostManyElements()
	{
		Assert.True(await AsyncSeq(1).AtMost(2));
	}

	[Fact]
	public async Task AtMostWithManyElementsHasAtMostOneElements()
	{
		Assert.False(await AsyncSeq(1, 2, 3).AtMost(1));
	}

	[Fact]
	public async Task AtMostDoesNotIterateUnnecessaryElements()
	{
		Assert.False(await AsyncSeqExceptionAt(4).AtMost(2));
	}
}
