namespace SuperLinq.Async;

public partial class AsyncSuperEnumerable
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
	/// <exception cref="ArgumentNullException"><paramref name="childrenSelector"/> is <see langword="null"/>.</exception>
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
	public static IAsyncEnumerable<T> TraverseBreadthFirst<T>(T root, Func<T, IAsyncEnumerable<T>> childrenSelector)
	{
		Guard.IsNotNull(childrenSelector);

		return Core(root, childrenSelector);

		static async IAsyncEnumerable<T> Core(
			T root,
			Func<T, IAsyncEnumerable<T>> childrenSelector,
			[EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			var queue = new Queue<T>();
			queue.Enqueue(root);

			while (queue.Count != 0)
			{
				cancellationToken.ThrowIfCancellationRequested();

				var current = queue.Dequeue();
				yield return current;
				await foreach (var child in childrenSelector(current).WithCancellation(cancellationToken).ConfigureAwait(false))
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
	/// <exception cref="ArgumentNullException"><paramref name="childrenSelector"/> is <see langword="null"/>.</exception>
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
	public static IAsyncEnumerable<T> TraverseDepthFirst<T>(T root, Func<T, IAsyncEnumerable<T>> childrenSelector)
	{
		Guard.IsNotNull(childrenSelector);

		return Core(root, childrenSelector);

		static async IAsyncEnumerable<T> Core(
			T root,
			Func<T, IAsyncEnumerable<T>> childrenSelector,
			[EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			var stack = new Stack<T>();
			stack.Push(root);

			while (stack.Count != 0)
			{
				cancellationToken.ThrowIfCancellationRequested();

				var current = stack.Pop();
				yield return current;
				// because a stack pops the elements out in LIFO order, we need to push them in reverse
				// if we want to traverse the returned list in the same order as was returned to us
				await foreach (var child in childrenSelector(current)
						.Reverse()
						.WithCancellation(cancellationToken)
						.ConfigureAwait(false))
					stack.Push(child);
			}
		}
	}
}
