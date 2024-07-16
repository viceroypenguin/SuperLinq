namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Searches for an element that matches the conditions defined by the specified predicate and returns the
	///     zero-based index of the first occurrence within the entire <see cref="IEnumerable{T}"/>.
	/// </summary>
	/// <typeparam name="TSource">
	///	    The type of elements of <paramref name="source"/>
	/// </typeparam>
	/// <param name="source">
	///	    The source sequence.
	/// </param>
	/// <param name="predicate">
	///	    The predicate that defines the conditions of the element to search for.
	/// </param>
	/// <returns>
	///	    The zero-based index of the first occurrence of an element that matches the conditions defined by <paramref
	///     name="predicate"/> within the entire <see cref="IEnumerable{T}"/>, if found; otherwise, <c>-1</c>.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    The <see cref="IEnumerable{T}"/> is searched forward starting at the first element and ending at the last
	///     element.
	/// </para>
	/// <para>
	///	    The <paramref name="predicate"/> is a delegate to a method that returns <see langword="true"/> if the object
	///     passed to it matches the conditions defined in the delegate. The elements of the current <see
	///     cref="IEnumerable{T}"/> are individually passed to the <paramref name="predicate"/> delegate.
	/// </para>
	/// <para>
	///	    This operator executes immediately.
	/// </para>
	/// </remarks>
	public static int FindIndex<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
	{
		return source.FindIndex(predicate, 0, int.MaxValue);
	}

	/// <summary>
	///	    Searches for an element that matches the conditions defined by the specified predicate and returns the
	///     zero-based index of the first occurrence within the range of elements in the <see cref="IEnumerable{T}"/>
	///     that extends from the specified index to the last element.
	/// </summary>
	/// <typeparam name="TSource">
	///	    The type of elements of <paramref name="source"/>
	/// </typeparam>
	/// <param name="source">
	///	    The source sequence.
	/// </param>
	/// <param name="predicate">
	///	    The predicate that defines the conditions of the element to search for.
	/// </param>
	/// <param name="index">
	///	    The <see cref="System.Index"/> of the starting element within the sequence.
	/// </param>
	/// <returns>
	///	    The zero-based index of the first occurrence of an element that matches the conditions defined by <paramref
	///     name="predicate"/> within the the range of elements in the <see cref="IEnumerable{T}"/> that extends from
	///     <paramref name="index"/> to the last element, if found; otherwise, <c>-1</c>.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    The <see cref="IEnumerable{T}"/> is searched forward starting at <paramref name="index"/> and ending at the
	///     last element.
	/// </para>
	/// <para>
	///	    The <paramref name="predicate"/> is a delegate to a method that returns <see langword="true"/> if the object
	///     passed to it matches the conditions defined in the delegate. The elements of the current <see
	///     cref="IEnumerable{T}"/> are individually passed to the <paramref name="predicate"/> delegate.
	/// </para>
	/// <para>
	///	    This operator executes immediately.
	/// </para>
	/// </remarks>
#if NETCOREAPP
	public static int FindIndex<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Index index)
#else
	internal static int FindIndex<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Index index)
#endif
	{
		return source.FindIndex(predicate, index, int.MaxValue);
	}

	/// <summary>
	///	    Searches for an element that matches the conditions defined by the specified predicate and returns the
	///     zero-based index of the first occurrence within the range of elements in the <see cref="IEnumerable{T}"/>
	///     that starts at the specified index to the last element and contains the specified number of elements.
	/// </summary>
	/// <typeparam name="TSource">
	///	    The type of elements of <paramref name="source"/>
	/// </typeparam>
	/// <param name="source">
	///	    The source sequence.</param>
	/// <param name="predicate">
	///	    The predicate that defines the conditions of the element to search for.
	/// </param>
	/// <param name="index">
	///	    The <see cref="System.Index"/> of the starting element within the sequence.
	/// </param>
	/// <param name="count">
	///	    The number of elements in the section to search.
	/// </param>
	/// <returns>
	///	    The zero-based index of the first occurrence of an element that matches the conditions defined by <paramref
	///     name="predicate"/> within the the range of elements in the <see cref="IEnumerable{T}"/> that that starts at
	///     <paramref name="index"/> and contains <paramref name="count"/> number of elements, if found; otherwise,
	///     <c>-1</c>.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.
	/// </exception>
	/// <exception cref="ArgumentOutOfRangeException">
	///	    <paramref name="count"/> is less than <c>0</c>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    The <see cref="IEnumerable{T}"/> is searched forward starting at <paramref name="index"/> and ending at
	///     <paramref name="index"/> plus <paramref name="count"/> minus <c>1</c>, if count is greater than <c>0</c>.
	/// </para>
	/// <para>
	///	    The <paramref name="predicate"/> is a delegate to a method that returns <see langword="true"/> if the object
	///     passed to it matches the conditions defined in the delegate. The elements of the current <see
	///     cref="IEnumerable{T}"/> are individually passed to the <paramref name="predicate"/> delegate.
	/// </para>
	/// <para>
	///	    This operator executes immediately.
	/// </para>
	/// </remarks>
#if NETCOREAPP
	public static int FindIndex<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Index index, int count)
#else
	internal static int FindIndex<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Index index, int count)
#endif
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(predicate);
		ArgumentOutOfRangeException.ThrowIfNegative(count);

		if (source.TryGetCollectionCount() is int length)
			index = index.GetOffset(length);

		if (!index.IsFromEnd)
		{
			var i = 0;
			var c = 0;
			foreach (var element in source)
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
