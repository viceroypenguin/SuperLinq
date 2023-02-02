namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Creates an array from an <see cref="IEnumerable{T}"/> where a function is used to determine the index at which
	/// an element will be placed in the array.
	/// </summary>
	/// <param name="source">The source sequence for the array.</param>
	/// <param name="indexSelector">
	/// A function that maps an element to its index.</param>
	/// <typeparam name="T">
	/// The type of the element in <paramref name="source"/>.</typeparam>
	/// <returns>
	/// An array that contains the elements from <paramref name="source"/>. The size of the array will be as large as
	/// the highest index returned by the <paramref name="indexSelector"/> plus 1.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="indexSelector"/> is <see
	/// langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException">An index returned by <paramref name="indexSelector"/> is less than
	/// <c>0</c>.</exception>
	/// <remarks>
	/// This method forces immediate query evaluation. It should not be used on infinite sequences. If more than one
	/// element maps to the same index then the latter element overwrites the former in the resulting array.
	/// </remarks>
	public static T?[] ToArrayByIndex<T>(
		this IEnumerable<T> source,
		Func<T, int> indexSelector)
	{
		return source.ToArrayByIndex(indexSelector, (e, _) => e);
	}

	/// <summary>
	/// Creates an array from an <see cref="IEnumerable{T}"/> where a function is used to determine the index at which
	/// an element will be placed in the array. The elements are projected into the array via an additional function.
	/// </summary>
	/// <param name="source">The source sequence for the array.</param>
	/// <param name="indexSelector">
	/// A function that maps an element to its index.</param>
	/// <param name="resultSelector">
	/// A function to project a source element into an element of the resulting array.</param>
	/// <typeparam name="T">
	/// The type of the element in <paramref name="source"/>.</typeparam>
	/// <typeparam name="TResult">
	/// The type of the element in the resulting array.</typeparam>
	/// <returns>
	/// An array that contains the projected elements from <paramref name="source"/>. The size of the array will be as
	/// large as the highest index returned by the <paramref name="indexSelector"/> plus 1.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/>, <paramref name="indexSelector"/>, or
	/// <paramref name="resultSelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException">An index returned by <paramref name="indexSelector"/> is less than
	/// <c>0</c>.</exception>
	/// <remarks>
	/// This method forces immediate query evaluation. It should not be used on infinite sequences. If more than one
	/// element maps to the same index then the latter element overwrites the former in the resulting array.
	/// </remarks>
	public static TResult?[] ToArrayByIndex<T, TResult>(
		this IEnumerable<T> source,
		Func<T, int> indexSelector,
		Func<T, TResult> resultSelector)
	{
		Guard.IsNotNull(resultSelector);
		return source.ToArrayByIndex(indexSelector, (e, _) => resultSelector(e));
	}

	/// <summary>
	/// Creates an array from an <see cref="IEnumerable{T}"/> where a function is used to determine the index at which
	/// an element will be placed in the array. The elements are projected into the array via an additional function.
	/// </summary>
	/// <param name="source">The source sequence for the array.</param>
	/// <param name="indexSelector">
	/// A function that maps an element to its index.</param>
	/// <param name="resultSelector">
	/// A function to project a source element into an element of the resulting array.</param>
	/// <typeparam name="T">
	/// The type of the element in <paramref name="source"/>.</typeparam>
	/// <typeparam name="TResult">
	/// The type of the element in the resulting array.</typeparam>
	/// <returns>
	/// An array that contains the projected elements from <paramref name="source"/>. The size of the array will be as
	/// large as the highest index returned by the <paramref name="indexSelector"/> plus 1.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/>, <paramref name="indexSelector"/>, or
	/// <paramref name="resultSelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException">An index returned by <paramref name="indexSelector"/> is less than
	/// <c>0</c>.</exception>
	/// <remarks>
	/// This method forces immediate query evaluation. It should not be used on infinite sequences. If more than one
	/// element maps to the same index then the latter element overwrites the former in the resulting array.
	/// </remarks>
	public static TResult?[] ToArrayByIndex<T, TResult>(
		this IEnumerable<T> source,
		Func<T, int> indexSelector,
		Func<T, int, TResult> resultSelector)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(indexSelector);
		Guard.IsNotNull(resultSelector);

		var lastIndex = -1;
		var indexed = (List<KeyValuePair<int, T>>?)null;
		List<KeyValuePair<int, T>> Indexed() => indexed ??= new List<KeyValuePair<int, T>>();

		foreach (var e in source)
		{
			var i = indexSelector(e);
			if (i < 0)
				throw new InvalidOperationException("'indexSelector' returned an invalid index for the current object.");
			lastIndex = Math.Max(i, lastIndex);
			Indexed().Add(new KeyValuePair<int, T>(i, e));
		}

		var length = lastIndex + 1;
		return length == 0
			 ? Array.Empty<TResult>()
			 : Indexed().ToArrayByIndex(length, e => e.Key, e => resultSelector(e.Value, e.Key));
	}

	/// <summary>
	/// Creates an array of user-specified length from an <see cref="IEnumerable{T}"/> where a function is used to
	/// determine the index at which an element will be placed in the array.
	/// </summary>
	/// <param name="source">The source sequence for the array.</param>
	/// <param name="length">The (non-negative) length of the resulting array.</param>
	/// <param name="indexSelector">
	/// A function that maps an element to its index.</param>
	/// <typeparam name="T">
	/// The type of the element in <paramref name="source"/>.</typeparam>
	/// <returns>
	/// An array of size <paramref name="length"/> that contains the elements from <paramref name="source"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="indexSelector"/> is <see
	/// langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is less than <c>0</c>. -or- An index
	/// returned by <paramref name="indexSelector"/> is invalid for an array of size <paramref
	/// name="length"/>.</exception>
	/// <remarks>
	/// This method forces immediate query evaluation. It should not be used on infinite sequences. If more than one
	/// element maps to the same index then the latter element overwrites the former in the resulting array.
	/// </remarks>
	public static T?[] ToArrayByIndex<T>(
		this IEnumerable<T> source,
		int length,
		Func<T, int> indexSelector)
	{
		return source.ToArrayByIndex(length, indexSelector, (e, _) => e);
	}

	/// <summary>
	/// Creates an array of user-specified length from an <see cref="IEnumerable{T}"/> where a function is used to
	/// determine the index at which an element will be placed in the array. The elements are projected into the array
	/// via an additional function.
	/// </summary>
	/// <param name="source">The source sequence for the array.</param>
	/// <param name="length">The (non-negative) length of the resulting array.</param>
	/// <param name="indexSelector">
	/// A function that maps an element to its index.</param>
	/// <param name="resultSelector">
	/// A function to project a source element into an element of the resulting array.</param>
	/// <typeparam name="T">
	/// The type of the element in <paramref name="source"/>.</typeparam>
	/// <typeparam name="TResult">
	/// The type of the element in the resulting array.</typeparam>
	/// <returns>
	/// An array of size <paramref name="length"/> that contains the projected elements from <paramref name="source"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/>, <paramref name="indexSelector"/>, <paramref
	/// name="resultSelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is less than <c>0</c>. -or- An index
	/// returned by <paramref name="indexSelector"/> is invalid for an array of size <paramref
	/// name="length"/>.</exception>
	/// <remarks>
	/// This method forces immediate query evaluation. It should not be used on infinite sequences. If more than one
	/// element maps to the same index then the latter element overwrites the former in the resulting array.
	/// </remarks>
	public static TResult?[] ToArrayByIndex<T, TResult>(
		this IEnumerable<T> source,
		int length,
		Func<T, int> indexSelector,
		Func<T, TResult> resultSelector)
	{
		Guard.IsNotNull(resultSelector);
		return source.ToArrayByIndex(length, indexSelector, (e, _) => resultSelector(e));
	}

	/// <summary>
	/// Creates an array of user-specified length from an <see cref="IEnumerable{T}"/> where a function is used to
	/// determine the index at which an element will be placed in the array. The elements are projected into the array
	/// via an additional function.
	/// </summary>
	/// <param name="source">The source sequence for the array.</param>
	/// <param name="length">The (non-negative) length of the resulting array.</param>
	/// <param name="indexSelector">
	/// A function that maps an element to its index.</param>
	/// <param name="resultSelector">
	/// A function to project a source element into an element of the resulting array.</param>
	/// <typeparam name="T">
	/// The type of the element in <paramref name="source"/>.</typeparam>
	/// <typeparam name="TResult">
	/// The type of the element in the resulting array.</typeparam>
	/// <returns>
	/// An array of size <paramref name="length"/> that contains the projected elements from the input sequence.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/>, <paramref name="indexSelector"/>, <paramref
	/// name="resultSelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is less than <c>0</c>. -or- An index
	/// returned by <paramref name="indexSelector"/> is invalid for an array of size <paramref
	/// name="length"/>.</exception>
	/// <remarks>
	/// This method forces immediate query evaluation. It should not be used on infinite sequences. If more than one
	/// element maps to the same index then the latter element overwrites the former in the resulting array.
	/// </remarks>
	public static TResult?[] ToArrayByIndex<T, TResult>(
		this IEnumerable<T> source,
		int length,
		Func<T, int> indexSelector,
		Func<T, int, TResult> resultSelector)
	{
		Guard.IsNotNull(source);
		Guard.IsGreaterThanOrEqualTo(length, 0);
		Guard.IsNotNull(indexSelector);
		Guard.IsNotNull(resultSelector);

		var array = new TResult?[length];
		foreach (var e in source)
		{
			var i = indexSelector(e);
			if (i < 0 || i >= array.Length)
				throw new InvalidOperationException("'indexSelector' returned an invalid index for the current object.");
			array[i] = resultSelector(e, i);
		}
		return array;
	}
}
