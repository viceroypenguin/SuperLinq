namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Returns a sequence of elements in random order from the original
	/// sequence.
	/// </summary>
	/// <typeparam name="T">The type of source sequence elements.</typeparam>
	/// <param name="source">
	/// The sequence from which to return random elements.</param>
	/// <returns>
	/// A sequence of elements <paramref name="source"/> randomized in
	/// their order.
	/// </returns>
	/// <remarks>
	/// This method uses deferred execution and streams its results. The
	/// source sequence is entirely buffered before the results are
	/// streamed.
	/// </remarks>

	public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
	{
		return Shuffle(source, new Random());
	}

	/// <summary>
	/// Returns a sequence of elements in random order from the original
	/// sequence. An additional parameter specifies a random generator to be
	/// used for the random selection algorithm.
	/// </summary>
	/// <typeparam name="T">The type of source sequence elements.</typeparam>
	/// <param name="source">
	/// The sequence from which to return random elements.</param>
	/// <param name="rand">
	/// A random generator used as part of the selection algorithm.</param>
	/// <returns>
	/// A sequence of elements <paramref name="source"/> randomized in
	/// their order.
	/// </returns>
	/// <remarks>
	/// This method uses deferred execution and streams its results. The
	/// source sequence is entirely buffered before the results are
	/// streamed.
	/// </remarks>

	public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random rand)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(rand);

		return RandomSubsetImpl(source, rand, seq =>
		{
			var array = seq.ToArray();
			return (array, array.Length);
		});
	}
}
