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
		ArgumentNullException.ThrowIfNull(source);
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(size);

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

	private sealed class WindowIterator<T>(
		IList<T> source,
		int size
	) : ListIterator<IList<T>>
	{
		public override int Count => Math.Max(source.Count - size + 1, 0);

		protected override IEnumerable<IList<T>> GetEnumerable()
		{
			if (Count < size)
				yield break;

			var window = new T[size];

			for (var i = 0; i < size; i++)
				window[i] = source[i];

			var count = (uint)source.Count;
			for (var i = size; i < count; i++)
			{
				var newWindow = new T[size];
				window.AsSpan()[1..].CopyTo(newWindow);
				newWindow[^1] = source[i];

				yield return window;
				window = newWindow;
			}

			yield return window;
		}

		protected override IList<T> ElementAt(int index)
		{
			ArgumentOutOfRangeException.ThrowIfNegative(index);
			ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, Count);

			var arr = new T[size];
			var max = (uint)(index + size);
			for (int i = 0, j = index; i < size && j < max; i++, j++)
				arr[i] = source[j];

			return arr;
		}
	}
}
