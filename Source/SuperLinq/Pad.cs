namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Pads a sequence with default values if it is narrower (shorter
	/// in length) than a given width.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
	/// <param name="source">The sequence to pad.</param>
	/// <param name="width">The width/length below which to pad.</param>
	/// <returns>
	/// Returns a sequence that is at least as wide/long as the width/length
	/// specified by the <paramref name="width"/> parameter.
	/// </returns>
	/// <remarks>
	/// This operator uses deferred execution and streams its results.
	/// </remarks>
	/// <example>
	/// <code><![CDATA[
	/// int[] numbers = { 123, 456, 789 };
	/// var result = numbers.Pad(5);
	/// ]]></code>
	/// The <c>result</c> variable, when iterated over, will yield
	/// 123, 456, 789 and two zeroes, in turn.
	/// </example>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="width"/> is less than 0.</exception>
	public static IEnumerable<TSource?> Pad<TSource>(this IEnumerable<TSource> source, int width)
	{
		return Pad(source, width, padding: default);
	}

	/// <summary>
	/// Pads a sequence with a given filler value if it is narrower (shorter
	/// in length) than a given width.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
	/// <param name="source">The sequence to pad.</param>
	/// <param name="width">The width/length below which to pad.</param>
	/// <param name="padding">The value to use for padding.</param>
	/// <returns>
	/// Returns a sequence that is at least as wide/long as the width/length
	/// specified by the <paramref name="width"/> parameter.
	/// </returns>
	/// <remarks>
	/// This operator uses deferred execution and streams its results.
	/// </remarks>
	/// <example>
	/// <code><![CDATA[
	/// int[] numbers = { 123, 456, 789 };
	/// var result = numbers.Pad(5, -1);
	/// ]]></code>
	/// The <c>result</c> variable, when iterated over, will yield
	/// 123, 456, and 789 followed by two occurrences of -1, in turn.
	/// </example>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="width"/> is less than 0.</exception>
	public static IEnumerable<TSource> Pad<TSource>(this IEnumerable<TSource> source, int width, TSource padding)
	{
		return Pad(source, width, paddingSelector: _ => padding);
	}

	/// <summary>
	/// Pads a sequence with a dynamic filler value if it is narrower (shorter
	/// in length) than a given width.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
	/// <param name="source">The sequence to pad.</param>
	/// <param name="width">The width/length below which to pad.</param>
	/// <param name="paddingSelector">Function to calculate padding.</param>
	/// <returns>
	/// Returns a sequence that is at least as wide/long as the width/length
	/// specified by the <paramref name="width"/> parameter.
	/// </returns>
	/// <remarks>
	/// This operator uses deferred execution and streams its results.
	/// </remarks>
	/// <example>
	/// <code><![CDATA[
	/// int[] numbers = { 0, 1, 2 };
	/// var result = numbers.Pad(5, i => -i);
	/// ]]></code>
	/// The <c>result</c> variable, when iterated over, will yield
	/// 0, 1, 2, -3 and -4, in turn.
	/// </example>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="paddingSelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="width"/> is less than 0.</exception>
	public static IEnumerable<TSource> Pad<TSource>(this IEnumerable<TSource> source, int width, Func<int, TSource> paddingSelector)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(paddingSelector);
		Guard.IsGreaterThanOrEqualTo(width, 0);

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
			Guard.IsNotNull(array);
			Guard.IsBetweenOrEqualTo(arrayIndex, 0, array.Length - Count);

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
			Guard.IsNotNull(array);
			Guard.IsBetweenOrEqualTo(arrayIndex, 0, array.Length - Count);

			_source.CopyTo(array, arrayIndex);

			for (var i = _source.Count; i < _width; i++)
				array[arrayIndex + i] = _paddingSelector(i);
		}

		protected override T ElementAt(int index)
		{
			Guard.IsBetweenOrEqualTo(index, 0, Count - 1);
			return index < _source.Count
				? _source[index]
				: _paddingSelector(index);
		}
	}
}
