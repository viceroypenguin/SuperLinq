namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	///	    Returns the sequence of elements whose keys occur in the source sequence more than once.
	/// </summary>
	/// <typeparam name="TSource">
	///	    The type of the elements in the source sequence.
	/// </typeparam>
	/// <typeparam name="TKey">
	///	    The type of the key used to identify duplicates.
	/// </typeparam>
	/// <param name="source">
	///	    The source sequence.
	/// </param>
	/// <param name="keySelector">
	///	    A function to extract the key for each element.
	/// </param>
	/// <returns>
	///	    One element for each key that occurs more than once.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="keySelector"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	///	    This operator uses deferred execution and streams its results.
	/// </remarks>
	public static IAsyncEnumerable<TSource> DuplicatesBy<TSource, TKey>(
		this IAsyncEnumerable<TSource> source,
		Func<TSource, TKey> keySelector)
	{
		return source.DuplicatesBy(keySelector, comparer: null);
	}

	/// <summary>
	///	    Returns the sequence of elements whose keys occur in the source sequence more than once.
	/// </summary>
	/// <typeparam name="TSource">
	///	    The type of the elements in the source sequence.
	/// </typeparam>
	/// <typeparam name="TKey">
	///	    The type of the key used to identify duplicates.
	/// </typeparam>
	/// <param name="source">
	///	    The source sequence.
	/// </param>
	/// <param name="keySelector">
	///	    A function to extract the key for each element.
	/// </param>
	/// <param name="comparer">
	///	    The equality comparer used to compare keys, or <see langword="null"/> to use the default.
	/// </param>
	/// <returns>
	///	    One element for each key that occurs more than once.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="keySelector"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	///	    This operator uses deferred execution and streams its results.
	/// </remarks>
	public static IAsyncEnumerable<TSource> DuplicatesBy<TSource, TKey>(
		this IAsyncEnumerable<TSource> source,
		Func<TSource, TKey> keySelector,
		IEqualityComparer<TKey>? comparer)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(keySelector);

		return Core(source, keySelector, comparer ?? EqualityComparer<TKey>.Default);

		static async IAsyncEnumerable<TSource> Core(
			IAsyncEnumerable<TSource> source,
			Func<TSource, TKey> keySelector,
			IEqualityComparer<TKey> comparer,
			[EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			var counts = new Collections.NullKeyDictionary<TKey, int>(comparer);
			await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
			{
				var key = keySelector(element);
				if (!counts.TryGetValue(key, out var count))
				{
					counts[key] = 1;
				}
				else if (count == 1)
				{
					yield return element;
					counts[key] = 2;
				}
			}
		}
	}
}
