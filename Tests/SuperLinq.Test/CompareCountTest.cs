namespace Test;

public class CompareCountTest
{
	public static IEnumerable<object[]> CompareCountData { get; } =
		from e in new[]
		{
				new { Count1 = 0, Count2 = 0, Comparison =  0 },
				new { Count1 = 0, Count2 = 1, Comparison = -1 },
				new { Count1 = 1, Count2 = 0, Comparison =  1 },
				new { Count1 = 1, Count2 = 1, Comparison =  0 },
		}
		from s in GetTestSequenceKinds(
					  Enumerable.Range(1, e.Count1),
					  Enumerable.Range(1, e.Count2),
					  (xs, ys) => new { First = xs, Second = ys })
		select new object[] { s.First.Data, s.Second.Data, e.Comparison, };

	[Theory, MemberData(nameof(CompareCountData))]
	public void CompareCount(IEnumerable<int> xs, IEnumerable<int> ys, int expected)
	{
		Assert.Equal(expected, xs.CompareCount(ys));
	}

	[Theory]
	[InlineData(0, 0, 0, 1)]
	[InlineData(0, 1, -1, 1)]
	[InlineData(1, 0, 1, 1)]
	[InlineData(1, 1, 0, 2)]
	public void CompareCountWithCollectionAndSequence(
		int collectionCount,
		int sequenceCount,
		int expectedCompareCount,
		int expectedMoveNextCallCount)
	{
		var collection = new BreakingCollection<int>(collectionCount);

		using var seq = Enumerable.Range(0, sequenceCount).AsTestingSequence();

		Assert.Equal(expectedCompareCount, collection.CompareCount(seq));
		Assert.Equal(expectedMoveNextCallCount, seq.MoveNextCallCount);
	}

	[Theory]
	[InlineData(0, 0, 0, 1)]
	[InlineData(0, 1, -1, 1)]
	[InlineData(1, 0, 1, 1)]
	[InlineData(1, 1, 0, 2)]
	public void CompareCountWithSequenceAndCollection(
		int sequenceCount,
		int collectionCount,
		int expectedCompareCount,
		int expectedMoveNextCallCount)
	{
		var collection = new BreakingCollection<int>(collectionCount);

		using var seq = Enumerable.Range(0, sequenceCount).AsTestingSequence();

		Assert.Equal(expectedCompareCount, seq.CompareCount(collection));
		Assert.Equal(expectedMoveNextCallCount, seq.MoveNextCallCount);
	}

	[Theory]
	[InlineData(0, 0, 0, 1)]
	[InlineData(0, 1, -1, 1)]
	[InlineData(1, 0, 1, 1)]
	[InlineData(1, 1, 0, 2)]
	public void CompareCountWithSequenceAndSequence(
		int sequenceCount1,
		int sequenceCount2,
		int expectedCompareCount,
		int expectedMoveNextCallCount)
	{
		using var seq1 = Enumerable.Range(0, sequenceCount1).AsTestingSequence();
		using var seq2 = Enumerable.Range(0, sequenceCount2).AsTestingSequence();

		Assert.Equal(expectedCompareCount, seq1.CompareCount(seq2));
		Assert.Equal(expectedMoveNextCallCount, seq1.MoveNextCallCount);
		Assert.Equal(expectedMoveNextCallCount, seq2.MoveNextCallCount);
	}

	[Fact]
	public void CompareCountDisposesSequenceEnumerators()
	{
		using var seq1 = TestingSequence.Of<int>();
		using var seq2 = TestingSequence.Of<int>();

		Assert.Equal(0, seq1.CompareCount(seq2));
	}

	[Fact]
	public void CompareCountDisposesFirstEnumerator()
	{
		var collection = new BreakingCollection<int>(0);

		using var seq = TestingSequence.Of<int>();

		Assert.Equal(0, seq.CompareCount(collection));
	}

	[Fact]
	public void CompareCountDisposesSecondEnumerator()
	{
		var collection = new BreakingCollection<int>(0);

		using var seq = TestingSequence.Of<int>();

		Assert.Equal(0, collection.CompareCount(seq));
	}

	[Fact]
	public void CompareCountDoesNotIterateUnnecessaryElements()
	{
		using var seq1 = SuperEnumerable
			.From(
				() => 1,
				() => 2,
				() => 3,
				() => 4,
				() => throw new TestException())
			.AsTestingSequence();

		using var seq2 = Enumerable.Range(1, 3).AsTestingSequence();

		Assert.Equal(1, seq1.CompareCount(seq2));
	}

	private static IEnumerable<TResult> GetTestSequenceKinds<T, TResult>(
		IEnumerable<T> s1, IEnumerable<T> s2,
		Func<
			(IEnumerable<T?> Data, SourceKind Kind),
			(IEnumerable<T?> Data, SourceKind Kind),
			TResult> selector)
	{
		// Test that the operator is optimized for collections

		var s1Seq = (s1.Select(x => x), SourceKind.Sequence);
		var s2Seq = (s2.Select(x => x), SourceKind.Sequence);

		var s1Col = (s1.ToSourceKind(SourceKind.BreakingCollection), SourceKind.BreakingCollection);
		var s2Col = (s2.ToSourceKind(SourceKind.BreakingCollection), SourceKind.BreakingCollection);

		// sequences
		yield return selector(s1Seq, s2Seq);

		// sequences and collections
		yield return selector(s1Seq, s2Col);
		yield return selector(s1Col, s2Seq);
		yield return selector(s1Col, s2Col);
	}
}
