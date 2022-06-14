using System.Collections;

namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Returns a sequence of <see cref="IList{T}"/> representing all of
	/// the subsets of any size that are part of the original sequence. In
	/// mathematics, it is equivalent to the <em>power set</em> of a set.
	/// </summary>
	/// <remarks>
	/// This operator produces all of the subsets of a given sequence. Subsets are returned
	/// in increasing cardinality, starting with the empty set and terminating with the
	/// entire original sequence.<br/>
	/// Subsets are produced in a deferred, streaming manner; however, each subset is returned
	/// as a materialized list.<br/>
	/// There are 2^N subsets of a given sequence, where N => sequence.Count().
	/// </remarks>
	/// <param name="sequence">Sequence for which to produce subsets</param>
	/// <typeparam name="T">The type of the elements in the sequence</typeparam>
	/// <returns>A sequence of lists that represent the all subsets of the original sequence</returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="sequence"/> is <see langword="null"/></exception>

	public static IEnumerable<IList<T>> Subsets<T>(this IEnumerable<T> sequence)
	{
		if (sequence == null) throw new ArgumentNullException(nameof(sequence));

		return _(sequence);

		static IEnumerable<IList<T>> _(IEnumerable<T> sequence)
		{
			var sequenceAsList = sequence.ToList();
			var sequenceLength = sequenceAsList.Count;

			// the first subset is the empty set
			yield return new List<T>();

			// all other subsets are computed using the subset generator
			// this check also resolves the case of permuting empty sets
			if (sequenceLength > 0)
			{
				for (var i = 1; i < sequenceLength; i++)
				{
					// each intermediate subset is a lexographically ordered K-subset
					var subsetGenerator = new SubsetGenerator<T>(sequenceAsList, i);
					foreach (var subset in subsetGenerator)
						yield return subset;
				}

				yield return sequenceAsList; // the last subet is the original set itself
			}
		}
	}

	/// <summary>
	/// Returns a sequence of <see cref="IList{T}"/> representing all
	/// subsets of a given size that are part of the original sequence. In
	/// mathematics, it is equivalent to the <em>combinations</em> or
	/// <em>k-subsets</em> of a set.
	/// </summary>
	/// <param name="sequence">Sequence for which to produce subsets</param>
	/// <param name="subsetSize">The size of the subsets to produce</param>
	/// <typeparam name="T">The type of the elements in the sequence</typeparam>
	/// <returns>A sequence of lists that represents of K-sized subsets of the original sequence</returns>
	/// <exception cref="ArgumentNullException">
	/// Thrown if <paramref name="sequence"/> is <see langword="null"/>
	/// </exception>
	/// <exception cref="ArgumentOutOfRangeException">
	/// Thrown if <paramref name="subsetSize"/> is less than zero.
	/// </exception>

	public static IEnumerable<IList<T>> Subsets<T>(this IEnumerable<T> sequence, int subsetSize)
	{
		if (sequence == null)
			throw new ArgumentNullException(nameof(sequence));
		if (subsetSize < 0)
			throw new ArgumentOutOfRangeException(nameof(subsetSize), "Subset size must be >= 0");

		// NOTE: Theres an interesting trade-off that we have to make in this operator.
		// Ideally, we would throw an exception here if the {subsetSize} parameter is
		// greater than the sequence length. Unforunately, determining the length of a
		// sequence is not always possible without enumerating it. Herein lies the rub.
		// We want Subsets() to be a deferred operation that only iterates the sequence
		// when the caller is ready to consume the results. However, this forces us to
		// defer the precondition check on the {subsetSize} upper bound. This can result
		// in an exception that is far removed from the point of failure - an unfortunate
		// and undesirable outcome.
		// At the moment, this operator prioritizes deferred execution over fail-fast
		// preconditions. This however, needs to be carefully considered - and perhaps
		// may change after further thought and review.

		return new SubsetGenerator<T>(sequence, subsetSize);
	}

	/// <summary>
	/// This class is responsible for producing the lexographically ordered k-subsets
	/// </summary>

	sealed class SubsetGenerator<T> : IEnumerable<IList<T>>
	{
		/// <summary>
		/// SubsetEnumerator uses a snapshot of the original sequence, and an
		/// iterative, reductive swap algorithm to produce all subsets of a
		/// predetermined size less than or equal to the original set size.
		/// </summary>

		class SubsetEnumerator : IEnumerator<IList<T>>
		{
			private readonly IList<T> _set;   // the original set of elements
			private readonly T[] _subset;     // the current subset to return
			private readonly int[] _indices;  // indices into the original set
			private readonly int _subsetSize; // size of the subset being produced

			private bool _continue;  // termination indicator, set when all subsets have been produced

			private int _prevIndex;  // previous swap index (upper index)
			private int _curIndex;   // current swap index (lower index)
			private int _seqSize;    // size of the original set (sequence)

			public SubsetEnumerator(IList<T> set, int subsetSize)
			{
				// precondition: subsetSize <= set.Count
				if (subsetSize > set.Count)
					throw new ArgumentOutOfRangeException(nameof(subsetSize), "Subset size must be <= sequence.Count()");

				// initialize set arrays...
				_set = set;
				_subsetSize = subsetSize;
				_subset = new T[subsetSize];
				_indices = new int[subsetSize];
				// initialize index counters...
				Reset();
			}

			public void Reset()
			{
				_prevIndex = _subset.Length;
				_curIndex = -1;
				_seqSize = _set.Count;
				_continue = _subset.Length > 0;
			}

			public IList<T> Current => (IList<T>)_subset.Clone();

			object IEnumerator.Current => Current;

			public bool MoveNext()
			{
				if (!_continue)
					return false;

				if (_curIndex == -1)
				{
					_curIndex = 0;
					_prevIndex = _subsetSize;
				}
				else
				{
					if (_curIndex < _seqSize - _prevIndex)
					{
						_prevIndex = 0;
					}
					_prevIndex++;
					_curIndex = _indices[_subsetSize - _prevIndex];
				}

				for (var j = 1; j <= _prevIndex; j++)
					_indices[_subsetSize + j - _prevIndex - 1] = _curIndex + j;

				ExtractSubset();

				_continue = _indices[0] != _seqSize - _subsetSize + 1;
				return true;
			}

			void IDisposable.Dispose() { }

			void ExtractSubset()
			{
				for (var i = 0; i < _subsetSize; i++)
					_subset[i] = _set[_indices[i] - 1];
			}
		}

		private readonly IEnumerable<T> _sequence;
		private readonly int _subsetSize;

		public SubsetGenerator(IEnumerable<T> sequence, int subsetSize)
		{
			_sequence = sequence ?? throw new ArgumentNullException(nameof(sequence));

			if (subsetSize < 0)
				throw new ArgumentOutOfRangeException(nameof(subsetSize), "{subsetSize} must be between 0 and set.Count()");
			_subsetSize = subsetSize;
		}

		/// <summary>
		/// Returns an enumerator that produces all of the k-sized
		/// subsets of the initial value set. The enumerator returns
		/// and <see cref="IList{T}"/> for each subset.
		/// </summary>
		/// <returns>an <see cref="IEnumerator"/> that enumerates all k-sized subsets</returns>

		public IEnumerator<IList<T>> GetEnumerator() =>
			new SubsetEnumerator(_sequence.ToList(), _subsetSize);

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
