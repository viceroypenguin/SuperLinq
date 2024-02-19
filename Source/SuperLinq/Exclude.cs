namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Excludes a contiguous number of elements from a sequence starting at a given index.
	/// </summary>
	/// <typeparam name="T">
	///	    The type of the elements of the sequence
	/// </typeparam>
	/// <param name="sequence">
	///	    The sequence to exclude elements from
	/// </param>
	/// <param name="startIndex">
	///	    The zero-based index at which to begin excluding elements
	/// </param>
	/// <param name="count">
	///	    The number of elements to exclude
	/// </param>
	/// <returns>
	///	    A sequence that excludes the specified portion of elements
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="sequence"/> is <see langword="null" />.
	/// </exception>
	/// <exception cref="ArgumentOutOfRangeException">
	///	    <paramref name="startIndex"/> or <paramref name="count"/> is less than <c>0</c>.
	/// </exception>
	/// <remarks>
	///	    This method uses deferred execution and streams its results.
	/// </remarks>
	public static IEnumerable<T> Exclude<T>(this IEnumerable<T> sequence, int startIndex, int count)
	{
		ArgumentNullException.ThrowIfNull(sequence);
		ArgumentOutOfRangeException.ThrowIfNegative(startIndex);
		ArgumentOutOfRangeException.ThrowIfNegative(count);

		if (count == 0)
			return sequence;

		return sequence switch
		{
			IList<T> list => new ExcludeListIterator<T>(list, startIndex, count),
			ICollection<T> collection => new ExcludeCollectionIterator<T>(collection, startIndex, count),
			_ => ExcludeCore(sequence, startIndex, count)
		};
	}

	/// <summary>
	///	    Excludes a contiguous number of elements from a sequence starting at a given index.
	/// </summary>
	/// <typeparam name="T">
	///	    The type of the elements of the sequence
	/// </typeparam>
	/// <param name="sequence">
	///	    The sequence to exclude elements from
	/// </param>
	/// <param name="range">
	///	    The zero-based index at which to begin excluding elements
	/// </param>
	/// <returns>
	///	    A sequence that excludes the specified portion of elements
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="sequence"/> is <see langword="null" />.
	/// </exception>
	/// <exception cref="ArgumentOutOfRangeException">
	///	    <paramref name="range.Start.Value"/> or <paramref name="range.End.Value"/> is less than <c>0</c>.
	/// </exception>
	/// <remarks>
	///	    This method uses deferred execution and streams its results.
	/// </remarks>
	public static IEnumerable<T> Exclude<T>(this IEnumerable<T> sequence, Range range)
	{
		ArgumentNullException.ThrowIfNull(sequence);

		var startFromEnd = range.Start.IsFromEnd;
		var endFromEnd = range.End.IsFromEnd;
		if ((startFromEnd, endFromEnd) == (false, false))
		{
			return Exclude(sequence, range.Start.Value, range.End.Value - range.Start.Value);
		}

		if (sequence.TryGetCollectionCount() is int count)
		{
			var (start, length) = range.GetOffsetAndLength(count);
			return Exclude(sequence, start, length);
		}

		return (startFromEnd, endFromEnd) switch
		{
			(false, true) => ExcludeEndFromEnd(sequence, range),
			(true, false) => ExcludeStartFromEnd(sequence, range),
			(true, true) when range.Start.Value < range.End.Value =>
				ThrowHelper.ThrowArgumentOutOfRangeException<IEnumerable<T>>("length"),
			_ => ExcludeRange(sequence, range),
		};
	}

	/// <summary>
	/// Represents an iterator for excluding elements from a collection.
	/// </summary>
	/// <typeparam name="T">Specifies the type of elements in the collection.</typeparam>
	private sealed class ExcludeCollectionIterator<T>(ICollection<T> source, int startIndex, int count)
		: CollectionIterator<T>
	{
		/// <summary>
		/// Gets the number of elements in the source collection after excluding the specified portion of elements.
		/// </summary>
		public override int Count =>
			source.Count < startIndex ? source.Count :
			source.Count < startIndex + count ? startIndex :
			source.Count - count;

		/// <inheritdoc cref="IEnumerable{T}" /> 
		protected override IEnumerable<T> GetEnumerable() =>
			ExcludeCore(source, startIndex, count);
	}

	private sealed class ExcludeListIterator<T>(IList<T> source, int startIndex, int count)
		: ListIterator<T>
	{
		/// <summary>
		/// Gets the number of elements in the source collection after excluding the specified portion of elements.
		/// </summary>
		public override int Count =>
			source.Count < startIndex ? source.Count :
			source.Count < startIndex + count ? startIndex :
			source.Count - count;

		/// <inheritdoc cref="IEnumerable{T}" />
		protected override IEnumerable<T> GetEnumerable()
		{
			var cnt = (uint)source.Count;
			for (var i = 0; i < cnt && i < startIndex; i++)
				yield return source[i];

			for (var i = startIndex + count; i < cnt; i++)
				yield return source[i];
		}

		/// <inheritdoc cref="IEnumerable{T}"/>
		protected override T ElementAt(int index)
		{
			ArgumentOutOfRangeException.ThrowIfNegative(index);
			ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, Count);

			return index < startIndex
				? source[index]
				: source[index + count];
		}
	}

	private static IEnumerable<T> ExcludeCore<T>(IEnumerable<T> sequence, int startIndex, int count)
	{
		var index = 0;
		var endIndex = startIndex + count;

		foreach (var item in sequence)
		{
			if (index < startIndex || index >= endIndex)
				yield return item;

			index++;
		}
	}

	private static IEnumerable<T> ExcludeRange<T>(IEnumerable<T> sequence, Range range)
	{
		var start = range.Start.Value;
		var queue = new Queue<T>(start + 1);
		foreach (var e in sequence)
		{
			queue.Enqueue(e);
			if (queue.Count > start)
				yield return queue.Dequeue();
		}

		start = Math.Min(start, queue.Count);
		var length = start - range.End.Value;
		while (length > 0)
		{
			if (!queue.TryDequeue(out var _))
				yield break;
			length--;
		}

		while (queue.TryDequeue(out var element))
			yield return element;
	}

	private static IEnumerable<T> ExcludeStartFromEnd<T>(IEnumerable<T> sequence, Range range)
	{
		var count = 0;
		var start = range.Start.Value;
		var queue = new Queue<T>(start + 1);
		foreach (var e in sequence)
		{
			count++;
			queue.Enqueue(e);
			if (queue.Count > start)
				yield return queue.Dequeue();
		}

		start = Math.Max(range.Start.GetOffset(count), 0);
		var length = range.End.Value - start;

		while (length > 0)
		{
			if (!queue.TryDequeue(out var _))
				yield break;
			length--;
		}

		while (queue.TryDequeue(out var element))
			yield return element;
	}

	private static IEnumerable<T> ExcludeEndFromEnd<T>(IEnumerable<T> sequence, Range range)
	{
		var count = 0;
		var start = range.Start.Value;
		var end = range.End.Value;
		var queue = new Queue<T>(end + 1);
		foreach (var e in sequence)
		{
			count++;
			queue.Enqueue(e);
			if (queue.Count > end)
			{
				var el = queue.Dequeue();
				if ((count - end) <= start)
					yield return el;
			}
		}

		while (queue.TryDequeue(out var element))
			yield return element;
	}
}
