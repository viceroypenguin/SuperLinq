namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
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
	/// An <see cref="IAsyncEnumerable{T}"/> whose elements are a tuple of the original element and
	/// the item returned from calling the <paramref name="selector"/> on that element.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="selector"/> is <see langword="null"/>.</exception>
	public static IAsyncEnumerable<(TSource item, TResult result)> ZipMap<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, TResult> selector)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(selector);

		return source.ZipMap((x, _) => new ValueTask<TResult>(selector(x)));
	}

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
	/// An <see cref="IAsyncEnumerable{T}"/> whose elements are a tuple of the original element and
	/// the item returned from calling the <paramref name="selector"/> on that element.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="selector"/> is <see langword="null"/>.</exception>
	public static IAsyncEnumerable<(TSource item, TResult result)> ZipMap<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TResult>> selector)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(selector);

		return source.ZipMap((x, _) => selector(x));
	}

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
	/// An <see cref="IAsyncEnumerable{T}"/> whose elements are a tuple of the original element and
	/// the item returned from calling the <paramref name="selector"/> on that element.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="selector"/> is <see langword="null"/>.</exception>
	public static IAsyncEnumerable<(TSource item, TResult result)> ZipMap<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TResult>> selector)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(selector);

		return Core(source, selector);

		static async IAsyncEnumerable<(TSource, TResult)> Core(
			IAsyncEnumerable<TSource> source,
			Func<TSource, CancellationToken, ValueTask<TResult>> selector,
			[EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
				yield return (item, await selector(item, cancellationToken).ConfigureAwait(false));
		}
	}
}
