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

	private sealed class PadCollectionIterator<T> : CollectionIterator<T>
	{
		private readonly IEnumerable<T> _source;
		private readonly int _width;
		private readonly Func<int, T> _paddingSelector;

		public PadCollectionIterator(
			IEnumerable<T> source, int width,
			Func<int, T> paddingSelector)
		{
			_source = source;
			_width = width;
			_paddingSelector = paddingSelector;
		}

		public override int Count => Math.Max(_source.GetCollectionCount(), _width);

		protected override IEnumerable<T> GetEnumerable() =>
			PadCore(_source, _width, _paddingSelector);

		public override void CopyTo(T[] array, int arrayIndex)
		{
			ArgumentNullException.ThrowIfNull(array);
			ArgumentOutOfRangeException.ThrowIfNegative(arrayIndex);
			ArgumentOutOfRangeException.ThrowIfGreaterThan(arrayIndex, array.Length - Count);

			var cnt = _source.CopyTo(array, arrayIndex);

			for (var i = cnt; i < _width; i++)
				array[arrayIndex + i] = _paddingSelector(i);
		}
	}

	private sealed class PadListIterator<T> : ListIterator<T>
	{
		private readonly IList<T> _source;
		private readonly int _width;
		private readonly Func<int, T> _paddingSelector;

		public PadListIterator(IList<T> source, int width, Func<int, T> paddingSelector)
		{
			_source = source;
			_width = width;
			_paddingSelector = paddingSelector;
		}

		public override int Count => Math.Max(_source.Count, _width);

		protected override IEnumerable<T> GetEnumerable()
		{
			var src = _source;
			var cnt = (uint)src.Count;
			for (var i = 0; i < cnt; i++)
				yield return src[i];

			for (var i = (int)cnt; i < _width; i++)
				yield return _paddingSelector(i);
		}

		public override void CopyTo(T[] array, int arrayIndex)
		{
			ArgumentNullException.ThrowIfNull(array);
			ArgumentOutOfRangeException.ThrowIfNegative(arrayIndex);
			ArgumentOutOfRangeException.ThrowIfGreaterThan(arrayIndex, array.Length - Count);

			_source.CopyTo(array, arrayIndex);

			for (var i = _source.Count; i < _width; i++)
				array[arrayIndex + i] = _paddingSelector(i);
		}

		protected override T ElementAt(int index)
		{
			ArgumentOutOfRangeException.ThrowIfNegative(index);
			ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, Count);

			return index < _source.Count
				? _source[index]
				: _paddingSelector(index);
		}
	}
}
