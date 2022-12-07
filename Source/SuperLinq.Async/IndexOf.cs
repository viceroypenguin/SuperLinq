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
	public static ValueTask<int> IndexOf<TSource>(this IAsyncEnumerable<TSource> source, TSource item)
	{
		return source.IndexOf(item, 0);
	}

	/// <summary>
	/// Searches for the specified object and returns the zero-based index of the first occurrence within the range of
	/// elements in the <see cref="IEnumerable{T}"/> that extends from the specified index to the last element.
	/// </summary>
	/// <typeparam name="TSource">
	/// The type of elements of <paramref name="source"/></typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="item">The object to locate in the <see cref="IEnumerable{T}"/>. The value can be <see
	/// langword="null"/> for reference types.</param>
	/// <param name="index">The zero-based starting index of the search. <c>0</c> (zero) is valid in an empty
	/// list.</param>
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any
	/// time.</param>
	/// <returns>
	/// The zero-based index of the first occurrence of <paramref name="item"/> within the the range of elements in the
	/// <see cref="IEnumerable{T}"/> that extends from <paramref name="index"/> to the last element, if found;
	/// otherwise, <c>-1</c>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is less than <c>0</c>.</exception>
	/// <remarks>
	/// <para>
	/// The <see cref="IEnumerable{T}"/> is searched forward starting at <paramref name="index"/> and ending at the last
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
	public static async ValueTask<int> IndexOf<TSource>(this IAsyncEnumerable<TSource> source, TSource item, Index index, CancellationToken cancellationToken = default)
	{
		Guard.IsNotNull(source);

		if (!index.IsFromEnd)
		{
			var i = 0;
			await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
			{
				if (i >= index.Value && EqualityComparer<TSource>.Default.Equals(element, item))
					return i;

				i++;
			}

			return -1;
		}
		else
		{
			await using var e = source.GetConfiguredAsyncEnumerator(cancellationToken);

			var indexFromEnd = index.Value;
			var i = 0;
			if (await e.MoveNextAsync())
			{
				Queue<TSource> queue = new();
				queue.Enqueue(e.Current);

				while (await e.MoveNextAsync())
				{
					if (queue.Count == indexFromEnd)
					{
						queue.Dequeue();
						i++;
					}

					queue.Enqueue(e.Current);
				}

				while (queue.Count != 0)
				{
					if (EqualityComparer<TSource>.Default.Equals(queue.Dequeue(), item))
						return i;
					i++;
				}
			}

			return -1;
		}
	}
}
