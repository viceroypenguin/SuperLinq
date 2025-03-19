namespace SuperLinq.Async.Tests;

public sealed class TimeoutTest
{
	[Test]
	public void TimeoutIsLazy()
	{
		_ = new AsyncBreakingSequence<int>().Timeout(TimeSpan.FromSeconds(1));
	}

	[Test]
	public async Task TimeoutNoException()
	{
		await using var ts = AsyncEnumerable.Range(1, 5)
			.SelectIdentityWithDelayAndToken(1)
			.AsTestingSequence();

		var result = ts.Timeout(TimeSpan.FromSeconds(1));

		await result.AssertSequenceEqual(1, 2, 3, 4, 5);
	}

	[Test]
	public async Task TimeoutException()
	{
		await using var ts = AsyncEnumerable.Range(1, 5)
			.SelectIdentityWithDelayAndToken(1_000)
			.AsTestingSequence();

		var result = ts.Timeout(TimeSpan.FromMilliseconds(0));

		_ = await Assert.ThrowsAsync<TimeoutException>(
			async () => await result.Consume());
	}

	[Test]
	public async Task TimeoutExceptionWithoutCancellation()
	{
		await using var ts = AsyncEnumerable.Range(1, 5)
			.SelectIdentityWithDelay(30)
			.AsTestingSequence();

		var result = ts.Timeout(TimeSpan.FromMilliseconds(0));

		_ = await Assert.ThrowsAsync<TimeoutException>(
			async () => await result.Consume());
	}

	[Test]
	public async Task TimeoutExceptionWithoutOperationCanceledExceptionInnerException()
	{
		await using var ts = new SequenceWithoutThrowIfCancellationRequested()
			.AsTestingSequence();

		var result = ts.Timeout(TimeSpan.FromMilliseconds(0));

		var timeoutException = await Assert.ThrowsAsync<TimeoutException>(
			async () => await result.Consume());

		Assert.Null(timeoutException.InnerException);
	}

	[Test]
	public async Task TimeoutExceptionWithOperationCanceledExceptionInnerException()
	{
		await using var ts = new SequenceWithThrowIfCancellationRequested()
			.AsTestingSequence();

		var result = ts.Timeout(TimeSpan.FromMilliseconds(0));

		var timeoutException = await Assert.ThrowsAsync<TimeoutException>(
			async () => await result.Consume()
		);

		_ = Assert.IsAssignableFrom<OperationCanceledException>(timeoutException.InnerException);
	}

	private sealed class SequenceWithoutThrowIfCancellationRequested : IAsyncEnumerable<int>
	{
		public IAsyncEnumerator<int> GetAsyncEnumerator(CancellationToken cancellationToken = new())
		{
			// cancellationToken.ThrowIfCancellationRequested() is purposefully not done here

			return AsyncEnumerable.Range(1, 5)
				.SelectIdentityWithDelay(10)
				.GetAsyncEnumerator(CancellationToken.None);
		}
	}

	private sealed class SequenceWithThrowIfCancellationRequested : IAsyncEnumerable<int>
	{
		public IAsyncEnumerator<int> GetAsyncEnumerator(CancellationToken cancellationToken = new())
		{
			cancellationToken.ThrowIfCancellationRequested();

			return AsyncEnumerable.Range(1, 5)
				.SelectIdentityWithDelayAndToken(10)
				.GetAsyncEnumerator(cancellationToken);
		}
	}
}

file static class AsyncEnumerableExtension
{
	public static IAsyncEnumerable<int> SelectIdentityWithDelay(
		this IAsyncEnumerable<int> source,
		int millisecondsDelay
	) =>
#if NET10_0_OR_GREATER
		source
			.Select(async (int i, CancellationToken ct) =>
			{
				await Task.Delay(millisecondsDelay, CancellationToken.None);
				return i;
			});
#else
		source
			.SelectAwait(async i =>
			{
				await Task.Delay(millisecondsDelay);
				return i;
			});
#endif

	public static IAsyncEnumerable<int> SelectIdentityWithDelayAndToken(
		this IAsyncEnumerable<int> source,
		int millisecondsDelay
	) =>
#if NET10_0_OR_GREATER
		source
			.Select(async (i, ct) =>
			{
				await Task.Delay(millisecondsDelay, ct);
				return i;
			});
#else
		source
			.SelectAwaitWithCancellation(async (i, ct) =>
			{
				await Task.Delay(millisecondsDelay, ct);
				return i;
			});
#endif
}
