namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Returns all of the items that share the minimum value of a sequence.
	/// </summary>
	/// <typeparam name="T">Type of elements in the sequence.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <remarks>
	/// <para>
	/// This operator is a shortcut for <see cref="EnumerableEx.MinByWithTies{TSource, TKey}(IEnumerable{TSource},
	/// Func{TSource, TKey})" />.
	/// </para>
	/// <para>
	/// This operator uses deferred execution and streams it results.
	/// </para>
	/// </remarks>
	public static IEnumerable<T> MinItems<T>(this IEnumerable<T> source)
	{
		return source.MinByWithTies(Identity);
	}

	/// <summary>
	/// Returns all of the items that share the minimum value of a sequence.
	/// </summary>
	/// <typeparam name="T">Type of elements in the sequence.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="comparer">A <see cref="IComparer{T}"/> to compare elements.</param>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <remarks>
	/// <para>
	/// This operator is a shortcut for <see cref="EnumerableEx.MinByWithTies{TSource, TKey}(IEnumerable{TSource},
	/// Func{TSource, TKey}, IComparer{TKey})" />.
	/// </para>
	/// <para>
	/// This operator uses deferred execution and streams it results.
	/// </para>
	/// </remarks>
	public static IEnumerable<T> MinItems<T>(this IEnumerable<T> source, IComparer<T> comparer)
	{
		return source.MinByWithTies(Identity, comparer);
	}

	/// <summary>
	/// Returns all of the items that share the minimum value of a sequence.
	/// </summary>
	/// <typeparam name="TSource">Type of elements in the sequence.</typeparam>
	/// <typeparam name="TKey">Type of keys.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="keySelector">A function to extract a key from an element.</param>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <remarks>
	/// <para>
	/// This operator is a shortcut for <see cref="EnumerableEx.MinByWithTies{TSource, TKey}(IEnumerable{TSource},
	/// Func{TSource, TKey})" />.
	/// </para>
	/// <para>
	/// This operator uses deferred execution and streams it results.
	/// </para>
	/// </remarks>
	public static IEnumerable<TSource> MinItemsBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
	{
		return source.MinByWithTies(keySelector);
	}

	/// <summary>
	/// Returns all of the items that share the minimum value of a sequence.
	/// </summary>
	/// <typeparam name="TSource">Type of elements in the sequence.</typeparam>
	/// <typeparam name="TKey">Type of keys.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="keySelector">A function to extract a key from an element.</param>
	/// <param name="comparer">A <see cref="IComparer{T}"/> to compare keys.</param>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <remarks>
	/// <para>
	/// This operator is a shortcut for <see cref="EnumerableEx.MinByWithTies{TSource, TKey}(IEnumerable{TSource},
	/// Func{TSource, TKey}, IComparer{TKey})" />.
	/// </para>
	/// <para>
	/// This operator uses deferred execution and streams it results.
	/// </para>
	/// </remarks>
	public static IEnumerable<TSource> MinItemsBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
	{
		return source.MinByWithTies(keySelector, comparer);
	}
}
