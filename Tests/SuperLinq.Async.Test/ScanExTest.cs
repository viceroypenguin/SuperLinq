namespace Test.Async;

public class ScanExTest
{
	[Fact]
	public async Task ScanExEmpty()
	{
		await using var seq = TestingSequence.Of<int>();

		var result = seq.ScanEx((a, b) => a + b);
		await result.AssertSequenceEqual();
	}

	[Fact]
	public async Task ScanExSum()
	{
		await using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		var result = seq.ScanEx((a, b) => a + b);
		await result.AssertSequenceEqual(1, 3, 6, 10, 15, 21, 28, 36, 45, 55);
	}

	[Fact]
	public void ScanExIsLazy()
	{
		new AsyncBreakingSequence<object>().ScanEx(BreakingFunc.Of<object, object, object>());
	}

	[Fact]
	public async Task ScanExDoesNotIterateExtra()
	{
		await using var seq = AsyncSeqExceptionAt(4).AsTestingSequence(maxEnumerations: 2);

		var result = seq.ScanEx((a, b) => a + b);

		await Assert.ThrowsAsync<TestException>(
			async () => await result.Consume());
		await result.Take(3).AssertSequenceEqual(1, 3, 6);
	}

	[Fact]
	public async Task SeededScanExEmpty()
	{
		await using var seq = TestingSequence.Of<int>();

		var result = seq.ScanEx(-1, (a, b) => a + b);
		Assert.Equal(-1, await result.SingleAsync());
	}

	[Fact]
	public async Task SeededScanExSum()
	{
		await using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		var result = seq.ScanEx(0, (a, b) => a + b);
		await result.AssertSequenceEqual(0, 1, 3, 6, 10, 15, 21, 28, 36, 45, 55);
	}

	[Fact]
	public void SeededScanExIsLazy()
	{
		new AsyncBreakingSequence<object>().ScanEx(seed: null,
			BreakingFunc.Of<object?, object, object>());
	}

	[Fact]
	public async Task SeededScanExDoesNotIterateExtra()
	{
		await using var seq = AsyncSeqExceptionAt(4).AsTestingSequence(maxEnumerations: 2);

		var result = seq.ScanEx(0, (a, b) => a + b);

		await Assert.ThrowsAsync<TestException>(async () => await result.Consume());
		await result.Take(4).AssertSequenceEqual(0, 1, 3, 6);
	}
}
