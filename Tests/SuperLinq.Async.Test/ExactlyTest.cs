namespace Test.Async;

public class ExactlyTest
{
	[Fact]
	public async ValueTask ExactlyWithNegativeCount()
	{
		await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
			await AsyncSeq(1).Exactly(-1));
	}

	[Fact]
	public async ValueTask ExactlyWithEmptySequenceHasExactlyZeroElements()
	{
		Assert.True(await AsyncEnumerable.Empty<int>().Exactly(0));
	}

	[Fact]
	public async ValueTask ExactlyWithEmptySequenceHasExactlyOneElement()
	{
		Assert.False(await AsyncEnumerable.Empty<int>().Exactly(1));
	}

	[Fact]
	public async ValueTask ExactlyWithSingleElementHasExactlyOneElements()
	{
		Assert.True(await AsyncSeq(1).Exactly(1));
	}

	[Fact]
	public async ValueTask ExactlyWithManyElementHasExactlyOneElement()
	{
		Assert.False(await AsyncSeq(1, 2, 3).Exactly(1));
	}

	[Fact]
	public async ValueTask ExactlyDoesNotIterateUnnecessaryElements()
	{
		Assert.False(await AsyncSeqExceptionAt(4).Exactly(2));
	}
}
