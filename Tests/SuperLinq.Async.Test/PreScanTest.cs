namespace Test.Async;

public class PreScanTest
{
	[Fact]
	public void PreScanIsLazy()
	{
		new AsyncBreakingSequence<int>().PreScan(BreakingFunc.Of<int, int, int>(), 0);
	}

	[Fact]
	public Task PreScanWithEmptySequence()
	{
		var source = AsyncEnumerable.Empty<int>();
		var result = source.PreScan(BreakingFunc.Of<int, int, int>(), 0);

		return result.AssertEmpty();
	}

	[Fact]
	public Task PreScanWithSingleElement()
	{
		var source = AsyncSeq(111);
		var result = source.PreScan(BreakingFunc.Of<int, int, int>(), 999);
		return result.AssertSequenceEqual(999);
	}

	[Fact]
	public Task PreScanSum()
	{
		var result = AsyncSeq(1, 2, 3, 4, 5, 6, 7, 8, 9, 10).PreScan((a, b) => a + b, 0);
		return result.AssertSequenceEqual(0, 1, 3, 6, 10, 15, 21, 28, 36, 45);
	}

	[Fact]
	public Task PreScanMul()
	{
		var seq = AsyncSeq(1, 2, 3);
		var result = seq.PreScan((a, b) => a * b, 1);
		return result.AssertSequenceEqual(1, 1, 2);
	}

	[Fact]
	public Task PreScanFuncIsNotInvokedUnnecessarily()
	{
		var count = 0;
		var gold = new[] { 0, 1, 3 };
		var sequence = AsyncEnumerable.Range(1, 3).PreScan((a, b) =>
			++count == gold.Length ? throw new NotSupportedException() : a + b, 0);

		return sequence.AssertSequenceEqual(gold);
	}
}
