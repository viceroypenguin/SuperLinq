namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Searches for an element that matches the conditions defined by the specified predicate and returns the
	/// zero-based index of the last occurrence within the entire <see cref="IAsyncEnumerable{T}"/>.
	/// </summary>
	/// <typeparam name="TSource">
	/// The type of elements of <paramref name="source"/></typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="predicate">The predicate that defines the conditions of the element to search for.</param>
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any
	/// time.</param>
	/// <returns>
	/// The zero-based index of the last occurrence of an element that matches the conditions defined by <paramref
	/// name="predicate"/> within the entire <see cref="IAsyncEnumerable{T}"/>, if found; otherwise, <c>-1</c>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is
	/// null.</exception>
	/// <remarks>
	/// <para>
	/// The <see cref="IAsyncEnumerable{T}"/> is searched forward starting at the first element and ending at the last
	/// element, and the index of the last instance of an element that matches the conditions defined by <paramref
	/// name="predicate"/> is returned.
	/// </para>
	/// <para>
	/// The <paramref name="predicate"/> is a delegate to a method that returns <see langword="true"/> if the object
	/// passed to it matches the conditions defined in the delegate. The elements of the current <see
	/// cref="IAsyncEnumerable{T}"/> are individually passed to the <paramref name="predicate"/> delegate. 
	/// </para>
	/// <para>
	/// This operator executes immediately.
	/// </para>
	/// </remarks>
	public static ValueTask<int> FindLastIndex<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken = default)
	{
		return source.FindLastIndex(predicate, ^1, int.MaxValue, cancellationToken);
	}

	/// <summary>
	/// Searches for an element that matches the conditions defined by the specified predicate and returns the
	/// zero-based index of the last occurrence within the range of elements in the <see cref="IAsyncEnumerable{T}"/>
	/// that extends backwards from the specified index to the first element.
	/// </summary>
	/// <typeparam name="TSource">
	/// The type of elements of <paramref name="source"/></typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="predicate">The predicate that defines the conditions of the element to search for.</param>
	/// <param name="index">The <see cref="System.Index"/> of the ending element within the sequence.</param>
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any
	/// time.</param>
	/// <returns>
	/// The zero-based index of the last occurrence of an element that matches the conditions defined by <paramref
	/// name="predicate"/> within the the range of elements in the <see cref="IAsyncEnumerable{T}"/> that extends
	/// backwards from <paramref name="index"/> to the first element, if found; otherwise, <c>-1</c>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is
	/// null.</exception>
	/// <remarks>
	/// <para>
	/// The <see cref="IAsyncEnumerable{T}"/> is searched forward starting at the first element and ending at <paramref
	/// name="index"/>, and the index of the last instance of an element that matches the conditions defined by
	/// <paramref name="predicate"/> is returned.
	/// </para>
	/// <para>
	/// The <paramref name="predicate"/> is a delegate to a method that returns <see langword="true"/> if the object
	/// passed to it matches the conditions defined in the delegate. The elements of the current <see
	/// cref="IAsyncEnumerable{T}"/> are individually passed to the <paramref name="predicate"/> delegate. 
	/// </para>
	/// <para>
	/// This operator executes immediately.
	/// </para>
	/// </remarks>
	public static ValueTask<int> FindLastIndex<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, Index index, CancellationToken cancellationToken = default)
	{
		return source.FindLastIndex(predicate, index, int.MaxValue, cancellationToken);
	}

	/// <summary>
	/// Searches for an element that matches the conditions defined by the specified predicate and returns the
	/// zero-based index of the last occurrence within the range of elements in the <see cref="IAsyncEnumerable{T}"/>
	/// that ends at the specified index to the last element and contains the specified number of elements.
	/// </summary>
	/// <typeparam name="TSource">
	/// The type of elements of <paramref name="source"/></typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="predicate">The predicate that defines the conditions of the element to search for.</param>
	/// <param name="index">The <see cref="System.Index"/> of the ending element within the sequence.</param>
	/// <param name="count">The number of elements in the section to search.</param>
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any
	/// time.</param>
	/// <returns>
	/// The zero-based index of the last occurrence of an element that matches the conditions defined by <paramref
	/// name="predicate"/> within the the range of elements in the <see cref="IAsyncEnumerable{T}"/> that that ends at
	/// <paramref name="index"/> and contains <paramref name="count"/> number of elements, if found; otherwise,
	/// <c>-1</c>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is
	/// null.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than <c>0</c>.</exception>
	/// <remarks>
	/// <para>
	/// The <see cref="IAsyncEnumerable{T}"/> is searched forward starting at the first element and ending at <paramref
	/// name="index"/>, and the index of the last instance of an element that matches the conditions defined by
	/// <paramref name="predicate"/> no earlier in the sequence than <paramref name="count"/> items before <paramref
	/// name="index"/> is returned.
	/// </para>
	/// <para>
	/// The <paramref name="predicate"/> is a delegate to a method that returns <see langword="true"/> if the object
	/// passed to it matches the conditions defined in the delegate. The elements of the current <see
	/// cref="IAsyncEnumerable{T}"/> are individually passed to the <paramref name="predicate"/> delegate. 
	/// </para>
	/// <para>
	/// This operator executes immediately.
	/// </para>
	/// </remarks>
	public static ValueTask<int> FindLastIndex<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, Index index, int count, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(predicate);
		ArgumentOutOfRangeException.ThrowIfNegative(count);

		return Core(source, predicate, index, count, cancellationToken);

		static async ValueTask<int> Core(IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, Index index, int count, CancellationToken cancellationToken)
		{
			if (!index.IsFromEnd)
			{
				var lastIndex = -1;
				var i = 0;
				await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
				{
					if (i >= index.Value)
						break;

					if (predicate(element))
						lastIndex = i;
					i++;
				}

				return i - lastIndex > count ? -1 : lastIndex;
			}
			else
			{
				var indexFromEnd = index.Value - 1;
				var lastIndex = -1;
				var i = 0;

				Queue<TSource> queue = new(indexFromEnd + 1);
				await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
				{
					queue.Enqueue(element);
					if (queue.Count > indexFromEnd)
					{
						if (predicate(queue.Dequeue()))
							lastIndex = i;

						i++;
					}
				}

				return i - lastIndex > count ? -1 : lastIndex;
			}
		}
	}
}
