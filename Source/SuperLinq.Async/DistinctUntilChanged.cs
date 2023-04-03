namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Returns consecutive distinct elements by using the default equality comparer to compare values.
	/// </summary>
	/// <typeparam name="TSource">Source sequence element type.</typeparam>
	/// <param name="source">Source sequence.</param>
	/// <returns>Sequence without adjacent non-distinct elements.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	public static IAsyncEnumerable<TSource> DistinctUntilChanged<TSource>(this IAsyncEnumerable<TSource> source)
	{
		return DistinctUntilChanged(source, Identity, comparer: null);
	}

	/// <summary>
	/// Returns consecutive distinct elements by using the specified equality comparer to compare values.
	/// </summary>
	/// <typeparam name="TSource">Source sequence element type.</typeparam>
	/// <param name="source">Source sequence.</param>
	/// <param name="comparer">Comparer used to compare values.</param>
	/// <returns>Sequence without adjacent non-distinct elements.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	public static IAsyncEnumerable<TSource> DistinctUntilChanged<TSource>(this IAsyncEnumerable<TSource> source, IEqualityComparer<TSource>? comparer)
	{
		Guard.IsNotNull(source);
		return DistinctUntilChanged(source, Identity, comparer);
	}

	/// <summary>
	/// Returns consecutive distinct elements based on a key value by using the specified equality comparer to compare
	/// key values.
	/// </summary>
	/// <typeparam name="TSource">Source sequence element type.</typeparam>
	/// <typeparam name="TKey">Key type.</typeparam>
	/// <param name="source">Source sequence.</param>
	/// <param name="keySelector">Key selector.</param>
	/// <returns>Sequence without adjacent non-distinct elements.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> is <see
	/// langword="null"/>.</exception>
	public static IAsyncEnumerable<TSource> DistinctUntilChanged<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector)
	{
		Guard.IsNotNull(keySelector);
		return DistinctUntilChanged<TSource, TKey>(source, keySelector.ToAsync(), comparer: null);
	}

	/// <summary>
	/// Returns consecutive distinct elements based on a key value by using the specified equality comparer to compare
	/// key values.
	/// </summary>
	/// <typeparam name="TSource">Source sequence element type.</typeparam>
	/// <typeparam name="TKey">Key type.</typeparam>
	/// <param name="source">Source sequence.</param>
	/// <param name="keySelector">Key selector.</param>
	/// <returns>Sequence without adjacent non-distinct elements.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> is <see
	/// langword="null"/>.</exception>
	public static IAsyncEnumerable<TSource> DistinctUntilChanged<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector)
	{
		return DistinctUntilChanged<TSource, TKey>(source, keySelector, comparer: null);
	}

	/// <summary>
	/// Returns consecutive distinct elements based on a key value by using the specified equality comparer to compare key values.
	/// </summary>
	/// <typeparam name="TSource">Source sequence element type.</typeparam>
	/// <typeparam name="TKey">Key type.</typeparam>
	/// <param name="source">Source sequence.</param>
	/// <param name="keySelector">Key selector.</param>
	/// <param name="comparer">Comparer used to compare key values.</param>
	/// <returns>Sequence without adjacent non-distinct elements.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> is <see
	/// langword="null"/>.</exception>
	public static IAsyncEnumerable<TSource> DistinctUntilChanged<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer)
	{
		Guard.IsNotNull(keySelector);
		return DistinctUntilChanged(source, keySelector.ToAsync(), comparer);
	}

	/// <summary>
	/// Returns consecutive distinct elements based on a key value by using the specified equality comparer to compare key values.
	/// </summary>
	/// <typeparam name="TSource">Source sequence element type.</typeparam>
	/// <typeparam name="TKey">Key type.</typeparam>
	/// <param name="source">Source sequence.</param>
	/// <param name="keySelector">Key selector.</param>
	/// <param name="comparer">Comparer used to compare key values.</param>
	/// <returns>Sequence without adjacent non-distinct elements.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> is <see
	/// langword="null"/>.</exception>
	public static IAsyncEnumerable<TSource> DistinctUntilChanged<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, IEqualityComparer<TKey>? comparer)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(keySelector);

		return Core(source, keySelector, comparer ?? EqualityComparer<TKey>.Default);

		static async IAsyncEnumerable<TSource> Core(
			IAsyncEnumerable<TSource> source,
			Func<TSource, ValueTask<TKey>> keySelector,
			IEqualityComparer<TKey> comparer,
			[EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			await using var e = source.GetConfiguredAsyncEnumerator(cancellationToken);
			if (!await e.MoveNextAsync())
				yield break;

			yield return e.Current;
			var lastKey = await keySelector(e.Current).ConfigureAwait(false);

			while (await e.MoveNextAsync())
			{
				var nextKey = await keySelector(e.Current).ConfigureAwait(false);
				if (!comparer.Equals(lastKey, nextKey))
				{
					yield return e.Current;
					lastKey = nextKey;
				}
			}
		}
	}
}
