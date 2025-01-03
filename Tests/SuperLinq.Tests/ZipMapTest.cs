using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SuperLinq.Tests;

public sealed class ZipMapTest
{
	public static IEnumerable<IDisposableEnumerable<int>> GetIntSequences() =>
		Enumerable.Range(1, 10)
			.GetListSequences();

	[Test]
	[MethodDataSource(nameof(GetIntSequences))]
	public void ZipMapIntTransformation(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.ZipMap(i => i.ToString(CultureInfo.InvariantCulture));
			result
				.AssertSequenceEqual(Enumerable.Range(1, 10).Select(i => (i, i.ToString(CultureInfo.InvariantCulture))));
		}
	}

	public static IEnumerable<(IDisposableEnumerable<string> seq, IEnumerable<string> src)> GetStringSequences1()
	{
		var seq = Seq("foo", "bar", "FOO", "Bar", "baz", "QUx", "bAz", "QuX");
		return seq
			.GetListSequences()
			.Select(x => (x, seq));
	}

	[Test]
	[MethodDataSource(nameof(GetStringSequences1))]
	public void ZipMapStringTransformation(IDisposableEnumerable<string> seq, IEnumerable<string> src)
	{
		using (seq)
		{
			var result = seq.ZipMap(s => s.Length);
			result.AssertSequenceEqual(
				src.Select(s => (s, s.Length)));
		}
	}

	[Test]
	[MethodDataSource(
		typeof(TestExtensions),
		nameof(TestExtensions.GetListSequences),
		Arguments = [new[] { "foo", "hello", "world", "Bar", "QuX", "ay", "az" }]
	)]
	[SuppressMessage("Usage", "TUnit0001:Invalid Data for Tests")]
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

	[Test]
	public void ZipMapListBehavior()
	{
		using var seq = Enumerable.Range(0, 10_000).AsBreakingList();

		var result = seq.ZipMap(a => a + 10);
		result.AssertCollectionErrorChecking(10_000);
		result.AssertListElementChecking(10_000);

		Assert.Equal((20, 30), result.ElementAt(20));
#if !NO_INDEX
		Assert.Equal((9_980, 9_990), result.ElementAt(^20));
#endif
	}

	[Test]
	public void ZipMapIsLazy()
	{
		var bs = new BreakingSequence<int>();
		_ = bs.ZipMap(BreakingFunc.Of<int, int>());
	}
}
