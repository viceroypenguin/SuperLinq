namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Merges two or more sequences that are in a common order into a single sequence that preserves that order.
	/// </summary>
	/// <typeparam name="TSource">
	///	    The type of the elements of the sequence
	/// </typeparam>
	/// <param name="source">
	///	    The primary sequence with which to merge
	/// </param>
	/// <param name="otherSequences">
	///	    A variable argument array of zero or more other sequences to merge with
	/// </param>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="otherSequences"/> is <see langword="null"/>.
	/// </exception>
	/// <returns>
	///	    A merged, order-preserving sequence containing all of the elements of the original sequences
	/// </returns>
	/// <remarks>
	/// <para>
	///	    Using SortedMerge on sequences that are not ordered or are not in the same order produces undefined results.
	/// </para>
	/// <para>
	///	    This method uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IEnumerable<TSource> SortedMerge<TSource>(this IEnumerable<TSource> source, params IEnumerable<TSource>[] otherSequences)
	{
		return SortedMerge(source, OrderByDirection.Ascending, comparer: null, otherSequences);
	}

	/// <summary>
	///	    Merges two or more sequences that are in a common order into a single sequence that preserves that order.
	/// </summary>
	/// <typeparam name="TSource">
	///	    The type of the elements of the sequence
	/// </typeparam>
	/// <param name="source">
	///	    The primary sequence with which to merge
	/// </param>
	/// <param name="comparer">
	///		An <see cref="IComparer{T}"/> to compare elements
	/// </param>
	/// <param name="otherSequences">
	///	    A variable argument array of zero or more other sequences to merge with
	/// </param>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="otherSequences"/> is <see langword="null"/>.
	/// </exception>
	/// <returns>
	///	    A merged, order-preserving sequence containing all of the elements of the original sequences
	/// </returns>
	/// <remarks>
	/// <para>
	///	    Using SortedMerge on sequences that are not ordered or are not in the same order produces undefined results.
	/// </para>
	/// <para>
	///	    This method uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IEnumerable<TSource> SortedMerge<TSource>(
		this IEnumerable<TSource> source,
		IComparer<TSource>? comparer,
		params IEnumerable<TSource>[] otherSequences
	)
	{
		return SortedMerge(source, OrderByDirection.Ascending, comparer, otherSequences);
	}

	/// <summary>
	///	    Merges two or more sequences that are in a common order (either ascending or descending) into a single sequence that preserves that order.
	/// </summary>
	/// <typeparam name="TSource">
	///	    The type of the elements of the sequence
	/// </typeparam>
	/// <param name="source">
	///	    The primary sequence with which to merge
	/// </param>
	/// <param name="direction">
	///	    A direction in which to order the elements (ascending, descending)
	/// </param>
	/// <param name="otherSequences">
	///	    A variable argument array of zero or more other sequences to merge with
	/// </param>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="otherSequences"/> is <see langword="null"/>.
	/// </exception>
	/// <returns>
	///	    A merged, order-preserving sequence containing all of the elements of the original sequences
	/// </returns>
	/// <remarks>
	/// <para>
	///	    Using <c>SortedMerge</c> on sequences that are not ordered or are not in the same order produces undefined results.
	/// </para>
	/// <para>
	///	    This method uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IEnumerable<TSource> SortedMerge<TSource>(
		this IEnumerable<TSource> source,
		OrderByDirection direction,
		params IEnumerable<TSource>[] otherSequences
	)
	{
		return SortedMerge(source, direction, comparer: null, otherSequences);
	}

	/// <summary>
	///	    Merges two or more sequences that are in a common order (either ascending or descending) into a single sequence that preserves that order.
	/// </summary>
	/// <typeparam name="TSource">
	///	    The type of the elements of the sequence
	/// </typeparam>
	/// <param name="source">
	///	    The primary sequence with which to merge
	/// </param>
	/// <param name="direction">
	///	    A direction in which to order the elements (ascending, descending)
	/// </param>
	/// <param name="comparer">
	///		An <see cref="IComparer{T}"/> to compare elements
	/// </param>
	/// <param name="otherSequences">
	///	    A variable argument array of zero or more other sequences to merge with
	/// </param>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="otherSequences"/> is <see langword="null"/>.
	/// </exception>
	/// <returns>
	///	    A merged, order-preserving sequence containing all of the elements of the original sequences
	/// </returns>
	/// <remarks>
	/// <para>
	///	    Using <c>SortedMerge</c> on sequences that are not ordered or are not in the same order produces undefined results.
	/// </para>
	/// <para>
	///	    This method uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IEnumerable<TSource> SortedMerge<TSource>(
		this IEnumerable<TSource> source,
		OrderByDirection direction,
		IComparer<TSource>? comparer,
		params IEnumerable<TSource>[] otherSequences
	)
	{
		return SortedMergeBy(source, Identity, direction, comparer, otherSequences);
	}

	/// <summary>
	///	    Merges two or more sequences that are in a common order according to a key into a single sequence that
	///     preserves that order.
	/// </summary>
	/// <typeparam name="TSource">
	///	    The type of the elements of the sequence
	/// </typeparam>
	/// <typeparam name="TKey">
	///	    The type of the key used to order elements
	/// </typeparam>
	/// <param name="source">
	///	    The primary sequence with which to merge
	/// </param>
	/// <param name="keySelector">
	///	    A key selector function
	/// </param>
	/// <param name="otherSequences">
	///	    A variable argument array of zero or more other sequences to merge with
	/// </param>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/>, <paramref name="keySelector"/> or <paramref name="otherSequences"/> is <see
	///     langword="null"/>.
	/// </exception>
	/// <returns>
	///	    A merged, order-preserving sequence containing all of the elements of the original sequences
	/// </returns>
	/// <remarks>
	/// <para>
	///	    Using <c>SortedMergeBy</c> on sequences that are not ordered or are not in the same order produces undefined
	///     results.
	/// </para>
	/// <para>
	///	    This method uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IEnumerable<TSource> SortedMergeBy<TSource, TKey>(
		this IEnumerable<TSource> source,
		Func<TSource, TKey> keySelector,
		params IEnumerable<TSource>[] otherSequences
	)
	{
		return SortedMergeBy(source, keySelector, OrderByDirection.Ascending, comparer: null, otherSequences);
	}

	/// <summary>
	///	    Merges two or more sequences that are in a common order according to a key into a single sequence that
	///     preserves that order.
	/// </summary>
	/// <typeparam name="TSource">
	///	    The type of the elements of the sequence
	/// </typeparam>
	/// <typeparam name="TKey">
	///	    The type of the key used to order elements
	/// </typeparam>
	/// <param name="source">
	///	    The primary sequence with which to merge
	/// </param>
	/// <param name="keySelector">
	///	    A key selector function
	/// </param>
	/// <param name="comparer">
	///		An <see cref="IComparer{T}"/> to compare keys
	/// </param>
	/// <param name="otherSequences">
	///	    A variable argument array of zero or more other sequences to merge with
	/// </param>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/>, <paramref name="keySelector"/> or <paramref name="otherSequences"/> is <see
	///     langword="null"/>.
	/// </exception>
	/// <returns>
	///	    A merged, order-preserving sequence containing all of the elements of the original sequences
	/// </returns>
	/// <remarks>
	/// <para>
	///	    Using <c>SortedMergeBy</c> on sequences that are not ordered or are not in the same order produces undefined
	///     results.
	/// </para>
	/// <para>
	///	    This method uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IEnumerable<TSource> SortedMergeBy<TSource, TKey>(
		this IEnumerable<TSource> source,
		Func<TSource, TKey> keySelector,
		IComparer<TKey>? comparer,
		params IEnumerable<TSource>[] otherSequences
	)
	{
		return SortedMergeBy(source, keySelector, OrderByDirection.Ascending, comparer, otherSequences);
	}

	/// <summary>
	///	    Merges two or more sequences that are in a common order (either ascending or descending) according to a key
	///     into a single sequence that preserves that order.
	/// </summary>
	/// <typeparam name="TSource">
	///	    The type of the elements of the sequence
	/// </typeparam>
	/// <typeparam name="TKey">
	///	    The type of the key used to order elements
	/// </typeparam>
	/// <param name="source">
	///	    The primary sequence with which to merge
	/// </param>
	/// <param name="keySelector">
	///	    A key selector function
	/// </param>
	/// <param name="direction">
	///	    A direction in which to order the elements (ascending, descending)
	/// </param>
	/// <param name="otherSequences">
	///	    A variable argument array of zero or more other sequences to merge with
	/// </param>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/>, <paramref name="keySelector"/> or <paramref name="otherSequences"/> is <see
	///     langword="null"/>.
	/// </exception>
	/// <returns>
	///	    A merged, order-preserving sequence containing all of the elements of the original sequences
	/// </returns>
	/// <remarks>
	/// <para>
	///	    Using <c>SortedMergeBy</c> on sequences that are not ordered or are not in the same order produces undefined
	///     results.
	/// </para>
	/// <para>
	///	    This method uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IEnumerable<TSource> SortedMergeBy<TSource, TKey>(
		this IEnumerable<TSource> source,
		Func<TSource, TKey> keySelector,
		OrderByDirection direction,
		params IEnumerable<TSource>[] otherSequences)
	{
		return SortedMergeBy(source, keySelector, direction, comparer: null, otherSequences);
	}

	/// <summary>
	///	    Merges two or more sequences that are in a common order (either ascending or descending) according to a key
	///     into a single sequence that preserves that order.
	/// </summary>
	/// <typeparam name="TSource">
	///	    The type of the elements of the sequence
	/// </typeparam>
	/// <typeparam name="TKey">
	///	    The type of the key used to order elements
	/// </typeparam>
	/// <param name="source">
	///	    The primary sequence with which to merge
	/// </param>
	/// <param name="keySelector">
	///	    A key selector function
	/// </param>
	/// <param name="direction">
	///	    A direction in which to order the elements (ascending, descending)
	/// </param>
	/// <param name="comparer">
	///		An <see cref="IComparer{T}"/> to compare keys
	/// </param>
	/// <param name="otherSequences">
	///	    A variable argument array of zero or more other sequences to merge with
	/// </param>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/>, <paramref name="keySelector"/> or <paramref name="otherSequences"/> is <see
	///     langword="null"/>.
	/// </exception>
	/// <returns>
	///	    A merged, order-preserving sequence containing all of the elements of the original sequences
	/// </returns>
	/// <remarks>
	/// <para>
	///	    Using <c>SortedMergeBy</c> on sequences that are not ordered or are not in the same order produces undefined
	///     results.
	/// </para>
	/// <para>
	///	    This method uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IEnumerable<TSource> SortedMergeBy<TSource, TKey>(
		this IEnumerable<TSource> source,
		Func<TSource, TKey> keySelector,
		OrderByDirection direction,
		IComparer<TKey>? comparer,
		params IEnumerable<TSource>[] otherSequences)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(keySelector);
		ArgumentNullException.ThrowIfNull(otherSequences);

		if (otherSequences.Length == 0)
			return source; // optimization for when otherSequences is empty

		comparer ??= Comparer<TKey>.Default;
		if (direction == OrderByDirection.Descending)
			comparer = new ReverseComparer<TKey>(comparer);

		// return the sorted merge result
		return Impl(otherSequences.Prepend(source), keySelector, comparer);

		// Private implementation method that performs a merge of multiple, ordered sequences using
		// a precedence function which encodes order-sensitive comparison logic based on the caller's arguments.
		//
		// The algorithm employed in this implementation is not necessarily the most optimal way to merge
		// two sequences. A swap-compare version would probably be somewhat more efficient - but at the
		// expense of considerably more complexity. One possible optimization would be to detect that only
		// a single sequence remains (all other being consumed) and break out of the main while-loop and
		// simply yield the items that are part of the final sequence.
		//
		// The algorithm used here will perform N*(K1+K2+...Kn-1) comparisons, where <c>N => otherSequences.Count()+1.</c>

		static IEnumerable<TSource> Impl(IEnumerable<IEnumerable<TSource>> sequences, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
		{
			using var list = new EnumeratorList<TSource>(sequences);

			// prime all of the iterators by advancing them to their first element (if any)
			for (var i = 0; list.MoveNext(i); i++)
			{ }

			// while all iterators have not yet been consumed...
			while (list.Any())
			{
				var nextIndex = 0;
				var nextValue = list.Current(0);
				var nextKey = keySelector(nextValue);

				// find the next least element to return
				for (var i = 1; i < list.Count; i++)
				{
					var anotherElement = list.Current(i);
					var anotherKey = keySelector(anotherElement);
					// determine which element follows based on ordering function
					if (comparer.Compare(nextKey, anotherKey) > 0)
					{
						nextIndex = i;
						nextValue = anotherElement;
						nextKey = anotherKey;
					}
				}

				yield return nextValue; // next value in precedence order

				// advance iterator that yielded element, excluding it when consumed
				_ = list.MoveNextOnce(nextIndex);
			}
		}
	}
}
