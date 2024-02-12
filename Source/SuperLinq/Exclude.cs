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
	private sealed class ExcludeCollectionIterator<T>
		: CollectionIterator<T>
	{
		private readonly ICollection<T> _source;
		private readonly int _startIndex;
		private readonly int _count;

		/// <summary>
		/// Creates a new collection iterator for excluding elements from the specified <paramref name="source"/> collection.
		/// </summary>
		/// <param name="source">The source collection to exclude elements from.</param>
		/// <param name="startIndex">The zero-based index at which to begin excluding elements.</param>
		/// <param name="count">The number of elements to exclude.</param>
		internal ExcludeCollectionIterator(ICollection<T> source, int startIndex, int count)
		{
			_source = source;
			_startIndex = startIndex;
			_count = count;
		}

		/// <summary>
		/// Creates a new collection iterator for excluding elements from the specified <paramref name="source"/> collection.
		/// </summary>
		/// <param name="source">The source collection to exclude elements from.</param>
		/// <param name="range">The range of elements to exclude.</param>
		internal ExcludeCollectionIterator(ICollection<T> source, Range range)
		{
			_source = source;
			_startIndex = range.Start.Value;
			_count = range.End.Value - _startIndex;
		}

		/// <summary>
		/// Gets the number of elements in the source collection after excluding the specified portion of elements.
		/// </summary>
		public override int Count =>
			_source.Count < _startIndex ? _source.Count :
			_source.Count < _startIndex + _count ? _startIndex :
			_source.Count - _count;

		/// <inheritdoc cref="IEnumerable{T}" /> 
		protected override IEnumerable<T> GetEnumerable() =>
			ExcludeCore(_source, _startIndex, _count);
	}

	private sealed class ExcludeListIterator<T>
		: ListIterator<T>
	{
		private readonly IList<T> _source;
		private readonly int _startIndex;
		private readonly int _count;

		/// <summary>
		/// Creates a new list iterator for excluding elements from the specified <paramref name="source"/> collection.
		/// </summary>
		/// <param name="source">The source collection to exclude elements from.</param>
		/// <param name="startIndex">The zero-based index at which to begin excluding elements.</param>
		/// <param name="count">The number of elements to exclude.</param>
		internal ExcludeListIterator(IList<T> source, int startIndex, int count)
		{
			_source = source;
			_startIndex = startIndex;
			_count = count;
		}

		/// <summary>
		/// Creates a new list iterator for excluding elements from the specified <paramref name="source"/> collection.
		/// </summary>
		/// <param name="source">The source collection to exclude elements from.</param>
		/// <param name="range">The range of elements to exclude.</param>
		internal ExcludeListIterator(IList<T> source, Range range)
		{
			_source = source;
			_startIndex = range.Start.Value;
			_count = range.End.Value - _startIndex;
		}

		/// <summary>
		/// Gets the number of elements in the source collection after excluding the specified portion of elements.
		/// </summary>
		public override int Count =>
			_source.Count < _startIndex ? _source.Count :
			_source.Count < _startIndex + _count ? _startIndex :
			_source.Count - _count;

		/// <inheritdoc cref="IEnumerable{T}" />
		protected override IEnumerable<T> GetEnumerable()
		{
			var cnt = (uint)_source.Count;
			for (var i = 0; i < cnt && i < _startIndex; i++)
				yield return _source[i];

			for (var i = _startIndex + _count; i < cnt; i++)
				yield return _source[i];
		}

		/// <inheritdoc cref="IEnumerable{T}"/>
		protected override T ElementAt(int index)
		{
			ArgumentOutOfRangeException.ThrowIfNegative(index);
			ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, Count);

			return index < _startIndex
				? _source[index]
				: _source[index + _count];
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

		start = range.Start.GetOffset(count);
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
				if (count < start)
					yield return el;
			}
		}

		end = range.End.GetOffset(count);
		var length = end - start;

		while (length > 0)
		{
			if (!queue.TryDequeue(out var _))
				yield break;
			length--;
		}

		while (queue.TryDequeue(out var element))
			yield return element;
	}
}
