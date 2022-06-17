namespace Test;

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
		new BreakingSequence<int>().Interleave(new BreakingSequence<int>());
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
		Assert.Throws<InvalidOperationException>(() => sequenceA.Interleave(sequenceB).Consume());
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
		Assert.Throws<TestException>(() => sequenceA.Interleave(sequenceB).Consume());
	}

	/// <summary>
	/// Verify that interleaving do not call enumerable GetEnumerator method eagerly
	/// </summary>
	[Fact]
	public void TestInterleaveDoNotCallGetEnumeratorEagerly()
	{
		var sequenceA = TestingSequence.Of(1);
		var sequenceB = new BreakingSequence<int>();

		sequenceA.Interleave(sequenceB).Take(1).Consume();
	}

	/// <summary>
	/// Verify that interleaving do not call enumerators MoveNext method eagerly
	/// </summary>
	[Fact]
	public void TestInterleaveDoNoCallMoveNextEagerly()
	{
		var sequenceA = Enumerable.Range(1, 1);
		var sequenceB = SuperEnumerable.From<int>(() => throw new TestException());

		sequenceA.Interleave(sequenceB).Take(1).Consume();
	}

	/// <summary>
	/// Verify that two balanced sequences will interleave all of their elements
	/// </summary>
	[Fact]
	public void TestInterleaveTwoBalancedSequences()
	{
		var sequenceA = Enumerable.Range(1, 10);
		var sequenceB = Enumerable.Range(1, 10);
		var result = sequenceA.Interleave(sequenceB);

		Assert.Equal(Enumerable.Range(1, 10).Select(x => new[] { x, x }).SelectMany(z => z), result);
	}

	/// <summary>
	/// Verify that interleaving two empty sequences results in an empty sequence
	/// </summary>
	[Fact]
	public void TestInterleaveTwoEmptySequences()
	{
		var sequenceA = Enumerable.Empty<int>();
		var sequenceB = Enumerable.Empty<int>();
		var result = sequenceA.Interleave(sequenceB);

		Assert.Equal(Enumerable.Empty<int>(), result);
	}

	/// <summary>
	/// Verify that interleaving two unequal sequences with the Skip strategy results in
	/// the shorter sequence being omitted from the interleave operation when consumed
	/// </summary>
	[Fact]
	public void TestInterleaveTwoImbalanceStrategySkip()
	{
		var sequenceA = new[] { 0, 0, 0, 0, 0, 0 };
		var sequenceB = new[] { 1, 1, 1, 1 };
		var result = sequenceA.Interleave(sequenceB);

		var expectedResult = new[] { 0, 1, 0, 1, 0, 1, 0, 1, 0, 0 };

		Assert.Equal(expectedResult, result);
	}

	/// <summary>
	/// Verify that interleaving multiple empty sequences results in an empty sequence
	/// </summary>
	[Fact]
	public void TestInterleaveManyEmptySequences()
	{
		var sequenceA = Enumerable.Empty<int>();
		var sequenceB = Enumerable.Empty<int>();
		var sequenceC = Enumerable.Empty<int>();
		var sequenceD = Enumerable.Empty<int>();
		var sequenceE = Enumerable.Empty<int>();
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
		var sequenceA = new[] { 1, 5, 8, 11, 14, 16, };
		var sequenceB = new[] { 2, 6, 9, 12, };
		var sequenceC = Array.Empty<int>();
		var sequenceD = new[] { 3 };
		var sequenceE = new[] { 4, 7, 10, 13, 15, 17, };
		var result = sequenceA.Interleave(sequenceB, sequenceC, sequenceD, sequenceE);

		var expectedResult = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 };

		Assert.Equal(expectedResult, result);
	}

	/// <summary>
	/// Verify that Interleave disposes of all iterators it creates regardless of which strategy
	/// is used to interleave the sequences
	/// </summary>
	[Fact]
	public void TestInterleaveDisposesAllIterators()
	{

		using var sequenceA = Enumerable.Range(1, 10).AsTestingSequence();
		using var sequenceB = Enumerable.Range(1, 10 - 1).AsTestingSequence();
		using var sequenceC = Enumerable.Range(1, 10 - 5).AsTestingSequence();
		using var sequenceD = Enumerable.Range(1, 0).AsTestingSequence();

		sequenceA.Interleave(sequenceB, sequenceC, sequenceD)
				 .Consume();
	}
}
