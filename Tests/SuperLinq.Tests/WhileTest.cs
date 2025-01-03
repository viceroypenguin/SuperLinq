namespace SuperLinq.Tests;

public sealed class WhileTest
{
	[Test]
	public void WhileIsLazy()
	{
		_ = SuperEnumerable.While(
			BreakingFunc.Of<bool>(),
			new BreakingSequence<int>());
	}

	[Test]
	public void WhileBehavior()
	{
		var starts = 0;
		var seq = SuperEnumerable.While(
			() =>
				starts++ switch
				{
					0 or 1 => true,
					2 => false,
					_ => throw new TestException(),
				},
			Enumerable.Range(1, 10));

		Assert.Equal(0, starts);
		seq.AssertSequenceEqual(
			Enumerable.Range(1, 10).Concat(Enumerable.Range(1, 10)));
		Assert.Equal(3, starts);
	}
}
