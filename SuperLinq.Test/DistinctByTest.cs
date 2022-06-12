using NUnit.Framework;

namespace SuperLinq.Test;

[TestFixture]
public class DistinctByTest
{
	[Test]
	public void DistinctBy()
	{
		string[] source = { "first", "second", "third", "fourth", "fifth" };
		var distinct = source.DistinctBy(word => word.Length);
		distinct.AssertSequenceEqual("first", "second");
	}

	[Test]
	public void DistinctByIsLazy()
	{
		new BreakingSequence<string>().DistinctBy(BreakingFunc.Of<string, int>());
	}

	[Test]
	public void DistinctByWithComparer()
	{
		string[] source = { "first", "FIRST", "second", "second", "third" };
		var distinct = source.DistinctBy(word => word, StringComparer.OrdinalIgnoreCase);
		distinct.AssertSequenceEqual("first", "second", "third");
	}

	[Test]
	public void DistinctByNullComparer()
	{
		string[] source = { "first", "second", "third", "fourth", "fifth" };
		var distinct = source.DistinctBy(word => word.Length, null);
		distinct.AssertSequenceEqual("first", "second");
	}

	[Test]
	public void DistinctByIsLazyWithComparer()
	{
		new BreakingSequence<string>()
			.DistinctBy(BreakingFunc.Of<string, string>(), StringComparer.Ordinal);
	}
}
