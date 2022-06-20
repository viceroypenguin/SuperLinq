namespace Test.Async;

public class ExactlyTest
{
	[Fact]
	public async Task ExactlyWithNegativeCount()
	{
		await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
			await AsyncSeq(1).Exactly(-1));
	}

	[Fact]
	public async Task ExactlyWithEmptySequenceHasExactlyZeroElements()
	{
		Assert.True(await AsyncEnumerable.Empty<int>().Exactly(0));
	}

	[Fact]
	public async Task ExactlyWithEmptySequenceHasExactlyOneElement()
	{
		Assert.False(await AsyncEnumerable.Empty<int>().Exactly(1));
	}

	[Fact]
	public async Task ExactlyWithSingleElementHasExactlyOneElements()
	{
		Assert.True(await AsyncSeq(1).Exactly(1));
	}

	[Fact]
	public async Task ExactlyWithManyElementHasExactlyOneElement()
	{
		Assert.False(await AsyncSeq(1, 2, 3).Exactly(1));
	}

	[Fact]
	public async Task ExactlyDoesNotIterateUnnecessaryElements()
	{
		Assert.False(await AsyncSeqExceptionAt(4).Exactly(2));
	}
}
