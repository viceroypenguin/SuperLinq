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

	private sealed class ExcludeCollectionIterator<T>(
		ICollection<T> source,
		int startIndex,
		int count
	) : CollectionIterator<T>
	{
		public override int Count =>
			source.Count < startIndex ? source.Count :
			source.Count < startIndex + count ? startIndex :
			source.Count - count;

		protected override IEnumerable<T> GetEnumerable() =>
			ExcludeCore(source, startIndex, count);
	}

	private sealed class ExcludeListIterator<T>(
		IList<T> source,
		int startIndex,
		int count
	) : ListIterator<T>
	{
		public override int Count =>
			source.Count < startIndex ? source.Count :
			source.Count < startIndex + count ? startIndex :
			source.Count - count;

		protected override IEnumerable<T> GetEnumerable()
		{
			var cnt = (uint)source.Count;
			for (var i = 0; i < cnt && i < startIndex; i++)
				yield return source[i];

			for (var i = startIndex + count; i < cnt; i++)
				yield return source[i];
		}

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
}
