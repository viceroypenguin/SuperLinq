namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Produces a projection of a sequence by evaluating pairs of elements separated by a negative offset.
	/// </summary>
	/// <remarks>
	/// This operator evaluates in a deferred and streaming manner.<br/>
	/// For elements prior to the lag offset, <see langword="default"/>(<typeparamref name="TSource"/>?) is used as the lagged value.<br/>
	/// </remarks>
	/// <typeparam name="TSource">The type of the elements of the source sequence</typeparam>
	/// <param name="source">The sequence over which to evaluate lag</param>
	/// <param name="offset">The offset (expressed as a positive number) by which to lag each value of the sequence</param>
	/// <returns>A sequence of tuples with the current and lagged elements</returns>

	public static IAsyncEnumerable<(TSource current, TSource? lead)> Lead<TSource>(this IAsyncEnumerable<TSource> source, int offset)
	{
		ArgumentNullException.ThrowIfNull(source);

		return source
			.Select(Some)
			.Lead(offset, default, (curr, lead) => (curr.Value, lead is (true, var some) ? some : default));
	}

	/// <summary>
	/// Produces a projection of a sequence by evaluating pairs of elements separated by a positive offset.
	/// </summary>
	/// <remarks>
	/// This operator evaluates in a deferred and streaming manner.<br/>
	/// For elements of the sequence that are less than <paramref name="offset"/> items from the end,
	/// <see langword="default"/>(<typeparamref name="TSource"/>?) is used as the lead value.<br/>
	/// </remarks>
	/// <typeparam name="TSource">The type of the elements in the source sequence</typeparam>
	/// <typeparam name="TResult">The type of the elements in the result sequence</typeparam>
	/// <param name="source">The sequence over which to evaluate Lead</param>
	/// <param name="offset">The offset (expressed as a positive number) by which to lead each element of the sequence</param>
	/// <param name="resultSelector">A projection function which accepts the current and subsequent (lead) element (in that order) and produces a result</param>
	/// <returns>A sequence produced by projecting each element of the sequence with its lead pairing</returns>

	public static IAsyncEnumerable<TResult> Lead<TSource, TResult>(
		this IAsyncEnumerable<TSource> source,
		int offset,
		Func<TSource, TSource?, TResult> resultSelector)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(resultSelector);

		return source
			.Select(Some)
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

	public static IAsyncEnumerable<TResult> Lead<TSource, TResult>(
		this IAsyncEnumerable<TSource> source,
		int offset,
		TSource defaultLeadValue,
		Func<TSource, TSource, TResult> resultSelector
	)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(resultSelector);

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

	public static IAsyncEnumerable<TResult> Lead<TSource, TResult>(
		this IAsyncEnumerable<TSource> source,
		int offset,
		TSource defaultLeadValue,
		Func<TSource, TSource, ValueTask<TResult>> resultSelector
	)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(resultSelector);
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(offset);

		return Core(source, offset, defaultLeadValue, resultSelector);

		static async IAsyncEnumerable<TResult> Core(
			IAsyncEnumerable<TSource> source,
			int offset,
			TSource defaultLeadValue,
			Func<TSource, TSource, ValueTask<TResult>> resultSelector,
			[EnumeratorCancellation] CancellationToken cancellationToken = default
		)
		{
			var queue = new Queue<TSource>(offset + 1);

			await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
			{
				queue.Enqueue(item);
				if (queue.Count > offset)
					yield return await resultSelector(queue.Dequeue(), item).ConfigureAwait(false);
			}

			while (queue.Count > 0)
				yield return await resultSelector(queue.Dequeue(), defaultLeadValue).ConfigureAwait(false);
		}
	}
}
