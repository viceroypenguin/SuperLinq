namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Split the elements of a sequence into chunks of size at most <paramref name="size"/>.
	/// </summary>
	/// <typeparam name="TSource">
	///	    The type of the elements of <paramref name="source"/>.
	///	</typeparam>
	/// <typeparam name="TResult">
	///	    The type of the value return by <paramref name="resultSelector"/>.
	/// </typeparam>
	/// <param name="source">
	///	    An <see cref="IEnumerable{T}"/> whose elements to chunk.
	/// </param>
	/// <param name="size">
	///	    The maximum size of each chunk.
	/// </param>
	/// <param name="resultSelector">
	///	    A transform function to apply to each chunk.
	/// </param>
	/// <returns>
	///	    A sequence of elements returned by <paramref name="resultSelector"/>.
	///	</returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="resultSelector"/> is <see langword="null"/>.
	/// </exception>
	/// <exception cref="ArgumentOutOfRangeException">
	///	    <paramref name="size"/> is below 1.
	///	</exception>
	/// <remarks>
	/// <para>
	///	    A chunk can contain fewer elements than <paramref name="size"/>, specifically the final chunk of <paramref
	///     name="source"/>.
	/// </para>
	/// <para>
	///	    In this overload of <c>Batch</c>, a single array of length <paramref name="size"/> is allocated as a buffer
	///     for all subsequences.
	/// </para>
	/// <para>
	///	    This operator uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IEnumerable<TResult> Batch<TSource, TResult>(
		this IEnumerable<TSource> source,
		int size,
		Func<IReadOnlyList<TSource>, TResult> resultSelector)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(resultSelector);
		Guard.IsGreaterThanOrEqualTo(size, 1);

		return BatchImpl(source, new TSource[size], size, resultSelector);
	}

	/// <summary>
	///	    Split the elements of a sequence into chunks of size at most <c><paramref name="array"/>.Length</c>.
	/// </summary>
	/// <typeparam name="TSource">
	///	    The type of the elements of <paramref name="source"/>.
	///	</typeparam>
	/// <typeparam name="TResult">
	///	    The type of the value return by <paramref name="resultSelector"/>.
	/// </typeparam>
	/// <param name="source">
	///	    An <see cref="IEnumerable{T}"/> whose elements to chunk.
	/// </param>
	/// <param name="array">
	///	    The array to use as a buffer for each chunk
	/// </param>
	/// <param name="resultSelector">
	///	    A transform function to apply to each chunk.
	/// </param>
	/// <returns>
	///	    A sequence of elements returned by <paramref name="resultSelector"/>.
	///	</returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/>, <paramref name="resultSelector"/>, or <paramref name="array"/> is <see
	///     langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    A chunk can contain fewer elements than <c><paramref name="array"/>.Length</c>, specifically the final chunk
	///     of <paramref name="source"/>.
	/// </para>
	/// <para>
	///	    In this overload of <c>Batch</c>, <paramref name="array"/> is used as a common buffer for all subsequences.
	/// </para>
	/// <para>
	///	    This operator uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IEnumerable<TResult> Batch<TSource, TResult>(
		this IEnumerable<TSource> source,
		TSource[] array,
		Func<IReadOnlyList<TSource>, TResult> resultSelector)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(array);
		Guard.IsNotNull(resultSelector);

		return BatchImpl(source, array, array.Length, resultSelector);
	}

	/// <summary>
	///	    Split the elements of a sequence into chunks of size at most <paramref name="size"/>.
	/// </summary>
	/// <typeparam name="TSource">
	///	    The type of the elements of <paramref name="source"/>.
	///	</typeparam>
	/// <typeparam name="TResult">
	///	    The type of the value return by <paramref name="resultSelector"/>.
	/// </typeparam>
	/// <param name="source">
	///	    An <see cref="IEnumerable{T}"/> whose elements to chunk.
	/// </param>
	/// <param name="array">
	///	    The array to use as a buffer for each chunk
	/// </param>
	/// <param name="size">
	///	    The maximum size of each chunk.
	/// </param>
	/// <param name="resultSelector">
	///	    A transform function to apply to each chunk.
	/// </param>
	/// <returns>
	///	    A sequence of elements returned by <paramref name="resultSelector"/>.
	///	</returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/>, <paramref name="resultSelector"/>, or <paramref name="array"/> is <see
	///     langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    A chunk can contain fewer elements than <paramref name="size"/>, specifically the final chunk of <paramref
	///     name="source"/>.
	/// </para>
	/// <para>
	///	    In this overload of <c>Batch</c>, <paramref name="array"/> is used as a common buffer for all
	///     subsequences. This overload is provided to ease usage of common buffers, such as those rented from <see
	///     cref="System.Buffers.ArrayPool{T}"/>, which may return an array larger than requested.
	/// </para>
	/// <para>
	///	    This operator uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IEnumerable<TResult> Batch<TSource, TResult>(
		this IEnumerable<TSource> source,
		TSource[] array,
		int size,
		Func<IReadOnlyList<TSource>, TResult> resultSelector)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(array);
		Guard.IsNotNull(resultSelector);
		Guard.IsBetweenOrEqualTo(size, 1, array.Length);

		return BatchImpl(source, array, size, resultSelector);
	}

	private static IEnumerable<TResult> BatchImpl<TSource, TResult>(
		IEnumerable<TSource> source,
		TSource[] array,
		int size,
		Func<IReadOnlyList<TSource>, TResult> resultSelector)
	{
		if (source is ICollection<TSource> coll
			&& coll.Count <= size)
		{
			coll.CopyTo(array, 0);
			yield return resultSelector(new ArraySegment<TSource>(array, 0, coll.Count));
			yield break;
		}

		var n = 0;
		foreach (var item in source)
		{
			array[n++] = item;
			if (n == size)
			{
				yield return resultSelector(new ArraySegment<TSource>(array, 0, n));
				n = 0;
			}
		}

		if (n != 0)
			yield return resultSelector(new ArraySegment<TSource>(array, 0, n));
	}
}
