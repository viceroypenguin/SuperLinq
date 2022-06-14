using NUnit.Framework;

namespace Test;

#pragma warning disable CS0618

[TestFixture]
public class DistinctByTest
{
	[Test]
	public void DistinctBy()
	{
		string[] source = { "first", "second", "third", "fourth", "fifth" };
		var distinct = SuperEnumerable.DistinctBy(source, word => word.Length);
		distinct.AssertSequenceEqual("first", "second");
	}

	[Test]
	public void DistinctByIsLazy()
	{
		SuperEnumerable.DistinctBy(new BreakingSequence<string>(), BreakingFunc.Of<string, int>());
	}

	[Test]
	public void DistinctByWithComparer()
	{
		string[] source = { "first", "FIRST", "second", "second", "third" };
		var distinct = SuperEnumerable.DistinctBy(source, word => word, StringComparer.OrdinalIgnoreCase);
		distinct.AssertSequenceEqual("first", "second", "third");
	}

	[Test]
	public void DistinctByNullComparer()
	{
		string[] source = { "first", "second", "third", "fourth", "fifth" };
		var distinct = SuperEnumerable.DistinctBy(source, word => word.Length, comparer: null);
		distinct.AssertSequenceEqual("first", "second");
	}

	[Test]
	public void DistinctByIsLazyWithComparer()
	{
		SuperEnumerable.DistinctBy(new BreakingSequence<string>(), BreakingFunc.Of<string, string>(), StringComparer.Ordinal);
	}
}
