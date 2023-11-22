using SuperLinq.Collections;

namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Executes a partial sort of the top <paramref name="count"/> elements of a sequence, including ties. If
	///     <paramref name="count"/> is less than the total number of elements in <paramref name="source"/>, then this
	///     method will improve performance.
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
	///	    <paramref name="count"/> is less than 1.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This is an <c>O(n * log(K))</c> operation where <c>K</c> is <paramref name="count"/>.
	/// </para>
	/// <para>
	///	    This method is implemented by using deferred execution. However, <paramref name="source"/> will be consumed
	///     in it's entirety immediately when first element of the returned sequence is consumed. 
	/// </para>
	/// <para>
	///	    This method performs a stable sort; that is, if the keys of two elements are equal, the order of the
	///     elements is preserved. In contrast, an unstable sort does not preserve the order of elements that have the
	///     same key.
	/// </para>
	/// </remarks>
	public static IEnumerable<T> DensePartialSort<T>(this IEnumerable<T> source, int count)
	{
		return source.DensePartialSort(count, comparer: null);
	}

	/// <summary>
	///	    Executes a <paramref name="direction"/> partial sort of the top <paramref name="count"/> elements of a
	///     sequence, including ties. If <paramref name="count"/> is less than the total number of elements in <paramref
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
	///	    <paramref name="count"/> is less than 1.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This is an <c>O(n * log(K))</c> operation where <c>K</c> is <paramref name="count"/>.
	/// </para>
	/// <para>
	///	    This method is implemented by using deferred execution. However, <paramref name="source"/> will be consumed
	///     in it's entirety immediately when first element of the returned sequence is consumed. 
	/// </para>
	/// <para>
	///	    This method performs a stable sort; that is, if the keys of two elements are equal, the order of the
	///     elements is preserved. In contrast, an unstable sort does not preserve the order of elements that have the
	///     same key.
	/// </para>
	/// </remarks>
	public static IEnumerable<T> DensePartialSort<T>(
		this IEnumerable<T> source, int count, OrderByDirection direction)
	{
		return source.DensePartialSort(count, comparer: null, direction);
	}

	/// <summary>
	///	    Executes a partial sort of the top <paramref name="count"/> elements of a sequence, including ties, using
	///     <paramref name="comparer"/> to compare elements. If <paramref name="count"/> is less than the total number
	///     of elements in <paramref name="source"/>, then this method will improve performance.
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
	///	    An <see cref="IComparer{T}"/> to compare elements.
	/// </param>
	/// <returns>
	///	    A sequence containing at most top <paramref name="count"/> elements from source, in their ascending order.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> is <see langword="null"/>.
	/// </exception>
	/// <exception cref="ArgumentOutOfRangeException">
	///	    <paramref name="count"/> is less than 1.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This is an <c>O(n * log(K))</c> operation where <c>K</c> is <paramref name="count"/>.
	/// </para>
	/// <para>
	///	    This method is implemented by using deferred execution. However, <paramref name="source"/> will be consumed
	///     in it's entirety immediately when first element of the returned sequence is consumed. 
	/// </para>
	/// <para>
	///	    This method performs a stable sort; that is, if the keys of two elements are equal, the order of the
	///     elements is preserved. In contrast, an unstable sort does not preserve the order of elements that have the
	///     same key.
	/// </para>
	/// </remarks>
	public static IEnumerable<T> DensePartialSort<T>(
		this IEnumerable<T> source,
		int count, IComparer<T>? comparer)
	{
		return DensePartialSort(source, count, comparer, OrderByDirection.Ascending);
	}

	/// <summary>
	///	    Executes a <paramref name="direction"/> partial sort of the top <paramref name="count"/> elements of a
	///     sequence, including ties, using <paramref name="comparer"/> to compare elements. If <paramref name="count"/>
	///     is less than the total number of elements in <paramref name="source"/>, then this method will improve
	///     performance.
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
	///	    <paramref name="count"/> is less than 1.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This is an <c>O(n * log(K))</c> operation where <c>K</c> is <paramref name="count"/>.
	/// </para>
	/// <para>
	///	    This method is implemented by using deferred execution. However, <paramref name="source"/> will be consumed
	///     in it's entirety immediately when first element of the returned sequence is consumed. 
	/// </para>
	/// <para>
	///	    This method performs a stable sort; that is, if the keys of two elements are equal, the order of the
	///     elements is preserved. In contrast, an unstable sort does not preserve the order of elements that have the
	///     same key.
	/// </para>
	/// </remarks>
	public static IEnumerable<T> DensePartialSort<T>(
		this IEnumerable<T> source, int count,
		IComparer<T>? comparer, OrderByDirection direction)
	{
		return DensePartialSortBy(source, count, Identity, comparer, direction);
	}

	/// <summary>
	///	    Executes a partial sort of the top <paramref name="count"/> elements of a sequence, including ties,
	///     according to the key for each element. If <paramref name="count"/> is less than the total number of elements
	///     in <paramref name="source"/>, then this method will improve performance.
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
	///	    <paramref name="count"/> is less than 1.</exception>
	/// <remarks>
	/// <para>
	///	    This is an <c>O(n * log(K))</c> operation where <c>K</c> is <paramref name="count"/>.
	/// </para>
	/// <para>
	///	    This method is implemented by using deferred execution. However, <paramref name="source"/> will be consumed
	///     in it's entirety immediately when first element of the returned sequence is consumed. 
	/// </para>
	/// <para>
	///	    This method performs a stable sort; that is, if the keys of two elements are equal, the order of the
	///     elements is preserved. In contrast, an unstable sort does not preserve the order of elements that have the
	///     same key.
	/// </para>
	/// </remarks>
	public static IEnumerable<TSource> DensePartialSortBy<TSource, TKey>(
		this IEnumerable<TSource> source, int count,
		Func<TSource, TKey> keySelector)
	{
		return source.DensePartialSortBy(count, keySelector, comparer: null);
	}

	/// <summary>
	///	    Executes a <paramref name="direction"/> partial sort of the top <paramref name="count"/> elements of a
	///     sequence, including ties, according to the key for each element. If <paramref name="count"/> is less than
	///     the total number of elements in <paramref name="source"/>, then this method will improve performance.
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
	///	    <paramref name="count"/> is less than 1.</exception>
	/// <remarks>
	/// <para>
	///	    This is an <c>O(n * log(K))</c> operation where <c>K</c> is <paramref name="count"/>.
	/// </para>
	/// <para>
	///	    This method is implemented by using deferred execution. However, <paramref name="source"/> will be consumed
	///     in it's entirety immediately when first element of the returned sequence is consumed. 
	/// </para>
	/// <para>
	///	    This method performs a stable sort; that is, if the keys of two elements are equal, the order of the
	///     elements is preserved. In contrast, an unstable sort does not preserve the order of elements that have the
	///     same key.
	/// </para>
	/// </remarks>
	public static IEnumerable<TSource> DensePartialSortBy<TSource, TKey>(
		this IEnumerable<TSource> source, int count,
		Func<TSource, TKey> keySelector, OrderByDirection direction)
	{
		return source.DensePartialSortBy(count, keySelector, comparer: null, direction);
	}

	/// <summary>
	///	    Executes a partial sort of the top <paramref name="count"/> elements of a sequence, including ties,
	///     according to the key for each element, using <paramref name="comparer"/> to compare the keys. If <paramref
	///     name="count"/> is less than the total number of elements in <paramref name="source"/>, then this method will
	///     improve performance.
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
	///	    <paramref name="count"/> is less than 1.</exception>
	/// <remarks>
	/// <para>
	///	    This is an <c>O(n * log(K))</c> operation where <c>K</c> is <paramref name="count"/>.
	/// </para>
	/// <para>
	///	    This method is implemented by using deferred execution. However, <paramref name="source"/> will be consumed
	///     in it's entirety immediately when first element of the returned sequence is consumed. 
	/// </para>
	/// <para>
	///	    This method performs a stable sort; that is, if the keys of two elements are equal, the order of the
	///     elements is preserved. In contrast, an unstable sort does not preserve the order of elements that have the
	///     same key.
	/// </para>
	/// </remarks>
	public static IEnumerable<TSource> DensePartialSortBy<TSource, TKey>(
		this IEnumerable<TSource> source, int count,
		Func<TSource, TKey> keySelector,
		IComparer<TKey>? comparer)
	{
		return DensePartialSortBy(source, count, keySelector, comparer, OrderByDirection.Ascending);
	}

	/// <summary>
	///	    Executes a <paramref name="direction"/> partial sort of the top <paramref name="count"/> elements of a
	///     sequence, including ties, according to the key for each element, using <paramref name="comparer"/> to
	///     compare the keys. If <paramref name="count"/> is less than the total number of elements in <paramref
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
	/// <param name="comparer">
	///	    A <see cref="IComparer{T}"/> to compare elements.
	/// </param>
	/// <param name="direction">
	///	    The direction in which to sort the elements
	/// </param>
	/// <returns>
	///	    A sequence containing at most top <paramref name="count"/> elements from source, in the specified order of
	///     their keys.</returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="keySelector"/> is <see langword="null"/>.
	/// </exception>
	/// <exception cref="ArgumentOutOfRangeException">
	///	    <paramref name="count"/> is less than 1.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This is an <c>O(n * log(K))</c> operation where <c>K</c> is <paramref name="count"/>.
	/// </para>
	/// <para>
	///	    This method is implemented by using deferred execution. However, <paramref name="source"/> will be consumed
	///     in it's entirety immediately when first element of the returned sequence is consumed. 
	/// </para>
	/// <para>
	///	    This method performs a stable sort; that is, if the keys of two elements are equal, the order of the
	///     elements is preserved. In contrast, an unstable sort does not preserve the order of elements that have the
	///     same key.
	/// </para>
	/// </remarks>
	public static IEnumerable<TSource> DensePartialSortBy<TSource, TKey>(
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
			var top = new SortedSet<TKey>(comparer);
			var dic = new NullKeyDictionary<TKey, List<TSource>>(count);

			foreach (var item in source)
			{
				var key = keySelector(item);
				if (top.TryGetValue(key, out var oKey))
				{
					dic[oKey].Add(item);
					continue;
				}

				if (top.Count < count)
				{
					_ = top.Add(key);
					dic[key] = new() { item, };
					continue;
				}

				var max = Debug.AssertNotNull(top.Max);
				if (comparer.Compare(key, max) > 0)
					continue;

				_ = dic.Remove(max);
				_ = top.Remove(max);
				_ = top.Add(key);
				dic[key] = new() { item, };
			}

			foreach (var entry in top)
			{
				foreach (var i in dic[entry])
					yield return i;
			}
		}
	}
}
