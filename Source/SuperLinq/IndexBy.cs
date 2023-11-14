﻿namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Applies a key-generating function to each element of a sequence and returns a sequence that contains the
	///     elements of the original sequence as well its key and index inside the group of its key.
	/// </summary>
	/// <typeparam name="TSource">
	///	    Type of the source sequence elements.
	/// </typeparam>
	/// <typeparam name="TKey">
	///	    Type of the projected key.
	/// </typeparam>
	/// <param name="source">
	///	    Source sequence.
	/// </param>
	/// <param name="keySelector">
	///	    Function that projects the key given an element in the source sequence.
	/// </param>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="keySelector"/> is <see langword="null"/>.
	/// </exception>
	/// <returns>
	///	    A sequence of elements paired with their index within the key-group. The index is the key and the element is
	///     the value of the pair.
	/// </returns>
	/// <remarks>
	/// <para>
	///	    This operator uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IEnumerable<(int index, TSource item)>
		IndexBy<TSource, TKey>(
			this IEnumerable<TSource> source,
			Func<TSource, TKey> keySelector)
	{
		return source.IndexBy(keySelector, comparer: null);
	}

	/// <summary>
	///	    Applies a key-generating function to each element of a sequence and returns a sequence that contains the
	///     elements of the original sequence as well its key and index inside the group of its key. An additional
	///     parameter specifies a comparer to use for testing the equivalence of keys.
	/// </summary>
	/// <typeparam name="TSource">
	///	    Type of the source sequence elements.
	/// </typeparam>
	/// <typeparam name="TKey">
	///	    Type of the projected key.
	/// </typeparam>
	/// <param name="source">
	///	    Source sequence.
	/// </param>
	/// <param name="keySelector">
	///	    Function that projects the key given an element in the source sequence.
	/// </param>
	/// <param name="comparer">
	///	    The equality comparer to use to determine whether or not keys are equal. If <see langword="null"/>, the
	///     default equality comparer for
	/// <typeparamref name="TSource"/> is used.
	/// </param>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="keySelector"/> is <see langword="null"/>.
	/// </exception>
	/// <returns>
	///	    A sequence of elements paired with their index within the key-group. The index is the key and the element is
	///     the value of the pair.
	/// </returns>
	/// <remarks>
	/// <para>
	///	    This operator uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IEnumerable<(int index, TSource item)>
		IndexBy<TSource, TKey>(
			this IEnumerable<TSource> source,
			Func<TSource, TKey> keySelector,
			IEqualityComparer<TKey>? comparer)
	{
		return source
			.ScanBy(
				keySelector,
				static k => (Index: -1, Item: default(TSource)!),
				static (s, k, e) => (s.Index + 1, e),
				comparer)
			.Select(e => e.state);
	}
}
