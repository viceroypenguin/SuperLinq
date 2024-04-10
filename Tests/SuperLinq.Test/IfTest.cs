namespace Test;

public sealed class IfTest
{
	[Fact]
	public void IfIsLazy()
	{
		_ = SuperEnumerable.If(BreakingFunc.Of<bool>(), new BreakingSequence<int>());
	}

	[Fact]
	public void IfBehavior()
	{
		var starts = 0;
		using var ts = Enumerable.Range(1, 10).AsTestingSequence();

		var seq = SuperEnumerable.If(
			() => starts++ == 0,
			ts);

		Assert.Equal(0, starts);
		seq.AssertSequenceEqual(Enumerable.Range(1, 10));

		Assert.Equal(1, starts);
		seq.AssertSequenceEqual();
	}

	[Fact]
	public void IfElseIsLazy()
	{
		_ = SuperEnumerable.If(
			BreakingFunc.Of<bool>(),
			new BreakingSequence<int>(),
			new BreakingSequence<int>());
	}

	[Fact]
	public void CaseSourceBehavior()
	{
		var starts = 0;
		using var ts1 = Enumerable.Range(1, 10).AsTestingSequence();
		using var ts2 = Enumerable.Range(1, 20).AsTestingSequence();

		var seq = SuperEnumerable.If(
			() => starts++ == 0,
			ts1,
			ts2);

		Assert.Equal(0, starts);
		seq.AssertSequenceEqual(Enumerable.Range(1, 10));

		Assert.Equal(1, starts);
		seq.AssertSequenceEqual(Enumerable.Range(1, 20));
	}
}
