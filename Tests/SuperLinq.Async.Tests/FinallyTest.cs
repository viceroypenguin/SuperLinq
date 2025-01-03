namespace SuperLinq.Async.Tests;

public sealed class FinallyTest
{
	[Test]
	public void FinallyIsLazy()
	{
		_ = new AsyncBreakingSequence<int>().Finally(BreakingAction.Of());
	}

	[Test]
	public async Task FinallyExecutesOnCompletion()
	{
		await using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		var ran = false;
		var result = seq.Finally(() => ran = true);

		Assert.False(ran);
		await result.AssertSequenceEqual(Enumerable.Range(1, 10));
		Assert.True(ran);
	}

	[Test]
	public async Task FinallyExecutesOnException()
	{
		await using var seq = AsyncSeqExceptionAt(4).AsTestingSequence();

		var ran = false;
		var result = seq.Finally(() => ran = true);

		Assert.False(ran);
		_ = await Assert.ThrowsAsync<TestException>(async () =>
		{
			var i = 1;
			await foreach (var item in result)
				Assert.Equal(i++, item);
		});
		Assert.True(ran);
	}

	[Test]
	public async Task FinallyExecutesOnTake()
	{
		await using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		var ran = false;
		var result = seq.Finally(() => ran = true);

		Assert.False(ran);
		await result
			.Take(5)
			.AssertSequenceEqual(Enumerable.Range(1, 5));
		Assert.True(ran);
	}

	[Test]
	public async Task FinallyExecutesOnEarlyDisposal()
	{
		await using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		var ran = false;
		var result = seq.Finally(() => ran = true);

		Assert.False(ran);

		{
			await using var iter = result.GetAsyncEnumerator();
			for (var i = 1; i < 5; i++)
			{
				Assert.True(await iter.MoveNextAsync());
				Assert.Equal(i, iter.Current);
			}
		}

		Assert.True(ran);
	}
}
