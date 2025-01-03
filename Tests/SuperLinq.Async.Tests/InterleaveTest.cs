﻿namespace SuperLinq.Async.Tests;

/// <summary>
/// Verify the behavior of the Interleave operator
/// </summary>
public sealed class InterleaveTests
{
	/// <summary>
	/// Verify that Interleave behaves in a lazy manner
	/// </summary>
	[Test]
	public void TestInterleaveIsLazy()
	{
		_ = new AsyncBreakingSequence<int>().Interleave(new AsyncBreakingSequence<int>());
	}

	/// <summary>
	/// Verify that interleaving disposes those enumerators that it managed
	/// to open successfully
	/// </summary>
	[Test]
	public async Task TestInterleaveDisposesOnErrorAtGetEnumerator()
	{
		await using var sequenceA = TestingSequence.Of<int>();
		var sequenceB = new AsyncBreakingSequence<int>();

		// Expected and thrown by BreakingSequence
		_ = await Assert.ThrowsAsync<TestException>(async () => await sequenceA.Interleave(sequenceB).Consume());
	}

	/// <summary>
	/// Verify that interleaving disposes those enumerators that it managed
	/// to open successfully
	/// </summary>
	[Test]
	public async Task TestInterleaveDisposesOnErrorAtMoveNext()
	{
		await using var sequenceA = TestingSequence.Of<int>();
		await using var sequenceB = AsyncSuperEnumerable.From<int>(() => throw new TestException()).AsTestingSequence();

		// Expected and thrown by sequenceB
		_ = await Assert.ThrowsAsync<TestException>(async () => await sequenceA.Interleave(sequenceB).Consume());
	}

	/// <summary>
	/// Verify that interleaving do not call enumerators MoveNext method eagerly
	/// </summary>
	[Test]
	public async Task TestInterleaveDoNoCallMoveNextEagerly()
	{
		await using var sequenceA = AsyncEnumerable.Range(1, 1).AsTestingSequence();
		await using var sequenceB = AsyncSuperEnumerable.From<int>(() => throw new NotSupportedException()).AsTestingSequence();

		await sequenceA.Interleave(sequenceB).Take(1).Consume();
	}

	/// <summary>
	/// Verify that two balanced sequences will interleave all of their elements
	/// </summary>
	[Test]
	public async Task TestInterleaveTwoBalancedSequences()
	{
		await using var sequenceA = AsyncEnumerable.Range(1, 10).AsTestingSequence();
		await using var sequenceB = AsyncEnumerable.Range(1, 10).AsTestingSequence();

		var result = sequenceA.Interleave(sequenceB);
		await result.AssertSequenceEqual(Enumerable.Range(1, 10).SelectMany(x => Enumerable.Repeat(x, 2)));
	}

	/// <summary>
	/// Verify that interleaving two empty sequences results in an empty sequence
	/// </summary>
	[Test]
	public async Task TestInterleaveTwoEmptySequences()
	{
		await using var sequenceA = TestingSequence.Of<int>();
		await using var sequenceB = TestingSequence.Of<int>();

		var result = sequenceA.Interleave(sequenceB);
		await result.AssertEmpty();
	}

	/// <summary>
	/// Verify that interleaving two unequal sequences with the Skip strategy results in
	/// the shorter sequence being omitted from the interleave operation when consumed
	/// </summary>
	[Test]
	public async Task TestInterleaveTwoImbalanceStrategySkip()
	{
		await using var sequenceA = TestingSequence.Of(0, 0, 0, 0, 0, 0);
		await using var sequenceB = TestingSequence.Of(1, 1, 1, 1);

		var result = sequenceA.Interleave(sequenceB);
		await result.AssertSequenceEqual(0, 1, 0, 1, 0, 1, 0, 1, 0, 0);
	}

	/// <summary>
	/// Verify that interleaving multiple empty sequences results in an empty sequence
	/// </summary>
	[Test]
	public async Task TestInterleaveManyEmptySequences()
	{
		await using var sequenceA = TestingSequence.Of<int>();
		await using var sequenceB = TestingSequence.Of<int>();
		await using var sequenceC = TestingSequence.Of<int>();
		await using var sequenceD = TestingSequence.Of<int>();
		await using var sequenceE = TestingSequence.Of<int>();

		var result = sequenceA.Interleave(sequenceB, sequenceC, sequenceD, sequenceE);
		await result.AssertEmpty();
	}

	/// <summary>
	/// Verify that interleaving multiple unequal sequences with the Skip strategy
	/// results in sequences being omitted form the interleave operation when consumed
	/// </summary>
	[Test]
	public async Task TestInterleaveManyImbalanceStrategySkip()
	{
		await using var sequenceA = TestingSequence.Of(1, 5, 8, 11, 14, 16);
		await using var sequenceB = TestingSequence.Of(2, 6, 9, 12);
		await using var sequenceC = TestingSequence.Of<int>();
		await using var sequenceD = TestingSequence.Of(3);
		await using var sequenceE = TestingSequence.Of(4, 7, 10, 13, 15, 17);

		var iterations = 0;
		var sequences = new[] { sequenceA, sequenceB, sequenceC, sequenceD, sequenceE }
			.Do<IAsyncEnumerable<int>>(_ => iterations++);
		var result = sequences.Interleave();

		await result.AssertSequenceEqual(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17);
		Assert.Equal(5, iterations);
	}
}
