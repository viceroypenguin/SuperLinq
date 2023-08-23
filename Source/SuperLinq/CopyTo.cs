namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Copies the contents of a sequence into a provided span.
	/// </summary>
	/// <typeparam name="TSource">
	///	    The type of elements of <paramref name="source"/>
	/// </typeparam>
	/// <param name="source">
	///	    The source sequence.
	/// </param>
	/// <param name="span">
	///	    The span that is the destination of the elements copied from <paramref name="source"/>.
	/// </param>
	/// <returns>
	///	    The number of elements actually copied.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> is <see langword="null"/>.
	/// </exception>
	/// <exception cref="ArgumentException">
	///	    <paramref name="span"/> is not long enough to hold the data from sequence.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    All data from <paramref name="source"/> will be copied to <paramref name="span"/> if possible.
	///	</para>
	///	<para>
	///	    If <paramref name="source"/> is shorter than <paramref name="span"/>, then any remaining elements will be
	///     untouched. If <paramref name="source"/> is longer than <paramref name="span"/>, then an exception will be
	///     thrown.
	/// </para>
	/// <para>
	///	    This operator executes immediately.
	/// </para>
	/// </remarks>
	public static int CopyTo<TSource>(this IEnumerable<TSource> source, Span<TSource> span)
	{
		Guard.IsNotNull(source);

		if (source is TSource[] arr)
		{
			arr.AsSpan().CopyTo(span);
			return arr.Length;
		}
		else if (source.TryGetCollectionCount() is int n)
		{
			if (n > span.Length)
				ThrowHelper.ThrowArgumentException(nameof(span), "Destination is not long enough.");

			var i = 0;
			foreach (var el in source)
				span[i++] = el;

			return i;
		}
		else
		{
			var i = 0;
			foreach (var el in source)
			{
				if (i >= span.Length)
					ThrowHelper.ThrowArgumentException(nameof(span), "Destination is not long enough.");

				span[i++] = el;
			}

			return i;
		}
	}

	/// <summary>
	///	    Copies the contents of a sequence into a provided span.
	/// </summary>
	/// <typeparam name="TSource">
	///	    The type of elements of <paramref name="source"/>
	/// </typeparam>
	/// <param name="source">
	///	    The source sequence.
	/// </param>
	/// <param name="array">
	///	    The span that is the destination of the elements copied from <paramref name="source"/>.
	/// </param>
	/// <returns>
	///	    The number of elements actually copied.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="array"/> is <see langword="null"/>.
	/// </exception>
	/// <exception cref="ArgumentException">
	///	    <paramref name="array"/> is not long enough to hold the data from sequence.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    All data from <paramref name="source"/> will be copied to <paramref name="array"/> if possible.
	/// </para>
	/// <para>
	///	    If <paramref name="source"/> is shorter than <paramref name="array"/>, then any remaining elements will be
	///     untouched. If <paramref name="source"/> is longer than <paramref name="array"/>, then an exception will be
	///     thrown.
	/// </para>
	/// <para>
	///	    This operator executes immediately.
	/// </para>
	/// </remarks>
	public static int CopyTo<TSource>(this IEnumerable<TSource> source, TSource[] array)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(array);

		return CopyTo(source, array, 0);
	}

	private static int CopyTo<TSource>(IEnumerable<TSource> source, TSource[] array, int index)
	{
		if (source is TSource[] arr)
		{
			arr.CopyTo(array, index);
			return arr.Length;
		}
		else if (source is ICollection<TSource> coll)
		{
			coll.CopyTo(array, index);
			return coll.Count;
		}
		else if (source.TryGetCollectionCount() is int n)
		{
			if (n + index > array.Length)
				ThrowHelper.ThrowArgumentException(nameof(array), "Destination is not long enough.");

			var i = index;
			foreach (var el in source)
				array[i++] = el;

			return i - index;
		}
		else
		{
			var i = index;
			foreach (var el in source)
			{
				if (i >= array.Length)
					ThrowHelper.ThrowArgumentException(nameof(array), "Destination is not long enough.");

				array[i++] = el;
			}

			return i - index;
		}
	}

	/// <summary>
	///	    Copies the contents of a sequence into a provided list.
	/// </summary>
	/// <typeparam name="TSource">
	///	    The type of elements of <paramref name="source"/></typeparam>
	/// <param name="source">
	///	    The source sequence.
	/// </param>
	/// <param name="list">
	///	    The list that is the destination of the elements copied from <paramref name="source"/>.
	/// </param>
	/// <returns>
	///	    The number of elements actually copied.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="list"/> is <see langword="null"/>.
	/// </exception>
	/// <exception cref="NotSupportedException">
	///	    The <paramref name="list"/> is readonly, or does not allow increasing the size via <see
	///     cref="ICollection{T}.Add(T)"/>
	/// </exception>
	/// <remarks>
	/// <para>
	///	    All data from <paramref name="source"/> will be copied to <paramref name="list"/>, starting at position 0.
	/// </para>
	/// <para>
	///	    If <paramref name="source"/> is shorter than <paramref name="list"/>, then any remaining elements will be
	///     untouched. If <paramref name="source"/> is longer than <paramref name="list"/>, then an exception may be
	///     thrown if the <paramref name="list"/> has a fixed size (an <see cref="Array"/>, for example).
	/// </para>
	/// <para>
	///	    This operator executes immediately.
	/// </para>
	/// </remarks>
	public static int CopyTo<TSource>(this IEnumerable<TSource> source, IList<TSource> list)
	{
		return source.CopyTo(list, 0);
	}

	/// <summary>
	///	    Copies the contents of a sequence into a provided list.
	/// </summary>
	/// <typeparam name="TSource">
	///	    The type of elements of <paramref name="source"/></typeparam>
	/// <param name="source">
	///	    The source sequence.
	/// </param>
	/// <param name="list">
	///	    The list that is the destination of the elements copied from <paramref name="source"/>.
	/// </param>
	/// <param name="index">
	///	    The position in <paramref name="list"/> at which to start copying data.
	///	</param>
	/// <returns>
	///	    The number of elements actually copied.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="list"/> is <see langword="null"/>.
	/// </exception>
	/// <exception cref="ArgumentOutOfRangeException">
	///	    <paramref name="index"/> is less than <c>0</c>.
	/// </exception>
	/// <exception cref="NotSupportedException">
	///	    The <paramref name="list"/> is readonly, or does not allow increasing the size via <see
	///     cref="ICollection{T}.Add(T)"/>
	/// </exception>
	/// <remarks>
	/// <para>
	///	    All data from <paramref name="source"/> will be copied to <paramref name="list"/>, starting at position
	///     <paramref name="index"/>.
	/// </para>
	/// <para>
	///	    If <paramref name="source"/> is shorter than <paramref name="list"/>, then any remaining elements will be
	///     untouched. If <paramref name="source"/> is longer than <paramref name="list"/>, then an exception may be
	///     thrown if the <paramref name="list"/> has a fixed size (an <see cref="Array"/>, for example).
	/// </para>
	/// <para>
	///	    This operator executes immediately.
	/// </para>
	/// </remarks>
	public static int CopyTo<TSource>(this IEnumerable<TSource> source, IList<TSource> list, int index)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(list);
		Guard.IsGreaterThanOrEqualTo(index, 0);

		if (list is TSource[] array)
		{
			return CopyTo(source, array, index);
		}
		else
		{
#if NET6_0_OR_GREATER
			if (list is List<TSource> l
				&& source.TryGetCollectionCount() is int n)
			{
				l.EnsureCapacity(n + index);
			}
#endif

			var i = index;
			foreach (var el in source)
			{
				if (i < list.Count)
					list[i] = el;
				else
					list.Add(el);
				i++;
			}

			return i - index;
		}
	}
}
