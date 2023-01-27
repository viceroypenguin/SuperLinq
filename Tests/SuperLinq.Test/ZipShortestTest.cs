namespace Test;

public class ZipShortestTest
{
	[Fact]
	public void ZipShortestIsLazy()
	{
		var bs = new BreakingSequence<int>();
		bs.ZipShortest(bs, BreakingFunc.Of<int, int, int>());
		bs.ZipShortest(bs, bs, BreakingFunc.Of<int, int, int, int>());
		bs.ZipShortest(bs, bs, bs, BreakingFunc.Of<int, int, int, int, int>());
	}

	[Fact]
	public void TwoParamsDisposesInnerSequencesCaseGetEnumeratorThrows()
	{
		using var s1 = TestingSequence.Of(1, 2);

		Assert.Throws<TestException>(() =>
			s1.ZipShortest(new BreakingSequence<int>()).Consume());
	}

	[Theory]
	[InlineData(1), InlineData(2)]
	public void TwoParamsWorksProperly(int offset)
	{
		var o1 = (offset + 0) % 2 + 2;
		var o2 = (offset + 1) % 2 + 2;

		using var ts1 = Enumerable.Range(1, o1).AsTestingSequence();
		using var ts2 = Enumerable.Range(1, o2).AsTestingSequence();

		ts1.ZipShortest(ts2).AssertSequenceEqual(
			Enumerable.Range(1, 2)
				.Select(x => (x, x)));
	}

	[Fact]
	public void ThreeParamsDisposesInnerSequencesCaseGetEnumeratorThrows()
	{
		using var s1 = TestingSequence.Of(1, 2);
		using var s2 = TestingSequence.Of(1, 2);

		Assert.Throws<TestException>(() =>
			s1.ZipShortest(s2, new BreakingSequence<int>()).Consume());
	}

	[Theory]
	[InlineData(1), InlineData(2), InlineData(3)]
	public void ThreeParamsWorksProperly(int offset)
	{
		var o1 = (offset + 0) % 3 + 2;
		var o2 = (offset + 1) % 3 + 2;
		var o3 = (offset + 2) % 3 + 2;

		using var ts1 = Enumerable.Range(1, o1).AsTestingSequence();
		using var ts2 = Enumerable.Range(1, o2).AsTestingSequence();
		using var ts3 = Enumerable.Range(1, o3).AsTestingSequence();

		ts1.ZipShortest(ts2, ts3).AssertSequenceEqual(
			Enumerable.Range(1, 2)
				.Select(x => (x, x, x)));
	}

	[Fact]
	public void FourParamsDisposesInnerSequencesCaseGetEnumeratorThrows()
	{
		using var s1 = TestingSequence.Of(1, 2);
		using var s2 = TestingSequence.Of(1, 2);
		using var s3 = TestingSequence.Of(1, 2);

		Assert.Throws<TestException>(() =>
			s1.ZipShortest(s2, s3, new BreakingSequence<int>()).Consume());
	}

	[Theory]
	[InlineData(1), InlineData(2), InlineData(3), InlineData(4)]
	public void FourParamsWorksProperly(int offset)
	{
		var o1 = (offset + 0) % 4 + 2;
		var o2 = (offset + 1) % 4 + 2;
		var o3 = (offset + 2) % 4 + 2;
		var o4 = (offset + 3) % 4 + 2;

		using var ts1 = Enumerable.Range(1, o1).AsTestingSequence();
		using var ts2 = Enumerable.Range(1, o2).AsTestingSequence();
		using var ts3 = Enumerable.Range(1, o3).AsTestingSequence();
		using var ts4 = Enumerable.Range(1, o4).AsTestingSequence();

		ts1.ZipShortest(ts2, ts3, ts4).AssertSequenceEqual(
			Enumerable.Range(1, 2)
				.Select(x => (x, x, x, x)));
	}
}
