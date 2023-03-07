namespace Test;

public class DoWhileTest
{
	[Fact]
	public void DoWhileIsLazy()
	{
		_ = new BreakingSequence<int>().DoWhile(BreakingFunc.Of<bool>());
	}

	[Fact]
	public void DoWhileBehavior()
	{
		var starts = 0;
		var seq = Enumerable.Range(1, 10)
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
