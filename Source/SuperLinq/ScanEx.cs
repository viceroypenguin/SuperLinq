namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Performs a scan (inclusive prefix sum) on a sequence of elements.
	/// This operator is similar to <see cref="Enumerable.Aggregate{TSource}"/>
	/// except that <see cref="ScanEx{TSource}"/> 
	/// returns the sequence of intermediate results as well as the final one.
	/// </summary>
	/// <typeparam name="TSource">Type of elements in source sequence</typeparam>
	/// <param name="source">Source sequence</param>
	/// <param name="transformation">Transformation operation</param>
	/// <returns>The scanned sequence</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
	/// <exception cref="ArgumentNullException"><paramref name="transformation"/> is null</exception>
	/// <remarks>
	/// <para>
	/// This operator returns the first element in <paramref name="source"/>,
	/// while <see cref="EnumerableEx.Scan{TSource}"/> skips the first element.
	/// </para>
	/// <para>
	/// This operator uses deferred execution and streams its result.
	/// </para>
	/// </remarks>
	public static IEnumerable<TSource> ScanEx<TSource>(
		this IEnumerable<TSource> source,
		Func<TSource, TSource, TSource> transformation)
	{
		source.ThrowIfNull();
		transformation.ThrowIfNull();

		return _(source, transformation);

		static IEnumerable<TSource> _(IEnumerable<TSource> source, Func<TSource, TSource, TSource> transformation)
		{
			using var e = source.GetEnumerator();

			if (!e.MoveNext())
				yield break;

			var state = e.Current;
			yield return state;

			while (e.MoveNext())
			{
				state = transformation(state, e.Current);
				yield return state;
			}
		}
	}

	/// <summary>
	/// Performs a scan (inclusive prefix sum) on a sequence of elements.
	/// This operator is similar to <see cref="Enumerable.Aggregate{TSource, TState}"/>
	/// except that <see cref="ScanEx{TSource, TState}"/> 
	/// returns the sequence of intermediate results as well as the final one.
	/// </summary>
	/// <typeparam name="TSource">Type of elements in source sequence</typeparam>
	/// <typeparam name="TState">Type of state</typeparam>
	/// <param name="source">Source sequence</param>
	/// <param name="seed">Initial state to seed</param>
	/// <param name="transformation">Transformation operation</param>
	/// <returns>The scanned sequence</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
	/// <exception cref="ArgumentNullException"><paramref name="transformation"/> is null</exception>
	/// <remarks>
	/// This operator uses deferred execution and streams its result.
	/// </remarks>
	public static IEnumerable<TState> ScanEx<TSource, TState>(
		this IEnumerable<TSource> source,
		TState seed,
		Func<TState, TSource, TState> transformation)
	{
		source.ThrowIfNull();
		transformation.ThrowIfNull();

		return _(source, seed, transformation);

		static IEnumerable<TState> _(
			IEnumerable<TSource> source,
			TState state,
			Func<TState, TSource, TState> transformation)
		{
			yield return state;

			foreach (var e in source)
			{
				state = transformation(state, e);
				yield return state;
			}
		}
	}
}
