namespace SuperLinq;

public static partial class SuperEnumerable
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
	///	    This operator uses deferred execution and streams its results. For each duplicated key, the second element
	///     with that key is returned.
	/// </remarks>
	public static IEnumerable<TSource> DuplicatesBy<TSource, TKey>(
		this IEnumerable<TSource> source,
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
	///	    The equality comparer to use to compare keys. If <see langword="null"/>, the default equality comparer for
	///     <typeparamref name="TKey"/> is used.
	/// </param>
	/// <returns>
	///	    One element for each key that occurs more than once.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="keySelector"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	///	    This operator uses deferred execution and streams its results. For each duplicated key, the second element
	///     with that key is returned.
	/// </remarks>
	public static IEnumerable<TSource> DuplicatesBy<TSource, TKey>(
		this IEnumerable<TSource> source,
		Func<TSource, TKey> keySelector,
		IEqualityComparer<TKey>? comparer)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(keySelector);

		return Core(source, keySelector, comparer ?? EqualityComparer<TKey>.Default);

		static IEnumerable<TSource> Core(
			IEnumerable<TSource> source,
			Func<TSource, TKey> keySelector,
			IEqualityComparer<TKey> comparer)
		{
			var counts = new Collections.NullKeyDictionary<TKey, int>(comparer);
			foreach (var element in source)
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
