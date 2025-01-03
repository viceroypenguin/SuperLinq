using System.Globalization;

namespace SuperLinq.Tests;

public sealed class ScanRightTest
{
	// ScanRight(source, func)

	[Test]
	public void ScanRightWithEmptySequence()
	{
		using var seq = TestingSequence.Of<int>();

		var result = seq.ScanRight((a, b) => a + b);
		result.AssertSequenceEqual();
	}

	[Test]
	public void ScanRightFuncIsNotInvokedOnSingleElementSequence()
	{
		using var seq = TestingSequence.Of(1);

		var result = seq.ScanRight(BreakingFunc.Of<int, int, int>());
		result.AssertSequenceEqual(1);
	}

	[Test]
	public void ScanRight()
	{
		using var seq = Enumerable.Range(1, 5)
			.Select(x => x.ToString(CultureInfo.InvariantCulture))
			.AsTestingSequence();

		var result = seq
			.ScanRight((a, b) => FormattableString.Invariant($"({a}+{b})"));

		result.AssertSequenceEqual(
			"(1+(2+(3+(4+5))))", "(2+(3+(4+5)))", "(3+(4+5))", "(4+5)", "5");
	}

	[Test]
	public void ScanRightWithList()
	{
		var list = Enumerable.Range(1, 5)
			.Select(x => x.ToString(CultureInfo.InvariantCulture))
			.ToList();

		var result = list
			.ScanRight((a, b) => FormattableString.Invariant($"({a}+{b})"));

		result.AssertSequenceEqual(
			"(1+(2+(3+(4+5))))", "(2+(3+(4+5)))", "(3+(4+5))", "(4+5)", "5");
	}

	[Test]
	public void ScanRightIsLazy()
	{
		_ = new BreakingSequence<int>().ScanRight(BreakingFunc.Of<int, int, int>());
	}

	// ScanRight(source, seed, func)

	[Test]
	[Arguments(5)]
	[Arguments("c")]
	[Arguments(true)]
	public void ScanRightSeedWithEmptySequence(object defaultValue)
	{
		using var seq = TestingSequence.Of<int>();

		var result = seq.ScanRight(defaultValue, (a, b) => b);
		result.AssertSequenceEqual(defaultValue);
	}

	[Test]
	public void ScanRightSeedFuncIsNotInvokedOnEmptySequence()
	{
		using var seq = TestingSequence.Of<int>();

		var result = seq.ScanRight(1, BreakingFunc.Of<int, int, int>());
		result.AssertSequenceEqual(1);
	}

	[Test]
	public void ScanRightSeed()
	{
		using var seq = Enumerable.Range(1, 4).AsTestingSequence();

		var result = seq
			.ScanRight("5", (a, b) => FormattableString.Invariant($"({a}+{b})"));

		result.AssertSequenceEqual(
			"(1+(2+(3+(4+5))))", "(2+(3+(4+5)))", "(3+(4+5))", "(4+5)", "5");
	}

	[Test]
	public void ScanRightSeedWithList()
	{
		var list = Enumerable.Range(1, 4).ToList();

		var result = list
			.ScanRight("5", (a, b) => FormattableString.Invariant($"({a}+{b})"));

		result.AssertSequenceEqual(
			"(1+(2+(3+(4+5))))", "(2+(3+(4+5)))", "(3+(4+5))", "(4+5)", "5");
	}

	[Test]
	public void ScanRightSeedIsLazy()
	{
		_ = new BreakingSequence<int>().ScanRight("", BreakingFunc.Of<int, string, string>());
	}
}
