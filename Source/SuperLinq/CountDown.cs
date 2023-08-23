namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Provides a countdown counter for a given count of elements at the tail of the sequence where zero always
	///     represents the last element, one represents the second-last element, two represents the third-last element
	///     and so on.
	/// </summary>
	/// <typeparam name="TSource">
	///	    The type of elements of <paramref name="source"/>
	/// </typeparam>
	/// <param name="source">
	///	    The source sequence.
	/// </param>
	/// <param name="count">
	///	    Count of tail elements of <paramref name="source"/> to count down.
	/// </param>
	/// <returns>
	///	    A sequence of tuples containing the element and it's count from the end of the sequence, or <see
	///     langword="null"/>.
	/// </returns>  
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> is <see langword="null"/>.
	/// </exception>
	/// <exception cref="ArgumentOutOfRangeException">
	///	    <paramref name="count"/> is <c>0</c> or negative.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    At most, <paramref name="count"/> elements of the source sequence may be buffered at any one time unless
	///     <paramref name="source"/> is a collection or a list.
	/// </para>
	/// <para>
	///	    This method uses deferred execution semantics and streams its results.
	/// </para>
	/// </remarks>
	public static IEnumerable<(TSource item, int? count)> CountDown<TSource>(this IEnumerable<TSource> source, int count)
	{
		return source.CountDown(count, ValueTuple.Create);
	}

	/// <summary>
	///	    Provides a countdown counter for a given count of elements at the tail of the sequence where zero always
	///     represents the last element, one represents the second-last element, two represents the third-last element
	///     and so on.
	/// </summary>
	/// <typeparam name="TSource">
	///	    The type of elements of <paramref name="source"/>
	/// </typeparam>
	/// <typeparam name="TResult">
	///	    The type of elements of the resulting sequence.
	/// </typeparam>
	///	    <param name="source">The source sequence.
	/// </param>
	///	    <param name="count">Count of tail elements of <paramref name="source"/> to count down.
	/// </param>
	/// <param name="resultSelector">
	///	    A function that receives the element and the current countdown value for the element and which returns those
	///     mapped to a result returned in the resulting sequence. For elements before the last <paramref
	///     name="count"/>, the countdown value is <see langword="null"/>.
	/// </param>
	/// <returns>
	///	    A sequence of results returned by <paramref name="resultSelector"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="resultSelector"/> is <see langword="null"/>.
	/// </exception>
	/// <exception cref="ArgumentOutOfRangeException">
	///	    <paramref name="count"/> is <c>0</c> or negative.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    At most, <paramref name="count"/> elements of the source sequence may be buffered at any one time unless
	///     <paramref name="source"/> is a collection or a list.
	/// </para>
	/// <para>
	///	    This method uses deferred execution semantics and streams its results.
	/// </para>
	/// </remarks>
	public static IEnumerable<TResult> CountDown<TSource, TResult>(
		this IEnumerable<TSource> source,
		int count, Func<TSource, int?, TResult> resultSelector)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(resultSelector);
		Guard.IsGreaterThanOrEqualTo(count, 1);

		if (source is IList<TSource> list)
			return new CountDownListIterator<TSource, TResult>(list, count, resultSelector);

		if (source.TryGetCollectionCount() is int)
			return new CountDownCollectionIterator<TSource, TResult>(source, count, resultSelector);

		return Core(source, count, resultSelector);

		static IEnumerable<TResult> Core(IEnumerable<TSource> source, int count, Func<TSource, int?, TResult> resultSelector)
		{
			var queue = new Queue<TSource>(Math.Max(1, count + 1));

			foreach (var item in source)
			{
				queue.Enqueue(item);
				if (queue.Count > count)
					yield return resultSelector(queue.Dequeue(), null);
			}

			while (queue.Count > 0)
				yield return resultSelector(queue.Dequeue(), queue.Count);
		}
	}

	private sealed class CountDownCollectionIterator<TSource, TResult> : CollectionIterator<TResult>
	{
		private readonly IEnumerable<TSource> _source;
		private readonly int _count;
		private readonly Func<TSource, int?, TResult> _resultSelector;

		public CountDownCollectionIterator(IEnumerable<TSource> source, int count, Func<TSource, int?, TResult> resultSelector)
		{
			_source = source;
			_count = count;
			_resultSelector = resultSelector;
		}

		public override int Count => _source.GetCollectionCount();

		protected override IEnumerable<TResult> GetEnumerable()
		{
			var i = Count;
			foreach (var item in _source)
				yield return _resultSelector(item, i-- <= _count ? i : null);
		}
	}

	private sealed class CountDownListIterator<TSource, TResult> : ListIterator<TResult>
	{
		private readonly IList<TSource> _source;
		private readonly int _count;
		private readonly Func<TSource, int?, TResult> _resultSelector;

		public CountDownListIterator(IList<TSource> source, int count, Func<TSource, int?, TResult> resultSelector)
		{
			_source = source;
			_count = count;
			_resultSelector = resultSelector;
		}

		public override int Count => _source.Count;

		protected override IEnumerable<TResult> GetEnumerable()
		{
			var cnt = (uint)_source.Count - _count;
			var i = 0;
			for (; i < cnt; i++)
			{
				yield return _resultSelector(
					_source[i],
					null);
			}

			cnt = (uint)_source.Count;
			for (; i < cnt; i++)
			{
				yield return _resultSelector(
					_source[i],
					(int)cnt - i - 1);
			}
		}

		protected override TResult ElementAt(int index)
		{
			Guard.IsBetweenOrEqualTo(index, 0, Count - 1);
			return _resultSelector(
				_source[index],
				_source.Count - index < _count ? _source.Count - index - 1 : null);
		}
	}
}
