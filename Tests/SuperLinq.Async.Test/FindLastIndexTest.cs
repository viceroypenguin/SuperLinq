#if !NO_INDEX

namespace Test.Async;

public sealed class FindLastIndexTest
{
	[Fact]
	public async Task FindLastIndexWithNegativeCount()
	{
		_ = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
			await new AsyncBreakingSequence<int>().FindLastIndex(i => i == 1, 1, -1));
	}

	[Fact]
	public async Task FindLastIndexWorksWithEmptySequence()
	{
		await using var sequence = TestingSequence.Of<int>();
		Assert.Equal(-1, await sequence.FindLastIndex(i => i == 5));
	}

	[Fact]
	public async Task FindLastIndexFromStart()
	{
		await using var sequence = AsyncEnumerable.Range(100, 5)
			.Concat(AsyncEnumerable.Range(100, 5))
			.AsTestingSequence();
		Assert.Equal(
			7,
			await sequence.FindLastIndex(i => i == 102));
	}

	[Fact]
	public async Task FindLastIndexFromStartCount()
	{
		await using var sequence = AsyncEnumerable.Range(100, 5)
			.Concat(AsyncEnumerable.Range(100, 5))
			.AsTestingSequence();
		Assert.Equal(
			7,
			await sequence.FindLastIndex(i => i == 102, int.MaxValue, 8));
	}

	[Fact]
	public async Task FindLastIndexFromStartIndex()
	{
		await using var sequence = AsyncEnumerable.Range(100, 5)
			.Concat(AsyncEnumerable.Range(100, 5))
			.AsTestingSequence();
		Assert.Equal(
			7,
			await sequence.FindLastIndex(i => i == 102, 8));
	}

	[Fact]
	public async Task FindLastIndexFromEndIndex()
	{
		await using var sequence = AsyncEnumerable.Range(100, 5)
			.Concat(AsyncEnumerable.Range(100, 5))
			.AsTestingSequence();
		Assert.Equal(
			7,
			await sequence.FindLastIndex(i => i == 102, ^3));
	}

	[Fact]
	public async Task FindLastIndexMissingValueFromStart()
	{
		await using var sequence = AsyncEnumerable.Range(100, 5).AsTestingSequence();
		Assert.Equal(
			-1,
			await sequence.FindLastIndex(i => i == 95));
	}

	[Fact]
	public async Task FindLastIndexMissingValueFromEnd()
	{
		await using var sequence = AsyncEnumerable.Range(100, 5).AsTestingSequence();
		Assert.Equal(
			-1,
			await sequence.FindLastIndex(i => i == 95, ^5));
	}

	[Fact]
	public async Task FindLastIndexMissingValueFromStartCount()
	{
		await using var sequence = AsyncEnumerable.Range(100, 5).AsTestingSequence();
		Assert.Equal(
			-1,
			await sequence.FindLastIndex(i => i == 100, int.MaxValue, 4));
	}

	[Fact]
	public async Task FindLastIndexMissingValueFromEndCount()
	{
		await using var sequence = AsyncEnumerable.Range(100, 5).AsTestingSequence();
		Assert.Equal(
			-1,
			await sequence.FindLastIndex(i => i == 100, ^1, 3));
	}
}

#endif
