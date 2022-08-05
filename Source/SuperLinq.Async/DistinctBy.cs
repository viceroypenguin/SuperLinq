namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
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
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="keySelector"/> is <see langword="null"/>.</exception>
	public static IAsyncEnumerable<TSource> DistinctBy<TSource, TKey>(
		this IAsyncEnumerable<TSource> source,
		Func<TSource, TKey> keySelector)
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
	/// If null, the default equality comparer for <typeparamref name="TSource"/> is used.</param>
	/// <returns>A sequence consisting of distinct elements from the source sequence,
	/// comparing them by the specified key projection.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="keySelector"/> is <see langword="null"/>.</exception>
	public static IAsyncEnumerable<TSource> DistinctBy<TSource, TKey>(
		this IAsyncEnumerable<TSource> source,
		Func<TSource, TKey> keySelector,
		IEqualityComparer<TKey>? comparer)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(keySelector);

		return _(source, keySelector, comparer ?? EqualityComparer<TKey>.Default);

		static async IAsyncEnumerable<TSource> _(IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer, [EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			var knownKeys = new HashSet<TKey>(comparer);
			await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
			{
				if (knownKeys.Add(keySelector(element)))
					yield return element;
			}
		}
	}
}
