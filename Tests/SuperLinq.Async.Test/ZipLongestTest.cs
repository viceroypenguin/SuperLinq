namespace Test.Async;
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
	public async Task ZipLongest(int[] first, string[] second, IEnumerable<(int, string?)> expected)
	{
		await using var ts1 = TestingSequence.Of(first);
		await using var ts2 = TestingSequence.Of(second);
		await ts1.ZipLongest(ts2, ValueTuple.Create).AssertSequenceEqual(expected);
	}

	[Fact]
	public void ZipLongestIsLazy()
	{
		var bs = new AsyncBreakingSequence<int>();
		bs.ZipLongest(bs, BreakingFunc.Of<int, int, int>());
	}

	[Fact]
	public async Task ZipLongestDisposesInnerSequencesCaseGetEnumeratorThrows()
	{
		await using var s1 = TestingSequence.Of(1, 2);

		await Assert.ThrowsAsync<NotSupportedException>(async () =>
			await s1.ZipLongest(new AsyncBreakingSequence<int>(), ValueTuple.Create).Consume());
	}
}
