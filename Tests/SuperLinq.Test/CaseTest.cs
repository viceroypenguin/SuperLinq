namespace Test;

public class CaseTest
{
	[Fact]
	public void CaseIsLazy()
	{
		_ = SuperEnumerable.Case(BreakingFunc.Of<int>(), new Dictionary<int, IEnumerable<int>>());
	}

	[Fact]
	public void CaseBehavior()
	{
		var starts = 0;
		using var ts = Enumerable.Range(1, 10).AsTestingSequence();
		var sources = new Dictionary<int, IEnumerable<int>>()
		{
			[0] = new BreakingSequence<int>(),
			[1] = ts,
		};

		var seq = SuperEnumerable.Case(
			() => starts++,
			sources);

		Assert.Equal(0, starts);
		_ = Assert.Throws<TestException>(() =>
			seq.Consume());

		Assert.Equal(1, starts);
		seq.AssertSequenceEqual(Enumerable.Range(1, 10));

		Assert.Equal(2, starts);
		seq.AssertSequenceEqual();
	}

	[Fact]
	public void CaseSourceIsLazy()
	{
		_ = SuperEnumerable.Case(BreakingFunc.Of<int>(), new Dictionary<int, IEnumerable<int>>(), new BreakingSequence<int>());
	}

	[Fact]
	public void CaseSourceBehavior()
	{
		var starts = 0;
		using var ts = Enumerable.Range(1, 10).AsTestingSequence();
		using var ts2 = Enumerable.Range(1, 20).AsTestingSequence();
		var sources = new Dictionary<int, IEnumerable<int>>()
		{
			[0] = new BreakingSequence<int>(),
			[1] = ts,
		};

		var seq = SuperEnumerable.Case(
			() => starts++,
			sources,
			ts2);

		Assert.Equal(0, starts);
		_ = Assert.Throws<TestException>(() =>
			seq.Consume());

		Assert.Equal(1, starts);
		seq.AssertSequenceEqual(Enumerable.Range(1, 10));

		Assert.Equal(2, starts);
		seq.AssertSequenceEqual(Enumerable.Range(1, 20));
	}
}
