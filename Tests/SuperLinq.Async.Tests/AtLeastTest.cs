namespace SuperLinq.Async.Tests;

public sealed class AtLeastTest
{
	[Fact]
	public async Task AtLeastWithNegativeCount()
	{
		_ = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
			await new AsyncBreakingSequence<int>().AtLeast(-1));
	}

	[Fact]
	public async Task AtLeastWithEmptySequenceHasAtLeastZeroElements()
	{
		await using var xs = AsyncEnumerable.Empty<int>().AsTestingSequence();
		Assert.True(await xs.AtLeast(0));
	}

	[Fact]
	public async Task AtLeastWithEmptySequenceHasAtLeastOneElement()
	{
		await using var xs = AsyncEnumerable.Empty<int>().AsTestingSequence();
		Assert.False(await xs.AtLeast(1));
	}

	[Fact]
	public async Task AtLeastWithEmptySequenceHasAtLeastManyElements()
	{
		await using var xs = AsyncEnumerable.Empty<int>().AsTestingSequence();
		Assert.False(await xs.AtLeast(2));
	}

	[Fact]
	public async Task AtLeastWithSingleElementHasAtLeastZeroElements()
	{
		await using var xs = TestingSequence.Of(1);
		Assert.True(await xs.AtLeast(0));
	}

	[Fact]
	public async Task AtLeastWithSingleElementHasAtLeastOneElement()
	{
		await using var xs = TestingSequence.Of(1);
		Assert.True(await xs.AtLeast(1));
	}

	[Fact]
	public async Task AtLeastWithSingleElementHasAtLeastManyElements()
	{
		await using var xs = TestingSequence.Of(1);
		Assert.False(await xs.AtLeast(2));
	}

	[Fact]
	public async Task AtLeastWithManyElementsHasAtLeastZeroElements()
	{
		await using var xs = TestingSequence.Of(1, 2, 3);
		Assert.True(await xs.AtLeast(0));
	}

	[Fact]
	public async Task AtLeastWithManyElementsHasAtLeastOneElement()
	{
		await using var xs = TestingSequence.Of(1, 2, 3);
		Assert.True(await xs.AtLeast(1));
	}

	[Fact]
	public async Task AtLeastWithManyElementsHasAtLeastManyElements()
	{
		await using var xs = TestingSequence.Of(1, 2, 3);
		Assert.True(await xs.AtLeast(2));
	}

	[Fact]
	public async Task AtLeastDoesNotIterateUnnecessaryElements()
	{
		await using var xs = AsyncSeqExceptionAt(3).AsTestingSequence();
		Assert.True(await xs.AtLeast(2));
	}
}
