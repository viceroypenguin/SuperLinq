namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Returns the elements of a sequence, but if it is empty then
	/// returns an alternate sequence from an array of values.
	/// </summary>
	/// <typeparam name="T">The type of the elements in the sequences.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="fallback">The array that is returned as the alternate
	/// sequence if <paramref name="source"/> is empty.</param>
	/// <returns>
	/// An <see cref="IAsyncEnumerable{T}"/> that containing fallback values
	/// if <paramref name="source"/> is empty; otherwise, <paramref name="source"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="fallback"/> is <see langword="null"/>.</exception>
	public static IAsyncEnumerable<T> FallbackIfEmpty<T>(this IAsyncEnumerable<T> source, params T[] fallback)
	{
		return source.FallbackIfEmpty((IEnumerable<T>)fallback);
	}

	/// <summary>
	/// Returns the elements of a sequence, but if it is empty then
	/// returns an alternate sequence from an array of values.
	/// </summary>
	/// <typeparam name="T">The type of the elements in the sequences.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="fallback">The array that is returned as the alternate
	/// sequence if <paramref name="source"/> is empty.</param>
	/// <returns>
	/// An <see cref="IAsyncEnumerable{T}"/> that containing fallback values
	/// if <paramref name="source"/> is empty; otherwise, <paramref name="source"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="fallback"/> is <see langword="null"/>.</exception>
	public static IAsyncEnumerable<T> FallbackIfEmpty<T>(this IAsyncEnumerable<T> source, IEnumerable<T> fallback)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(fallback);

		return source.FallbackIfEmpty(fallback.ToAsyncEnumerable());
	}

	/// <summary>
	/// Returns the elements of a sequence, but if it is empty then
	/// returns an alternate sequence of values.
	/// </summary>
	/// <typeparam name="T">The type of the elements in the sequences.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="fallback">The alternate sequence that is returned
	/// if <paramref name="source"/> is empty.</param>
	/// <returns>
	/// An <see cref="IAsyncEnumerable{T}"/> that containing fallback values
	/// if <paramref name="source"/> is empty; otherwise, <paramref name="source"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="fallback"/> is <see langword="null"/>.</exception>
	public static IAsyncEnumerable<T> FallbackIfEmpty<T>(this IAsyncEnumerable<T> source, IAsyncEnumerable<T> fallback)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(fallback);

		return _(source, fallback);

		static async IAsyncEnumerable<T> _(IAsyncEnumerable<T> source, IAsyncEnumerable<T> fallback, [EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken))
			{
				if (await e.MoveNextAsync())
				{
					do { yield return e.Current; }
					while (await e.MoveNextAsync());
					yield break;
				}
			}

			await foreach (var item in fallback.WithCancellation(cancellationToken).ConfigureAwait(false))
				yield return item;
		}
	}
}
