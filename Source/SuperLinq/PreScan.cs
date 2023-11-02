namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Performs a pre-scan (exclusive prefix sum) on a sequence of elements.
	/// </summary>
	/// <typeparam name="TSource">
	///	    Type of elements in source sequence
	/// </typeparam>
	/// <param name="source">
	///	    Source sequence
	/// </param>
	/// <param name="transformation">
	///	    An accumulator function to be invoked on each element.
	/// </param>
	/// <param name="identity">
	///	    The initial accumulator value.
	/// </param>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="transformation"/> is <see langword="null"/>.
	/// </exception>
	/// <returns>
	///	    The scanned sequence
	/// </returns>
	/// <remarks>
	/// <para>
	///	    An exclusive prefix scan returns an equal-length sequence where the N-th element is the aggregation of the
	///     first N-1 input elements, where the first element is simply the <paramref name="identity"/> value.
	/// </para>
	/// <para>
	///	    The inclusive version of PreScan is <see cref="Scan{TSource}"/>.
	/// </para>
	/// <para>
	///	    This operator uses deferred execution and streams its result.
	/// </para>
	/// </remarks>
	public static IEnumerable<TSource> PreScan<TSource>(
		this IEnumerable<TSource> source,
		Func<TSource, TSource, TSource> transformation,
		TSource identity)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(transformation);

		return Core(source, transformation, identity);

		static IEnumerable<TSource> Core(IEnumerable<TSource> source, Func<TSource, TSource, TSource> transformation, TSource identity)
		{
			var aggregator = identity;
			using var e = source.GetEnumerator();

			if (!e.MoveNext())
				yield break;

			yield return aggregator;
			var current = e.Current;

			while (e.MoveNext())
			{
				aggregator = transformation(aggregator, current);
				yield return aggregator;
				current = e.Current;
			}
		}
	}
}
