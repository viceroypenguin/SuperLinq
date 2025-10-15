namespace SuperLinq.Tests;

public sealed class MinItemsTest
{
	[Fact]
	public void MinItemsIsLazy()
	{
		_ = new BreakingSequence<int>().MinItems();
	}

	[Fact]
	public void MinItemsEmptyList()
	{
		using var seq = TestingSequence.Of<int>();
		var result = seq.MinItems();
		result.AssertSequenceEqual();
	}

	[Fact]
	public void MinItemsBehavior()
	{
		using var seq = TestingSequence.Of(2, 2, 0, 5, 5, 1, 1, 0, 3, 4, 2, 3, 1, 4, 0, 2, 4, 3, 3, 0);
		var result = seq.MinItems();
		result.AssertSequenceEqual(0, 0, 0, 0);
	}

	[Fact]
	public void MinItemsComparerIsLazy()
	{
		_ = new BreakingSequence<int>().MinItems(comparer: null);
	}

	[Fact]
	public void MinItemsComparerEmptyList()
	{
		using var seq = TestingSequence.Of<int>();
		var result = seq.MinItems(comparer: Comparer<int>.Default);
		result.AssertSequenceEqual();
	}

	[Fact]
	public void MinItemsComparerBehavior()
	{
		using var seq = TestingSequence.Of(2, 2, 0, 5, 5, 1, 1, 0, 3, 4, 2, 3, 1, 4, 0, 2, 4, 3, 3, 0);
		var result = seq.MinItems(comparer: Comparer<int>.Create((x, y) => Comparer<int>.Default.Compare(x % 3, y % 3)));
		result.AssertSequenceEqual(0, 0, 3, 3, 0, 3, 3, 0);
	}

	[Fact]
	public void MinItemsByIsLazy()
	{
		_ = new BreakingSequence<int>().MinItemsBy(BreakingFunc.Of<int, int>());
		_ = new BreakingSequence<int>().MinByWithTies(BreakingFunc.Of<int, int>());
	}

	[Fact]
	public void MinItemsByEmptyList()
	{
		using var seq = TestingSequence.Of<int>();
		var result = seq.MinItemsBy(x => -x);
		result.AssertSequenceEqual();
	}

	[Fact]
	public void MinItemsByBehavior()
	{
		using var seq = TestingSequence.Of(2, 2, 0, 5, 5, 1, 1, 0, 3, 4, 2, 3, 1, 4, 0, 2, 4, 3, 3, 0);
		var result = seq.MinItemsBy(x => -x);
		result.AssertSequenceEqual(5, 5);
	}

	[Fact]
	public void MinItemsByComparerIsLazy()
	{
		_ = new BreakingSequence<int>().MinItemsBy(BreakingFunc.Of<int, int>(), comparer: null);
		_ = new BreakingSequence<int>().MinByWithTies(BreakingFunc.Of<int, int>(), comparer: null);
	}

	[Fact]
	public void MinItemsByComparerEmptyList()
	{
		using var seq = TestingSequence.Of<int>();
		var result = seq.MinItemsBy(x => -x, comparer: Comparer<int>.Default);
		result.AssertSequenceEqual();
	}

	[Fact]
	public void MinItemsByComparerBehavior()
	{
		using var seq = TestingSequence.Of(2, 2, 0, 5, 5, 1, 1, 0, 3, 4, 2, 3, 1, 4, 0, 2, 4, 3, 3, 0);
		var result = seq.MinItemsBy(x => -x, comparer: Comparer<int>.Create((x, y) => Comparer<int>.Default.Compare(x % 3, y % 3)));
		result.AssertSequenceEqual(2, 2, 5, 5, 2, 2);
	}
}
