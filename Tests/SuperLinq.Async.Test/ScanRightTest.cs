namespace Test.Async;

public class ScanRightTest
{
	// ScanRight(source, func)

	[Fact]
	public Task ScanRightWithEmptySequence()
	{
		var result = AsyncEnumerable.Empty<int>().ScanRight((a, b) => a + b);

		return result.AssertEmpty();
	}

	[Fact]
	public async Task ScanRightDisposesEnumerator()
	{
		await using var result = TestingSequence.Of<int>();

		await result.ScanRight((a, b) => a + b).AssertEmpty();
	}

	[Fact]
	public async Task ScanRightFuncIsNotInvokedOnSingleElementSequence()
	{
		var result = AsyncSeq(1).ScanRight(BreakingFunc.Of<int, int, int>());

		await result.AssertSequenceEqual(1);
	}

	[Fact]
	public async Task ScanRight()
	{
		var result = AsyncEnumerable
			.Range(1, 5)
			.Select(x => x.ToString())
			.ScanRight((a, b) => string.Format("({0}+{1})", a, b));

		var expectations = new[] { "(1+(2+(3+(4+5))))", "(2+(3+(4+5)))", "(3+(4+5))", "(4+5)", "5" };

		await result.AssertSequenceEqual(expectations);
	}

	[Fact]
	public void ScanRightIsLazy()
	{
		new AsyncBreakingSequence<int>().ScanRight(BreakingFunc.Of<int, int, int>());
	}

	// ScanRight(source, seed, func)

	[Theory]
	[InlineData(5)]
	[InlineData("c")]
	[InlineData(true)]
	public Task ScanRightSeedWithEmptySequence(object defaultValue)
	{
		return AsyncEnumerable.Empty<int>().ScanRight(defaultValue, (a, b) => b).AssertSequenceEqual(defaultValue);
	}

	[Fact]
	public Task ScanRightSeedFuncIsNotInvokedOnEmptySequence()
	{
		var result = AsyncEnumerable.Empty<int>().ScanRight(1, BreakingFunc.Of<int, int, int>());

		return result.AssertSequenceEqual(1);
	}

	[Fact]
	public Task ScanRightSeed()
	{
		var result = AsyncEnumerable
			.Range(1, 4)
			.ScanRight("5", (a, b) => string.Format("({0}+{1})", a, b));

		var expectations = new[] { "(1+(2+(3+(4+5))))", "(2+(3+(4+5)))", "(3+(4+5))", "(4+5)", "5" };

		return result.AssertSequenceEqual(expectations);
	}

	[Fact]
	public void ScanRightSeedIsLazy()
	{
		new AsyncBreakingSequence<int>().ScanRight(string.Empty, BreakingFunc.Of<int, string, string>());
	}
}
