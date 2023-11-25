namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///     Checks if sequence contains duplicates.
	/// </summary>
	/// <typeparam name="T">
	///	    The type of the elements in the source sequence.
	/// </typeparam>
	/// <param name="source">
	///	    The sequence to check.
	/// </param>
	/// <returns>
	///     <see langword="true" /> if any element of the sequence <paramref name="source" /> is duplicated, <see
	///     langword="false" /> otherwise.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This method executes immediately.
	/// </para>
	/// </remarks>
	public static bool HasDuplicates<T>(this IEnumerable<T> source)
	{
		return source.HasDuplicates(EqualityComparer<T>.Default);
	}

	/// <summary>
	///     Checks if sequence contains duplicates, using the specified element equality comparer.
	/// </summary>
	/// <typeparam name="T">
	///	    The type of the elements in the source sequence.
	/// </typeparam>
	/// <param name="source">
	///	    The sequence to check.
	/// </param>
	/// <param name="comparer">
	///     The equality comparer to use to determine whether or not keys are equal. If <see langword="null"/>, the
	///     default equality comparer for <typeparamref name="T"/> is used.
	/// </param>
	/// <returns>
	///     <see langword="true" /> if any element of the sequence <paramref name="source" /> is duplicated, <see
	///     langword="false" /> otherwise.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This method executes immediately.
	/// </para>
	/// </remarks>
	public static bool HasDuplicates<T>(this IEnumerable<T> source, IEqualityComparer<T>? comparer)
	{
		return source.HasDuplicates(Identity, comparer);
	}

	/// <summary>
	///     Checks if sequence contains duplicates according to a specified key selector function.
	/// </summary>
	/// <typeparam name="TSource">
	///	    Type of the source sequence.
	/// </typeparam>
	/// <typeparam name="TKey">
	///	    The type of key to distinguish elements by.
	/// </typeparam>
	/// <param name="source">
	///	    The sequence to check.
	/// </param>
	/// <param name="keySelector">
	///	    A function to extract the key for each element.
	/// </param>
	/// <returns>
	///     <see langword="true" /> if any element of the sequence <paramref name="source" /> is duplicated, <see
	///     langword="false" /> otherwise.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="keySelector"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This method executes immediately.
	/// </para>
	/// </remarks>
	public static bool HasDuplicates<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
	{
		return source.HasDuplicates(keySelector, EqualityComparer<TKey>.Default);
	}

	/// <summary>
	///     Checks if sequence contains duplicates according to a specified key selector function, using the specified
	///     element equality comparer.
	/// </summary>
	/// <typeparam name="TSource">
	///	    Type of the source sequence.
	/// </typeparam>
	/// <typeparam name="TKey">
	///	    The type of key to distinguish elements by.
	/// </typeparam>
	/// <param name="source">
	///	    The sequence to check.
	/// </param>
	/// <param name="keySelector">
	///	    A function to extract the key for each element.
	/// </param>
	/// <param name="comparer">
	///     The equality comparer to use to determine whether or not keys are equal. If <see langword="null"/>, the
	///     default equality comparer for <typeparamref name="TKey"/> is used.
	/// </param>
	/// <returns>
	///     <see langword="true" /> if any element of the sequence <paramref name="source" /> is duplicated, <see
	///     langword="false" /> otherwise.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="keySelector"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This method executes immediately.
	/// </para>
	/// </remarks>
	public static bool HasDuplicates<TSource, TKey>(
		this IEnumerable<TSource> source,
		Func<TSource, TKey> keySelector,
		IEqualityComparer<TKey>? comparer)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(keySelector);

		return source
			.Select(keySelector)
			.Duplicates(comparer)
			.Any();
	}
}
