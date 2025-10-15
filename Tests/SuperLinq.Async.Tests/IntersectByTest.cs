namespace SuperLinq.Async.Tests;

public sealed class IntersectByTest
{
	[Fact]
	public async Task SimpleIntersectBy()
	{
		using var first = TestingSequence.Of("aaa", "bb", "c", "dddd");
		using var second = TestingSequence.Of("aaa", "bb");
		var result = first.IntersectBy(second, x => x.Length);
		await result.AssertSequenceEqual("aaa", "bb");
	}

	[Fact]
	public void IntersectByIsLazy()
	{
		var bs = new AsyncBreakingSequence<string>();
		_ = bs.IntersectBy(bs, BreakingFunc.Of<string, int>());
	}

	[Fact]
	public async Task IntersectByDoesNotRepeatSourceElementsWithDuplicateKeys()
	{
		using var first = TestingSequence.Of("aaa", "bb", "c", "a", "b", "c", "dddd");
		using var second = TestingSequence.Of("aaa", "c", "dddd");
		var result = first.IntersectBy(second, x => x.Length);
		await result.AssertSequenceEqual("aaa", "c", "dddd");
	}

	[Fact]
	public async Task IntersectByWithComparer()
	{
		using var first = TestingSequence.Of("first", "second", "third", "fourth");
		using var second = TestingSequence.Of("FIRST", "thiRD", "FIFTH");
		var result = first.IntersectBy(second, StringComparer.OrdinalIgnoreCase.GetHashCode, EqualityComparer.Create<int>((x, y) => x == y));
		await result.AssertSequenceEqual("first", "third");
	}

	[Fact]
	public async Task IntersectByNullComparer()
	{
		using var first = TestingSequence.Of("aaa", "bb", "c", "dddd");
		using var second = TestingSequence.Of("aaa", "dddd");
		var result = first.IntersectBy(second, x => x.Length, keyComparer: null);
		await result.AssertSequenceEqual("aaa", "dddd");
	}

	[Fact]
	public void IntersectByIsLazyWithComparer()
	{
		var bs = new AsyncBreakingSequence<int>();
		_ = bs.IntersectBy(bs, BreakingFunc.Of<int, string>(), StringComparer.Ordinal);
	}
}
