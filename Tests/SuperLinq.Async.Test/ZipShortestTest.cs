﻿namespace Test.Async;

public class ZipShortestTest
{
	[Fact]
	public async Task BothSequencesDisposedWithUnequalLengthsAndLongerFirst()
	{
		await using var longer = TestingSequence.Of(1, 2, 3);
		await using var shorter = TestingSequence.Of(1, 2);

		await longer.ZipShortest(shorter, (x, y) => x + y).Consume();
	}

	[Fact]
	public async Task BothSequencesDisposedWithUnequalLengthsAndShorterFirst()
	{
		await using var longer = TestingSequence.Of(1, 2, 3);
		await using var shorter = TestingSequence.Of(1, 2);

		await shorter.ZipShortest(longer, (x, y) => x + y).Consume();
	}

	[Fact]
	public async Task ZipShortestWithEqualLengthSequences()
	{
		var zipped = AsyncSeq(1, 2, 3).ZipShortest(AsyncSeq(4, 5, 6), ValueTuple.Create);
		await zipped.AssertSequenceEqual((1, 4), (2, 5), (3, 6));
	}

	[Fact]
	public async Task ZipShortestWithFirstSequenceShorterThanSecond()
	{
		var zipped = AsyncSeq(1, 2).ZipShortest(AsyncSeq(4, 5, 6), ValueTuple.Create);
		await zipped.AssertSequenceEqual((1, 4), (2, 5));
	}

	[Fact]
	public async Task ZipShortestWithFirstSequnceLongerThanSecond()
	{
		var zipped = AsyncSeq(1, 2, 3).ZipShortest(AsyncSeq(4, 5), ValueTuple.Create);
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
		var seq1 = AsyncEnumerable.Range(1, shortSequence == 1 ? 2 : 3);
		var seq2 = AsyncEnumerable.Range(1, shortSequence == 2 ? 2 : 3);
		var seq3 = AsyncEnumerable.Range(1, shortSequence == 3 ? 2 : 3);
		var seq4 = AsyncEnumerable.Range(1, shortSequence == 4 ? 2 : 3);

		var seq = seq1.ZipShortest(seq2, seq3, seq4, (a, _, _, _) => a);
		await seq.AssertSequenceEqual(1, 2);
	}

	[Fact]
	public async Task MoveNextIsNotCalledUnnecessarilyWhenFirstIsShorter()
	{
		await using var s1 = TestingSequence.Of(1, 2);
		await using var s2 = AsyncSuperEnumerable
			.From(
				() => Task.FromResult(4),
				() => Task.FromResult(5),
				AsyncBreakingFunc.Of<int>())
			.AsTestingSequence();

		var zipped = s1.ZipShortest(s2, ValueTuple.Create);

		await zipped.AssertSequenceEqual((1, 4), (2, 5));
	}

	[Fact]
	public async Task ZipShortestNotIterateUnnecessaryElements()
	{
		await using var s1 = AsyncSuperEnumerable
			.From(
				() => Task.FromResult(4),
				() => Task.FromResult(5),
				() => Task.FromResult(6),
				AsyncBreakingFunc.Of<int>())
			.AsTestingSequence();
		await using var s2 = TestingSequence.Of(1, 2);

		var zipped = s1.ZipShortest(s2, ValueTuple.Create);

		await zipped.AssertSequenceEqual((4, 1), (5, 2));
	}

	[Fact]
	public async Task ZipShortestDisposesInnerSequencesCaseGetEnumeratorThrows()
	{
		await using var s1 = TestingSequence.Of(1, 2);

		await Assert.ThrowsAsync<NotSupportedException>(async () =>
			await s1.ZipShortest(new AsyncBreakingSequence<int>(), ValueTuple.Create).Consume());
	}
}
