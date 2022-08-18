namespace Test;

#pragma warning disable CS0618

public class ExceptByTest
{
	[Fact]
	public void SimpleExceptBy()
	{
		string[] first = { "aaa", "bb", "c", "dddd" };
		string[] second = { "xx", "y" };
		var result = SuperEnumerable.ExceptBy(first, second, x => x.Length);
		result.AssertSequenceEqual("aaa", "dddd");
	}

	[Fact]
	public void ExceptByIsLazy()
	{
		var bs = new BreakingSequence<string>();
		SuperEnumerable.ExceptBy(bs, bs, BreakingFunc.Of<string, int>());
	}

	[Fact]
	public void ExceptByDoesNotRepeatSourceElementsWithDuplicateKeys()
	{
		string[] first = { "aaa", "bb", "c", "a", "b", "c", "dddd" };
		string[] second = { "xx" };
		var result = SuperEnumerable.ExceptBy(first, second, x => x.Length);
		result.AssertSequenceEqual("aaa", "c", "dddd");
	}

	[Fact]
	public void ExceptByWithComparer()
	{
		string[] first = { "first", "second", "third", "fourth" };
		string[] second = { "FIRST", "thiRD", "FIFTH" };
		var result = SuperEnumerable.ExceptBy(first, second, word => word, StringComparer.OrdinalIgnoreCase);
		result.AssertSequenceEqual("second", "fourth");
	}

	[Fact]
	public void ExceptByNullComparer()
	{
		string[] first = { "aaa", "bb", "c", "dddd" };
		string[] second = { "xx", "y" };
		var result = SuperEnumerable.ExceptBy(first, second, x => x.Length, keyComparer: null);
		result.AssertSequenceEqual("aaa", "dddd");
	}

	[Fact]
	public void ExceptByIsLazyWithComparer()
	{
		var bs = new BreakingSequence<string>();
		SuperEnumerable.ExceptBy(bs, bs, BreakingFunc.Of<string, string>(), StringComparer.Ordinal);
	}
}

#pragma warning restore CS0618 // Type or member is obsolete
