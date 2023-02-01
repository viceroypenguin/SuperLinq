namespace Test.Async;

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
		var sequenceA = new AsyncBreakingSequence<int>();
		var sequenceB = new AsyncBreakingSequence<int>();

		sequenceA.SortedMergeBy(SuperEnumerable.Identity, OrderByDirection.Ascending, sequenceB);
	}

	/// <summary>
	/// Verify that SortedMergeBy disposes those enumerators that it managed
	/// to open successfully
	/// </summary>
	[Fact]
	public async Task TestSortedMergeByDisposesOnError()
	{
		await using var sequenceA = TestingSequence.Of<int>();

		// Expected and thrown by BreakingSequence
		await Assert.ThrowsAsync<TestException>(async () =>
			await sequenceA.SortedMergeBy(SuperEnumerable.Identity, OrderByDirection.Ascending, new AsyncBreakingSequence<int>()).Consume());
	}

	/// <summary>
	/// Verify that SortedMergeBy sorts correctly with a <see langword="null"/> comparer.
	/// </summary>
	[Fact]
	public async Task TestSortedMergeByComparerNull()
	{
		await using var sequenceA = Enumerable.Range(1, 3).AsTestingSequence();
		await using var sequenceB = Enumerable.Range(4, 3).AsTestingSequence();

		var result = sequenceA.SortedMergeBy(SuperEnumerable.Identity, OrderByDirection.Ascending, (IComparer<int>?)null, sequenceB);
		await result.AssertSequenceEqual(Enumerable.Range(1, 6));
	}

	/// <summary>
	/// Verify that if <c>otherSequences</c> is empty, SortedMergeBy yields the contents of <c>sequence</c>
	/// </summary>
	[Fact]
	public async Task TestSortedMergeByOtherSequencesEmpty()
	{
		await using var sequenceA = Enumerable.Range(1, 10).AsTestingSequence();

		var result = sequenceA.SortedMergeBy(SuperEnumerable.Identity, OrderByDirection.Ascending);
		await result.AssertSequenceEqual(Enumerable.Range(1, 10));
	}

	/// <summary>
	/// Verify that if all sequences passed to SortedMergeBy are empty, the result is an empty sequence.
	/// </summary>
	[Fact]
	public async Task TestSortedMergeByAllSequencesEmpty()
	{
		await using var sequenceA = TestingSequence.Of<int>();
		await using var sequenceB = TestingSequence.Of<int>();
		await using var sequenceC = TestingSequence.Of<int>();

		var result = sequenceA.SortedMergeBy(SuperEnumerable.Identity, OrderByDirection.Ascending, sequenceB, sequenceC);
		await result.AssertSequenceEqual();
	}

	/// <summary>
	/// Verify that if the primary sequence is empty, SortedMergeBy correctly merges <c>otherSequences</c>
	/// </summary>
	[Fact]
	public async Task TestSortedMergeByFirstSequenceEmpty()
	{
		await using var sequenceA = TestingSequence.Of<int>();
		await using var sequenceB = TestingSequence.Of(1, 3, 5, 7, 9, 11);
		await using var sequenceC = TestingSequence.Of(2, 4, 6, 8, 10, 12);

		var result = sequenceA.SortedMergeBy(SuperEnumerable.Identity, sequenceB, sequenceC);
		await result.AssertSequenceEqual(Enumerable.Range(1, 12));
	}

	/// <summary>
	/// Verify that SortedMergeBy correctly merges sequences of equal length.
	/// </summary>
	[Fact]
	public async Task TestSortedMergeByEqualLengthSequences()
	{
		await using var sequenceA = Enumerable.Range(0, 10).Select(x => x * 3 + 0).AsTestingSequence();
		await using var sequenceB = Enumerable.Range(0, 10).Select(x => x * 3 + 1).AsTestingSequence();
		await using var sequenceC = Enumerable.Range(0, 10).Select(x => x * 3 + 2).AsTestingSequence();

		var result = sequenceA.SortedMergeBy(SuperEnumerable.Identity, sequenceB, sequenceC);
		await result.AssertSequenceEqual(Enumerable.Range(0, 10 * 3));
	}

	/// <summary>
	/// Verify that sorted merge correctly merges sequences of unequal length.
	/// </summary>
	[Fact]
	public async Task TestSortedMergeByUnequalLengthSequences()
	{
		await using var sequenceA = Enumerable.Range(0, 30).Select(x => x * 3 + 0).AsTestingSequence(maxEnumerations: 2);
		await using var sequenceB = Enumerable.Range(0, 30).Select(x => x * 3 + 1).Take(30 / 2).AsTestingSequence(maxEnumerations: 2);
		await using var sequenceC = Enumerable.Range(0, 30).Select(x => x * 3 + 2).Take(30 / 3).AsTestingSequence(maxEnumerations: 2);

		var expectedResult = sequenceA.Concat(sequenceB).Concat(sequenceC).OrderBy(SuperEnumerable.Identity);
		var result = sequenceA.SortedMergeBy(SuperEnumerable.Identity, sequenceB, sequenceC);
		await result.AssertSequenceEqual(expectedResult);
	}

	/// <summary>
	/// Verify that sorted merge correctly merges descending-ordered sequences.
	/// </summary>
	[Fact]
	public async Task TestSortedMergeByDescendingOrder()
	{
		await using var sequenceA = Enumerable.Range(0, 10).Select(x => x * 3 + 0).Reverse().AsTestingSequence();
		await using var sequenceB = Enumerable.Range(0, 10).Select(x => x * 3 + 1).Reverse().AsTestingSequence();
		await using var sequenceC = Enumerable.Range(0, 10).Select(x => x * 3 + 2).Reverse().AsTestingSequence();

		var result = sequenceA.SortedMergeByDescending(SuperEnumerable.Identity, sequenceB, sequenceC);
		await result.AssertSequenceEqual(Enumerable.Range(0, 10 * 3).Reverse());
	}

	/// <summary>
	/// Verify that sorted merge correctly uses a custom comparer supplied to it.
	/// </summary>
	[Fact]
	public async Task TestSortedMergeByCustomComparer()
	{
		await using var sequenceA = new[] { "a", "D", "G", "h", "i", "J", "O", "t", "z" }.AsTestingSequence(maxEnumerations: 2);
		await using var sequenceB = new[] { "b", "E", "k", "q", "r", "u", "V", "x", "Y" }.AsTestingSequence(maxEnumerations: 2);
		await using var sequenceC = new[] { "C", "F", "l", "m", "N", "P", "s", "w" }.AsTestingSequence(maxEnumerations: 2);

		var comparer = StringComparer.InvariantCultureIgnoreCase;
		var result = sequenceA.SortedMergeBy(SuperEnumerable.Identity, comparer, sequenceB, sequenceC);
		var expectedResult = sequenceA
			.Concat(sequenceB).Concat(sequenceC)
			.OrderBy(SuperEnumerable.Identity, comparer);

		await result.AssertSequenceEqual(expectedResult);
	}
}
