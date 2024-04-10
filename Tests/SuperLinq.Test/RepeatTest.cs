namespace Test;

public sealed class RepeatTest
{
	[Theory]
	[InlineData(1)]
	[InlineData(10)]
	[InlineData(50)]
	public void RepeatItemForeverBehavior(int repeats)
	{
		var result = SuperEnumerable.Repeat(42);

		Assert.True(result
			.Take(repeats)
			.AssertCount(repeats)
			.All(x => x == 42));
	}

	[Fact]
	public void RepeatIsLazy()
	{
		_ = new BreakingSequence<int>().Repeat(4);
	}

	[Fact]
	public void RepeatValidatesArguments()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>("count", () =>
			new BreakingSequence<int>().Repeat(0));
	}

	[Fact]
	public void RepeatBehavior()
	{
		using var sequence = Enumerable.Range(1, 10).AsTestingSequence();

		var result = sequence.Repeat(3);

		var expected = Enumerable.Empty<int>();
		for (var i = 0; i < 3; i++)
			expected = expected.Concat(Enumerable.Range(1, 10));

		result.AssertSequenceEqual(expected);
	}

	[Fact]
	public void RepeatForeverIsLazy()
	{
		_ = new BreakingSequence<int>().Repeat();
	}

	[Theory]
	[InlineData(1)]
	[InlineData(10)]
	[InlineData(50)]
	public void RepeatForeverBehavior(int repeats)
	{
		using var sequence = Enumerable.Range(1, 10).AsTestingSequence();

		var result = sequence.Repeat();

		var expected = Enumerable.Empty<int>();
		for (var i = 0; i < repeats; i++)
			expected = expected.Concat(Enumerable.Range(1, 10));

		result.Take(repeats * 10).AssertSequenceEqual(expected);
	}
}
