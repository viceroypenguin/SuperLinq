namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Performs a scan (inclusive prefix sum) on a sequence of elements. This operator is similar to <see
	/// cref="Enumerable.Aggregate{TSource}"/> except that <see cref="Scan{TSource}"/> returns the sequence of
	/// intermediate results as well as the final one.
	/// </summary>
	/// <typeparam name="TSource">Type of elements in source sequence</typeparam>
	/// <param name="source">Source sequence</param>
	/// <param name="transformation">Transformation operation</param>
	/// <returns>The scanned sequence</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="transformation"/> is <see
	/// langword="null"/>.</exception>
	/// <remarks>
	/// This operator uses deferred execution and streams its result.
	/// </remarks>
	public static IAsyncEnumerable<TSource> Scan<TSource>(
		this IAsyncEnumerable<TSource> source,
		Func<TSource, TSource, TSource> transformation)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(transformation);

		return Core(source, transformation);

		static async IAsyncEnumerable<TSource> Core(
			IAsyncEnumerable<TSource> source,
			Func<TSource, TSource, TSource> transformation,
			[EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			await using var e = source.GetConfiguredAsyncEnumerator(cancellationToken);

			if (!await e.MoveNextAsync())
				yield break;

			var state = e.Current;
			yield return state;

			while (await e.MoveNextAsync())
			{
				state = transformation(state, e.Current);
				yield return state;
			}
		}
	}

	/// <summary>
	/// Performs a scan (inclusive prefix sum) on a sequence of elements. This operator is similar to <see
	/// cref="Enumerable.Aggregate{TSource, TState}"/> except that <see cref="Scan{TSource,TState}"/> returns the
	/// sequence of intermediate results as well as the final one.
	/// </summary>
	/// <typeparam name="TSource">Type of elements in source sequence</typeparam>
	/// <typeparam name="TState">Type of state</typeparam>
	/// <param name="source">Source sequence</param>
	/// <param name="seed">Initial state to seed</param>
	/// <param name="transformation">Transformation operation</param>
	/// <returns>The scanned sequence</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="transformation"/> is <see
	/// langword="null"/>.</exception>
	/// <remarks>
	/// This operator uses deferred execution and streams its result.
	/// </remarks>
	public static IAsyncEnumerable<TState> Scan<TSource, TState>(
		this IAsyncEnumerable<TSource> source,
		TState seed,
		Func<TState, TSource, TState> transformation)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(transformation);

		return Core(source, seed, transformation);

		static async IAsyncEnumerable<TState> Core(
			IAsyncEnumerable<TSource> source,
			TState state,
			Func<TState, TSource, TState> transformation,
			[EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			yield return state;

			await foreach (var e in source.WithCancellation(cancellationToken).ConfigureAwait(false))
			{
				state = transformation(state, e);
				yield return state;
			}
		}
	}
}
