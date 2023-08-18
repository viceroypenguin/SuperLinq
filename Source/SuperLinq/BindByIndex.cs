namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Selects elements by index from a sequence.
	/// </summary>
	/// <typeparam name="TSource">
	///	    The type of the elements of <paramref name="source"/>.
	///	</typeparam>
	/// <param name="source">
	///	    The source sequence.
	///	</param>
	/// <param name="indices">
	///	    The list of indices of elements in the <paramref name="source"/> sequence to select.
	///	</param>
	/// <returns>
	///	    An <see cref="IEnumerable{T}"/> whose elements are the result of selecting elements according to the
	///     <paramref name="indices"/> sequence.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="indices"/> is <see langword="null"/>.
	///	</exception>
	/// <exception cref="ArgumentOutOfRangeException">
	///	    (Thrown lazily) An index in <paramref name="indices"/> is out of range for the input sequence <paramref
	///     name="source"/>.
	///	</exception>
	///	<remarks>
	///	<para>
	///	    This operator uses deferred execution and streams its results.
	///	</para>
	///	</remarks>
	public static IEnumerable<TSource> BindByIndex<TSource>(
		this IEnumerable<TSource> source,
		IEnumerable<int> indices)
	{
		return BindByIndex(source, indices, static (e, i) => e, static i => throw new ArgumentOutOfRangeException(nameof(indices), "Index is greater than the length of the first sequence."));
	}

	/// <summary>
	///	    Selects elements by index from a sequence and transforms them using the provided functions.
	/// </summary>
	/// <typeparam name="TSource">
	///	    The type of the elements of <paramref name="source"/>.
	///	</typeparam>
	/// <typeparam name="TResult">
	///	    The type of the elements of the resulting sequence.
	///	</typeparam>
	/// <param name="source">
	///	    The source sequence.
	///	</param>
	/// <param name="indices">
	///	    The list of indices of elements in the <paramref name="source"/> sequence to select.
	///	</param>
	/// <param name="resultSelector">
	///	    A transform function to apply to each source element; the second parameter of the function represents the
	///     index of the output sequence.
	///	</param>
	/// <param name="missingSelector">
	///	    A transform function to apply to missing source elements; the parameter represents the index of the output
	///     sequence.
	///	</param>
	/// <returns>
	///	    An <see cref="IEnumerable{T}"/> whose elements are the result of selecting elements according to the
	///     <paramref name="indices"/> sequence and invoking the transform function.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/>, <paramref name="indices"/>, <paramref name="resultSelector"/>, or <paramref
	///     name="missingSelector"/> is <see langword="null"/>.
	/// </exception>
	///	<remarks>
	///	<para>
	///	    This operator uses deferred execution and streams its results.
	///	</para>
	///	</remarks>
	public static IEnumerable<TResult> BindByIndex<TSource, TResult>(
		this IEnumerable<TSource> source,
		IEnumerable<int> indices,
		Func<TSource, int, TResult> resultSelector,
		Func<int, TResult> missingSelector)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(indices);
		Guard.IsNotNull(resultSelector);
		Guard.IsNotNull(missingSelector);

		return Core(source, indices, resultSelector, missingSelector);

		static IEnumerable<TResult> Core(IEnumerable<TSource> source, IEnumerable<int> indices, Func<TSource, int, TResult> resultSelector, Func<int, TResult> missingSelector)
		{
			// keeps track of the order of indices to know what order items should be output in
			var lookup = indices.Index().ToDictionary(x => { Guard.IsGreaterThanOrEqualTo(x.item, 0, nameof(indices)); return x.item; }, x => x.index);
			// keep track of items out of output order
			var lookback = new Dictionary<int, TSource>();

			// which input index are we on?
			var index = 0;
			// which output index are we on?
			var outputIndex = 0;

			// for each item in input
			foreach (var item in source)
			{
				// does the current input index have an output?
				if (lookup.TryGetValue(index, out var oi))
				{
					// is the current item's output order the next one?
					if (oi == outputIndex)
					{
						// return the item and increment output order
						yield return resultSelector(item, outputIndex);
						outputIndex++;

						// while we're here, catch up on any lookbacks
						while (lookback.TryGetValue(outputIndex, out var e))
						{
							yield return resultSelector(e, outputIndex);
							_ = lookback.Remove(outputIndex);
							outputIndex++;
						}
					}
					// otherwise, store in lookback for later
					else
					{
						lookback[oi] = item;
					}
				}

				index++;
			}

			// catch up any remaining items
			while (outputIndex < lookup.Count)
			{
				// can we find the current output index in lookback?
				if (lookback.TryGetValue(outputIndex, out var e))
				{
					// return it
					yield return resultSelector(e, outputIndex);
					_ = lookback.Remove(outputIndex);
				}
				else
				{
					// otherwise, return a missing item
					yield return missingSelector(outputIndex);
				}

				outputIndex++;
			}
		}
	}
}
