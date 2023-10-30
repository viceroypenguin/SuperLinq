namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Excludes a contiguous number of elements from a sequence starting at a given index.
	/// </summary>
	/// <typeparam name="T">The type of the elements of the sequence</typeparam>
	/// <param name="sequence">The sequence to exclude elements from</param>
	/// <param name="startIndex">The zero-based index at which to begin excluding elements</param>
	/// <param name="count">The number of elements to exclude</param>
	/// <returns>A sequence that excludes the specified portion of elements</returns>
	/// <exception cref="ArgumentNullException"><paramref name="sequence"/> is null.</exception>
	/// <exception cref="ArgumentOutOfRangeException">
	/// <paramref name="startIndex"/> or <paramref name="count"/> is less than <c>0</c>.
	/// </exception>
	public static IEnumerable<T> Exclude<T>(this IEnumerable<T> sequence, int startIndex, int count)
	{
		ArgumentNullException.ThrowIfNull(sequence);
		Guard.IsGreaterThanOrEqualTo(startIndex, 0);
		Guard.IsGreaterThanOrEqualTo(count, 0);

		if (count == 0)
			return sequence;

		if (sequence is IList<T> list)
			return new ExcludeIterator<T>(list, startIndex, count);

		return Core(sequence, startIndex, count);

		static IEnumerable<T> Core(IEnumerable<T> sequence, int startIndex, int count)
		{
			var index = 0;
			var endIndex = startIndex + count;

			foreach (var item in sequence)
			{
				if (index < startIndex || index >= endIndex)
					yield return item;
				index++;
			}
		}
	}

	private sealed class ExcludeIterator<T> : ListIterator<T>
	{
		private readonly IList<T> _source;
		private readonly int _startIndex;
		private readonly int _count;

		public ExcludeIterator(IList<T> source, int startIndex, int count)
		{
			_source = source;
			_startIndex = startIndex;
			_count = count;
		}

		public override int Count =>
			_source.Count < _startIndex ? _source.Count :
			_source.Count < _startIndex + _count ? _startIndex :
			_source.Count - _count;

		protected override IEnumerable<T> GetEnumerable()
		{
			var cnt = (uint)_source.Count;
			for (var i = 0; i < cnt && i < _startIndex; i++)
				yield return _source[i];

			for (var i = _startIndex + _count; i < cnt; i++)
				yield return _source[i];
		}

		protected override T ElementAt(int index)
		{
			Guard.IsBetweenOrEqualTo(index, 0, Count - 1);

			return index < _startIndex
				? _source[index]
				: _source[index + _count];
		}
	}
}
