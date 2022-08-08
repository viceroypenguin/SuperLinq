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

		return _(source, selector);

		static IEnumerable<(TSource, TResult)> _(IEnumerable<TSource> source, Func<TSource, TResult> selector)
		{
			foreach (var item in source)
				yield return (item, selector(item));
		}
	}
}
