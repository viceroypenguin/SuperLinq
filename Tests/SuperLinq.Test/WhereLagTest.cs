namespace Test;

public sealed class WhereLagTest
{
	[Fact]
	public void WhereLagIsLazy()
	{
		_ = new BreakingSequence<int>().WhereLag(5, BreakingFunc.Of<int, int, bool>());
		_ = new BreakingSequence<int>().WhereLag(5, -1, BreakingFunc.Of<int, int, bool>());
	}

	[Fact]
	public void WhereLagZeroOffset()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new BreakingSequence<int>().WhereLag(0, BreakingFunc.Of<int, int, bool>()));
	}

	[Fact]
	public void WhereLagExplicitDefaultValue()
	{
		using var seq = Enumerable.Range(1, 100).AsTestingSequence();
		var result = seq.WhereLag(3, -1, (val, lagVal) => lagVal % 10 == 4 || lagVal + val == 1);
		result.AssertSequenceEqual(
			2, 7, 17, 27, 37, 47, 57, 67, 77, 87, 97);
	}

	[Fact]
	public void WhereLagImplicitDefaultValue()
	{
		using var seq = Enumerable.Range(1, 100).AsTestingSequence();
		var result = seq.WhereLag(3, (val, lagVal) => lagVal % 10 == 4 || lagVal + val == 1);
		result.AssertSequenceEqual(
			1, 7, 17, 27, 37, 47, 57, 67, 77, 87, 97);
	}

	[Fact]
	public void WhereLagOffsetGreaterThanSequenceLength()
	{
		using var seq = Enumerable.Range(1, 100).AsTestingSequence();
		var result = seq.WhereLag(101, -1, (val, lagVal) => lagVal == -1);
		result.AssertSequenceEqual(Enumerable.Range(1, 100));
	}

	[Fact]
	public void WhereLagWithNullableReferences()
	{
		using var seq = Seq("foo", "bar", "baz", "qux").AsTestingSequence();
		var result = seq.WhereLag(2, (a, b) => (b ?? a) == "foo");
		result.AssertSequenceEqual("foo", "baz");
	}

	[Fact]
	public void WhereLagWithNonNullableReferences()
	{
		using var seq = Seq("foo", "bar", "baz", "qux").AsTestingSequence();
		var result = seq.WhereLag(2, string.Empty, (a, b) => b == "foo" || a == "foo");
		result.AssertSequenceEqual("foo", "baz");
	}
}
