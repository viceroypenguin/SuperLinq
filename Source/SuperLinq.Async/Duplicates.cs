namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	///	    Returns all duplicate elements of the given source.
	/// </summary>
	/// <typeparam name="TSource">
	///	    The type of the elements in the source sequence.
	/// </typeparam>
	/// <param name="source">
	///	    The source sequence.
	/// </param>
	/// <param name="comparer">
	///	    The equality comparer to use to determine whether one <typeparamref name="TSource"/> equals another. If <see
	///     langword="null"/>, the default equality comparer for <typeparamref name="TSource"/> is used.
	/// </param>
	/// <returns>
	///	    All elements that are duplicated.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	///	    This operator uses deferred execution and streams its results.
	/// </remarks>
	public static IAsyncEnumerable<TSource> Duplicates<TSource>(this IAsyncEnumerable<TSource> source, IEqualityComparer<TSource>? comparer = null)
	{
		ArgumentNullException.ThrowIfNull(source);

		comparer ??= EqualityComparer<TSource>.Default;

		return Core(source, comparer);

		static async IAsyncEnumerable<TSource> Core(
			IAsyncEnumerable<TSource> source,
			IEqualityComparer<TSource> comparer,
			[EnumeratorCancellation] CancellationToken token = default)
		{
			var counts = new Collections.NullKeyDictionary<TSource, int>(comparer);
			await foreach (var element in source.WithCancellation(token).ConfigureAwait(false))
			{
				if (!counts.TryGetValue(element, out var count))
				{
					counts[element] = 1;
				}
				else if (count == 1)
				{
					yield return element;
					counts[element] = 2;
				}
			}
		}
	}
}
