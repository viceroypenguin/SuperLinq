namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Produces a projection of a sequence by evaluating pairs of elements separated by a negative offset.
	/// </summary>
	/// <remarks>
	/// This operator evaluates in a deferred and streaming manner.<br/>
	/// For elements prior to the lag offset, <c>default(<typeparamref name="TSource"/>?)</c> is used as the lagged value.<br/>
	/// </remarks>
	/// <typeparam name="TSource">The type of the elements of the source sequence</typeparam>
	/// <param name="source">The sequence over which to evaluate lag</param>
	/// <param name="offset">The offset (expressed as a positive number) by which to lag each value of the sequence</param>
	/// <returns>A sequence of tuples with the current and lagged elements</returns>

	public static IAsyncEnumerable<(TSource current, TSource? lead)> Lead<TSource>(this IAsyncEnumerable<TSource> source, int offset)
	{
		source.ThrowIfNull();

		return source.Select(Some)
					 .Lead(offset, default, (curr, lead) => (curr.Value, lead is (true, var some) ? some : default));
	}

	/// <summary>
	/// Produces a projection of a sequence by evaluating pairs of elements separated by a positive offset.
	/// </summary>
	/// <remarks>
	/// This operator evaluates in a deferred and streaming manner.<br/>
	/// For elements of the sequence that are less than <paramref name="offset"/> items from the end,
	/// default(T) is used as the lead value.<br/>
	/// </remarks>
	/// <typeparam name="TSource">The type of the elements in the source sequence</typeparam>
	/// <typeparam name="TResult">The type of the elements in the result sequence</typeparam>
	/// <param name="source">The sequence over which to evaluate Lead</param>
	/// <param name="offset">The offset (expressed as a positive number) by which to lead each element of the sequence</param>
	/// <param name="resultSelector">A projection function which accepts the current and subsequent (lead) element (in that order) and produces a result</param>
	/// <returns>A sequence produced by projecting each element of the sequence with its lead pairing</returns>

	public static IAsyncEnumerable<TResult> Lead<TSource, TResult>(this IAsyncEnumerable<TSource> source, int offset, Func<TSource, TSource?, TResult> resultSelector)
	{
		source.ThrowIfNull();
		resultSelector.ThrowIfNull();

		return source.Select(Some)
					 .Lead(offset, default, (curr, lead) => resultSelector(curr.Value, lead is (true, var some) ? some : default));
	}

	/// <summary>
	/// Produces a projection of a sequence by evaluating pairs of elements separated by a positive offset.
	/// </summary>
	/// <remarks>
	/// This operator evaluates in a deferred and streaming manner.<br/>
	/// </remarks>
	/// <typeparam name="TSource">The type of the elements in the source sequence</typeparam>
	/// <typeparam name="TResult">The type of the elements in the result sequence</typeparam>
	/// <param name="source">The sequence over which to evaluate Lead</param>
	/// <param name="offset">The offset (expressed as a positive number) by which to lead each element of the sequence</param>
	/// <param name="defaultLeadValue">A default value supplied for the leading element when none is available</param>
	/// <param name="resultSelector">A projection function which accepts the current and subsequent (lead) element (in that order) and produces a result</param>
	/// <returns>A sequence produced by projecting each element of the sequence with its lead pairing</returns>

	public static IAsyncEnumerable<TResult> Lead<TSource, TResult>(this IAsyncEnumerable<TSource> source, int offset, TSource defaultLeadValue, Func<TSource, TSource, TResult> resultSelector)
	{
		return source.Lead(offset, defaultLeadValue, (curr, lead) => new ValueTask<TResult>(resultSelector(curr, lead)));
	}

	/// <summary>
	/// Produces a projection of a sequence by evaluating pairs of elements separated by a positive offset.
	/// </summary>
	/// <remarks>
	/// This operator evaluates in a deferred and streaming manner.<br/>
	/// </remarks>
	/// <typeparam name="TSource">The type of the elements in the source sequence</typeparam>
	/// <typeparam name="TResult">The type of the elements in the result sequence</typeparam>
	/// <param name="source">The sequence over which to evaluate Lead</param>
	/// <param name="offset">The offset (expressed as a positive number) by which to lead each element of the sequence</param>
	/// <param name="defaultLeadValue">A default value supplied for the leading element when none is available</param>
	/// <param name="resultSelector">A projection function which accepts the current and subsequent (lead) element (in that order) and produces a result</param>
	/// <returns>A sequence produced by projecting each element of the sequence with its lead pairing</returns>

	public static IAsyncEnumerable<TResult> Lead<TSource, TResult>(this IAsyncEnumerable<TSource> source, int offset, TSource defaultLeadValue, Func<TSource, TSource, ValueTask<TResult>> resultSelector)
	{
		source.ThrowIfNull();
		resultSelector.ThrowIfNull();
		offset.ThrowIfLessThan(1);

		return _(source, offset, defaultLeadValue, resultSelector);

		static async IAsyncEnumerable<TResult> _(IAsyncEnumerable<TSource> source, int offset, TSource defaultLeadValue, Func<TSource, TSource, ValueTask<TResult>> resultSelector, [EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			var leadQueue = new Queue<TSource>(offset);
			await using var iter = source.GetConfiguredAsyncEnumerator(cancellationToken);

			bool hasMore;
			// first, prefetch and populate the lead queue with the next step of
			// items to be streamed out to the consumer of the sequence
			while ((hasMore = await iter.MoveNextAsync()) && leadQueue.Count < offset)
				leadQueue.Enqueue(iter.Current);
			// next, while the source sequence has items, yield the result of
			// the projection function applied to the top of queue and current item
			while (hasMore)
			{
				yield return await resultSelector(leadQueue.Dequeue(), iter.Current).ConfigureAwait(false);
				leadQueue.Enqueue(iter.Current);
				hasMore = await iter.MoveNextAsync();
			}
			// yield the remaining values in the lead queue with the default lead value
			while (leadQueue.Count > 0)
				yield return await resultSelector(leadQueue.Dequeue(), defaultLeadValue).ConfigureAwait(false);
		}
	}
}
