namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///   Checks if sequence contains duplicates
	/// </summary>
	/// <param name="source">The sequence to check.</param>
	/// <typeparam name="T">The type of the elements in the source sequence</typeparam>
	/// <returns>
	/// <see langword="true"/> if any element of the sequence <paramref name="source" /> is duplicated, <see langword="false"/> otherwise 
	/// </returns>
	public static bool HasDuplicates<T>(this IEnumerable<T> source)
		=> source?.HasDuplicates(EqualityComparer<T>.Default) ?? throw new ArgumentNullException(nameof(source));

	/// <summary>
	///   Checks if sequence contains duplicates, using the specified element equality comparer
	/// </summary>
	/// <param name="source">The sequence to check.</param>
	/// <param name="comparer">The equality comparer to use to determine whether or not keys are equal.
	/// If null, the default equality comparer for <c>TSource</c> is used.</param>
	/// <typeparam name="T">The type of the elements in the source sequence</typeparam>
	/// <returns>
	/// <see langword="true"/> if any element of the sequence <paramref name="source" /> is duplicated, <see langword="false"/> otherwise 
	/// </returns>
	public static bool HasDuplicates<T>(this IEnumerable<T> source, IEqualityComparer<T>? comparer)
		=> source?.HasDuplicates(Identity, comparer) ?? throw new ArgumentNullException(nameof(source));

	/// <summary>
	///   Checks if sequence contains duplicates, using the specified element equality comparer
	/// </summary>
	/// <param name="source">The sequence to check.</param>
	/// <param name="keySelector">Projection for determining "distinctness"</param>
	/// <typeparam name="TSource">Type of the source sequence</typeparam>
	/// <typeparam name="TKey">Type of the projected element</typeparam>
	/// <returns>
	/// <see langword="true"/> if any element of the sequence <paramref name="source" /> is duplicated, <see langword="false"/> otherwise 
	/// </returns>
	public static bool HasDuplicates<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		=> source?.HasDuplicates(keySelector, EqualityComparer<TKey>.Default) ?? throw new ArgumentNullException(nameof(source));

	/// <summary>
	///   Checks if sequence contains duplicates, using the specified element equality comparer
	/// </summary>
	/// <param name="source">The sequence to check.</param>
	/// <param name="keySelector">Projection for determining "distinctness"</param>
	/// <param name="comparer">The equality comparer to use to determine whether or not keys are equal.
	/// If null, the default equality comparer for <c>TSource</c> is used.</param>
	/// <typeparam name="TSource">Type of the source sequence</typeparam>
	/// <typeparam name="TKey">Type of the projected element</typeparam>
	/// <returns>
	/// <see langword="true"/> if any element of the sequence <paramref name="source" /> is duplicated, <see langword="false"/> otherwise 
	/// </returns>
	public static bool HasDuplicates<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(keySelector);

		var enumeratedElements = new HashSet<TKey>(comparer);
		return source.Any(element => !enumeratedElements.Add(keySelector(element)));
	}
}
