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
}
