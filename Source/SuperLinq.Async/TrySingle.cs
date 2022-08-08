namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Returns a tuple with the cardinality of the sequence and the
	/// single element in the sequence if it contains exactly one element.
	/// similar to <see cref="AsyncEnumerable.SingleAsync{TSource}(IAsyncEnumerable{TSource}, CancellationToken)"/>.
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
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
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
	public static ValueTask<(TCardinality Cardinality, T? Value)>
		TrySingle<T, TCardinality>(
			this IAsyncEnumerable<T> source,
			TCardinality zero, TCardinality one, TCardinality many,
			CancellationToken cancellationToken = default)
	{
		return TrySingle(source, zero, one, many, ValueTuple.Create, cancellationToken);
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
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
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
	public static async ValueTask<TResult> TrySingle<T, TCardinality, TResult>(
		this IAsyncEnumerable<T> source,
		TCardinality zero, TCardinality one, TCardinality many,
		Func<TCardinality, T?, TResult> resultSelector,
		CancellationToken cancellationToken = default)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(resultSelector);

		await using var e = source.GetConfiguredAsyncEnumerator(cancellationToken);
		if (!await e.MoveNextAsync())
			return resultSelector(zero, default);

		var current = e.Current;
		return !await e.MoveNextAsync()
			? resultSelector(one, current)
			: resultSelector(many, default);
	}
}
