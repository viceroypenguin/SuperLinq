namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Creates a left-aligned sliding window of a given size over the
	/// source sequence.
	/// </summary>
	/// <typeparam name="TSource">
	/// The type of the elements of <paramref name="source"/>.</typeparam>
	/// <param name="source">
	/// The sequence over which to create the sliding window.</param>
	/// <param name="size">Size of the sliding window.</param>
	/// <returns>A sequence representing each sliding window.</returns>
	/// <remarks>
	/// <para>
	/// A window can contain fewer elements than <paramref name="size"/>,
	/// especially as it slides over the end of the sequence.</para>
	/// <para>
	/// This operator uses deferred execution and streams its results.</para>
	/// </remarks>
	/// <example>
	/// <code><![CDATA[
	/// Console.WriteLine(
	///     Enumerable
	///         .Range(1, 5)
	///         .WindowLeft(3)
	///         .Select(w => "AVG(" + w.ToDelimitedString(",") + ") = " + w.Average())
	///         .ToDelimitedString(Environment.NewLine));
	///
	/// // Output:
	/// // AVG(1,2,3) = 2
	/// // AVG(2,3,4) = 3
	/// // AVG(3,4,5) = 4
	/// // AVG(4,5) = 4.5
	/// // AVG(5) = 5
	/// ]]></code>
	/// </example>
	public static IEnumerable<IList<TSource>> WindowLeft<TSource>(this IEnumerable<TSource> source, int size)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(size);

		if (source is IList<TSource> list)
			return new WindowLeftIterator<TSource>(list, size);

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
				{
					Array.Resize(ref window, i);
					goto skipLoop;
				}
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

skipLoop:
			while (window.Length > 1)
			{
				var newWindow = new TSource[window.Length - 1];
				window.AsSpan()[1..].CopyTo(newWindow);

				yield return window;
				window = newWindow;
			}

			yield return window;
		}
	}

	private sealed class WindowLeftIterator<T> : ListIterator<IList<T>>
	{
		private readonly IList<T> _source;
		private readonly int _size;

		public WindowLeftIterator(IList<T> source, int size)
		{
			_source = source;
			_size = size;
		}

		public override int Count => _source.Count;

		protected override IEnumerable<IList<T>> GetEnumerable()
		{
			T[] window;

			if (_source.Count == 0)
			{
				yield break;
			}
			else if (_source.Count > _size)
			{
				window = new T[_size];

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
			}
			else
			{
				window = _source.ToArray();
			}

			while (window.Length > 1)
			{
				var newWindow = new T[window.Length - 1];
				window.AsSpan()[1..].CopyTo(newWindow);

				yield return window;
				window = newWindow;
			}

			yield return window;
		}

		protected override IList<T> ElementAt(int index)
		{
			ArgumentOutOfRangeException.ThrowIfNegative(index);
			ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, Count);

			if (index < _source.Count - _size)
			{
				var arr = new T[_size];
				var max = (uint)(index + _size);
				for (int i = 0, j = index; i < _size && j < max; i++, j++)
					arr[i] = _source[j];

				return arr;
			}
			else
			{
				var arr = new T[_source.Count - index];
				var max = (uint)_source.Count;
				for (int i = 0, j = index; i < arr.Length && j < max; i++, j++)
					arr[i] = _source[j];
				return arr;
			}
		}
	}
}
