namespace Test.Async;

public class ExactlyTest
{
	[Fact]
	public async Task ExactlyWithNegativeCount()
	{
		await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
			await new AsyncBreakingSequence<int>().Exactly(-1));
	}

	[Fact]
	public async Task ExactlyWithEmptySequenceHasExactlyZeroElements()
	{
		await using var xs = AsyncEnumerable.Empty<int>().AsTestingSequence();
		Assert.True(await xs.Exactly(0));
	}

	[Fact]
	public async Task ExactlyWithEmptySequenceHasExactlyOneElement()
	{
		await using var xs = AsyncEnumerable.Empty<int>().AsTestingSequence();
		Assert.False(await xs.Exactly(1));
	}

	[Fact]
	public async Task ExactlyWithSingleElementHasExactlyOneElements()
	{
		await using var xs = AsyncSeq(1).AsTestingSequence();
		Assert.True(await xs.Exactly(1));
	}

	[Fact]
	public async Task ExactlyWithManyElementHasExactlyOneElement()
	{
		await using var xs = AsyncSeq(1, 2, 3).AsTestingSequence();
		Assert.False(await xs.Exactly(1));
	}

	[Fact]
	public async Task ExactlyDoesNotIterateUnnecessaryElements()
	{
		await using var xs = AsyncSeqExceptionAt(4).AsTestingSequence();
		Assert.False(await xs.Exactly(2));
	}
}
