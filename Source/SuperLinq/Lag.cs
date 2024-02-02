namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Produces a projection of a sequence by evaluating pairs of elements separated by a negative offset.
	/// </summary>
	/// <typeparam name="TSource">
	///	    The type of the elements of the source sequence
	/// </typeparam>
	/// <param name="source">
	///	    The sequence over which to evaluate lag
	/// </param>
	/// <param name="offset">
	///	    The offset (expressed as a positive number) by which to lag each value of the sequence
	/// </param>
	/// <returns>
	///	    A sequence of tuples with the current and lagged elements
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> is <see langword="null"/>.
	/// </exception>
	/// <exception cref="ArgumentOutOfRangeException">
	///	    <paramref name="offset"/> is below <c>1</c>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    For elements prior to the lag offset, <c><see langword="default"/>(<typeparamref name="TSource"/>?)</c> is
	///     used as the lagged value.<br/>
	/// </para>
	/// <para>
	///	    This operator evaluates in a deferred and streaming manner.
	/// </para>
	/// </remarks>
	public static IEnumerable<(TSource current, TSource? lag)> Lag<TSource>(this IEnumerable<TSource> source, int offset)
	{
		return source.Lag(offset, ValueTuple.Create);
	}

	/// <summary>
	///	    Produces a projection of a sequence by evaluating pairs of elements separated by a negative offset.
	/// </summary>
	/// <typeparam name="TSource">
	///	    The type of the elements of the source sequence
	/// </typeparam>
	/// <typeparam name="TResult">
	///	    The type of the elements of the result sequence
	/// </typeparam>
	/// <param name="source">
	///	    The sequence over which to evaluate lag
	/// </param>
	/// <param name="offset">
	///	    The offset (expressed as a positive number) by which to lag each value of the sequence
	/// </param>
	/// <param name="resultSelector">
	///	    A projection function which accepts the current and lagged items (in that order) and returns a result
	/// </param>
	/// <returns>
	///	    A sequence produced by projecting each element of the sequence with its lagged pairing
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="resultSelector"/> is <see langword="null"/>.
	/// </exception>
	/// <exception cref="ArgumentOutOfRangeException">
	///	    <paramref name="offset"/> is below <c>1</c>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    For elements prior to the lag offset, <c><see langword="default"/>(<typeparamref name="TSource"/>?)</c> is
	///     used as the lagged value.<br/>
	/// </para>
	/// <para>
	///	    This operator evaluates in a deferred and streaming manner.
	/// </para>
	/// </remarks>
	public static IEnumerable<TResult> Lag<TSource, TResult>(this IEnumerable<TSource> source, int offset, Func<TSource, TSource?, TResult> resultSelector)
	{
		return source.Lag(offset, default!, resultSelector);
	}

	/// <summary>
	///	    Produces a projection of a sequence by evaluating pairs of elements separated by a negative offset.
	/// </summary>
	/// <typeparam name="TSource">
	///	    The type of the elements of the source sequence
	/// </typeparam>
	/// <typeparam name="TResult">
	///	    The type of the elements of the result sequence
	/// </typeparam>
	/// <param name="source">
	///	    The sequence over which to evaluate lag
	/// </param>
	/// <param name="offset">
	///	    The offset (expressed as a positive number) by which to lag each value of the sequence
	/// </param>
	/// <param name="defaultLagValue">
	///	    A default value supplied for the lagged value prior to the lag offset
	/// </param>
	/// <param name="resultSelector">
	///	    A projection function which accepts the current and lagged items (in that order) and returns a result
	/// </param>
	/// <returns>
	///	    A sequence produced by projecting each element of the sequence with its lagged pairing
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="resultSelector"/> is <see langword="null"/>.
	/// </exception>
	/// <exception cref="ArgumentOutOfRangeException">
	///	    <paramref name="offset"/> is below <c>1</c>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    For elements prior to the lag offset, <paramref name="defaultLagValue"/> is used as the lagged value.<br/>
	/// </para>
	/// <para>
	///	    This operator evaluates in a deferred and streaming manner.
	/// </para>
	/// </remarks>
	public static IEnumerable<TResult> Lag<TSource, TResult>(this IEnumerable<TSource> source, int offset, TSource defaultLagValue, Func<TSource, TSource, TResult> resultSelector)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(resultSelector);
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(offset);

		if (source is IList<TSource> list)
			return new LagIterator<TSource, TResult>(list, offset, defaultLagValue, resultSelector);

		return Core(source, offset, defaultLagValue, resultSelector);

		static IEnumerable<TResult> Core(IEnumerable<TSource> source, int offset, TSource defaultLagValue, Func<TSource, TSource, TResult> resultSelector)
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

	private sealed class LagIterator<TSource, TResult>(
		IList<TSource> source,
		int offset,
		TSource defaultLagValue,
		Func<TSource, TSource, TResult> resultSelector
	) : ListIterator<TResult>
	{
		public override int Count => source.Count;

		protected override IEnumerable<TResult> GetEnumerable()
		{
			var cnt = (uint)source.Count;
			for (var i = 0; i < cnt; i++)
				yield return resultSelector(
					source[i],
					i < offset ? defaultLagValue : source[i - offset]);
		}

		protected override TResult ElementAt(int index)
		{
			ArgumentOutOfRangeException.ThrowIfNegative(index);
			ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, Count);

			return resultSelector(
				source[index],
				index < offset ? defaultLagValue : source[index - offset]);
		}
	}
}
