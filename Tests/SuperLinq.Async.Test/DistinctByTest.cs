namespace Test.Async;

public class DistinctByTest
{
	[Fact]
	public async Task DistinctBy()
	{
		await using var source = AsyncSeq("first", "second", "third", "fourth", "fifth")
			.AsTestingSequence();
		var distinct = source.DistinctBy(word => word.Length);
		await distinct.AssertSequenceEqual("first", "second");
	}

	[Fact]
	public void DistinctByIsLazy()
	{
		new AsyncBreakingSequence<string>().DistinctBy(BreakingFunc.Of<string, int>());
	}

	[Fact]
	public async Task DistinctByWithComparer()
	{
		await using var source = AsyncSeq("first", "FIRST", "second", "second", "third")
			.AsTestingSequence();
		var distinct = source.DistinctBy(word => word, StringComparer.OrdinalIgnoreCase);
		await distinct.AssertSequenceEqual("first", "second", "third");
	}

	[Fact]
	public async Task DistinctByNullComparer()
	{
		await using var source = AsyncSeq("first", "second", "third", "fourth", "fifth")
			.AsTestingSequence();
		var distinct = source.DistinctBy(word => word.Length, comparer: null);
		await distinct.AssertSequenceEqual("first", "second");
	}

	[Fact]
	public void DistinctByIsLazyWithComparer()
	{
		new AsyncBreakingSequence<string>().DistinctBy(BreakingFunc.Of<string, string>(), StringComparer.Ordinal);
	}
}
