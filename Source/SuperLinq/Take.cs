// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace SuperLinq;

public static partial class SuperEnumerable
{
#if !NET6_0_OR_GREATER
	/// <summary>Returns a specified range of contiguous elements from a sequence.</summary>
	/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
	/// <param name="source">The sequence to return elements from.</param>
	/// <param name="range">The range of elements to return, which has start and end indexes either from the start or the end.</param>
	/// <exception cref="ArgumentNullException"><paramref name="source" /> is <see langword="null" />.</exception>
	/// <returns>An <see cref="IEnumerable{T}" /> that contains the specified <paramref name="range" /> of elements from the <paramref name="source" /> sequence.</returns>
	/// <remarks>
	/// <para>This method is implemented by using deferred execution. The immediate return value is an object that stores all the information that is required to perform the action. The query represented by this method is not executed until the object is enumerated either by calling its `GetEnumerator` method directly or by using `foreach` in Visual C# or `For Each` in Visual Basic.</para>
	/// <para><see cref="SuperEnumerable.Take" /> enumerates <paramref name="source" /> and yields elements whose indices belong to the specified <paramref name="range"/>.</para>
	/// </remarks>
	public static IEnumerable<TSource> Take<TSource>(this IEnumerable<TSource> source, Range range)
	{
		if (source == null)
		{
			throw new ArgumentNullException(nameof(source));
		}

		var start = range.Start;
		var end = range.End;
		var isStartIndexFromEnd = start.IsFromEnd;
		var isEndIndexFromEnd = end.IsFromEnd;
		var startIndex = start.Value;
		var endIndex = end.Value;

		if (isStartIndexFromEnd)
		{
			if (startIndex == 0 || (isEndIndexFromEnd && endIndex >= startIndex))
			{
				return Array.Empty<TSource>();
			}
		}
		else if (!isEndIndexFromEnd)
		{
			return startIndex >= endIndex
				? Array.Empty<TSource>()
				: TakeRangeIterator(source, startIndex, endIndex);
		}

		return TakeRangeFromEndIterator(source, isStartIndexFromEnd, startIndex, isEndIndexFromEnd, endIndex);
	}

	private static IEnumerable<TSource> TakeRangeIterator<TSource>(IEnumerable<TSource> source, int startIndex, int endIndex)
	{
		using var e = source.GetEnumerator();

		var index = 0;
		while (index < startIndex && e.MoveNext())
		{
			++index;
		}

		if (index < startIndex)
		{
			yield break;
		}

		while (index < endIndex && e.MoveNext())
		{
			yield return e.Current;
			++index;
		}
	}

	private static IEnumerable<TSource> TakeRangeFromEndIterator<TSource>(IEnumerable<TSource> source, bool isStartIndexFromEnd, int startIndex, bool isEndIndexFromEnd, int endIndex)
	{
		// Attempt to extract the count of the source enumerator,
		// in order to convert fromEnd indices to regular indices.
		// Enumerable counts can change over time, so it is very
		// important that this check happens at enumeration time;
		// do not move it outside of the iterator method.
		if (source is ICollection<TSource> c)
		{
			var count = c.Count;

			startIndex = CalculateStartIndex(isStartIndexFromEnd, startIndex, count);
			endIndex = CalculateEndIndex(isEndIndexFromEnd, endIndex, count);

			if (startIndex < endIndex)
			{
				foreach (var element in TakeRangeIterator(source, startIndex, endIndex))
				{
					yield return element;
				}
			}

			yield break;
		}

		Queue<TSource> queue;

		if (isStartIndexFromEnd)
		{
			var count = 0;

			// TakeLast compat: enumerator should be disposed before yielding the first element.
			using (var e = source.GetEnumerator())
			{
				if (!e.MoveNext())
				{
					yield break;
				}

				queue = new Queue<TSource>();
				queue.Enqueue(e.Current);
				count = 1;

				while (e.MoveNext())
				{
					if (count < startIndex)
					{
						queue.Enqueue(e.Current);
						++count;
					}
					else
					{
						do
						{
							queue.Dequeue();
							queue.Enqueue(e.Current);
							checked { ++count; }
						} while (e.MoveNext());
						break;
					}
				}
			}

			startIndex = CalculateStartIndex(isStartIndexFromEnd: true, startIndex, count);
			endIndex = CalculateEndIndex(isEndIndexFromEnd, endIndex, count);

			for (var rangeIndex = startIndex; rangeIndex < endIndex; rangeIndex++)
			{
				yield return queue.Dequeue();
			}
		}
		else
		{
			// SkipLast compat: the enumerator should be disposed at the end of the enumeration.
			using var e = source.GetEnumerator();

			var count = 0;
			while (count < startIndex && e.MoveNext())
			{
				++count;
			}

			if (count == startIndex)
			{
				queue = new Queue<TSource>();
				while (e.MoveNext())
				{
					if (queue.Count == endIndex)
					{
						do
						{
							queue.Enqueue(e.Current);
							yield return queue.Dequeue();
						} while (e.MoveNext());

						break;
					}
					else
					{
						queue.Enqueue(e.Current);
					}
				}
			}
		}

		static int CalculateStartIndex(bool isStartIndexFromEnd, int startIndex, int count) =>
			Math.Max(0, isStartIndexFromEnd ? count - startIndex : startIndex);

		static int CalculateEndIndex(bool isEndIndexFromEnd, int endIndex, int count) =>
			Math.Min(count, isEndIndexFromEnd ? count - endIndex : endIndex);
	}
#endif
}
