namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Split the elements of a sequence into chunks of size at most <paramref name="size"/>.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
	/// <param name="source">An <see cref="IAsyncEnumerable{T}"/> whose elements to chunk.</param>
	/// <param name="size">The maximum size of each chunk.</param>
	/// <returns>An <see cref="IAsyncEnumerable{T}"/> that contains the elements the input sequence split into chunks of size
	/// size.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="size"/> is below 1.</exception>
	/// <remarks>
	/// <para>
	/// A chunk can contain fewer elements than <paramref name="size"/>, specifically the final buffer of <paramref
	/// name="source"/>.
	/// </para>
	/// <para>
	/// Returned subsequences are buffered, but the overall operation is streamed.<br/>
	/// </para>
	/// </remarks>
	public static IAsyncEnumerable<IList<TSource>> Batch<TSource>(this IAsyncEnumerable<TSource> source, int size)
	{
		// yes this operator duplicates on net6+; but no name overlap, so leave alone
		ArgumentNullException.ThrowIfNull(source);
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(size);

		return Core(source, size);

		static async IAsyncEnumerable<IList<TSource>> Core(
			IAsyncEnumerable<TSource> source, int size,
			[EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			TSource[]? array = null;

			var n = 0;
			await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
			{
				(array ??= new TSource[size])[n++] = item;
				if (n == size)
				{
					yield return array;
					n = 0;
				}
			}

			if (n != 0)
			{
				Array.Resize(ref array, n);
				yield return array;
			}
		}
	}
}
