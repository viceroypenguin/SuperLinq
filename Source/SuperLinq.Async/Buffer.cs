namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Generates a sequence of non-overlapping adjacent buffers over the source sequence.
	/// </summary>
	/// <typeparam name="TSource">Source sequence element type.</typeparam>
	/// <param name="source">Source sequence.</param>
	/// <param name="count">Number of elements for allocated buffers.</param>
	/// <returns>Sequence of buffers containing source sequence elements.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than or equal to
	/// <c>0</c>.</exception>
	/// <remarks>
	/// <para>
	/// A chunk can contain fewer elements than <paramref name="count"/>, specifically the final buffer of <paramref
	/// name="source"/>.
	/// </para>
	/// <para>
	/// This method is a synonym for <see cref="Batch{TSource}(IAsyncEnumerable{TSource}, int)"/>.
	/// </para>
	/// <para>
	/// Returned subsequences are buffered, but the overall operation is streamed.<br/>
	/// </para>
	/// </remarks>
	public static IAsyncEnumerable<IList<TSource>> Buffer<TSource>(this IAsyncEnumerable<TSource> source, int count)
	{
		return Batch(source, count);
	}
}
