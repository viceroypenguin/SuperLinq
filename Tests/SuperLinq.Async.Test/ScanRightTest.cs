using System.Globalization;

namespace Test.Async;

public sealed class ScanRightTest
{
	// ScanRight(source, func)

	[Fact]
	public async Task ScanRightWithEmptySequence()
	{
		await using var seq = TestingSequence.Of<int>();

		var result = seq.ScanRight((a, b) => a + b);
		await result.AssertSequenceEqual();
	}

	[Fact]
	public async Task ScanRightFuncIsNotInvokedOnSingleElementSequence()
	{
		await using var seq = TestingSequence.Of(1);

		var result = seq.ScanRight(BreakingFunc.Of<int, int, int>());
		await result.AssertSequenceEqual(1);
	}

	[Fact]
	public async Task ScanRight()
	{
		await using var seq = Enumerable.Range(1, 5).AsTestingSequence();

		var result = seq
			.Select(x => x.ToString(CultureInfo.InvariantCulture))
			.ScanRight((a, b) => FormattableString.Invariant($"({a}+{b})"));

		await result.AssertSequenceEqual(
			"(1+(2+(3+(4+5))))", "(2+(3+(4+5)))", "(3+(4+5))", "(4+5)", "5");
	}

	[Fact]
	public void ScanRightIsLazy()
	{
		_ = new AsyncBreakingSequence<int>().ScanRight(BreakingFunc.Of<int, int, int>());
	}

	// ScanRight(source, seed, func)

	[Theory]
	[InlineData(5)]
	[InlineData("c")]
	[InlineData(true)]
	public async Task ScanRightSeedWithEmptySequence(object defaultValue)
	{
		await using var seq = TestingSequence.Of<int>();

		var result = seq.ScanRight(defaultValue, (a, b) => b);
		await result.AssertSequenceEqual(defaultValue);
	}

	[Fact]
	public async Task ScanRightSeedFuncIsNotInvokedOnEmptySequence()
	{
		await using var seq = TestingSequence.Of<int>();

		var result = seq.ScanRight(1, BreakingFunc.Of<int, int, int>());
		await result.AssertSequenceEqual(1);
	}

	[Fact]
	public async Task ScanRightSeed()
	{
		await using var seq = Enumerable.Range(1, 4).AsTestingSequence();

		var result = seq
			.ScanRight("5", (a, b) => FormattableString.Invariant($"({a}+{b})"));

		await result.AssertSequenceEqual(
			"(1+(2+(3+(4+5))))", "(2+(3+(4+5)))", "(3+(4+5))", "(4+5)", "5");
	}

	[Fact]
	public void ScanRightSeedIsLazy()
	{
		_ = new AsyncBreakingSequence<int>().ScanRight(string.Empty, BreakingFunc.Of<int, string, string>());
	}
}
