namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Merges two or more sequences that are in a common order (either ascending or descending) into
	/// a single sequence that preserves that order.
	/// </summary>
	/// <remarks>
	/// Using SortedMerge on sequences that are not ordered or are not in the same order produces
	/// undefined results.<br/>
	/// This method uses deferred execution and streams its results.<br />
	///
	/// Here is an example of a merge, as well as the produced result:
	/// <code><![CDATA[
	///   var s1 = new[] { 3, 7, 11 };
	///   var s2 = new[] { 2, 4, 20 };
	///   var s3 = new[] { 17, 19, 25 };
	///   var merged = s1.SortedMerge( OrderByDirection.Ascending, s2, s3 );
	///   var result = merged.ToArray();
	///   // result will be:
	///   // { 2, 3, 4, 7, 11, 17, 19, 20, 25 }
	/// ]]></code>
	/// </remarks>
	/// <typeparam name="TSource">The type of the elements of the sequence</typeparam>
	/// <param name="source">The primary sequence with which to merge</param>
	/// <param name="otherSequences">A variable argument array of zero or more other sequences to merge with</param>
	/// <returns>A merged, order-preserving sequence containing all of the elements of the original sequences</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="otherSequences"/> is <see langword="null"/>.</exception>
	public static IEnumerable<TSource> SortedMerge<TSource>(this IEnumerable<TSource> source, params IEnumerable<TSource>[] otherSequences)
	{
		return SortedMerge(source, OrderByDirection.Ascending, comparer: null, otherSequences);
	}

	/// <summary>
	/// Merges two or more sequences that are in a common order (either ascending or descending) into
	/// a single sequence that preserves that order.
	/// </summary>
	/// <remarks>
	/// Using SortedMergeDescending on sequences that are not ordered or are not in the same order produces
	/// undefined results.<br/>
	/// This method uses deferred execution and streams its results.<br />
	///
	/// Here is an example of a merge, as well as the produced result:
	/// <code><![CDATA[
	///   var s1 = new[] { 3, 7, 11 };
	///   var s2 = new[] { 2, 4, 20 };
	///   var s3 = new[] { 17, 19, 25 };
	///   var merged = s1.SortedMerge( OrderByDirection.Ascending, s2, s3 );
	///   var result = merged.ToArray();
	///   // result will be:
	///   // { 2, 3, 4, 7, 11, 17, 19, 20, 25 }
	/// ]]></code>
	/// </remarks>
	/// <typeparam name="TSource">The type of the elements of the sequence</typeparam>
	/// <param name="source">The primary sequence with which to merge</param>
	/// <param name="otherSequences">A variable argument array of zero or more other sequences to merge with</param>
	/// <returns>A merged, order-preserving sequence containing all of the elements of the original sequences</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="otherSequences"/> is <see langword="null"/>.</exception>
	public static IEnumerable<TSource> SortedMergeDescending<TSource>(this IEnumerable<TSource> source, params IEnumerable<TSource>[] otherSequences)
	{
		return SortedMerge(source, OrderByDirection.Descending, comparer: null, otherSequences);
	}

	/// <summary>
	/// Merges two or more sequences that are in a common order (either ascending or descending) into
	/// a single sequence that preserves that order.
	/// </summary>
	/// <remarks>
	/// Using SortedMerge on sequences that are not ordered or are not in the same order produces
	/// undefined results.<br/>
	/// This method uses deferred execution and streams its results.<br />
	///
	/// Here is an example of a merge, as well as the produced result:
	/// <code><![CDATA[
	///   var s1 = new[] { 3, 7, 11 };
	///   var s2 = new[] { 2, 4, 20 };
	///   var s3 = new[] { 17, 19, 25 };
	///   var merged = s1.SortedMerge( OrderByDirection.Ascending, s2, s3 );
	///   var result = merged.ToArray();
	///   // result will be:
	///   // { 2, 3, 4, 7, 11, 17, 19, 20, 25 }
	/// ]]></code>
	/// </remarks>
	/// <typeparam name="TSource">The type of the elements of the sequence</typeparam>
	/// <param name="source">The primary sequence with which to merge</param>
	/// <param name="comparer">The comparer used to evaluate the relative order between elements</param>
	/// <param name="otherSequences">A variable argument array of zero or more other sequences to merge with</param>
	/// <returns>A merged, order-preserving sequence containing all of the elements of the original sequences</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="otherSequences"/> is <see langword="null"/>.</exception>
	public static IEnumerable<TSource> SortedMerge<TSource>(this IEnumerable<TSource> source, IComparer<TSource>? comparer, params IEnumerable<TSource>[] otherSequences)
	{
		return SortedMerge(source, OrderByDirection.Ascending, comparer, otherSequences);
	}

	/// <summary>
	/// Merges two or more sequences that are in a common order (either ascending or descending) into
	/// a single sequence that preserves that order.
	/// </summary>
	/// <remarks>
	/// Using SortedMergeDescending on sequences that are not ordered or are not in the same order produces
	/// undefined results.<br/>
	/// This method uses deferred execution and streams its results.<br />
	///
	/// Here is an example of a merge, as well as the produced result:
	/// <code><![CDATA[
	///   var s1 = new[] { 3, 7, 11 };
	///   var s2 = new[] { 2, 4, 20 };
	///   var s3 = new[] { 17, 19, 25 };
	///   var merged = s1.SortedMerge( OrderByDirection.Ascending, s2, s3 );
	///   var result = merged.ToArray();
	///   // result will be:
	///   // { 2, 3, 4, 7, 11, 17, 19, 20, 25 }
	/// ]]></code>
	/// </remarks>
	/// <typeparam name="TSource">The type of the elements of the sequence</typeparam>
	/// <param name="source">The primary sequence with which to merge</param>
	/// <param name="comparer">The comparer used to evaluate the relative order between elements</param>
	/// <param name="otherSequences">A variable argument array of zero or more other sequences to merge with</param>
	/// <returns>A merged, order-preserving sequence containing all of the elements of the original sequences</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="otherSequences"/> is <see langword="null"/>.</exception>
	public static IEnumerable<TSource> SortedMergeDescending<TSource>(this IEnumerable<TSource> source, IComparer<TSource>? comparer, params IEnumerable<TSource>[] otherSequences)
	{
		return SortedMerge(source, OrderByDirection.Descending, comparer, otherSequences);
	}

	/// <summary>
	/// Merges two or more sequences that are in a common order (either ascending or descending) into
	/// a single sequence that preserves that order.
	/// </summary>
	/// <remarks>
	/// Using SortedMerge on sequences that are not ordered or are not in the same order produces
	/// undefined results.<br/>
	/// This method uses deferred execution and streams its results.<br />
	///
	/// Here is an example of a merge, as well as the produced result:
	/// <code><![CDATA[
	///   var s1 = new[] { 3, 7, 11 };
	///   var s2 = new[] { 2, 4, 20 };
	///   var s3 = new[] { 17, 19, 25 };
	///   var merged = s1.SortedMerge( OrderByDirection.Ascending, s2, s3 );
	///   var result = merged.ToArray();
	///   // result will be:
	///   // { 2, 3, 4, 7, 11, 17, 19, 20, 25 }
	/// ]]></code>
	/// </remarks>
	/// <typeparam name="TSource">The type of the elements of the sequence</typeparam>
	/// <param name="source">The primary sequence with which to merge</param>
	/// <param name="direction">The ordering that all sequences must already exhibit</param>
	/// <param name="otherSequences">A variable argument array of zero or more other sequences to merge with</param>
	/// <returns>A merged, order-preserving sequence containing all of the elements of the original sequences</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="otherSequences"/> is <see langword="null"/>.</exception>
	public static IEnumerable<TSource> SortedMerge<TSource>(this IEnumerable<TSource> source, OrderByDirection direction, params IEnumerable<TSource>[] otherSequences)
	{
		return SortedMerge(source, direction, comparer: null, otherSequences);
	}

	/// <summary>
	/// Merges two or more sequences that are in a common order (either ascending or descending) into
	/// a single sequence that preserves that order.
	/// </summary>
	/// <remarks>
	/// Using SortedMerge on sequences that are not ordered or are not in the same order produces
	/// undefined results.<br/>
	/// This method uses deferred execution and streams its results.<br />
	///
	/// Here is an example of a merge, as well as the produced result:
	/// <code><![CDATA[
	///   var s1 = new[] { 3, 7, 11 };
	///   var s2 = new[] { 2, 4, 20 };
	///   var s3 = new[] { 17, 19, 25 };
	///   var merged = s1.SortedMerge( OrderByDirection.Ascending, s2, s3 );
	///   var result = merged.ToArray();
	///   // result will be:
	///   // { 2, 3, 4, 7, 11, 17, 19, 20, 25 }
	/// ]]></code>
	/// </remarks>
	/// <typeparam name="TSource">The type of the elements in the sequence</typeparam>
	/// <param name="source">The primary sequence with which to merge</param>
	/// <param name="direction">The ordering that all sequences must already exhibit</param>
	/// <param name="comparer">The comparer used to evaluate the relative order between elements</param>
	/// <param name="otherSequences">A variable argument array of zero or more other sequences to merge with</param>
	/// <returns>A merged, order-preserving sequence containing all of the elements of the original sequences</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="otherSequences"/> is <see langword="null"/>.</exception>
	public static IEnumerable<TSource> SortedMerge<TSource>(this IEnumerable<TSource> source, OrderByDirection direction, IComparer<TSource>? comparer, params IEnumerable<TSource>[] otherSequences)
	{
		return SortedMergeBy(source, Identity, direction, comparer, otherSequences);
	}

	/// <summary>
	/// Merges two or more sequences that are in a common order (either ascending or descending)
	/// according to a key into a single sequence that preserves that order.
	/// </summary>
	/// <remarks>
	/// Using SortedMergeBy on sequences that are not ordered or are not in the same order produces
	/// undefined results.<br/>
	/// This method uses deferred execution and streams its results.<br />
	/// </remarks>
	/// <typeparam name="TSource">The type of the elements of the sequence</typeparam>
	/// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector"/></typeparam>
	/// <param name="source">The primary sequence with which to merge</param>
	/// <param name="keySelector">A function to extract a key from an element.</param>
	/// <param name="otherSequences">A variable argument array of zero or more other sequences to merge with</param>
	/// <returns>A merged, order-preserving sequence containing all of the elements of the original sequences</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="keySelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="otherSequences"/> is <see langword="null"/>.</exception>
	public static IEnumerable<TSource> SortedMergeBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, params IEnumerable<TSource>[] otherSequences)
	{
		return SortedMergeBy(source, keySelector, OrderByDirection.Ascending, comparer: null, otherSequences);
	}

	/// <summary>
	/// Merges two or more sequences that are in a common order (either ascending or descending)
	/// according to a key into a single sequence that preserves that order.
	/// </summary>
	/// <remarks>
	/// Using SortedMergeByDescending on sequences that are not ordered or are not in the same order produces
	/// undefined results.<br/>
	/// This method uses deferred execution and streams its results.<br />
	/// </remarks>
	/// <typeparam name="TSource">The type of the elements of the sequence</typeparam>
	/// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector"/></typeparam>
	/// <param name="source">The primary sequence with which to merge</param>
	/// <param name="keySelector">A function to extract a key from an element.</param>
	/// <param name="otherSequences">A variable argument array of zero or more other sequences to merge with</param>
	/// <returns>A merged, order-preserving sequence containing all of the elements of the original sequences</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="keySelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="otherSequences"/> is <see langword="null"/>.</exception>
	public static IEnumerable<TSource> SortedMergeByDescending<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, params IEnumerable<TSource>[] otherSequences)
	{
		return SortedMergeBy(source, keySelector, OrderByDirection.Descending, comparer: null, otherSequences);
	}

	/// <summary>
	/// Merges two or more sequences that are in a common order (either ascending or descending)
	/// according to a key into a single sequence that preserves that order.
	/// </summary>
	/// <remarks>
	/// Using SortedMergeBy on sequences that are not ordered or are not in the same order produces
	/// undefined results.<br/>
	/// This method uses deferred execution and streams its results.<br />
	/// </remarks>
	/// <typeparam name="TSource">The type of the elements of the sequence</typeparam>
	/// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector"/></typeparam>
	/// <param name="source">The primary sequence with which to merge</param>
	/// <param name="keySelector">A function to extract a key from an element.</param>
	/// <param name="comparer">The comparer used to evaluate the relative order between elements</param>
	/// <param name="otherSequences">A variable argument array of zero or more other sequences to merge with</param>
	/// <returns>A merged, order-preserving sequence containing all of the elements of the original sequences</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="keySelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="otherSequences"/> is <see langword="null"/>.</exception>
	public static IEnumerable<TSource> SortedMergeBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey>? comparer, params IEnumerable<TSource>[] otherSequences)
	{
		return SortedMergeBy(source, keySelector, OrderByDirection.Ascending, comparer, otherSequences);
	}

	/// <summary>
	/// Merges two or more sequences that are in a common order (either ascending or descending)
	/// according to a key into a single sequence that preserves that order.
	/// </summary>
	/// <remarks>
	/// Using SortedMergeByDescending on sequences that are not ordered or are not in the same order produces
	/// undefined results.<br/>
	/// This method uses deferred execution and streams its results.<br />
	/// </remarks>
	/// <typeparam name="TSource">The type of the elements of the sequence</typeparam>
	/// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector"/></typeparam>
	/// <param name="source">The primary sequence with which to merge</param>
	/// <param name="keySelector">A function to extract a key from an element.</param>
	/// <param name="comparer">The comparer used to evaluate the relative order between elements</param>
	/// <param name="otherSequences">A variable argument array of zero or more other sequences to merge with</param>
	/// <returns>A merged, order-preserving sequence containing all of the elements of the original sequences</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="keySelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="otherSequences"/> is <see langword="null"/>.</exception>
	public static IEnumerable<TSource> SortedMergeByDescending<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey>? comparer, params IEnumerable<TSource>[] otherSequences)
	{
		return SortedMergeBy(source, keySelector, OrderByDirection.Descending, comparer, otherSequences);
	}

	/// <summary>
	/// Merges two or more sequences that are in a common order (either ascending or descending)
	/// according to a key into a single sequence that preserves that order.
	/// </summary>
	/// <remarks>
	/// Using SortedMergeBy on sequences that are not ordered or are not in the same order produces
	/// undefined results.<br/>
	/// This method uses deferred execution and streams its results.<br />
	/// </remarks>
	/// <typeparam name="TSource">The type of the elements of the sequence</typeparam>
	/// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector"/></typeparam>
	/// <param name="source">The primary sequence with which to merge</param>
	/// <param name="keySelector">A function to extract a key from an element.</param>
	/// <param name="direction">The ordering that all sequences must already exhibit</param>
	/// <param name="otherSequences">A variable argument array of zero or more other sequences to merge with</param>
	/// <returns>A merged, order-preserving sequence containing all of the elements of the original sequences</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="keySelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="otherSequences"/> is <see langword="null"/>.</exception>
	public static IEnumerable<TSource> SortedMergeBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, OrderByDirection direction, params IEnumerable<TSource>[] otherSequences)
	{
		return SortedMergeBy(source, keySelector, direction, comparer: null, otherSequences);
	}

	/// <summary>
	/// Merges two or more sequences that are in a common order (either ascending or descending)
	/// according to a key into a single sequence that preserves that order.
	/// </summary>
	/// <remarks>
	/// Using SortedMergeBy on sequences that are not ordered or are not in the same order produces
	/// undefined results.<br/>
	/// This method uses deferred execution and streams its results.<br />
	/// </remarks>
	/// <typeparam name="TSource">The type of the elements in the sequence</typeparam>
	/// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector"/></typeparam>
	/// <param name="source">The primary sequence with which to merge</param>
	/// <param name="keySelector">A function to extract a key from an element.</param>
	/// <param name="direction">The ordering that all sequences must already exhibit</param>
	/// <param name="comparer">The comparer used to evaluate the relative order between elements</param>
	/// <param name="otherSequences">A variable argument array of zero or more other sequences to merge with</param>
	/// <returns>A merged, order-preserving sequence containing all of the elements of the original sequences</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="keySelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="otherSequences"/> is <see langword="null"/>.</exception>
	public static IEnumerable<TSource> SortedMergeBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, OrderByDirection direction, IComparer<TKey>? comparer, params IEnumerable<TSource>[] otherSequences)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(keySelector);
		Guard.IsNotNull(otherSequences);

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
				list.MoveNextOnce(nextIndex);
			}
		}
	}
}
