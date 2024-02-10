namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Ranks each item in the sequence in ascending order using a default comparer,
	/// with no gaps in the ranking values. The rank starts at one and keeps incrementing
	/// by one for each different item.
	/// </summary>
	/// <typeparam name="TSource">Type of item in the sequence</typeparam>
	/// <param name="source">The sequence whose items will be ranked</param>
	/// <returns>A sorted sequence of items and their rank.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	public static IAsyncEnumerable<(TSource item, int rank)> DenseRank<TSource>(
		this IAsyncEnumerable<TSource> source)
	{
		return source.DenseRankBy(Identity);
	}

	/// <summary>
	/// Ranks each item in the sequence in the order defined by <paramref name="sortDirection"/>
	/// using a default comparer, with no gaps in the ranking values. The rank starts at one
	/// and keeps incrementing by one for each different item.
	/// </summary>
	/// <typeparam name="TSource">Type of item in the sequence</typeparam>
	/// <param name="source">The sequence whose items will be ranked</param>
	/// <param name="sortDirection">Defines the ordering direction for the sequence</param>
	/// <returns>A sorted sequence of items and their rank.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	public static IAsyncEnumerable<(TSource item, int rank)> DenseRank<TSource>(
		this IAsyncEnumerable<TSource> source, OrderByDirection sortDirection)
	{
		return source.DenseRankBy(Identity, sortDirection);
	}

	/// <summary>
	/// Ranks each item in the sequence in ascending order using a caller-supplied comparer,
	/// with no gaps in the ranking values. The rank starts at one and keeps incrementing
	/// by one for each different item.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements in the source sequence</typeparam>
	/// <param name="source">The sequence of items to rank</param>
	/// <param name="comparer">A object that defines comparison semantics for the elements in the sequence</param>
	/// <returns>A sorted sequence of items and their rank.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	public static IAsyncEnumerable<(TSource item, int rank)> DenseRank<TSource>(
		this IAsyncEnumerable<TSource> source, IComparer<TSource> comparer)
	{
		return source.DenseRankBy(Identity, comparer);
	}

	/// <summary>
	/// Ranks each item in the sequence in the order defined by <paramref name="sortDirection"/>
	/// using a caller-supplied comparer, with no gaps in the ranking values. The rank starts at one
	/// and keeps incrementing by one for each different item.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements in the source sequence</typeparam>
	/// <param name="source">The sequence of items to rank</param>
	/// <param name="comparer">A object that defines comparison semantics for the elements in the sequence</param>
	/// <param name="sortDirection">Defines the ordering direction for the sequence</param>
	/// <returns>A sorted sequence of items and their rank.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	public static IAsyncEnumerable<(TSource item, int rank)> DenseRank<TSource>(
		this IAsyncEnumerable<TSource> source,
		IComparer<TSource> comparer,
		OrderByDirection sortDirection)
	{
		return source.DenseRankBy(Identity, comparer, sortDirection);
	}
}
