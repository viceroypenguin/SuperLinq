namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Executes a partial sort of the top <paramref name="count"/> elements of a sequence. If <paramref
	///     name="count"/> is less than the total number of elements in <paramref name="source"/>, then this method will
	///     improve performance.
	/// </summary>
	/// <typeparam name="T">
	///	    Type of elements in the sequence.
	/// </typeparam>
	/// <param name="source">
	///	    The source sequence.
	/// </param>
	/// <param name="count">
	///	    Number of (maximum) elements to return.
	/// </param>
	/// <returns>
	///	    A sequence containing at most top <paramref name="count"/> elements from source, in their ascending order.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> is <see langword="null"/>.
	/// </exception>
	/// <exception cref="ArgumentOutOfRangeException">
	///	    <paramref name="count"/> is less than <c>1</c>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This operation is an <c>O(n * log(K))</c> where <c>K</c> is <paramref name="count"/>.
	/// </para>
	/// <para>
	///	    This operator uses deferred execution and streams it results.
	/// </para>
	/// </remarks>
	public static IEnumerable<T> PartialSort<T>(this IEnumerable<T> source, int count)
	{
		return source.PartialSort(count, comparer: null);
	}

	/// <summary>
	///	    Executes a <paramref name="direction"/> partial sort of the top <paramref name="count"/> elements of a
	///     sequence. If <paramref name="count"/> is less than the total number of elements in <paramref
	///     name="source"/>, then this method will improve performance.
	/// </summary>
	/// <typeparam name="T">
	///	    Type of elements in the sequence.
	/// </typeparam>
	/// <param name="source">
	///	    The source sequence.
	/// </param>
	/// <param name="count">
	///	    Number of (maximum) elements to return.
	/// </param>
	/// <param name="direction">
	///	    The direction in which to sort the elements
	/// </param>
	/// <returns>
	///	    A sequence containing at most top <paramref name="count"/> elements from source, in the specified order.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> is <see langword="null"/>.
	/// </exception>
	/// <exception cref="ArgumentOutOfRangeException">
	///	    <paramref name="count"/> is less than <c>1</c>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This operation is an <c>O(n * log(K))</c> where <c>K</c> is <paramref name="count"/>.
	/// </para>
	/// <para>
	///	    This operator uses deferred execution and streams it results.
	/// </para>
	/// </remarks>
	public static IEnumerable<T> PartialSort<T>(
		this IEnumerable<T> source, int count, OrderByDirection direction)
	{
		return source.PartialSort(count, comparer: null, direction);
	}

	/// <summary>
	///	    Executes a partial sort of the top <paramref name="count"/> elements of a sequence, using <paramref
	///     name="comparer"/> to compare elements. If <paramref name="count"/> is less than the total number of elements
	///     in <paramref name="source"/>, then this method will improve performance.
	/// </summary>
	/// <typeparam name="T">
	///	    Type of elements in the sequence.
	/// </typeparam>
	/// <param name="source">
	///	    The source sequence.
	/// </param>
	/// <param name="count">
	///	    Number of (maximum) elements to return.
	/// </param>
	/// <param name="comparer">
	///	    A <see cref="IComparer{T}"/> to compare elements.
	/// </param>
	/// <returns>
	///	    A sequence containing at most top <paramref name="count"/> elements from source, in their ascending order.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> is <see langword="null"/>.
	/// </exception>
	/// <exception cref="ArgumentOutOfRangeException">
	///	    <paramref name="count"/> is less than <c>1</c>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This operation is an <c>O(n * log(K))</c> where <c>K</c> is <paramref name="count"/>.
	/// </para>
	/// <para>
	///	    This operator uses deferred execution and streams it results.
	/// </para>
	/// </remarks>
	public static IEnumerable<T> PartialSort<T>(
		this IEnumerable<T> source,
		int count, IComparer<T>? comparer)
	{
		return PartialSort(source, count, comparer, OrderByDirection.Ascending);
	}

	/// <summary>
	///	    Executes a <paramref name="direction"/> partial sort of the top <paramref name="count"/> elements of a
	///     sequence, using <paramref name="comparer"/> to compare elements. If <paramref name="count"/> is less than
	///     the total number of elements in <paramref name="source"/>, then this method will improve performance.
	/// </summary>
	/// <typeparam name="T">
	///	    Type of elements in the sequence.
	/// </typeparam>
	/// <param name="source">
	///	    The source sequence.
	/// </param>
	/// <param name="count">
	///	    Number of (maximum) elements to return.
	/// </param>
	/// <param name="comparer">
	///	    A <see cref="IComparer{T}"/> to compare elements.
	/// </param>
	/// <param name="direction">
	///	    The direction in which to sort the elements
	/// </param>
	/// <returns>
	///	    A sequence containing at most top <paramref name="count"/> elements from source, in the specified order.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> is <see langword="null"/>.
	/// </exception>
	/// <exception cref="ArgumentOutOfRangeException">
	///	    <paramref name="count"/> is less than <c>1</c>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This operation is an <c>O(n * log(K))</c> where <c>K</c> is <paramref name="count"/>.
	/// </para>
	/// <para>
	///	    This operator uses deferred execution and streams it results.
	/// </para>
	/// </remarks>
	public static IEnumerable<T> PartialSort<T>(
		this IEnumerable<T> source, int count,
		IComparer<T>? comparer, OrderByDirection direction)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(count);

		comparer ??= Comparer<T>.Default;
		if (direction == OrderByDirection.Descending)
			comparer = new ReverseComparer<T>(comparer);

		return Core(source, count, comparer);

		static IEnumerable<T> Core(IEnumerable<T> source, int count, IComparer<T> comparer)
		{
			var top = new SortedSet<(T Item, int Index)>(
				ValueTupleComparer.Create<T, int>(comparer, default));

			var index = -1;
			foreach (var item in source)
			{
				index++;

				if (top.Count < count)
				{
					_ = top.Add((item, index));
					continue;
				}

				if (comparer.Compare(item, top.Max.Item) >= 0)
					continue;

				_ = top.Remove(top.Max);
				_ = top.Add((item, index));
			}

			foreach (var (item, _) in top)
				yield return item;
		}
	}

	/// <summary>
	///	    Executes a partial sort of the top <paramref name="count"/> elements of a sequence according to the key for
	///     each element. If <paramref name="count"/> is less than the total number of elements in <paramref
	///     name="source"/>, then this method will improve performance.
	/// </summary>
	/// <typeparam name="TSource">
	///	    Type of elements in the sequence.
	/// </typeparam>
	/// <typeparam name="TKey">
	///	    Type of keys.
	/// </typeparam>
	/// <param name="source">
	///	    The source sequence.
	/// </param>
	/// <param name="keySelector">
	///	    A function to extract a key from an element.
	/// </param>
	/// <param name="count">
	///	    Number of (maximum) elements to return.
	/// </param>
	/// <returns>
	///	    A sequence containing at most top <paramref name="count"/> elements from source, in ascending order of their
	///     keys.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="keySelector"/> is <see langword="null"/>.
	/// </exception>
	/// <exception cref="ArgumentOutOfRangeException">
	///	    <paramref name="count"/> is less than <c>1</c>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This operation is an <c>O(n * log(K))</c> where <c>K</c> is <paramref name="count"/>.
	/// </para>
	/// <para>
	///	    This operator uses deferred execution and streams it results.
	/// </para>
	/// </remarks>
	public static IEnumerable<TSource> PartialSortBy<TSource, TKey>(
		this IEnumerable<TSource> source, int count,
		Func<TSource, TKey> keySelector)
	{
		return source.PartialSortBy(count, keySelector, comparer: null);
	}

	/// <summary>
	///	    Executes a <paramref name="direction"/> partial sort of the top <paramref name="count"/> elements of a
	///     sequence according to the key for each element. If <paramref name="count"/> is less than the total number of
	///     elements in <paramref name="source"/>, then this method will improve performance.
	/// </summary>
	/// <typeparam name="TSource">
	///	    Type of elements in the sequence.
	/// </typeparam>
	/// <typeparam name="TKey">
	///	    Type of keys.
	/// </typeparam>
	/// <param name="source">
	///	    The source sequence.
	/// </param>
	/// <param name="keySelector">
	///	    A function to extract a key from an element.
	/// </param>
	/// <param name="count">
	///	    Number of (maximum) elements to return.
	/// </param>
	/// <param name="direction">
	///	    The direction in which to sort the elements
	/// </param>
	/// <returns>
	///	    A sequence containing at most top <paramref name="count"/> elements from source, in the specified order of
	///     their keys.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="keySelector"/> is <see langword="null"/>.
	/// </exception>
	/// <exception cref="ArgumentOutOfRangeException">
	///	    <paramref name="count"/> is less than <c>1</c>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This operation is an <c>O(n * log(K))</c> where <c>K</c> is <paramref name="count"/>.
	/// </para>
	/// <para>
	///	    This operator uses deferred execution and streams it results.
	/// </para>
	/// </remarks>
	public static IEnumerable<TSource> PartialSortBy<TSource, TKey>(
		this IEnumerable<TSource> source, int count,
		Func<TSource, TKey> keySelector, OrderByDirection direction)
	{
		return source.PartialSortBy(count, keySelector, comparer: null, direction);
	}

	/// <summary>
	///	    Executes a partial sort of the top <paramref name="count"/> elements of a sequence according to the key for
	///     each element, using <paramref name="comparer"/> to compare the keys. If <paramref name="count"/> is less
	///     than the total number of elements in <paramref name="source"/>, then this method will improve performance.
	/// </summary>
	/// <typeparam name="TSource">
	///	    Type of elements in the sequence.
	/// </typeparam>
	/// <typeparam name="TKey">
	///	    Type of keys.
	/// </typeparam>
	/// <param name="source">
	///	    The source sequence.
	/// </param>
	/// <param name="keySelector">
	///	    A function to extract a key from an element.
	/// </param>
	/// <param name="count">
	///	    Number of (maximum) elements to return.
	/// </param>
	/// <param name="comparer">
	///	    A <see cref="IComparer{T}"/> to compare elements.
	/// </param>
	/// <returns>
	///	    A sequence containing at most top <paramref name="count"/> elements from source, in ascending order of their
	///     keys.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="keySelector"/> is <see langword="null"/>.
	/// </exception>
	/// <exception cref="ArgumentOutOfRangeException">
	///	    <paramref name="count"/> is less than <c>1</c>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This operation is an <c>O(n * log(K))</c> where <c>K</c> is <paramref name="count"/>.
	/// </para>
	/// <para>
	///	    This operator uses deferred execution and streams it results.
	/// </para>
	/// </remarks>
	public static IEnumerable<TSource> PartialSortBy<TSource, TKey>(
		this IEnumerable<TSource> source, int count,
		Func<TSource, TKey> keySelector,
		IComparer<TKey>? comparer)
	{
		return PartialSortBy(source, count, keySelector, comparer, OrderByDirection.Ascending);
	}

	/// <summary>
	///	    Executes a <paramref name="direction"/> partial sort of the top <paramref name="count"/> elements of a
	///     sequence according to the key for each element, using <paramref name="comparer"/> to compare the keys. If
	///     <paramref name="count"/> is less than the total number of elements in <paramref name="source"/>, then this
	///     method will improve performance.
	/// </summary>
	/// <typeparam name="TSource">
	///	    Type of elements in the sequence.
	/// </typeparam>
	/// <typeparam name="TKey">
	///	    Type of keys.
	/// </typeparam>
	/// <param name="source">
	///	    The source sequence.
	/// </param>
	/// <param name="keySelector">
	///	    A function to extract a key from an element.
	/// </param>
	/// <param name="count">
	///	    Number of (maximum) elements to return.
	/// </param>
	/// <param name="comparer">
	///	    A <see cref="IComparer{T}"/> to compare elements.
	/// </param>
	/// <param name="direction">
	///	    The direction in which to sort the elements
	/// </param>
	/// <returns>
	///	    A sequence containing at most top <paramref name="count"/> elements from source, in the specified order of
	///     their keys.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="keySelector"/> is <see langword="null"/>.
	/// </exception>
	/// <exception cref="ArgumentOutOfRangeException">
	///	    <paramref name="count"/> is less than <c>1</c>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This operation is an <c>O(n * log(K))</c> where <c>K</c> is <paramref name="count"/>.
	/// </para>
	/// <para>
	///	    This operator uses deferred execution and streams it results.
	/// </para>
	/// </remarks>
	public static IEnumerable<TSource> PartialSortBy<TSource, TKey>(
		this IEnumerable<TSource> source, int count,
		Func<TSource, TKey> keySelector,
		IComparer<TKey>? comparer,
		OrderByDirection direction)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(count);
		ArgumentNullException.ThrowIfNull(keySelector);

		comparer ??= Comparer<TKey>.Default;
		if (direction == OrderByDirection.Descending)
			comparer = new ReverseComparer<TKey>(comparer);

		return Core(source, count, keySelector, comparer);

		static IEnumerable<TSource> Core(IEnumerable<TSource> source, int count, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
		{
			var top = new SortedSet<(TKey Key, int Index)>(
				ValueTupleComparer.Create<TKey, int>(comparer, default));
			var dic = new Dictionary<(TKey Key, int Index), TSource>(count);

			var index = 0;
			foreach (var item in source)
			{
				var key = (key: keySelector(item), index++);
				if (top.Count < count)
				{
					_ = top.Add(key);
					dic[key] = item;
					continue;
				}

				if (comparer.Compare(key.key, top.Max.Key) >= 0)
					continue;

				_ = dic.Remove(top.Max);
				_ = top.Remove(top.Max);
				_ = top.Add(key);
				dic[key] = item;
			}

			foreach (var entry in top)
				yield return dic[entry];
		}
	}
}
