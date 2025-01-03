namespace SuperLinq.Async.Tests;

public sealed class DistinctByTest
{
	[Test]
	public async Task DistinctBy()
	{
		await using var source = TestingSequence.Of("first", "second", "third", "fourth", "fifth");
		var distinct = source.DistinctBy(word => word.Length);
		await distinct.AssertSequenceEqual("first", "second");
	}

	[Test]
	public void DistinctByIsLazy()
	{
		_ = new AsyncBreakingSequence<string>().DistinctBy(BreakingFunc.Of<string, int>());
	}

	[Test]
	public async Task DistinctByWithComparer()
	{
		await using var source = TestingSequence.Of("first", "FIRST", "second", "second", "third");
		var distinct = source.DistinctBy(word => word, StringComparer.OrdinalIgnoreCase);
		await distinct.AssertSequenceEqual("first", "second", "third");
	}

	[Test]
	public async Task DistinctByNullComparer()
	{
		await using var source = TestingSequence.Of("first", "second", "third", "fourth", "fifth");
		var distinct = source.DistinctBy(word => word.Length, comparer: null);
		await distinct.AssertSequenceEqual("first", "second");
	}

	[Test]
	public void DistinctByIsLazyWithComparer()
	{
		_ = new AsyncBreakingSequence<string>().DistinctBy(BreakingFunc.Of<string, string>(), StringComparer.Ordinal);
	}
}
