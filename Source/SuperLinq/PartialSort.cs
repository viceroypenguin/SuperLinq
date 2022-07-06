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
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than 1.</exception>
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
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than 1.</exception>
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
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than 1.</exception>
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
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than 1.</exception>
	/// <remarks>
	/// This operator uses deferred execution and streams it results.
	/// </remarks>
	public static IEnumerable<T> PartialSort<T>(
		this IEnumerable<T> source, int count,
		IComparer<T>? comparer, OrderByDirection direction)
	{
		source.ThrowIfNull();
		count.ThrowIfLessThan(1);

		comparer ??= Comparer<T>.Default;
		if (direction == OrderByDirection.Descending)
			comparer = new ReverseComparer<T>(comparer);

		return _(source, count, comparer);

		static IEnumerable<T> _(IEnumerable<T> source, int count, IComparer<T> comparer)
		{
			var top = new SortedSet<(T item, int index)>(
				Comparer<(T item, int index)>.Create((x, y) =>
				{
					var result = comparer.Compare(x.item, y.item);
					return result != 0 ? result :
						Comparer<long>.Default.Compare(x.index, y.index);
				}));

			foreach (var (index, item) in source.Index())
			{
				if (top.Count < count)
				{
					top.Add((item, index));
					continue;
				}

				if (comparer.Compare(item, top.Max.item) >= 0)
					continue;

				top.Remove(top.Max);
				top.Add((item, index));
			}

			foreach (var (item, _) in top)
				yield return item;
		}
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
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="keySelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than 1.</exception>
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
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="keySelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than 1.</exception>
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
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="keySelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than 1.</exception>
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
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="keySelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than 1.</exception>
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
		count.ThrowIfLessThan(1);
		keySelector.ThrowIfNull();

		comparer ??= Comparer<TKey>.Default;
		if (direction == OrderByDirection.Descending)
			comparer = new ReverseComparer<TKey>(comparer);

		return _(source, count, keySelector, comparer);

		static IEnumerable<TSource> _(IEnumerable<TSource> source, int count, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
		{
			var top = new SortedSet<(TKey Item, int Index)>(
				Comparer<(TKey item, int index)>.Create((x, y) =>
				{
					var result = comparer.Compare(x.item, y.item);
					return result != 0 ? result :
						Comparer<long>.Default.Compare(x.index, y.index);
				}));
			var dic = new Dictionary<(TKey Item, int Index), TSource>(count);

			foreach (var (index, item) in source.Index())
			{
				var key = (key: keySelector(item), index);
				if (top.Count < count)
				{
					top.Add(key);
					dic[key] = item;
					continue;
				}

				if (comparer.Compare(key.key, top.Max.Item) >= 0)
					continue;

				dic.Remove(top.Max);
				top.Remove(top.Max);
				top.Add(key);
				dic[key] = item;
			}

			foreach (var entry in top)
				yield return dic[entry];
		}
	}
}
