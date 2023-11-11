namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Performs a scan (inclusive prefix sum) on a sequence of elements. This operator is similar to <see
	///     cref="Enumerable.Aggregate{TSource}"/> except that <see cref="Scan{TSource}"/> returns the sequence of
	///     intermediate results as well as the final one.
	/// </summary>
	/// <typeparam name="TSource">
	///	    Type of elements in source sequence
	/// </typeparam>
	/// <param name="source">
	///	    Source sequence
	/// </param>
	/// <param name="transformation">
	///	    Transformation operation
	/// </param>
	/// <returns>
	///	    The scanned sequence
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="transformation"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This operator uses deferred execution and streams its result.
	/// </para>
	/// </remarks>
	public static IEnumerable<TSource> Scan<TSource>(
		this IEnumerable<TSource> source,
		Func<TSource, TSource, TSource> transformation)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(transformation);

		return Core(source, transformation);

		static IEnumerable<TSource> Core(IEnumerable<TSource> source, Func<TSource, TSource, TSource> transformation)
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
	///	    Performs a scan (inclusive prefix sum) on a sequence of elements. This operator is similar to <see
	///     cref="Enumerable.Aggregate{TSource}"/> except that <see cref="Scan{TSource}"/> returns the sequence of
	///     intermediate results as well as the final one.
	/// </summary>
	/// <typeparam name="TSource">
	///	    Type of elements in source sequence
	/// </typeparam>
	/// <param name="source">
	///	    Source sequence
	/// </param>
	/// <param name="transformation">
	///	    Transformation operation
	/// </param>
	/// <returns>
	///	    The scanned sequence
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="transformation"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This operator uses deferred execution and streams its result.
	/// </para>
	/// </remarks>
	[Obsolete("Method renamed back to `Scan`.")]
	public static IEnumerable<TSource> ScanEx<TSource>(
		this IEnumerable<TSource> source,
		Func<TSource, TSource, TSource> transformation)
	{
		return Scan(source, transformation);
	}

	/// <summary>
	///	    Performs a scan (inclusive prefix sum) on a sequence of elements. This operator is similar to <see
	///     cref="Enumerable.Aggregate{TSource,TState}"/> except that <see cref="Scan{TSource,TState}"/> returns the sequence of
	///     intermediate results as well as the final one.
	/// </summary>
	/// <typeparam name="TSource">
	///	    Type of elements in source sequence
	/// </typeparam>
	/// <typeparam name="TState">
	///		Type of state
	/// </typeparam>
	/// <param name="source">
	///	    Source sequence
	/// </param>
	/// <param name="seed">
	///		Initial state to seed
	/// </param>
	/// <param name="transformation">
	///	    Transformation operation
	/// </param>
	/// <returns>
	///	    The scanned sequence
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="transformation"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This operator uses deferred execution and streams its result.
	/// </para>
	/// </remarks>
	public static IEnumerable<TState> Scan<TSource, TState>(
		this IEnumerable<TSource> source,
		TState seed,
		Func<TState, TSource, TState> transformation)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(transformation);

		return Core(source, seed, transformation);

		static IEnumerable<TState> Core(
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

	/// <summary>
	///	    Performs a scan (inclusive prefix sum) on a sequence of elements. This operator is similar to <see
	///     cref="Enumerable.Aggregate{TSource,TState}"/> except that <see cref="Scan{TSource,TState}"/> returns the sequence of
	///     intermediate results as well as the final one.
	/// </summary>
	/// <typeparam name="TSource">
	///	    Type of elements in source sequence
	/// </typeparam>
	/// <typeparam name="TState">
	///		Type of state
	/// </typeparam>
	/// <param name="source">
	///	    Source sequence
	/// </param>
	/// <param name="seed">
	///		Initial state to seed
	/// </param>
	/// <param name="transformation">
	///	    Transformation operation
	/// </param>
	/// <returns>
	///	    The scanned sequence
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="transformation"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This operator uses deferred execution and streams its result.
	/// </para>
	/// </remarks>
	[Obsolete("Method renamed back to `Scan`.")]
	public static IEnumerable<TState> ScanEx<TSource, TState>(
	this IEnumerable<TSource> source,
	TState seed,
	Func<TState, TSource, TState> transformation)
	{
		return Scan(source, seed, transformation);
	}
}
