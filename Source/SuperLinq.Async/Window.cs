namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
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
	public static IAsyncEnumerable<IList<TSource>> Window<TSource>(this IAsyncEnumerable<TSource> source, int size)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(size);

		return Core(source, size);

		static async IAsyncEnumerable<IList<TSource>> Core(
			IAsyncEnumerable<TSource> source, int size,
			[EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			await using var e = source.GetConfiguredAsyncEnumerator(cancellationToken);
			if (!await e.MoveNextAsync())
				yield break;

			var window = new TSource[size];
			window[0] = e.Current;

			for (var i = 1; i < size; i++)
			{
				if (!await e.MoveNextAsync())
					yield break;

				window[i] = e.Current;
			}

			while (await e.MoveNextAsync())
			{
				var newWindow = new TSource[size];
				window.AsSpan()[1..].CopyTo(newWindow);
				newWindow[^1] = e.Current;

				yield return window;
				window = newWindow;
			}

			yield return window;
		}
	}
}
