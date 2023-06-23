namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Searches for an element that matches the conditions defined by the specified predicate and returns the
	/// zero-based index of the first occurrence within the entire <see cref="IAsyncEnumerable{T}"/>.
	/// </summary>
	/// <typeparam name="TSource">
	/// The type of elements of <paramref name="source"/></typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="predicate">The predicate that defines the conditions of the element to search for.</param>
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any
	/// time.</param>
	/// <returns>
	/// The zero-based index of the first occurrence of an element that matches the conditions defined by <paramref
	/// name="predicate"/> within the entire <see cref="IAsyncEnumerable{T}"/>, if found; otherwise, <c>-1</c>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is
	/// null.</exception>
	/// <remarks>
	/// <para>
	/// The <see cref="IAsyncEnumerable{T}"/> is searched forward starting at the first element and ending at the last
	/// element.
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
	public static ValueTask<int> FindIndex<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken = default)
	{
		return source.FindIndex(predicate, 0, int.MaxValue, cancellationToken);
	}

	/// <summary>
	/// Searches for an element that matches the conditions defined by the specified predicate and returns the
	/// zero-based index of the first occurrence within the range of elements in the <see cref="IAsyncEnumerable{T}"/>
	/// that extends from the specified index to the last element.
	/// </summary>
	/// <typeparam name="TSource">
	/// The type of elements of <paramref name="source"/></typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="predicate">The predicate that defines the conditions of the element to search for.</param>
	/// <param name="index">The <see cref="System.Index"/> of the starting element within the sequence.</param>
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any
	/// time.</param>
	/// <returns>
	/// The zero-based index of the first occurrence of an element that matches the conditions defined by <paramref
	/// name="predicate"/> within the the range of elements in the <see cref="IAsyncEnumerable{T}"/> that extends from
	/// <paramref name="index"/> to the last element, if found; otherwise, <c>-1</c>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is
	/// null.</exception>
	/// <remarks>
	/// <para>
	/// The <see cref="IAsyncEnumerable{T}"/> is searched forward starting at <paramref name="index"/> and ending at the
	/// last element.
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
	public static ValueTask<int> FindIndex<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, Index index, CancellationToken cancellationToken = default)
	{
		return source.FindIndex(predicate, index, int.MaxValue, cancellationToken);
	}

	/// <summary>
	/// Searches for an element that matches the conditions defined by the specified predicate and returns the
	/// zero-based index of the first occurrence within the range of elements in the <see cref="IAsyncEnumerable{T}"/>
	/// that starts at the specified index to the last element and contains the specified number of elements.
	/// </summary>
	/// <typeparam name="TSource">
	/// The type of elements of <paramref name="source"/></typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="predicate">The predicate that defines the conditions of the element to search for.</param>
	/// <param name="index">The <see cref="System.Index"/> of the starting element within the sequence.</param>
	/// <param name="count">The number of elements in the section to search.</param>
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any
	/// time.</param>
	/// <returns>
	/// The zero-based index of the first occurrence of an element that matches the conditions defined by <paramref
	/// name="predicate"/> within the the range of elements in the <see cref="IAsyncEnumerable{T}"/> that that starts at
	/// <paramref name="index"/> and contains <paramref name="count"/> number of elements, if found; otherwise,
	/// <c>-1</c>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is
	/// null.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than <c>0</c>.</exception>
	/// <remarks>
	/// <para>
	/// The <see cref="IAsyncEnumerable{T}"/> is searched forward starting at <paramref name="index"/> and ending at
	/// <paramref name="index"/> plus <paramref name="count"/> minus <c>1</c>, if count is greater than <c>0</c>.
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
	public static ValueTask<int> FindIndex<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, Index index, int count, CancellationToken cancellationToken = default)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(predicate);
		Guard.IsGreaterThanOrEqualTo(count, 0);

		return Core(source, predicate, index, count, cancellationToken);

		static async ValueTask<int> Core(IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, Index index, int count, CancellationToken cancellationToken)
		{
			if (!index.IsFromEnd)
			{
				var i = 0;
				var c = 0;
				await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
				{
					if (i >= index.Value)
					{
						if (predicate(element))
							return i;
						if (++c >= count)
							return -1;
					}

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
							_ = queue.Dequeue();
							i++;
						}

						queue.Enqueue(e.Current);
					}

					var c = 0;
					while (queue.Count != 0)
					{
						if (predicate(queue.Dequeue()))
							return i;
						if (++c >= count)
							return -1;
						i++;
					}
				}

				return -1;
			}
		}
	}
}
