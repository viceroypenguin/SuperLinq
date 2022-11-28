namespace Test;

public class ZipShortestTest
{
	[Fact]
	public void BothSequencesDisposedWithUnequalLengthsAndLongerFirst()
	{
		using var longer = TestingSequence.Of(1, 2, 3);
		using var shorter = TestingSequence.Of(1, 2);

		longer.ZipShortest(shorter, (x, y) => x + y).Consume();
	}

	[Fact]
	public void BothSequencesDisposedWithUnequalLengthsAndShorterFirst()
	{
		using var longer = TestingSequence.Of(1, 2, 3);
		using var shorter = TestingSequence.Of(1, 2);

		shorter.ZipShortest(longer, (x, y) => x + y).Consume();
	}

	[Fact]
	public void ZipShortestWithEqualLengthSequences()
	{
		var zipped = new[] { 1, 2, 3 }.ZipShortest(new[] { 4, 5, 6 }, ValueTuple.Create);
		Assert.NotNull(zipped);
		zipped.AssertSequenceEqual((1, 4), (2, 5), (3, 6));
	}

	[Fact]
	public void ZipShortestWithFirstSequenceShorterThanSecond()
	{
		var zipped = new[] { 1, 2 }.ZipShortest(new[] { 4, 5, 6 }, ValueTuple.Create);
		Assert.NotNull(zipped);
		zipped.AssertSequenceEqual((1, 4), (2, 5));
	}

	[Fact]
	public void ZipShortestWithFirstSequnceLongerThanSecond()
	{
		var zipped = new[] { 1, 2, 3 }.ZipShortest(new[] { 4, 5 }, ValueTuple.Create);
		Assert.NotNull(zipped);
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
		var seq1 = Enumerable.Range(1, shortSequence == 1 ? 2 : 3);
		var seq2 = Enumerable.Range(1, shortSequence == 2 ? 2 : 3);
		var seq3 = Enumerable.Range(1, shortSequence == 3 ? 2 : 3);
		var seq4 = Enumerable.Range(1, shortSequence == 4 ? 2 : 3);

		var seq = seq1.ZipShortest(seq2, seq3, seq4, (a, _, _, _) => a);
		seq.AssertSequenceEqual(1, 2);
	}

	[Fact]
	public void MoveNextIsNotCalledUnnecessarilyWhenFirstIsShorter()
	{
		using var s1 = TestingSequence.Of(1, 2);
		using var s2 = SuperEnumerable.From(() => 4,
										   () => 5,
										   () => throw new TestException())
									 .AsTestingSequence();

		var zipped = s1.ZipShortest(s2, ValueTuple.Create);

		Assert.NotNull(zipped);
		zipped.AssertSequenceEqual((1, 4), (2, 5));
	}

	[Fact]
	public void ZipShortestNotIterateUnnecessaryElements()
	{
		using var s1 = SuperEnumerable.From(() => 4,
										   () => 5,
										   () => 6,
										   () => throw new TestException())
									 .AsTestingSequence();
		using var s2 = TestingSequence.Of(1, 2);

		var zipped = s1.ZipShortest(s2, ValueTuple.Create);

		Assert.NotNull(zipped);
		zipped.AssertSequenceEqual((4, 1), (5, 2));
	}

	[Fact]
	public void ZipShortestDisposesInnerSequencesCaseGetEnumeratorThrows()
	{
		using var s1 = TestingSequence.Of(1, 2);

		Assert.Throws<InvalidOperationException>(() =>
			s1.ZipShortest(new BreakingSequence<int>(), ValueTuple.Create).Consume());
	}
}
