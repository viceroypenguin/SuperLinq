namespace Test.Async;

public sealed class DistinctUntilChangedTest
{
	[Fact]
	public void DistinctUntilChangedIsLazy()
	{
		_ = new AsyncBreakingSequence<int>().DistinctUntilChanged();
	}

	[Fact]
	public async Task DistinctUntilChangedEmptySequence()
	{
		await using var source = TestingSequence.Of<int>();
		var result = source.DistinctUntilChanged();
		await result.AssertSequenceEqual();
	}

	[Fact]
	public async Task DistinctUntilChanged()
	{
		await using var source = TestingSequence.Of(2, 2, 0, 5, 5, 1, 1, 0, 3, 0, 2, 3, 1, 4, 0, 2, 4, 3, 3, 0);
		var result = source.DistinctUntilChanged();
		await result.AssertSequenceEqual(2, 0, 5, 1, 0, 3, 0, 2, 3, 1, 4, 0, 2, 4, 3, 0);
	}

	[Fact]
	public async Task DistinctUntilChangedComparer()
	{
		await using var source = TestingSequence.Of(2, 2, 0, 5, 5, 1, 1, 0, 3, 0, 2, 3, 1, 4, 0, 2, 4, 3, 3, 0);
		var result = source.DistinctUntilChanged(
			EqualityComparer.Create<int>((x, y) => (x % 3) == (y % 3)));
		await result.AssertSequenceEqual(2, 0, 5, 1, 0, 2, 3, 1, 0, 2, 4, 3);
	}

	[Fact]
	public void DistinctUntilChangedSelectorIsLazy()
	{
		_ = new AsyncBreakingSequence<int>().DistinctUntilChanged(BreakingFunc.Of<int, int>());
		_ = new AsyncBreakingSequence<int>().DistinctUntilChanged(BreakingFunc.Of<int, ValueTask<int>>());
	}

	[Fact]
	public async Task DistinctUntilChangedSelectorEmptySequence()
	{
		await using var source = TestingSequence.Of<int>();
		var result = source.DistinctUntilChanged(BreakingFunc.Of<int, int>());
		await result.AssertSequenceEqual();
	}

	[Fact]
	public async Task DistinctUntilChangedSelector()
	{
		await using var source = TestingSequence.Of(
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
		await result.AssertSequenceEqual(
			"one",
			"three",
			"four",
			"six",
			"seven",
			"nine",
			"ten");
	}

	[Fact]
	public async Task DistinctUntilChangedSelectorComparer()
	{
		await using var source = TestingSequence.Of(
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
		await result.AssertSequenceEqual(
			"one",
			"three",
			"six",
			"seven",
			"ten");
	}
}
