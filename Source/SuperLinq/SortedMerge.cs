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
		// Where available, PriorityQueue is used to maintain the remaining enumerators ordered by the
		// value of the Current property.
		//
		// Otherwise, a sorted array is used, with BinarySearch finding the re-insert location.

		static IEnumerable<TSource> Impl(IEnumerable<IEnumerable<TSource>> sequences, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
		{
			var enumerators = new List<IEnumerator<TSource>>();

			try
			{
				// Ensure we dispose first N enumerators if N+1 throws 
				foreach (var sequence in sequences)
				{
					var e = sequence.GetEnumerator();
					if (e.MoveNext())
					{
						enumerators.Add(e);
					}
					else
					{
						e.Dispose();
					}
				}

#if NET6_0_OR_GREATER
				var queue = new PriorityQueue<IEnumerator<TSource>, TKey>(
					enumerators.Select(x => (x, keySelector(x.Current))),
					comparer);

#pragma warning disable CA2000 // e will be disposed via enumerators list
				while (queue.TryDequeue(out var e, out var _))
#pragma warning restore CA2000 // Dispose objects before losing scope
				{
					yield return e.Current;

					// Fast drain of final enumerator
					if (queue.Count == 0)
					{
						while (e.MoveNext())
						{
							yield return e.Current;
						}

						break;
					}

					if (e.MoveNext())
					{
						queue.Enqueue(e, keySelector(e.Current));
					}
				}

#else
				enumerators.Sort((x, y) => comparer.Compare(keySelector(x.Current), keySelector(y.Current)));

				var arr = enumerators.ToArray();
				var count = arr.Length;
				var sourceComparer = new SourceComparer<TSource, TKey>(comparer, keySelector);

				while (count > 1)
				{
					var e = arr[0];
					yield return e.Current;

					if (!e.MoveNext())
					{
						count--;
						Array.Copy(arr, 1, arr, 0, count);
						continue;
					}

					var index = Array.BinarySearch(arr, 1, count - 1, e, sourceComparer);
					if (index < 0) 
					{
						index = ~index;
					}

					index--;

					if (index > 0)
					{
						Array.Copy(arr, 1, arr, 0, index);
					}

					arr[index] = e;
				}

				if (count == 1)
				{
					var e = arr[0];
					yield return e.Current;

					while (e.MoveNext())
					{
						yield return e.Current;
					}
				}
#endif
			}
			finally
			{
				foreach (var e in enumerators)
				{
					e.Dispose();
				}
			}
		}
	}

#if !NET6_0_OR_GREATER
	internal sealed record class SourceComparer<TItem, TKey>(
		IComparer<TKey> KeyComparer,
		Func<TItem, TKey> KeySelector
	) : IComparer<IEnumerator<TItem>>
	{
		public int Compare(IEnumerator<TItem>? x, IEnumerator<TItem>? y)
			=> KeyComparer.Compare(KeySelector(x!.Current), KeySelector(y!.Current));
	}
#endif
}
