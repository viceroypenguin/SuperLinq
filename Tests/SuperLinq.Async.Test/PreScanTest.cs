namespace Test.Async;

public sealed class PreScanTest
{
	[Fact]
	public void PreScanIsLazy()
	{
		_ = new AsyncBreakingSequence<int>().PreScan(BreakingFunc.Of<int, int, int>(), 0);
	}

	[Fact]
	public async Task PreScanWithEmptySequence()
	{
		await using var source = TestingSequence.Of<int>();

		var result = source.PreScan(BreakingFunc.Of<int, int, int>(), 0);
		await result.AssertSequenceEqual();
	}

	[Fact]
	public async Task PreScanWithSingleElement()
	{
		await using var source = TestingSequence.Of(111);

		var result = source.PreScan(BreakingFunc.Of<int, int, int>(), 999);
		await result.AssertSequenceEqual(999);
	}

	[Fact]
	public async Task PreScanSum()
	{
		await using var source = TestingSequence.Of(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);

		var result = source.PreScan((a, b) => a + b, 0);
		await result.AssertSequenceEqual(0, 1, 3, 6, 10, 15, 21, 28, 36, 45);
	}

	[Fact]
	public async Task PreScanMul()
	{
		await using var source = TestingSequence.Of(1, 2, 3);

		var result = source.PreScan((a, b) => a * b, 1);
		await result.AssertSequenceEqual(1, 1, 2);
	}

	[Fact]
	public async Task PreScanFuncIsNotInvokedUnnecessarily()
	{
		await using var source = Enumerable.Range(1, 3).AsTestingSequence();

		var count = 0;
		var sequence = source
			.PreScan(
				(a, b) => ++count == 3 ? throw new TestException() : a + b,
				0);
		await sequence.AssertSequenceEqual(0, 1, 3);
	}
}
