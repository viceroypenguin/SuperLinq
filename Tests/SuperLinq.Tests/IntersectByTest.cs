namespace SuperLinq.Tests;

public sealed class IntersectByTest
{
	[Test]
	public void SimpleIntersectBy()
	{
		using var first = TestingSequence.Of("aaa", "bb", "c", "dddd");
		using var second = TestingSequence.Of("aaa", "bb");
		var result = first.IntersectBy(second, x => x.Length);
		result.AssertSequenceEqual("aaa", "bb");
	}

	[Test]
	public void IntersectByIsLazy()
	{
		var bs = new BreakingSequence<string>();
		_ = bs.IntersectBy(bs, BreakingFunc.Of<string, int>());
	}

	[Test]
	public void IntersectByDoesNotRepeatSourceElementsWithDuplicateKeys()
	{
		using var first = TestingSequence.Of("aaa", "bb", "c", "a", "b", "c", "dddd");
		using var second = TestingSequence.Of("aaa", "c", "dddd");
		var result = first.IntersectBy(second, x => x.Length);
		result.AssertSequenceEqual("aaa", "c", "dddd");
	}

	[Test]
	public void IntersectByWithComparer()
	{
		using var first = TestingSequence.Of("first", "second", "third", "fourth");
		using var second = TestingSequence.Of("FIRST", "thiRD", "FIFTH");
		var result = first.IntersectBy(second, StringComparer.OrdinalIgnoreCase.GetHashCode, EqualityComparer.Create<int>((x, y) => x == y));
		result.AssertSequenceEqual("first", "third");
	}

	[Test]
	public void IntersectByNullComparer()
	{
		using var first = TestingSequence.Of("aaa", "bb", "c", "dddd");
		using var second = TestingSequence.Of("aaa", "dddd");
		var result = first.IntersectBy(second, x => x.Length, keyComparer: null);
		result.AssertSequenceEqual("aaa", "dddd");
	}

	[Test]
	public void IntersectByIsLazyWithComparer()
	{
		var bs = new BreakingSequence<int>();
		_ = bs.IntersectBy(bs, BreakingFunc.Of<int, string>(), StringComparer.Ordinal);
	}
}
