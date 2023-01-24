namespace Test;

public class ZipShortestTest
{
	[Fact]
	public void ZipShortestWithEqualLengthSequences()
	{
		using var seq1 = TestingSequence.Of(1, 2, 3);
		using var seq2 = TestingSequence.Of(4, 5, 6);

		var zipped = seq1.ZipShortest(seq2, ValueTuple.Create);
		zipped.AssertSequenceEqual((1, 4), (2, 5), (3, 6));
	}

	[Fact]
	public void ZipShortestWithFirstSequenceShorterThanSecond()
	{
		using var seq1 = TestingSequence.Of(1, 2);
		using var seq2 = TestingSequence.Of(4, 5, 6);

		var zipped = seq1.ZipShortest(seq2, ValueTuple.Create);
		zipped.AssertSequenceEqual((1, 4), (2, 5));
	}

	[Fact]
	public void ZipShortestWithFirstSequnceLongerThanSecond()
	{
		using var seq1 = TestingSequence.Of(1, 2, 3);
		using var seq2 = TestingSequence.Of(4, 5);

		var zipped = seq1.ZipShortest(seq2, ValueTuple.Create);
		zipped.AssertSequenceEqual((1, 4), (2, 5));
	}

	[Fact]
	public void ZipShortestIsLazy()
	{
		var bs = new BreakingSequence<int>();
		bs.ZipShortest(bs, BreakingFunc.Of<int, int, int>());
	}

	[Theory]
	[InlineData(1), InlineData(2), InlineData(3), InlineData(4)]
	public void ZipShortestEndsAtShortestSequence(int shortSequence)
	{
		using var seq1 = Enumerable.Range(1, shortSequence == 1 ? 2 : 3).AsTestingSequence();
		using var seq2 = Enumerable.Range(1, shortSequence == 2 ? 2 : 3).AsTestingSequence();
		using var seq3 = Enumerable.Range(1, shortSequence == 3 ? 2 : 3).AsTestingSequence();
		using var seq4 = Enumerable.Range(1, shortSequence == 4 ? 2 : 3).AsTestingSequence();

		var seq = seq1.ZipShortest(seq2, seq3, seq4, (a, _, _, _) => a);
		seq.AssertSequenceEqual(1, 2);
	}

	[Fact]
	public void MoveNextIsNotCalledUnnecessarilyWhenFirstIsShorter()
	{
		using var s1 = TestingSequence.Of(1, 2);
		using var s2 = SeqExceptionAt(3).AsTestingSequence();

		var zipped = s1.ZipShortest(s2, ValueTuple.Create);

		zipped.AssertSequenceEqual((1, 1), (2, 2));
	}

	[Fact]
	public void ZipShortestNotIterateUnnecessaryElements()
	{
		using var s1 = SeqExceptionAt(3).AsTestingSequence();
		using var s2 = TestingSequence.Of(1, 2);

		var zipped = s1.ZipShortest(s2, ValueTuple.Create);
		zipped.AssertSequenceEqual((1, 1), (2, 2));
	}

	[Fact]
	public void ZipShortestDisposesInnerSequencesCaseGetEnumeratorThrows()
	{
		using var s1 = TestingSequence.Of(1, 2);

		Assert.Throws<TestException>(() =>
			s1.ZipShortest(new BreakingSequence<int>(), ValueTuple.Create).Consume());
	}
}
