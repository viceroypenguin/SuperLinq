namespace Test.Async;

public sealed class WhereLeadTest
{
	[Fact]
	public void WhereLeadIsLazy()
	{
		_ = new AsyncBreakingSequence<int>().WhereLead(5, BreakingFunc.Of<int, int, bool>());
		_ = new AsyncBreakingSequence<int>().WhereLead(5, BreakingFunc.Of<int, int, ValueTask<bool>>());
		_ = new AsyncBreakingSequence<int>().WhereLead(5, -1, BreakingFunc.Of<int, int, bool>());
		_ = new AsyncBreakingSequence<int>().WhereLead(5, -1, BreakingFunc.Of<int, int, ValueTask<bool>>());
	}

	[Fact]
	public void WhereLeadZeroOffset()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new AsyncBreakingSequence<int>().WhereLead(0, BreakingFunc.Of<int, int, bool>()));
	}

	[Fact]
	public async Task WhereLeadExplicitDefaultValue()
	{
		await using var seq = Enumerable.Range(1, 100).AsTestingSequence();
		var result = seq.WhereLead(3, -1, (val, leadVal) => leadVal % 10 == 0 || leadVal + val == 99);
		await result.AssertSequenceEqual(
			7, 17, 27, 37, 47, 48, 57, 67, 77, 87, 97, 100);
	}

	[Fact]
	public async Task WhereLeadImplicitDefaultValue()
	{
		await using var seq = Enumerable.Range(1, 100).AsTestingSequence();
		var result = seq.WhereLead(3, (val, leadVal) => leadVal % 10 == 2 || leadVal + val == 99);
		await result.AssertSequenceEqual(
			9, 19, 29, 39, 48, 49, 59, 69, 79, 89, 99);
	}

	[Fact]
	public async Task WhereLeadOffsetGreaterThanSequenceLength()
	{
		await using var seq = Enumerable.Range(1, 100).AsTestingSequence();
		var result = seq.WhereLead(101, -1, (val, leadVal) => leadVal == -1);
		await result.AssertSequenceEqual(Enumerable.Range(1, 100));
	}

	[Fact]
	public async Task WhereLeadWithNullableReferences()
	{
		await using var seq = Seq("foo", "bar", "baz", "qux").AsTestingSequence();
		var result = seq.WhereLead(2, (a, b) => (b ?? a) == "baz");
		await result.AssertSequenceEqual("foo", "baz");
	}

	[Fact]
	public async Task WhereLeadWithNonNullableReferences()
	{
		await using var seq = Seq("foo", "bar", "baz", "qux").AsTestingSequence();
		var result = seq.WhereLead(2, string.Empty, (a, b) => b == "baz" || a == "baz");
		await result.AssertSequenceEqual("foo", "baz");
	}
}
