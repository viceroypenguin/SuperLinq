﻿namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Partitions or splits a sequence in two using a predicate.
	/// </summary>
	/// <param name="source">
	///	    The source sequence.
	/// </param>
	/// <param name="predicate">
	///	    The predicate function.
	/// </param>
	/// <typeparam name="T">
	///	    Type of source elements.
	/// </typeparam>
	/// <returns>
	///	    A tuple of elements satisfying the predicate and those that do not, respectively.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This method executes immediately.
	/// </para>
	/// </remarks>
	public static (IEnumerable<T> True, IEnumerable<T> False)
		Partition<T>(this IEnumerable<T> source, Func<T, bool> predicate)
	{
		return source.Partition(predicate, ValueTuple.Create);
	}

	/// <summary>
	///	    Partitions or splits a sequence in two using a predicate and then projects a result from the two.
	/// </summary>
	/// <param name="source">
	///	    The source sequence.
	/// </param>
	/// <param name="predicate">
	///	    The predicate function.
	/// </param>
	/// <param name="resultSelector">
	///	    Function that projects the result from sequences of elements that satisfy the predicate and those that do
	///     not, respectively, passed as arguments.
	/// </param>
	/// <typeparam name="T">
	///	    Type of source elements.
	/// </typeparam>
	/// <typeparam name="TResult">
	///	    Type of the result.
	/// </typeparam>
	/// <returns>
	///	    The return value from <paramref name="resultSelector"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/>, <paramref name="predicate"/>, or <paramref name="resultSelector"/> is <see
	///     langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This method executes immediately.
	/// </para>
	/// </remarks>
	public static TResult Partition<T, TResult>(
		this IEnumerable<T> source,
		Func<T, bool> predicate,
		Func<IEnumerable<T>, IEnumerable<T>, TResult> resultSelector)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(predicate);
		ArgumentNullException.ThrowIfNull(resultSelector);

		var lookup = source.ToLookup(predicate);
		return resultSelector(lookup[true], lookup[false]);
	}

	/// <summary>
	/// Partitions a grouping by Boolean keys into a projection of true
	/// elements and false elements, respectively.
	/// </summary>
	/// <typeparam name="T">Type of elements in source groupings.</typeparam>
	/// <typeparam name="TResult">Type of the result.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="resultSelector">
	/// Function that projects the result from sequences of true elements
	/// and false elements, respectively, passed as arguments.
	/// </param>
	/// <returns>
	/// The return value from <paramref name="resultSelector"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
	/// <exception cref="ArgumentNullException"><paramref name="resultSelector"/> is null</exception>
	[Obsolete("Better implemented via `.ToLookup()`; will be removed in v6.0.0")]
	public static TResult Partition<T, TResult>(
		this IEnumerable<IGrouping<bool, T>> source,
		Func<IEnumerable<T>, IEnumerable<T>, TResult> resultSelector)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(resultSelector);

		return source.Partition(
			key1: true, key2: false,
			(t, f, _) => resultSelector(t, f));
	}

	/// <summary>
	/// Partitions a grouping by nullable Boolean keys into a projection of
	/// true elements, false elements and null elements, respectively.
	/// </summary>
	/// <typeparam name="T">Type of elements in source groupings.</typeparam>
	/// <typeparam name="TResult">Type of the result.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="resultSelector">
	/// Function that projects the result from sequences of true elements,
	/// false elements and null elements, respectively, passed as
	/// arguments.
	/// </param>
	/// <returns>
	/// The return value from <paramref name="resultSelector"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
	/// <exception cref="ArgumentNullException"><paramref name="resultSelector"/> is null</exception>
	[Obsolete("Better implemented via `.ToLookup()`; will be removed in v6.0.0")]
	public static TResult Partition<T, TResult>(
		this IEnumerable<IGrouping<bool?, T>> source,
		Func<IEnumerable<T>, IEnumerable<T>, IEnumerable<T>, TResult> resultSelector)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(resultSelector);

		return source.Partition(
			key1: true, key2: false, key3: null,
			(t, f, n, _) => resultSelector(t, f, n));
	}

	/// <summary>
	/// Partitions a grouping and projects a result from group elements
	/// matching a key and those groups that do not.
	/// </summary>
	/// <typeparam name="TKey">Type of keys in source groupings.</typeparam>
	/// <typeparam name="TElement">Type of elements in source groupings.</typeparam>
	/// <typeparam name="TResult">Type of the result.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="key">The key to partition.</param>
	/// <param name="resultSelector">
	/// Function that projects the result from sequences of elements
	/// matching <paramref name="key"/> and those groups that do not (in
	/// the order in which they appear in <paramref name="source"/>),
	/// passed as arguments.
	/// </param>
	/// <returns>
	/// The return value from <paramref name="resultSelector"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
	/// <exception cref="ArgumentNullException"><paramref name="resultSelector"/> is null</exception>
	[Obsolete("Better implemented via `.ToLookup()`; will be removed in v6.0.0")]
	public static TResult Partition<TKey, TElement, TResult>(
		this IEnumerable<IGrouping<TKey, TElement>> source,
		TKey key,
		Func<IEnumerable<TElement>, IEnumerable<IGrouping<TKey, TElement>>, TResult> resultSelector)
	{
		return Partition(source, key, comparer: default, resultSelector);
	}

	/// <summary>
	/// Partitions a grouping and projects a result from group elements
	/// matching a key and those groups that do not. An additional parameter
	/// specifies how to compare keys for equality.
	/// </summary>
	/// <typeparam name="TKey">Type of keys in source groupings.</typeparam>
	/// <typeparam name="TElement">Type of elements in source groupings.</typeparam>
	/// <typeparam name="TResult">Type of the result.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="key">The key to partition on.</param>
	/// <param name="comparer">The comparer for keys.</param>
	/// <param name="resultSelector">
	/// Function that projects the result from elements of the group
	/// matching <paramref name="key"/> and those groups that do not (in
	/// the order in which they appear in <paramref name="source"/>),
	/// passed as arguments.
	/// </param>
	/// <returns>
	/// The return value from <paramref name="resultSelector"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
	/// <exception cref="ArgumentNullException"><paramref name="resultSelector"/> is null</exception>
	[Obsolete("Better implemented via `.ToLookup()`; will be removed in v6.0.0")]
	public static TResult Partition<TKey, TElement, TResult>(
		this IEnumerable<IGrouping<TKey, TElement>> source,
		TKey key, IEqualityComparer<TKey>? comparer,
		Func<IEnumerable<TElement>, IEnumerable<IGrouping<TKey, TElement>>, TResult> resultSelector)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(resultSelector);

		return PartitionImpl(
			source, 1, key, key2: default, key3: default, comparer,
			(a, _, _, rest) => resultSelector(a, rest));
	}

	/// <summary>
	/// Partitions a grouping and projects a result from elements of
	/// groups matching a set of two keys and those groups that do not.
	/// </summary>
	/// <typeparam name="TKey">Type of keys in source groupings.</typeparam>
	/// <typeparam name="TElement">Type of elements in source groupings.</typeparam>
	/// <typeparam name="TResult">Type of the result.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="key1">The first key to partition on.</param>
	/// <param name="key2">The second key to partition on.</param>
	/// <param name="resultSelector">
	/// Function that projects the result from elements of the group
	/// matching <paramref name="key1"/>, elements of the group matching
	/// <paramref name="key2"/> and those groups that do not (in the order
	/// in which they appear in <paramref name="source"/>), passed as
	/// arguments.
	/// </param>
	/// <returns>
	/// The return value from <paramref name="resultSelector"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
	/// <exception cref="ArgumentNullException"><paramref name="resultSelector"/> is null</exception>
	[Obsolete("Better implemented via `.ToLookup()`; will be removed in v6.0.0")]
	public static TResult Partition<TKey, TElement, TResult>(
		this IEnumerable<IGrouping<TKey, TElement>> source,
		TKey key1, TKey key2,
		Func<IEnumerable<TElement>, IEnumerable<TElement>, IEnumerable<IGrouping<TKey, TElement>>, TResult> resultSelector)
	{
		return Partition(source, key1, key2, comparer: default, resultSelector);
	}

	/// <summary>
	/// Partitions a grouping and projects a result from elements of
	/// groups matching a set of two keys and those groups that do not.
	/// An additional parameter specifies how to compare keys for equality.
	/// </summary>
	/// <typeparam name="TKey">Type of keys in source groupings.</typeparam>
	/// <typeparam name="TElement">Type of elements in source groupings.</typeparam>
	/// <typeparam name="TResult">Type of the result.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="key1">The first key to partition on.</param>
	/// <param name="key2">The second key to partition on.</param>
	/// <param name="comparer">The comparer for keys.</param>
	/// <param name="resultSelector">
	/// Function that projects the result from elements of the group
	/// matching <paramref name="key1"/>, elements of the group matching
	/// <paramref name="key2"/> and those groups that do not (in the order
	/// in which they appear in <paramref name="source"/>), passed as
	/// arguments.
	/// </param>
	/// <returns>
	/// The return value from <paramref name="resultSelector"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
	/// <exception cref="ArgumentNullException"><paramref name="resultSelector"/> is null</exception>
	[Obsolete("Better implemented via `.ToLookup()`; will be removed in v6.0.0")]
	public static TResult Partition<TKey, TElement, TResult>(
		this IEnumerable<IGrouping<TKey, TElement>> source,
		TKey key1, TKey key2, IEqualityComparer<TKey>? comparer,
		Func<IEnumerable<TElement>, IEnumerable<TElement>, IEnumerable<IGrouping<TKey, TElement>>, TResult> resultSelector)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(resultSelector);

		return PartitionImpl(
			source, 2, key1, key2, key3: default, comparer,
			(a, b, c, rest) => resultSelector(a, b, rest));
	}

	/// <summary>
	/// Partitions a grouping and projects a result from elements groups
	/// matching a set of three keys and those groups that do not.
	/// </summary>
	/// <typeparam name="TKey">Type of keys in source groupings.</typeparam>
	/// <typeparam name="TElement">Type of elements in source groupings.</typeparam>
	/// <typeparam name="TResult">Type of the result.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="key1">The first key to partition on.</param>
	/// <param name="key2">The second key to partition on.</param>
	/// <param name="key3">The third key to partition on.</param>
	/// <param name="resultSelector">
	/// Function that projects the result from elements of groups
	/// matching <paramref name="key1"/>, <paramref name="key2"/> and
	/// <paramref name="key3"/> and those groups that do not (in the order
	/// in which they appear in <paramref name="source"/>), passed as
	/// arguments.
	/// </param>
	/// <returns>
	/// The return value from <paramref name="resultSelector"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
	/// <exception cref="ArgumentNullException"><paramref name="resultSelector"/> is null</exception>
	[Obsolete("Better implemented via `.ToLookup()`; will be removed in v6.0.0")]
	public static TResult Partition<TKey, TElement, TResult>(
		this IEnumerable<IGrouping<TKey, TElement>> source,
		TKey key1, TKey key2, TKey key3,
		Func<IEnumerable<TElement>, IEnumerable<TElement>, IEnumerable<TElement>, IEnumerable<IGrouping<TKey, TElement>>, TResult> resultSelector)
	{
		return Partition(source, key1, key2, key3, comparer: default, resultSelector);
	}

	/// <summary>
	/// Partitions a grouping and projects a result from elements groups
	/// matching a set of three keys and those groups that do not. An
	/// additional parameter specifies how to compare keys for equality.
	/// </summary>
	/// <typeparam name="TKey">Type of keys in source groupings.</typeparam>
	/// <typeparam name="TElement">Type of elements in source groupings.</typeparam>
	/// <typeparam name="TResult">Type of the result.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="key1">The first key to partition on.</param>
	/// <param name="key2">The second key to partition on.</param>
	/// <param name="key3">The third key to partition on.</param>
	/// <param name="comparer">The comparer for keys.</param>
	/// <param name="resultSelector">
	/// Function that projects the result from elements of groups
	/// matching <paramref name="key1"/>, <paramref name="key2"/> and
	/// <paramref name="key3"/> and those groups that do not (in
	/// the order in which they appear in <paramref name="source"/>),
	/// passed as arguments.
	/// </param>
	/// <returns>
	/// The return value from <paramref name="resultSelector"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
	/// <exception cref="ArgumentNullException"><paramref name="resultSelector"/> is null</exception>
	[Obsolete("Better implemented via `.ToLookup()`; will be removed in v6.0.0")]
	public static TResult Partition<TKey, TElement, TResult>(
		this IEnumerable<IGrouping<TKey, TElement>> source,
		TKey key1, TKey key2, TKey key3, IEqualityComparer<TKey>? comparer,
		Func<IEnumerable<TElement>, IEnumerable<TElement>, IEnumerable<TElement>, IEnumerable<IGrouping<TKey, TElement>>, TResult> resultSelector)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(resultSelector);

		return PartitionImpl(source, 3, key1, key2, key3, comparer, resultSelector);
	}

	private static TResult PartitionImpl<TKey, TElement, TResult>(
		IEnumerable<IGrouping<TKey, TElement>> source,
		int count, TKey? key1, TKey? key2, TKey? key3, IEqualityComparer<TKey>? comparer,
		Func<IEnumerable<TElement>, IEnumerable<TElement>, IEnumerable<TElement>, IEnumerable<IGrouping<TKey, TElement>>, TResult> resultSelector)
	{
		comparer ??= EqualityComparer<TKey>.Default;

		List<IGrouping<TKey, TElement>>? etc = null;

		var groups = new[]
		{
			Enumerable.Empty<TElement>(),
			Enumerable.Empty<TElement>(),
			Enumerable.Empty<TElement>(),
		};

		foreach (var e in source)
		{
			var i = count > 0 && comparer.Equals(e.Key, key1) ? 0
				  : count > 1 && comparer.Equals(e.Key, key2) ? 1
				  : count > 2 && comparer.Equals(e.Key, key3) ? 2
				  : -1;

			if (i < 0)
			{
				etc ??= new List<IGrouping<TKey, TElement>>();
				etc.Add(e);
			}
			else
			{
				groups[i] = e;
			}
		}

		return resultSelector(groups[0], groups[1], groups[2], etc ?? Enumerable.Empty<IGrouping<TKey, TElement>>());
	}
}
