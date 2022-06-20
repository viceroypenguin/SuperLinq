namespace Test;

public class ScanRightTest
{
	// ScanRight(source, func)

	[Fact]
	public void ScanRightWithEmptySequence()
	{
		var result = Array.Empty<int>().ScanRight((a, b) => a + b);

		Assert.Equal(Array.Empty<int>(), result);
	}

	[Fact]
	public void ScanRightDisposesEnumerator()
	{
		using var result = TestingSequence.Of<int>();

		Assert.Equal(Array.Empty<int>(), result.ScanRight((a, b) => a + b));
	}

	[Fact]
	public void ScanRightFuncIsNotInvokedOnSingleElementSequence()
	{
		const int value = 1;

		var result = new[] { value }.ScanRight(BreakingFunc.Of<int, int, int>());

		Assert.Equal(new[] { value }, result);
	}

	[Fact]
	public void ScanRight()
	{
		var result = Enumerable.Range(1, 5)
							   .Select(x => x.ToString())
							   .ScanRight((a, b) => string.Format("({0}+{1})", a, b));

		var expectations = new[] { "(1+(2+(3+(4+5))))", "(2+(3+(4+5)))", "(3+(4+5))", "(4+5)", "5" };

		Assert.Equal(expectations, result);
	}

	[Fact]
	public void ScanRightIsLazy()
	{
		new BreakingSequence<int>().ScanRight(BreakingFunc.Of<int, int, int>());
	}

	// ScanRight(source, seed, func)

	[Theory]
	[InlineData(5)]
	[InlineData("c")]
	[InlineData(true)]
	public void ScanRightSeedWithEmptySequence(object defaultValue)
	{
		Assert.Equal(new[] { defaultValue }, Array.Empty<int>().ScanRight(defaultValue, (a, b) => b));
	}

	[Fact]
	public void ScanRightSeedFuncIsNotInvokedOnEmptySequence()
	{
		const int value = 1;

		var result = Array.Empty<int>().ScanRight(value, BreakingFunc.Of<int, int, int>());

		Assert.Equal(new[] { value }, result);
	}

	[Fact]
	public void ScanRightSeed()
	{
		var result = Enumerable.Range(1, 4)
							   .ScanRight("5", (a, b) => string.Format("({0}+{1})", a, b));

		var expectations = new[] { "(1+(2+(3+(4+5))))", "(2+(3+(4+5)))", "(3+(4+5))", "(4+5)", "5" };

		Assert.Equal(expectations, result);
	}

	[Fact]
	public void ScanRightSeedIsLazy()
	{
		new BreakingSequence<int>().ScanRight(string.Empty, BreakingFunc.Of<int, string, string>());
	}
}
