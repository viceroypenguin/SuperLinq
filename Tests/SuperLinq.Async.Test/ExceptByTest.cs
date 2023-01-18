namespace Test.Async;

public class ExceptByTest
{
	[Fact]
	public async Task SimpleExceptBy()
	{
		await using var first = AsyncSeq("aaa", "bb", "c", "dddd").AsTestingSequence();
		await using var second = AsyncSeq("xx", "y").AsTestingSequence();
		var result = first.ExceptBy(second, x => x.Length);
		await result.AssertSequenceEqual("aaa", "dddd");
	}

	[Fact]
	public void ExceptByIsLazy()
	{
		var bs = new AsyncBreakingSequence<string>();
		bs.ExceptBy(bs, BreakingFunc.Of<string, int>());
	}

	[Fact]
	public async Task ExceptByDoesNotRepeatSourceElementsWithDuplicateKeys()
	{
		await using var first = AsyncSeq("aaa", "bb", "c", "a", "b", "c", "dddd").AsTestingSequence();
		await using var second = AsyncSeq("xx").AsTestingSequence();
		var result = first.ExceptBy(second, x => x.Length);
		await result.AssertSequenceEqual("aaa", "c", "dddd");
	}

	[Fact]
	public async Task ExceptByWithComparer()
	{
		await using var first = AsyncSeq("first", "second", "third", "fourth").AsTestingSequence();
		await using var second = AsyncSeq("FIRST", "thiRD", "FIFTH").AsTestingSequence();
		var result = first.ExceptBy(second, word => word, StringComparer.OrdinalIgnoreCase);
		await result.AssertSequenceEqual("second", "fourth");
	}

	[Fact]
	public async Task ExceptByNullComparer()
	{
		await using var first = AsyncSeq("aaa", "bb", "c", "dddd").AsTestingSequence();
		await using var second = AsyncSeq("xx", "y").AsTestingSequence();
		var result = first.ExceptBy(second, x => x.Length, keyComparer: null);
		await result.AssertSequenceEqual("aaa", "dddd");
	}

	[Fact]
	public void ExceptByIsLazyWithComparer()
	{
		var bs = new AsyncBreakingSequence<string>();
		bs.ExceptBy(bs, BreakingFunc.Of<string, string>(), StringComparer.Ordinal);
	}
}
