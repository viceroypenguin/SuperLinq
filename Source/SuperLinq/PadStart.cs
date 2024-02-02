namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Pads a sequence with default values in the beginning if it is narrower (shorter in length) than a given
	///     width.
	/// </summary>
	/// <typeparam name="TSource">
	///	    The type of the elements of <paramref name="source"/>.
	/// </typeparam>
	/// <param name="source">
	///	    The sequence to pad.
	/// </param>
	/// <param name="width">
	///	    The width/length below which to pad.
	/// </param>
	/// <returns>
	///	    Returns a sequence that is at least as wide/long as the width/length specified by the <paramref
	///     name="width"/> parameter.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> is <see langword="null"/>.
	/// </exception>
	/// <exception cref="ArgumentOutOfRangeException">
	///	    <paramref name="width"/> is less than <c>0</c>.
	/// </exception>
	/// <remarks>
	///	    This operator uses deferred execution and streams its results.
	/// </remarks>
	public static IEnumerable<TSource?> PadStart<TSource>(this IEnumerable<TSource> source, int width)
	{
		return PadStart(source, width, padding: default);
	}

	/// <summary>
	///	    Pads a sequence with default values in the beginning if it is narrower (shorter in length) than a given
	///     width.
	/// </summary>
	/// <typeparam name="TSource">
	///	    The type of the elements of <paramref name="source"/>.
	/// </typeparam>
	/// <param name="source">
	///	    The sequence to pad.
	/// </param>
	/// <param name="width">
	///	    The width/length below which to pad.
	/// </param>
	/// <param name="padding">
	///		The value to use for padding.
	/// </param>
	/// <returns>
	///	    Returns a sequence that is at least as wide/long as the width/length specified by the <paramref
	///     name="width"/> parameter.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> is <see langword="null"/>.
	/// </exception>
	/// <exception cref="ArgumentOutOfRangeException">
	///	    <paramref name="width"/> is less than <c>0</c>.
	/// </exception>
	/// <remarks>
	///	    This operator uses deferred execution and streams its results.
	/// </remarks>
	public static IEnumerable<TSource> PadStart<TSource>(this IEnumerable<TSource> source, int width, TSource padding)
	{
		return PadStart(source, width, paddingSelector: _ => padding);
	}

	/// <summary>
	///	    Pads a sequence with default values in the beginning if it is narrower (shorter in length) than a given
	///     width.
	/// </summary>
	/// <typeparam name="TSource">
	///	    The type of the elements of <paramref name="source"/>.
	/// </typeparam>
	/// <param name="source">
	///	    The sequence to pad.
	/// </param>
	/// <param name="width">
	///	    The width/length below which to pad.
	/// </param>
	/// <param name="paddingSelector">
	///		A function to generate the value used as padding.
	/// </param>
	/// <returns>
	///	    Returns a sequence that is at least as wide/long as the width/length specified by the <paramref
	///     name="width"/> parameter.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="paddingSelector"/> is <see langword="null"/>.
	/// </exception>
	/// <exception cref="ArgumentOutOfRangeException">
	///	    <paramref name="width"/> is less than <c>0</c>.
	/// </exception>
	/// <remarks>
	///	    This operator uses deferred execution and streams its results.
	/// </remarks>
	public static IEnumerable<TSource> PadStart<TSource>(this IEnumerable<TSource> source, int width, Func<int, TSource> paddingSelector)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(paddingSelector);
		ArgumentOutOfRangeException.ThrowIfNegative(width);

		if (source is IList<TSource> list)
			return new PadStartListIterator<TSource>(list, width, paddingSelector);

		if (source.TryGetCollectionCount() is int)
			return new PadStartCollectionIterator<TSource>(source, width, paddingSelector);

		return Core(source, width, paddingSelector);

		static IEnumerable<TSource> Core(
			IEnumerable<TSource> source, int width,
			Func<int, TSource> paddingSelector)
		{
			var array = new TSource[width];
			var count = 0;

			using (var e = source.GetEnumerator())
			{
				for (; count < width && e.MoveNext(); count++)
					array[count] = e.Current;

				if (count == width)
				{
					for (var i = 0; i < count; i++)
						yield return array[i];

					while (e.MoveNext())
						yield return e.Current;

					yield break;
				}
			}

			var len = width - count;

			for (var i = 0; i < len; i++)
				yield return paddingSelector(i);

			for (var i = 0; i < count; i++)
				yield return array[i];
		}
	}

	private sealed class PadStartCollectionIterator<T>(
		IEnumerable<T> source,
		int width,
		Func<int, T> paddingSelector
	) : CollectionIterator<T>
	{
		public override int Count => Math.Max(source.GetCollectionCount(), width);

		protected override IEnumerable<T> GetEnumerable()
		{
			var cnt = width - source.GetCollectionCount();
			for (var i = 0; i < cnt; i++)
				yield return paddingSelector(i);
			foreach (var item in source)
				yield return item;
		}

		public override void CopyTo(T[] array, int arrayIndex)
		{
			ArgumentNullException.ThrowIfNull(array);
			ArgumentOutOfRangeException.ThrowIfNegative(arrayIndex);
			ArgumentOutOfRangeException.ThrowIfGreaterThan(arrayIndex, array.Length - Count);

			var offset = Math.Max(width - source.GetCollectionCount(), 0);
			for (var i = 0; i < offset; i++)
				array[arrayIndex + i] = paddingSelector(i);

			_ = source.CopyTo(array, arrayIndex + offset);
		}
	}

	private sealed class PadStartListIterator<T>(
		IList<T> source,
		int width,
		Func<int, T> paddingSelector
	) : ListIterator<T>
	{
		public override int Count => Math.Max(source.Count, width);

		protected override IEnumerable<T> GetEnumerable()
		{
			var cnt = (uint)source.Count;
			for (var i = 0; i < width - cnt; i++)
				yield return paddingSelector(i);
			for (var i = 0; i < cnt; i++)
				yield return source[i];
		}

		public override void CopyTo(T[] array, int arrayIndex)
		{
			ArgumentNullException.ThrowIfNull(array);
			ArgumentOutOfRangeException.ThrowIfNegative(arrayIndex);
			ArgumentOutOfRangeException.ThrowIfGreaterThan(arrayIndex, array.Length - Count);

			var offset = Math.Max(width - source.Count, 0);
			for (var i = 0; i < offset; i++)
				array[arrayIndex + i] = paddingSelector(i);

			source.CopyTo(array, arrayIndex + offset);
		}

		protected override T ElementAt(int index)
		{
			ArgumentOutOfRangeException.ThrowIfNegative(index);
			ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, Count);

			var offset = Math.Max(width - source.Count, 0);
			return index < offset
				? paddingSelector(index)
				: source[index - offset];
		}
	}
}
