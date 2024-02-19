namespace Test;

/// <summary>
/// Verify the behavior of the Interleave operator
/// </summary>
public class InterleaveTest
{
	/// <summary>
	/// Verify that Interleave behaves in a lazy manner
	/// </summary>
	[Fact]
	public void TestInterleaveIsLazy()
	{
		_ = new BreakingSequence<int>().Interleave(new BreakingSequence<int>());
	}

	/// <summary>
	/// Verify that interleaving disposes those enumerators that it managed
	/// to open successfully
	/// </summary>
	[Fact]
	public void TestInterleaveDisposesOnErrorAtGetEnumerator()
	{
		using var sequenceA = TestingSequence.Of<int>();
		var sequenceB = new BreakingSequence<int>();

		// Expected and thrown by BreakingSequence
		_ = Assert.Throws<TestException>(() => sequenceA.Interleave(sequenceB).Consume());
	}

	/// <summary>
	/// Verify that interleaving disposes those enumerators that it managed
	/// to open successfully
	/// </summary>
	[Fact]
	public void TestInterleaveDisposesOnErrorAtMoveNext()
	{
		using var sequenceA = TestingSequence.Of<int>();
		using var sequenceB = SuperEnumerable.From<int>(() => throw new TestException()).AsTestingSequence();

		// Expected and thrown by sequenceB
		_ = Assert.Throws<TestException>(() => sequenceA.Interleave(sequenceB).Consume());
	}

	/// <summary>
	/// Verify that interleaving do not call enumerators MoveNext method eagerly
	/// </summary>
	[Fact]
	public void TestInterleaveDoNoCallMoveNextEagerly()
	{
		using var sequenceA = Enumerable.Range(1, 1).AsTestingSequence();
		var sequenceB = SuperEnumerable.From<int>(() => throw new TestException());

		sequenceA.Interleave(sequenceB).Take(1).Consume();
	}

	/// <summary>
	/// Verify that two balanced sequences will interleave all of their elements
	/// </summary>
	[Fact]
	public void TestInterleaveTwoBalancedSequences()
	{
		using var sequenceA = Enumerable.Range(1, 10).AsTestingSequence();
		using var sequenceB = Enumerable.Range(1, 10).AsTestingSequence();

		var result = sequenceA.Interleave(sequenceB);
		result.AssertSequenceEqual(Enumerable.Range(1, 10).SelectMany(x => Enumerable.Repeat(x, 2)));
	}

	/// <summary>
	/// Verify that interleaving two empty sequences results in an empty sequence
	/// </summary>
	[Fact]
	public void TestInterleaveTwoEmptySequences()
	{
		using var sequenceA = TestingSequence.Of<int>();
		using var sequenceB = TestingSequence.Of<int>();

		var result = sequenceA.Interleave(sequenceB);
		Assert.Equal([], result);
	}

	/// <summary>
	/// Verify that interleaving two unequal sequences with the Skip strategy results in
	/// the shorter sequence being omitted from the interleave operation when consumed
	/// </summary>
	[Fact]
	public void TestInterleaveTwoImbalanceStrategySkip()
	{
		using var sequenceA = TestingSequence.Of(0, 0, 0, 0, 0, 0);
		using var sequenceB = TestingSequence.Of(1, 1, 1, 1);

		var result = sequenceA.Interleave(sequenceB);
		result.AssertSequenceEqual(0, 1, 0, 1, 0, 1, 0, 1, 0, 0);
	}

	/// <summary>
	/// Verify that interleaving multiple empty sequences results in an empty sequence
	/// </summary>
	[Fact]
	public void TestInterleaveManyEmptySequences()
	{
		using var sequenceA = TestingSequence.Of<int>();
		using var sequenceB = TestingSequence.Of<int>();
		using var sequenceC = TestingSequence.Of<int>();
		using var sequenceD = TestingSequence.Of<int>();
		using var sequenceE = TestingSequence.Of<int>();

		var result = sequenceA.Interleave(sequenceB, sequenceC, sequenceD, sequenceE);
		Assert.Empty(result);
	}

	/// <summary>
	/// Verify that interleaving multiple unequal sequences with the Skip strategy
	/// results in sequences being omitted form the interleave operation when consumed
	/// </summary>
	[Fact]
	public void TestInterleaveManyImbalanceStrategySkip()
	{
		using var sequenceA = TestingSequence.Of(1, 5, 8, 11, 14, 16);
		using var sequenceB = TestingSequence.Of(2, 6, 9, 12);
		using var sequenceC = TestingSequence.Of<int>();
		using var sequenceD = TestingSequence.Of(3);
		using var sequenceE = TestingSequence.Of(4, 7, 10, 13, 15, 17);

		using var sequences = TestingSequence.Of<IEnumerable<int>>(sequenceA, sequenceB, sequenceC, sequenceD, sequenceE);
		var result = sequences.Interleave();

		result.AssertSequenceEqual(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17);
	}

	[Fact]
	public void TestInterleaveCollectionCount()
	{
		using var sequenceA = Enumerable.Range(1, 10_000).AsBreakingCollection();
		using var sequenceB = Enumerable.Range(1, 10_000).AsBreakingCollection();

		var result = sequenceA.Interleave(sequenceB);
		result.AssertCollectionErrorChecking(20_000);

		result = Seq(sequenceA, sequenceB).Interleave<int>();
		result.AssertCollectionErrorChecking(20_000);
	}
}
