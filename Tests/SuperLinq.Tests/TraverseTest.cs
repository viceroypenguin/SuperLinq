namespace SuperLinq.Tests;

public sealed class TraverseTest
{
	[Test]
	public void TraverseDepthFirstIsStreaming()
	{
		_ = SuperEnumerable.TraverseDepthFirst(new object(), o => new BreakingSequence<object>());
	}

	[Test]
	public void TraverseBreadthFirstIsStreaming()
	{
		_ = SuperEnumerable.TraverseBreadthFirst(new object(), o => new BreakingSequence<object>());
	}

	[Test]
	public void TraverseDepthFirstPreservesChildrenOrder()
	{
		using var root = Enumerable.Range(1, 10).AsTestingSequence();
		using var child = Enumerable.Empty<int>().AsTestingSequence(maxEnumerations: 10);
		var res = SuperEnumerable.TraverseDepthFirst(0, i => i == 0 ? root : child);
		res.AssertSequenceEqual(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
	}

	[Test]
	public void TraverseBreadthFirstPreservesChildrenOrder()
	{
		using var root = Enumerable.Range(1, 10).AsTestingSequence();
		using var child = Enumerable.Empty<int>().AsTestingSequence(maxEnumerations: 10);
		var res = SuperEnumerable.TraverseBreadthFirst(0, i => i == 0 ? root : child);
		res.AssertSequenceEqual(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
	}

	private sealed class Tree<T>(
		T value,
		IEnumerable<Tree<T>> children
	)
	{
		public T Value { get; } = value;
		public IEnumerable<Tree<T>> Children { get; } = children;
	}

	private static class Tree
	{
		public static Tree<T> New<T>(T value, params Tree<T>[] children) =>
			new(value, children);
	}

	[Test]
	public void TraverseBreadthFirstTraversesBreadthFirst()
	{
		var tree = Tree.New(1,
			Tree.New(2,
				Tree.New(3)),
			Tree.New(5,
				Tree.New(6))
		);
		var res = SuperEnumerable.TraverseBreadthFirst(tree, t => t.Children).Select(t => t.Value);
		res.AssertSequenceEqual(1, 2, 5, 3, 6);
	}

	[Test]
	public void TraverseDepthFirstTraversesDepthFirst()
	{
		var tree = Tree.New(1,
			Tree.New(2,
				Tree.New(3)),
			Tree.New(5,
				Tree.New(6))
		);
		var res = SuperEnumerable.TraverseDepthFirst(tree, t => t.Children).Select(t => t.Value);
		res.AssertSequenceEqual(1, 2, 3, 5, 6);
	}
}
