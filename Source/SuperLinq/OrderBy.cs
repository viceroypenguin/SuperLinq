namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Sorts the elements of a sequence in a particular direction (ascending, descending) according to a key
	/// </summary>
	/// <typeparam name="T">
	///	    The type of the elements in the source sequence
	/// </typeparam>
	/// <typeparam name="TKey">
	///	    The type of the key used to order elements
	/// </typeparam>
	/// <param name="source">
	///	    The sequence to order
	/// </param>
	/// <param name="keySelector">
	///	    A key selector function
	/// </param>
	/// <param name="direction">
	///	    A direction in which to order the elements (ascending, descending)
	/// </param>
	/// <returns>
	///	    An ordered copy of the source sequence
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="keySelector"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This method is implemented by using deferred execution. However, <paramref name="source"/> will be consumed
	///     in it's entirety immediately when first element of the returned sequence is consumed. 
	/// </para>
	/// </remarks>
	public static IOrderedEnumerable<T> OrderBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector, OrderByDirection direction)
	{
		return OrderBy(source, keySelector, null, direction);
	}

	/// <summary>
	///	    Sorts the elements of a sequence in a particular direction (ascending, descending) according to a key
	/// </summary>
	/// <typeparam name="T">
	///	    The type of the elements in the source sequence
	/// </typeparam>
	/// <typeparam name="TKey">
	///	    The type of the key used to order elements
	/// </typeparam>
	/// <param name="source">
	///	    The sequence to order
	/// </param>
	/// <param name="keySelector">
	///	    A key selector function
	/// </param>
	/// <param name="comparer">
	///		An <see cref="IComparer{T}"/> to compare keys
	/// </param>
	/// <param name="direction">
	///	    A direction in which to order the elements (ascending, descending)
	/// </param>
	/// <returns>
	///	    An ordered copy of the source sequence
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="keySelector"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This method is implemented by using deferred execution. However, <paramref name="source"/> will be consumed
	///     in it's entirety immediately when first element of the returned sequence is consumed. 
	/// </para>
	/// </remarks>
	public static IOrderedEnumerable<T> OrderBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector, IComparer<TKey>? comparer, OrderByDirection direction)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(keySelector);
		return direction == OrderByDirection.Ascending
			? source.OrderBy(keySelector, comparer)
			: source.OrderByDescending(keySelector, comparer);
	}

	/// <summary>
	///	    Performs a subsequent ordering of elements of a sequence in a particular direction (ascending, descending) according to a key
	/// </summary>
	/// <typeparam name="T">
	///	    The type of the elements in the source sequence
	/// </typeparam>
	/// <typeparam name="TKey">
	///	    The type of the key used to order elements
	/// </typeparam>
	/// <param name="source">
	///	    The sequence to order
	/// </param>
	/// <param name="keySelector">
	///	    A key selector function
	/// </param>
	/// <param name="direction">
	///	    A direction in which to order the elements (ascending, descending)
	/// </param>
	/// <returns>
	///	    An ordered copy of the source sequence
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="keySelector"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This method is implemented by using deferred execution. However, <paramref name="source"/> will be consumed
	///     in it's entirety immediately when first element of the returned sequence is consumed. 
	/// </para>
	/// </remarks>
	public static IOrderedEnumerable<T> ThenBy<T, TKey>(this IOrderedEnumerable<T> source, Func<T, TKey> keySelector, OrderByDirection direction)
	{
		return ThenBy(source, keySelector, null, direction);
	}

	/// <summary>
	///	    Performs a subsequent ordering of elements of a sequence in a particular direction (ascending, descending) according to a key
	/// </summary>
	/// <typeparam name="T">
	///	    The type of the elements in the source sequence
	/// </typeparam>
	/// <typeparam name="TKey">
	///	    The type of the key used to order elements
	/// </typeparam>
	/// <param name="source">
	///	    The sequence to order
	/// </param>
	/// <param name="keySelector">
	///	    A key selector function
	/// </param>
	/// <param name="comparer">
	///		An <see cref="IComparer{T}"/> to compare keys
	/// </param>
	/// <param name="direction">
	///	    A direction in which to order the elements (ascending, descending)
	/// </param>
	/// <returns>
	///	    An ordered copy of the source sequence
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="keySelector"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This method is implemented by using deferred execution. However, <paramref name="source"/> will be consumed
	///     in it's entirety immediately when first element of the returned sequence is consumed. 
	/// </para>
	/// </remarks>
	public static IOrderedEnumerable<T> ThenBy<T, TKey>(this IOrderedEnumerable<T> source, Func<T, TKey> keySelector, IComparer<TKey>? comparer, OrderByDirection direction)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(keySelector);
		return direction == OrderByDirection.Ascending
			? source.ThenBy(keySelector, comparer)
			: source.ThenByDescending(keySelector, comparer);
	}
}
