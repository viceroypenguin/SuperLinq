namespace Test.Async;

public sealed class ExceptByTest
{
	[Fact]
	public async Task SimpleExceptBy()
	{
		await using var first = TestingSequence.Of("aaa", "bb", "c", "dddd");
		await using var second = TestingSequence.Of("xx", "y");
		var result = first.ExceptBy(second, x => x.Length);
		await result.AssertSequenceEqual("aaa", "dddd");
	}

	[Fact]
	public void ExceptByIsLazy()
	{
		var bs = new AsyncBreakingSequence<string>();
		_ = bs.ExceptBy(bs, BreakingFunc.Of<string, int>());
	}

	[Fact]
	public async Task ExceptByDoesNotRepeatSourceElementsWithDuplicateKeys()
	{
		await using var first = TestingSequence.Of("aaa", "bb", "c", "a", "b", "c", "dddd");
		await using var second = TestingSequence.Of("xx");
		var result = first.ExceptBy(second, x => x.Length);
		await result.AssertSequenceEqual("aaa", "c", "dddd");
	}

	[Fact]
	public async Task ExceptByWithComparer()
	{
		await using var first = TestingSequence.Of("first", "second", "third", "fourth");
		await using var second = TestingSequence.Of("FIRST", "thiRD", "FIFTH");
		var result = first.ExceptBy(second, word => word, StringComparer.OrdinalIgnoreCase);
		await result.AssertSequenceEqual("second", "fourth");
	}

	[Fact]
	public async Task ExceptByNullComparer()
	{
		await using var first = TestingSequence.Of("aaa", "bb", "c", "dddd");
		await using var second = TestingSequence.Of("xx", "y");
		var result = first.ExceptBy(second, x => x.Length, keyComparer: null);
		await result.AssertSequenceEqual("aaa", "dddd");
	}

	[Fact]
	public void ExceptByIsLazyWithComparer()
	{
		var bs = new AsyncBreakingSequence<string>();
		_ = bs.ExceptBy(bs, BreakingFunc.Of<string, string>(), StringComparer.Ordinal);
	}
}
