namespace SuperLinq.Async;

public partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Generates a sequence by repeating the given value infinitely.
	/// </summary>
	/// <typeparam name="TResult">Result sequence element type.</typeparam>
	/// <param name="value">Value to repeat in the resulting sequence.</param>
	/// <returns>Sequence repeating the given value infinitely.</returns>
	public static IAsyncEnumerable<TResult> Repeat<TResult>(TResult value)
	{
		throw new NotImplementedException();
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
		throw new NotImplementedException();
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
		throw new NotImplementedException();
	}
}
