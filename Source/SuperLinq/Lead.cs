namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Produces a projection of a sequence by evaluating pairs of elements separated by a positive offset.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements in the source sequence</typeparam>
	/// <param name="source">The sequence over which to evaluate Lead</param>
	/// <param name="offset">The offset (expressed as a positive number) by which to lead each element of the sequence</param>
	/// <returns>A sequence of tuples with the current and lead elements</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is below 1.</exception>
	/// <remarks>
	/// This operator evaluates in a deferred and streaming manner.<br/>
	/// For elements of the sequence that are less than <paramref name="offset"/> items from the end,
	/// <see langword="default"/>(<typeparamref name="TSource"/>?) is used as the lead value.<br/>
	/// </remarks>
	public static IEnumerable<(TSource current, TSource? lead)> Lead<TSource>(this IEnumerable<TSource> source, int offset)
	{
		return source.Lead(offset, ValueTuple.Create);
	}

	/// <summary>
	/// Produces a projection of a sequence by evaluating pairs of elements separated by a positive offset.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements in the source sequence</typeparam>
	/// <typeparam name="TResult">The type of the elements in the result sequence</typeparam>
	/// <param name="source">The sequence over which to evaluate Lead</param>
	/// <param name="offset">The offset (expressed as a positive number) by which to lead each element of the sequence</param>
	/// <param name="resultSelector">A projection function which accepts the current and subsequent (lead) element (in that order) and produces a result</param>
	/// <returns>A sequence produced by projecting each element of the sequence with its lead pairing</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="resultSelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is below 1.</exception>
	/// <remarks>
	/// This operator evaluates in a deferred and streaming manner.<br/>
	/// For elements of the sequence that are less than <paramref name="offset"/> items from the end,
	/// <see langword="default"/>(<typeparamref name="TSource"/>?) is used as the lead value.<br/>
	/// </remarks>
	public static IEnumerable<TResult> Lead<TSource, TResult>(this IEnumerable<TSource> source, int offset, Func<TSource, TSource?, TResult> resultSelector)
	{
		return source.Lead(offset, default!, resultSelector);
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
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="resultSelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is below 1.</exception>
	/// <remarks>
	/// This operator evaluates in a deferred and streaming manner.<br/>
	/// </remarks>
	public static IEnumerable<TResult> Lead<TSource, TResult>(this IEnumerable<TSource> source, int offset, TSource defaultLeadValue, Func<TSource, TSource, TResult> resultSelector)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(resultSelector);
		Guard.IsGreaterThanOrEqualTo(offset, 1);

		if (source is IList<TSource> list)
			return new LeadIterator<TSource, TResult>(list, offset, defaultLeadValue, resultSelector);

		return Core(source, offset, defaultLeadValue, resultSelector);

		static IEnumerable<TResult> Core(IEnumerable<TSource> source, int offset, TSource defaultLeadValue, Func<TSource, TSource, TResult> resultSelector)
		{
			var queue = new Queue<TSource>(offset + 1);

			foreach (var item in source)
			{
				queue.Enqueue(item);
				if (queue.Count > offset)
					yield return resultSelector(queue.Dequeue(), item);
			}

			while (queue.Count > 0)
				yield return resultSelector(queue.Dequeue(), defaultLeadValue);
		}
	}

	private sealed class LeadIterator<TSource, TResult> : ListIterator<TResult>
	{
		private readonly IList<TSource> _source;
		private readonly int _offset;
		private readonly TSource _defaultLeadValue;
		private readonly Func<TSource, TSource, TResult> _resultSelector;

		public LeadIterator(IList<TSource> source, int offset, TSource defaultLeadValue, Func<TSource, TSource, TResult> resultSelector)
		{
			_source = source;
			_offset = offset;
			_defaultLeadValue = defaultLeadValue;
			_resultSelector = resultSelector;
		}

		public override int Count => _source.Count;

		protected override IEnumerable<TResult> GetEnumerable()
		{
			var cnt = (uint)_source.Count;
			var maxOffset = Math.Max(_source.Count - _offset, 0);
			for (var i = 0; i < cnt; i++)
				yield return _resultSelector(
					_source[i],
					i < maxOffset ? _source[i + _offset] : _defaultLeadValue);
		}

		protected override TResult ElementAt(int index)
		{
			Guard.IsBetweenOrEqualTo(index, 0, Count - 1);
			return _resultSelector(
				_source[index],
				index < Math.Max(_source.Count - _offset, 0)
					? _source[index + _offset]
					: _defaultLeadValue);
		}
	}
}
