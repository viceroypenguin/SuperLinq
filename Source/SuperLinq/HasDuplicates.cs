namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///     Checks if sequence contains duplicates
	/// </summary>
	/// <param name="source">The sequence to check.</param>
	/// <typeparam name="T">The type of the elements in the source sequence</typeparam>
	/// <returns>
	///     <see langword="true" /> if any element of the sequence <paramref name="source" /> is duplicated,
	///     <see langword="false" /> otherwise
	/// </returns>
	public static bool HasDuplicates<T>(this IEnumerable<T> source)
	{
		return source.HasDuplicates(EqualityComparer<T>.Default);
	}

	/// <summary>
	///     Checks if sequence contains duplicates, using the specified element equality comparer
	/// </summary>
	/// <param name="source">The sequence to check.</param>
	/// <param name="comparer">
	///     The equality comparer to use to determine whether or not keys are equal.
	///     If null, the default equality comparer for <c>TSource</c> is used.
	/// </param>
	/// <typeparam name="T">The type of the elements in the source sequence</typeparam>
	/// <returns>
	///     <see langword="true" /> if any element of the sequence <paramref name="source" /> is duplicated,
	///     <see langword="false" /> otherwise
	/// </returns>
	public static bool HasDuplicates<T>(this IEnumerable<T> source, IEqualityComparer<T>? comparer)
	{
		return source.HasDuplicates(Identity, comparer);
	}

	/// <summary>
	///     Checks if sequence contains duplicates, using the specified element equality comparer
	/// </summary>
	/// <param name="source">The sequence to check.</param>
	/// <param name="keySelector">Projection for determining "distinctness"</param>
	/// <typeparam name="TSource">Type of the source sequence</typeparam>
	/// <typeparam name="TKey">Type of the projected element</typeparam>
	/// <returns>
	///     <see langword="true" /> if any element of the sequence <paramref name="source" /> is duplicated,
	///     <see langword="false" /> otherwise
	/// </returns>
	public static bool HasDuplicates<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
	{
		return source.HasDuplicates(keySelector, EqualityComparer<TKey>.Default);
	}

	/// <summary>
	///     Checks if sequence contains duplicates, using the specified element equality comparer
	/// </summary>
	/// <param name="source">The sequence to check.</param>
	/// <param name="keySelector">Projection for determining "distinctness"</param>
	/// <param name="comparer">
	///     The equality comparer to use to determine whether or not keys are equal.
	///     If null, the default equality comparer for <c>TSource</c> is used.
	/// </param>
	/// <typeparam name="TSource">Type of the source sequence</typeparam>
	/// <typeparam name="TKey">Type of the projected element</typeparam>
	/// <returns>
	///     <see langword="true" /> if any element of the sequence <paramref name="source" /> is duplicated,
	///     <see langword="false" /> otherwise
	/// </returns>
	public static bool HasDuplicates<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector,
		IEqualityComparer<TKey>? comparer)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(keySelector);

		var enumeratedElements = source.TryGetCollectionCount() is { } collectionCount
			? new HashSet<TKey>(collectionCount, comparer)
			: new HashSet<TKey>(comparer);

		foreach (var element in source)
		{
			if (enumeratedElements.Add(keySelector(element)) is false)
			{
				return true;
			}
		}

		return false;
	}
}
