using SuperLinq;

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

		sequenceA.SortedMergeBy(SuperEnumerable.Identity<int>(), OrderByDirection.Ascending, sequenceB);
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
		await Assert.ThrowsAsync<NotSupportedException>(async () =>
			await sequenceA.SortedMergeBy(SuperEnumerable.Identity<int>(), OrderByDirection.Ascending, new AsyncBreakingSequence<int>()).Consume());
	}

	/// <summary>
	/// Verify that SortedMergeBy throws an exception if invoked on a <see langword="null"/> sequence.
	/// </summary>
	[Fact]
	public async Task TestSortedMergeByComparerNull()
	{
		var sequenceA = AsyncEnumerable.Range(1, 3);
		var sequenceB = AsyncEnumerable.Range(4, 3);
		var result = sequenceA.SortedMergeBy(SuperEnumerable.Identity<int>(), OrderByDirection.Ascending, (IComparer<int>?)null, sequenceB);

		await result.AssertSequenceEqual(sequenceA.Concat(sequenceB));
	}

	/// <summary>
	/// Verify that if <c>otherSequences</c> is empty, SortedMergeBy yields the contents of <c>sequence</c>
	/// </summary>
	[Fact]
	public async Task TestSortedMergeByOtherSequencesEmpty()
	{
		var sequenceA = AsyncEnumerable.Range(1, 10);
		var result = sequenceA.SortedMergeBy(SuperEnumerable.Identity<int>(), OrderByDirection.Ascending);

		await result.AssertSequenceEqual(sequenceA);
	}

	/// <summary>
	/// Verify that if all sequences passed to SortedMergeBy are empty, the result is an empty sequence.
	/// </summary>
	[Fact]
	public async Task TestSortedMergeByAllSequencesEmpty()
	{
		var sequenceA = AsyncEnumerable.Empty<int>();
		var sequenceB = AsyncEnumerable.Empty<int>();
		var sequenceC = AsyncEnumerable.Empty<int>();
		var result = sequenceA.SortedMergeBy(SuperEnumerable.Identity<int>(), OrderByDirection.Ascending, sequenceB, sequenceC);

		await result.AssertSequenceEqual(sequenceA);
	}

	/// <summary>
	/// Verify that if the primary sequence is empty, SortedMergeBy correctly merges <c>otherSequences</c>
	/// </summary>
	[Fact]
	public async Task TestSortedMergeByFirstSequenceEmpty()
	{
		var sequenceA = AsyncSeq<int>();
		var sequenceB = AsyncSeq<int>(1, 3, 5, 7, 9, 11);
		var sequenceC = AsyncSeq<int>(2, 4, 6, 8, 10, 12);
		var expectedResult = Enumerable.Range(1, 12);

		var result = sequenceA.SortedMergeBy(SuperEnumerable.Identity<int>(), OrderByDirection.Ascending, sequenceB, sequenceC);
		await result.AssertSequenceEqual(expectedResult);

		result = sequenceA.SortedMergeBy(SuperEnumerable.Identity<int>(), sequenceB, sequenceC);
		await result.AssertSequenceEqual(expectedResult);
	}

	/// <summary>
	/// Verify that SortedMergeBy correctly merges sequences of equal length.
	/// </summary>
	[Fact]
	public async Task TestSortedMergeByEqualLengthSequences()
	{
		var sequenceA = AsyncEnumerable.Range(0, 10).Select(x => x * 3 + 0);
		var sequenceB = AsyncEnumerable.Range(0, 10).Select(x => x * 3 + 1);
		var sequenceC = AsyncEnumerable.Range(0, 10).Select(x => x * 3 + 2);
		var expectedResult = Enumerable.Range(0, 10 * 3);

		var result = sequenceA.SortedMergeBy(SuperEnumerable.Identity<int>(), OrderByDirection.Ascending, sequenceB, sequenceC);
		await result.AssertSequenceEqual(expectedResult);

		result = sequenceA.SortedMergeBy(SuperEnumerable.Identity<int>(), sequenceB, sequenceC);
		await result.AssertSequenceEqual(expectedResult);
	}

	/// <summary>
	/// Verify that sorted merge correctly merges sequences of unequal length.
	/// </summary>
	[Fact]
	public async Task TestSortedMergeByUnequalLengthSequences()
	{
		var sequenceA = AsyncEnumerable.Range(0, 30).Select(x => x * 3 + 0);
		var sequenceB = AsyncEnumerable.Range(0, 30).Select(x => x * 3 + 1).Take(30 / 2);
		var sequenceC = AsyncEnumerable.Range(0, 30).Select(x => x * 3 + 2).Take(30 / 3);
		var expectedResult = sequenceA.Concat(sequenceB).Concat(sequenceC).OrderBy(x => x);

		var result = sequenceA.SortedMergeBy(SuperEnumerable.Identity<int>(), OrderByDirection.Ascending, sequenceB, sequenceC);
		await result.AssertSequenceEqual(expectedResult);

		result = sequenceA.SortedMergeBy(SuperEnumerable.Identity<int>(), sequenceB, sequenceC);
		await result.AssertSequenceEqual(expectedResult);
	}

	/// <summary>
	/// Verify that sorted merge correctly merges descending-ordered sequences.
	/// </summary>
	[Fact]
	public async Task TestSortedMergeByDescendingOrder()
	{
		var sequenceA = AsyncEnumerable.Range(0, 10).Select(x => x * 3 + 0).Reverse();
		var sequenceB = AsyncEnumerable.Range(0, 10).Select(x => x * 3 + 1).Reverse();
		var sequenceC = AsyncEnumerable.Range(0, 10).Select(x => x * 3 + 2).Reverse();
		var expectedResult = Enumerable.Range(0, 10 * 3).Reverse();

		var result = sequenceA.SortedMergeBy(SuperEnumerable.Identity<int>(), OrderByDirection.Descending, sequenceB, sequenceC);
		await result.AssertSequenceEqual(expectedResult);

		result = sequenceA.SortedMergeByDescending(SuperEnumerable.Identity<int>(), sequenceB, sequenceC);
		await result.AssertSequenceEqual(expectedResult);
	}

	/// <summary>
	/// Verify that sorted merge correctly uses a custom comparer supplied to it.
	/// </summary>
	[Fact]
	public async Task TestSortedMergeByCustomComparer()
	{
		var sequenceA = AsyncSeq("a", "D", "G", "h", "i", "J", "O", "t", "z");
		var sequenceB = AsyncSeq("b", "E", "k", "q", "r", "u", "V", "x", "Y");
		var sequenceC = AsyncSeq("C", "F", "l", "m", "N", "P", "s", "w");
		var comparer = StringComparer.InvariantCultureIgnoreCase;
		var expectedResult = sequenceA.Concat(sequenceB).Concat(sequenceC)
									  .OrderBy(a => a, comparer);

		var result = sequenceA.SortedMergeBy(SuperEnumerable.Identity<string>(), OrderByDirection.Ascending, comparer, sequenceB, sequenceC);
		await result.AssertSequenceEqual(expectedResult);

		result = sequenceA.SortedMergeBy(SuperEnumerable.Identity<string>(), comparer, sequenceB, sequenceC);
		await result.AssertSequenceEqual(expectedResult);
	}

	/// <summary>
	/// Verify that sorted merge disposes enumerators of all sequences that are passed to it.
	/// </summary>
	[Fact]
	public async Task TestSortedMergeByAllSequencesDisposed()
	{
		await using var sequenceA = AsyncEnumerable.Range(1, 10).AsTestingSequence();
		await using var sequenceB = AsyncEnumerable.Range(1, 10 - 1).AsTestingSequence();
		await using var sequenceC = AsyncEnumerable.Range(1, 10 - 5).AsTestingSequence();
		await using var sequenceD = AsyncEnumerable.Range(1, 0).AsTestingSequence();

		await sequenceA
			.SortedMergeBy(SuperEnumerable.Identity<int>(), OrderByDirection.Ascending, sequenceB, sequenceC, sequenceD)
			.Consume(); // ensures the sequences are actually merged and iterators are obtained
	}
}
