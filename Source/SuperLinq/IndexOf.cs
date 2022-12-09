namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Searches for the specified object and returns the zero-based index of the first occurrence within the entire
	/// <see cref="IEnumerable{T}"/>.
	/// </summary>
	/// <typeparam name="TSource">
	/// The type of elements of <paramref name="source"/></typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="item">The object to locate in the <see cref="IEnumerable{T}"/>. The value can be <see
	/// langword="null"/> for reference types.</param>
	/// <returns>
	/// The zero-based index of the first occurrence of <paramref name="item"/> within the entire <see
	/// cref="IEnumerable{T}"/>, if found; otherwise, <c>-1</c>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
	/// <remarks>
	/// <para>
	/// The <see cref="IEnumerable{T}"/> is searched forward starting at the first element and ending at the last
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
	public static int IndexOf<TSource>(this IEnumerable<TSource> source, TSource item)
	{
		return source.IndexOf(item, 0, int.MaxValue);
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
	public static int IndexOf<TSource>(this IEnumerable<TSource> source, TSource item, Index index)
	{
		return source.IndexOf(item, index, int.MaxValue);
	}

	/// <summary>
	/// Searches for the specified object and returns the zero-based index of the first occurrence within the range of
	/// elements in the <see cref="IEnumerable{T}"/> that starts at the specified index to the last element and contains
	/// the specified number of elements.
	/// </summary>
	/// <typeparam name="TSource">
	/// The type of elements of <paramref name="source"/></typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="item">The object to locate in the <see cref="IEnumerable{T}"/>. The value can be <see
	/// langword="null"/> for reference types.</param>
	/// <param name="index">The zero-based starting index of the search. <c>0</c> (zero) is valid in an empty
	/// list.</param>
	/// <param name="count">The number of elements in the section to search.</param>
	/// <returns>
	/// The zero-based index of the first occurrence of <paramref name="item"/> within the the range of elements in the
	/// <see cref="IEnumerable{T}"/> that that starts at <paramref name="index"/> and contains <paramref name="count"/>
	/// number of elements, if found; otherwise, <c>-1</c>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than <c>0</c>.</exception>
	/// <remarks>
	/// <para>
	/// The <see cref="IEnumerable{T}"/> is searched forward starting at <paramref name="index"/> and ending at
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
	public static int IndexOf<TSource>(this IEnumerable<TSource> source, TSource item, Index index, int count)
	{
		Guard.IsNotNull(source);
		Guard.IsGreaterThanOrEqualTo(count, 0);

		if (TryGetCollectionCount(source, out var length))
		{
			index = index.GetOffset(length);
		}

		if (!index.IsFromEnd)
		{
			var i = 0;
			var c = 0;
			foreach (var element in source)
			{
				if (i >= index.Value)
				{
					if (EqualityComparer<TSource>.Default.Equals(element, item))
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
			using var e = source.GetEnumerator();

			var indexFromEnd = index.Value;
			var i = 0;
			if (e.MoveNext())
			{
				Queue<TSource> queue = new();
				queue.Enqueue(e.Current);

				while (e.MoveNext())
				{
					if (queue.Count == indexFromEnd)
					{
						queue.Dequeue();
						i++;
					}

					queue.Enqueue(e.Current);
				}

				var c = 0;
				while (queue.Count != 0)
				{
					if (EqualityComparer<TSource>.Default.Equals(queue.Dequeue(), item))
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
