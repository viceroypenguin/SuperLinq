namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Asserts that a source sequence contains a given count of elements.
	/// </summary>
	/// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
	/// <param name="source">Source sequence.</param>
	/// <param name="count">Count to assert.</param>
	/// <returns>
	/// Returns the original sequence as long it is contains the number of elements specified by <paramref
	/// name="count"/>. Otherwise it throws <see cref="ArgumentException" />.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than <c>0</c>.</exception>
	/// <exception cref="ArgumentException"><paramref name="source"/> has a length different than <paramref
	/// name="count"/>.</exception>
	/// <remarks>
	/// This operator uses deferred execution and streams its results.
	/// </remarks>
	public static IAsyncEnumerable<TSource> AssertCount<TSource>(this IAsyncEnumerable<TSource> source, int count)
	{
		ArgumentNullException.ThrowIfNull(source);
		Guard.IsGreaterThanOrEqualTo(count, 0);

		return Core(source, count);

		static async IAsyncEnumerable<TSource> Core(
			IAsyncEnumerable<TSource> source,
			int count,
			[EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			var c = 0;
			await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
			{
				if (++c > count)
					break;
				yield return item;
			}
			Guard.IsEqualTo(c, count, $"{nameof(source)}.Count()");
		}
	}
}
