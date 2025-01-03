namespace SuperLinq.Async.Tests;

public sealed class RepeatTest
{
	[Test]
	[Arguments(1)]
	[Arguments(10)]
	[Arguments(50)]
	public async Task RepeatItemForeverBehavior(int repeats)
	{
		var result = AsyncSuperEnumerable.Repeat(42);

		Assert.True(await result
			.Take(repeats)
			.AssertCount(repeats)
			.AllAsync(x => x == 42));
	}

	[Test]
	public void RepeatIsLazy()
	{
		_ = new AsyncBreakingSequence<int>().Repeat(4);
	}

	[Test]
	public void RepeatValidatesArguments()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>("count", () =>
			new AsyncBreakingSequence<int>().Repeat(0));
	}

	[Test]
	public async Task RepeatBehavior()
	{
		await using var sequence = Enumerable.Range(1, 10).AsTestingSequence();

		var result = sequence.Repeat(3);

		var expected = Enumerable.Empty<int>();
		for (var i = 0; i < 3; i++)
			expected = expected.Concat(Enumerable.Range(1, 10));

		await result.AssertSequenceEqual(expected);
	}

	[Test]
	public void RepeatForeverIsLazy()
	{
		_ = new AsyncBreakingSequence<int>().Repeat();
	}

	[Test]
	[Arguments(1)]
	[Arguments(10)]
	[Arguments(50)]
	public async Task RepeatForeverBehavior(int repeats)
	{
		await using var sequence = Enumerable.Range(1, 10).AsTestingSequence();

		var result = sequence.Repeat();

		var expected = Enumerable.Empty<int>();
		for (var i = 0; i < repeats; i++)
			expected = expected.Concat(Enumerable.Range(1, 10));

		await result.Take(repeats * 10).AssertSequenceEqual(expected);
	}
}
