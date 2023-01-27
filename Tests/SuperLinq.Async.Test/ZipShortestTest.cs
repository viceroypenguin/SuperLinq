namespace Test.Async;

public class ZipShortestTest
{
	[Fact]
	public async Task ZipShortestWithEqualLengthSequences()
	{
		await using var seq1 = TestingSequence.Of(1, 2, 3);
		await using var seq2 = TestingSequence.Of(4, 5, 6);

		var zipped = seq1.ZipShortest(seq2, ValueTuple.Create);
		await zipped.AssertSequenceEqual((1, 4), (2, 5), (3, 6));
	}

	[Fact]
	public async Task ZipShortestWithFirstSequenceShorterThanSecond()
	{
		await using var seq1 = TestingSequence.Of(1, 2);
		await using var seq2 = TestingSequence.Of(4, 5, 6);

		var zipped = seq1.ZipShortest(seq2, ValueTuple.Create);
		await zipped.AssertSequenceEqual((1, 4), (2, 5));
	}

	[Fact]
	public async Task ZipShortestWithFirstSequnceLongerThanSecond()
	{
		await using var seq1 = TestingSequence.Of(1, 2, 3);
		await using var seq2 = TestingSequence.Of(4, 5);

		var zipped = seq1.ZipShortest(seq2, ValueTuple.Create);
		await zipped.AssertSequenceEqual((1, 4), (2, 5));
	}

	[Fact]
	public void ZipShortestIsLazy()
	{
		var bs = new AsyncBreakingSequence<int>();
		bs.ZipShortest(bs, BreakingFunc.Of<int, int, int>());
	}

	[Theory]
	[InlineData(1), InlineData(2), InlineData(3), InlineData(4)]
	public async Task ZipShortestEndsAtShortestSequence(int shortSequence)
	{
		await using var seq1 = Enumerable.Range(1, shortSequence == 1 ? 2 : 3).AsTestingSequence();
		await using var seq2 = Enumerable.Range(1, shortSequence == 2 ? 2 : 3).AsTestingSequence();
		await using var seq3 = Enumerable.Range(1, shortSequence == 3 ? 2 : 3).AsTestingSequence();
		await using var seq4 = Enumerable.Range(1, shortSequence == 4 ? 2 : 3).AsTestingSequence();

		var seq = seq1.ZipShortest(seq2, seq3, seq4, (a, _, _, _) => a);
		await seq.AssertSequenceEqual(1, 2);
	}

	[Fact]
	public async Task MoveNextIsNotCalledUnnecessarilyWhenFirstIsShorter()
	{
		await using var s1 = TestingSequence.Of(1, 2);
		await using var s2 = AsyncSeqExceptionAt(3).AsTestingSequence();

		var zipped = s1.ZipShortest(s2, ValueTuple.Create);
		await zipped.AssertSequenceEqual((1, 1), (2, 2));
	}

	[Fact]
	public async Task ZipShortestNotIterateUnnecessaryElements()
	{
		await using (var s1 = AsyncSeqExceptionAt(3).AsTestingSequence())
		await using (var s2 = TestingSequence.Of(1, 2))
		{
			var zipped = s1.ZipShortest(s2, ValueTuple.Create);
			await zipped.AssertSequenceEqual((1, 1), (2, 2));
		}

		await using (var s1 = TestingSequence.Of(1, 2, 3))
		await using (var s2 = TestingSequence.Of(1, 2))
		await using (var s3 = AsyncSeqExceptionAt(3).AsTestingSequence())
		{
			var zipped = s1.ZipShortest(s2, s3, ValueTuple.Create);
			await zipped.AssertSequenceEqual((1, 1, 1), (2, 2, 2));
		}

		await using (var s1 = TestingSequence.Of(1, 2, 3))
		await using (var s2 = TestingSequence.Of(1, 2, 3))
		await using (var s3 = TestingSequence.Of(1, 2))
		await using (var s4 = AsyncSeqExceptionAt(3).AsTestingSequence())
		{
			var zipped = s1.ZipShortest(s2, s3, s4, ValueTuple.Create);
			await zipped.AssertSequenceEqual((1, 1, 1, 1), (2, 2, 2, 2));
		}
	}
}
