namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Searches for the specified object and returns the zero-based index of the last occurrence within the entire <see
	/// cref="IAsyncEnumerable{T}"/>.
	/// </summary>
	/// <typeparam name="TSource">
	/// The type of elements of <paramref name="source"/></typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="item">The object to locate in the <see cref="IAsyncEnumerable{T}"/>. The value can be <see
	/// langword="null"/> for reference types.</param>
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any
	/// time.</param>
	/// <returns>
	/// The zero-based index of the last occurrence of <paramref name="item"/> within the entire <see
	/// cref="IAsyncEnumerable{T}"/>, if found; otherwise, <c>-1</c>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
	/// <remarks>
	/// <para>
	/// The <see cref="IAsyncEnumerable{T}"/> is searched forward starting at the first element and ending at the last
	/// element, and the index of the last instance of <paramref name="item"/> is returned.
	/// </para>
	/// <para>
	/// This method determines equality using the default equality comparer <see cref="EqualityComparer{T}.Default"/>
	/// for <typeparamref name="TSource"/>, the type of values in the list.
	/// </para>
	/// <para>
	/// This operator executes immediately.
	/// </para>
	/// </remarks>
	public static ValueTask<int> LastIndexOf<TSource>(this IAsyncEnumerable<TSource> source, TSource item, CancellationToken cancellationToken = default)
	{
		return source.LastIndexOf(item, ^1, int.MaxValue, cancellationToken);
	}

	/// <summary>
	/// Searches for the specified object and returns the zero-based index of the last occurrence within the range of
	/// elements in the <see cref="IAsyncEnumerable{T}"/> that extends backwards from the specified index to the first
	/// element.
	/// </summary>
	/// <typeparam name="TSource">
	/// The type of elements of <paramref name="source"/></typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="item">The object to locate in the <see cref="IAsyncEnumerable{T}"/>. The value can be <see
	/// langword="null"/> for reference types.</param>
	/// <param name="index">The <see cref="System.Index"/> of the ending element within the sequence.</param>
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any
	/// time.</param>
	/// <returns>
	/// The zero-based index of the last occurrence of <paramref name="item"/> within the the range of elements in the
	/// <see cref="IAsyncEnumerable{T}"/> that extends backwards from <paramref name="index"/> to the first element, if
	/// found; otherwise, <c>-1</c>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is less than <c>0</c>.</exception>
	/// <remarks>
	/// <para>
	/// The <see cref="IAsyncEnumerable{T}"/> is searched forward starting at the first element and ending at <paramref
	/// name="index"/>, and the index of the last instance of <paramref name="item"/> is returned.
	/// </para>
	/// <para>
	/// This method determines equality using the default equality comparer <see cref="EqualityComparer{T}.Default"/>
	/// for <typeparamref name="TSource"/>, the type of values in the list.
	/// </para>
	/// <para>
	/// This operator executes immediately.
	/// </para>
	/// </remarks>
	public static ValueTask<int> LastIndexOf<TSource>(this IAsyncEnumerable<TSource> source, TSource item, Index index, CancellationToken cancellationToken = default)
	{
		return source.LastIndexOf(item, index, int.MaxValue, cancellationToken);
	}

	/// <summary>
	/// Searches for the specified object and returns the zero-based index of the last occurrence within the range of
	/// elements in the <see cref="IAsyncEnumerable{T}"/> that ends at the specified index to the last element and contains
	/// the specified number of elements.
	/// </summary>
	/// <typeparam name="TSource">
	/// The type of elements of <paramref name="source"/></typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="item">The object to locate in the <see cref="IAsyncEnumerable{T}"/>. The value can be <see
	/// langword="null"/> for reference types.</param>
	/// <param name="index">The <see cref="System.Index"/> of the ending element within the sequence.</param>
	/// <param name="count">The number of elements in the section to search.</param>
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any
	/// time.</param>
	/// <returns>
	/// The zero-based index of the last occurrence of <paramref name="item"/> within the the range of elements in the
	/// <see cref="IAsyncEnumerable{T}"/> that that ends at <paramref name="index"/> and contains <paramref name="count"/>
	/// number of elements, if found; otherwise, <c>-1</c>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than <c>0</c>.</exception>
	/// <remarks>
	/// <para>
	/// The <see cref="IAsyncEnumerable{T}"/> is searched forward starting at the first element and ending at <paramref
	/// name="index"/>, and the index of the last instance of <paramref name="item"/> no earlier in the sequence than
	/// <paramref name="count"/> items before <paramref name="index"/> is returned.
	/// </para>
	/// <para>
	/// This method determines equality using the default equality comparer <see cref="EqualityComparer{T}.Default"/>
	/// for <typeparamref name="TSource"/>, the type of values in the list.
	/// </para>
	/// <para>
	/// This operator executes immediately.
	/// </para>
	/// </remarks>
	public static ValueTask<int> LastIndexOf<TSource>(this IAsyncEnumerable<TSource> source, TSource item, Index index, int count, CancellationToken cancellationToken = default)
	{
		return FindLastIndex(source, i => EqualityComparer<TSource>.Default.Equals(i, item), index, count, cancellationToken);
	}
}
