namespace Test;

public class ScanRightTest
{
	// ScanRight(source, func)

	[Fact]
	public void ScanRightWithEmptySequence()
	{
		using var seq = TestingSequence.Of<int>();

		var result = seq.ScanRight((a, b) => a + b);
		result.AssertSequenceEqual();
	}

	[Fact]
	public void ScanRightFuncIsNotInvokedOnSingleElementSequence()
	{
		using var seq = TestingSequence.Of(1);

		var result = seq.ScanRight(BreakingFunc.Of<int, int, int>());
		result.AssertSequenceEqual(1);
	}

	[Fact]
	public void ScanRight()
	{
		using var seq = Enumerable.Range(1, 5)
			.Select(x => x.ToString())
			.AsTestingSequence();

		var result = seq
			.ScanRight((a, b) => string.Format("({0}+{1})", a, b));

		result.AssertSequenceEqual(
			"(1+(2+(3+(4+5))))", "(2+(3+(4+5)))", "(3+(4+5))", "(4+5)", "5");
	}

	[Fact]
	public void ScanRightWithList()
	{
		var list = Enumerable.Range(1, 5)
			.Select(x => x.ToString())
			.ToList();

		var result = list
			.ScanRight((a, b) => string.Format("({0}+{1})", a, b));

		result.AssertSequenceEqual(
			"(1+(2+(3+(4+5))))", "(2+(3+(4+5)))", "(3+(4+5))", "(4+5)", "5");
	}

	[Fact]
	public void ScanRightIsLazy()
	{
		_ = new BreakingSequence<int>().ScanRight(BreakingFunc.Of<int, int, int>());
	}

	[Fact]
	public void ScanRightCollection()
	{
		using var seq = Enumerable.Range(1, 10).AsBreakingCollection();

		var result = seq.ScanRight((a, b) => a + b);
		result.AssertCollectionErrorChecking(10);

		result.ToArray()
			.AssertSequenceEqual(55, 54, 52, 49, 45, 40, 34, 27, 19, 10);
		Assert.Equal(1, seq.CopyCount);

		var arr = new int[20];
		_ = result.CopyTo(arr, 5);
		arr
			.AssertSequenceEqual(0, 0, 0, 0, 0, 55, 54, 52, 49, 45, 40, 34, 27, 19, 10, 0, 0, 0, 0, 0);
		Assert.Equal(2, seq.CopyCount);
	}

	[Fact]
	public void ScanRightList()
	{
		using var seq = Enumerable.Range(1, 10).AsBreakingList();

		var result = seq.ScanRight((a, b) => a + b);
		Assert.Equal(10, result.Count());

		result.ToArray()
			.AssertSequenceEqual(55, 54, 52, 49, 45, 40, 34, 27, 19, 10);

		var arr = new int[20];
		_ = result.CopyTo(arr, 5);
		arr
			.AssertSequenceEqual(0, 0, 0, 0, 0, 55, 54, 52, 49, 45, 40, 34, 27, 19, 10, 0, 0, 0, 0, 0);
	}

	// ScanRight(source, seed, func)

	[Theory]
	[InlineData(5)]
	[InlineData("c")]
	[InlineData(true)]
	public void ScanRightSeedWithEmptySequence(object defaultValue)
	{
		using var seq = TestingSequence.Of<int>();

		var result = seq.ScanRight(defaultValue, (a, b) => b);
		result.AssertSequenceEqual(defaultValue);
	}

	[Fact]
	public void ScanRightSeedFuncIsNotInvokedOnEmptySequence()
	{
		using var seq = TestingSequence.Of<int>();

		var result = seq.ScanRight(1, BreakingFunc.Of<int, int, int>());
		result.AssertSequenceEqual(1);
	}

	[Fact]
	public void ScanRightSeed()
	{
		using var seq = Enumerable.Range(1, 4).AsTestingSequence();

		var result = seq
			.ScanRight("5", (a, b) => string.Format("({0}+{1})", a, b));

		result.AssertSequenceEqual(
			"(1+(2+(3+(4+5))))", "(2+(3+(4+5)))", "(3+(4+5))", "(4+5)", "5");
	}

	[Fact]
	public void ScanRightSeedWithList()
	{
		var list = Enumerable.Range(1, 4).ToList();

		var result = list
			.ScanRight("5", (a, b) => string.Format("({0}+{1})", a, b));

		result.AssertSequenceEqual(
			"(1+(2+(3+(4+5))))", "(2+(3+(4+5)))", "(3+(4+5))", "(4+5)", "5");
	}

	[Fact]
	public void ScanRightSeedIsLazy()
	{
		_ = new BreakingSequence<int>().ScanRight(string.Empty, BreakingFunc.Of<int, string, string>());
	}

	[Fact]
	public void ScanRightSeedCollection()
	{
		using var seq = Enumerable.Range(1, 10).AsBreakingCollection();

		var result = seq.ScanRight(5, (a, b) => a + b);
		result.AssertCollectionErrorChecking(11);

		result.ToArray()
			.AssertSequenceEqual(60, 59, 57, 54, 50, 45, 39, 32, 24, 15, 5);
		Assert.Equal(1, seq.CopyCount);

		var arr = new int[20];
		_ = result.CopyTo(arr, 5);
		arr
			.AssertSequenceEqual(0, 0, 0, 0, 0, 60, 59, 57, 54, 50, 45, 39, 32, 24, 15, 5, 0, 0, 0, 0);
		Assert.Equal(2, seq.CopyCount);
	}

	[Fact]
	public void ScanRightSeedList()
	{
		using var seq = Enumerable.Range(1, 10).AsBreakingList();

		var result = seq.ScanRight(5, (a, b) => a + b);
		result.AssertCollectionErrorChecking(11);

		result.ToArray()
			.AssertSequenceEqual(60, 59, 57, 54, 50, 45, 39, 32, 24, 15, 5);

		var arr = new int[20];
		_ = result.CopyTo(arr, 5);
		arr
			.AssertSequenceEqual(0, 0, 0, 0, 0, 60, 59, 57, 54, 50, 45, 39, 32, 24, 15, 5, 0, 0, 0, 0);
	}
}
