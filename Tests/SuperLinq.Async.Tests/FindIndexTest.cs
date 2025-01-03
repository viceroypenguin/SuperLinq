#if !NO_INDEX

namespace SuperLinq.Async.Tests;

public sealed class FindIndexTest
{
	[Test]
	public async Task FindIndexWithNegativeCount()
	{
		_ = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
			await new AsyncBreakingSequence<int>().FindIndex(BreakingFunc.Of<int, bool>(), 1, -1));
	}

	[Test]
	public async Task FindIndexWorksWithEmptySequence()
	{
		await using var sequence = TestingSequence.Of<int>();
		Assert.Equal(-1, await sequence.FindIndex(BreakingFunc.Of<int, bool>()));
	}

	[Test]
	public async Task FindIndexFromStart()
	{
		await using var sequence = AsyncEnumerable.Range(100, 5).AsTestingSequence();
		Assert.Equal(
			2,
			await sequence.FindIndex(i => i == 102));
	}

	[Test]
	public async Task FindIndexFromStartCount()
	{
		await using var sequence = AsyncEnumerable.Range(100, 5).AsTestingSequence();
		Assert.Equal(
			2,
			await sequence.FindIndex(i => i == 102, 0, 3));
	}

	[Test]
	public async Task FindIndexFromStartIndex()
	{
		await using var sequence = AsyncEnumerable.Range(100, 5)
			.Concat(AsyncEnumerable.Range(100, 5))
			.AsTestingSequence();
		Assert.Equal(
			7,
			await sequence.FindIndex(i => i == 102, 5));
	}

	[Test]
	public async Task FindIndexFromEndIndex()
	{
		await using var sequence = AsyncEnumerable.Range(100, 5)
			.Concat(AsyncEnumerable.Range(100, 5))
			.AsTestingSequence();
		Assert.Equal(
			7,
			await sequence.FindIndex(i => i == 102, ^5));
	}

	[Test]
	public async Task FindIndexMissingValueFromStart()
	{
		await using var sequence = AsyncEnumerable.Range(100, 5).AsTestingSequence();
		Assert.Equal(
			-1,
			await sequence.FindIndex(i => i == 95));
	}

	[Test]
	public async Task FindIndexMissingValueFromEnd()
	{
		await using var sequence = AsyncEnumerable.Range(100, 5).AsTestingSequence();
		Assert.Equal(
			-1,
			await sequence.FindIndex(i => i == 95, ^5));
	}

	[Test]
	public async Task FindIndexMissingValueFromStartCount()
	{
		await using var sequence = AsyncEnumerable.Range(100, 5).AsTestingSequence();
		Assert.Equal(
			-1,
			await sequence.FindIndex(i => i == 104, 0, 4));
	}

	[Test]
	public async Task FindIndexMissingValueFromEndCount()
	{
		await using var sequence = AsyncEnumerable.Range(100, 5).AsTestingSequence();
		Assert.Equal(
			-1,
			await sequence.FindIndex(i => i == 104, ^5, 4));
	}

	[Test]
	public async Task FindIndexDoesNotIterateUnnecessaryElements()
	{
		using var source = AsyncSuperEnumerable
			.From(
				() => Task.FromResult("ana"),
				() => Task.FromResult("beatriz"),
				() => Task.FromResult("carla"),
				() => Task.FromResult("bob"),
				() => Task.FromResult("davi"),
				AsyncBreakingFunc.Of<string>(),
				() => Task.FromResult("angelo"),
				() => Task.FromResult("carlos"))
			.AsTestingSequence();

		Assert.Equal(4, await source.FindIndex(i => i == "davi"));
	}

	[Test]
	public async Task FindIndexDoesNotIterateUnnecessaryElementsCount()
	{
		using var source = AsyncSuperEnumerable
			.From(
				() => Task.FromResult("ana"),
				() => Task.FromResult("beatriz"),
				() => Task.FromResult("carla"),
				() => Task.FromResult("bob"),
				() => Task.FromResult("davi"),
				AsyncBreakingFunc.Of<string>(),
				() => Task.FromResult("angelo"),
				() => Task.FromResult("carlos"))
			.AsTestingSequence();

		Assert.Equal(-1, await source.FindIndex(i => i == "carlos", 0, 5));
	}
}

#endif
