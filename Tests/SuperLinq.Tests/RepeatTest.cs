namespace SuperLinq.Tests;

public sealed class RepeatTest
{
	[Test]
	[Arguments(1)]
	[Arguments(10)]
	[Arguments(50)]
	public void RepeatItemForeverBehavior(int repeats)
	{
		var result = SuperEnumerable.Repeat(42);

		Assert.True(result
			.Take(repeats)
			.AssertCount(repeats)
			.All(x => x == 42));
	}

	[Test]
	public void RepeatIsLazy()
	{
		_ = new BreakingSequence<int>().Repeat(4);
	}

	[Test]
	public void RepeatValidatesArguments()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>("count", () =>
			new BreakingSequence<int>().Repeat(0));
	}

	[Test]
	public void RepeatBehavior()
	{
		using var sequence = Enumerable.Range(1, 10).AsTestingSequence();

		var result = sequence.Repeat(3);

		var expected = Enumerable.Empty<int>();
		for (var i = 0; i < 3; i++)
			expected = expected.Concat(Enumerable.Range(1, 10));

		result.AssertSequenceEqual(expected);
	}

	[Test]
	public void RepeatForeverIsLazy()
	{
		_ = new BreakingSequence<int>().Repeat();
	}

	[Test]
	[Arguments(1)]
	[Arguments(10)]
	[Arguments(50)]
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
