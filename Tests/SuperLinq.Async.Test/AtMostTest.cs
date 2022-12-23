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
		await using var xs = AsyncEnumerable.Empty<int>().AsTestingSequence();
		Assert.True(await xs.AtMost(0));
	}

	[Fact]
	public async Task AtMostWithEmptySequenceHasAtMostOneElement()
	{
		await using var xs = AsyncEnumerable.Empty<int>().AsTestingSequence();
		Assert.True(await xs.AtMost(1));
	}

	[Fact]
	public async Task AtMostWithSingleElementHasAtMostZeroElements()
	{
		await using var xs = AsyncSeq(1).AsTestingSequence();
		Assert.False(await xs.AtMost(0));
	}

	[Fact]
	public async Task AtMostWithSingleElementHasAtMostOneElement()
	{
		await using var xs = AsyncSeq(1).AsTestingSequence();
		Assert.True(await xs.AtMost(1));
	}

	[Fact]
	public async Task AtMostWithSingleElementHasAtMostManyElements()
	{
		await using var xs = AsyncSeq(1).AsTestingSequence();
		Assert.True(await xs.AtMost(2));
	}

	[Fact]
	public async Task AtMostWithManyElementsHasAtMostOneElements()
	{
		await using var xs = AsyncSeq(1, 2, 3).AsTestingSequence();
		Assert.False(await xs.AtMost(1));
	}

	[Fact]
	public async Task AtMostDoesNotIterateUnnecessaryElements()
	{
		await using var xs = AsyncSeqExceptionAt(4).AsTestingSequence();
		Assert.False(await xs.AtMost(2));
	}
}
