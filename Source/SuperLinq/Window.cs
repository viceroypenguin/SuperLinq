namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Processes a sequence into a series of subsequences representing a windowed subset of the original
	/// </summary>
	/// <remarks>
	/// The number of sequences returned is: <c>Max(0, sequence.Count() - windowSize) + 1</c><br/>
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

		return _(); IEnumerable<IList<TSource>> _()
		{
			using var iter = source.GetEnumerator();

			// generate the first window of items
			var window = new TSource[size];
			int i;
			for (i = 0; i < size && iter.MoveNext(); i++)
				window[i] = iter.Current;

			if (i < size)
				yield break;

			while (iter.MoveNext())
			{
				// generate the next window by shifting forward by one item
				// and do that before exposing the data
				var newWindow = new TSource[size];
				Array.Copy(window, 1, newWindow, 0, size - 1);
				newWindow[size - 1] = iter.Current;

				yield return window;
				window = newWindow;
			}

			// return the last window.
			yield return window;
		}
	}

	/// <summary>
	/// Processes a sequence into a series of subsequences representing a windowed subset of the original
	/// </summary>
	/// <remarks>
	/// The number of sequences returned is: <c>Max(0, sequence.Count() - windowSize) + 1</c><br/>
	/// Returned subsequences are buffered, but the overall operation is streamed.<br/>
	/// </remarks>
	/// <typeparam name="TSource">The type of the elements of the source sequence</typeparam>
	/// <param name="source">The sequence to evaluate a sliding window over</param>
	/// <param name="size">The size (number of elements) in each window</param>
	/// <returns>A series of sequences representing each sliding window subsequence</returns>

	[Obsolete("Use " + nameof(Window) + " instead.")]
	public static IEnumerable<IEnumerable<TSource>> Windowed<TSource>(this IEnumerable<TSource> source, int size) =>
		source.Window(size);
}
