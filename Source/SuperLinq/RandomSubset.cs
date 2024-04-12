namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Returns a sequence of a specified size of random elements from the original sequence.
	/// </summary>
	/// <typeparam name="T">
	///	    The type of source sequence elements.
	/// </typeparam>
	/// <param name="source">
	///	    The sequence from which to return random elements.
	/// </param>
	/// <param name="subsetSize">
	///	    The size of the random subset to return.
	/// </param>
	/// <returns>
	///	    A random sequence of elements in random order from the original sequence.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> is <see langword="null"/>.
	/// </exception>
	/// <exception cref="ArgumentOutOfRangeException">
	///		<paramref name="subsetSize"/> is negative or larger than the length of <paramref name="source"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This method is implemented by using deferred execution. However, <paramref name="source"/> will be consumed
	///     in it's entirety immediately when first element of the returned sequence is consumed.
	/// </para>
	/// </remarks>
	public static IEnumerable<T> RandomSubset<T>(this IEnumerable<T> source, int subsetSize)
	{
		return RandomSubset(source, subsetSize, new Random());
	}

	/// <summary>
	///	    Returns a sequence of a specified size of random elements from the original sequence.
	/// </summary>
	/// <typeparam name="T">
	///	    The type of source sequence elements.
	/// </typeparam>
	/// <param name="source">
	///	    The sequence from which to return random elements.
	/// </param>
	/// <param name="subsetSize">
	///	    The size of the random subset to return.
	/// </param>
	/// <param name="rand">
	///		A random generator used as part of the selection algorithm.
	/// </param>
	/// <returns>
	///	    A random sequence of elements in random order from the original sequence.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="rand"/> is <see langword="null"/>.
	/// </exception>
	/// <exception cref="ArgumentOutOfRangeException">
	///		<paramref name="subsetSize"/> is negative or larger than the length of <paramref name="source"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This method is implemented by using deferred execution. However, <paramref name="source"/> will be consumed
	///     in it's entirety immediately when first element of the returned sequence is consumed.
	/// </para>
	/// </remarks>
	public static IEnumerable<T> RandomSubset<T>(this IEnumerable<T> source, int subsetSize, Random rand)
	{
		ArgumentNullException.ThrowIfNull(rand);
		ArgumentNullException.ThrowIfNull(source);
		ArgumentOutOfRangeException.ThrowIfNegative(subsetSize);

		return RandomSubsetImpl(source, rand, subsetSize);
	}

	private static IEnumerable<T> RandomSubsetImpl<T>(IEnumerable<T> source, Random rand, int? subsetSize)
	{
		// The simplest and most efficient way to return a random subset is to perform
		// an in-place, partial Fisher-Yates shuffle of the sequence. While we could do
		// a full shuffle, it would be wasteful in the cases where subsetSize is shorter
		// than the length of the sequence.
		// See: http://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle

		var list = source.ToList();
		subsetSize ??= list.Count;

		if (list.Count < subsetSize)
		{
			ThrowHelper.ThrowArgumentOutOfRangeException(
				nameof(subsetSize),
				"Subset size must be less than or equal to the source length.");
		}

		var m = 0;                // keeps track of count items shuffled
		var w = list.Count;       // upper bound of shrinking swap range
		var g = w - 1;            // used to compute the second swap index

		// perform in-place, partial Fisher-Yates shuffle
		while (m < subsetSize)
		{
			var k = g - rand.Next(w);
			(list[m], list[k]) = (list[k], list[m]);
			++m;
			--w;
		}

		// yield the random subset as a new sequence
		for (var i = 0; i < subsetSize; i++)
			yield return list[i];
	}
}
