namespace Test;

public sealed class MaxItemsTest
{
	[Fact]
	public void MaxItemsIsLazy()
	{
		_ = new BreakingSequence<int>().MaxItems();
	}

	[Fact]
	public void MaxItemsEmptyList()
	{
		using var seq = TestingSequence.Of<int>();
		var result = seq.MaxItems();
		result.AssertSequenceEqual();
	}

	[Fact]
	public void MaxItemsBehavior()
	{
		using var seq = TestingSequence.Of(2, 2, 0, 5, 5, 1, 1, 0, 3, 4, 2, 3, 1, 4, 0, 2, 4, 3, 3, 0);
		var result = seq.MaxItems();
		result.AssertSequenceEqual(5, 5);
	}

	[Fact]
	public void MaxItemsComparerIsLazy()
	{
		_ = new BreakingSequence<int>().MaxItems(comparer: null);
	}

	[Fact]
	public void MaxItemsComparerEmptyList()
	{
		using var seq = TestingSequence.Of<int>();
		var result = seq.MaxItems(comparer: Comparer<int>.Default);
		result.AssertSequenceEqual();
	}

	[Fact]
	public void MaxItemsComparerBehavior()
	{
		using var seq = TestingSequence.Of(2, 2, 0, 5, 5, 1, 1, 0, 3, 4, 2, 3, 1, 4, 0, 2, 4, 3, 3, 0);
		var result = seq.MaxItems(comparer: Comparer<int>.Create((x, y) => Comparer<int>.Default.Compare(x % 3, y % 3)));
		result.AssertSequenceEqual(2, 2, 5, 5, 2, 2);
	}

	[Fact]
	public void MaxItemsByIsLazy()
	{
		_ = new BreakingSequence<int>().MaxItemsBy(BreakingFunc.Of<int, int>());
		_ = new BreakingSequence<int>().MaxByWithTies(BreakingFunc.Of<int, int>());
	}

	[Fact]
	public void MaxItemsByEmptyList()
	{
		using var seq = TestingSequence.Of<int>();
		var result = seq.MaxItemsBy(x => -x);
		result.AssertSequenceEqual();
	}

	[Fact]
	public void MaxItemsByBehavior()
	{
		using var seq = TestingSequence.Of(2, 2, 0, 5, 5, 1, 1, 0, 3, 4, 2, 3, 1, 4, 0, 2, 4, 3, 3, 0);
		var result = seq.MaxItemsBy(x => -x);
		result.AssertSequenceEqual(0, 0, 0, 0);
	}

	[Fact]
	public void MaxItemsByComparerIsLazy()
	{
		_ = new BreakingSequence<int>().MaxByWithTies(BreakingFunc.Of<int, int>(), comparer: null);
	}

	[Fact]
	public void MaxItemsByComparerEmptyList()
	{
		using var seq = TestingSequence.Of<int>();
		var result = seq.MaxItemsBy(x => -x, comparer: Comparer<int>.Default);
		result.AssertSequenceEqual();
	}

	[Fact]
	public void MaxItemsByComparerBehavior()
	{
		using var seq = TestingSequence.Of(2, 2, 0, 5, 5, 1, 1, 0, 3, 4, 2, 3, 1, 4, 0, 2, 4, 3, 3, 0);
		var result = seq.MaxItemsBy(x => -x, comparer: Comparer<int>.Create((x, y) => Comparer<int>.Default.Compare(x % 3, y % 3)));
		result.AssertSequenceEqual(0, 0, 3, 3, 0, 3, 3, 0);
	}
}
