namespace Test.Async;

public class LastIndexOfTest
{
	[Fact]
	public async Task LastIndexOfWithNegativeCount()
	{
		await using var sequence = AsyncSeq(1).AsTestingSequence();
		await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
			await sequence.LastIndexOf(1, 1, -1));
	}

	[Fact]
	public async Task LastIndexOfWorksWithEmptySequence()
	{
		await using var sequence = Array.Empty<int>().AsTestingSequence();
		Assert.Equal(-1, await sequence.LastIndexOf(5));
	}

	[Fact]
	public async Task LastIndexOfFromStart()
	{
		await using var sequence = AsyncEnumerable.Range(100, 5)
			.Concat(AsyncEnumerable.Range(100, 5))
			.AsTestingSequence();
		Assert.Equal(
			7,
			await sequence.LastIndexOf(102));
	}

	[Fact]
	public async Task LastIndexOfFromStartCount()
	{
		await using var sequence = AsyncEnumerable.Range(100, 5)
			.Concat(AsyncEnumerable.Range(100, 5))
			.AsTestingSequence();
		Assert.Equal(
			7,
			await sequence.LastIndexOf(102, int.MaxValue, 8));
	}

	[Fact]
	public async Task LastIndexOfFromStartIndex()
	{
		await using var sequence = AsyncEnumerable.Range(100, 5)
			.Concat(AsyncEnumerable.Range(100, 5))
			.AsTestingSequence();
		Assert.Equal(
			7,
			await sequence.LastIndexOf(102, 8));
	}

	[Fact]
	public async Task LastIndexOfFromEndIndex()
	{
		await using var sequence = AsyncEnumerable.Range(100, 5)
			.Concat(AsyncEnumerable.Range(100, 5))
			.AsTestingSequence();
		Assert.Equal(
			7,
			await sequence.LastIndexOf(102, ^3));
	}

	[Fact]
	public async Task LastIndexOfMissingValueFromStart()
	{
		await using var sequence = AsyncEnumerable.Range(100, 5).AsTestingSequence();
		Assert.Equal(
			-1,
			await sequence.LastIndexOf(95));
	}

	[Fact]
	public async Task LastIndexOfMissingValueFromEnd()
	{
		await using var sequence = AsyncEnumerable.Range(100, 5).AsTestingSequence();
		Assert.Equal(
			-1,
			await sequence.LastIndexOf(95, ^5));
	}

	[Fact]
	public async Task LastIndexOfMissingValueFromStartCount()
	{
		await using var sequence = AsyncEnumerable.Range(100, 5).AsTestingSequence();
		Assert.Equal(
			-1,
			await sequence.LastIndexOf(100, int.MaxValue, 4));
	}

	[Fact]
	public async Task LastIndexOfMissingValueFromEndCount()
	{
		await using var sequence = AsyncEnumerable.Range(100, 5).AsTestingSequence();
		Assert.Equal(
			-1,
			await sequence.LastIndexOf(100, ^1, 3));
	}
}
