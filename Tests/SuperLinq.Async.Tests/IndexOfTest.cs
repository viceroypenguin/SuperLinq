#if !NO_INDEX

namespace SuperLinq.Async.Tests;

public sealed class IndexOfTest
{
	[Test]
	public async Task IndexOfWithNegativeCount()
	{
		await using var sequence = TestingSequence.Of(1);
		_ = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
			await sequence.IndexOf(1, 1, -1));
	}

	[Test]
	public async Task IndexOfWorksWithEmptySequence()
	{
		await using var sequence = TestingSequence.Of<int>();
		Assert.Equal(-1, await sequence.IndexOf(5));
	}

	[Test]
	public async Task IndexOfFromStart()
	{
		await using var sequence = AsyncEnumerable.Range(100, 5).AsTestingSequence();
		Assert.Equal(
			2,
			await sequence.IndexOf(102));
	}

	[Test]
	public async Task IndexOfFromStartCount()
	{
		await using var sequence = AsyncEnumerable.Range(100, 5).AsTestingSequence();
		Assert.Equal(
			2,
			await sequence.IndexOf(102, 0, 3));
	}

	[Test]
	public async Task IndexOfFromStartIndex()
	{
		await using var sequence = AsyncEnumerable.Range(100, 5).Concat(AsyncEnumerable.Range(100, 5)).AsTestingSequence();
		Assert.Equal(
			7,
			await sequence.IndexOf(102, 5));
	}

	[Test]
	public async Task IndexOfFromEndIndex()
	{
		await using var sequence = AsyncEnumerable.Range(100, 5).Concat(AsyncEnumerable.Range(100, 5)).AsTestingSequence();
		Assert.Equal(
			7,
			await sequence.IndexOf(102, ^5));
	}

	[Test]
	public async Task IndexOfMissingValueFromStart()
	{
		await using var sequence = AsyncEnumerable.Range(100, 5).AsTestingSequence();
		Assert.Equal(
			-1,
			await sequence.IndexOf(95));
	}

	[Test]
	public async Task IndexOfMissingValueFromEnd()
	{
		await using var sequence = AsyncEnumerable.Range(100, 5).AsTestingSequence();
		Assert.Equal(
			-1,
			await sequence.IndexOf(95, ^5));
	}

	[Test]
	public async Task IndexOfMissingValueFromStartCount()
	{
		await using var sequence = AsyncEnumerable.Range(100, 5).AsTestingSequence();
		Assert.Equal(
			-1,
			await sequence.IndexOf(104, 0, 4));
	}

	[Test]
	public async Task IndexOfMissingValueFromEndCount()
	{
		await using var sequence = AsyncEnumerable.Range(100, 5).AsTestingSequence();
		Assert.Equal(
			-1,
			await sequence.IndexOf(104, ^5, 4));
	}

	[Test]
	public async Task IndexOfDoesNotIterateUnnecessaryElements()
	{
		await using var source = SuperLinq.SuperEnumerable
			.From(
				() => "ana",
				() => "beatriz",
				() => "carla",
				() => "bob",
				() => "davi",
				() => throw new TestException(),
				() => "angelo",
				() => "carlos")
			.AsTestingSequence();

		Assert.Equal(4, await source.IndexOf("davi"));
	}

	[Test]
	public async Task IndexOfDoesNotIterateUnnecessaryElementsCount()
	{
		await using var source = SuperLinq.SuperEnumerable
			.From(
				() => "ana",
				() => "beatriz",
				() => "carla",
				() => "bob",
				() => "davi",
				() => throw new TestException(),
				() => "angelo",
				() => "carlos")
			.AsTestingSequence();

		Assert.Equal(-1, await source.IndexOf("carlos", 0, 5));
	}
}

#endif
