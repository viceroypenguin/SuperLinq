namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Ranks each item in the sequence in ascending order using a default comparer.
	/// The rank is equal to index + 1 of the first element of the item's equality set.
	/// </summary>
	/// <typeparam name="TSource">Type of item in the sequence</typeparam>
	/// <param name="source">The sequence whose items will be ranked</param>
	/// <returns>A sorted sequence of items and their rank.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	public static IAsyncEnumerable<(TSource item, int rank)> Rank<TSource>(
		this IAsyncEnumerable<TSource> source)
	{
		return source.RankBy(Identity);
	}

	/// <summary>
	/// Ranks each item in the sequence in ascending order using a caller-supplied comparer.
	/// The rank is equal to index + 1 of the first element of the item's equality set.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements in the source sequence</typeparam>
	/// <param name="source">The sequence of items to rank</param>
	/// <param name="comparer">A object that defines comparison semantics for the elements in the sequence</param>
	/// <returns>A sorted sequence of items and their rank.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	public static IAsyncEnumerable<(TSource item, int rank)> Rank<TSource>(
		this IAsyncEnumerable<TSource> source, IComparer<TSource> comparer)
	{
		return source.RankBy(Identity, comparer);
	}

	/// <summary>
	/// Ranks each item in the sequence in the order defined by <paramref name="sortDirection"/>
	/// using a default comparer. The rank is equal to index + 1 of the first element of the item's equality set.
	/// </summary>
	/// <typeparam name="TSource">Type of item in the sequence</typeparam>
	/// <param name="source">The sequence whose items will be ranked</param>
	/// <param name="sortDirection">Defines the ordering direction for the sequence</param>
	/// <returns>A sorted sequence of items and their rank.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	public static IAsyncEnumerable<(TSource item, int rank)> Rank<TSource>(
		this IAsyncEnumerable<TSource> source, OrderByDirection sortDirection)
	{
		return source.RankBy(Identity, sortDirection);
	}

	/// <summary>
	/// Ranks each item in the sequence in the order defined by <paramref name="sortDirection"/>
	/// using a caller-supplied comparer. The rank is equal to index + 1 of the first element of the item's equality set.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements in the source sequence</typeparam>
	/// <param name="source">The sequence of items to rank</param>
	/// <param name="comparer">A object that defines comparison semantics for the elements in the sequence</param>
	/// <param name="sortDirection">Defines the ordering direction for the sequence</param>
	/// <returns>A sorted sequence of items and their rank.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	public static IAsyncEnumerable<(TSource item, int rank)> Rank<TSource>(
		this IAsyncEnumerable<TSource> source,
		IComparer<TSource> comparer,
		OrderByDirection sortDirection)
	{
		return source.RankBy(Identity, comparer, sortDirection);
	}

	private static async IAsyncEnumerable<(TSource, int)> RankCore<TSource, TKey>(
		IAsyncEnumerable<TSource> source,
		Func<TSource, TKey> keySelector,
		IComparer<TKey>? comparer,
		bool isDense,
		OrderByDirection sortDirection = OrderByDirection.Ascending,
		[EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		comparer ??= Comparer<TKey>.Default;

		var rank = 0;
		var count = 1;
		await foreach (var (cur, lag) in source
			.OrderBy(keySelector, comparer, sortDirection)
			.Lag(1)
			.WithCancellation(cancellationToken)
			.ConfigureAwait(false))
		{
#pragma warning disable CA1508 // Avoid dead conditional code
			if (rank == 0
				|| comparer.Compare(
					keySelector(cur),
					keySelector(Debug.AssertNotNull(lag))) != 0)
			{
				rank += isDense ? 1 : count;
				count = 0;
			}
#pragma warning restore CA1508 // Avoid dead conditional code

			count++;

			yield return (cur, rank);
		}
	}
}
