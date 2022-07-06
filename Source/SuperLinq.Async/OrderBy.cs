namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Sorts the elements of a sequence in a particular direction (ascending, descending) according to a key
	/// </summary>
	/// <typeparam name="T">The type of the elements in the source sequence</typeparam>
	/// <typeparam name="TKey">The type of the key used to order elements</typeparam>
	/// <param name="source">The sequence to order</param>
	/// <param name="keySelector">A key selector function</param>
	/// <param name="direction">A direction in which to order the elements (ascending, descending)</param>
	/// <returns>An ordered copy of the source sequence</returns>

	public static IOrderedAsyncEnumerable<T> OrderBy<T, TKey>(this IAsyncEnumerable<T> source, Func<T, TKey> keySelector, OrderByDirection direction)
	{
		source.ThrowIfNull();
		keySelector.ThrowIfNull();
		return direction == OrderByDirection.Ascending
			? source.OrderBy(keySelector)
			: source.OrderByDescending(keySelector);
	}

	/// <summary>
	/// Sorts the elements of a sequence in a particular direction (ascending, descending) according to a key
	/// </summary>
	/// <typeparam name="T">The type of the elements in the source sequence</typeparam>
	/// <typeparam name="TKey">The type of the key used to order elements</typeparam>
	/// <param name="source">The sequence to order</param>
	/// <param name="keySelector">A key selector function</param>
	/// <param name="direction">A direction in which to order the elements (ascending, descending)</param>
	/// <param name="comparer">A comparer used to define the semantics of element comparison</param>
	/// <returns>An ordered copy of the source sequence</returns>

	public static IOrderedAsyncEnumerable<T> OrderBy<T, TKey>(this IAsyncEnumerable<T> source, Func<T, TKey> keySelector, IComparer<TKey> comparer, OrderByDirection direction)
	{
		source.ThrowIfNull();
		keySelector.ThrowIfNull();
		return direction == OrderByDirection.Ascending
			? source.OrderBy(keySelector, comparer)
			: source.OrderByDescending(keySelector, comparer);
	}

	/// <summary>
	/// Performs a subsequent ordering of elements in a sequence in a particular direction (ascending, descending) according to a key
	/// </summary>
	/// <typeparam name="T">The type of the elements in the source sequence</typeparam>
	/// <typeparam name="TKey">The type of the key used to order elements</typeparam>
	/// <param name="source">The sequence to order</param>
	/// <param name="keySelector">A key selector function</param>
	/// <param name="direction">A direction in which to order the elements (ascending, descending)</param>
	/// <returns>An ordered copy of the source sequence</returns>

	public static IOrderedAsyncEnumerable<T> ThenBy<T, TKey>(this IOrderedAsyncEnumerable<T> source, Func<T, TKey> keySelector, OrderByDirection direction)
	{
		source.ThrowIfNull();
		keySelector.ThrowIfNull();
		return direction == OrderByDirection.Ascending
			? source.ThenBy(keySelector)
			: source.ThenByDescending(keySelector);
	}

	/// <summary>
	/// Performs a subsequent ordering of elements in a sequence in a particular direction (ascending, descending) according to a key
	/// </summary>
	/// <typeparam name="T">The type of the elements in the source sequence</typeparam>
	/// <typeparam name="TKey">The type of the key used to order elements</typeparam>
	/// <param name="source">The sequence to order</param>
	/// <param name="keySelector">A key selector function</param>
	/// <param name="direction">A direction in which to order the elements (ascending, descending)</param>
	/// <param name="comparer">A comparer used to define the semantics of element comparison</param>
	/// <returns>An ordered copy of the source sequence</returns>

	public static IOrderedAsyncEnumerable<T> ThenBy<T, TKey>(this IOrderedAsyncEnumerable<T> source, Func<T, TKey> keySelector, IComparer<TKey> comparer, OrderByDirection direction)
	{
		source.ThrowIfNull();
		keySelector.ThrowIfNull();
		return direction == OrderByDirection.Ascending
			? source.ThenBy(keySelector, comparer)
			: source.ThenByDescending(keySelector, comparer);
	}
}
