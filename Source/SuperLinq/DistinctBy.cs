namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Returns all distinct elements of the given source, where "distinctness"
	/// is determined via a projection and the default equality comparer for the projected type.
	/// </summary>
	/// <remarks>
	/// This operator uses deferred execution and streams the results, although
	/// a set of already-seen keys is retained. If a key is seen multiple times,
	/// only the first element with that key is returned.
	/// </remarks>
	/// <typeparam name="TSource">Type of the source sequence</typeparam>
	/// <typeparam name="TKey">Type of the projected element</typeparam>
	/// <param name="source">Source sequence</param>
	/// <param name="keySelector">Projection for determining "distinctness"</param>
	/// <returns>A sequence consisting of distinct elements from the source sequence,
	/// comparing them by the specified key projection.</returns>
#if NET6_0_OR_GREATER
	[Obsolete("This method has been implemented by the framework.")]
	public static IEnumerable<TSource> DistinctBy<TSource, TKey>(
		IEnumerable<TSource> source,
		Func<TSource, TKey> keySelector)
#else
	public static IEnumerable<TSource> DistinctBy<TSource, TKey>(
		this IEnumerable<TSource> source,
		Func<TSource, TKey> keySelector)
#endif
	{
		return DistinctBy(source, keySelector, comparer: default);
	}

	/// <summary>
	/// Returns all distinct elements of the given source, where "distinctness"
	/// is determined via a projection and the specified comparer for the projected type.
	/// </summary>
	/// <remarks>
	/// This operator uses deferred execution and streams the results, although
	/// a set of already-seen keys is retained. If a key is seen multiple times,
	/// only the first element with that key is returned.
	/// </remarks>
	/// <typeparam name="TSource">Type of the source sequence</typeparam>
	/// <typeparam name="TKey">Type of the projected element</typeparam>
	/// <param name="source">Source sequence</param>
	/// <param name="keySelector">Projection for determining "distinctness"</param>
	/// <param name="comparer">The equality comparer to use to determine whether or not keys are equal.
	/// If null, the default equality comparer for <c>TSource</c> is used.</param>
	/// <returns>A sequence consisting of distinct elements from the source sequence,
	/// comparing them by the specified key projection.</returns>
#if NET6_0_OR_GREATER
	[Obsolete("This method has been implemented by the framework.")]
	public static IEnumerable<TSource> DistinctBy<TSource, TKey>(
		IEnumerable<TSource> source,
		Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer)
#else
	public static IEnumerable<TSource> DistinctBy<TSource, TKey>(
		this IEnumerable<TSource> source,
		Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer)
#endif
	{
		if (source == null) throw new ArgumentNullException(nameof(source));
		if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));

		return _(); IEnumerable<TSource> _()
		{
			var knownKeys = new HashSet<TKey>(comparer);
			foreach (var element in source)
			{
				if (knownKeys.Add(keySelector(element)))
					yield return element;
			}
		}
	}
}
