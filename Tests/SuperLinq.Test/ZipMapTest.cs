using System.Text.RegularExpressions;

namespace Test;

public class ZipMapTest
{
	public static IEnumerable<object[]> GetIntSequences() =>
		Enumerable.Range(1, 10)
			.GetListSequences()
			.Select(x => new object[] { x });

	[Theory]
	[MemberData(nameof(GetIntSequences))]
	public void ZipMapIntTransformation(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.ZipMap(i => i.ToString());
			result
				.AssertSequenceEqual(Enumerable.Range(1, 10).Select(i => (i, i.ToString())));
		}
	}

	public static IEnumerable<object[]> GetStringSequences1()
	{
		var seq = Seq("foo", "bar", "FOO", "Bar", "baz", "QUx", "bAz", "QuX");
		return seq
			.GetListSequences()
			.Select(x => new object[] { x, seq, });
	}

	[Theory]
	[MemberData(nameof(GetStringSequences1))]
	public void ZipMapStringTransformation(IDisposableEnumerable<string> seq, IEnumerable<string> src)
	{
		using (seq)
		{
			var result = seq.ZipMap(s => s.Length);
			result.AssertSequenceEqual(
				src.Select(s => (s, s.Length)));
		}
	}

	public static IEnumerable<object[]> GetStringSequences2() =>
		Seq("foo", "hello", "world", "Bar", "QuX", "ay", "az")
			.GetListSequences()
			.Select(x => new object[] { x, });

	[Theory]
	[MemberData(nameof(GetStringSequences2))]
	public void ZipMapRegexChoose(IDisposableEnumerable<string> seq)
	{
		using (seq)
		{
			var result = seq
				.ZipMap(s => Regex.IsMatch(s, @"^\w{3}$"))
				.Choose(x => (x.result, x.item));

			result.AssertSequenceEqual("foo", "Bar", "QuX");
		}
	}

	[Fact]
	public void ZipMapIsLazy()
	{
		var bs = new BreakingSequence<int>();
		_ = bs.ZipMap(BreakingFunc.Of<int, int>());
	}
}
