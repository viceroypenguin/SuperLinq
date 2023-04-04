namespace Test.Async;

public class WhileTest
{
	[Fact]
	public void WhileIsLazy()
	{
		_ = AsyncSuperEnumerable.While(
				BreakingFunc.Of<bool>(),
				new AsyncBreakingSequence<int>());
		_ = AsyncSuperEnumerable.While(
				BreakingFunc.Of<ValueTask<bool>>(),
				new AsyncBreakingSequence<int>());
	}

	[Fact]
	public async Task WhileBehavior()
	{
		var starts = 0;
		var seq = AsyncSuperEnumerable.While(
			() =>
				starts++ switch
				{
					0 or 1 => true,
					2 => false,
					_ => throw new TestException(),
				},
			AsyncEnumerable.Range(1, 10));

		Assert.Equal(0, starts);
		await seq.AssertSequenceEqual(
			Enumerable.Range(1, 10).Concat(Enumerable.Range(1, 10)));
		Assert.Equal(3, starts);
	}
}
