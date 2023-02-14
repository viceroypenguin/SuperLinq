namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Returns a tuple with the cardinality of the sequence and the
	/// single element in the sequence if it contains exactly one element.
	/// similar to <see cref="Enumerable.Single{T}(IEnumerable{T})"/>.
	/// </summary>
	/// <param name="source">The source sequence.</param>
	/// <param name="zero">
	/// The value that is returned in the tuple if the sequence has zero
	/// elements.</param>
	/// <param name="one">
	/// The value that is returned in the tuple if the sequence has a
	/// single element only.</param>
	/// <param name="many">
	/// The value that is returned in the tuple if the sequence has two or
	/// more elements.</param>
	/// <typeparam name="T">
	/// The type of the elements of <paramref name="source"/>.</typeparam>
	/// <typeparam name="TCardinality">
	/// The type that expresses cardinality.</typeparam>
	/// <returns>
	/// A tuple containing the identified <typeparamref name="TCardinality"/>
	/// and either the single value of <typeparamref name="T"/> in the sequence
	/// or its default value.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
	/// <remarks>
	/// This operator uses immediate execution, but never consumes more
	/// than two elements from the sequence.
	/// </remarks>
	public static (TCardinality Cardinality, T? Value)
		TrySingle<T, TCardinality>(this IEnumerable<T> source,
			TCardinality zero, TCardinality one, TCardinality many)
	{
		return TrySingle(source, zero, one, many, ValueTuple.Create);
	}

	/// <summary>
	/// Returns a result projected from the the cardinality of the sequence
	/// and the single element in the sequence if it contains exactly one
	/// element.
	/// </summary>
	/// <param name="source">The source sequence.</param>
	/// <param name="zero">
	/// The value that is passed as the first argument to
	/// <paramref name="resultSelector" /> if the sequence has zero
	/// elements.</param>
	/// <param name="one">
	/// The value that is passed as the first argument to
	/// <paramref name="resultSelector" /> if the sequence has a
	/// single element only.</param>
	/// <param name="many">
	/// The value that is passed as the first argument to
	/// <paramref name="resultSelector" /> if the sequence has two or
	/// more elements.</param>
	/// <param name="resultSelector">
	/// A function that receives the cardinality and, if the
	/// sequence has just one element, the value of that element as
	/// argument and projects a resulting value of type
	/// <typeparamref name="TResult"/>.</param>
	/// <typeparam name="T">
	/// The type of the elements of <paramref name="source"/>.</typeparam>
	/// <typeparam name="TCardinality">
	/// The type that expresses cardinality.</typeparam>
	/// <typeparam name="TResult">
	/// The type of the result value returned by the
	/// <paramref name="resultSelector"/> function. </typeparam>
	/// <returns>
	/// The value returned by <paramref name="resultSelector"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
	/// <exception cref="ArgumentNullException"><paramref name="resultSelector"/> is null</exception>
	/// <remarks>
	/// This operator uses immediate execution, but never consumes more
	/// than two elements from the sequence.
	/// </remarks>

	public static TResult TrySingle<T, TCardinality, TResult>(
		this IEnumerable<T> source,
		TCardinality zero, TCardinality one, TCardinality many,
		Func<TCardinality, T?, TResult> resultSelector)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(resultSelector);

		switch (source.TryGetCollectionCount())
		{
			case 0:
				return resultSelector(zero, default);
			case 1:
			{
				var item = source switch
				{
					IList<T> list => list[0],
					_ => source.First(),
				};
				return resultSelector(one, item);
			}
			case > 1:
				return resultSelector(many, default);

			default:
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
	}
}
