namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Returns all distinct elements of the given source, where "distinctness" is determined via a projection and
	///     the default equality comparer for the projected type.
	/// </summary>
	/// <typeparam name="TSource">
	///	    Type of the source sequence
	/// </typeparam>
	/// <typeparam name="TKey">
	///	    Type of the projected element
	/// </typeparam>
	/// <param name="source">
	///	    Source sequence
	/// </param>
	/// <param name="keySelector">
	///	    Projection for determining "distinctness"
	/// </param>
	/// <returns>
	///	    A sequence consisting of distinct elements from the source sequence, comparing them by the specified key
	///     projection.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="keySelector"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This operator uses deferred execution and streams the results, although a set of already-seen keys is
	///     retained. If a key is seen multiple times, only the first element with that key is returned.
	/// </para>
	/// <para>
	///		This operator is implemented in the bcl as of net6. Source and binary compatibility should be retained
	///		across net versions, but this method should be inaccessible in net6+.
	/// </para>
	/// </remarks>
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
	///	    Returns all distinct elements of the given source, where "distinctness" is determined via a projection and
	///     the default equality comparer for the projected type.
	/// </summary>
	/// <typeparam name="TSource">
	///	    Type of the source sequence
	/// </typeparam>
	/// <typeparam name="TKey">
	///	    Type of the projected element
	/// </typeparam>
	/// <param name="source">
	///	    Source sequence
	/// </param>
	/// <param name="keySelector">
	///	    Projection for determining "distinctness"
	/// </param>
	/// <param name="comparer">
	///	    The equality comparer to use to determine whether or not keys are equal. If <see langword="null"/>, the
	///     default equality comparer for <typeparamref name="TSource"/> is used.
	/// </param>
	/// <returns>
	///	    A sequence consisting of distinct elements from the source sequence, comparing them by the specified key
	///     projection.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="keySelector"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This operator uses deferred execution and streams the results, although a set of already-seen keys is
	///     retained. If a key is seen multiple times, only the first element with that key is returned.
	/// </para>
	/// <para>
	///	    This operator is implemented in the bcl as of net6. Source and binary compatibility should be retained
	///	    across net versions, though but method should be inaccessible in net6+.
	/// </para>
	/// </remarks>
#if NET6_0_OR_GREATER
	[Obsolete("This method has been implemented by the framework.")]
	public static IEnumerable<TSource> DistinctBy<TSource, TKey>(
		IEnumerable<TSource> source,
		Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer)
#else
	public static IEnumerable<TSource> DistinctBy<TSource, TKey>(
		this IEnumerable<TSource> source,
		Func<TSource, TKey> keySelector,
		IEqualityComparer<TKey>? comparer)
#endif
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(keySelector);

		return Core(source, keySelector, comparer ?? EqualityComparer<TKey>.Default);

		static IEnumerable<TSource> Core(IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
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
