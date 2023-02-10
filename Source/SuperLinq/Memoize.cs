namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Creates a sequence that lazily caches the source as it is iterated for the first time, reusing the cache
	/// thereafter for future re-iterations. By default, all sequences are cached, whether they are instantiated or
	/// lazy.
	/// </summary>
	/// <typeparam name="TSource">
	/// Type of elements in <paramref name="source"/>.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="forceCache">Force caching of <see cref="ICollection{T}"/>s.</param>
	/// <returns>
	/// Returns a sequence that corresponds to a cached version of the input sequence.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	/// <paramref name="source"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	/// The returned <see cref="IEnumerable{T}"/> will cache items from <paramref name="source"/> in a thread-safe
	/// manner. The sequence supplied in <paramref name="source"/> is not expected to be thread-safe but it is required
	/// to be thread-agnostic. The iterator returned by <see cref="IEnumerable{T}.GetEnumerator"/> is not thread-safe,
	/// and access should be limited to a single thread/task or controlled via external locks.
	/// </para>
	/// <para>
	/// By default, <see cref="Memoize"/> will choose the safe option and cache all <see
	/// cref="IEnumerable{T}"/>s. <see cref="ICollection{T}"/> will use an optimized form using <see
	/// cref="ICollection{T}.CopyTo(T[], int)"/>, while other <see cref="IEnumerable{T}"/>s will cache iteratively as
	/// each element is requested.
	/// </para>
	/// <para>
	/// However, if <paramref name="forceCache"/> is set to <see langword="false"/>, then data in an <see
	/// cref="ICollection{T}"/> will be returned directly and not cached. In most cases, this is safe, but if the
	/// collection is modified in between uses, then different data may be returned for each iteration.
	/// </para>
	/// </remarks>
	public static IBuffer<TSource> Memoize<TSource>(this IEnumerable<TSource> source, bool forceCache = true)
	{
		throw new NotImplementedException();
	}
}
