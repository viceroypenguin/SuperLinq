namespace SuperLinq.Async.Tests;

public sealed class RepeatTest
{
	[Theory]
	[InlineData(1)]
	[InlineData(10)]
	[InlineData(50)]
	public async Task RepeatItemForeverBehavior(int repeats)
	{
		var result = AsyncSuperEnumerable.Repeat(42);

		Assert.True(await result
			.Take(repeats)
			.AssertCount(repeats)
			.AllAsync(x => x == 42));
	}

	[Fact]
	public void RepeatIsLazy()
	{
		_ = new AsyncBreakingSequence<int>().Repeat(4);
	}

	[Fact]
	public void RepeatValidatesArguments()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>("count", () =>
			new AsyncBreakingSequence<int>().Repeat(0));
	}

	[Fact]
	public async Task RepeatBehavior()
	{
		await using var sequence = Enumerable.Range(1, 10).AsTestingSequence();

		var result = sequence.Repeat(3);

		var expected = Enumerable.Empty<int>();
		for (var i = 0; i < 3; i++)
			expected = expected.Concat(Enumerable.Range(1, 10));

		await result.AssertSequenceEqual(expected);
	}

	[Fact]
	public void RepeatForeverIsLazy()
	{
		_ = new AsyncBreakingSequence<int>().Repeat();
	}

	[Theory]
	[InlineData(1)]
	[InlineData(10)]
	[InlineData(50)]
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
