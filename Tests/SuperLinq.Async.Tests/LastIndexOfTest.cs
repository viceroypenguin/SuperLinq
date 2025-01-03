#if !NO_INDEX

namespace SuperLinq.Async.Tests;

public sealed class LastIndexOfTest
{
	[Test]
	public async Task LastIndexOfWithNegativeCount()
	{
		await using var sequence = TestingSequence.Of(1);
		_ = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
			await sequence.LastIndexOf(1, 1, -1));
	}

	[Test]
	public async Task LastIndexOfWorksWithEmptySequence()
	{
		await using var sequence = Array.Empty<int>().AsTestingSequence();
		Assert.Equal(-1, await sequence.LastIndexOf(5));
	}

	[Test]
	public async Task LastIndexOfFromStart()
	{
		await using var sequence = AsyncEnumerable.Range(100, 5)
			.Concat(AsyncEnumerable.Range(100, 5))
			.AsTestingSequence();
		Assert.Equal(
			7,
			await sequence.LastIndexOf(102));
	}

	[Test]
	public async Task LastIndexOfFromStartCount()
	{
		await using var sequence = AsyncEnumerable.Range(100, 5)
			.Concat(AsyncEnumerable.Range(100, 5))
			.AsTestingSequence();
		Assert.Equal(
			7,
			await sequence.LastIndexOf(102, int.MaxValue, 8));
	}

	[Test]
	public async Task LastIndexOfFromStartIndex()
	{
		await using var sequence = AsyncEnumerable.Range(100, 5)
			.Concat(AsyncEnumerable.Range(100, 5))
			.AsTestingSequence();
		Assert.Equal(
			7,
			await sequence.LastIndexOf(102, 8));
	}

	[Test]
	public async Task LastIndexOfFromEndIndex()
	{
		await using var sequence = AsyncEnumerable.Range(100, 5)
			.Concat(AsyncEnumerable.Range(100, 5))
			.AsTestingSequence();
		Assert.Equal(
			7,
			await sequence.LastIndexOf(102, ^3));
	}

	[Test]
	public async Task LastIndexOfMissingValueFromStart()
	{
		await using var sequence = AsyncEnumerable.Range(100, 5).AsTestingSequence();
		Assert.Equal(
			-1,
			await sequence.LastIndexOf(95));
	}

	[Test]
	public async Task LastIndexOfMissingValueFromEnd()
	{
		await using var sequence = AsyncEnumerable.Range(100, 5).AsTestingSequence();
		Assert.Equal(
			-1,
			await sequence.LastIndexOf(95, ^5));
	}

	[Test]
	public async Task LastIndexOfMissingValueFromStartCount()
	{
		await using var sequence = AsyncEnumerable.Range(100, 5).AsTestingSequence();
		Assert.Equal(
			-1,
			await sequence.LastIndexOf(100, int.MaxValue, 4));
	}

	[Test]
	public async Task LastIndexOfMissingValueFromEndCount()
	{
		await using var sequence = AsyncEnumerable.Range(100, 5).AsTestingSequence();
		Assert.Equal(
			-1,
			await sequence.LastIndexOf(100, ^1, 3));
	}
}

#endif
