using System.Collections;

namespace SuperLinq.Async.Tests;

public sealed class MemoizeTest
{
	[Test]
	public void MemoizeIsLazy()
	{
		_ = new AsyncBreakingSequence<int>().Memoize();
	}

	[Test]
	public async Task MemoizeSimpleUse()
	{
		await using var seq = AsyncEnumerable.Range(1, 10).AsTestingSequence();

		await using var buffer = seq.Memoize();
		Assert.Equal(0, buffer.Count);

		await buffer.AssertSequenceEqual(Enumerable.Range(1, 10));
		Assert.Equal(10, buffer.Count);
	}

	[Test]
	public async Task MemoizeReturningExpectedElementsWhenUsedAtInnerForEach()
	{
		await using var seq = AsyncEnumerable.Range(1, 10).AsTestingSequence();

		var buffer = seq.Memoize();

		var flowArray = InnerForEach(AsyncEnumerable.Range(1, 10));
		var flowBuffer = InnerForEach(buffer);

		await flowArray.AssertSequenceEqual(flowBuffer);

		static async IAsyncEnumerable<object> InnerForEach(IAsyncEnumerable<int> source)
		{
			var firstVisitAtInnerLoopDone = false;

			//add 1-3 to cache (so enter inner loop)
			//consume 4-5 already cached
			//add 6-7 to cache (so enter inner loop)
			//consume 8-10 already cached

			yield return "enter outer loop";
			await foreach (var i in source)
			{
				yield return i;

				if (i is 3 or 7)
				{
					//consume 1-3 already cached
					//add 4-5 to cache (so go to outer loop)
					//consume 1-7 already cached
					//add 8-10 to cache (so go to outer loop)

					yield return "enter inner loop";
					await foreach (var j in source)
					{
						yield return j;

						if (!firstVisitAtInnerLoopDone && j == 5)
						{
							firstVisitAtInnerLoopDone = true;
							break;
						}
					}

					yield return "exit inner loop";
				}
			}

			yield return "exit outer loop";

			yield return "enter last loop";
			//consume 1-10 (all item were already cached)
			await foreach (var k in source)
			{
				yield return k;
			}

			yield return "exit last loop";
		}
	}

	[Test]
	public async Task MemoizeThrowsWhenCacheDisposedDuringIteration()
	{
		await using var seq = AsyncEnumerable.Range(1, 10).AsTestingSequence();

		await using var buffer = seq.Memoize();

		await using var reader = buffer.Read();
		Assert.Equal(1, await reader.Read());
		await buffer.DisposeAsync();

		_ = await Assert.ThrowsAsync<ObjectDisposedException>(
			async () => await reader.Read());
	}

	[Test]
	public async Task MemoizeThrowsWhenResetDuringIteration()
	{
		await using var seq = AsyncEnumerable.Range(1, 10).AsTestingSequence();

		await using var buffer = seq.Memoize();

		await using var reader = buffer.Read();
		Assert.Equal(1, await reader.Read());

		await buffer.Reset();

		var ex = await Assert.ThrowsAsync<InvalidOperationException>(
			async () => await reader.Read());
		Assert.Equal("Buffer reset during iteration.", ex.Message);
	}

	[Test]
	public async Task MemoizeThrowsWhenGettingAfterDispose()
	{
		await using var seq = AsyncEnumerable.Range(1, 10).AsTestingSequence();

		await using var buffer = seq.Memoize();

		await buffer.Consume();
		await buffer.DisposeAsync();

		_ = await Assert.ThrowsAsync<ObjectDisposedException>(
			async () => await buffer.Consume());
	}

	[Test]
	public async Task MemoizeThrowsWhenResettingAfterDispose()
	{
		await using var seq = AsyncEnumerable.Range(1, 10).AsTestingSequence();

		await using var buffer = seq.Memoize();

		await buffer.Consume();
		await buffer.DisposeAsync();

		_ = await Assert.ThrowsAsync<ObjectDisposedException>(
			async () => await buffer.Reset());
	}

	[Test]
	public async Task MemoizeWithPartialIterationBeforeCompleteIteration()
	{
		await using var seq = AsyncEnumerable.Range(1, 10).AsTestingSequence();

		await using var buffer = seq.Memoize();
		Assert.Equal(0, buffer.Count);

		await buffer.Take(5).AssertSequenceEqual(Enumerable.Range(1, 5));
		Assert.Equal(5, buffer.Count);

		await buffer.AssertSequenceEqual(Enumerable.Range(1, 10));
		Assert.Equal(10, buffer.Count);
	}

	[Test]
	public async Task MemoizeWithDisposeOnEarlyExitTrue()
	{
		await using var seq = AsyncEnumerable.Range(1, 10).AsTestingSequence();

		await using var buffer = seq.Memoize();
		Assert.Equal(0, buffer.Count);

		await using (buffer)
			await buffer.Take(1).Consume();

		Assert.Equal(0, buffer.Count);
		Assert.True(seq.IsDisposed);
	}

	[Test]
	public async Task MemoizeDisposesAfterSourceIsIteratedEntirely()
	{
		await using var seq = AsyncEnumerable.Range(1, 10).AsTestingSequence();

		await using var buffer = seq.Memoize();
		await buffer.Consume();

		Assert.True(seq.IsDisposed);
	}

	[Test]
	public async Task MemoizeEnumeratesOnlyOnce()
	{
		await using var seq = AsyncEnumerable.Range(1, 10).AsTestingSequence();

		await using var buffer = seq.Memoize();

		await buffer.AssertSequenceEqual(Enumerable.Range(1, 10));
		await buffer.AssertSequenceEqual(Enumerable.Range(1, 10));
	}

	[Test]
	public async Task MemoizeRestartsAfterReset()
	{
		var starts = 0;

		IEnumerable<int> TestSequence()
		{
			starts++;
			yield return 1;
			yield return 2;
		}

		await using var seq = TestSequence().AsTestingSequence(maxEnumerations: 2);
		await using var memoized = seq.Memoize();

		await memoized.Take(1).Consume();
		Assert.Equal(1, starts);

		await memoized.Reset();
		await memoized.Take(1).Consume();
		Assert.Equal(2, starts);
	}

	[Test]
	public async Task MemoizeRethrowsErrorDuringIterationToAllsUntilReset()
	{
		await using var xs = AsyncSeqExceptionAt(2).AsTestingSequence(maxEnumerations: 2);

		await using var memoized = xs.Memoize();

		await using (var r1 = memoized.Read())
		await using (var r2 = memoized.Read())
		{
			Assert.True(await r1.Read() == await r2.Read());
			_ = await Assert.ThrowsAsync<TestException>(async () => await r1.Read());

			Assert.True(xs.IsDisposed);

			_ = await Assert.ThrowsAsync<TestException>(async () => await r2.Read());
		}

		await memoized.Reset();

		await using (var r1 = memoized.Read())
			Assert.Equal(1, await r1.Read());
	}

	[Test]
	public async Task MemoizeRethrowsErrorDuringIterationStartToAllsUntilReset()
	{
		var i = 0;
		await using var xs = AsyncSuperEnumerable
			.From(() => i++ == 0 ? throw new TestException() : Task.FromResult(42))
			.AsTestingSequence(maxEnumerations: 2);

		await using var buffer = xs.Memoize();
		Assert.Equal(0, buffer.Count);

		await using (var r1 = buffer.Read())
		await using (var r2 = buffer.Read())
		{
			_ = await Assert.ThrowsAsync<TestException>(async () => await r1.Read());
			Assert.True(xs.IsDisposed);
			_ = await Assert.ThrowsAsync<TestException>(async () => await r2.Read());
		}

		Assert.Equal(0, buffer.Count);

		await buffer.Reset();
		Assert.Equal(0, buffer.Count);
		await using (var r1 = buffer.Read())
		await using (var r2 = buffer.Read())
			Assert.True(await r1.Read() == await r2.Read());

		Assert.Equal(1, buffer.Count);
	}

	[Test]
	public async Task MemoizeRethrowsErrorDuringFirstIterationStartToAllIterationsUntilReset()
	{
		using var seq = new FailingEnumerable().AsTestingSequence(maxEnumerations: 2);

		await using var buffer = seq.Memoize();
		Assert.Equal(0, buffer.Count);

		for (var i = 0; i < 2; i++)
			_ = await Assert.ThrowsAsync<TestException>(async () => await buffer.FirstAsync());

		await buffer.Reset();
		await buffer.AssertSequenceEqual(1);
		Assert.Equal(1, buffer.Count);
	}

	private sealed class FailingEnumerable : IEnumerable<int>
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
