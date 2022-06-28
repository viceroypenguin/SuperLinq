namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Processes a sequence into a series of subsequences representing a windowed subset of the original
	/// </summary>
	/// <remarks>
	/// The number of sequences returned is: <c>Max(0, <paramref name="source"/>.Count() - <paramref name="size"/> + 1)</c><br/>
	/// Returned subsequences are buffered, but the overall operation is streamed.<br/>
	/// </remarks>
	/// <typeparam name="TSource">The type of the elements of the source sequence</typeparam>
	/// <param name="source">The sequence to evaluate a sliding window over</param>
	/// <param name="size">The size (number of elements) in each window</param>
	/// <returns>A series of sequences representing each sliding window subsequence</returns>
	public static IEnumerable<IList<TSource>> Window<TSource>(this IEnumerable<TSource> source, int size)
	{
		source.ThrowIfNull();
		size.ThrowIfLessThan(1);

		return WindowImpl(source, size, WindowType.Normal);
	}
}
