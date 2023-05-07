namespace Test;

public class PreScanTest
{
	[Fact]
	public void PreScanIsLazy()
	{
		_ = new BreakingSequence<int>().PreScan(BreakingFunc.Of<int, int, int>(), 0);
	}

	[Fact]
	public void PreScanWithEmptySequence()
	{
		using var source = TestingSequence.Of<int>();

		var result = source.PreScan(BreakingFunc.Of<int, int, int>(), 0);
		result.AssertSequenceEqual();
	}

	[Fact]
	public void PreScanWithSingleElement()
	{
		using var source = TestingSequence.Of(111);

		var result = source.PreScan(BreakingFunc.Of<int, int, int>(), 999);
		result.AssertSequenceEqual(999);
	}

	[Fact]
	public void PreScanSum()
	{
		using var source = TestingSequence.Of(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);

		var result = source.PreScan((a, b) => a + b, 0);
		result.AssertSequenceEqual(0, 1, 3, 6, 10, 15, 21, 28, 36, 45);
	}

	[Fact]
	public void PreScanMul()
	{
		using var source = TestingSequence.Of(1, 2, 3);

		var result = source.PreScan((a, b) => a * b, 1);
		result.AssertSequenceEqual(1, 1, 2);
	}

	[Fact]
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

	[Fact]
	public void PreScanCollection()
	{
		using var seq = Enumerable.Range(1, 10).AsBreakingCollection();

		var result = seq.PreScan((a, b) => a + b, 5);
		Assert.Equal(10, result.Count());

		result.ToArray()
			.AssertSequenceEqual(5, 6, 8, 11, 15, 20, 26, 33, 41, 50);
		Assert.Equal(1, seq.CopyCount);

		var arr = new int[20];
		_ = result.CopyTo(arr, 5);
		arr
			.AssertSequenceEqual(0, 0, 0, 0, 0, 5, 6, 8, 11, 15, 20, 26, 33, 41, 50, 0, 0, 0, 0, 0);
		Assert.Equal(2, seq.CopyCount);
	}

	[Fact]
	public void PreScanList()
	{
		using var seq = Enumerable.Range(1, 10).AsBreakingList();

		var result = seq.PreScan((a, b) => a + b, 5);
		Assert.Equal(10, result.Count());

		result.ToArray()
			.AssertSequenceEqual(5, 6, 8, 11, 15, 20, 26, 33, 41, 50);

		var arr = new int[20];
		_ = result.CopyTo(arr, 5);
		arr
			.AssertSequenceEqual(0, 0, 0, 0, 0, 5, 6, 8, 11, 15, 20, 26, 33, 41, 50, 0, 0, 0, 0, 0);
	}
}
