namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Replaces a single value in a sequence at a specified index with the given replacement value.
	/// </summary>
	/// <typeparam name="TSource">
	///	    Type of item in the sequence
	/// </typeparam>
	/// <param name="source">
	///	    The source sequence.
	/// </param>
	/// <param name="index">
	///	    The index of the value to replace.
	/// </param>
	/// <param name="value">
	///	    The replacement value to use at <paramref name="index"/>.
	/// </param>
	/// <returns>
	///	    A sequence with the original values from <paramref name="source"/>, except for position <paramref
	///     name="index"/> which has the value <paramref name="value"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	///	    This operator evaluates in a deferred and streaming manner.
	/// </remarks>
	public static IEnumerable<TSource> Replace<TSource>(
		this IEnumerable<TSource> source,
		int index,
		TSource value)
	{
		ArgumentNullException.ThrowIfNull(source);

		if (source is IList<TSource> list)
			return new ReplaceIterator<TSource>(list, value, index);

		return Core(source, index, value);

		static IEnumerable<TSource> Core(
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
	///	    Replaces a single value in a sequence at a specified index with the given replacement value.
	/// </summary>
	/// <typeparam name="TSource">
	///	    Type of item in the sequence
	/// </typeparam>
	/// <param name="source">
	///	    The source sequence.
	/// </param>
	/// <param name="index">
	///	    The index of the value to replace.
	/// </param>
	/// <param name="value">
	///	    The replacement value to use at <paramref name="index"/>.
	/// </param>
	/// <returns>
	///	    A sequence with the original values from <paramref name="source"/>, except for position <paramref
	///     name="index"/> which has the value <paramref name="value"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	///	    This operator evaluates in a deferred and streaming manner.
	/// </remarks>
	public static IEnumerable<TSource> Replace<TSource>(
		this IEnumerable<TSource> source,
		Index index,
		TSource value)
	{
		ArgumentNullException.ThrowIfNull(source);

		if (source is IList<TSource> list)
			return new ReplaceIterator<TSource>(list, value, index);

		return Core(source, value, index);

		static IEnumerable<TSource> Core(IEnumerable<TSource> source, TSource value, Index index)
		{
			if (index.IsFromEnd
				&& source.TryGetCollectionCount() is int count)
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
				var cnt = index.Value;
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
					_ = queue.Dequeue();
				}

				while (queue.Count != 0)
					yield return queue.Dequeue();
			}
		}
	}

	private sealed class ReplaceIterator<TSource>(
		IList<TSource> source,
		TSource value,
		Index index
	) : ListIterator<TSource>
	{
		public override int Count => source.Count;

		protected override IEnumerable<TSource> GetEnumerable()
		{
			var cnt = (uint)source.Count;
			var idx = index.GetOffset(source.Count);

			for (var i = 0; i < cnt; i++)
				yield return i == idx ? value : source[i];
		}

		public override void CopyTo(TSource[] array, int arrayIndex)
		{
			ArgumentNullException.ThrowIfNull(array);
			ArgumentOutOfRangeException.ThrowIfNegative(arrayIndex);
			ArgumentOutOfRangeException.ThrowIfGreaterThan(arrayIndex, array.Length - Count);

			source.CopyTo(array, arrayIndex);

			var idx = index.GetOffset(source.Count);
			if (idx >= 0 && idx < source.Count)
				array[arrayIndex + idx] = value;
		}

		protected override TSource ElementAt(int index1)
		{
			ArgumentOutOfRangeException.ThrowIfNegative(index1);
			ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index1, Count);

			return index1 == index.GetOffset(source.Count)
				? value
				: source[index1];
		}
	}
}
