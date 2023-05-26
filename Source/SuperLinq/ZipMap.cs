namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Applies a function to each element in a sequence
	/// and returns a sequence of tuples containing both
	/// the original item as well as the function result.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements of source</typeparam>
	/// <typeparam name="TResult">The type of the value returned by selector</typeparam>
	/// <param name="source">A sequence of values to invoke a transform function on</param>
	/// <param name="selector">A transform function to apply to each source element</param>
	/// <returns>
	/// An <see cref="IEnumerable{T}"/> whose elements are a tuple of the original element and
	/// the item returned from calling the <paramref name="selector"/> on that element.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="selector"/> is <see langword="null"/>.</exception>
	public static IEnumerable<(TSource item, TResult result)> ZipMap<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(selector);

		if (source is IList<TSource> list)
			return new ZipMapIterator<TSource, TResult>(list, selector);

		return Core(source, selector);

		static IEnumerable<(TSource, TResult)> Core(IEnumerable<TSource> source, Func<TSource, TResult> selector)
		{
			foreach (var item in source)
				yield return (item, selector(item));
		}
	}

	private sealed class ZipMapIterator<TSource, TResult> : ListIterator<(TSource, TResult)>
	{
		private readonly IList<TSource> _source;
		private readonly Func<TSource, TResult> _selector;

		public ZipMapIterator(IList<TSource> source, Func<TSource, TResult> selector)
		{
			_source = source;
			_selector = selector;
		}

		public override int Count => _source.Count;

		protected override IEnumerable<(TSource, TResult)> GetEnumerable()
		{
			var src = _source;
			var cnt = (uint)src.Count;
			for (var i = 0; i < cnt; i++)
			{
				var el = src[i];
				yield return (el, _selector(el));
			}
		}

		protected override (TSource, TResult) ElementAt(int index)
		{
			Guard.IsBetweenOrEqualTo(index, 0, Count - 1);

			var el = _source[index];
			return (el, _selector(el));
		}
	}
}
