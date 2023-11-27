namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Returns a sequence with a range of elements in the source sequence moved to a new offset.
	/// </summary>
	/// <typeparam name="T">
	///	    Type of the source sequence.
	/// </typeparam>
	/// <param name="source">
	///	    The source sequence.
	/// </param>
	/// <param name="fromIndex">
	///	    The zero-based index identifying the first element in the range of elements to move.
	/// </param>
	/// <param name="count">
	///	    The count of items to move.
	/// </param>
	/// <param name="toIndex">
	///	    The index where the specified range will be moved.
	/// </param>
	/// <returns>
	///	    A sequence with the specified range moved to the new position.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> is <see langword="null"/>.
	/// </exception>
	/// <exception cref="ArgumentOutOfRangeException">
	///	    <paramref name="fromIndex"/>, <paramref name="count"/>, or <paramref name="toIndex"/> is less than <c>0</c>.
	/// </exception>
	/// <remarks>
	///	    This operator uses deferred execution and streams its results.
	/// </remarks>
	public static IEnumerable<T> Move<T>(this IEnumerable<T> source, int fromIndex, int count, int toIndex)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentOutOfRangeException.ThrowIfNegative(fromIndex);
		ArgumentOutOfRangeException.ThrowIfNegative(count);
		ArgumentOutOfRangeException.ThrowIfNegative(toIndex);

		return
			toIndex == fromIndex || count == 0
				? source :
			toIndex < fromIndex
				 ? Core(source, toIndex, fromIndex - toIndex, count)
				 : Core(source, fromIndex, count, toIndex - fromIndex);

		static IEnumerable<T> Core(IEnumerable<T> source, int bufferStartIndex, int bufferSize, int bufferYieldIndex)
		{
			var hasMore = true;
			bool MoveNext(IEnumerator<T> e) => hasMore && (hasMore = e.MoveNext());

			using var e = source.GetEnumerator();

			for (var i = 0; i < bufferStartIndex && MoveNext(e); i++)
				yield return e.Current;

			var buffer = new T[bufferSize];
			var length = 0;

			for (; length < bufferSize && MoveNext(e); length++)
				buffer[length] = e.Current;

			for (var i = 0; i < bufferYieldIndex && MoveNext(e); i++)
				yield return e.Current;

			for (var i = 0; i < length; i++)
				yield return buffer[i];

			while (MoveNext(e))
				yield return e.Current;
		}
	}

	/// <summary>
	/// 	Returns a sequence with a range of elements in the source sequence moved to a new offset.
	/// </summary>
	/// <typeparam name="T">
	/// 	Type of the source sequence.
	/// </typeparam>
	/// <param name="source">
	/// 	The source sequence.
	/// </param>
	/// <param name="range">
	/// 	The range of values to move.
	/// </param>
	/// <param name="toIndex">
	/// 	The index where the specified range will be moved.</param>
	/// <returns>
	/// 	A sequence with the specified range moved to the new position.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> is <see langword="null"/>.
	/// </exception>
	/// <exception cref="ArgumentOutOfRangeException">
	///	    <paramref name="range"/>'s start is less than <c>0</c> or <paramref name="range"/>'s end is before start in the sequence.
	/// </exception>
	/// <remarks>
	/// 	This operator uses deferred executing and streams its results.
	/// </remarks>
	public static IEnumerable<T> Move<T>(this IEnumerable<T> source, Range range, Index toIndex)
	{
		int? length = 0;
		if (range.Start.IsFromEnd || range.End.IsFromEnd || toIndex.IsFromEnd)
		{
			length = source.TryGetCollectionCount();
			if (!length.HasValue)
			{
				length = source.GetCollectionCount();
			}
		}
		var fromIndex = range.Start.IsFromEnd ? range.Start.GetOffset(length.Value) : range.Start.Value;
		var count = (range.End.IsFromEnd ? range.End.GetOffset(length.Value) : range.End.Value) - fromIndex;
		var to = toIndex.IsFromEnd ? toIndex.GetOffset(length.Value) : toIndex.Value;
		return source.Move
		(
			fromIndex,
			count,
			to
		);
	}
}