namespace Test;
public class ZipLongestTest
{
	public static readonly IEnumerable<object[]> TestData =
		new[]
		{
			new object[] { Seq<int>(  ), Seq("foo", "bar", "baz"), Seq<(int, string?)>((0, "foo"), (0, "bar"), (0, "baz")) },
			new object[] { Seq(1      ), Seq("foo", "bar", "baz"), Seq<(int, string?)>((1, "foo"), (0, "bar"), (0, "baz")) },
			new object[] { Seq(1, 2   ), Seq("foo", "bar", "baz"), Seq<(int, string?)>((1, "foo"), (2, "bar"), (0, "baz")) },
			new object[] { Seq(1, 2, 3), Seq<string>(           ), Seq<(int, string?)>((1, null ), (2, null ), (3, null )) },
			new object[] { Seq(1, 2, 3), Seq("foo"              ), Seq<(int, string?)>((1, "foo"), (2, null ), (3, null )) },
			new object[] { Seq(1, 2, 3), Seq("foo", "bar"       ), Seq<(int, string?)>((1, "foo"), (2, "bar"), (3, null )) },
			new object[] { Seq(1, 2, 3), Seq("foo", "bar", "baz"), Seq<(int, string?)>((1, "foo"), (2, "bar"), (3, "baz")) },
		};


	[Theory]
	[MemberData(nameof(TestData))]
	public void ZipLongest(int[] first, string[] second, IEnumerable<(int, string?)> expected)
	{
		using var ts1 = TestingSequence.Of(first);
		using var ts2 = TestingSequence.Of(second);
		Assert.Equal(expected, ts1.ZipLongest(ts2, ValueTuple.Create).ToArray());
	}

	[Fact]
	public void ZipLongestIsLazy()
	{
		var bs = new BreakingSequence<int>();
		bs.ZipLongest(bs, BreakingFunc.Of<int, int, int>());
	}

	[Fact]
	public void ZipLongestDisposesInnerSequencesCaseGetEnumeratorThrows()
	{
		using var s1 = TestingSequence.Of(1, 2);

		Assert.Throws<TestException>(() =>
			s1.ZipLongest(new BreakingSequence<int>(), ValueTuple.Create).Consume());
	}
}
