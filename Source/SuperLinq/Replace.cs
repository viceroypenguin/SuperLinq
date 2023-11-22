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

	private sealed class ReplaceIterator<TSource> : ListIterator<TSource>
	{
		private readonly IList<TSource> _source;
		private readonly TSource _value;
		private readonly Index _index;

		public ReplaceIterator(IList<TSource> source, TSource value, Index index)
		{
			_source = source;
			_value = value;
			_index = index;
		}

		public override int Count => _source.Count;

		protected override IEnumerable<TSource> GetEnumerable()
		{
			var cnt = (uint)_source.Count;
			var idx = _index.GetOffset(_source.Count);

			for (var i = 0; i < cnt; i++)
				yield return i == idx ? _value : _source[i];
		}

		public override void CopyTo(TSource[] array, int arrayIndex)
		{
			ArgumentNullException.ThrowIfNull(array);
			ArgumentOutOfRangeException.ThrowIfNegative(arrayIndex);
			ArgumentOutOfRangeException.ThrowIfGreaterThan(arrayIndex, array.Length - Count);

			_source.CopyTo(array, arrayIndex);

			var idx = _index.GetOffset(_source.Count);
			if (idx >= 0 && idx < _source.Count)
				array[arrayIndex + idx] = _value;
		}

		protected override TSource ElementAt(int index)
		{
			ArgumentOutOfRangeException.ThrowIfNegative(index);
			ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, Count);

			return index == _index.GetOffset(_source.Count)
				? _value
				: _source[index];
		}
	}
}
