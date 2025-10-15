namespace SuperLinq.Async.Tests;

public sealed class ScanTest
{
	[Fact]
	public async Task ScanEmpty()
	{
		await using var seq = TestingSequence.Of<int>();

		var result = seq.Scan((a, b) => a + b);
		await result.AssertSequenceEqual();
	}

	[Fact]
	public async Task ScanSum()
	{
		await using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		var result = seq.Scan((a, b) => a + b);
		await result.AssertSequenceEqual(1, 3, 6, 10, 15, 21, 28, 36, 45, 55);
	}

	[Fact]
	public void ScanIsLazy()
	{
		_ = new AsyncBreakingSequence<object>().Scan(BreakingFunc.Of<object, object, object>());
	}

	[Fact]
	public async Task ScanDoesNotIterateExtra()
	{
		await using var seq = AsyncSeqExceptionAt(4).AsTestingSequence(maxEnumerations: 2);

		var result = seq.Scan((a, b) => a + b);

		_ = await Assert.ThrowsAsync<TestException>(
			async () => await result.Consume());
		await result.Take(3).AssertSequenceEqual(1, 3, 6);
	}

	[Fact]
	public async Task SeededScanEmpty()
	{
		await using var seq = TestingSequence.Of<int>();

		var result = seq.Scan(-1, (a, b) => a + b);
		Assert.Equal(-1, await result.SingleAsync());
	}

	[Fact]
	public async Task SeededScanSum()
	{
		await using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		var result = seq.Scan(0, (a, b) => a + b);
		await result.AssertSequenceEqual(0, 1, 3, 6, 10, 15, 21, 28, 36, 45, 55);
	}

	[Fact]
	public void SeededScanIsLazy()
	{
		_ = new AsyncBreakingSequence<object>().Scan(seed: null,
			BreakingFunc.Of<object?, object, object>());
	}

	[Fact]
	public async Task SeededScanDoesNotIterateExtra()
	{
		await using var seq = AsyncSeqExceptionAt(4).AsTestingSequence(maxEnumerations: 2);

		var result = seq.Scan(0, (a, b) => a + b);

		_ = await Assert.ThrowsAsync<TestException>(async () => await result.Consume());
		await result.Take(4).AssertSequenceEqual(0, 1, 3, 6);
	}
}

#pragma warning restore CS0618
