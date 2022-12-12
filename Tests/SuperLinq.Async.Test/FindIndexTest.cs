namespace Test.Async;

public class FindIndexTest
{
	[Fact]
	public async Task FindIndexWithNegativeCount()
	{
		await using var sequence = AsyncSeq(1).AsTestingSequence();
		await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
			await sequence.FindIndex(BreakingFunc.Of<int, bool>(), 1, -1));
	}

	[Fact]
	public async Task FindIndexWorksWithEmptySequence()
	{
		await using var sequence = AsyncSeq<int>().AsTestingSequence();
		Assert.Equal(-1, await sequence.FindIndex(BreakingFunc.Of<int, bool>()));
	}

	[Fact]
	public async Task FindIndexFromStart()
	{
		await using var sequence = AsyncEnumerable.Range(100, 5).AsTestingSequence();
		Assert.Equal(
			2,
			await sequence.FindIndex(i => i == 102));
	}

	[Fact]
	public async Task FindIndexFromStartCount()
	{
		await using var sequence = AsyncEnumerable.Range(100, 5).AsTestingSequence();
		Assert.Equal(
			2,
			await sequence.FindIndex(i => i == 102, 0, 3));
	}

	[Fact]
	public async Task FindIndexFromStartIndex()
	{
		await using var sequence = AsyncEnumerable.Range(100, 5)
			.Concat(AsyncEnumerable.Range(100, 5))
			.AsTestingSequence();
		Assert.Equal(
			7,
			await sequence.FindIndex(i => i == 102, 5));
	}

	[Fact]
	public async Task FindIndexFromEndIndex()
	{
		await using var sequence = AsyncEnumerable.Range(100, 5)
			.Concat(AsyncEnumerable.Range(100, 5))
			.AsTestingSequence();
		Assert.Equal(
			7,
			await sequence.FindIndex(i => i == 102, ^5));
	}

	[Fact]
	public async Task FindIndexMissingValueFromStart()
	{
		await using var sequence = AsyncEnumerable.Range(100, 5).AsTestingSequence();
		Assert.Equal(
			-1,
			await sequence.FindIndex(i => i == 95));
	}

	[Fact]
	public async Task FindIndexMissingValueFromEnd()
	{
		await using var sequence = AsyncEnumerable.Range(100, 5).AsTestingSequence();
		Assert.Equal(
			-1,
			await sequence.FindIndex(i => i == 95, ^5));
	}

	[Fact]
	public async Task FindIndexMissingValueFromStartCount()
	{
		await using var sequence = AsyncEnumerable.Range(100, 5).AsTestingSequence();
		Assert.Equal(
			-1,
			await sequence.FindIndex(i => i == 104, 0, 4));
	}

	[Fact]
	public async Task FindIndexMissingValueFromEndCount()
	{
		await using var sequence = AsyncEnumerable.Range(100, 5).AsTestingSequence();
		Assert.Equal(
			-1,
			await sequence.FindIndex(i => i == 104, ^5, 4));
	}
}
