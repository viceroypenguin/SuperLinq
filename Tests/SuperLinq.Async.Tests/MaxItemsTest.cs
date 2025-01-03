namespace SuperLinq.Async.Tests;

public sealed class MaxItemsTest
{
	[Test]
	public void MaxItemsIsLazy()
	{
		_ = new AsyncBreakingSequence<int>().MaxItems();
	}

	[Test]
	public async Task MaxItemsEmptyList()
	{
		await using var seq = TestingSequence.Of<int>();
		var result = seq.MaxItems();
		await result.AssertSequenceEqual();
	}

	[Test]
	public async Task MaxItemsBehavior()
	{
		await using var seq = TestingSequence.Of(2, 2, 0, 5, 5, 1, 1, 0, 3, 4, 2, 3, 1, 4, 0, 2, 4, 3, 3, 0);
		var result = seq.MaxItems();
		await result.AssertSequenceEqual(5, 5);
	}

	[Test]
	public void MaxItemsComparerIsLazy()
	{
		_ = new AsyncBreakingSequence<int>().MaxItems(comparer: null);
	}

	[Test]
	public async Task MaxItemsComparerEmptyList()
	{
		await using var seq = TestingSequence.Of<int>();
		var result = seq.MaxItems(comparer: Comparer<int>.Default);
		await result.AssertSequenceEqual();
	}

	[Test]
	public async Task MaxItemsComparerBehavior()
	{
		await using var seq = TestingSequence.Of(2, 2, 0, 5, 5, 1, 1, 0, 3, 4, 2, 3, 1, 4, 0, 2, 4, 3, 3, 0);
		var result = seq.MaxItems(comparer: Comparer<int>.Create((x, y) => Comparer<int>.Default.Compare(x % 3, y % 3)));
		await result.AssertSequenceEqual(2, 2, 5, 5, 2, 2);
	}

	[Test]
	public void MaxItemsByIsLazy()
	{
		_ = new AsyncBreakingSequence<int>().MaxItemsBy(BreakingFunc.Of<int, int>());
		_ = new AsyncBreakingSequence<int>().MaxByWithTies(BreakingFunc.Of<int, int>());
	}

	[Test]
	public async Task MaxItemsByEmptyList()
	{
		await using var seq = TestingSequence.Of<int>();
		var result = seq.MaxItemsBy(x => -x);
		await result.AssertSequenceEqual();
	}

	[Test]
	public async Task MaxItemsByBehavior()
	{
		await using var seq = TestingSequence.Of(2, 2, 0, 5, 5, 1, 1, 0, 3, 4, 2, 3, 1, 4, 0, 2, 4, 3, 3, 0);
		var result = seq.MaxItemsBy(x => -x);
		await result.AssertSequenceEqual(0, 0, 0, 0);
	}

	[Test]
	public void MaxItemsByComparerIsLazy()
	{
		_ = new AsyncBreakingSequence<int>().MaxItemsBy(BreakingFunc.Of<int, int>(), comparer: null);
		_ = new AsyncBreakingSequence<int>().MaxByWithTies(BreakingFunc.Of<int, int>(), comparer: null);
	}

	[Test]
	public async Task MaxItemsByComparerEmptyList()
	{
		await using var seq = TestingSequence.Of<int>();
		var result = seq.MaxItemsBy(x => -x, comparer: Comparer<int>.Default);
		await result.AssertSequenceEqual();
	}

	[Test]
	public async Task MaxItemsByComparerBehavior()
	{
		await using var seq = TestingSequence.Of(2, 2, 0, 5, 5, 1, 1, 0, 3, 4, 2, 3, 1, 4, 0, 2, 4, 3, 3, 0);
		var result = seq.MaxItemsBy(x => -x, comparer: Comparer<int>.Create((x, y) => Comparer<int>.Default.Compare(x % 3, y % 3)));
		await result.AssertSequenceEqual(0, 0, 3, 3, 0, 3, 3, 0);
	}
}
