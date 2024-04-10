namespace Test.Async;

public sealed class DistinctByTest
{
	[Fact]
	public async Task DistinctBy()
	{
		await using var source = TestingSequence.Of("first", "second", "third", "fourth", "fifth");
		var distinct = source.DistinctBy(word => word.Length);
		await distinct.AssertSequenceEqual("first", "second");
	}

	[Fact]
	public void DistinctByIsLazy()
	{
		_ = new AsyncBreakingSequence<string>().DistinctBy(BreakingFunc.Of<string, int>());
	}

	[Fact]
	public async Task DistinctByWithComparer()
	{
		await using var source = TestingSequence.Of("first", "FIRST", "second", "second", "third");
		var distinct = source.DistinctBy(word => word, StringComparer.OrdinalIgnoreCase);
		await distinct.AssertSequenceEqual("first", "second", "third");
	}

	[Fact]
	public async Task DistinctByNullComparer()
	{
		await using var source = TestingSequence.Of("first", "second", "third", "fourth", "fifth");
		var distinct = source.DistinctBy(word => word.Length, comparer: null);
		await distinct.AssertSequenceEqual("first", "second");
	}

	[Fact]
	public void DistinctByIsLazyWithComparer()
	{
		_ = new AsyncBreakingSequence<string>().DistinctBy(BreakingFunc.Of<string, string>(), StringComparer.Ordinal);
	}
}
