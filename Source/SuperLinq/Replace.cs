namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Replaces a single value in a sequence at a specified index with the given replacement value.
	/// </summary>
	/// <typeparam name="TSource">Type of item in the sequence</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="index">The index of the value to replace.</param>
	/// <param name="value">The replacement value to use at <paramref name="index"/>.</param>
	/// <returns>
	/// A sequence with the original values from <paramref name="source"/>, except for position <paramref name="index"/>
	/// which has the value <paramref name="value"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <remarks>
	/// This operator evaluates in a deferred and streaming manner.
	/// </remarks>
	public static IEnumerable<TSource> Replace<TSource>(
		this IEnumerable<TSource> source,
		int index,
		TSource value)
	{
		Guard.IsNotNull(source);

		return _(source, index, value);

		static IEnumerable<TSource> _(
			IEnumerable<TSource> source,
			int index,
			TSource value)
		{
			var i = 0;
			foreach (var e in source)
				yield return i++ == index ? value : e;
		}
	}

	/// <summary>
	/// Replaces a single value in a sequence at a specified index with the given replacement value.
	/// </summary>
	/// <typeparam name="TSource">Type of item in the sequence</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="index">The index of the value to replace.</param>
	/// <param name="value">The replacement value to use at <paramref name="index"/>.</param>
	/// <returns>
	/// A sequence with the original values from <paramref name="source"/>, except for position <paramref name="index"/>
	/// which has the value <paramref name="value"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <remarks>
	/// This operator evaluates in a deferred and streaming manner.
	/// </remarks>
	public static IEnumerable<TSource> Replace<TSource>(
		this IEnumerable<TSource> source,
		Index index,
		TSource value)
	{
		Guard.IsNotNull(source);

		return _(source, value, index);

		static IEnumerable<TSource> _(IEnumerable<TSource> source, TSource value, Index index)
		{
			if (index.IsFromEnd
				&& source.TryGetCollectionCount(out var count))
			{
				index = index.GetOffset(count);
			}

			if (!index.IsFromEnd)
			{
				var cnt = index.Value;
				var i = 0;
				foreach (var e in source)
					yield return i++ == cnt ? value : e;
			}
			else
			{
				var cnt = index.Value + 1;
				var queue = new Queue<TSource>();

				foreach (var e in source)
				{
					queue.Enqueue(e);
					if (queue.Count > cnt)
						yield return queue.Dequeue();
				}

				if (queue.Count == 0)
					yield break;

				if (queue.Count == cnt)
				{
					yield return value;
					queue.Dequeue();
				}

				while (queue.Count != 0)
					yield return queue.Dequeue();
			}
		}
	}
}
