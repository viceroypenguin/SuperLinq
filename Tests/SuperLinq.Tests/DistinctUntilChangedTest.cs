namespace SuperLinq.Tests;

public sealed class DistinctUntilChangedTest
{
	[Fact]
	public void DistinctUntilChangedIsLazy()
	{
		_ = new BreakingSequence<int>().DistinctUntilChanged();
	}

	[Fact]
	public void DistinctUntilChangedEmptySequence()
	{
		using var source = TestingSequence.Of<int>();
		var result = source.DistinctUntilChanged();
		result.AssertSequenceEqual();
	}

	[Fact]
	public void DistinctUntilChanged()
	{
		using var source = TestingSequence.Of(2, 2, 0, 5, 5, 1, 1, 0, 3, 0, 2, 3, 1, 4, 0, 2, 4, 3, 3, 0);
		var result = source.DistinctUntilChanged();
		result.AssertSequenceEqual(2, 0, 5, 1, 0, 3, 0, 2, 3, 1, 4, 0, 2, 4, 3, 0);
	}

	[Fact]
	public void DistinctUntilChangedComparer()
	{
		using var source = TestingSequence.Of(2, 2, 0, 5, 5, 1, 1, 0, 3, 0, 2, 3, 1, 4, 0, 2, 4, 3, 3, 0);
		var result = source.DistinctUntilChanged(
			EqualityComparer.Create<int>((x, y) => (x % 3) == (y % 3)));
		result.AssertSequenceEqual(2, 0, 5, 1, 0, 2, 3, 1, 0, 2, 4, 3);
	}

	[Fact]
	public void DistinctUntilChangedSelectorIsLazy()
	{
		_ = new BreakingSequence<int>().DistinctUntilChanged(BreakingFunc.Of<int, int>());
	}

	[Fact]
	public void DistinctUntilChangedSelectorEmptySequence()
	{
		using var source = TestingSequence.Of<int>();
		var result = source.DistinctUntilChanged(BreakingFunc.Of<int, int>());
		result.AssertSequenceEqual();
	}

	[Fact]
	public void DistinctUntilChangedSelector()
	{
		using var source = TestingSequence.Of(
			"one",
			"two",
			"three",
			"four",
			"five",
			"six",
			"seven",
			"eight",
			"nine",
			"ten");
		var result = source.DistinctUntilChanged(x => x.Length);
		result.AssertSequenceEqual(
			"one",
			"three",
			"four",
			"six",
			"seven",
			"nine",
			"ten");
	}

	[Fact]
	public void DistinctUntilChangedSelectorComparer()
	{
		using var source = TestingSequence.Of(
			"one",
			"two",
			"three",
			"four",
			"five",
			"six",
			"seven",
			"eight",
			"nine",
			"ten");
		var result = source.DistinctUntilChanged(
			x => x.Length,
			EqualityComparer.Create<int>((x, y) => Math.Abs(x - y) <= 1));
		result.AssertSequenceEqual(
			"one",
			"three",
			"six",
			"seven",
			"ten");
	}
}
