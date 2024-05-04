namespace Test;

/// <summary>
/// Tests that verify the behavior of the SortedMergeBy operator.
/// </summary>
public sealed class SortedMergeByTests
{
	/// <summary>
	/// Verify that SortedMergeBy behaves in a lazy manner.
	/// </summary>
	[Fact]
	public void TestSortedMergeByIsLazy()
	{
		var sequenceA = new BreakingSequence<int>();
		var sequenceB = new BreakingSequence<int>();

		_ = sequenceA.SortedMergeBy(SuperEnumerable.Identity, OrderByDirection.Ascending, sequenceB);
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
		_ = Assert.Throws<TestException>(() =>
			sequenceA.SortedMergeBy(SuperEnumerable.Identity, OrderByDirection.Ascending, new BreakingSequence<int>()).Consume());
	}

	/// <summary>
	/// Verify that SortedMergeBy sorts correctly with a <see langword="null"/> comparer.
	/// </summary>
	[Fact]
	public void TestSortedMergeByComparerNull()
	{
		using var sequenceA = Enumerable.Range(1, 3).AsTestingSequence();
		using var sequenceB = Enumerable.Range(4, 3).AsTestingSequence();

		var result = sequenceA.SortedMergeBy(SuperEnumerable.Identity, OrderByDirection.Ascending, (IComparer<int>?)null, sequenceB);
		result.AssertSequenceEqual(Enumerable.Range(1, 6));
	}

	/// <summary>
	/// Verify that if <c>otherSequences</c> is empty, SortedMergeBy yields the contents of <c>sequence</c>
	/// </summary>
	[Fact]
	public void TestSortedMergeByOtherSequencesEmpty()
	{
		using var sequenceA = Enumerable.Range(1, 10).AsTestingSequence();

		var result = sequenceA.SortedMergeBy(SuperEnumerable.Identity, OrderByDirection.Ascending);
		result.AssertSequenceEqual(Enumerable.Range(1, 10));
	}

	/// <summary>
	/// Verify that if all sequences passed to SortedMergeBy are empty, the result is an empty sequence.
	/// </summary>
	[Fact]
	public void TestSortedMergeByAllSequencesEmpty()
	{
		using var sequenceA = TestingSequence.Of<int>();
		using var sequenceB = TestingSequence.Of<int>();
		using var sequenceC = TestingSequence.Of<int>();

		var result = sequenceA.SortedMergeBy(SuperEnumerable.Identity, OrderByDirection.Ascending, sequenceB, sequenceC);
		result.AssertSequenceEqual();
	}

	/// <summary>
	/// Verify that if the primary sequence is empty, SortedMergeBy correctly merges <c>otherSequences</c>
	/// </summary>
	[Fact]
	public void TestSortedMergeByFirstSequenceEmpty()
	{
		using var sequenceA = TestingSequence.Of<int>();
		using var sequenceB = TestingSequence.Of(1, 3, 5, 7, 9, 11);
		using var sequenceC = TestingSequence.Of(2, 4, 6, 8, 10, 12);

		var result = sequenceA.SortedMergeBy(SuperEnumerable.Identity, sequenceB, sequenceC);
		result.AssertSequenceEqual(Enumerable.Range(1, 12));
	}

	/// <summary>
	/// Verify that SortedMergeBy correctly merges sequences of equal length.
	/// </summary>
	[Fact]
	public void TestSortedMergeByEqualLengthSequences()
	{
		using var sequenceA = Enumerable.Range(0, 10).Select(x => (x * 3) + 0).AsTestingSequence();
		using var sequenceB = Enumerable.Range(0, 10).Select(x => (x * 3) + 1).AsTestingSequence();
		using var sequenceC = Enumerable.Range(0, 10).Select(x => (x * 3) + 2).AsTestingSequence();

		var result = sequenceA.SortedMergeBy(SuperEnumerable.Identity, sequenceB, sequenceC);
		result.AssertSequenceEqual(Enumerable.Range(0, 10 * 3));
	}

	/// <summary>
	/// Verify that sorted merge correctly merges sequences of unequal length.
	/// </summary>
	[Fact]
	public void TestSortedMergeByUnequalLengthSequences()
	{
		using var sequenceA = Enumerable.Range(0, 30).Select(x => (x * 3) + 0).AsTestingSequence(maxEnumerations: 2);
		using var sequenceB = Enumerable.Range(0, 30).Select(x => (x * 3) + 1).Take(30 / 2).AsTestingSequence(maxEnumerations: 2);
		using var sequenceC = Enumerable.Range(0, 30).Select(x => (x * 3) + 2).Take(30 / 3).AsTestingSequence(maxEnumerations: 2);

		var expectedResult = sequenceA.Concat(sequenceB).Concat(sequenceC).OrderBy(SuperEnumerable.Identity);
		var result = sequenceA.SortedMergeBy(SuperEnumerable.Identity, sequenceB, sequenceC);
		result.AssertSequenceEqual(expectedResult);
	}

	/// <summary>
	/// Verify that sorted merge correctly uses a custom comparer supplied to it.
	/// </summary>
	[Fact]
	public void TestSortedMergeByCustomComparer()
	{
		using var sequenceA = new[] { "a", "D", "G", "h", "i", "J", "O", "t", "z" }.AsTestingSequence(maxEnumerations: 2);
		using var sequenceB = new[] { "b", "E", "k", "q", "r", "u", "V", "x", "Y" }.AsTestingSequence(maxEnumerations: 2);
		using var sequenceC = new[] { "C", "F", "l", "m", "N", "P", "s", "w" }.AsTestingSequence(maxEnumerations: 2);

		var comparer = StringComparer.InvariantCultureIgnoreCase;
		var result = sequenceA.SortedMergeBy(SuperEnumerable.Identity, comparer, sequenceB, sequenceC);
		var expectedResult = sequenceA
			.Concat(sequenceB).Concat(sequenceC)
			.OrderBy(SuperEnumerable.Identity, comparer);

		result.AssertSequenceEqual(expectedResult);
	}

	/// <summary>
	/// Verify that SortedMergeBy correctly merges sequences with overlapping contents.
	/// </summary>
	[Fact]
	public void TestSortedMergeByOverlappingSequences()
	{
		using var sequenceA = TestingSequence.Of(1, 3, 5, 7, 9, 11);
		using var sequenceB = TestingSequence.Of(1, 4, 5, 10, 12);
		using var sequenceC = TestingSequence.Of(2, 4, 6, 8, 10, 12);

		var result = sequenceA.SortedMergeBy(SuperEnumerable.Identity, sequenceB, sequenceC);
		result.AssertSequenceEqual([1, 1, 2, 3, 4, 4, 5, 5, 6, 7, 8, 9, 10, 10, 11, 12, 12]);
	}
}
