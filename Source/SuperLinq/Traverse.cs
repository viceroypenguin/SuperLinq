namespace SuperLinq;

public partial class SuperEnumerable
{
	/// <summary>
	/// Traverses a tree in a breadth-first fashion, starting at a root
	/// node and using a user-defined function to get the children at each
	/// node of the tree.
	/// </summary>
	/// <typeparam name="T">The tree node type</typeparam>
	/// <param name="root">The root of the tree to traverse.</param>
	/// <param name="childrenSelector">
	/// The function that produces the children of each element.</param>
	/// <returns>A sequence containing the traversed values.</returns>
	/// <remarks>
	/// <para>
	/// The tree is not checked for loops. If the resulting sequence needs
	/// to be finite then it is the responsibility of
	/// <paramref name="childrenSelector"/> to ensure that loops are not
	/// produced.</para>
	/// <para>
	/// This function defers traversal until needed and streams the
	/// results.</para>
	/// </remarks>

	public static IEnumerable<T> TraverseBreadthFirst<T>(T root, Func<T, IEnumerable<T>> childrenSelector)
	{
		if (childrenSelector == null) throw new ArgumentNullException(nameof(childrenSelector));

		return _(); IEnumerable<T> _()
		{
			var queue = new Queue<T>();
			queue.Enqueue(root);

			while (queue.Count != 0)
			{
				var current = queue.Dequeue();
				yield return current;
				foreach (var child in childrenSelector(current))
					queue.Enqueue(child);
			}
		}
	}

	/// <summary>
	/// Traverses a tree in a depth-first fashion, starting at a root node
	/// and using a user-defined function to get the children at each node
	/// of the tree.
	/// </summary>
	/// <typeparam name="T">The tree node type.</typeparam>
	/// <param name="root">The root of the tree to traverse.</param>
	/// <param name="childrenSelector">
	/// The function that produces the children of each element.</param>
	/// <returns>A sequence containing the traversed values.</returns>
	/// <remarks>
	/// <para>
	/// The tree is not checked for loops. If the resulting sequence needs
	/// to be finite then it is the responsibility of
	/// <paramref name="childrenSelector"/> to ensure that loops are not
	/// produced.</para>
	/// <para>
	/// This function defers traversal until needed and streams the
	/// results.</para>
	/// </remarks>

	public static IEnumerable<T> TraverseDepthFirst<T>(T root, Func<T, IEnumerable<T>> childrenSelector)
	{
		if (childrenSelector == null) throw new ArgumentNullException(nameof(childrenSelector));

		return _(); IEnumerable<T> _()
		{
			var stack = new Stack<T>();
			stack.Push(root);

			while (stack.Count != 0)
			{
				var current = stack.Pop();
				yield return current;
				// because a stack pops the elements out in LIFO order, we need to push them in reverse
				// if we want to traverse the returned list in the same order as was returned to us
				foreach (var child in childrenSelector(current).Reverse())
					stack.Push(child);
			}
		}
	}
}
