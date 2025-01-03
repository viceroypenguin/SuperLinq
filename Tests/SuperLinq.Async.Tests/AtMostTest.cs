namespace SuperLinq.Async.Tests;

public sealed class AtMostTest
{
	[Test]
	public async Task AtMostWithNegativeCount()
	{
		_ = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
			await new AsyncBreakingSequence<int>().AtMost(-1));
	}

	[Test]
	public async Task AtMostWithEmptySequenceHasAtMostZeroElements()
	{
		await using var xs = AsyncEnumerable.Empty<int>().AsTestingSequence();
		Assert.True(await xs.AtMost(0));
	}

	[Test]
	public async Task AtMostWithEmptySequenceHasAtMostOneElement()
	{
		await using var xs = AsyncEnumerable.Empty<int>().AsTestingSequence();
		Assert.True(await xs.AtMost(1));
	}

	[Test]
	public async Task AtMostWithSingleElementHasAtMostZeroElements()
	{
		await using var xs = TestingSequence.Of(1);
		Assert.False(await xs.AtMost(0));
	}

	[Test]
	public async Task AtMostWithSingleElementHasAtMostOneElement()
	{
		await using var xs = TestingSequence.Of(1);
		Assert.True(await xs.AtMost(1));
	}

	[Test]
	public async Task AtMostWithSingleElementHasAtMostManyElements()
	{
		await using var xs = TestingSequence.Of(1);
		Assert.True(await xs.AtMost(2));
	}

	[Test]
	public async Task AtMostWithManyElementsHasAtMostOneElements()
	{
		await using var xs = TestingSequence.Of(1, 2, 3);
		Assert.False(await xs.AtMost(1));
	}

	[Test]
	public async Task AtMostDoesNotIterateUnnecessaryElements()
	{
		await using var xs = AsyncSeqExceptionAt(4).AsTestingSequence();
		Assert.False(await xs.AtMost(2));
	}
}
