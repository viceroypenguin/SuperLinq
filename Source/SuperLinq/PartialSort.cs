namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Combines <see cref="Enumerable.OrderBy{TSource,TKey}(IEnumerable{TSource},Func{TSource,TKey})"/>,
	/// where each element is its key, and <see cref="Enumerable.Take{TSource}(IEnumerable{TSource}, int)"/>
	/// in a single operation.
	/// </summary>
	/// <typeparam name="T">Type of elements in the sequence.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="count">Number of (maximum) elements to return.</param>
	/// <returns>A sequence containing at most top <paramref name="count"/>
	/// elements from source, in their ascending order.</returns>
	/// <remarks>
	/// This operator uses deferred execution and streams it results.
	/// </remarks>

	public static IEnumerable<T> PartialSort<T>(this IEnumerable<T> source, int count)
	{
		return source.PartialSort(count, comparer: null);
	}

	/// <summary>
	/// Combines <see cref="SuperEnumerable.OrderBy{T, TKey}(IEnumerable{T}, Func{T, TKey}, IComparer{TKey}, OrderByDirection)"/>,
	/// where each element is its key, and <see cref="Enumerable.Take{TSource}(IEnumerable{TSource}, int)"/>
	/// in a single operation.
	/// An additional parameter specifies the direction of the sort
	/// </summary>
	/// <typeparam name="T">Type of elements in the sequence.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="count">Number of (maximum) elements to return.</param>
	/// <param name="direction">The direction in which to sort the elements</param>
	/// <returns>A sequence containing at most top <paramref name="count"/>
	/// elements from source, in the specified order.</returns>
	/// <remarks>
	/// This operator uses deferred execution and streams it results.
	/// </remarks>

	public static IEnumerable<T> PartialSort<T>(
		this IEnumerable<T> source, int count, OrderByDirection direction)
	{
		return source.PartialSort(count, comparer: null, direction);
	}

	/// <summary>
	/// Combines <see cref="Enumerable.OrderBy{TSource,TKey}(IEnumerable{TSource},Func{TSource,TKey},IComparer{TKey})"/>,
	/// where each element is its key, and <see cref="Enumerable.Take{TSource}(IEnumerable{TSource}, int)"/>
	/// in a single operation. An additional parameter specifies how the
	/// elements compare to each other.
	/// </summary>
	/// <typeparam name="T">Type of elements in the sequence.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="count">Number of (maximum) elements to return.</param>
	/// <param name="comparer">A <see cref="IComparer{T}"/> to compare elements.</param>
	/// <returns>A sequence containing at most top <paramref name="count"/>
	/// elements from source, in their ascending order.</returns>
	/// <remarks>
	/// This operator uses deferred execution and streams it results.
	/// </remarks>

	public static IEnumerable<T> PartialSort<T>(
		this IEnumerable<T> source,
		int count, IComparer<T>? comparer)
	{
		return PartialSort(source, count, comparer, OrderByDirection.Ascending);
	}

	/// <summary>
	/// Combines <see cref="SuperEnumerable.OrderBy{T, TKey}(IEnumerable{T}, Func{T, TKey}, IComparer{TKey}, OrderByDirection)"/>,
	/// where each element is its key, and <see cref="Enumerable.Take{TSource}(IEnumerable{TSource}, int)"/>
	/// in a single operation.
	/// Additional parameters specify how the elements compare to each other and
	/// the direction of the sort.
	/// </summary>
	/// <typeparam name="T">Type of elements in the sequence.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="count">Number of (maximum) elements to return.</param>
	/// <param name="comparer">A <see cref="IComparer{T}"/> to compare elements.</param>
	/// <param name="direction">The direction in which to sort the elements</param>
	/// <returns>A sequence containing at most top <paramref name="count"/>
	/// elements from source, in the specified order.</returns>
	/// <remarks>
	/// This operator uses deferred execution and streams it results.
	/// </remarks>

	public static IEnumerable<T> PartialSort<T>(
		this IEnumerable<T> source, int count,
		IComparer<T>? comparer, OrderByDirection direction)
	{
		source.ThrowIfNull();
		comparer ??= Comparer<T>.Default;
		if (direction == OrderByDirection.Descending)
			comparer = new ReverseComparer<T>(comparer);
		return PartialSortByImpl<T, T>(source, count, keySelector: null, keyComparer: null, comparer);
	}

	/// <summary>
	/// Combines <see cref="Enumerable.OrderBy{TSource,TKey}(IEnumerable{TSource},Func{TSource,TKey},IComparer{TKey})"/>,
	/// and <see cref="Enumerable.Take{TSource}(IEnumerable{TSource}, int)"/> in a single operation.
	/// </summary>
	/// <typeparam name="TSource">Type of elements in the sequence.</typeparam>
	/// <typeparam name="TKey">Type of keys.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="keySelector">A function to extract a key from an element.</param>
	/// <param name="count">Number of (maximum) elements to return.</param>
	/// <returns>A sequence containing at most top <paramref name="count"/>
	/// elements from source, in ascending order of their keys.</returns>
	/// <remarks>
	/// This operator uses deferred execution and streams it results.
	/// </remarks>

	public static IEnumerable<TSource> PartialSortBy<TSource, TKey>(
		this IEnumerable<TSource> source, int count,
		Func<TSource, TKey> keySelector)
	{
		return source.PartialSortBy(count, keySelector, comparer: null);
	}

	/// <summary>
	/// Combines <see cref="SuperEnumerable.OrderBy{T, TKey}(IEnumerable{T}, Func{T, TKey}, OrderByDirection)"/>,
	/// and <see cref="Enumerable.Take{TSource}(IEnumerable{TSource}, int)"/> in a single operation.
	/// An additional parameter specifies the direction of the sort
	/// </summary>
	/// <typeparam name="TSource">Type of elements in the sequence.</typeparam>
	/// <typeparam name="TKey">Type of keys.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="keySelector">A function to extract a key from an element.</param>
	/// <param name="count">Number of (maximum) elements to return.</param>
	/// <param name="direction">The direction in which to sort the elements</param>
	/// <returns>A sequence containing at most top <paramref name="count"/>
	/// elements from source, in the specified order of their keys.</returns>
	/// <remarks>
	/// This operator uses deferred execution and streams it results.
	/// </remarks>

	public static IEnumerable<TSource> PartialSortBy<TSource, TKey>(
		this IEnumerable<TSource> source, int count,
		Func<TSource, TKey> keySelector, OrderByDirection direction)
	{
		return source.PartialSortBy(count, keySelector, comparer: null, direction);
	}

	/// <summary>
	/// Combines <see cref="Enumerable.OrderBy{TSource,TKey}(IEnumerable{TSource},Func{TSource,TKey},IComparer{TKey})"/>,
	/// and <see cref="Enumerable.Take{TSource}(IEnumerable{TSource}, int)"/> in a single operation.
	/// An additional parameter specifies how the keys compare to each other.
	/// </summary>
	/// <typeparam name="TSource">Type of elements in the sequence.</typeparam>
	/// <typeparam name="TKey">Type of keys.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="keySelector">A function to extract a key from an element.</param>
	/// <param name="count">Number of (maximum) elements to return.</param>
	/// <param name="comparer">A <see cref="IComparer{T}"/> to compare elements.</param>
	/// <returns>A sequence containing at most top <paramref name="count"/>
	/// elements from source, in ascending order of their keys.</returns>
	/// <remarks>
	/// This operator uses deferred execution and streams it results.
	/// </remarks>

	public static IEnumerable<TSource> PartialSortBy<TSource, TKey>(
		this IEnumerable<TSource> source, int count,
		Func<TSource, TKey> keySelector,
		IComparer<TKey>? comparer)
	{
		return PartialSortBy(source, count, keySelector, comparer, OrderByDirection.Ascending);
	}

	/// <summary>
	/// Combines <see cref="SuperEnumerable.OrderBy{T, TKey}(IEnumerable{T}, Func{T, TKey}, OrderByDirection)"/>,
	/// and <see cref="Enumerable.Take{TSource}(IEnumerable{TSource}, int)"/> in a single operation.
	/// Additional parameters specify how the elements compare to each other and
	/// the direction of the sort.
	/// </summary>
	/// <typeparam name="TSource">Type of elements in the sequence.</typeparam>
	/// <typeparam name="TKey">Type of keys.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="keySelector">A function to extract a key from an element.</param>
	/// <param name="count">Number of (maximum) elements to return.</param>
	/// <param name="comparer">A <see cref="IComparer{T}"/> to compare elements.</param>
	/// <param name="direction">The direction in which to sort the elements</param>
	/// <returns>A sequence containing at most top <paramref name="count"/>
	/// elements from source, in the specified order of their keys.</returns>
	/// <remarks>
	/// This operator uses deferred execution and streams it results.
	/// </remarks>

	public static IEnumerable<TSource> PartialSortBy<TSource, TKey>(
		this IEnumerable<TSource> source, int count,
		Func<TSource, TKey> keySelector,
		IComparer<TKey>? comparer,
		OrderByDirection direction)
	{
		source.ThrowIfNull();
		keySelector.ThrowIfNull();

		comparer ??= Comparer<TKey>.Default;
		if (direction == OrderByDirection.Descending)
			comparer = new ReverseComparer<TKey>(comparer);
		return PartialSortByImpl(source, count, keySelector, keyComparer: comparer, comparer: null);
	}

	static IEnumerable<TSource> PartialSortByImpl<TSource, TKey>(
		IEnumerable<TSource> source, int count,
		Func<TSource, TKey>? keySelector,
		IComparer<TKey>? keyComparer,
		IComparer<TSource>? comparer)
	{
		var top = new List<TSource>(count);

		static int? Insert<T>(List<T> list, T item, IComparer<T> comparer, int count)
		{
			var i = list.BinarySearch(item, comparer);
			// find the place to insert
			if (i < 0 && (i = ~i) >= count)
				return null;
			// move forward until we get to next larger
			while (i < list.Count && comparer.Compare(item, list[i]) == 0)
				i++;
			// is the list full?
			if (list.Count == count)
			{
				// if our insert location is at the end of the list
				if (i == list.Count
					// and we're _not larger_ than the last item
					&& comparer.Compare(item, list[^1]) <= 0)
				{
					// then don't affect the list
					return null;
				}
				// remove last item
				list.RemoveAt(count - 1);
			}

			list.Insert(i, item);
			return i;
		}

		if (keyComparer != null)
		{
			var keys = new List<TKey>(count);

			foreach (var item in source)
			{
				var key = keySelector!(item);
				if (Insert(keys, key, keyComparer, count) is { } i)
				{
					if (top.Count == count)
						top.RemoveAt(count - 1);
					top.Insert(i, item);
				}
			}
		}
		else if (comparer != null)
		{
			foreach (var item in source)
				_ = Insert(top, item, comparer, count);
		}
		else
		{
			throw new NotSupportedException("Should not be able to reach here.");
		}

		foreach (var item in top)
			yield return item;
	}
}
