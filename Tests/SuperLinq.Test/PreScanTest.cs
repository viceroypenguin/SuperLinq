namespace Test;
public class PreScanTest
{
	[Fact]
	public void PreScanIsLazy()
	{
		new BreakingSequence<int>().PreScan(BreakingFunc.Of<int, int, int>(), 0);
	}

	[Fact]
	public void PreScanWithEmptySequence()
	{
		var source = Enumerable.Empty<int>();
		var result = source.PreScan(BreakingFunc.Of<int, int, int>(), 0);

		Assert.Empty(result);
	}

	[Fact]
	public void PreScanWithSingleElement()
	{
		var source = Seq(111);
		var result = source.PreScan(BreakingFunc.Of<int, int, int>(), 999);
		result.AssertSequenceEqual(999);
	}

	[Fact]
	public void PreScanSum()
	{
		var result = Seq(1, 2, 3, 4, 5, 6, 7, 8, 9, 10).PreScan((a, b) => a + b, 0);
		result.AssertSequenceEqual(0, 1, 3, 6, 10, 15, 21, 28, 36, 45);
	}

	[Fact]
	public void PreScanMul()
	{
		var seq = Seq(1, 2, 3);
		var result = seq.PreScan((a, b) => a * b, 1);
		result.AssertSequenceEqual(1, 1, 2);
	}

	[Fact]
	public void PreScanFuncIsNotInvokedUnnecessarily()
	{
		var count = 0;
		var sequence = Enumerable.Range(1, 3).PreScan((a, b) =>
			++count == 3 ? throw new TestException() : a + b, 0);

		sequence.AssertSequenceEqual(0, 1, 3);
	}
}
