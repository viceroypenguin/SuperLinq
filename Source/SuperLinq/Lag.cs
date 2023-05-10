namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Produces a projection of a sequence by evaluating pairs of elements separated by a negative offset.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements of the source sequence</typeparam>
	/// <param name="source">The sequence over which to evaluate lag</param>
	/// <param name="offset">The offset (expressed as a positive number) by which to lag each value of the sequence</param>
	/// <returns>A sequence of tuples with the current and lagged elements</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is below 1.</exception>
	/// <remarks>
	/// This operator evaluates in a deferred and streaming manner.<br/>
	/// For elements prior to the lag offset, <see langword="default"/>(<typeparamref name="TSource"/>?) is used as the lagged value.<br/>
	/// </remarks>
	public static IEnumerable<(TSource current, TSource? lag)> Lag<TSource>(this IEnumerable<TSource> source, int offset)
	{
		return source.Lag(offset, ValueTuple.Create);
	}

	/// <summary>
	/// Produces a projection of a sequence by evaluating pairs of elements separated by a negative offset.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements of the source sequence</typeparam>
	/// <typeparam name="TResult">The type of the elements of the result sequence</typeparam>
	/// <param name="source">The sequence over which to evaluate lag</param>
	/// <param name="offset">The offset (expressed as a positive number) by which to lag each value of the sequence</param>
	/// <param name="resultSelector">A projection function which accepts the current and lagged items (in that order) and returns a result</param>
	/// <returns>A sequence produced by projecting each element of the sequence with its lagged pairing</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="resultSelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is below 1.</exception>
	/// <remarks>
	/// This operator evaluates in a deferred and streaming manner.<br/>
	/// For elements prior to the lag offset, <see langword="default"/>(<typeparamref name="TSource"/>?) is used as the lagged value.<br/>
	/// </remarks>
	public static IEnumerable<TResult> Lag<TSource, TResult>(this IEnumerable<TSource> source, int offset, Func<TSource, TSource?, TResult> resultSelector)
	{
		return source.Lag(offset, default!, resultSelector);
	}

	/// <summary>
	/// Produces a projection of a sequence by evaluating pairs of elements separated by a negative offset.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements of the source sequence</typeparam>
	/// <typeparam name="TResult">The type of the elements of the result sequence</typeparam>
	/// <param name="source">The sequence over which to evaluate lag</param>
	/// <param name="offset">The offset (expressed as a positive number) by which to lag each value of the sequence</param>
	/// <param name="defaultLagValue">A default value supplied for the lagged value prior to the lag offset</param>
	/// <param name="resultSelector">A projection function which accepts the current and lagged items (in that order) and returns a result</param>
	/// <returns>A sequence produced by projecting each element of the sequence with its lagged pairing</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="resultSelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is below 1.</exception>
	/// <remarks>
	/// This operator evaluates in a deferred and streaming manner.<br/>
	/// </remarks>
	public static IEnumerable<TResult> Lag<TSource, TResult>(this IEnumerable<TSource> source, int offset, TSource defaultLagValue, Func<TSource, TSource, TResult> resultSelector)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(resultSelector);
		Guard.IsGreaterThanOrEqualTo(offset, 1);

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

	private sealed class LagIterator<TSource, TResult> : ListIterator<TResult>
	{
		private readonly IList<TSource> _source;
		private readonly int _offset;
		private readonly TSource _defaultLagValue;
		private readonly Func<TSource, TSource, TResult> _resultSelector;

		public LagIterator(IList<TSource> source, int offset, TSource defaultLagValue, Func<TSource, TSource, TResult> resultSelector)
		{
			_source = source;
			_offset = offset;
			_defaultLagValue = defaultLagValue;
			_resultSelector = resultSelector;
		}

		public override int Count => _source.Count;

		protected override IEnumerable<TResult> GetEnumerable()
		{
			var cnt = (uint)_source.Count;
			for (var i = 0; i < cnt; i++)
				yield return _resultSelector(
					_source[i],
					i < _offset ? _defaultLagValue : _source[i - _offset]);
		}

		protected override TResult ElementAt(int index) =>
			_resultSelector(
				_source[index],
				index < _offset ? _defaultLagValue : _source[index - _offset]);
	}
}
