namespace Test.Async;

public class DistinctByTest
{
	[Fact]
	public Task DistinctBy()
	{
		var source = AsyncSeq("first", "second", "third", "fourth", "fifth");
		var distinct = source.DistinctBy(word => word.Length);
		return distinct.AssertSequenceEqual("first", "second");
	}

	[Fact]
	public void DistinctByIsLazy()
	{
		new AsyncBreakingSequence<string>().DistinctBy(BreakingFunc.Of<string, int>());
	}

	[Fact]
	public Task DistinctByWithComparer()
	{
		var source = AsyncSeq("first", "FIRST", "second", "second", "third");
		var distinct = source.DistinctBy(word => word, StringComparer.OrdinalIgnoreCase);
		return distinct.AssertSequenceEqual("first", "second", "third");
	}

	[Fact]
	public Task DistinctByNullComparer()
	{
		var source = AsyncSeq("first", "second", "third", "fourth", "fifth");
		var distinct = source.DistinctBy(word => word.Length, comparer: null);
		return distinct.AssertSequenceEqual("first", "second");
	}

	[Fact]
	public void DistinctByIsLazyWithComparer()
	{
		new AsyncBreakingSequence<string>().DistinctBy(BreakingFunc.Of<string, string>(), StringComparer.Ordinal);
	}
}
