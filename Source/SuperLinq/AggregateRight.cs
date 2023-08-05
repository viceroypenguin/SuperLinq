namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Applies a right-associative accumulator function over a sequence. This operator is the right-associative
	///	    version of the <see cref="Enumerable.Aggregate{TSource}(IEnumerable{TSource}, Func{TSource, TSource,
	///	    TSource})"/> LINQ operator.
	/// </summary>
	/// <typeparam name="TSource">
	///	    The type of the elements of source.
	///	</typeparam>
	/// <param name="source">
	///	    Source sequence.
	///	</param>
	/// <param name="func">
	///	    A right-associative accumulator function to be invoked on each element.
	///	</param>
	/// <returns>
	///	    The final accumulator value.
	///	</returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source" /> or <paramref name="func" /> is <see langword="null" />.
	///	</exception>
	/// <remarks>
	///	    This operator executes immediately.
	/// </remarks>
	public static TSource AggregateRight<TSource>(this IEnumerable<TSource> source, Func<TSource, TSource, TSource> func)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(func);

		var list = source is IList<TSource> l ? l : source.ToList();

		if (list.Count == 0)
			ThrowHelper.ThrowInvalidOperationException("Sequence contains no elements");

		var seed = list[^1];

		for (var i = list.Count - 2; i >= 0; i--)
			seed = func(list[i], seed);

		return seed;
	}

	/// <summary>
	///	    Applies a right-associative accumulator function over a sequence. The specified <paramref name="seed"/>
	///     value is used as the initial accumulator value. This operator is the right-associative version of the <see
	///     cref="Enumerable.Aggregate{TSource, TAccumulate}(IEnumerable{TSource}, TAccumulate, Func{TAccumulate,
	///     TSource, TAccumulate})"/> LINQ operator.
	/// </summary>
	/// <typeparam name="TSource">
	///	    The type of the elements of source.
	///	</typeparam>
	/// <typeparam name="TAccumulate">
	///	    The type of the accumulator value.
	///	    </typeparam>
	/// <param name="source">
	///	    Source sequence.
	///	</param>
	/// <param name="seed">
	///		The initial accumulator value.
	///	</param>
	/// <param name="func">
	///	    A right-associative accumulator function to be invoked on each element.
	///	</param>
	/// <returns>
	///	    The final accumulator value.
	///	</returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source" /> or <paramref name="func" /> is <see langword="null" />.
	///	</exception>
	/// <remarks>
	///	    This operator executes immediately.
	/// </remarks>
	public static TAccumulate AggregateRight<TSource, TAccumulate>(this IEnumerable<TSource> source, TAccumulate seed, Func<TSource, TAccumulate, TAccumulate> func)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(func);

		var list = source is IList<TSource> l ? l : source.ToList();

		for (var i = list.Count - 1; i >= 0; i--)
			seed = func(list[i], seed);

		return seed;
	}

	/// <summary>
	///	    Applies a right-associative accumulator function over a sequence. The specified <paramref name="seed"/>
	///     value is used as the initial accumulator value, and the <paramref name="resultSelector"/> function is used
	///     to select the result value. This operator is the right-associative version of the <see
	///     cref="Enumerable.Aggregate{TSource, TAccumulate, TResult}(IEnumerable{TSource}, TAccumulate,
	///     Func{TAccumulate, TSource, TAccumulate}, Func{TAccumulate, TResult})"/> LINQ operator.
	/// </summary>
	/// <typeparam name="TSource">
	///	    The type of the elements of source.
	///	</typeparam>
	/// <typeparam name="TAccumulate">
	///	    The type of the accumulator value.
	///	    </typeparam>
	/// <typeparam name="TResult">
	///	    The type of the resulting value.
	///	</typeparam>
	/// <param name="source">
	///	    Source sequence.
	///	</param>
	/// <param name="seed">
	///	    The initial accumulator value.
	///	</param>
	/// <param name="func">
	///	    A right-associative accumulator function to be invoked on each element.
	///	</param>
	/// <param name="resultSelector">
	///	    A function to transform the final accumulator value into the result value.
	///	</param>
	/// <returns>
	///	    The transformed final accumulator value.
	///	</returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source" />, <paramref name="func" />, or <paramref name="resultSelector"/> is <see
	///     langword="null" />.
	///	</exception>
	/// <remarks>
	///	    This operator executes immediately.
	/// </remarks>
	public static TResult AggregateRight<TSource, TAccumulate, TResult>(this IEnumerable<TSource> source, TAccumulate seed, Func<TSource, TAccumulate, TAccumulate> func, Func<TAccumulate, TResult> resultSelector)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(func);
		Guard.IsNotNull(resultSelector);

		return resultSelector(source.AggregateRight(seed, func));
	}
}
