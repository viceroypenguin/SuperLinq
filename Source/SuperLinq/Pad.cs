namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Pads a sequence with default values if it is narrower (shorter in length) than a given width.
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
	public static IEnumerable<TSource?> Pad<TSource>(this IEnumerable<TSource> source, int width)
	{
		return Pad(source, width, padding: default);
	}

	/// <summary>
	///	    Pads a sequence with default values if it is narrower (shorter in length) than a given width.
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
	public static IEnumerable<TSource> Pad<TSource>(this IEnumerable<TSource> source, int width, TSource padding)
	{
		return Pad(source, width, paddingSelector: _ => padding);
	}

	/// <summary>
	///	    Pads a sequence with default values if it is narrower (shorter in length) than a given width.
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
	public static IEnumerable<TSource> Pad<TSource>(this IEnumerable<TSource> source, int width, Func<int, TSource> paddingSelector)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(paddingSelector);
		ArgumentOutOfRangeException.ThrowIfNegative(width);

		if (source is IList<TSource> list)
			return new PadListIterator<TSource>(list, width, paddingSelector);

		if (source.TryGetCollectionCount() is int)
			return new PadCollectionIterator<TSource>(source, width, paddingSelector);

		return PadCore(source, width, paddingSelector);
	}

	private static IEnumerable<TSource> PadCore<TSource>(
		IEnumerable<TSource> source, int width,
		Func<int, TSource> paddingSelector)
	{
		var count = 0;
		foreach (var item in source)
		{
			yield return item;
			count++;
		}

		while (count < width)
		{
			yield return paddingSelector(count);
			count++;
		}
	}

	private sealed class PadCollectionIterator<T>(
		IEnumerable<T> source,
		int width,
		Func<int, T> paddingSelector
	) : CollectionIterator<T>
	{
		public override int Count => Math.Max(source.GetCollectionCount(), width);

		protected override IEnumerable<T> GetEnumerable() =>
			PadCore(source, width, paddingSelector);

		public override void CopyTo(T[] array, int arrayIndex)
		{
			ArgumentNullException.ThrowIfNull(array);
			ArgumentOutOfRangeException.ThrowIfNegative(arrayIndex);
			ArgumentOutOfRangeException.ThrowIfGreaterThan(arrayIndex, array.Length - Count);

			var cnt = source.CopyTo(array, arrayIndex);

			for (var i = cnt; i < width; i++)
				array[arrayIndex + i] = paddingSelector(i);
		}
	}

	private sealed class PadListIterator<T>(
		IList<T> source,
		int width, Func<int, T> paddingSelector
	) : ListIterator<T>
	{
		public override int Count => Math.Max(source.Count, width);

		protected override IEnumerable<T> GetEnumerable()
		{
			var src = source;
			var cnt = (uint)src.Count;
			for (var i = 0; i < cnt; i++)
				yield return src[i];

			for (var i = (int)cnt; i < width; i++)
				yield return paddingSelector(i);
		}

		public override void CopyTo(T[] array, int arrayIndex)
		{
			ArgumentNullException.ThrowIfNull(array);
			ArgumentOutOfRangeException.ThrowIfNegative(arrayIndex);
			ArgumentOutOfRangeException.ThrowIfGreaterThan(arrayIndex, array.Length - Count);

			source.CopyTo(array, arrayIndex);

			for (var i = source.Count; i < width; i++)
				array[arrayIndex + i] = paddingSelector(i);
		}

		protected override T ElementAt(int index)
		{
			ArgumentOutOfRangeException.ThrowIfNegative(index);
			ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, Count);

			return index < source.Count
				? source[index]
				: paddingSelector(index);
		}
	}
}
