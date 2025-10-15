namespace SuperLinq.Async.Tests;

public sealed class PublishTest
{
	[Fact]
	public void PublishIsLazy()
	{
		_ = new AsyncBreakingSequence<int>().Publish();
	}

	[Fact]
	public async Task PublishWithSingleConsumer()
	{
		await using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		await using var result = seq.Publish();
		await result.AssertSequenceEqual(Enumerable.Range(1, 10));
	}

	[Fact]
	public async Task PublishWithMultipleConsumers()
	{
		await using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		await using var result = seq.Publish();

		await using var r1 = result.Read();
		Assert.Equal(1, await r1.Read());
		Assert.Equal(2, await r1.Read());
		Assert.Equal(0, result.Count);

		await using var r2 = result.Read();
		Assert.Equal(3, await r1.Read());
		Assert.Equal(3, await r2.Read());
		Assert.Equal(0, result.Count);

		Assert.Equal(4, await r1.Read());
		Assert.Equal(5, await r1.Read());
		Assert.Equal(6, await r1.Read());
		Assert.Equal(7, await r1.Read());
		Assert.Equal(4, result.Count);

		Assert.Equal(4, await r2.Read());
		Assert.Equal(5, await r2.Read());
		Assert.Equal(2, result.Count);

		Assert.Equal(8, await r1.Read());
		Assert.Equal(9, await r1.Read());
		Assert.Equal(4, result.Count);

		await using var r3 = result.Read();
		Assert.Equal(10, await r3.Read());
		await r3.ReadEnd();
		Assert.Equal(5, result.Count);

		Assert.Equal(6, await r2.Read());
		Assert.Equal(7, await r2.Read());
		Assert.Equal(3, result.Count);

		Assert.Equal(10, await r1.Read());
		await r1.ReadEnd();

		Assert.Equal(8, await r2.Read());
		Assert.Equal(9, await r2.Read());
		Assert.Equal(10, await r2.Read());
		await r2.ReadEnd();
		Assert.Equal(0, result.Count);
	}

	[Fact]
	public async Task PublishWithInnerConsumer()
	{
		await using var seq = Enumerable.Range(1, 6).AsTestingSequence();

		await using var result = seq.Publish();

		await using var r1 = result.Read();
		Assert.Equal(1, await r1.Read());
		Assert.Equal(2, await r1.Read());

		await using (var r2 = result.Read())
		{
			Assert.Equal(3, await r2.Read());
			Assert.Equal(4, await r2.Read());
			Assert.Equal(2, result.Count);
		}

		Assert.Equal(3, await r1.Read());
		Assert.Equal(4, await r1.Read());
		Assert.Equal(5, await r1.Read());
		Assert.Equal(6, await r1.Read());

		await r1.ReadEnd();
		Assert.Equal(0, result.Count);
	}

	[Fact]
	public async Task PublishWithSequentialPartialConsumers()
	{
		await using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		await using var result = seq.Publish();

		await using (var r1 = result.Read())
		{
			Assert.Equal(1, await r1.Read());
			Assert.Equal(2, await r1.Read());
			Assert.Equal(3, await r1.Read());
			Assert.Equal(4, await r1.Read());
			Assert.Equal(5, await r1.Read());
			Assert.Equal(0, result.Count);
		}

		await using (var r2 = result.Read())
		{
			Assert.Equal(6, await r2.Read());
			Assert.Equal(7, await r2.Read());
			Assert.Equal(8, await r2.Read());
			Assert.Equal(9, await r2.Read());
			Assert.Equal(10, await r2.Read());
			await r2.ReadEnd();
			Assert.Equal(0, result.Count);
		}

		await using var r3 = result.Read();
		await r3.ReadEnd();
		Assert.Equal(0, result.Count);
	}

	[Fact]
	public async Task PublishDisposesAfterSourceIsIteratedEntirely()
	{
		await using var seq = Enumerable.Range(0, 10).AsTestingSequence();

		await using var buffer = seq.Publish();
		await buffer.Consume();

		Assert.True(seq.IsDisposed);
	}

	[Fact]
	public async Task PublishDisposesWithPartialEnumeration()
	{
		await using var seq = Enumerable.Range(0, 10).AsTestingSequence();

		await using var buffer = seq.Publish();

		await using (buffer)
			await buffer.Take(5).Consume();

		Assert.True(seq.IsDisposed);
	}

	[Fact]
	public async Task PublishRestartsAfterReset()
	{
		var starts = 0;

		IEnumerable<int> TestSequence()
		{
			starts++;
			yield return 1;
			yield return 2;
		}

		await using var seq = TestSequence().AsTestingSequence(maxEnumerations: 2);
		await using var buffer = seq.Publish();

		await buffer.Take(1).Consume();
		Assert.Equal(1, starts);

		await buffer.Reset();
		await buffer.Take(1).Consume();
		Assert.Equal(2, starts);
	}

	[Fact]
	public async Task PublishThrowsWhenCacheDisposedDuringIteration()
	{
		await using var seq = Enumerable.Range(0, 10).AsTestingSequence();
		await using var buffer = seq.Publish();

		await using var reader = buffer.Read();

		Assert.Equal(0, await reader.Read());
		await buffer.DisposeAsync();

		_ = await Assert.ThrowsAsync<ObjectDisposedException>(
			async () => await reader.Read());
	}

	[Fact]
	public async Task PublishThrowsWhenResetDuringIteration()
	{
		await using var seq = Enumerable.Range(0, 10).AsTestingSequence();
		await using var buffer = seq.Publish();

		await using var reader = buffer.Read();

		Assert.Equal(0, await reader.Read());
		await buffer.Reset();

		var ex = await Assert.ThrowsAsync<InvalidOperationException>(
			async () => await reader.Read());
		Assert.Equal("Buffer reset during iteration.", ex.Message);
	}

	[Fact]
	public async Task PublishThrowsWhenGettingIteratorAfterDispose()
	{
		await using var seq = Enumerable.Range(0, 10).AsTestingSequence();
		await using var buffer = seq.Publish();
		await buffer.Consume();
		await buffer.DisposeAsync();

		_ = await Assert.ThrowsAsync<ObjectDisposedException>(
			async () => await buffer.Consume());
	}

	[Fact]
	public async Task PublishThrowsWhenResettingAfterDispose()
	{
		await using var seq = Enumerable.Range(0, 10).AsTestingSequence();
		await using var buffer = seq.Publish();
		await buffer.Consume();
		await buffer.DisposeAsync();

		_ = await Assert.ThrowsAsync<ObjectDisposedException>(
			async () => await buffer.Reset());
	}

	[Fact]
	public async Task PublishRethrowsErrorDuringIterationToAllIteratorsUntilReset()
	{
		await using var xs = AsyncSeqExceptionAt(2).AsTestingSequence(maxEnumerations: 2);

		await using var buffer = xs.Publish();

		await using (var r1 = buffer.Read())
		await using (var r2 = buffer.Read())
		{
			Assert.Equal(1, await r1.Read());
			Assert.Equal(1, await r2.Read());

			_ = await Assert.ThrowsAsync<TestException>(async () => await r1.Read());
			_ = await Assert.ThrowsAsync<TestException>(async () => await r2.Read());
			Assert.True(xs.IsDisposed);
		}

		await using (var r3 = buffer.Read())
			_ = await Assert.ThrowsAsync<TestException>(async () => await r3.Read());

		await buffer.Reset();

		await using var r4 = buffer.Read();
		Assert.Equal(1, await r4.Read());
	}

	[Fact]
	public async Task PublishRethrowsErrorDuringFirstIterationStartToAllIterationsUntilReset()
	{
		await using var seq = new FailingEnumerable().AsTestingSequence(maxEnumerations: 2);

		await using var buffer = seq.Publish();

		for (var i = 0; i < 2; i++)
		{
			_ = await Assert.ThrowsAsync<TestException>(
				async () => await buffer.FirstAsync());
		}

		await buffer.Reset();
		await buffer.AssertSequenceEqual(1);
	}

	private sealed class FailingEnumerable : IAsyncEnumerable<int>
	{
		private bool _started;

		public IAsyncEnumerator<int> GetAsyncEnumerator(CancellationToken cancellationToken = default)
		{
			if (!_started)
			{
				_started = true;
				throw new TestException();
			}

			return AsyncEnumerable.Range(1, 1).GetAsyncEnumerator(cancellationToken);
		}
	}
}
