namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Returns a sequence of tuples where the `key` is
	/// the zero-based index of the `value` in the source
	/// sequence.
	/// </summary>
	/// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <returns>A sequence of tuples.</returns>
	/// <remarks>This operator uses deferred execution and streams its results.</remarks>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	public static IAsyncEnumerable<(int index, TSource item)> Index<TSource>(this IAsyncEnumerable<TSource> source)
	{
		return source.Index(0);
	}

	/// <summary>
	/// Returns a sequence of tuples where the `key` is
	/// the zero-based index of the `value` in the source
	/// sequence. An additional parameter specifies the
	/// starting index.
	/// </summary>
	/// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="startIndex"></param>
	/// <returns>A sequence of tuples.</returns>
	/// <remarks>This operator uses deferred execution and streams its results.</remarks>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	public static IAsyncEnumerable<(int index, TSource item)> Index<TSource>(this IAsyncEnumerable<TSource> source, int startIndex)
	{
		ArgumentNullException.ThrowIfNull(source);
		return source.Select((item, index) => (startIndex + index, item));
	}
}
