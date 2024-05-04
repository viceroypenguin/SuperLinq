namespace Test.Async;

/// <summary>
/// Tests that verify the behavior of the SortedMerge operator.
/// </summary>
public sealed class SortedMergeTests
{
	/// <summary>
	/// Verify that SortedMerge behaves in a lazy manner.
	/// </summary>
	[Fact]
	public void TestSortedMergeIsLazy()
	{
		var sequenceA = new AsyncBreakingSequence<int>();
		var sequenceB = new AsyncBreakingSequence<int>();

		_ = sequenceA.SortedMerge(OrderByDirection.Ascending, sequenceB);
	}

	/// <summary>
	/// Verify that SortedMerge disposes those enumerators that it managed
	/// to open successfully
	/// </summary>
	[Fact]
	public async Task TestSortedMergeDisposesOnError()
	{
		await using var sequenceA = TestingSequence.Of<int>();

		// Expected and thrown by BreakingSequence
		_ = await Assert.ThrowsAsync<TestException>(async () =>
			await sequenceA.SortedMerge(OrderByDirection.Ascending, new AsyncBreakingSequence<int>()).Consume());
	}

	/// <summary>
	/// Verify that SortedMerge sorts correctly with a <see langword="null"/> comparer.
	/// </summary>
	[Fact]
	public async Task TestSortedMergeComparerNull()
	{
		await using var sequenceA = Enumerable.Range(1, 3).AsTestingSequence();
		await using var sequenceB = Enumerable.Range(4, 3).AsTestingSequence();

		var result = sequenceA.SortedMerge(OrderByDirection.Ascending, (IComparer<int>?)null, sequenceB);
		await result.AssertSequenceEqual(Enumerable.Range(1, 6));
	}

	/// <summary>
	/// Verify that if <c>otherSequences</c> is empty, SortedMerge yields the contents of <c>sequence</c>
	/// </summary>
	[Fact]
	public async Task TestSortedMergeOtherSequencesEmpty()
	{
		await using var sequenceA = Enumerable.Range(1, 10).AsTestingSequence();

		var result = sequenceA.SortedMerge(OrderByDirection.Ascending);
		await result.AssertSequenceEqual(Enumerable.Range(1, 10));
	}

	/// <summary>
	/// Verify that if all sequences passed to SortedMerge are empty, the result is an empty sequence.
	/// </summary>
	[Fact]
	public async Task TestSortedMergeAllSequencesEmpty()
	{
		await using var sequenceA = TestingSequence.Of<int>();
		await using var sequenceB = TestingSequence.Of<int>();
		await using var sequenceC = TestingSequence.Of<int>();

		var result = sequenceA.SortedMerge(OrderByDirection.Ascending, sequenceB, sequenceC);
		await result.AssertSequenceEqual();
	}

	/// <summary>
	/// Verify that if the primary sequence is empty, SortedMerge correctly merges <c>otherSequences</c>
	/// </summary>
	[Fact]
	public async Task TestSortedMergeFirstSequenceEmpty()
	{
		await using var sequenceA = TestingSequence.Of<int>();
		await using var sequenceB = TestingSequence.Of(1, 3, 5, 7, 9, 11);
		await using var sequenceC = TestingSequence.Of(2, 4, 6, 8, 10, 12);

		var result = sequenceA.SortedMerge(sequenceB, sequenceC);
		await result.AssertSequenceEqual(Enumerable.Range(1, 12));
	}

	/// <summary>
	/// Verify that SortedMerge correctly merges sequences of equal length.
	/// </summary>
	[Fact]
	public async Task TestSortedMergeEqualLengthSequences()
	{
		await using var sequenceA = Enumerable.Range(0, 10).Select(x => (x * 3) + 0).AsTestingSequence();
		await using var sequenceB = Enumerable.Range(0, 10).Select(x => (x * 3) + 1).AsTestingSequence();
		await using var sequenceC = Enumerable.Range(0, 10).Select(x => (x * 3) + 2).AsTestingSequence();

		var result = sequenceA.SortedMerge(sequenceB, sequenceC);
		await result.AssertSequenceEqual(Enumerable.Range(0, 10 * 3));
	}

	/// <summary>
	/// Verify that sorted merge correctly merges sequences of unequal length.
	/// </summary>
	[Fact]
	public async Task TestSortedMergeUnequalLengthSequences()
	{
		await using var sequenceA = Enumerable.Range(0, 30).Select(x => (x * 3) + 0).AsTestingSequence(maxEnumerations: 2);
		await using var sequenceB = Enumerable.Range(0, 30).Select(x => (x * 3) + 1).Take(30 / 2).AsTestingSequence(maxEnumerations: 2);
		await using var sequenceC = Enumerable.Range(0, 30).Select(x => (x * 3) + 2).Take(30 / 3).AsTestingSequence(maxEnumerations: 2);

		var expectedResult = sequenceA.Concat(sequenceB).Concat(sequenceC).OrderBy(SuperEnumerable.Identity);
		var result = sequenceA.SortedMerge(sequenceB, sequenceC);
		await result.AssertSequenceEqual(expectedResult);
	}

	/// <summary>
	/// Verify that sorted merge correctly uses a custom comparer supplied to it.
	/// </summary>
	[Fact]
	public async Task TestSortedMergeCustomComparer()
	{
		await using var sequenceA = new[] { "a", "D", "G", "h", "i", "J", "O", "t", "z" }.AsTestingSequence(maxEnumerations: 2);
		await using var sequenceB = new[] { "b", "E", "k", "q", "r", "u", "V", "x", "Y" }.AsTestingSequence(maxEnumerations: 2);
		await using var sequenceC = new[] { "C", "F", "l", "m", "N", "P", "s", "w" }.AsTestingSequence(maxEnumerations: 2);

		var comparer = StringComparer.InvariantCultureIgnoreCase;
		var result = sequenceA.SortedMerge(comparer, sequenceB, sequenceC);
		var expectedResult = sequenceA
			.Concat(sequenceB).Concat(sequenceC)
			.OrderBy(SuperEnumerable.Identity, comparer);

		await result.AssertSequenceEqual(expectedResult);
	}

	/// <summary>
	/// Verify that SortedMerge correctly merges sequences with overlapping contents.
	/// </summary>
	[Fact]
	public async Task TestSortedMergeOverlappingSequences()
	{
		await using var sequenceA = TestingSequence.Of(1, 3, 5, 7, 9, 11);
		await using var sequenceB = TestingSequence.Of(1, 4, 5, 10, 12);
		await using var sequenceC = TestingSequence.Of(2, 4, 6, 8, 10, 12);

		var result = sequenceA.SortedMerge(sequenceB, sequenceC);
		await result.AssertSequenceEqual([1, 1, 2, 3, 4, 4, 5, 5, 6, 7, 8, 9, 10, 10, 11, 12, 12]);
	}
}
