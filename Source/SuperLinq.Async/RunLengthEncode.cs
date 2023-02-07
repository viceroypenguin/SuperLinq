namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Run-length encodes a sequence by converting consecutive instances of the same element into
	/// a tuple representing the item and its occurrence count.
	/// </summary>
	/// <typeparam name="T">The type of the elements in the sequence</typeparam>
	/// <param name="sequence">The sequence to run length encode</param>
	/// <returns>A sequence of tuples containing the element and the occurrence count</returns>

	public static IAsyncEnumerable<(T value, int count)> RunLengthEncode<T>(this IAsyncEnumerable<T> sequence)
	{
		return RunLengthEncode(sequence, comparer: null);
	}

	/// <summary>
	/// Run-length encodes a sequence by converting consecutive instances of the same element into
	/// a tuple representing the item and its occurrence count. This overload
	/// uses a custom equality comparer to identify equivalent items.
	/// </summary>
	/// <typeparam name="T">The type of the elements in the sequence</typeparam>
	/// <param name="sequence">The sequence to run length encode</param>
	/// <param name="comparer">The comparer used to identify equivalent items</param>
	/// <returns>A sequence of tuples containing the element and the occurrence count</returns>

	public static IAsyncEnumerable<(T value, int count)> RunLengthEncode<T>(this IAsyncEnumerable<T> sequence, IEqualityComparer<T>? comparer)
	{
		Guard.IsNotNull(sequence);

		return Core(sequence, comparer ?? EqualityComparer<T>.Default);

		static async IAsyncEnumerable<(T value, int count)> Core(IAsyncEnumerable<T> sequence, IEqualityComparer<T> comparer, [EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			// This implementation could also have been written using a foreach loop,
			// but it proved to be easier to deal with edge certain cases that occur
			// (such as empty sequences) using an explicit iterator and a while loop.

			await using var iter = sequence.GetConfiguredAsyncEnumerator(cancellationToken);

			if (await iter.MoveNextAsync())
			{
				var prevItem = iter.Current;
				var runCount = 1;

				while (await iter.MoveNextAsync())
				{
					if (comparer.Equals(prevItem, iter.Current))
					{
						++runCount;
					}
					else
					{
						yield return (prevItem, runCount);
						prevItem = iter.Current;
						runCount = 1;
					}
				}

				yield return (prevItem, runCount);
			}
		}
	}
}
