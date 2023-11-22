namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Returns every N-th element of a sequence.
	/// </summary>
	/// <typeparam name="TSource">
	///	    Type of the source sequence
	/// </typeparam>
	/// <param name="source">
	///	    Source sequence
	/// </param>
	/// <param name="step">
	///	    Number of elements to bypass before returning the next element.
	/// </param>
	/// <returns>
	///	    A sequence with every N-th element of the input sequence.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> is <see langword="null"/>.
	/// </exception>
	/// <exception cref="ArgumentOutOfRangeException">
	///	    <paramref name="step"/> is negative.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This operator uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IEnumerable<TSource> TakeEvery<TSource>(this IEnumerable<TSource> source, int step)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(step);

		return source.Where((e, i) => i % step == 0);
	}
}
