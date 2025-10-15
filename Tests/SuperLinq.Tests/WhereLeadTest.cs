namespace SuperLinq.Tests;

public sealed class WhereLeadTest
{
	[Fact]
	public void WhereLeadIsLazy()
	{
		_ = new BreakingSequence<int>().WhereLead(5, BreakingFunc.Of<int, int, bool>());
		_ = new BreakingSequence<int>().WhereLead(5, -1, BreakingFunc.Of<int, int, bool>());
	}

	[Fact]
	public void WhereLeadZeroOffset()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new BreakingSequence<int>().WhereLead(0, BreakingFunc.Of<int, int, bool>()));
	}

	[Fact]
	public void WhereLeadExplicitDefaultValue()
	{
		using var seq = Enumerable.Range(1, 100).AsTestingSequence();
		var result = seq.WhereLead(3, -1, (val, leadVal) => leadVal % 10 == 0 || leadVal + val == 99);
		result.AssertSequenceEqual(
			7, 17, 27, 37, 47, 48, 57, 67, 77, 87, 97, 100);
	}

	[Fact]
	public void WhereLeadImplicitDefaultValue()
	{
		using var seq = Enumerable.Range(1, 100).AsTestingSequence();
		var result = seq.WhereLead(3, (val, leadVal) => leadVal % 10 == 2 || leadVal + val == 99);
		result.AssertSequenceEqual(
			9, 19, 29, 39, 48, 49, 59, 69, 79, 89, 99);
	}

	[Fact]
	public void WhereLeadOffsetGreaterThanSequenceLength()
	{
		using var seq = Enumerable.Range(1, 100).AsTestingSequence();
		var result = seq.WhereLead(101, -1, (val, leadVal) => leadVal == -1);
		result.AssertSequenceEqual(Enumerable.Range(1, 100));
	}

	[Fact]
	public void WhereLeadWithNullableReferences()
	{
		using var seq = Seq("foo", "bar", "baz", "qux").AsTestingSequence();
		var result = seq.WhereLead(2, (a, b) => (b ?? a) is "baz");
		result.AssertSequenceEqual("foo", "baz");
	}

	[Fact]
	public void WhereLeadWithNonNullableReferences()
	{
		using var seq = Seq("foo", "bar", "baz", "qux").AsTestingSequence();
		var result = seq.WhereLead(2, "", (a, b) => b is "baz" || a is "baz");
		result.AssertSequenceEqual("foo", "baz");
	}
}
