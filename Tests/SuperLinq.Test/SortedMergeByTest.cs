namespace Test;

/// <summary>
/// Tests that verify the behavior of the SortedMergeBy operator.
/// </summary>
public class SortedMergeByTests
{
	/// <summary>
	/// Verify that SortedMergeBy behaves in a lazy manner.
	/// </summary>
	[Fact]
	public void TestSortedMergeByIsLazy()
	{
		var sequenceA = new BreakingSequence<int>();
		var sequenceB = new BreakingSequence<int>();

		sequenceA.SortedMergeBy(SuperEnumerable.Identity, OrderByDirection.Ascending, sequenceB);
	}

	/// <summary>
	/// Verify that SortedMergeBy disposes those enumerators that it managed
	/// to open successfully
	/// </summary>
	[Fact]
	public void TestSortedMergeByDisposesOnError()
	{
		using var sequenceA = TestingSequence.Of<int>();

		// Expected and thrown by BreakingSequence
		Assert.Throws<TestException>(() =>
			sequenceA.SortedMergeBy(SuperEnumerable.Identity, OrderByDirection.Ascending, new BreakingSequence<int>()).Consume());
	}

	/// <summary>
	/// Verify that SortedMergeBy sorts correctly with a <see langword="null"/> comparer.
	/// </summary>
	[Fact]
	public void TestSortedMergeByComparerNull()
	{
		var sequenceA = Enumerable.Range(1, 3);
		var sequenceB = Enumerable.Range(4, 3);
		var result = sequenceA.SortedMergeBy(SuperEnumerable.Identity, OrderByDirection.Ascending, (IComparer<int>?)null, sequenceB);

		Assert.Equal(sequenceA.Concat(sequenceB), result);
	}

	/// <summary>
	/// Verify that if <c>otherSequences</c> is empty, SortedMergeBy yields the contents of <c>sequence</c>
	/// </summary>
	[Fact]
	public void TestSortedMergeByOtherSequencesEmpty()
	{
		var sequenceA = Enumerable.Range(1, 10);
		var result = sequenceA.SortedMergeBy(SuperEnumerable.Identity, OrderByDirection.Ascending);

		Assert.Equal(sequenceA, result);
	}

	/// <summary>
	/// Verify that if all sequences passed to SortedMergeBy are empty, the result is an empty sequence.
	/// </summary>
	[Fact]
	public void TestSortedMergeByAllSequencesEmpty()
	{
		var sequenceA = Enumerable.Empty<int>();
		var sequenceB = Enumerable.Empty<int>();
		var sequenceC = Enumerable.Empty<int>();
		var result = sequenceA.SortedMergeBy(SuperEnumerable.Identity, OrderByDirection.Ascending, sequenceB, sequenceC);

		Assert.Equal(sequenceA, result);
	}

	/// <summary>
	/// Verify that if the primary sequence is empty, SortedMergeBy correctly merges <c>otherSequences</c>
	/// </summary>
	[Fact]
	public void TestSortedMergeByFirstSequenceEmpty()
	{
		var sequenceA = Enumerable.Empty<int>();
		var sequenceB = new[] { 1, 3, 5, 7, 9, 11 };
		var sequenceC = new[] { 2, 4, 6, 8, 10, 12 };
		var expectedResult = Enumerable.Range(1, 12);

		var result = sequenceA.SortedMergeBy(SuperEnumerable.Identity, OrderByDirection.Ascending, sequenceB, sequenceC);
		Assert.Equal(expectedResult, result);

		result = sequenceA.SortedMergeBy(SuperEnumerable.Identity, sequenceB, sequenceC);
		Assert.Equal(expectedResult, result);
	}

	/// <summary>
	/// Verify that SortedMergeBy correctly merges sequences of equal length.
	/// </summary>
	[Fact]
	public void TestSortedMergeByEqualLengthSequences()
	{
		var sequenceA = Enumerable.Range(0, 10).Select(x => x * 3 + 0);
		var sequenceB = Enumerable.Range(0, 10).Select(x => x * 3 + 1);
		var sequenceC = Enumerable.Range(0, 10).Select(x => x * 3 + 2);
		var expectedResult = Enumerable.Range(0, 10 * 3);

		var result = sequenceA.SortedMergeBy(SuperEnumerable.Identity, OrderByDirection.Ascending, sequenceB, sequenceC);
		Assert.Equal(expectedResult, result);

		result = sequenceA.SortedMergeBy(SuperEnumerable.Identity, sequenceB, sequenceC);
		Assert.Equal(expectedResult, result);
	}

	/// <summary>
	/// Verify that sorted merge correctly merges sequences of unequal length.
	/// </summary>
	[Fact]
	public void TestSortedMergeByUnequalLengthSequences()
	{
		var sequenceA = Enumerable.Range(0, 30).Select(x => x * 3 + 0);
		var sequenceB = Enumerable.Range(0, 30).Select(x => x * 3 + 1).Take(30 / 2);
		var sequenceC = Enumerable.Range(0, 30).Select(x => x * 3 + 2).Take(30 / 3);
		var expectedResult = sequenceA.Concat(sequenceB).Concat(sequenceC).OrderBy(SuperEnumerable.Identity);

		var result = sequenceA.SortedMergeBy(SuperEnumerable.Identity, OrderByDirection.Ascending, sequenceB, sequenceC);
		Assert.Equal(expectedResult, result);

		result = sequenceA.SortedMergeBy(SuperEnumerable.Identity, sequenceB, sequenceC);
		Assert.Equal(expectedResult, result);
	}

	/// <summary>
	/// Verify that sorted merge correctly merges descending-ordered sequences.
	/// </summary>
	[Fact]
	public void TestSortedMergeByDescendingOrder()
	{
		var sequenceA = Enumerable.Range(0, 10).Select(x => x * 3 + 0).Reverse();
		var sequenceB = Enumerable.Range(0, 10).Select(x => x * 3 + 1).Reverse();
		var sequenceC = Enumerable.Range(0, 10).Select(x => x * 3 + 2).Reverse();
		var expectedResult = Enumerable.Range(0, 10 * 3).Reverse();

		var result = sequenceA.SortedMergeBy(SuperEnumerable.Identity, OrderByDirection.Descending, sequenceB, sequenceC);
		Assert.Equal(expectedResult, result);

		result = sequenceA.SortedMergeByDescending(SuperEnumerable.Identity, sequenceB, sequenceC);
		Assert.Equal(expectedResult, result);
	}

	/// <summary>
	/// Verify that sorted merge correctly uses a custom comparer supplied to it.
	/// </summary>
	[Fact]
	public void TestSortedMergeByCustomComparer()
	{
		var sequenceA = new[] { "a", "D", "G", "h", "i", "J", "O", "t", "z" };
		var sequenceB = new[] { "b", "E", "k", "q", "r", "u", "V", "x", "Y" };
		var sequenceC = new[] { "C", "F", "l", "m", "N", "P", "s", "w" };
		var comparer = StringComparer.InvariantCultureIgnoreCase;
		var expectedResult = sequenceA.Concat(sequenceB).Concat(sequenceC)
									  .OrderBy(SuperEnumerable.Identity, comparer);

		var result = sequenceA.SortedMergeBy(SuperEnumerable.Identity, OrderByDirection.Ascending, comparer, sequenceB, sequenceC);
		Assert.Equal(expectedResult, result);

		result = sequenceA.SortedMergeBy(SuperEnumerable.Identity, comparer, sequenceB, sequenceC);
		Assert.Equal(expectedResult, result);
	}

	/// <summary>
	/// Verify that sorted merge disposes enumerators of all sequences that are passed to it.
	/// </summary>
	[Fact]
	public void TestSortedMergeByAllSequencesDisposed()
	{
		using var sequenceA = Enumerable.Range(1, 10).AsTestingSequence();
		using var sequenceB = Enumerable.Range(1, 10 - 1).AsTestingSequence();
		using var sequenceC = Enumerable.Range(1, 10 - 5).AsTestingSequence();
		using var sequenceD = Enumerable.Range(1, 0).AsTestingSequence();

		sequenceA.SortedMergeBy(SuperEnumerable.Identity, OrderByDirection.Ascending, sequenceB, sequenceC, sequenceD)
				 .Consume(); // ensures the sequences are actually merged and iterators are obtained
	}
}
