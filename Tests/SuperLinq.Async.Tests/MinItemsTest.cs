namespace SuperLinq.Async.Tests;

public sealed class MinItemsTest
{
	[Test]
	public void MinItemsIsLazy()
	{
		_ = new AsyncBreakingSequence<int>().MinItems();
	}

	[Test]
	public async Task MinItemsEmptyList()
	{
		await using var seq = TestingSequence.Of<int>();
		var result = seq.MinItems();
		await result.AssertSequenceEqual();
	}

	[Test]
	public async Task MinItemsBehavior()
	{
		await using var seq = TestingSequence.Of(2, 2, 0, 5, 5, 1, 1, 0, 3, 4, 2, 3, 1, 4, 0, 2, 4, 3, 3, 0);
		var result = seq.MinItems();
		await result.AssertSequenceEqual(0, 0, 0, 0);
	}

	[Test]
	public void MinItemsComparerIsLazy()
	{
		_ = new AsyncBreakingSequence<int>().MinItems(comparer: null);
	}

	[Test]
	public async Task MinItemsComparerEmptyList()
	{
		await using var seq = TestingSequence.Of<int>();
		var result = seq.MinItems(comparer: Comparer<int>.Default);
		await result.AssertSequenceEqual();
	}

	[Test]
	public async Task MinItemsComparerBehavior()
	{
		await using var seq = TestingSequence.Of(2, 2, 0, 5, 5, 1, 1, 0, 3, 4, 2, 3, 1, 4, 0, 2, 4, 3, 3, 0);
		var result = seq.MinItems(comparer: Comparer<int>.Create((x, y) => Comparer<int>.Default.Compare(x % 3, y % 3)));
		await result.AssertSequenceEqual(0, 0, 3, 3, 0, 3, 3, 0);
	}

	[Test]
	public void MinItemsByIsLazy()
	{
		_ = new AsyncBreakingSequence<int>().MinItemsBy(BreakingFunc.Of<int, int>());
		_ = new AsyncBreakingSequence<int>().MinByWithTies(BreakingFunc.Of<int, int>());
	}

	[Test]
	public async Task MinItemsByEmptyList()
	{
		await using var seq = TestingSequence.Of<int>();
		var result = seq.MinItemsBy(x => -x);
		await result.AssertSequenceEqual();
	}

	[Test]
	public async Task MinItemsByBehavior()
	{
		await using var seq = TestingSequence.Of(2, 2, 0, 5, 5, 1, 1, 0, 3, 4, 2, 3, 1, 4, 0, 2, 4, 3, 3, 0);
		var result = seq.MinItemsBy(x => -x);
		await result.AssertSequenceEqual(5, 5);
	}

	[Test]
	public void MinItemsByComparerIsLazy()
	{
		_ = new AsyncBreakingSequence<int>().MinItemsBy(BreakingFunc.Of<int, int>(), comparer: null);
		_ = new AsyncBreakingSequence<int>().MinByWithTies
			(BreakingFunc.Of<int, int>(), comparer: null);
	}

	[Test]
	public async Task MinItemsByComparerEmptyList()
	{
		await using var seq = TestingSequence.Of<int>();
		var result = seq.MinItemsBy(x => -x, comparer: Comparer<int>.Default);
		await result.AssertSequenceEqual();
	}

	[Test]
	public async Task MinItemsByComparerBehavior()
	{
		await using var seq = TestingSequence.Of(2, 2, 0, 5, 5, 1, 1, 0, 3, 4, 2, 3, 1, 4, 0, 2, 4, 3, 3, 0);
		var result = seq.MinItemsBy(x => -x, comparer: Comparer<int>.Create((x, y) => Comparer<int>.Default.Compare(x % 3, y % 3)));
		await result.AssertSequenceEqual(2, 2, 5, 5, 2, 2);
	}
}
