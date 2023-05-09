namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Pads a sequence with default values in the beginning if it is narrower (shorter
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
	/// var result = numbers.PadStart(5);
	/// ]]></code>
	/// The <c>result</c> variable will contain <c>{ 0, 0, 123, 456, 789 }</c>.
	/// </example>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="width"/> is less than 0.</exception>
	public static IEnumerable<TSource?> PadStart<TSource>(this IEnumerable<TSource> source, int width)
	{
		return PadStart(source, width, padding: default);
	}

	/// <summary>
	/// Pads a sequence with a given filler value in the beginning if it is narrower (shorter
	/// in length) than a given width.
	/// An additional parameter specifies the value to use for padding.
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
	/// var result = numbers.PadStart(5, -1);
	/// ]]></code>
	/// The <c>result</c> variable will contain <c>{ -1, -1, 123, 456, 789 }</c>.
	/// </example>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="width"/> is less than 0.</exception>
	public static IEnumerable<TSource> PadStart<TSource>(this IEnumerable<TSource> source, int width, TSource padding)
	{
		return PadStart(source, width, paddingSelector: _ => padding);
	}

	/// <summary>
	/// Pads a sequence with a dynamic filler value in the beginning if it is narrower (shorter
	/// in length) than a given width.
	/// An additional parameter specifies the function to calculate padding.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
	/// <param name="source">The sequence to pad.</param>
	/// <param name="width">The width/length below which to pad.</param>
	/// <param name="paddingSelector">
	/// Function to calculate padding given the index of the missing element.
	/// </param>
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
	/// var result = numbers.PadStart(6, i => -i);
	/// ]]></code>
	/// The <c>result</c> variable will contain <c>{ 0, -1, -2, 123, 456, 789 }</c>.
	/// </example>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="paddingSelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="width"/> is less than 0.</exception>
	public static IEnumerable<TSource> PadStart<TSource>(this IEnumerable<TSource> source, int width, Func<int, TSource> paddingSelector)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(paddingSelector);
		Guard.IsGreaterThanOrEqualTo(width, 0);

		if (source is IList<TSource> list)
			return new PadStartListIterator<TSource>(list, width, paddingSelector);

		return Core(source, width, paddingSelector);

		static IEnumerable<TSource> Core(
			IEnumerable<TSource> source, int width,
			Func<int, TSource> paddingSelector)
		{
			if (source.TryGetCollectionCount() is int collectionCount)
			{
				for (var i = 0; i < width - collectionCount; i++)
					yield return paddingSelector(i);
				foreach (var item in source)
					yield return item;
			}
			else
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
	}

	private sealed class PadStartListIterator<T> : ListIterator<T>
	{
		private readonly IList<T> _source;
		private readonly int _width;
		private readonly Func<int, T> _paddingSelector;

		public PadStartListIterator(IList<T> source, int width, Func<int, T> paddingSelector)
		{
			_source = source;
			_width = width;
			_paddingSelector = paddingSelector;
		}

		public override int Count => Math.Max(_source.Count, _width);

		protected override IEnumerable<T> GetEnumerable()
		{
			var cnt = (uint)_source.Count;
			for (var i = 0; i < _width - cnt; i++)
				yield return _paddingSelector(i);
			for (var i = 0; i < cnt; i++)
				yield return _source[i];
		}

		public override void CopyTo(T[] array, int arrayIndex)
		{
			Guard.IsNotNull(array);
			Guard.IsGreaterThanOrEqualTo(arrayIndex, 0);

			var offset = Math.Max(_width - _source.Count, 0);
			for (var i = 0; i < offset; i++)
				array[arrayIndex + i] = _paddingSelector(i);

			_source.CopyTo(array, arrayIndex + offset);
		}

		protected override T ElementAt(int index)
		{
			Guard.IsLessThan(index, Count);

			var offset = Math.Max(_width - _source.Count, 0);
			return index < offset
				? _paddingSelector(index)
				: _source[index - offset];
		}
	}
}
