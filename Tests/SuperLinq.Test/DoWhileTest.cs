namespace Test;

public sealed class DoWhileTest
{
	[Fact]
	public void DoWhileIsLazy()
	{
		_ = new BreakingSequence<int>().DoWhile(BreakingFunc.Of<bool>());
	}

	[Fact]
	public void DoWhileBehavior()
	{
		using var ts = Enumerable.Range(1, 10).AsTestingSequence();

		var starts = 0;
		var seq = ts
			.DoWhile(
				() =>
					starts++ switch
					{
						0 or 1 => true,
						2 => false,
						_ => throw new TestException(),
					});

		Assert.Equal(0, starts);
		seq.AssertSequenceEqual(
			Enumerable.Range(1, 10)
				.Concat(Enumerable.Range(1, 10))
				.Concat(Enumerable.Range(1, 10)));
		Assert.Equal(3, starts);
	}
}
