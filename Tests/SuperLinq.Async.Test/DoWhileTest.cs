namespace Test.Async;

public class DoWhileTest
{
	[Fact]
	public void DoWhileIsLazy()
	{
		_ = new AsyncBreakingSequence<int>().DoWhile(BreakingFunc.Of<bool>());
		_ = new AsyncBreakingSequence<int>().DoWhile(BreakingFunc.Of<ValueTask<bool>>());
	}

	[Fact]
	public async Task DoWhileBehavior()
	{
		await using var ts = Enumerable.Range(1, 10).AsTestingSequence();

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
		await seq.AssertSequenceEqual(
			Enumerable.Range(1, 10)
				.Concat(Enumerable.Range(1, 10))
				.Concat(Enumerable.Range(1, 10)));
		Assert.Equal(3, starts);
	}
}
