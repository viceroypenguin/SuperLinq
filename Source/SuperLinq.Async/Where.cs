namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Filters a sequence of values based on an enumeration of boolean values
	/// </summary>
	/// <typeparam name="TSource">The type of the elements of source.</typeparam>
	/// <param name="source">An <see cref="IAsyncEnumerable{T}"/> to filter.</param>
	/// <param name="filter">An <see cref="IAsyncEnumerable{T}"/> of boolean values identifying which elements of <paramref name="source"/> to keep.</param>
	/// <returns>
	/// An <see cref="IAsyncEnumerable{T}"/> that contains elements from the input sequence
	/// where the matching value in <paramref name="filter"/> is <see langword="true"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="filter"/> is <see langword="null"/>.</exception>
	public static IAsyncEnumerable<TSource> Where<TSource>(this IAsyncEnumerable<TSource> source, IAsyncEnumerable<bool> filter)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(filter);

		return Core(source, filter);

		static async IAsyncEnumerable<TSource> Core(
			IAsyncEnumerable<TSource> source,
			IAsyncEnumerable<bool> filter,
			[EnumeratorCancellation] CancellationToken cancellation = default
		)
		{
			await using var sIter = source.GetConfiguredAsyncEnumerator(cancellation);
			await using var fIter = filter.GetConfiguredAsyncEnumerator(cancellation);

			while (true)
			{
				var sMoved = await sIter.MoveNextAsync();
				var fMoved = await fIter.MoveNextAsync();
				if (sMoved != fMoved)
					ThrowHelper.ThrowArgumentException(nameof(filter), "'source' and 'filter' did not have equal lengths.");

				if (!sMoved)
					yield break;

				if (fIter.Current)
					yield return sIter.Current;
			}
		}
	}
}
