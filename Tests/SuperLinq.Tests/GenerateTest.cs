namespace SuperLinq.Tests;

public sealed class GenerateTest
{
	[Test]
	public void GenerateTerminatesWhenCheckReturnsFalse()
	{
		var result = SuperEnumerable.Generate(1, n => n + 2).TakeWhile(n => n < 10);

		result.AssertSequenceEqual(1, 3, 5, 7, 9);
	}

	[Test]
	public void GenerateProcessesNonNumerics()
	{
		var result = SuperEnumerable.Generate("", s => s + 'a').TakeWhile(s => s.Length < 5);

		result.AssertSequenceEqual("", "a", "aa", "aaa", "aaaa");
	}

	[Test]
	public void GenerateIsLazy()
	{
		_ = SuperEnumerable.Generate(0, BreakingFunc.Of<int, int>());
	}

	[Test]
	public void GenerateFuncIsNotInvokedUnnecessarily()
	{
		SuperEnumerable.Generate(0, BreakingFunc.Of<int, int>())
					  .Take(1)
					  .Consume();
	}
}
