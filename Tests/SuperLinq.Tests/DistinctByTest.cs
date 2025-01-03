namespace SuperLinq.Tests;

[Obsolete("References `DistinctBy` which is obsolete in net6+")]
public sealed class DistinctByTest
{
	[Test]
	public void DistinctBy()
	{
		using var source = TestingSequence.Of("first", "second", "third", "fourth", "fifth");
		var distinct = SuperEnumerable.DistinctBy(source, word => word.Length);
		distinct.AssertSequenceEqual("first", "second");
	}

	[Test]
	public void DistinctByIsLazy()
	{
		_ = SuperEnumerable.DistinctBy(new BreakingSequence<string>(), BreakingFunc.Of<string, int>());
	}

	[Test]
	public void DistinctByWithComparer()
	{
		using var source = TestingSequence.Of("first", "FIRST", "second", "second", "third");
		var distinct = SuperEnumerable.DistinctBy(source, word => word, StringComparer.OrdinalIgnoreCase);
		distinct.AssertSequenceEqual("first", "second", "third");
	}

	[Test]
	public void DistinctByNullComparer()
	{
		using var source = TestingSequence.Of("first", "second", "third", "fourth", "fifth");
		var distinct = SuperEnumerable.DistinctBy(source, word => word.Length, comparer: null);
		distinct.AssertSequenceEqual("first", "second");
	}

	[Test]
	public void DistinctByIsLazyWithComparer()
	{
		_ = SuperEnumerable.DistinctBy(new BreakingSequence<string>(), BreakingFunc.Of<string, string>(), StringComparer.Ordinal);
	}
}
