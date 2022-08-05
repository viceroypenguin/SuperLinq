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

	public static IAsyncEnumerable<(TSource current, TSource? lag)> Lag<TSource>(this IAsyncEnumerable<TSource> source, int offset)
	{
		Guard.IsNotNull(source);

		return source.Select(Some)
					 .Lag(offset, default, (curr, lag) => (curr.Value, lag is (true, var some) ? some : default));
	}

	/// <summary>
	/// Produces a projection of a sequence by evaluating pairs of elements separated by a negative offset.
	/// </summary>
	/// <remarks>
	/// This operator evaluates in a deferred and streaming manner.<br/>
	/// For elements prior to the lag offset, <see langword="default"/>(<typeparamref name="TSource"/>?) is used as the lagged value.<br/>
	/// </remarks>
	/// <typeparam name="TSource">The type of the elements of the source sequence</typeparam>
	/// <typeparam name="TResult">The type of the elements of the result sequence</typeparam>
	/// <param name="source">The sequence over which to evaluate lag</param>
	/// <param name="offset">The offset (expressed as a positive number) by which to lag each value of the sequence</param>
	/// <param name="resultSelector">A projection function which accepts the current and lagged items (in that order) and returns a result</param>
	/// <returns>A sequence produced by projecting each element of the sequence with its lagged pairing</returns>

	public static IAsyncEnumerable<TResult> Lag<TSource, TResult>(this IAsyncEnumerable<TSource> source, int offset, Func<TSource, TSource?, TResult> resultSelector)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(resultSelector);

		return source.Select(Some)
					 .Lag(offset, default, (curr, lag) => resultSelector(curr.Value, lag is (true, var some) ? some : default));
	}

	/// <summary>
	/// Produces a projection of a sequence by evaluating pairs of elements separated by a negative offset.
	/// </summary>
	/// <remarks>
	/// This operator evaluates in a deferred and streaming manner.<br/>
	/// </remarks>
	/// <typeparam name="TSource">The type of the elements of the source sequence</typeparam>
	/// <typeparam name="TResult">The type of the elements of the result sequence</typeparam>
	/// <param name="source">The sequence over which to evaluate lag</param>
	/// <param name="offset">The offset (expressed as a positive number) by which to lag each value of the sequence</param>
	/// <param name="defaultLagValue">A default value supplied for the lagged value prior to the lag offset</param>
	/// <param name="resultSelector">A projection function which accepts the current and lagged items (in that order) and returns a result</param>
	/// <returns>A sequence produced by projecting each element of the sequence with its lagged pairing</returns>

	public static IAsyncEnumerable<TResult> Lag<TSource, TResult>(this IAsyncEnumerable<TSource> source, int offset, TSource defaultLagValue, Func<TSource, TSource, TResult> resultSelector)
	{
		return source.Lag(offset, defaultLagValue, (curr, lag) => new ValueTask<TResult>(resultSelector(curr, lag)));
	}

	/// <summary>
	/// Produces a projection of a sequence by evaluating pairs of elements separated by a negative offset.
	/// </summary>
	/// <remarks>
	/// This operator evaluates in a deferred and streaming manner.<br/>
	/// </remarks>
	/// <typeparam name="TSource">The type of the elements of the source sequence</typeparam>
	/// <typeparam name="TResult">The type of the elements of the result sequence</typeparam>
	/// <param name="source">The sequence over which to evaluate lag</param>
	/// <param name="offset">The offset (expressed as a positive number) by which to lag each value of the sequence</param>
	/// <param name="defaultLagValue">A default value supplied for the lagged value prior to the lag offset</param>
	/// <param name="resultSelector">A projection function which accepts the current and lagged items (in that order) and returns a result</param>
	/// <returns>A sequence produced by projecting each element of the sequence with its lagged pairing</returns>

	public static IAsyncEnumerable<TResult> Lag<TSource, TResult>(this IAsyncEnumerable<TSource> source, int offset, TSource defaultLagValue, Func<TSource, TSource, ValueTask<TResult>> resultSelector)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(resultSelector);
		Guard.IsGreaterThanOrEqualTo(offset, 1);

		return _(source, offset, defaultLagValue, resultSelector);

		static async IAsyncEnumerable<TResult> _(IAsyncEnumerable<TSource> source, int offset, TSource defaultLagValue, Func<TSource, TSource, ValueTask<TResult>> resultSelector, [EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			var lagQueue = new Queue<TSource>(offset + 1);
			await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
			{
				lagQueue.Enqueue(item);
				yield return await resultSelector(
						item,
						lagQueue.Count > offset ? lagQueue.Dequeue() : defaultLagValue)
					.ConfigureAwait(false);
			}
		}
	}
}
