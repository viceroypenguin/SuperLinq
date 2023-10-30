namespace SuperLinq.Async;

public partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Generates a sequence by repeating the given value infinitely.
	/// </summary>
	/// <typeparam name="TResult">Result sequence element type.</typeparam>
	/// <param name="value">Value to repeat in the resulting sequence.</param>
	/// <returns>Sequence repeating the given value infinitely.</returns>
	public static async IAsyncEnumerable<TResult> Repeat<TResult>(TResult value)
	{
		await default(ValueTask);
		while (true)
			yield return value;
	}

	/// <summary>
	/// Repeats and concatenates the source sequence infinitely.
	/// </summary>
	/// <typeparam name="TSource">Source sequence element type.</typeparam>
	/// <param name="source">Source sequence.</param>
	/// <returns>Sequence obtained by concatenating the source sequence to itself infinitely.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	public static IAsyncEnumerable<TSource> Repeat<TSource>(this IAsyncEnumerable<TSource> source)
	{
		ArgumentNullException.ThrowIfNull(source);

		return Core(source);

		static async IAsyncEnumerable<TSource> Core(
			IAsyncEnumerable<TSource> source,
			[EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			await using var buffer = source.Memoize();
			while (true)
			{
				await foreach (var el in buffer
						.WithCancellation(cancellationToken)
						.ConfigureAwait(false))
				{
					yield return el;
				}
			}
		}
	}

	/// <summary>
	/// Repeats and concatenates the source sequence the given number of times.
	/// </summary>
	/// <typeparam name="TSource">Source sequence element type.</typeparam>
	/// <param name="source">Source sequence.</param>
	/// <param name="count">Number of times to repeat the source sequence.</param>
	/// <returns>Sequence obtained by concatenating the source sequence to itself the specified number of
	/// times.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than or equal to
	/// <c>0</c>.</exception>
	public static IAsyncEnumerable<TSource> Repeat<TSource>(this IAsyncEnumerable<TSource> source, int count)
	{
		ArgumentNullException.ThrowIfNull(source);
		Guard.IsGreaterThan(count, 0);

		return Core(source, count);

		static async IAsyncEnumerable<TSource> Core(
			IAsyncEnumerable<TSource> source,
			int count,
			[EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			await using var buffer = source.Memoize();
			while (count-- > 0)
			{
				await foreach (var el in buffer
						.WithCancellation(cancellationToken)
						.ConfigureAwait(false))
				{
					yield return el;
				}
			}
		}
	}
}
