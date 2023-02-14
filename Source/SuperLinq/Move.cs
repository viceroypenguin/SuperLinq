namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Returns a sequence with a range of elements in the source sequence
	/// moved to a new offset.
	/// </summary>
	/// <typeparam name="T">Type of the source sequence.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="fromIndex">
	/// The zero-based index identifying the first element in the range of
	/// elements to move.</param>
	/// <param name="count">The count of items to move.</param>
	/// <param name="toIndex">
	/// The index where the specified range will be moved.</param>
	/// <returns>
	/// A sequence with the specified range moved to the new position.
	/// </returns>
	/// <remarks>
	/// This operator uses deferred execution and streams its results.
	/// </remarks>
	/// <example>
	/// <code><![CDATA[
	/// var result = Enumerable.Range(0, 6).Move(3, 2, 0);
	/// ]]></code>
	/// The <c>result</c> variable will contain <c>{ 3, 4, 0, 1, 2, 5 }</c>.
	/// </example>

	public static IEnumerable<T> Move<T>(this IEnumerable<T> source, int fromIndex, int count, int toIndex)
	{
		Guard.IsNotNull(source);
		Guard.IsGreaterThanOrEqualTo(fromIndex, 0);
		Guard.IsGreaterThanOrEqualTo(count, 0);
		Guard.IsGreaterThanOrEqualTo(toIndex, 0);

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
}
