namespace Test.Async;

public class ExceptByTest
{
	[Fact]
	public Task SimpleExceptBy()
	{
		var first = AsyncSeq("aaa", "bb", "c", "dddd");
		var second = AsyncSeq("xx", "y");
		var result = first.ExceptBy(second, x => x.Length);
		return result.AssertSequenceEqual("aaa", "dddd");
	}

	[Fact]
	public void ExceptByIsLazy()
	{
		var bs = new AsyncBreakingSequence<string>();
		bs.ExceptBy(bs, BreakingFunc.Of<string, int>());
	}

	[Fact]
	public Task ExceptByDoesNotRepeatSourceElementsWithDuplicateKeys()
	{
		var first = AsyncSeq("aaa", "bb", "c", "a", "b", "c", "dddd");
		var second = AsyncSeq("xx");
		var result = first.ExceptBy(second, x => x.Length);
		return result.AssertSequenceEqual("aaa", "c", "dddd");
	}

	[Fact]
	public Task ExceptByWithComparer()
	{
		var first = AsyncSeq("first", "second", "third", "fourth");
		var second = AsyncSeq("FIRST", "thiRD", "FIFTH");
		var result = first.ExceptBy(second, word => word, StringComparer.OrdinalIgnoreCase);
		return result.AssertSequenceEqual("second", "fourth");
	}

	[Fact]
	public Task ExceptByNullComparer()
	{
		var first = AsyncSeq("aaa", "bb", "c", "dddd");
		var second = AsyncSeq("xx", "y");
		var result = first.ExceptBy(second, x => x.Length, keyComparer: null);
		return result.AssertSequenceEqual("aaa", "dddd");
	}

	[Fact]
	public void ExceptByIsLazyWithComparer()
	{
		var bs = new AsyncBreakingSequence<string>();
		bs.ExceptBy(bs, BreakingFunc.Of<string, string>(), StringComparer.Ordinal);
	}
}
