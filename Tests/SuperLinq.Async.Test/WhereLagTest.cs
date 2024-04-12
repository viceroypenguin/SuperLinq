namespace Test.Async;

public sealed class WhereLagTest
{
	[Fact]
	public void WhereLagIsLazy()
	{
		_ = new AsyncBreakingSequence<int>().WhereLag(5, BreakingFunc.Of<int, int, bool>());
		_ = new AsyncBreakingSequence<int>().WhereLag(5, BreakingFunc.Of<int, int, ValueTask<bool>>());
		_ = new AsyncBreakingSequence<int>().WhereLag(5, -1, BreakingFunc.Of<int, int, bool>());
		_ = new AsyncBreakingSequence<int>().WhereLag(5, -1, BreakingFunc.Of<int, int, ValueTask<bool>>());
	}

	[Fact]
	public void WhereLagZeroOffset()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new AsyncBreakingSequence<int>().WhereLag(0, BreakingFunc.Of<int, int, bool>()));
	}

	[Fact]
	public async Task WhereLagExplicitDefaultValue()
	{
		await using var seq = Enumerable.Range(1, 100).AsTestingSequence();
		var result = seq.WhereLag(3, -1, (val, lagVal) => lagVal % 10 == 4 || lagVal + val == 1);
		await result.AssertSequenceEqual(
			2, 7, 17, 27, 37, 47, 57, 67, 77, 87, 97);
	}

	[Fact]
	public async Task WhereLagImplicitDefaultValue()
	{
		await using var seq = Enumerable.Range(1, 100).AsTestingSequence();
		var result = seq.WhereLag(3, (val, lagVal) => lagVal % 10 == 4 || lagVal + val == 1);
		await result.AssertSequenceEqual(
			1, 7, 17, 27, 37, 47, 57, 67, 77, 87, 97);
	}

	[Fact]
	public async Task WhereLagOffsetGreaterThanSequenceLength()
	{
		await using var seq = Enumerable.Range(1, 100).AsTestingSequence();
		var result = seq.WhereLag(101, -1, (val, lagVal) => lagVal == -1);
		await result.AssertSequenceEqual(Enumerable.Range(1, 100));
	}

	[Fact]
	public async Task WhereLagWithNullableReferences()
	{
		await using var seq = Seq("foo", "bar", "baz", "qux").AsTestingSequence();
		var result = seq.WhereLag(2, (a, b) => (b ?? a) == "foo");
		await result.AssertSequenceEqual("foo", "baz");
	}

	[Fact]
	public async Task WhereLagWithNonNullableReferences()
	{
		await using var seq = Seq("foo", "bar", "baz", "qux").AsTestingSequence();
		var result = seq.WhereLag(2, "", (a, b) => b == "foo" || a == "foo");
		await result.AssertSequenceEqual("foo", "baz");
	}
}
