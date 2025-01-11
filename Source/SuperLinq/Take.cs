// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if !NO_INDEX

namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Returns a specified range of contiguous elements from a sequence.
	/// </summary>
	/// <typeparam name="TSource">
	///	    The type of the elements of <paramref name="source" />.
	/// </typeparam>
	/// <param name="source">
	///	    The sequence to return elements from.
	/// </param>
	/// <param name="range">
	///	    The range of elements to return, which has start and end indexes either from the start or the end.
	/// </param>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source" /> is <see langword="null" />.
	/// </exception>
	///	<returns>
	///	    An <see cref="IEnumerable{T}" /> that contains the specified <paramref name="range" /> of elements from the
	///     <paramref name="source" /> sequence.
	/// </returns>
	/// <remarks>
	/// <para>
	///	    <see cref="Take" /> enumerates <paramref name="source" /> and yields elements whose indices belong to the
	///     specified <paramref name="range"/>.
	/// </para>
	/// <para>
	///	    This method is implemented by using deferred execution.
	/// </para>
	/// </remarks>
#if NET6_0_OR_GREATER
	[Obsolete("This method has been implemented by the framework.")]
	public static IEnumerable<TSource> Take<TSource>(IEnumerable<TSource> source, Range range)
#else
	public static IEnumerable<TSource> Take<TSource>(this IEnumerable<TSource> source, Range range)
#endif
	{
		ArgumentNullException.ThrowIfNull(source);

		var start = range.Start;
		var end = range.End;

		if (start.IsFromEnd)
		{
			if (start.Value == 0 || (end.IsFromEnd && end.Value >= start.Value))
				return [];
		}
		else if (!end.IsFromEnd)
		{
			return start.Value >= end.Value
				? []
				: TakeRangeIterator(source, start.Value, end.Value);
		}

		return TakeRangeFromEndIterator(source, start, end);
	}

	private static IEnumerable<TSource> TakeRangeIterator<TSource>(IEnumerable<TSource> source, int startIndex, int endIndex)
	{
		using var e = source.GetEnumerator();

		var index = 0;
		while (index < startIndex && e.MoveNext())
			++index;

		if (index < startIndex)
			yield break;

		while (index < endIndex && e.MoveNext())
		{
			yield return e.Current;
			++index;
		}
	}

	private static IEnumerable<TSource> TakeRangeFromEndIterator<TSource>(IEnumerable<TSource> source, Index start, Index end)
	{
		// Attempt to extract the count of the source enumerator,
		// in order to convert fromEnd indices to regular indices.
		// Enumerable counts can change over time, so it is very
		// important that this check happens at enumeration time;
		// do not move it outside of the iterator method.
		if (source.TryGetCollectionCount() is int count)
		{
			var startIndex = start.GetOffset(count);
			var endIndex = end.GetOffset(count);

			if (startIndex < endIndex)
			{
				foreach (var element in TakeRangeIterator(source, startIndex, endIndex))
					yield return element;
			}

			yield break;
		}

		Queue<TSource> queue;

		if (start.IsFromEnd)
		{
			// TakeLast compat: enumerator should be disposed before yielding the first element.
			using (var e = source.GetEnumerator())
			{
				if (!e.MoveNext())
					yield break;

				queue = new Queue<TSource>();
				queue.Enqueue(e.Current);
				count = 1;

				var startCount = start.Value;
				while (e.MoveNext())
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
			using var e = source.GetEnumerator();

			var startCount = start.Value;
			var endCount = end.Value;
			count = 0;
			while (count < startCount && e.MoveNext())
				++count;

			if (count == startCount)
			{
				queue = new Queue<TSource>();
				while (queue.Count != endCount && e.MoveNext())
					queue.Enqueue(e.Current);

				while (e.MoveNext())
				{
					queue.Enqueue(e.Current);
					yield return queue.Dequeue();
				}
			}
		}
	}
}

#endif
