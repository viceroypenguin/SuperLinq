namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Creates an array from an <see cref="IEnumerable{T}"/> where a function is used to determine the index at
	///     which an element will be placed in the array.
	/// </summary>
	/// <param name="source">
	///	    The source sequence for the array.
	/// </param>
	/// <param name="indexSelector">
	///	    A function that maps an element to its index.
	/// </param>
	/// <typeparam name="T">
	///	    The type of the element in <paramref name="source"/>.
	/// </typeparam>
	/// <returns>
	///	    An array that contains the elements from <paramref name="source"/>. The size of the array will be as large
	///     as the highest index returned by the <paramref name="indexSelector"/> plus <c>1</c>.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="indexSelector"/> is <see langword="null"/>.
	/// </exception>
	/// <exception cref="ArgumentOutOfRangeException">
	///	    An index returned by <paramref name="indexSelector"/> is less than <c>0</c>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This method forces immediate query evaluation. It should not be used on infinite sequences. If more than one
	///     element maps to the same index then the latter element overwrites the former in the resulting array.
	/// </para>
	/// </remarks>
	public static T?[] ToArrayByIndex<T>(
		this IEnumerable<T> source,
		Func<T, int> indexSelector)
	{
		return source.ToArrayByIndex(indexSelector, (e, _) => e);
	}

	/// <summary>
	///	    Creates an array from an <see cref="IEnumerable{T}"/> where a function is used to determine the index at
	///     which an element will be placed in the array. The elements are projected into the array via an additional
	///     function.
	/// </summary>
	/// <param name="source">
	///	    The source sequence for the array.
	/// </param>
	/// <param name="indexSelector">
	///	    A function that maps an element to its index.
	/// </param>
	/// <param name="resultSelector">
	///	    A function to project a source element into an element of the resulting array.
	/// </param>
	/// <typeparam name="T">
	///	    The type of the element in <paramref name="source"/>.
	/// </typeparam>
	/// <typeparam name="TResult">
	///	    The type of the element in the resulting array.
	/// </typeparam>
	/// <returns>
	///	    An array that contains the projected elements from <paramref name="source"/>. The size of the array will be
	///     as large as the highest index returned by the <paramref name="indexSelector"/> plus <c>1</c>.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/>, <paramref name="indexSelector"/>, or <paramref name="resultSelector"/> is <see
	///     langword="null"/>.
	/// </exception>
	/// <exception cref="ArgumentOutOfRangeException">
	///	    An index returned by <paramref name="indexSelector"/> is less than <c>0</c>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This method forces immediate query evaluation. It should not be used on infinite sequences. If more than one
	///     element maps to the same index then the latter element overwrites the former in the resulting array.
	/// </para>
	/// </remarks>
	public static TResult?[] ToArrayByIndex<T, TResult>(
		this IEnumerable<T> source,
		Func<T, int> indexSelector,
		Func<T, TResult> resultSelector)
	{
		ArgumentNullException.ThrowIfNull(resultSelector);
		return source.ToArrayByIndex(indexSelector, (e, _) => resultSelector(e));
	}

	/// <summary>
	///	    Creates an array from an <see cref="IEnumerable{T}"/> where a function is used to determine the index at
	///     which an element will be placed in the array. The elements are projected into the array via an additional
	///     function.
	/// </summary>
	/// <param name="source">
	///	    The source sequence for the array.
	/// </param>
	/// <param name="indexSelector">
	///	    A function that maps an element to its index.
	/// </param>
	/// <param name="resultSelector">
	///	    A function to project a source element into an element of the resulting array.
	/// </param>
	/// <typeparam name="T">
	///	    The type of the element in <paramref name="source"/>.
	/// </typeparam>
	/// <typeparam name="TResult">
	///	    The type of the element in the resulting array.
	/// </typeparam>
	/// <returns>
	///	    An array that contains the projected elements from <paramref name="source"/>. The size of the array will be
	///     as large as the highest index returned by the <paramref name="indexSelector"/> plus <c>1</c>.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/>, <paramref name="indexSelector"/>, or <paramref name="resultSelector"/> is <see
	///     langword="null"/>.
	/// </exception>
	/// <exception cref="ArgumentOutOfRangeException">
	///	    An index returned by <paramref name="indexSelector"/> is less than <c>0</c>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This method forces immediate query evaluation. It should not be used on infinite sequences. If more than one
	///     element maps to the same index then the latter element overwrites the former in the resulting array.
	/// </para>
	/// </remarks>
	public static TResult?[] ToArrayByIndex<T, TResult>(
		this IEnumerable<T> source,
		Func<T, int> indexSelector,
		Func<T, int, TResult> resultSelector)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(indexSelector);
		ArgumentNullException.ThrowIfNull(resultSelector);

		var lastIndex = -1;
		var indexed = new List<(int, T)>();

		foreach (var e in source)
		{
			var i = indexSelector(e);
			ArgumentOutOfRangeException.ThrowIfNegative(i, "indexSelector(e)");

			lastIndex = Math.Max(i, lastIndex);
			indexed.Add((i, e));
		}

		if (lastIndex == -1)
			return [];

		var length = lastIndex + 1;
		var array = new TResult?[length];

		foreach (var (idx, el) in indexed)
			array[idx] = resultSelector(el, idx);

		return array;
	}

	/// <summary>
	///	    Creates an array of user-specified length from an <see cref="IEnumerable{T}"/> where a function is used to
	///     determine the index at which an element will be placed in the array.
	/// </summary>
	/// <param name="source">
	///	    The source sequence for the array.
	/// </param>
	/// <param name="length">
	///	    The (non-negative) length of the resulting array.
	/// </param>
	/// <param name="indexSelector">
	///	    A function that maps an element to its index.
	/// </param>
	/// <typeparam name="T">
	///	    The type of the element in <paramref name="source"/>.
	/// </typeparam>
	/// <returns>
	///	    An array of size <paramref name="length"/> that contains the elements from <paramref name="source"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="indexSelector"/> is <see langword="null"/>.
	/// </exception>
	/// <exception cref="ArgumentOutOfRangeException">
	///	    <paramref name="length"/> is less than <c>0</c>. -or- An index returned by <paramref name="indexSelector"/>
	///     is invalid for an array of size <paramref name="length"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This method forces immediate query evaluation. It should not be used on infinite sequences. If more than one
	///     element maps to the same index then the latter element overwrites the former in the resulting array.
	/// </para>
	/// </remarks>
	public static T?[] ToArrayByIndex<T>(
		this IEnumerable<T> source,
		int length,
		Func<T, int> indexSelector)
	{
		return source.ToArrayByIndex(length, indexSelector, (e, _) => e);
	}

	/// <summary>
	///	    Creates an array of user-specified length from an <see cref="IEnumerable{T}"/> where a function is used to
	///     determine the index at which an element will be placed in the array. The elements are projected into the
	///     array via an additional function.
	/// </summary>
	/// <param name="source">
	///	    The source sequence for the array.
	///	</param>
	/// <param name="length">
	///	    The (non-negative) length of the resulting array.
	///	</param>
	/// <param name="indexSelector">
	///	    A function that maps an element to its index.
	/// </param>
	/// <param name="resultSelector">
	///	    A function to project a source element into an element of the resulting array.
	/// </param>
	/// <typeparam name="T">
	///	    The type of the element in <paramref name="source"/>.
	/// </typeparam>
	/// <typeparam name="TResult">
	///	    The type of the element in the resulting array.
	/// </typeparam>
	/// <returns>
	///	    An array of size <paramref name="length"/> that contains the projected elements from <paramref
	///     name="source"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/>, <paramref name="indexSelector"/>, <paramref name="resultSelector"/> is <see
	///     langword="null"/>.
	/// </exception>
	/// <exception cref="ArgumentOutOfRangeException">
	///	    <paramref name="length"/> is less than <c>0</c>. -or- An index returned by <paramref name="indexSelector"/>
	///     is invalid for an array of size <paramref name="length"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This method forces immediate query evaluation. It should not be used on infinite sequences. If more than one
	///     element maps to the same index then the latter element overwrites the former in the resulting array.
	/// </para>
	/// </remarks>
	public static TResult?[] ToArrayByIndex<T, TResult>(
		this IEnumerable<T> source,
		int length,
		Func<T, int> indexSelector,
		Func<T, TResult> resultSelector)
	{
		ArgumentNullException.ThrowIfNull(resultSelector);
		return source.ToArrayByIndex(length, indexSelector, (e, _) => resultSelector(e));
	}

	/// <summary>
	///	    Creates an array of user-specified length from an <see cref="IEnumerable{T}"/> where a function is used to
	///     determine the index at which an element will be placed in the array. The elements are projected into the
	///     array via an additional function.
	/// </summary>
	/// <param name="source">
	///	    The source sequence for the array.
	/// </param>
	/// <param name="length">
	///	    The (non-negative) length of the resulting array.
	/// </param>
	/// <param name="indexSelector">
	///	    A function that maps an element to its index.
	/// </param>
	/// <param name="resultSelector">
	///	    A function to project a source element into an element of the resulting array.
	/// </param>
	/// <typeparam name="T">
	///	    The type of the element in <paramref name="source"/>.
	/// </typeparam>
	/// <typeparam name="TResult">
	///	    The type of the element in the resulting array.
	/// </typeparam>
	/// <returns>
	///	    An array of size <paramref name="length"/> that contains the projected elements from the input sequence.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/>, <paramref name="indexSelector"/>, <paramref name="resultSelector"/> is <see
	///     langword="null"/>.
	/// </exception>
	/// <exception cref="ArgumentOutOfRangeException">
	///	    <paramref name="length"/> is less than <c>0</c>. -or- An index returned by <paramref name="indexSelector"/>
	///     is invalid for an array of size <paramref name="length"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This method forces immediate query evaluation. It should not be used on infinite sequences. If more than one
	///     element maps to the same index then the latter element overwrites the former in the resulting array.
	/// </para>
	/// </remarks>
	public static TResult?[] ToArrayByIndex<T, TResult>(
		this IEnumerable<T> source,
		int length,
		Func<T, int> indexSelector,
		Func<T, int, TResult> resultSelector)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentOutOfRangeException.ThrowIfNegative(length);
		ArgumentNullException.ThrowIfNull(indexSelector);
		ArgumentNullException.ThrowIfNull(resultSelector);

		var array = new TResult?[length];
		foreach (var e in source)
		{
			var i = indexSelector(e);
			ArgumentOutOfRangeException.ThrowIfNegative(i, "indexSelector(e)");
			ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(i, array.Length, "indexSelector(e)");

			array[i] = resultSelector(e, i);
		}
		return array;
	}
}
