namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Returns a tuple with the cardinality of the sequence and the single element in the sequence if it contains
	///     exactly one element. similar to <see cref="Enumerable.Single{T}(IEnumerable{T})"/>.
	/// </summary>
	/// <typeparam name="T">
	///	    The type of the elements of <paramref name="source"/>.
	/// </typeparam>
	/// <typeparam name="TCardinality">
	///	    The type that expresses cardinality.
	/// </typeparam>
	/// <param name="source">
	///	    The source sequence.
	///	</param>
	/// <param name="zero">
	///	    The value that is returned in the tuple if the sequence has zero elements.
	/// </param>
	/// <param name="one">
	///	    The value that is returned in the tuple if the sequence has a single element only.
	/// </param>
	/// <param name="many">
	///	    The value that is returned in the tuple if the sequence has two or more elements.
	/// </param>
	/// <returns>
	///	    A tuple containing the identified <typeparamref name="TCardinality"/> and either the single value of
	///     <typeparamref name="T"/> in the sequence or its default value.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	///	    This operator uses immediate execution, but never consumes more than two elements from the sequence.
	/// </remarks>
	public static (TCardinality Cardinality, T? Value)
		TrySingle<T, TCardinality>(this IEnumerable<T> source,
			TCardinality zero, TCardinality one, TCardinality many)
	{
		return TrySingle(source, zero, one, many, ValueTuple.Create);
	}

	/// <summary>
	///	    Returns a result projected from the the cardinality of the sequence and the single element in the sequence
	///     if it contains exactly one element.
	/// </summary>
	/// <typeparam name="T">
	///	    The type of the elements of <paramref name="source"/>.
	/// </typeparam>
	/// <typeparam name="TCardinality">
	///	    The type that expresses cardinality.
	/// </typeparam>
	/// <typeparam name="TResult">
	///	    The type of the result value returned by the <paramref name="resultSelector"/> function.
	/// </typeparam>
	/// <param name="source">
	///	    The source sequence.
	/// </param>
	/// <param name="zero">
	///	    The value that is passed as the first argument to <paramref name="resultSelector" /> if the sequence has
	///     zero elements.
	/// </param>
	/// <param name="one">
	///	    The value that is passed as the first argument to <paramref name="resultSelector" /> if the sequence has a
	///     single element only.
	/// </param>
	/// <param name="many">
	///	    The value that is passed as the first argument to <paramref name="resultSelector" /> if the sequence has two
	///     or more elements.
	/// </param>
	/// <param name="resultSelector">
	///	    A function that receives the cardinality and, if the sequence has just one element, the value of that
	///     element as argument and projects a resulting value of type
	/// <typeparamref name="TResult"/>.
	/// </param>
	/// <returns>
	///	    The value returned by <paramref name="resultSelector"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="resultSelector"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	///	    This operator uses immediate execution, but never consumes more than two elements from the sequence.
	/// </remarks>
	public static TResult TrySingle<T, TCardinality, TResult>(
		this IEnumerable<T> source,
		TCardinality zero, TCardinality one, TCardinality many,
		Func<TCardinality, T?, TResult> resultSelector)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(resultSelector);

		if (source.TryGetCollectionCount() is int n)
		{
			return n switch
			{
				0 => resultSelector(zero, default),
				1 => resultSelector(one, source.First()),
				_ => resultSelector(many, default),
			};
		}
		else
		{
			using var e = source.GetEnumerator();
			if (!e.MoveNext())
				return resultSelector(zero, default);

			var current = e.Current;
			return !e.MoveNext()
				? resultSelector(one, current)
				: resultSelector(many, default);
		}
	}

	/// <summary>
	///	    Returns a single value if the sequence is 1 element long, otherwise returns default.
	/// </summary>
	/// <typeparam name="TSource">
	///	    The type of the elements of <paramref name="source"/>.
	/// </typeparam>
	/// <returns>
	///	    The value returned by <paramref name="source"/>.
	/// </returns>
	/// 	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	///	    Alternatively, you can use source.SingleOrDefault();
	/// </remarks>
	public static TSource? TrySingle<TSource>(this IEnumerable<TSource> source)
	{
		return source.SingleOrDefault();
	}
}
