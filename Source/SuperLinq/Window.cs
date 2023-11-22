namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Processes a sequence into a series of subsequences representing a windowed subset of the original
	/// </summary>
	/// <typeparam name="TSource">
	///	    The type of the elements of the source sequence
	/// </typeparam>
	/// <param name="source">
	///	    The sequence to evaluate a sliding window over
	/// </param>
	/// <param name="size">
	///	    The size (number of elements) in each window
	/// </param>
	/// <returns>
	///	    A series of sequences representing each sliding window subsequence
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> is <see langword="null" />.
	/// </exception>
	/// <exception cref="ArgumentOutOfRangeException">
	///	    <paramref name="size"/> is below <c>1</c>.
	/// </exception>
	/// <remarks>
	///	    The number of sequences returned is: <c>Max(0, <paramref name="source"/>.Count() - <paramref name="size"/> +
	///     1)</c><br/> Returned subsequences are buffered, but the overall operation is streamed.
	/// </remarks>
	public static IEnumerable<IList<TSource>> Window<TSource>(this IEnumerable<TSource> source, int size)
	{
		Guard.IsNotNull(source);
		Guard.IsGreaterThanOrEqualTo(size, 1);

		if (source is IList<TSource> list)
			return new WindowIterator<TSource>(list, size);

		return Core(source, size);

		static IEnumerable<IList<TSource>> Core(IEnumerable<TSource> source, int size)
		{
			using var e = source.GetEnumerator();
			if (!e.MoveNext())
				yield break;

			var window = new TSource[size];
			window[0] = e.Current;

			for (var i = 1; i < size; i++)
			{
				if (!e.MoveNext())
					yield break;
				window[i] = e.Current;
			}

			while (e.MoveNext())
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

	private sealed class WindowIterator<T> : ListIterator<IList<T>>
	{
		private readonly IList<T> _source;
		private readonly int _size;

		public WindowIterator(IList<T> source, int size)
		{
			_source = source;
			_size = size;
		}

		public override int Count => Math.Max(_source.Count - _size + 1, 0);

		protected override IEnumerable<IList<T>> GetEnumerable()
		{
			if (Count < _size)
				yield break;

			var window = new T[_size];

			for (var i = 0; i < _size; i++)
				window[i] = _source[i];

			var count = (uint)_source.Count;
			for (var i = _size; i < count; i++)
			{
				var newWindow = new T[_size];
				window.AsSpan()[1..].CopyTo(newWindow);
				newWindow[^1] = _source[i];

				yield return window;
				window = newWindow;
			}

			yield return window;
		}

		protected override IList<T> ElementAt(int index)
		{
			Guard.IsBetweenOrEqualTo(index, 0, Count - 1);

			var arr = new T[_size];
			var max = (uint)(index + _size);
			for (int i = 0, j = index; i < _size && j < max; i++, j++)
				arr[i] = _source[j];

			return arr;
		}
	}
}
