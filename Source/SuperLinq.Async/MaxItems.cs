namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Returns all of the items that share the maximum value of a sequence.
	/// </summary>
	/// <typeparam name="T">Type of elements in the sequence.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <remarks>
	/// <para>
	/// This operator is a shortcut for <see cref="DensePartialSort{T}(IAsyncEnumerable{T}, int, OrderByDirection)"/> with a
	/// <c>direction</c> of <see cref="OrderByDirection.Descending"/> and a <c>count</c> of <c>1</c>. 
	/// </para>
	/// <para>
	/// This operator uses deferred execution and streams it results.
	/// </para>
	/// </remarks>
	public static IAsyncEnumerable<T> MaxItems<T>(this IAsyncEnumerable<T> source)
	{
		return source.DensePartialSort(1, OrderByDirection.Descending);
	}

	/// <summary>
	/// Returns all of the items that share the maximum value of a sequence.
	/// </summary>
	/// <typeparam name="T">Type of elements in the sequence.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="comparer">A <see cref="IComparer{T}"/> to compare elements.</param>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <remarks>
	/// <para>
	/// This operator is a shortcut for <see cref="DensePartialSort{T}(IAsyncEnumerable{T}, int, IComparer{T}?, OrderByDirection)"/> with a
	/// <c>count</c> of <c>1</c>. 
	/// </para>
	/// <para>
	/// This operator uses deferred execution and streams it results.
	/// </para>
	/// </remarks>
	public static IAsyncEnumerable<T> MaxItems<T>(this IAsyncEnumerable<T> source, IComparer<T>? comparer)
	{
		return source.DensePartialSort(1, comparer, OrderByDirection.Descending);
	}

	/// <summary>
	/// Returns all of the items that share the maximum value of a sequence.
	/// </summary>
	/// <typeparam name="TSource">Type of elements in the sequence.</typeparam>
	/// <typeparam name="TKey">Type of keys.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="keySelector">A function to extract a key from an element.</param>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <remarks>
	/// <para>
	/// This operator is a shortcut for <see cref="DensePartialSortBy{TSource, TKey}(IAsyncEnumerable{TSource}, int,
	/// Func{TSource, TKey}, OrderByDirection)"/> with a <c>direction</c> of <see cref="OrderByDirection.Descending"/>
	/// and a <c>count</c> of <c>1</c>. 
	/// </para>
	/// <para>
	/// This operator uses deferred execution and streams it results.
	/// </para>
	/// </remarks>
	public static IAsyncEnumerable<TSource> MaxItemsBy<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector)
	{
		return source.DensePartialSortBy(1, keySelector, OrderByDirection.Descending);
	}

	/// <summary>
	/// Returns all of the items that share the maximum value of a sequence.
	/// </summary>
	/// <typeparam name="TSource">Type of elements in the sequence.</typeparam>
	/// <typeparam name="TKey">Type of keys.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="keySelector">A function to extract a key from an element.</param>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <remarks>
	/// <para>
	/// This operator is a shortcut for <see cref="DensePartialSortBy{TSource, TKey}(IAsyncEnumerable{TSource}, int,
	/// Func{TSource, TKey}, OrderByDirection)"/> with a <c>direction</c> of <see cref="OrderByDirection.Descending"/>
	/// and a <c>count</c> of <c>1</c>. 
	/// </para>
	/// <para>
	/// This operator uses deferred execution and streams it results.
	/// </para>
	/// </remarks>
	public static IAsyncEnumerable<TSource> MaxByWithTies<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector)
	{
		return source.DensePartialSortBy(1, keySelector, OrderByDirection.Descending);
	}

	/// <summary>
	/// Returns all of the items that share the maximum value of a sequence.
	/// </summary>
	/// <typeparam name="TSource">Type of elements in the sequence.</typeparam>
	/// <typeparam name="TKey">Type of keys.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="keySelector">A function to extract a key from an element.</param>
	/// <param name="comparer">A <see cref="IComparer{T}"/> to compare keys.</param>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <remarks>
	/// <para>
	/// This operator is a shortcut for <see cref="DensePartialSortBy{TSource, TKey}(IAsyncEnumerable{TSource}, int,
	/// Func{TSource, TKey}, IComparer{TKey}?, OrderByDirection)"/> with a <c>direction</c> of <see
	/// cref="OrderByDirection.Descending"/> and a <c>count</c> of <c>1</c>. 
	/// </para>
	/// <para>
	/// This operator uses deferred execution and streams it results.
	/// </para>
	/// </remarks>
	public static IAsyncEnumerable<TSource> MaxItemsBy<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey>? comparer)
	{
		return source.DensePartialSortBy(1, keySelector, comparer, OrderByDirection.Descending);
	}

	/// <summary>
	/// Returns all of the items that share the maximum value of a sequence.
	/// </summary>
	/// <typeparam name="TSource">Type of elements in the sequence.</typeparam>
	/// <typeparam name="TKey">Type of keys.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="keySelector">A function to extract a key from an element.</param>
	/// <param name="comparer">A <see cref="IComparer{T}"/> to compare keys.</param>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <remarks>
	/// <para>
	/// This operator is a shortcut for <see cref="DensePartialSortBy{TSource, TKey}(IAsyncEnumerable{TSource}, int,
	/// Func{TSource, TKey}, IComparer{TKey}?, OrderByDirection)"/> with a <c>direction</c> of <see
	/// cref="OrderByDirection.Descending"/> and a <c>count</c> of <c>1</c>. 
	/// </para>
	/// <para>
	/// This operator uses deferred execution and streams it results.
	/// </para>
	/// </remarks>
	public static IAsyncEnumerable<TSource> MaxByWithTies<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey>? comparer)
	{
		return source.DensePartialSortBy(1, keySelector, comparer, OrderByDirection.Descending);
	}
}
