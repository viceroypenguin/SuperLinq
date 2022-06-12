namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Bypasses a specified number of elements at the end of the sequence.
	/// </summary>
	/// <typeparam name="T">Type of the source sequence</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="count">The number of elements to bypass at the end of the source sequence.</param>
	/// <returns>
	/// An <see cref="IEnumerable{T}"/> containing the source sequence elements except for the bypassed ones at the end.
	/// </returns>

	public static IEnumerable<T> SkipLast<T>(this IEnumerable<T> source, int count)
	{
		if (source == null) throw new ArgumentNullException(nameof(source));

		if (count < 1)
			return source;

		return
			source.TryGetCollectionCount() is { } collectionCount
			? source.Take(collectionCount - count)
			: source.CountDown(count, (e, cd) => (Element: e, Countdown: cd))
					.TakeWhile(e => e.Countdown == null)
					.Select(e => e.Element);
	}
}
