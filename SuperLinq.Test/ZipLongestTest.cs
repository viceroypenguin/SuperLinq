using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace SuperLinq.Test;
[TestFixture]
public class ZipLongestTest
{
	static IEnumerable<T> Seq<T>(params T[] values) => values;

	public static readonly IEnumerable<ITestCaseData> TestData =
		from e in new[]
		{
				new { A = Seq<int>(  ), B = Seq("foo", "bar", "baz"), Result = Seq((0, "foo"), (0, "bar"), (0, "baz")) },
				new { A = Seq(1      ), B = Seq("foo", "bar", "baz"), Result = Seq((1, "foo"), (0, "bar"), (0, "baz")) },
				new { A = Seq(1, 2   ), B = Seq("foo", "bar", "baz"), Result = Seq((1, "foo"), (2, "bar"), (0, "baz")) },
				new { A = Seq(1, 2, 3), B = Seq<string>(           ), Result = Seq((1, null ), (2, null ), (3, (string) null)) },
				new { A = Seq(1, 2, 3), B = Seq("foo"              ), Result = Seq((1, "foo"), (2, null ), (3, null )) },
				new { A = Seq(1, 2, 3), B = Seq("foo", "bar"       ), Result = Seq((1, "foo"), (2, "bar"), (3, null )) },
				new { A = Seq(1, 2, 3), B = Seq("foo", "bar", "baz"), Result = Seq((1, "foo"), (2, "bar"), (3, "baz")) },
		}
		select new TestCaseData(e.A, e.B)
			.Returns(e.Result);


	[Test, TestCaseSource(nameof(TestData))]
	public IEnumerable<(int, string)> ZipLongest(int[] first, string[] second)
	{
		using var ts1 = TestingSequence.Of(first);
		using var ts2 = TestingSequence.Of(second);
		return ts1.ZipLongest(ts2, ValueTuple.Create).ToArray();
	}

	[Test]
	public void ZipLongestIsLazy()
	{
		var bs = new BreakingSequence<int>();
		bs.ZipLongest(bs, BreakingFunc.Of<int, int, int>());
	}

	[Test]
	public void ZipLongestDisposeSequencesEagerly()
	{
		var shorter = TestingSequence.Of(1, 2, 3);
		var longer = SuperEnumerable.Generate(1, x => x + 1);
		var zipped = shorter.ZipLongest(longer, ValueTuple.Create);

		var count = 0;
		foreach (var _ in zipped.Take(10))
		{
			if (++count == 4)
				((IDisposable)shorter).Dispose();
		}
	}

	[Test]
	public void ZipLongestDisposesInnerSequencesCaseGetEnumeratorThrows()
	{
		using var s1 = TestingSequence.Of(1, 2);

		Assert.Throws<InvalidOperationException>(() =>
			s1.ZipLongest(new BreakingSequence<int>(), ValueTuple.Create).Consume());
	}
}
