using System.Text.RegularExpressions;

namespace Test.Async;

public class ZipMapTest
{
	[Fact]
	public async Task ZipMapIntTransformation()
	{
		var range = Enumerable.Range(1, 10);
		await using var ts1 = range.AsTestingSequence();
		await ts1.ZipMap(i => i.ToString()).AssertSequenceEqual(
			range.Select(i => (i, i.ToString())));
	}

	[Fact]
	public async Task ZipMapStringTransformation()
	{
		var words = Seq("foo", "bar", "FOO", "Bar", "baz", "QUx", "bAz", "QuX");
		await using var ts1 = words.AsTestingSequence();

		await ts1.ZipMap(s => s.Length).AssertSequenceEqual(
			words.Select(s => (s, s.Length)));
	}

	[Fact]
	public async Task ZipMapRegexChoose()
	{
		var words = Seq("foo", "hello", "world", "Bar", "QuX", "ay", "az");
		await using var ts1 = words.AsTestingSequence();

		await ts1.ZipMap(s => Regex.IsMatch(s, @"^\w{3}$"))
			.Choose(x => (x.result, x.item))
			.AssertSequenceEqual("foo", "Bar", "QuX");
	}

	[Fact]
	public void ZipMapIsLazy()
	{
		var bs = new AsyncBreakingSequence<int>();
		_ = bs.ZipMap(BreakingFunc.Of<int, int>());
	}
}
