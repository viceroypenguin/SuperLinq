namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Filters a sequence of values based on a predicate evaluated on the current value and a leading value.
	/// </summary>
	/// <typeparam name="TSource">
	///	    The type of the elements in the source sequence.
	/// </typeparam>
	/// <param name="source">
	///	    The sequence over which to evaluate Lead.
	/// </param>
	/// <param name="offset">
	///	    The offset (expressed as a positive number) by which to lead each element of the sequence.
	/// </param>
	/// <param name="predicate">
	///	    A function which accepts the current and subsequent (lead) element (in that order) to test for a condition.
	/// </param>
	/// <returns>
	///	    An <see cref="IEnumerable{T}"/> that contains elements from the input sequence that satisfy the condition.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.
	/// </exception>
	/// <exception cref="ArgumentOutOfRangeException">
	///	    <paramref name="offset"/> is below <c>1</c>.</exception>
	/// <remarks>
	/// <para>
	///	    For elements of the sequence that are less than <paramref name="offset"/> items from the end, <see
	///     langword="default"/>(<typeparamref name="TSource"/>?) is used as the lead value.
	/// </para>
	/// <para>
	///	    This operator evaluates in a deferred and streaming manner.
	/// </para>
	/// </remarks>
	public static IEnumerable<TSource> WhereLead<TSource>(
		this IEnumerable<TSource> source,
		int offset,
		Func<TSource, TSource?, bool> predicate
	)
	{
		return source.WhereLead(offset, default!, predicate);
	}

	/// <summary>
	///	    Filters a sequence of values based on a predicate evaluated on the current value and a leading value.
	/// </summary>
	/// <typeparam name="TSource">
	///	    The type of the elements in the source sequence.
	/// </typeparam>
	/// <param name="source">
	///	    The sequence over which to evaluate Lead.
	/// </param>
	/// <param name="offset">
	///	    The offset (expressed as a positive number) by which to lead each element of the sequence.
	/// </param>
	/// <param name="defaultLeadValue">
	///	    A default value supplied for the leading element when none is available
	/// </param>
	/// <param name="predicate">
	///	    A function which accepts the current and subsequent (lead) element (in that order) to test for a condition.
	/// </param>
	/// <returns>
	///	    An <see cref="IEnumerable{T}"/> that contains elements from the input sequence that satisfy the condition.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.
	/// </exception>
	/// <exception cref="ArgumentOutOfRangeException">
	///	    <paramref name="offset"/> is below <c>1</c>.</exception>
	/// <remarks>
	/// <para>
	///	    This operator evaluates in a deferred and streaming manner.
	/// </para>
	/// </remarks>
	public static IEnumerable<TSource> WhereLead<TSource>(
		this IEnumerable<TSource> source,
		int offset,
		TSource defaultLeadValue,
		Func<TSource, TSource, bool> predicate
	)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(predicate);
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(offset);

		return Core(source, offset, defaultLeadValue, predicate);

		static IEnumerable<TSource> Core(
			IEnumerable<TSource> source,
			int offset,
			TSource defaultLeadValue,
			Func<TSource, TSource, bool> predicate
		)
		{
			var queue = new Queue<TSource>(offset + 1);

			foreach (var item in source)
			{
				queue.Enqueue(item);
				if (queue.Count > offset)
				{
					var deq = queue.Dequeue();
					if (predicate(deq, item))
						yield return deq;
				}
			}

			while (queue.Count > 0)
			{
				var deq = queue.Dequeue();
				if (predicate(deq, defaultLeadValue))
					yield return deq;
			}
		}
	}
}
