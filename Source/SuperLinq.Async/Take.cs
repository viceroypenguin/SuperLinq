#if !NO_INDEX

namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>Returns a specified range of contiguous elements from a sequence.</summary>
	/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
	/// <param name="source">The sequence to return elements from.</param>
	/// <param name="range">The range of elements to return, which has start and end indexes either from the start or the end.</param>
	/// <exception cref="ArgumentNullException"><paramref name="source" /> is <see langword="null" />.</exception>
	/// <returns>An <see cref="IEnumerable{T}" /> that contains the specified <paramref name="range" /> of elements from the <paramref name="source" /> sequence.</returns>
	/// <remarks>
	/// <para>This method is implemented by using deferred execution. The immediate return value is an object that stores all the information that is required to perform the action. The query represented by this method is not executed until the object is enumerated either by calling its `GetEnumerator` method directly or by using `foreach` in Visual C# or `For Each` in Visual Basic.</para>
	/// <para><see cref="AsyncSuperEnumerable.Take" /> enumerates <paramref name="source" /> and yields elements whose indices belong to the specified <paramref name="range"/>.</para>
	/// </remarks>
	public static IAsyncEnumerable<TSource> Take<TSource>(this IAsyncEnumerable<TSource> source, Range range)
	{
		ArgumentNullException.ThrowIfNull(source);

		var start = range.Start;
		var end = range.End;

		if (start.IsFromEnd)
		{
			if (start.Value == 0 || (end.IsFromEnd && end.Value >= start.Value))
				return AsyncEnumerable.Empty<TSource>();
		}
		else if (!end.IsFromEnd)
		{
			return start.Value >= end.Value
				? AsyncEnumerable.Empty<TSource>()
				: TakeRangeIterator(source, start.Value, end.Value);
		}

		return TakeRangeFromEndIterator(source, start, end);
	}

	private static async IAsyncEnumerable<TSource> TakeRangeIterator<TSource>(
		IAsyncEnumerable<TSource> source,
		int startIndex,
		int endIndex,
		[EnumeratorCancellation] CancellationToken cancellationToken = default
	)
	{
		await using var e = source.GetConfiguredAsyncEnumerator(cancellationToken);

		var index = 0;
		while (index < startIndex && await e.MoveNextAsync())
			++index;

		if (index < startIndex)
			yield break;

		while (index < endIndex && await e.MoveNextAsync())
		{
			yield return e.Current;
			++index;
		}
	}

	private static async IAsyncEnumerable<TSource> TakeRangeFromEndIterator<TSource>(
		IAsyncEnumerable<TSource> source,
		Index start,
		Index end,
		[EnumeratorCancellation] CancellationToken cancellationToken = default
	)
	{
		Queue<TSource> queue;
		var count = 0;

		if (start.IsFromEnd)
		{
			// TakeLast compat: enumerator should be disposed before yielding the first element.
			await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken))
			{
				if (!await e.MoveNextAsync())
					yield break;

				queue = new Queue<TSource>();
				queue.Enqueue(e.Current);
				count = 1;

				var startCount = start.Value;
				while (await e.MoveNextAsync())
				{
					if (count >= startCount)
						_ = queue.Dequeue();

					queue.Enqueue(e.Current);
					checked
					{
						++count;
					}
				}
			}

			var startIndex = Math.Max(0, start.GetOffset(count));
			var endIndex = Math.Min(count, end.GetOffset(count));

			for (var rangeIndex = startIndex; rangeIndex < endIndex; rangeIndex++)
				yield return queue.Dequeue();
		}
		else
		{
			// SkipLast compat: the enumerator should be disposed at the end of the enumeration.
			await using var e = source.GetConfiguredAsyncEnumerator(cancellationToken);

			var startCount = start.Value;
			var endCount = end.Value;
			while (count < startCount && await e.MoveNextAsync())
				++count;

			if (count == startCount)
			{
				queue = new Queue<TSource>();
				while (queue.Count != endCount && await e.MoveNextAsync())
					queue.Enqueue(e.Current);

				while (await e.MoveNextAsync())
				{
					queue.Enqueue(e.Current);
					yield return queue.Dequeue();
				}
			}
		}
	}
}

#endif
