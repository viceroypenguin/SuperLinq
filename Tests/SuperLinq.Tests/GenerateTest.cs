namespace SuperLinq.Tests;

public sealed class GenerateTest
{
	[Fact]
	public void GenerateTerminatesWhenCheckReturnsFalse()
	{
		var result = SuperEnumerable.Generate(1, n => n + 2).TakeWhile(n => n < 10);

		result.AssertSequenceEqual(1, 3, 5, 7, 9);
	}

	[Fact]
	public void GenerateProcessesNonNumerics()
	{
		var result = SuperEnumerable.Generate("", s => s + 'a').TakeWhile(s => s.Length < 5);

		result.AssertSequenceEqual("", "a", "aa", "aaa", "aaaa");
	}

	[Fact]
	public void GenerateIsLazy()
	{
		_ = SuperEnumerable.Generate(0, BreakingFunc.Of<int, int>());
	}

	[Fact]
	public void GenerateFuncIsNotInvokedUnnecessarily()
	{
		SuperEnumerable.Generate(0, BreakingFunc.Of<int, int>())
					  .Take(1)
					  .Consume();
	}
}
