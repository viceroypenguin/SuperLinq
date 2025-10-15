namespace SuperLinq.Tests;

public sealed class ExceptByTest
{
	[Fact]
	public void SimpleExceptBy()
	{
		using var first = TestingSequence.Of("aaa", "bb", "c", "dddd");
		using var second = TestingSequence.Of("xx", "y");
		var result = first.ExceptBy(second, x => x.Length);
		result.AssertSequenceEqual("aaa", "dddd");
	}

	[Fact]
	public void ExceptByIsLazy()
	{
		var bs = new BreakingSequence<string>();
		_ = bs.ExceptBy(bs, BreakingFunc.Of<string, int>());
	}

	[Fact]
	public void ExceptByDoesNotRepeatSourceElementsWithDuplicateKeys()
	{
		using var first = TestingSequence.Of("aaa", "bb", "c", "a", "b", "c", "dddd");
		using var second = TestingSequence.Of("xx");
		var result = first.ExceptBy(second, x => x.Length);
		result.AssertSequenceEqual("aaa", "c", "dddd");
	}

	[Fact]
	public void ExceptByWithComparer()
	{
		using var first = TestingSequence.Of("first", "second", "third", "fourth");
		using var second = TestingSequence.Of("FIRST", "thiRD", "FIFTH");
		var result = first.ExceptBy(second, StringComparer.OrdinalIgnoreCase.GetHashCode, EqualityComparer.Create<int>((x, y) => x == y));
		result.AssertSequenceEqual("second", "fourth");
	}

	[Fact]
	public void ExceptByNullComparer()
	{
		using var first = TestingSequence.Of("aaa", "bb", "c", "dddd");
		using var second = TestingSequence.Of("xx", "y");
		var result = first.ExceptBy(second, x => x.Length, keyComparer: null);
		result.AssertSequenceEqual("aaa", "dddd");
	}

	[Fact]
	public void ExceptByIsLazyWithComparer()
	{
		var bs = new BreakingSequence<int>();
		_ = bs.ExceptBy(bs, BreakingFunc.Of<int, string>(), StringComparer.Ordinal);
	}
}
