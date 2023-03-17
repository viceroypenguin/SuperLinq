namespace Test.Async;

public class CaseTest
{
	[Fact]
	public void CaseIsLazy()
	{
		_ = AsyncSuperEnumerable.Case(BreakingFunc.Of<int>(), new Dictionary<int, IAsyncEnumerable<int>>());
		_ = AsyncSuperEnumerable.Case(BreakingFunc.Of<ValueTask<int>>(), new Dictionary<int, IAsyncEnumerable<int>>());
	}

	[Fact]
	public async Task CaseBehavior()
	{
		var starts = 0;
		await using var ts = Enumerable.Range(1, 10).AsTestingSequence();
		var sources = new Dictionary<int, IAsyncEnumerable<int>>()
		{
			[0] = new AsyncBreakingSequence<int>(),
			[1] = ts,
		};

		var seq = AsyncSuperEnumerable.Case(
			() => starts++,
			sources);

		Assert.Equal(0, starts);
		_ = await Assert.ThrowsAsync<TestException>(async () =>
			await seq.Consume());

		Assert.Equal(1, starts);
		await seq.AssertSequenceEqual(Enumerable.Range(1, 10));

		Assert.Equal(2, starts);
		await seq.AssertSequenceEqual();
	}

	[Fact]
	public void CaseSourceIsLazy()
	{
		_ = AsyncSuperEnumerable.Case(BreakingFunc.Of<int>(), new Dictionary<int, IAsyncEnumerable<int>>(), new AsyncBreakingSequence<int>());
		_ = AsyncSuperEnumerable.Case(BreakingFunc.Of<ValueTask<int>>(), new Dictionary<int, IAsyncEnumerable<int>>(), new AsyncBreakingSequence<int>());
	}

	[Fact]
	public async Task CaseSourceBehavior()
	{
		var starts = 0;
		await using var ts = Enumerable.Range(1, 10).AsTestingSequence();
		await using var ts2 = Enumerable.Range(1, 20).AsTestingSequence();
		var sources = new Dictionary<int, IAsyncEnumerable<int>>()
		{
			[0] = new AsyncBreakingSequence<int>(),
			[1] = ts,
		};

		var seq = AsyncSuperEnumerable.Case(
			() => starts++,
			sources,
			ts2);

		Assert.Equal(0, starts);
		_ = await Assert.ThrowsAsync<TestException>(async () =>
			await seq.Consume());

		Assert.Equal(1, starts);
		await seq.AssertSequenceEqual(Enumerable.Range(1, 10));

		Assert.Equal(2, starts);
		await seq.AssertSequenceEqual(Enumerable.Range(1, 20));
	}
}
