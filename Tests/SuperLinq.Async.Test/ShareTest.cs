using System.Collections;
using CommunityToolkit.Diagnostics;

namespace Test.Async;

public class ShareTest
{
	[Fact]
	public void ShareIsLazy()
	{
		_ = new AsyncBreakingSequence<int>().Share();
	}

	[Fact]
	public async Task ShareWithSingleConsumer()
	{
		await using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		await using var result = seq.Share();
		await result.AssertSequenceEqual(Enumerable.Range(1, 10));
		Assert.Equal(0, result.Count);
	}

	[Fact]
	public async Task ShareWithMultipleConsumers()
	{
		await using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		await using var result = seq.Share();

		await using var r1 = result.Read();
		await using var r2 = result.Read();

		Assert.Equal(1, await r1.Read());
		Assert.Equal(2, await r2.Read());
		Assert.Equal(3, await r2.Read());
		Assert.Equal(4, await r1.Read());
		Assert.Equal(5, await r1.Read());
		Assert.Equal(6, await r1.Read());
		Assert.Equal(7, await r2.Read());
		Assert.Equal(8, await r2.Read());
		Assert.Equal(9, await r1.Read());
		Assert.Equal(10, await r2.Read());

		await r1.ReadEnd();
		await r2.ReadEnd();
	}

	[Fact]
	public async Task ShareWithInnerConsumer()
	{
		await using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		await using var result = seq.Share();

		await using var r1 = result.Read();
		Assert.Equal(1, await r1.Read());
		Assert.Equal(2, await r1.Read());

		await using (var r2 = result.Read())
		{
			Assert.Equal(3, await r2.Read());
			Assert.Equal(4, await r1.Read());
			Assert.Equal(5, await r2.Read());
			Assert.Equal(6, await r2.Read());
		}

		Assert.Equal(7, await r1.Read());
		Assert.Equal(8, await r1.Read());
		Assert.Equal(9, await r1.Read());
		Assert.Equal(10, await r1.Read());

		await r1.ReadEnd();
	}

	[Fact]
	public async Task ShareWithSequentialPartialConsumers()
	{
		await using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		await using var result = seq.Share();

		await using (var r1 = result.Read())
		{
			Assert.Equal(1, await r1.Read());
			Assert.Equal(2, await r1.Read());
			Assert.Equal(3, await r1.Read());
			Assert.Equal(4, await r1.Read());
			Assert.Equal(5, await r1.Read());
		}

		await using (var r2 = result.Read())
		{
			Assert.Equal(6, await r2.Read());
			Assert.Equal(7, await r2.Read());
			Assert.Equal(8, await r2.Read());
			Assert.Equal(9, await r2.Read());
			Assert.Equal(10, await r2.Read());
			await r2.ReadEnd();
		}

		await using var r3 = result.Read();
		await r3.ReadEnd();
	}

	[Fact]
	public async Task ShareDisposesAfterSourceIsIteratedEntirely()
	{
		await using var seq = Enumerable.Range(0, 10).AsTestingSequence();

		await using var buffer = seq.Share();
		await buffer.Consume();

		Assert.True(seq.IsDisposed);
	}

	[Fact]
	public async Task ShareDisposesWithPartialEnumeration()
	{
		await using var seq = Enumerable.Range(0, 10).AsTestingSequence();

		await using var buffer = seq.Share();

		await using (buffer)
			await buffer.Take(5).Consume();

		Assert.True(seq.IsDisposed);
	}

	[Fact]
	public async Task ShareRestartsAfterReset()
	{
		var starts = 0;

		IEnumerable<int> TestSequence()
		{
			starts++;
			yield return 1;
			yield return 2;
		}

		await using var seq = TestSequence().AsTestingSequence(maxEnumerations: 2);
		await using var buffer = seq.Share();

		await buffer.Take(1).Consume();
		Assert.Equal(1, starts);

		await buffer.Reset();
		await buffer.Take(1).Consume();
		Assert.Equal(2, starts);
	}

	[Fact]
	public async Task ShareThrowsWhenCacheDisposedDuringIteration()
	{
		await using var seq = Enumerable.Range(0, 10).AsTestingSequence();
		await using var buffer = seq.Share();

		await using var reader = buffer.Read();

		Assert.Equal(0, await reader.Read());
		await buffer.DisposeAsync();

		_ = await Assert.ThrowsAsync<ObjectDisposedException>(
			async () => await reader.Read());
	}

	[Fact]
	public async Task ShareThrowsWhenResetDuringIteration()
	{
		await using var seq = Enumerable.Range(0, 10).AsTestingSequence();
		await using var buffer = seq.Share();

		await using var reader = buffer.Read();

		Assert.Equal(0, await reader.Read());
		await buffer.Reset();

		var ex = await Assert.ThrowsAsync<InvalidOperationException>(
			async () => await reader.Read());
		Assert.Equal("Buffer reset during iteration.", ex.Message);
	}

	[Fact]
	public async Task ShareThrowsWhenGettingIteratorAfterDispose()
	{
		await using var seq = Enumerable.Range(0, 10).AsTestingSequence();
		await using var buffer = seq.Share();
		await buffer.Consume();
		await buffer.DisposeAsync();

		_ = await Assert.ThrowsAsync<ObjectDisposedException>(
			async () => await buffer.Consume());
	}

	[Fact]
	public async Task ShareThrowsWhenResettingAfterDispose()
	{
		await using var seq = Enumerable.Range(0, 10).AsTestingSequence();
		await using var buffer = seq.Share();
		await buffer.Consume();
		await buffer.DisposeAsync();

		_ = await Assert.ThrowsAsync<ObjectDisposedException>(
			async () => await buffer.Reset());
	}

	[Fact]
	public async Task ShareRethrowsErrorDuringIterationToAllIteratorsUntilReset()
	{
		await using var xs = AsyncSeqExceptionAt(2).AsTestingSequence(maxEnumerations: 2);

		await using var buffer = xs.Share();

		await using (var r1 = buffer.Read())
		await using (var r2 = buffer.Read())
		{
			Assert.Equal(1, await r1.Read());
			_ = await Assert.ThrowsAsync<TestException>(async () => await r2.Read());

			Guard.IsTrue(xs.IsDisposed);
			_ = await Assert.ThrowsAsync<TestException>(async () => await r1.Read());
		}

		_ = Assert.Throws<TestException>(() => buffer.Read());

		await buffer.Reset();

		await using (var r1 = buffer.Read())
			Assert.Equal(1, await r1.Read());
	}

	[Fact]
	public async Task ShareRethrowsErrorDuringFirstIterationStartToAllIterationsUntilReset()
	{
		await using var seq = new FailingEnumerable().AsTestingSequence(maxEnumerations: 2);

		await using var buffer = seq.Share();

		for (var i = 0; i < 2; i++)
			_ = await Assert.ThrowsAsync<TestException>(async () => await buffer.FirstAsync());

		await buffer.Reset();
		await buffer.AssertSequenceEqual(1);
	}

	private class FailingEnumerable : IEnumerable<int>
	{
		private bool _started;
		public IEnumerator<int> GetEnumerator()
		{
			if (!_started)
			{
				_started = true;
				throw new TestException();
			}

			return Enumerable.Range(1, 1).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
