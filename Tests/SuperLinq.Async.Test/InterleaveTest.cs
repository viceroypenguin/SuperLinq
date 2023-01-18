namespace Test.Async;

/// <summary>
/// Verify the behavior of the Interleave operator
/// </summary>
public class InterleaveTests
{
	/// <summary>
	/// Verify that Interleave behaves in a lazy manner
	/// </summary>
	[Fact]
	public void TestInterleaveIsLazy()
	{
		new AsyncBreakingSequence<int>().Interleave(new AsyncBreakingSequence<int>());
	}

	/// <summary>
	/// Verify that Interleave fails when encountering a null source
	/// </summary>
	[Fact]
	public void TestInterleaveTestsSourcesForNull()
	{
		Assert.Throws<ArgumentNullException>("sources", () =>
			new[] { new AsyncBreakingSequence<int>(), default!, }.Interleave<int>());
	}

	/// <summary>
	/// Verify that interleaving disposes those enumerators that it managed
	/// to open successfully
	/// </summary>
	[Fact]
	public async Task TestInterleaveDisposesOnErrorAtGetEnumerator()
	{
		await using var sequenceA = TestingSequence.Of<int>();
		var sequenceB = new AsyncBreakingSequence<int>();

		// Expected and thrown by BreakingSequence
		await Assert.ThrowsAsync<TestException>(async () => await sequenceA.Interleave(sequenceB).Consume());
	}

	/// <summary>
	/// Verify that interleaving disposes those enumerators that it managed
	/// to open successfully
	/// </summary>
	[Fact]
	public async Task TestInterleaveDisposesOnErrorAtMoveNext()
	{
		await using var sequenceA = TestingSequence.Of<int>();
		await using var sequenceB = AsyncSuperEnumerable.From<int>(() => throw new TestException()).AsTestingSequence();

		// Expected and thrown by sequenceB
		await Assert.ThrowsAsync<TestException>(async () => await sequenceA.Interleave(sequenceB).Consume());
	}

	/// <summary>
	/// Verify that interleaving do not call enumerators MoveNext method eagerly
	/// </summary>
	[Fact]
	public async Task TestInterleaveDoNoCallMoveNextEagerly()
	{
		var sequenceA = AsyncEnumerable.Range(1, 1);
		var sequenceB = AsyncSuperEnumerable.From<int>(() => throw new NotSupportedException());

		await sequenceA.Interleave(sequenceB).Take(1).Consume();
	}

	/// <summary>
	/// Verify that two balanced sequences will interleave all of their elements
	/// </summary>
	[Fact]
	public Task TestInterleaveTwoBalancedSequences()
	{
		var sequenceA = AsyncEnumerable.Range(1, 10);
		var sequenceB = AsyncEnumerable.Range(1, 10);
		var result = sequenceA.Interleave(sequenceB);

		return result.AssertSequenceEqual(Enumerable.Range(1, 10).Select(x => new[] { x, x }).SelectMany(z => z));
	}

	/// <summary>
	/// Verify that interleaving two empty sequences results in an empty sequence
	/// </summary>
	[Fact]
	public Task TestInterleaveTwoEmptySequences()
	{
		var sequenceA = AsyncEnumerable.Empty<int>();
		var sequenceB = AsyncEnumerable.Empty<int>();
		var result = sequenceA.Interleave(sequenceB);

		return result.AssertSequenceEqual(Enumerable.Empty<int>());
	}

	/// <summary>
	/// Verify that interleaving two unequal sequences with the Skip strategy results in
	/// the shorter sequence being omitted from the interleave operation when consumed
	/// </summary>
	[Fact]
	public Task TestInterleaveTwoImbalanceStrategySkip()
	{
		var sequenceA = AsyncSeq(0, 0, 0, 0, 0, 0);
		var sequenceB = AsyncSeq(1, 1, 1, 1);
		var result = sequenceA.Interleave(sequenceB);

		var expectedResult = new[] { 0, 1, 0, 1, 0, 1, 0, 1, 0, 0 };

		return result.AssertSequenceEqual(expectedResult);
	}

	/// <summary>
	/// Verify that interleaving multiple empty sequences results in an empty sequence
	/// </summary>
	[Fact]
	public Task TestInterleaveManyEmptySequences()
	{
		var sequenceA = AsyncEnumerable.Empty<int>();
		var sequenceB = AsyncEnumerable.Empty<int>();
		var sequenceC = AsyncEnumerable.Empty<int>();
		var sequenceD = AsyncEnumerable.Empty<int>();
		var sequenceE = AsyncEnumerable.Empty<int>();
		var result = sequenceA.Interleave(sequenceB, sequenceC, sequenceD, sequenceE);

		return result.AssertEmpty();
	}

	/// <summary>
	/// Verify that interleaving multiple unequal sequences with the Skip strategy
	/// results in sequences being omitted form the interleave operation when consumed
	/// </summary>
	[Fact]
	public Task TestInterleaveManyImbalanceStrategySkip()
	{
		var sequenceA = AsyncSeq<int>(1, 5, 8, 11, 14, 16);
		var sequenceB = AsyncSeq<int>(2, 6, 9, 12);
		var sequenceC = AsyncSeq<int>();
		var sequenceD = AsyncSeq<int>(3);
		var sequenceE = AsyncSeq<int>(4, 7, 10, 13, 15, 17);
		var result = sequenceA.Interleave(sequenceB, sequenceC, sequenceD, sequenceE);

		var expectedResult = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 };

		return result.AssertSequenceEqual(expectedResult);
	}

	/// <summary>
	/// Verify that Interleave disposes of all iterators it creates regardless of which strategy
	/// is used to interleave the sequences
	/// </summary>
	[Fact]
	public async Task TestInterleaveDisposesAllIterators()
	{
		await using var sequenceA = Enumerable.Range(1, 10).AsTestingSequence();
		await using var sequenceB = Enumerable.Range(1, 10 - 1).AsTestingSequence();
		await using var sequenceC = Enumerable.Range(1, 10 - 5).AsTestingSequence();
		await using var sequenceD = Enumerable.Range(1, 0).AsTestingSequence();

		await sequenceA.Interleave(sequenceB, sequenceC, sequenceD)
			.Consume();
	}
}
