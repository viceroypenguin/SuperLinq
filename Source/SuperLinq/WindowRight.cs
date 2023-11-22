﻿namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Creates a right-aligned sliding window over the source sequence of a given size.
	/// </summary>
	/// <typeparam name="TSource">
	///	    The type of the elements of <paramref name="source"/>.
	/// </typeparam>
	/// <param name="source">
	///	    The sequence over which to create the sliding window.
	/// </param>
	/// <param name="size">
	///	    Size of the sliding window.
	/// </param>
	/// <returns>
	///	    A sequence representing each sliding window.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> is <see langword="null" />.
	/// </exception>
	/// <exception cref="ArgumentOutOfRangeException">
	///	    <paramref name="size"/> is below <c>1</c>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    A window can contain fewer elements than <paramref name="size"/>, especially as it slides over the start of
	///     the sequence.
	/// </para>
	/// <para>
	///	    This operator uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IEnumerable<IList<TSource>> WindowRight<TSource>(this IEnumerable<TSource> source, int size)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(size);

		if (source is IList<TSource> list)
			return new WindowRightIterator<TSource>(list, size);

		return Core(source, size);

		static IEnumerable<IList<TSource>> Core(IEnumerable<TSource> source, int size)
		{
			using var e = source.GetEnumerator();
			if (!e.MoveNext())
				yield break;

			var window = new TSource[1] { e.Current };

			for (var i = 1; i < size; i++)
			{
				if (!e.MoveNext())
				{
					yield return window;
					yield break;
				}

				var newWindow = new TSource[i + 1];
				window.AsSpan().CopyTo(newWindow);
				newWindow[i] = e.Current;

				yield return window;
				window = newWindow;
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

	private sealed class WindowRightIterator<T> : ListIterator<IList<T>>
	{
		private readonly IList<T> _source;
		private readonly int _size;

		public WindowRightIterator(IList<T> source, int size)
		{
			_source = source;
			_size = size;
		}

		public override int Count => _source.Count;

		protected override IEnumerable<IList<T>> GetEnumerable()
		{
			if (_source.Count == 0)
			{
				yield break;
			}

			var window = new T[1] { _source[0], };
			var max = (uint)Math.Min(_source.Count, _size);
			for (var i = 1; i < max; i++)
			{
				var newWindow = new T[i + 1];
				window.AsSpan()[..].CopyTo(newWindow);
				newWindow[^1] = _source[i];

				yield return window;
				window = newWindow;
			}

			max = (uint)_source.Count;
			for (var i = window.Length; i < max; i++)
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
			ArgumentOutOfRangeException.ThrowIfNegative(index);
			ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, Count);

			if (index < _size)
			{
				var arr = new T[index];
				var max = (uint)index;
				for (var i = 0; i < max; i++)
					arr[i] = _source[i];

				return arr;
			}
			else
			{
				var arr = new T[_size];
				var max = (uint)index + 1;
				for (int i = 0, j = index - _size + 1; i < arr.Length && j < max; i++, j++)
					arr[i] = _source[j];
				return arr;
			}
		}
	}
}
