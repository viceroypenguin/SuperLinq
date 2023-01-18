namespace Test;

#pragma warning disable CS0618

public class ExceptByTest
{
	[Fact]
	public void SimpleExceptBy()
	{
		using var first = Seq("aaa", "bb", "c", "dddd").AsTestingSequence();
		using var second = Seq("xx", "y").AsTestingSequence();
		var result = SuperEnumerable.ExceptBy(first, second, x => x.Length);
		result.AssertSequenceEqual("aaa", "dddd");
	}

	[Fact]
	public void ExceptByIsLazy()
	{
		var bs = new BreakingSequence<string>();
		SuperEnumerable.ExceptBy(bs, bs, BreakingFunc.Of<string, int>());
	}

	[Fact]
	public void ExceptByDoesNotRepeatSourceElementsWithDuplicateKeys()
	{
		using var first = Seq("aaa", "bb", "c", "a", "b", "c", "dddd").AsTestingSequence();
		using var second = Seq("xx").AsTestingSequence();
		var result = SuperEnumerable.ExceptBy(first, second, x => x.Length);
		result.AssertSequenceEqual("aaa", "c", "dddd");
	}

	[Fact]
	public void ExceptByWithComparer()
	{
		using var first = Seq("first", "second", "third", "fourth").AsTestingSequence();
		using var second = Seq("FIRST", "thiRD", "FIFTH").AsTestingSequence();
		var result = SuperEnumerable.ExceptBy(first, second, word => word, StringComparer.OrdinalIgnoreCase);
		result.AssertSequenceEqual("second", "fourth");
	}

	[Fact]
	public void ExceptByNullComparer()
	{
		using var first = Seq("aaa", "bb", "c", "dddd").AsTestingSequence();
		using var second = Seq("xx", "y").AsTestingSequence();
		var result = SuperEnumerable.ExceptBy(first, second, x => x.Length, keyComparer: null);
		result.AssertSequenceEqual("aaa", "dddd");
	}

	[Fact]
	public void ExceptByIsLazyWithComparer()
	{
		var bs = new BreakingSequence<string>();
		SuperEnumerable.ExceptBy(bs, bs, BreakingFunc.Of<string, string>(), StringComparer.Ordinal);
	}
}

#pragma warning restore CS0618 // Type or member is obsolete
