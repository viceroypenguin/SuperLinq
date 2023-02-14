using System.Text.RegularExpressions;

namespace Test;

public class ZipMapTest
{
	[Fact]
	public void ZipMapIntTransformation()
	{
		var range = Enumerable.Range(1, 10);
		using var ts1 = range.AsTestingSequence();
		ts1.ZipMap(i => i.ToString()).AssertSequenceEqual(
			range.Select(i => (i, i.ToString())));
	}

	[Fact]
	public void ZipMapStringTransformation()
	{
		var words = Seq("foo", "bar", "FOO", "Bar", "baz", "QUx", "bAz", "QuX");
		using var ts1 = words.AsTestingSequence();

		ts1.ZipMap(s => s.Length).AssertSequenceEqual(
			words.Select(s => (s, s.Length)));
	}

	[Fact]
	public void ZipMapRegexChoose()
	{
		var words = Seq("foo", "hello", "world", "Bar", "QuX", "ay", "az");
		using var ts1 = words.AsTestingSequence();

		ts1.ZipMap(s => Regex.IsMatch(s, @"^\w{3}$"))
			.Choose(x => (x.result, x.item))
			.AssertSequenceEqual("foo", "Bar", "QuX");
	}

	[Fact]
	public void ZipMapIsLazy()
	{
		var bs = new BreakingSequence<int>();
		_ = bs.ZipMap(BreakingFunc.Of<int, int>());
	}
}
