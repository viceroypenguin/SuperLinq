namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Returns a sequence of <see cref="IList{T}"/> representing all of the subsets of any size that are part of
	///     the original sequence. In mathematics, it is equivalent to the <em>power set</em> of a set.
	/// </summary>
	/// <param name="sequence">
	///	    Sequence for which to produce subsets
	/// </param>
	/// <typeparam name="T">
	///	    The type of the elements in the sequence
	/// </typeparam>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="sequence"/> is <see langword="null"/>
	/// </exception>
	/// <returns>
	///	    A sequence of lists that represent the all subsets of the original sequence
	/// </returns>
	/// <remarks>
	/// <para>
	///	    This operator produces all of the subsets of a given sequence. Subsets are returned in increasing
	///     cardinality, starting with the empty set and terminating with the entire original sequence. Subsets are
	///     produced in a deferred, streaming manner; however, each subset is returned as a materialized list. There are
	///     2^N subsets of a given sequence, where <c>N = sequence.Count()</c>.
	/// </para>
	/// <para>
	///	    This method is implemented by using deferred execution. However, <paramref name="sequence"/> will be
	///     consumed in it's entirety immediately when first element of the returned sequence is consumed. 
	/// </para>
	/// </remarks>
	public static IEnumerable<IList<T>> Subsets<T>(this IEnumerable<T> sequence)
	{
		ArgumentNullException.ThrowIfNull(sequence);

		return Core(sequence);

		static IEnumerable<IList<T>> Core(IEnumerable<T> sequence)
		{
			var sequenceAsList = sequence.ToList();
			var sequenceLength = sequenceAsList.Count;

			yield return []; // the first subset is the empty set

			// next check also resolves the case of permuting empty sets
			if (sequenceLength is 0)
				yield break;

			// all other subsets are computed using the subset generator...

			for (var k = 1; k < sequenceLength; k++)
			{
				// each intermediate subset is a lexographically ordered K-subset
				foreach (var subset in Subsets(sequenceAsList, k))
					yield return subset;
			}

			yield return sequenceAsList; // the last subset is the original set itself
		}
	}

	/// <summary>
	///	    Returns a sequence of <see cref="IList{T}"/> representing all subsets of a given size that are part of the
	///     original sequence. In mathematics, it is equivalent to the <em>combinations</em> or <em>k-subsets</em> of a
	///     set.
	/// </summary>
	/// <param name="sequence">
	///	    Sequence for which to produce subsets
	/// </param>
	/// <param name="subsetSize">
	///	    The size of the subsets to produce
	/// </param>
	/// <typeparam name="T">
	///	    The type of the elements in the sequence
	/// </typeparam>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="sequence"/> is <see langword="null"/>
	/// </exception>
	/// <exception cref="ArgumentOutOfRangeException">
	///	    <paramref name="subsetSize"/> is less than <c>0</c>.
	/// </exception>
	/// <returns>
	///	    A sequence of lists that represents of K-sized subsets of the original sequence
	/// </returns>
	public static IEnumerable<IList<T>> Subsets<T>(this IEnumerable<T> sequence, int subsetSize)
	{
		ArgumentNullException.ThrowIfNull(sequence);
		ArgumentOutOfRangeException.ThrowIfNegative(subsetSize);

		if (sequence.TryGetCollectionCount() is int length)
			ArgumentOutOfRangeException.ThrowIfGreaterThan(subsetSize, length);

		return Core(sequence, subsetSize);

		static IEnumerable<IList<T>> Core(IEnumerable<T> sequence, int subsetSize)
		{
			if (subsetSize == 0)
			{
				yield return [];
				yield break;
			}

			foreach (var subset in Subsets(sequence.ToList(), subsetSize))
				yield return subset;
		}
	}

	/// <summary>
	///	    Produces lexographically ordered k-subsets.
	/// </summary>
	/// <remarks>
	///	    It uses a snapshot of the original sequence, and an iterative, reductive swap algorithm to produce all
	///     subsets of a predetermined size less than or equal to the original set size.
	/// </remarks>
	private static IEnumerable<IList<T>> Subsets<T>(List<T> set, int subsetSize)
	{
		ArgumentOutOfRangeException.ThrowIfGreaterThan(subsetSize, set.Count);

		var indices = new int[subsetSize];  // indices into the original set
		var prevSwapIndex = subsetSize;     // previous swap index (upper index)
		var currSwapIndex = -1;             // current swap index (lower index)
		var setSize = set.Count;

		do
		{
			if (currSwapIndex == -1)
			{
				currSwapIndex = 0;
				prevSwapIndex = subsetSize;
			}
			else
			{
				if (currSwapIndex < setSize - prevSwapIndex)
					prevSwapIndex = 0;

				prevSwapIndex++;
				currSwapIndex = indices[subsetSize - prevSwapIndex];
			}

			for (var i = 1; i <= prevSwapIndex; i++)
				indices[subsetSize + i - prevSwapIndex - 1] = currSwapIndex + i;

			var subset = new T[subsetSize];     // the current subset to return
			for (var i = 0; i < subsetSize; i++)
				subset[i] = set[indices[i] - 1];

			yield return subset;
		}
		while (indices[0] != setSize - subsetSize + 1);
	}
}
