namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Returns a sequence of elements in random order from the original sequence.
	/// </summary>
	/// <typeparam name="T">
	///		The type of source sequence elements.
	/// </typeparam>
	/// <param name="source">
	///	    The sequence from which to return random elements.
	/// </param>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> is <see langword="null"/>
	/// </exception>
	/// <returns>
	///	    A sequence of elements <paramref name="source"/> randomized in their order.
	/// </returns>
	/// <remarks>
	/// <para>
	///	    This method is implemented by using deferred execution. However, <paramref name="source"/> will be consumed
	///     in it's entirety immediately when first element of the returned sequence is consumed.
	/// </para>
	/// </remarks>
#if NET10_0_OR_GREATER
	[Obsolete("This method has been implemented by the framework.")]
	public static IEnumerable<T> Shuffle<T>(IEnumerable<T> source)
#else
	public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
#endif
	{
		return Shuffle(source, new Random());
	}

	/// <summary>
	///	    Returns a sequence of elements in random order from the original sequence. An additional parameter specifies
	///     a random generator to be used for the random selection algorithm.
	/// </summary>
	/// <typeparam name="T">
	///	    The type of source sequence elements.
	/// </typeparam>
	/// <param name="source">
	///	    The sequence from which to return random elements.
	/// </param>
	/// <param name="rand">
	///	    A random generator used as part of the selection algorithm.
	/// </param>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="rand"/> is <see langword="null"/>
	/// </exception>
	/// <returns>
	///	    A sequence of elements <paramref name="source"/> randomized in their order.
	/// </returns>
	/// <remarks>
	/// <para>
	///	    This method is implemented by using deferred execution. However, <paramref name="source"/> will be consumed
	///     in it's entirety immediately when first element of the returned sequence is consumed.
	/// </para>
	/// </remarks>
#if NET10_0_OR_GREATER
	[Obsolete("This method has been implemented by the framework.")]
	public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random rand)
#else
	public static IEnumerable<T> Shuffle<T>(IEnumerable<T> source, Random rand)
#endif
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(rand);

		return RandomSubsetImpl(source, rand, subsetSize: null);
	}
}
