namespace SuperLinq.Async.Tests;

public sealed class ZipShortestTest
{
	[Test]
	public void ZipShortestIsLazy()
	{
		var bs = new AsyncBreakingSequence<int>();
		_ = bs.ZipShortest(bs, BreakingFunc.Of<int, int, int>());
		_ = bs.ZipShortest(bs, bs, BreakingFunc.Of<int, int, int, int>());
		_ = bs.ZipShortest(bs, bs, bs, BreakingFunc.Of<int, int, int, int, int>());
	}

	[Test]
	public async Task TwoParamsDisposesInnerSequencesCaseGetEnumeratorThrows()
	{
		await using var s1 = TestingSequence.Of(1, 2);

		_ = await Assert.ThrowsAsync<TestException>(async () =>
			await s1.ZipShortest(new AsyncBreakingSequence<int>()).Consume());
	}

	[Test]
	[Arguments(1), Arguments(2)]
	public async Task TwoParamsWorksProperly(int offset)
	{
		var o1 = ((offset + 0) % 2) + 2;
		var o2 = ((offset + 1) % 2) + 2;

		await using var ts1 = Enumerable.Range(1, o1).AsTestingSequence();
		await using var ts2 = Enumerable.Range(1, o2).AsTestingSequence();

		await ts1.ZipShortest(ts2).AssertSequenceEqual(
			Enumerable.Range(1, 2)
				.Select(x => (x, x)));
	}

	[Test]
	public async Task ThreeParamsDisposesInnerSequencesCaseGetEnumeratorThrows()
	{
		await using var s1 = TestingSequence.Of(1, 2);
		await using var s2 = TestingSequence.Of(1, 2);

		_ = await Assert.ThrowsAsync<TestException>(async () =>
			await s1.ZipShortest(s2, new AsyncBreakingSequence<int>()).Consume());
	}

	[Test]
	[Arguments(1), Arguments(2), Arguments(3)]
	public async Task ThreeParamsWorksProperly(int offset)
	{
		var o1 = ((offset + 0) % 3) + 2;
		var o2 = ((offset + 1) % 3) + 2;
		var o3 = ((offset + 2) % 3) + 2;

		await using var ts1 = Enumerable.Range(1, o1).AsTestingSequence();
		await using var ts2 = Enumerable.Range(1, o2).AsTestingSequence();
		await using var ts3 = Enumerable.Range(1, o3).AsTestingSequence();

		await ts1.ZipShortest(ts2, ts3).AssertSequenceEqual(
			Enumerable.Range(1, 2)
				.Select(x => (x, x, x)));
	}

	[Test]
	public async Task FourParamsDisposesInnerSequencesCaseGetEnumeratorThrows()
	{
		await using var s1 = TestingSequence.Of(1, 2);
		await using var s2 = TestingSequence.Of(1, 2);
		await using var s3 = TestingSequence.Of(1, 2);

		_ = await Assert.ThrowsAsync<TestException>(async () =>
			await s1.ZipShortest(s2, s3, new AsyncBreakingSequence<int>()).Consume());
	}

	[Test]
	[Arguments(1), Arguments(2), Arguments(3), Arguments(4)]
	public async Task FourParamsWorksProperly(int offset)
	{
		var o1 = ((offset + 0) % 4) + 2;
		var o2 = ((offset + 1) % 4) + 2;
		var o3 = ((offset + 2) % 4) + 2;
		var o4 = ((offset + 3) % 4) + 2;

		await using var ts1 = Enumerable.Range(1, o1).AsTestingSequence();
		await using var ts2 = Enumerable.Range(1, o2).AsTestingSequence();
		await using var ts3 = Enumerable.Range(1, o3).AsTestingSequence();
		await using var ts4 = Enumerable.Range(1, o4).AsTestingSequence();

		await ts1.ZipShortest(ts2, ts3, ts4).AssertSequenceEqual(
			Enumerable.Range(1, 2)
				.Select(x => (x, x, x, x)));
	}

	[Test]
	public async Task ZipShortestNotIterateUnnecessaryElements()
	{
		using (var s1 = TestingSequence.Of(1, 2))
		using (var s2 = AsyncSeqExceptionAt(3).AsTestingSequence())
		{
			var zipped = s1.ZipShortest(s2, ValueTuple.Create);
			await zipped.AssertSequenceEqual((1, 1), (2, 2));
		}

		using (var s1 = AsyncSeqExceptionAt(4).AsTestingSequence())
		using (var s2 = TestingSequence.Of(1, 2))
		{
			var zipped = s1.ZipShortest(s2, ValueTuple.Create);
			await zipped.AssertSequenceEqual((1, 1), (2, 2));
		}

		using (var s1 = TestingSequence.Of(1, 2, 3))
		using (var s2 = TestingSequence.Of(1, 2))
		using (var s3 = AsyncSeqExceptionAt(3).AsTestingSequence())
		{
			var zipped = s1.ZipShortest(s2, s3, ValueTuple.Create);
			await zipped.AssertSequenceEqual((1, 1, 1), (2, 2, 2));
		}

		using (var s1 = AsyncSeqExceptionAt(4).AsTestingSequence())
		using (var s2 = TestingSequence.Of(1, 2, 3))
		using (var s3 = TestingSequence.Of(1, 2))
		{
			var zipped = s1.ZipShortest(s2, s3, ValueTuple.Create);
			await zipped.AssertSequenceEqual((1, 1, 1), (2, 2, 2));
		}

		using (var s1 = TestingSequence.Of(1, 2, 3))
		using (var s2 = TestingSequence.Of(1, 2, 3))
		using (var s3 = TestingSequence.Of(1, 2))
		using (var s4 = AsyncSeqExceptionAt(3).AsTestingSequence())
		{
			var zipped = s1.ZipShortest(s2, s3, s4, ValueTuple.Create);
			await zipped.AssertSequenceEqual((1, 1, 1, 1), (2, 2, 2, 2));
		}

		using (var s1 = AsyncSeqExceptionAt(4).AsTestingSequence())
		using (var s2 = TestingSequence.Of(1, 2, 3))
		using (var s3 = TestingSequence.Of(1, 2, 3))
		using (var s4 = TestingSequence.Of(1, 2))
		{
			var zipped = s1.ZipShortest(s2, s3, s4, ValueTuple.Create);
			await zipped.AssertSequenceEqual((1, 1, 1, 1), (2, 2, 2, 2));
		}
	}
}
