﻿namespace Test.Async;

public class TraverseTest
{
	[Fact]
	public void TraverseDepthFirstIsStreaming()
	{
		AsyncSuperEnumerable.TraverseDepthFirst(new object(), o => new AsyncBreakingSequence<object>());
	}

	[Fact]
	public void TraverseBreadthFirstIsStreaming()
	{
		AsyncSuperEnumerable.TraverseBreadthFirst(new object(), o => new AsyncBreakingSequence<object>());
	}

	[Fact]
	public async Task TraverseDepthFirstPreservesChildrenOrder()
	{
		await using var root = Enumerable.Range(1, 10).AsTestingSequence();
		await using var child = Enumerable.Empty<int>().AsTestingSequence(maxEnumerations: 10);
		var res = AsyncSuperEnumerable.TraverseDepthFirst(0, i => i == 0 ? root : child);
		await res.AssertSequenceEqual(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
	}


	[Fact]
	public async Task TraverseBreadthFirstPreservesChildrenOrder()
	{
		await using var root = Enumerable.Range(1, 10).AsTestingSequence();
		await using var child = Enumerable.Empty<int>().AsTestingSequence(maxEnumerations: 10);
		var res = AsyncSuperEnumerable.TraverseBreadthFirst(0, i => i == 0 ? root : child);
		await res.AssertSequenceEqual(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
	}

	private class Tree<T>
	{
		public T Value { get; }
		public IEnumerable<Tree<T>> Children { get; }

		public Tree(T value, IEnumerable<Tree<T>> children)
		{
			Value = value;
			Children = children;
		}
	}

	private static class Tree
	{
		public static Tree<T> New<T>(T value, params Tree<T>[] children) =>
			new Tree<T>(value, children);
	}

	[Fact]
	public Task TraverseBreadthFirstTraversesBreadthFirst()
	{
		var tree = Tree.New(1,
			Tree.New(2,
				Tree.New(3)),
			Tree.New(5,
				Tree.New(6))
		);
		var res = AsyncSuperEnumerable.TraverseBreadthFirst(tree, t => t.Children.ToAsyncEnumerable())
			.Select(t => t.Value);
		return res.AssertSequenceEqual(1, 2, 5, 3, 6);
	}

	[Fact]
	public Task TraverseDepthFirstTraversesDepthFirst()
	{
		var tree = Tree.New(1,
			Tree.New(2,
				Tree.New(3)),
			Tree.New(5,
				Tree.New(6))
		);
		var res = AsyncSuperEnumerable.TraverseDepthFirst(tree, t => t.Children.ToAsyncEnumerable())
			.Select(t => t.Value);
		return res.AssertSequenceEqual(1, 2, 3, 5, 6);
	}
}
