namespace Test.Async;

public class TimeoutTest
{
	[Fact]
	public void TimeoutIsLazy()
	{
		_ = new AsyncBreakingSequence<int>().Timeout(TimeSpan.FromSeconds(1));
	}

	[Fact]
	public async Task TimeoutNoException()
	{
		await using var ts = AsyncEnumerable.Range(1, 5)
			.SelectAwaitWithCancellation(async (x, ct) =>
			{
				await Task.Delay(TimeSpan.FromMilliseconds(1), ct);
				return x;
			})
			.AsTestingSequence();

		var result = ts.Timeout(TimeSpan.FromSeconds(1));

		await result.AssertSequenceEqual(1, 2, 3, 4, 5);
	}

	[Fact]
	public async Task TimeoutException()
	{
		await using var ts = AsyncEnumerable.Range(1, 5)
			.SelectAwaitWithCancellation(async (x, ct) =>
			{
				await Task.Delay(TimeSpan.FromSeconds(1), ct);
				return x;
			})
			.AsTestingSequence();

		var result = ts.Timeout(TimeSpan.FromMilliseconds(0));

		_ = await Assert.ThrowsAsync<TimeoutException>(
			async () => await result.Consume());
	}

	[Fact]
	public async Task TimeoutExceptionWithoutCancellation()
	{
		await using var ts = AsyncEnumerable.Range(1, 5)
			.SelectAwait(async x =>
			{
				await Task.Delay(TimeSpan.FromMilliseconds(30));
				return x;
			})
			.AsTestingSequence();

		var result = ts.Timeout(TimeSpan.FromMilliseconds(0));

		_ = await Assert.ThrowsAsync<TimeoutException>(
			async () => await result.Consume());
	}
}
