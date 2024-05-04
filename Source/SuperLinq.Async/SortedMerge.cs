namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
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
	public static IAsyncEnumerable<TSource> SortedMerge<TSource>(
		this IAsyncEnumerable<TSource> source,
		params IAsyncEnumerable<TSource>[] otherSequences
	)
	{
		return SortedMerge(source, OrderByDirection.Ascending, comparer: null, otherSequences);
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
	public static IAsyncEnumerable<TSource> SortedMerge<TSource>(
		this IAsyncEnumerable<TSource> source,
		IComparer<TSource>? comparer,
		params IAsyncEnumerable<TSource>[] otherSequences
	)
	{
		return SortedMerge(source, OrderByDirection.Ascending, comparer, otherSequences);
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
	public static IAsyncEnumerable<TSource> SortedMerge<TSource>(
		this IAsyncEnumerable<TSource> source,
		OrderByDirection direction,
		params IAsyncEnumerable<TSource>[] otherSequences
	)
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
	public static IAsyncEnumerable<TSource> SortedMerge<TSource>(
		this IAsyncEnumerable<TSource> source,
		OrderByDirection direction,
		IComparer<TSource>? comparer,
		params IAsyncEnumerable<TSource>[] otherSequences
	)
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
	public static IAsyncEnumerable<TSource> SortedMergeBy<TSource, TKey>(
		this IAsyncEnumerable<TSource> source,
		Func<TSource, TKey> keySelector,
		params IAsyncEnumerable<TSource>[] otherSequences
	)
	{
		return SortedMergeBy(source, keySelector, OrderByDirection.Ascending, comparer: null, otherSequences);
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
	public static IAsyncEnumerable<TSource> SortedMergeBy<TSource, TKey>(
		this IAsyncEnumerable<TSource> source,
		Func<TSource, TKey> keySelector,
		IComparer<TKey>? comparer,
		params IAsyncEnumerable<TSource>[] otherSequences
	)
	{
		return SortedMergeBy(source, keySelector, OrderByDirection.Ascending, comparer, otherSequences);
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
	public static IAsyncEnumerable<TSource> SortedMergeBy<TSource, TKey>(
		this IAsyncEnumerable<TSource> source,
		Func<TSource, TKey> keySelector,
		OrderByDirection direction,
		params IAsyncEnumerable<TSource>[] otherSequences
	)
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
	public static IAsyncEnumerable<TSource> SortedMergeBy<TSource, TKey>(
		this IAsyncEnumerable<TSource> source,
		Func<TSource, TKey> keySelector,
		OrderByDirection direction,
		IComparer<TKey>? comparer,
		params IAsyncEnumerable<TSource>[] otherSequences
	)
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

		static async IAsyncEnumerable<TSource> Impl(
			IEnumerable<IAsyncEnumerable<TSource>> sequences,
			Func<TSource, TKey> keySelector,
			IComparer<TKey> comparer,
			[EnumeratorCancellation] CancellationToken cancellationToken = default
		)
		{
			var enumerators = new List<IAsyncEnumerator<TSource>>();

			try
			{
				// Ensure we dispose first N enumerators if N+1 throws 
				foreach (var sequence in sequences)
				{
					var e = sequence.GetAsyncEnumerator(cancellationToken);

					if (await e.MoveNextAsync())
						enumerators.Add(e);
					else
						await e.DisposeAsync();
				}
#if NET6_0_OR_GREATER
				var queue = new PriorityQueue<IAsyncEnumerator<TSource>, TKey>(
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
						while (await e.MoveNextAsync())
							yield return e.Current;

						break;
					}

					if (await e.MoveNextAsync())
						queue.Enqueue(e, keySelector(e.Current));
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

					if (!await e.MoveNextAsync())
					{
						count--;
						Array.Copy(arr, 1, arr, 0, count);
						continue;
					}

					var index = Array.BinarySearch(arr, 1, count - 1, e, sourceComparer);
					if (index < 0)
						index = ~index;

					index--;
					if (index > 0)
						Array.Copy(arr, 1, arr, 0, index);

					arr[index] = e;
				}

				if (count == 1)
				{
					var e = arr[0];
					yield return e.Current;

					while (await e.MoveNextAsync()) yield return e.Current;
				}
#endif
			}
			finally
			{
				foreach (var e in enumerators)
					await e.DisposeAsync();
			}
		}
	}

#if !NET6_0_OR_GREATER
	internal sealed class SourceComparer<TItem, TKey>(
		IComparer<TKey> keyComparer,
		Func<TItem, TKey> keySelector
	) : IComparer<IAsyncEnumerator<TItem>>
	{
		public int Compare(IAsyncEnumerator<TItem>? x, IAsyncEnumerator<TItem>? y) =>
			keyComparer.Compare(keySelector(x!.Current), keySelector(y!.Current));
	}
#endif
}
