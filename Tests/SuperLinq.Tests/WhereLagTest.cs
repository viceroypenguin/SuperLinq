namespace SuperLinq.Tests;

public sealed class WhereLagTest
{
	[Test]
	public void WhereLagIsLazy()
	{
		_ = new BreakingSequence<int>().WhereLag(5, BreakingFunc.Of<int, int, bool>());
		_ = new BreakingSequence<int>().WhereLag(5, -1, BreakingFunc.Of<int, int, bool>());
	}

	[Test]
	public void WhereLagZeroOffset()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new BreakingSequence<int>().WhereLag(0, BreakingFunc.Of<int, int, bool>()));
	}

	[Test]
	public void WhereLagExplicitDefaultValue()
	{
		using var seq = Enumerable.Range(1, 100).AsTestingSequence();
		var result = seq.WhereLag(3, -1, (val, lagVal) => lagVal % 10 == 4 || lagVal + val == 1);
		result.AssertSequenceEqual(
			2, 7, 17, 27, 37, 47, 57, 67, 77, 87, 97);
	}

	[Test]
	public void WhereLagImplicitDefaultValue()
	{
		using var seq = Enumerable.Range(1, 100).AsTestingSequence();
		var result = seq.WhereLag(3, (val, lagVal) => lagVal % 10 == 4 || lagVal + val == 1);
		result.AssertSequenceEqual(
			1, 7, 17, 27, 37, 47, 57, 67, 77, 87, 97);
	}

	[Test]
	public void WhereLagOffsetGreaterThanSequenceLength()
	{
		using var seq = Enumerable.Range(1, 100).AsTestingSequence();
		var result = seq.WhereLag(101, -1, (val, lagVal) => lagVal == -1);
		result.AssertSequenceEqual(Enumerable.Range(1, 100));
	}

	[Test]
	public void WhereLagWithNullableReferences()
	{
		using var seq = Seq("foo", "bar", "baz", "qux").AsTestingSequence();
		var result = seq.WhereLag(2, (a, b) => (b ?? a) == "foo");
		result.AssertSequenceEqual("foo", "baz");
	}

	[Test]
	public void WhereLagWithNonNullableReferences()
	{
		using var seq = Seq("foo", "bar", "baz", "qux").AsTestingSequence();
		var result = seq.WhereLag(2, "", (a, b) => b == "foo" || a == "foo");
		result.AssertSequenceEqual("foo", "baz");
	}
}
