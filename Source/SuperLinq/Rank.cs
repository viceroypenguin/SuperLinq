namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Ranks each item in the sequence in ascending order using a default comparer. The rank is equal to
	///		index + 1 of the first next different item, the first item has a rank of 1 (index 0 + 1).
	/// </summary>
	/// <typeparam name="TSource">
	///	    Type of item in the sequence
	/// </typeparam>
	/// <param name="source">
	///	    The sequence whose items will be ranked
	/// </param>
	/// <returns>
	///	    A sorted sequence of items and their rank.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This method is implemented by using deferred execution. However, <paramref name="source"/> will be consumed
	///     in it's entirety immediately when first element of the returned sequence is consumed.
	/// </para>
	/// </remarks>
	public static IEnumerable<(TSource item, int rank)> Rank<TSource>(
		this IEnumerable<TSource> source)
	{
		return source.RankBy(Identity);
	}

	/// <summary>
	///	    Ranks each item in the sequence in ascending order using a caller-supplied comparer. The rank is equal to
	///		index + 1 of the first next different item, the first item has a rank of 1 (index 0 + 1).
	/// </summary>
	/// <typeparam name="TSource">
	///	    The type of the elements in the source sequence
	/// </typeparam>
	/// <param name="source">
	///	    The sequence of items to rank
	/// </param>
	/// <param name="comparer">
	///	    A object that defines comparison semantics for the elements in the sequence
	/// </param>
	/// <returns>
	///	    A sorted sequence of items and their rank.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This method is implemented by using deferred execution. However, <paramref name="source"/> will be consumed
	///     in it's entirety immediately when first element of the returned sequence is consumed.
	/// </para>
	/// </remarks>
	public static IEnumerable<(TSource item, int rank)> Rank<TSource>(
		this IEnumerable<TSource> source, IComparer<TSource> comparer)
	{
		return source.RankBy(Identity, comparer);
	}

	///  <summary>
	/// 	Ranks each item in the sequence in the order defined by <paramref name="sortDirection"/> using a default
	///		comparer. The rank is equal to index + 1 of the first next different item, the first item has a rank of 1
	///		(index 0 + 1).
	///  </summary>
	///  <typeparam name="TSource">
	/// 	Type of item in the sequence
	///  </typeparam>
	///  <param name="source">
	/// 	The sequence whose items will be ranked
	///  </param>
	///  <param name="sortDirection">
	///		Defines the ordering direction for the sequence
	///	 </param>
	///  <returns>
	/// 	A sorted sequence of items and their rank.
	///  </returns>
	///  <exception cref="ArgumentNullException">
	/// 	<paramref name="source"/> is <see langword="null"/>.
	///  </exception>
	///  <remarks>
	///  <para>
	/// 	This method is implemented by using deferred execution. However, <paramref name="source"/> will be consumed
	///     in it's entirety immediately when first element of the returned sequence is consumed.
	///  </para>
	///  </remarks>
	public static IEnumerable<(TSource item, int rank)> Rank<TSource>(
		this IEnumerable<TSource> source, OrderByDirection sortDirection)
	{
		return source.RankBy(Identity, sortDirection);
	}

	///  <summary>
	/// 	Ranks each item in the sequence in the order defined by <paramref name="sortDirection"/> using a
	///		caller-supplied comparer. The rank is equal to index + 1 of the first next different item, the first item
	///		has a rank of 1 (index 0 + 1).
	///  </summary>
	///  <typeparam name="TSource">
	/// 	The type of the elements in the source sequence
	///  </typeparam>
	///  <param name="source">
	/// 	The sequence of items to rank
	///  </param>
	///  <param name="comparer">
	/// 	A object that defines comparison semantics for the elements in the sequence
	///  </param>
	///  <param name="sortDirection">
	///		Defines the ordering direction for the sequence
	///  </param>
	///  <returns>
	/// 	A sorted sequence of items and their rank.
	///  </returns>
	///  <exception cref="ArgumentNullException">
	/// 	<paramref name="source"/> is <see langword="null"/>.
	///  </exception>
	///  <remarks>
	///  <para>
	/// 	This method is implemented by using deferred execution. However, <paramref name="source"/> will be consumed
	///     in it's entirety immediately when first element of the returned sequence is consumed.
	///  </para>
	///  </remarks>
	public static IEnumerable<(TSource item, int rank)> Rank<TSource>(
		this IEnumerable<TSource> source,
		IComparer<TSource> comparer,
		OrderByDirection sortDirection)
	{
		return source.RankBy(Identity, comparer, sortDirection);
	}

	private static IEnumerable<(TSource, int)> RankCore<TSource, TKey>(
		IEnumerable<TSource> source,
		Func<TSource, TKey> keySelector,
		IComparer<TKey>? comparer,
		bool isDense,
		OrderByDirection sortDirection = OrderByDirection.Ascending)
	{
		comparer ??= Comparer<TKey>.Default;

		var rank = 0;
		var count = 1;
		foreach (var (cur, lag) in source
			.OrderBy(keySelector, comparer, sortDirection)
			.Lag(1))
		{
			if (rank == 0
				|| comparer.Compare(
					keySelector(cur),
					keySelector(Debug.AssertNotNull(lag))) != 0)
			{
				rank += isDense ? 1 : count;
				count = 0;
			}

			count++;

			yield return (cur, rank);
		}
	}
}
