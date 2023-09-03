namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Searches for an element that matches the conditions defined by the specified predicate and returns the
	///     zero-based index of the last occurrence within the entire <see cref="IEnumerable{T}"/>.
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
	///	    The zero-based index of the last occurrence of an element that matches the conditions defined by <paramref
	///     name="predicate"/> within the entire <see cref="IEnumerable{T}"/>, if found; otherwise, <c>-1</c>.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    The <see cref="IEnumerable{T}"/> is searched forward starting at the first element and ending at the last
	///     element, and the index of the last instance of an element that matches the conditions defined by <paramref
	///     name="predicate"/> is returned.
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
	public static int FindLastIndex<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
	{
		return source.FindLastIndex(predicate, ^1, int.MaxValue);
	}

	/// <summary>
	///	    Searches for an element that matches the conditions defined by the specified predicate and returns the
	///     zero-based index of the last occurrence within the range of elements in the <see cref="IEnumerable{T}"/>
	///     that extends backwards from the specified index to the first element.
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
	///	    The <see cref="System.Index"/> of the ending element within the sequence.
	/// </param>
	/// <returns>
	///	    The zero-based index of the last occurrence of an element that matches the conditions defined by <paramref
	///     name="predicate"/> within the the range of elements in the <see cref="IEnumerable{T}"/> that extends
	///     backwards from <paramref name="index"/> to the first element, if found; otherwise, <c>-1</c>.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    The <see cref="IEnumerable{T}"/> is searched forward starting at the first element and ending at <paramref
	///     name="index"/>, and the index of the last instance of an element that matches the conditions defined by
	///     <paramref name="predicate"/> is returned.
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
	public static int FindLastIndex<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Index index)
	{
		return source.FindLastIndex(predicate, index, int.MaxValue);
	}

	/// <summary>
	///	    Searches for an element that matches the conditions defined by the specified predicate and returns the
	///     zero-based index of the last occurrence within the range of elements in the <see cref="IEnumerable{T}"/>
	///     that ends at the specified index to the last element and contains the specified number of elements.
	/// </summary>
	/// <typeparam name="TSource">
	///	    The type of elements of <paramref name="source"/>
	/// </typeparam>
	/// <param name="source">
	///	    The source sequence.
	///	</param>
	/// <param name="predicate">
	///	    The predicate that defines the conditions of the element to search for.
	/// </param>
	/// <param name="index">
	///	    The <see cref="System.Index"/> of the ending element within the sequence.
	/// </param>
	/// <param name="count">
	///	    The number of elements in the section to search.
	/// </param>
	/// <returns>
	///	    The zero-based index of the last occurrence of an element that matches the conditions defined by <paramref
	///     name="predicate"/> within the the range of elements in the <see cref="IEnumerable{T}"/> that that ends at
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
	///	    The <see cref="IEnumerable{T}"/> is searched forward starting at the first element and ending at <paramref
	///     name="index"/>, and the index of the last instance of an element that matches the conditions defined by
	///     <paramref name="predicate"/> no earlier in the sequence than <paramref name="count"/> items before <paramref
	///     name="index"/> is returned.
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
	public static int FindLastIndex<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Index index, int count)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(predicate);
		Guard.IsGreaterThanOrEqualTo(count, 0);

		if (source.TryGetCollectionCount() is int length)
		{
			index = index.GetOffset(length);
		}

		if (!index.IsFromEnd)
		{
			var lastIndex = -1;
			var i = 0;
			foreach (var element in source)
			{
				if (i > index.Value)
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
			foreach (var element in source)
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
