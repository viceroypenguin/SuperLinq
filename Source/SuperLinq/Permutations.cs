namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Generates a sequence of lists that represent the permutations of the original sequence.
	/// </summary>
	/// <typeparam name="T">
	///	    The type of the elements in the sequence
	/// </typeparam>
	/// <param name="sequence">
	///	    The original sequence to permute
	/// </param>
	/// <returns>
	///	    A sequence of lists representing permutations of the original sequence
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="sequence"/> is <see langword="null"/>.
	/// </exception>
	/// <exception cref="ArgumentException">
	///	    <paramref name="sequence"/> has too many elements to permute properly.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    A permutation is a unique re-ordering of the elements of the sequence.
	/// </para>
	/// <para>
	///	    This method is implemented by using deferred execution. However, <paramref name="sequence"/> will be
	///     consumed in it's entirety immediately when first element of the returned sequence is consumed. 
	/// </para>
	/// <para>
	///	    Each permutation is materialized into a new list. There are N! permutations of a sequence, where N is the
	///     number of elements in <paramref name="sequence"/>.
	/// </para>
	/// <para>
	///	    Note that the original sequence is considered one of the permutations and will be returned as one of the
	///     results.
	/// </para>
	/// </remarks>
	public static IEnumerable<IList<T>> Permutations<T>(this IEnumerable<T> sequence)
	{
		ArgumentNullException.ThrowIfNull(sequence);

		if (sequence.TryGetCollectionCount() is int count
			&& count > 21)
		{
			ThrowHelper.ThrowArgumentException(nameof(sequence), "Input set is too large to permute properly.");
		}

		return Core(sequence);

		static IEnumerable<IList<T>> Core(IEnumerable<T> sequence)
		{
			var valueSet = sequence.ToList();
			if (valueSet.Count <= 1)
			{
				yield return valueSet;
				yield break;
			}

			if (valueSet.Count > 21)
				ThrowHelper.ThrowArgumentException(nameof(sequence), "Input set is too large to permute properly.");

			var permutation = Enumerable.Range(0, valueSet.Count).ToArray();
			var permutationCount =
				Enumerable.Range(2, Math.Max(0, valueSet.Count - 1))
					.Aggregate(1ul, (acc, x) => acc * (uint)x);

			yield return PermuteValueSet(permutation, valueSet);
			for (var i = 1ul; i < permutationCount; i++)
			{
				NextPermutation(permutation);
				yield return PermuteValueSet(permutation, valueSet);
			}
		}

		static T[] PermuteValueSet(int[] permutation, List<T> valueSet)
		{
			var permutedSet = new T[permutation.Length];
			for (var i = 0; i < permutation.Length; i++)
				permutedSet[i] = valueSet[permutation[i]];
			return permutedSet;
		}

		// NOTE: The algorithm used to generate permutations uses the fact that any set
		//       can be put into 1-to-1 correspondence with the set of ordinals number (0..n).
		//       The implementation here is based on the algorithm described by Kenneth H. Rosen,
		//       in Discrete Mathematics and Its Applications, 2nd edition, pp. 282-284.
		static void NextPermutation(int[] permutation)
		{
			// find the largest index j with m_Permutation[j] < m_Permutation[j+1]
			var j = permutation.Length - 2;
			while (permutation[j] > permutation[j + 1])
				j--;

			// find index k such that m_Permutation[k] is the smallest integer
			// greater than m_Permutation[j] to the right of m_Permutation[j]
			var k = permutation.Length - 1;
			while (permutation[j] > permutation[k])
				k--;

			// interchange m_Permutation[j] and m_Permutation[k]
			(permutation[j], permutation[k]) = (permutation[k], permutation[j]);

			// move the tail of the permutation after the jth position in increasing order
			var x = permutation.Length - 1;
			var y = j + 1;

			while (x > y)
			{
				(permutation[x], permutation[y]) = (permutation[y], permutation[x]);
				x--;
				y++;
			}
		}
	}
}
