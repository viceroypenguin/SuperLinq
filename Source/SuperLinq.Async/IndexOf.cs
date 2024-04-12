namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Searches for the specified object and returns the zero-based index of the first occurrence within the entire
	/// <see cref="IAsyncEnumerable{T}"/>.
	/// </summary>
	/// <typeparam name="TSource">
	/// The type of elements of <paramref name="source"/></typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="item">The object to locate in the <see cref="IAsyncEnumerable{T}"/>. The value can be <see
	/// langword="null"/> for reference types.</param>
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any
	/// time.</param>
	/// <returns>
	/// The zero-based index of the first occurrence of <paramref name="item"/> within the entire <see
	/// cref="IAsyncEnumerable{T}"/>, if found; otherwise, <c>-1</c>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
	/// <remarks>
	/// <para>
	/// The <see cref="IAsyncEnumerable{T}"/> is searched forward starting at the first element and ending at the last
	/// element.
	/// </para>
	/// <para>
	/// This method determines equality using the default equality comparer <see cref="EqualityComparer{T}.Default"/>
	/// for <typeparamref name="TSource"/>, the type of values in the list.
	/// </para>
	/// <para>
	/// This operator executes immediately.
	/// </para>
	/// </remarks>
	public static ValueTask<int> IndexOf<TSource>(
		this IAsyncEnumerable<TSource> source,
		TSource item,
		CancellationToken cancellationToken = default
	)
	{
		return source.IndexOf(item, 0, int.MaxValue, cancellationToken);
	}

	/// <summary>
	/// Searches for the specified object and returns the zero-based index of the first occurrence within the range of
	/// elements in the <see cref="IAsyncEnumerable{T}"/> that extends from the specified index to the last element.
	/// </summary>
	/// <typeparam name="TSource">
	/// The type of elements of <paramref name="source"/></typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="item">The object to locate in the <see cref="IAsyncEnumerable{T}"/>. The value can be <see
	/// langword="null"/> for reference types.</param>
	/// <param name="index">The <see cref="System.Index"/> of the starting element within the sequence.</param>
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any
	/// time.</param>
	/// <returns>
	/// The zero-based index of the first occurrence of <paramref name="item"/> within the the range of elements in the
	/// <see cref="IAsyncEnumerable{T}"/> that extends from <paramref name="index"/> to the last element, if found;
	/// otherwise, <c>-1</c>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
	/// <remarks>
	/// <para>
	/// The <see cref="IAsyncEnumerable{T}"/> is searched forward starting at <paramref name="index"/> and ending at the
	/// last element.
	/// </para>
	/// <para>
	/// This method determines equality using the default equality comparer <see cref="EqualityComparer{T}.Default"/>
	/// for <typeparamref name="TSource"/>, the type of values in the list.
	/// </para>
	/// <para>
	/// This operator executes immediately.
	/// </para>
	/// </remarks>
	public static ValueTask<int> IndexOf<TSource>(
		this IAsyncEnumerable<TSource> source,
		TSource item,
		Index index,
		CancellationToken cancellationToken = default
	)
	{
		return source.IndexOf(item, index, int.MaxValue, cancellationToken);
	}

	/// <summary>
	/// Searches for the specified object and returns the zero-based index of the first occurrence within the range of
	/// elements in the <see cref="IAsyncEnumerable{T}"/> that starts at the specified index to the last element and
	/// contains the specified number of elements.
	/// </summary>
	/// <typeparam name="TSource">
	/// The type of elements of <paramref name="source"/></typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="item">The object to locate in the <see cref="IAsyncEnumerable{T}"/>. The value can be <see
	/// langword="null"/> for reference types.</param>
	/// <param name="index">The <see cref="System.Index"/> of the starting element within the sequence.</param>
	/// <param name="count">The number of elements in the section to search.</param>
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any
	/// time.</param>
	/// <returns>
	/// The zero-based index of the first occurrence of <paramref name="item"/> within the the range of elements in the
	/// <see cref="IAsyncEnumerable{T}"/> that that starts at <paramref name="index"/> and contains <paramref
	/// name="count"/> number of elements, if found; otherwise, <c>-1</c>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is less than <c>0</c>.</exception>
	/// <remarks>
	/// <para>
	/// The <see cref="IAsyncEnumerable{T}"/> is searched forward starting at <paramref name="index"/> and ending at
	/// <paramref name="index"/> plus <paramref name="count"/> minus <c>1</c>, if count is greater than <c>0</c>.
	/// </para>
	/// <para>
	/// This method determines equality using the default equality comparer <see cref="EqualityComparer{T}.Default"/>
	/// for <typeparamref name="TSource"/>, the type of values in the list.
	/// </para>
	/// <para>
	/// This operator executes immediately.
	/// </para>
	/// </remarks>
	public static ValueTask<int> IndexOf<TSource>(
		this IAsyncEnumerable<TSource> source,
		TSource item,
		Index index,
		int count,
		CancellationToken cancellationToken = default
	)
	{
		return FindIndex(source, i => EqualityComparer<TSource>.Default.Equals(i, item), index, count, cancellationToken);
	}
}
