namespace SuperLinq.Async.Tests;

public sealed class ZipLongestTest
{
	[Test]
	public void ZipLongestIsLazy()
	{
		var bs = new AsyncBreakingSequence<int>();
		_ = bs.ZipLongest(bs, BreakingFunc.Of<int, int, int>());
		_ = bs.ZipLongest(bs, bs, BreakingFunc.Of<int, int, int, int>());
		_ = bs.ZipLongest(bs, bs, bs, BreakingFunc.Of<int, int, int, int, int>());
	}

	[Test]
	public async Task TwoParamsDisposesInnerSequencesCaseGetEnumeratorThrows()
	{
		using var s1 = TestingSequence.Of(1, 2);

		_ = await Assert.ThrowsAsync<TestException>(async () =>
			await s1.ZipLongest(new AsyncBreakingSequence<int>()).Consume());
	}

	[Test]
	[Arguments(1), Arguments(2)]
	public async Task TwoParamsWorksProperly(int offset)
	{
		var o1 = ((offset + 0) % 2) + 2;
		var o2 = ((offset + 1) % 2) + 2;

		await using var ts1 = Enumerable.Range(1, o1).AsTestingSequence();
		await using var ts2 = Enumerable.Range(1, o2).AsTestingSequence();

		await ts1.ZipLongest(ts2).AssertSequenceEqual(
			Enumerable.Range(1, 3)
				.Select(x => (
					x > o1 ? 0 : x,
					x > o2 ? 0 : x)));
	}

	[Test]
	public async Task ThreeParamsDisposesInnerSequencesCaseGetEnumeratorThrows()
	{
		using var s1 = TestingSequence.Of(1, 2);
		using var s2 = TestingSequence.Of(1, 2);

		_ = await Assert.ThrowsAsync<TestException>(async () =>
			await s1.ZipLongest(s2, new AsyncBreakingSequence<int>()).Consume());
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

		await ts1.ZipLongest(ts2, ts3).AssertSequenceEqual(
			Enumerable.Range(1, 4)
				.Select(x => (
					x > o1 ? 0 : x,
					x > o2 ? 0 : x,
					x > o3 ? 0 : x)));
	}

	[Test]
	public async Task FourParamsDisposesInnerSequencesCaseGetEnumeratorThrows()
	{
		using var s1 = TestingSequence.Of(1, 2);
		using var s2 = TestingSequence.Of(1, 2);
		using var s3 = TestingSequence.Of(1, 2);

		_ = await Assert.ThrowsAsync<TestException>(async () =>
			await s1.ZipLongest(s2, s3, new AsyncBreakingSequence<int>()).Consume());
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

		await ts1.ZipLongest(ts2, ts3, ts4).AssertSequenceEqual(
			Enumerable.Range(1, 5)
				.Select(x => (
					x > o1 ? 0 : x,
					x > o2 ? 0 : x,
					x > o3 ? 0 : x,
					x > o4 ? 0 : x)));
	}
}
