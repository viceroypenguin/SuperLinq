namespace SuperLinq.Async.Tests;

public sealed class IfTest
{
	[Test]
	public void IfIsLazy()
	{
		_ = AsyncSuperEnumerable.If(BreakingFunc.Of<bool>(), new AsyncBreakingSequence<int>());
		_ = AsyncSuperEnumerable.If(BreakingFunc.Of<ValueTask<bool>>(), new AsyncBreakingSequence<int>());
	}

	[Test]
	public async Task IfBehavior()
	{
		var starts = 0;
		await using var ts = Enumerable.Range(1, 10).AsTestingSequence();

		var seq = AsyncSuperEnumerable.If(
			() => starts++ == 0,
			ts);

		Assert.Equal(0, starts);
		await seq.AssertSequenceEqual(Enumerable.Range(1, 10));

		Assert.Equal(1, starts);
		await seq.AssertSequenceEqual();
	}

	[Test]
	public void IfElseIsLazy()
	{
		_ = AsyncSuperEnumerable.If(
			BreakingFunc.Of<bool>(),
			new AsyncBreakingSequence<int>(),
			new AsyncBreakingSequence<int>());
		_ = AsyncSuperEnumerable.If(
			BreakingFunc.Of<ValueTask<bool>>(),
			new AsyncBreakingSequence<int>(),
			new AsyncBreakingSequence<int>());
	}

	[Test]
	public async Task IfElseBehavior()
	{
		var starts = 0;
		await using var ts1 = Enumerable.Range(1, 10).AsTestingSequence();
		await using var ts2 = Enumerable.Range(1, 20).AsTestingSequence();

		var seq = AsyncSuperEnumerable.If(
			() => starts++ == 0,
			ts1,
			ts2);

		Assert.Equal(0, starts);
		await seq.AssertSequenceEqual(Enumerable.Range(1, 10));

		Assert.Equal(1, starts);
		await seq.AssertSequenceEqual(Enumerable.Range(1, 20));
	}
}
