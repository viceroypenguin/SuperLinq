namespace Test;

public class TraverseTest
{
	[Fact]
	public void TraverseDepthFirstIsStreaming()
	{
		SuperEnumerable.TraverseDepthFirst(new object(), o => new BreakingSequence<object>());
	}

	[Fact]
	public void TraverseBreadthFirstIsStreaming()
	{
		SuperEnumerable.TraverseBreadthFirst(new object(), o => new BreakingSequence<object>());
	}

	[Fact]
	public void TraverseDepthFirstPreservesChildrenOrder()
	{
		var res = SuperEnumerable.TraverseDepthFirst(0, i => i == 0 ? Enumerable.Range(1, 10) : Enumerable.Empty<int>());
		res.AssertSequenceEqual(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
	}


	[Fact]
	public void TraverseBreadthFirstPreservesChildrenOrder()
	{
		var res = SuperEnumerable.TraverseBreadthFirst(0, i => i == 0 ? Enumerable.Range(1, 10) : Enumerable.Empty<int>());
		res.AssertSequenceEqual(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
	}

	class Tree<T>
	{
		public T Value { get; }
		public IEnumerable<Tree<T>> Children { get; }

		public Tree(T value, IEnumerable<Tree<T>> children)
		{
			Value = value;
			Children = children;
		}
	}

	static class Tree
	{
		public static Tree<T> New<T>(T value, params Tree<T>[] children) =>
			new Tree<T>(value, children);
	}

	[Fact]
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

	[Fact]
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
