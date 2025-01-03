namespace SuperLinq.Async.Tests;

public sealed class ExactlyTest
{
	[Test]
	public async Task ExactlyWithNegativeCount()
	{
		_ = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
			await new AsyncBreakingSequence<int>().Exactly(-1));
	}

	[Test]
	public async Task ExactlyWithEmptySequenceHasExactlyZeroElements()
	{
		await using var xs = AsyncEnumerable.Empty<int>().AsTestingSequence();
		Assert.True(await xs.Exactly(0));
	}

	[Test]
	public async Task ExactlyWithEmptySequenceHasExactlyOneElement()
	{
		await using var xs = AsyncEnumerable.Empty<int>().AsTestingSequence();
		Assert.False(await xs.Exactly(1));
	}

	[Test]
	public async Task ExactlyWithSingleElementHasExactlyOneElements()
	{
		await using var xs = TestingSequence.Of(1);
		Assert.True(await xs.Exactly(1));
	}

	[Test]
	public async Task ExactlyWithManyElementHasExactlyOneElement()
	{
		await using var xs = TestingSequence.Of(1, 2, 3);
		Assert.False(await xs.Exactly(1));
	}

	[Test]
	public async Task ExactlyDoesNotIterateUnnecessaryElements()
	{
		await using var xs = AsyncSeqExceptionAt(4).AsTestingSequence();
		Assert.False(await xs.Exactly(2));
	}
}
