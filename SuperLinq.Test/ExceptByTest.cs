using NUnit.Framework;

namespace Test;

[TestFixture]
public class ExceptByTest
{
	[Test]
	public void SimpleExceptBy()
	{
		string[] first = { "aaa", "bb", "c", "dddd" };
		string[] second = { "xx", "y" };
		var result = first.ExceptByItems(second, x => x.Length);
		result.AssertSequenceEqual("aaa", "dddd");
	}

	[Test]
	public void ExceptByIsLazy()
	{
		var bs = new BreakingSequence<string>();
		bs.ExceptByItems(bs, BreakingFunc.Of<string, int>());
	}

	[Test]
	public void ExceptByDoesNotRepeatSourceElementsWithDuplicateKeys()
	{
		string[] first = { "aaa", "bb", "c", "a", "b", "c", "dddd" };
		string[] second = { "xx" };
		var result = first.ExceptByItems(second, x => x.Length);
		result.AssertSequenceEqual("aaa", "c", "dddd");
	}

	[Test]
	public void ExceptByWithComparer()
	{
		string[] first = { "first", "second", "third", "fourth" };
		string[] second = { "FIRST", "thiRD", "FIFTH" };
		var result = first.ExceptByItems(second, word => word, StringComparer.OrdinalIgnoreCase);
		result.AssertSequenceEqual("second", "fourth");
	}

	[Test]
	public void ExceptByNullComparer()
	{
		string[] first = { "aaa", "bb", "c", "dddd" };
		string[] second = { "xx", "y" };
		var result = first.ExceptByItems(second, x => x.Length, keyComparer: null);
		result.AssertSequenceEqual("aaa", "dddd");
	}

	[Test]
	public void ExceptByIsLazyWithComparer()
	{
		var bs = new BreakingSequence<string>();
		bs.ExceptByItems(bs, BreakingFunc.Of<string, string>(), StringComparer.Ordinal);
	}
}
