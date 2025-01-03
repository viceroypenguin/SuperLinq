namespace SuperLinq.Tests;

public sealed class PreScanTest
{
	[Test]
	public void PreScanIsLazy()
	{
		_ = new BreakingSequence<int>().PreScan(BreakingFunc.Of<int, int, int>(), 0);
	}

	[Test]
	public void PreScanWithEmptySequence()
	{
		using var source = TestingSequence.Of<int>();

		var result = source.PreScan(BreakingFunc.Of<int, int, int>(), 0);
		result.AssertSequenceEqual();
	}

	[Test]
	public void PreScanWithSingleElement()
	{
		using var source = TestingSequence.Of(111);

		var result = source.PreScan(BreakingFunc.Of<int, int, int>(), 999);
		result.AssertSequenceEqual(999);
	}

	[Test]
	public void PreScanSum()
	{
		using var source = TestingSequence.Of(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);

		var result = source.PreScan((a, b) => a + b, 0);
		result.AssertSequenceEqual(0, 1, 3, 6, 10, 15, 21, 28, 36, 45);
	}

	[Test]
	public void PreScanMul()
	{
		using var source = TestingSequence.Of(1, 2, 3);

		var result = source.PreScan((a, b) => a * b, 1);
		result.AssertSequenceEqual(1, 1, 2);
	}

	[Test]
	public void PreScanFuncIsNotInvokedUnnecessarily()
	{
		using var source = Enumerable.Range(1, 3).AsTestingSequence();

		var count = 0;
		var sequence = source
			.PreScan(
				(a, b) => ++count == 3 ? throw new TestException() : a + b,
				0);
		sequence.AssertSequenceEqual(0, 1, 3);
	}
}
