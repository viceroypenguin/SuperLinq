namespace SuperLinq;

public static partial class SuperEnumerable
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

	public static IEnumerable<(TSource current, TSource? lag)> Lag<TSource>(this IEnumerable<TSource> source, int offset)
	{
		source.ThrowIfNull();

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

	public static IEnumerable<TResult> Lag<TSource, TResult>(this IEnumerable<TSource> source, int offset, Func<TSource, TSource?, TResult> resultSelector)
	{
		source.ThrowIfNull();
		resultSelector.ThrowIfNull();

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

	public static IEnumerable<TResult> Lag<TSource, TResult>(this IEnumerable<TSource> source, int offset, TSource defaultLagValue, Func<TSource, TSource, TResult> resultSelector)
	{
		source.ThrowIfNull();
		resultSelector.ThrowIfNull();
		offset.ThrowIfLessThan(1);

		return _(source, offset, defaultLagValue, resultSelector);

		static IEnumerable<TResult> _(IEnumerable<TSource> source, int offset, TSource defaultLagValue, Func<TSource, TSource, TResult> resultSelector)
		{
			var lagQueue = new Queue<TSource>(offset + 1);
			foreach (var item in source)
			{
				lagQueue.Enqueue(item);
				yield return resultSelector(
					item,
					lagQueue.Count > offset ? lagQueue.Dequeue() : defaultLagValue);
			}
		}
	}
}
