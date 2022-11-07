namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Executes a partial sort of the top <paramref name="count"/> elements of a sequence, including ties. If <paramref
	/// name="count"/> is less than the total number of elements in <paramref name="source"/>, then this method will
	/// improve performance.
	/// </summary>
	/// <typeparam name="T">Type of elements in the sequence.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="count">Number of (maximum) elements to return.</param>
	/// <returns>A sequence containing at most top <paramref name="count"/> elements from source, in their ascending
	/// order.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than 1.</exception>
	/// <remarks>
	/// <para>
	/// This operation is an <c>O(n * log(K))</c> where <c>K</c> is <paramref name="count"/>.
	/// </para>
	/// <para>
	/// This operator uses deferred execution and streams it results.
	/// </para>
	/// </remarks>
	public static IEnumerable<T> DensePartialSort<T>(this IEnumerable<T> source, int count)
	{
		return source.DensePartialSort(count, comparer: null);
	}

	/// <summary>
	/// Executes a <paramref name="direction"/> partial sort of the top <paramref name="count"/> elements of a sequence,
	/// including ties. If <paramref name="count"/> is less than the total number of elements in <paramref
	/// name="source"/>, then this method will improve performance.
	/// </summary>
	/// <typeparam name="T">Type of elements in the sequence.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="count">Number of (maximum) elements to return.</param>
	/// <param name="direction">The direction in which to sort the elements</param>
	/// <returns>A sequence containing at most top <paramref name="count"/> elements from source, in the specified
	/// order.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than 1.</exception>
	/// <remarks>
	/// <para>
	/// This operation is an <c>O(n * log(K))</c> where <c>K</c> is <paramref name="count"/>.
	/// </para>
	/// <para>
	/// This operator uses deferred execution and streams it results.
	/// </para>
	/// </remarks>
	public static IEnumerable<T> DensePartialSort<T>(
		this IEnumerable<T> source, int count, OrderByDirection direction)
	{
		return source.DensePartialSort(count, comparer: null, direction);
	}

	/// <summary>
	/// Executes a partial sort of the top <paramref name="count"/> elements of a sequence, including ties, using
	/// <paramref name="comparer"/> to compare elements. If <paramref name="count"/> is less than the total number of
	/// elements in <paramref name="source"/>, then this method will improve performance.
	/// </summary>
	/// <typeparam name="T">Type of elements in the sequence.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="count">Number of (maximum) elements to return.</param>
	/// <param name="comparer">A <see cref="IComparer{T}"/> to compare elements.</param>
	/// <returns>A sequence containing at most top <paramref name="count"/> elements from source, in their ascending
	/// order.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than 1.</exception>
	/// <remarks>
	/// <para>
	/// This operation is an <c>O(n * log(K))</c> where <c>K</c> is <paramref name="count"/>.
	/// </para>
	/// <para>
	/// This operator uses deferred execution and streams it results.
	/// </para>
	/// </remarks>
	public static IEnumerable<T> DensePartialSort<T>(
		this IEnumerable<T> source,
		int count, IComparer<T>? comparer)
	{
		return DensePartialSort(source, count, comparer, OrderByDirection.Ascending);
	}

	/// <summary>
	/// Executes a <paramref name="direction"/> partial sort of the top <paramref name="count"/> elements of a sequence,
	/// including ties, using <paramref name="comparer"/> to compare elements. If <paramref name="count"/> is less than
	/// the total number of elements in <paramref name="source"/>, then this method will improve performance.
	/// </summary>
	/// <typeparam name="T">Type of elements in the sequence.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="count">Number of (maximum) elements to return.</param>
	/// <param name="comparer">A <see cref="IComparer{T}"/> to compare elements.</param>
	/// <param name="direction">The direction in which to sort the elements</param>
	/// <returns>A sequence containing at most top <paramref name="count"/> elements from source, in the specified
	/// order.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than 1.</exception>
	/// <remarks>
	/// <para>
	/// This operation is an <c>O(n * log(K))</c> where <c>K</c> is <paramref name="count"/>.
	/// </para>
	/// <para>
	/// This operator uses deferred execution and streams it results.
	/// </para>
	/// </remarks>
	public static IEnumerable<T> DensePartialSort<T>(
		this IEnumerable<T> source, int count,
		IComparer<T>? comparer, OrderByDirection direction)
	{
		return DensePartialSortBy(source, count, Identity, comparer, direction);
	}

	/// <summary>
	/// Executes a partial sort of the top <paramref name="count"/> elements of a sequence, including ties, according to
	/// the key for each element. If <paramref name="count"/> is less than the total number of elements in <paramref
	/// name="source"/>, then this method will improve performance.
	/// </summary>
	/// <typeparam name="TSource">Type of elements in the sequence.</typeparam>
	/// <typeparam name="TKey">Type of keys.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="keySelector">A function to extract a key from an element.</param>
	/// <param name="count">Number of (maximum) elements to return.</param>
	/// <returns>A sequence containing at most top <paramref name="count"/> elements from source, in ascending order of
	/// their keys.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="keySelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than 1.</exception>
	/// <remarks>
	/// <para>
	/// This operation is an <c>O(n * log(K))</c> where <c>K</c> is <paramref name="count"/>.
	/// </para>
	/// <para>
	/// This operator uses deferred execution and streams it results.
	/// </para>
	/// </remarks>
	public static IEnumerable<TSource> DensePartialSortBy<TSource, TKey>(
		this IEnumerable<TSource> source, int count,
		Func<TSource, TKey> keySelector)
	{
		return source.DensePartialSortBy(count, keySelector, comparer: null);
	}

	/// <summary>
	/// Executes a <paramref name="direction"/> partial sort of the top <paramref name="count"/> elements of a sequence,
	/// including ties, according to the key for each element. If <paramref name="count"/> is less than the total number
	/// of elements in <paramref name="source"/>, then this method will improve performance.
	/// </summary>
	/// <typeparam name="TSource">Type of elements in the sequence.</typeparam>
	/// <typeparam name="TKey">Type of keys.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="keySelector">A function to extract a key from an element.</param>
	/// <param name="count">Number of (maximum) elements to return.</param>
	/// <param name="direction">The direction in which to sort the elements</param>
	/// <returns>A sequence containing at most top <paramref name="count"/> elements from source, in the specified order
	/// of their keys.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="keySelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than 1.</exception>
	/// <remarks>
	/// <para>
	/// This operation is an <c>O(n * log(K))</c> where <c>K</c> is <paramref name="count"/>.
	/// </para>
	/// <para>
	/// This operator uses deferred execution and streams it results.
	/// </para>
	/// </remarks>
	public static IEnumerable<TSource> DensePartialSortBy<TSource, TKey>(
		this IEnumerable<TSource> source, int count,
		Func<TSource, TKey> keySelector, OrderByDirection direction)
	{
		return source.DensePartialSortBy(count, keySelector, comparer: null, direction);
	}

	/// <summary>
	/// Executes a partial sort of the top <paramref name="count"/> elements of a sequence, including ties, according to
	/// the key for each element, using <paramref name="comparer"/> to compare the keys. If <paramref name="count"/> is
	/// less than the total number of elements in <paramref name="source"/>, then this method will improve performance.
	/// </summary>
	/// <typeparam name="TSource">Type of elements in the sequence.</typeparam>
	/// <typeparam name="TKey">Type of keys.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="keySelector">A function to extract a key from an element.</param>
	/// <param name="count">Number of (maximum) elements to return.</param>
	/// <param name="comparer">A <see cref="IComparer{T}"/> to compare elements.</param>
	/// <returns>A sequence containing at most top <paramref name="count"/> elements from source, in ascending order of
	/// their keys.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="keySelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than 1.</exception>
	/// <remarks>
	/// <para>
	/// This operation is an <c>O(n * log(K))</c> where <c>K</c> is <paramref name="count"/>.
	/// </para>
	/// <para>
	/// This operator uses deferred execution and streams it results.
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
	/// Executes a <paramref name="direction"/> partial sort of the top <paramref name="count"/> elements of a sequence,
	/// including ties, according to the key for each element, using <paramref name="comparer"/> to compare the keys. If
	/// <paramref name="count"/> is less than the total number of elements in <paramref name="source"/>, then this
	/// method will improve performance.
	/// </summary>
	/// <typeparam name="TSource">Type of elements in the sequence.</typeparam>
	/// <typeparam name="TKey">Type of keys.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="keySelector">A function to extract a key from an element.</param>
	/// <param name="count">Number of (maximum) elements to return.</param>
	/// <param name="comparer">A <see cref="IComparer{T}"/> to compare elements.</param>
	/// <param name="direction">The direction in which to sort the elements</param>
	/// <returns>A sequence containing at most top <paramref name="count"/> elements from source, in the specified order
	/// of their keys.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="keySelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than 1.</exception>
	/// <remarks>
	/// <para>
	/// This operation is an <c>O(n * log(K))</c> where <c>K</c> is <paramref name="count"/>.
	/// </para>
	/// <para>
	/// This operator uses deferred execution and streams it results.
	/// </para>
	/// </remarks>
	public static IEnumerable<TSource> DensePartialSortBy<TSource, TKey>(
		this IEnumerable<TSource> source, int count,
		Func<TSource, TKey> keySelector,
		IComparer<TKey>? comparer,
		OrderByDirection direction)
	{
		Guard.IsNotNull(source);
		Guard.IsGreaterThanOrEqualTo(count, 1);
		Guard.IsNotNull(keySelector);

		comparer ??= Comparer<TKey>.Default;
		if (direction == OrderByDirection.Descending)
			comparer = new ReverseComparer<TKey>(comparer);

		return _(source, count, keySelector, comparer);

		static IEnumerable<TSource> _(IEnumerable<TSource> source, int count, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
		{
			var top = new SortedSet<TKey>(comparer);
			var dic = new Dictionary<(TKey Key, int Index), List<TSource>>(count);

			foreach (var item in source)
			{
				var key = keySelector(item);
				if (top.TryGetValue(key, out var oKey))
				{
					dic[(oKey, 1)].Add(item);
					continue;
				}

				if (top.Count < count)
				{
					top.Add(key);
					dic[(key, 1)] = new() { item, };
					continue;
				}

				var max = top.Max!;
				if (comparer.Compare(key, max) > 0)
					continue;

				dic.Remove((max, 1));
				top.Remove(max);
				top.Add(key);
				dic[(key, 1)] = new() { item, };
			}

			foreach (var entry in top)
				foreach (var i in dic[(entry, 1)])
					yield return i;
		}
	}
}
