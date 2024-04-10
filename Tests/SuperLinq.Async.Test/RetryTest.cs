namespace Test.Async;

public sealed class RetryTest
{
	[Fact]
	public void RetryIsLazy()
	{
		_ = new AsyncBreakingSequence<int>().Retry();
	}

	[Fact]
	public async Task RetryNoExceptions()
	{
		await using var ts = Enumerable.Range(1, 10).AsTestingSequence();

		var result = ts.Retry();
		await result.AssertSequenceEqual(Enumerable.Range(1, 10));
	}

	[Fact]
	public async Task RetryWithExceptions()
	{
		await using var ts1 = AsyncSeqExceptionAt(2).AsTestingSequence();
		await using var ts2 = AsyncSeqExceptionAt(5).AsTestingSequence();
		await using var ts3 = Enumerable.Range(1, 10).AsTestingSequence();

		var starts = 0;
		var seq = AsyncSuperEnumerable.Case(
			() => starts++,
			new Dictionary<int, IAsyncEnumerable<int>>()
			{
				[0] = ts1,
				[1] = ts2,
				[2] = ts3,
			});
		using var ts = seq.AsTestingSequence(maxEnumerations: 3);

		var result = ts.Retry();
		await result.AssertSequenceEqual(
			Enumerable.Range(1, 1)
				.Concat(Enumerable.Range(1, 4))
				.Concat(Enumerable.Range(1, 10)));
	}

	[Fact]
	public void RetryCountIsLazy()
	{
		_ = new AsyncBreakingSequence<int>().Retry(3);
	}

	[Fact]
	public async Task RetryCountNoExceptions()
	{
		await using var ts = Enumerable.Range(1, 10).AsTestingSequence();

		var result = ts.Retry(3);
		await result.AssertSequenceEqual(Enumerable.Range(1, 10));
	}

	[Fact]
	public async Task RetryCountWithExceptionsComplete()
	{
		await using var ts1 = AsyncSeqExceptionAt(2).AsTestingSequence();
		await using var ts2 = AsyncSeqExceptionAt(5).AsTestingSequence();
		await using var ts3 = Enumerable.Range(1, 10).AsTestingSequence();

		var starts = 0;
		var seq = AsyncSuperEnumerable.Case(
			() => starts++,
			new Dictionary<int, IAsyncEnumerable<int>>()
			{
				[0] = ts1,
				[1] = ts2,
				[2] = ts3,
			});
		await using var ts = seq.AsTestingSequence(maxEnumerations: 3);

		var result = ts.Retry(4);
		await result.AssertSequenceEqual(
			Enumerable.Range(1, 1)
				.Concat(Enumerable.Range(1, 4))
				.Concat(Enumerable.Range(1, 10)));
	}

	[Fact]
	public async Task RetryCountWithExceptionsThrow()
	{
		await using var ts1 = AsyncSeqExceptionAt(2).AsTestingSequence();
		await using var ts2 = AsyncSeqExceptionAt(5).AsTestingSequence();
		await using var ts3 = Enumerable.Range(1, 10).AsTestingSequence();

		var starts = 0;
		var seq = AsyncSuperEnumerable.Case(
			() => starts++,
			new Dictionary<int, IAsyncEnumerable<int>>()
			{
				[0] = ts1,
				[1] = ts2,
				[2] = ts3,
			});
		await using var ts = seq.AsTestingSequence(maxEnumerations: 2);

		var result = ts.Retry(2);
		await using var reader = result.Read();
		Assert.Equal(1, await reader.Read());
		Assert.Equal(1, await reader.Read());
		Assert.Equal(2, await reader.Read());
		Assert.Equal(3, await reader.Read());
		Assert.Equal(4, await reader.Read());
		_ = await Assert.ThrowsAsync<TestException>(async () =>
			await reader.Read());
	}
}
