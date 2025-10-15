namespace SuperLinq.Async.Tests;

public sealed class DoTest
{
	[Fact]
	public void DoOverloadsAreLazy()
	{
		_ = new AsyncBreakingSequence<int>().Do(BreakingAction.Of<int>());
		_ = new AsyncBreakingSequence<int>().Do(BreakingAction.Of<int>(), BreakingAction.Of());
		_ = new AsyncBreakingSequence<int>().Do(BreakingAction.Of<int>(), BreakingAction.Of<Exception>(), BreakingAction.Of());
	}

	[Fact]
	public async Task DoBehavior()
	{
		await using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		var count = 0;
		var result = seq.Do(i => count += i);

		await result.AssertSequenceEqual(Enumerable.Range(1, 10));
		Assert.Equal(55, count);
	}

	[Fact]
	public async Task DoCompletedBehavior()
	{
		await using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		var count = 0;
		var result = seq.Do(i => count += i, () => count += 10);

		await result.AssertSequenceEqual(Enumerable.Range(1, 10));
		Assert.Equal(65, count);
	}

	[Fact]
	public async Task DoBehaviorNoError()
	{
		await using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		var count = 0;
		var result = seq.Do(i => count += i, BreakingAction.Of<Exception>());

		await result.AssertSequenceEqual(Enumerable.Range(1, 10));
		Assert.Equal(55, count);
	}

	[Fact]
	public async Task DoCompletedBehaviorNoError()
	{
		await using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		var count = 0;
		var result = seq.Do(i => count += i, BreakingAction.Of<Exception>(), () => count += 10);

		await result.AssertSequenceEqual(Enumerable.Range(1, 10));
		Assert.Equal(65, count);
	}

	[Fact]
	public async Task DoBehaviorError()
	{
		await using var seq = Enumerable.Range(1, 10)
			.Concat(SuperEnumerable.From(BreakingFunc.Of<int>()))
			.AsTestingSequence();

		var count = 0;
		_ = await Assert.ThrowsAsync<TestException>(async () =>
			await seq
				.Do(
					i => count += i,
					ex => { _ = Assert.IsType<TestException>(ex); count += 100; })
				.Consume());

		Assert.Equal(155, count);
	}

	[Fact]
	public async Task DoCompletedBehaviorError()
	{
		await using var seq = Enumerable.Range(1, 10)
			.Concat(SuperEnumerable.From(BreakingFunc.Of<int>()))
			.AsTestingSequence();

		var count = 0;
		_ = await Assert.ThrowsAsync<TestException>(async () =>
			await seq
				.Do(
					i => count += i,
					ex => { _ = Assert.IsType<TestException>(ex); count += 100; },
					BreakingAction.Of())
				.Consume());

		Assert.Equal(155, count);
	}
}
