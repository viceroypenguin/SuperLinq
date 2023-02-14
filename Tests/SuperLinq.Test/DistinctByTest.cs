namespace Test;

#pragma warning disable CS0618

public class DistinctByTest
{
	[Fact]
	public void DistinctBy()
	{
		using var source = TestingSequence.Of("first", "second", "third", "fourth", "fifth");
		var distinct = SuperEnumerable.DistinctBy(source, word => word.Length);
		distinct.AssertSequenceEqual("first", "second");
	}

	[Fact]
	public void DistinctByIsLazy()
	{
		_ = SuperEnumerable.DistinctBy(new BreakingSequence<string>(), BreakingFunc.Of<string, int>());
	}

	[Fact]
	public void DistinctByWithComparer()
	{
		using var source = TestingSequence.Of("first", "FIRST", "second", "second", "third");
		var distinct = SuperEnumerable.DistinctBy(source, word => word, StringComparer.OrdinalIgnoreCase);
		distinct.AssertSequenceEqual("first", "second", "third");
	}

	[Fact]
	public void DistinctByNullComparer()
	{
		using var source = TestingSequence.Of("first", "second", "third", "fourth", "fifth");
		var distinct = SuperEnumerable.DistinctBy(source, word => word.Length, comparer: null);
		distinct.AssertSequenceEqual("first", "second");
	}

	[Fact]
	public void DistinctByIsLazyWithComparer()
	{
		_ = SuperEnumerable.DistinctBy(new BreakingSequence<string>(), BreakingFunc.Of<string, string>(), StringComparer.Ordinal);
	}
}

#pragma warning restore CS0618 // Type or member is obsolete
