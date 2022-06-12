using NUnit.Framework;

namespace SuperLinq.Test;

[TestFixture]
public class ExceptByTest
{
	[Test]
	public void SimpleExceptBy()
	{
		string[] first = { "aaa", "bb", "c", "dddd" };
		string[] second = { "xx", "y" };
		var result = first.ExceptBy(second, x => x.Length);
		result.AssertSequenceEqual("aaa", "dddd");
	}

	[Test]
	public void ExceptByIsLazy()
	{
		var bs = new BreakingSequence<string>();
		bs.ExceptBy(bs, BreakingFunc.Of<string, int>());
	}

	[Test]
	public void ExceptByDoesNotRepeatSourceElementsWithDuplicateKeys()
	{
		string[] first = { "aaa", "bb", "c", "a", "b", "c", "dddd" };
		string[] second = { "xx" };
		var result = first.ExceptBy(second, x => x.Length);
		result.AssertSequenceEqual("aaa", "c", "dddd");
	}

	[Test]
	public void ExceptByWithComparer()
	{
		string[] first = { "first", "second", "third", "fourth" };
		string[] second = { "FIRST", "thiRD", "FIFTH" };
		var result = first.ExceptBy(second, word => word, StringComparer.OrdinalIgnoreCase);
		result.AssertSequenceEqual("second", "fourth");
	}

	[Test]
	public void ExceptByNullComparer()
	{
		string[] first = { "aaa", "bb", "c", "dddd" };
		string[] second = { "xx", "y" };
		var result = first.ExceptBy(second, x => x.Length, null);
		result.AssertSequenceEqual("aaa", "dddd");
	}

	[Test]
	public void ExceptByIsLazyWithComparer()
	{
		var bs = new BreakingSequence<string>();
		bs.ExceptBy(bs, BreakingFunc.Of<string, string>(), StringComparer.Ordinal);
	}
}
