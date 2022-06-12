namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Returns the minimal elements of the given sequence, based on
	/// the given projection.
	/// </summary>
	/// <remarks>
	/// This overload uses the default comparer for the projected type.
	/// This operator uses deferred execution. The results are evaluated
	/// and cached on first use to returned sequence.
	/// </remarks>
	/// <typeparam name="TSource">Type of the source sequence</typeparam>
	/// <typeparam name="TKey">Type of the projected element</typeparam>
	/// <param name="source">Source sequence</param>
	/// <param name="selector">Selector to use to pick the results to compare</param>
	/// <returns>The sequence of minimal elements, according to the projection.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null</exception>

	public static IExtremaEnumerable<TSource> MinBy<TSource, TKey>(this IEnumerable<TSource> source,
		Func<TSource, TKey> selector)
	{
		return source.MinBy(selector, null);
	}

	/// <summary>
	/// Returns the minimal elements of the given sequence, based on
	/// the given projection and the specified comparer for projected values.
	/// </summary>
	/// <remarks>
	/// This operator uses deferred execution. The results are evaluated
	/// and cached on first use to returned sequence.
	/// </remarks>
	/// <typeparam name="TSource">Type of the source sequence</typeparam>
	/// <typeparam name="TKey">Type of the projected element</typeparam>
	/// <param name="source">Source sequence</param>
	/// <param name="selector">Selector to use to pick the results to compare</param>
	/// <param name="comparer">Comparer to use to compare projected values</param>
	/// <returns>The sequence of minimal elements, according to the projection.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/>, <paramref name="selector"/>
	/// or <paramref name="comparer"/> is null</exception>

	public static IExtremaEnumerable<TSource> MinBy<TSource, TKey>(this IEnumerable<TSource> source,
		Func<TSource, TKey> selector, IComparer<TKey>? comparer)
	{
		if (source == null) throw new ArgumentNullException(nameof(source));
		if (selector == null) throw new ArgumentNullException(nameof(selector));

		comparer ??= Comparer<TKey>.Default;
		return new ExtremaEnumerable<TSource, TKey>(source, selector, (x, y) => -Math.Sign(comparer.Compare(x, y)));
	}
}
