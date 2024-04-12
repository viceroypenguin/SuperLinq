using System.Diagnostics.CodeAnalysis;

namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Creates a left-aligned sliding window of a given size over the source sequence.
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
	///	    A window can contain fewer elements than <paramref name="size"/>, especially as it slides over the end of
	///     the sequence.
	/// </para>
	/// <para>
	///	    This operator uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
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

	private sealed class WindowLeftIterator<T>(
		IList<T> source,
		int size
	) : ListIterator<IList<T>>
	{
		public override int Count => source.Count;

		[SuppressMessage("Style", "IDE0305:Simplify collection initialization")]
		protected override IEnumerable<IList<T>> GetEnumerable()
		{
			T[] window;

			if (source.Count == 0)
			{
				yield break;
			}
			else if (source.Count > size)
			{
				window = new T[size];

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
			}
			else
			{
				window = source.ToArray();
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

			if (index < source.Count - size)
			{
				var arr = new T[size];
				var max = (uint)(index + size);
				for (int i = 0, j = index; i < size && j < max; i++, j++)
					arr[i] = source[j];

				return arr;
			}
			else
			{
				var arr = new T[source.Count - index];
				var max = (uint)source.Count;
				for (int i = 0, j = index; i < arr.Length && j < max; i++, j++)
					arr[i] = source[j];

				return arr;
			}
		}
	}
}
