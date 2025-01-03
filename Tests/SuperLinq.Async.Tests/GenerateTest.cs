namespace SuperLinq.Async.Tests;

public sealed class GenerateTest
{
	[Test]
	public Task GenerateTerminatesWhenCheckReturnsFalse()
	{
		var result = AsyncSuperEnumerable.Generate(1, n => new ValueTask<int>(n + 2)).TakeWhile(n => n < 10);

		return result.AssertSequenceEqual(1, 3, 5, 7, 9);
	}

	[Test]
	public Task GenerateProcessesNonNumerics()
	{
		var result = AsyncSuperEnumerable.Generate("", s => new ValueTask<string>(s + 'a')).TakeWhile(s => s.Length < 5);

		return result.AssertSequenceEqual("", "a", "aa", "aaa", "aaaa");
	}

	[Test]
	public void GenerateIsLazy()
	{
		_ = AsyncSuperEnumerable.Generate(0, async i => await AsyncBreakingFunc.Of<int, int>()(i));
	}

	[Test]
	public async Task GenerateFuncIsNotInvokedUnnecessarily()
	{
		await AsyncSuperEnumerable.Generate(0, async i => await AsyncBreakingFunc.Of<int, int>()(i))
			.Take(1)
			.Consume();
	}
}
