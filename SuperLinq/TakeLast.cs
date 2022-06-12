namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Returns a specified number of contiguous elements from the end of
	/// a sequence.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
	/// <param name="source">The sequence to return the last element of.</param>
	/// <param name="count">The number of elements to return.</param>
	/// <returns>
	/// An <see cref="IEnumerable{T}"/> that contains the specified number of
	/// elements from the end of the input sequence.
	/// </returns>
	/// <remarks>
	/// This operator uses deferred execution and streams its results.
	/// </remarks>
	/// <example>
	/// <code><![CDATA[
	/// int[] numbers = { 12, 34, 56, 78 };
	/// var result = numbers.TakeLast(2);
	/// ]]></code>
	/// The <c>result</c> variable, when iterated over, will yield
	/// 56 and 78 in turn.
	/// </example>

	public static IEnumerable<TSource> TakeLast<TSource>(this IEnumerable<TSource> source, int count)
	{
		if (source == null) throw new ArgumentNullException(nameof(source));

		if (count < 1)
			return Enumerable.Empty<TSource>();

		return
			source.TryGetCollectionCount() is { } collectionCount
			? source.Slice(Math.Max(0, collectionCount - count), int.MaxValue)
			: source.CountDown(count, (e, cd) => (Element: e, Countdown: cd))
					.SkipWhile(e => e.Countdown == null)
					.Select(e => e.Element);
	}
}
