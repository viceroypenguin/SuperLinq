namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Creates a sequence that retries enumerating the source sequence as long as an error occurs.
	/// </summary>
	/// <typeparam name="TSource">
	///	    Source sequence element type.
	/// </typeparam>
	/// <param name="source">
	///	    Source sequence.
	/// </param>
	/// <returns>
	///	    Sequence concatenating the results of the source sequence as long as an error occurs.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    <paramref name="source"/> will be enumerated and values streamed until it either completes or encounters an
	///     error. If an error is thrown, then <paramref name="source"/> will be re-enumerated from the beginning. This
	///     will happen until an iteration of <paramref name="source"/> has completed without errors.
	/// </para>
	/// <para>
	///	    This method uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IEnumerable<TSource> Retry<TSource>(this IEnumerable<TSource> source)
	{
		ArgumentNullException.ThrowIfNull(source);

		return Repeat<IEnumerable<TSource>>(source).Catch();
	}

	/// <summary>
	///	    Creates a sequence that retries enumerating the source sequence as long as an error occurs, with the
	///     specified maximum number of retries.
	/// </summary>
	/// <typeparam name="TSource">
	///	    Source sequence element type.
	/// </typeparam>
	/// <param name="source">
	///	    Source sequence.
	/// </param>
	/// <param name="count">
	///	    Maximum number of retries.
	/// </param>
	/// <returns>
	///	    Sequence concatenating the results of the source sequence as long as an error occurs.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> is <see langword="null"/>.
	/// </exception>
	/// <exception cref="ArgumentOutOfRangeException">
	///	    <paramref name="count"/> is less than or equal to <c>0</c>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    <paramref name="source"/> will be enumerated and values streamed until it either completes or encounters an
	///     error. If an error is thrown, then <paramref name="source"/> will be re-enumerated from the beginning. This
	///     will happen until an iteration of <paramref name="source"/> has completed without errors, or <paramref
	///     name="source"/> has been enumerated <paramref name="count"/> times. If an error is thrown during the final
	///     iteration, it will not be caught and will be thrown to the consumer.
	/// </para>
	/// <para>
	///	    This method uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IEnumerable<TSource> Retry<TSource>(this IEnumerable<TSource> source, int count)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(count);

		return Enumerable.Repeat(source, count).Catch();
	}
}
