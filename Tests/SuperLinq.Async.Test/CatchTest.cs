namespace Test.Async;

public class CatchTest
{
	[Fact]
	public void CatchIsLazy()
	{
		_ = new AsyncBreakingSequence<int>().Catch(BreakingFunc.Of<Exception, IAsyncEnumerable<int>>());
		_ = new AsyncBreakingSequence<int>().Catch(new AsyncBreakingSequence<int>());
		_ = AsyncSuperEnumerable.Catch(new AsyncBreakingSequence<IAsyncEnumerable<int>>());
		_ = new[] { new AsyncBreakingSequence<int>(), new AsyncBreakingSequence<int>() }.Catch();
	}

	[Fact]
	public async Task CatchThrowsDelayedExceptionOnNullSource()
	{
		var seq = AsyncSuperEnumerable.Catch(new IAsyncEnumerable<int>[] { null!, });
		_ = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
			await seq.Consume());
	}

	[Fact]
	public async Task CatchHandlerWithNoExceptions()
	{
		await using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		var result = seq.Catch(BreakingFunc.Of<Exception, IAsyncEnumerable<int>>());
		await result.AssertSequenceEqual(Enumerable.Range(1, 10));
	}

	[Fact]
	public async Task CatchHandlerWithException()
	{
		await using var seq = AsyncSeqExceptionAt(5).AsTestingSequence();

		var ran = false;
		var result = seq
			.Catch((Exception _) =>
			{
				ran = true;
				return SuperEnumerable.Return(-5).ToAsyncEnumerable();
			});

		Assert.False(ran);
		await result.AssertSequenceEqual(1, 2, 3, 4, -5);
		Assert.True(ran);
	}

	[Fact]
	public async Task CatchWithEmptySequenceList()
	{
		await using var seq = Enumerable.Empty<IAsyncEnumerable<int>>().AsTestingSequence();

		var result = seq.Catch();
		await result.AssertSequenceEqual();
	}

	[Theory]
	[InlineData(1)]
	[InlineData(2)]
	[InlineData(3)]
	public async Task CatchMultipleSequencesNoExceptions(int count)
	{
		await using var ts1 = Enumerable.Range(1, 10).AsTestingSequence();
		await using var ts2 = Enumerable.Range(1, 10).AsTestingSequence();
		await using var ts3 = Enumerable.Range(1, 10).AsTestingSequence();

		await using var seq = new[] { ts1, ts2, ts3 }
			.Take(count)
			.AsTestingSequence();

		var result = seq.Catch();

		// no matter what, only first sequence enumerated due to no exceptions
		await result.AssertSequenceEqual(Enumerable.Range(1, 10));
	}

	[Theory]
	[InlineData(2)]
	[InlineData(3)]
	[InlineData(4)]
	public async Task CatchMultipleSequencesWithNoExceptionOnSequence(int sequenceNumber)
	{
		var cnt = 1;
		await using var ts1 = (cnt++ == sequenceNumber ? AsyncEnumerable.Range(1, 10) : AsyncSeqExceptionAt(5)).AsTestingSequence();
		await using var ts2 = (cnt++ == sequenceNumber ? AsyncEnumerable.Range(1, 10) : AsyncSeqExceptionAt(5)).AsTestingSequence();
		await using var ts3 = (cnt++ == sequenceNumber ? AsyncEnumerable.Range(1, 10) : AsyncSeqExceptionAt(5)).AsTestingSequence();
		await using var ts4 = (cnt++ == sequenceNumber ? AsyncEnumerable.Range(1, 10) : AsyncSeqExceptionAt(5)).AsTestingSequence();
		await using var ts5 = (cnt++ == sequenceNumber ? AsyncEnumerable.Range(1, 10) : AsyncSeqExceptionAt(5)).AsTestingSequence();

		await using var seq = new[] { ts1, ts2, ts3, ts4, ts5, }.AsTestingSequence();

		var result = seq.Catch();

		await result.AssertSequenceEqual(
			Enumerable.Range(1, 4)
				.Repeat(sequenceNumber - 1)
				.Concat(Enumerable.Range(1, 10)));
	}

	[Fact]
	public async Task CatchMultipleSequencesThrowsIfNoFollowingSequence()
	{
		await using var ts1 = AsyncSeqExceptionAt(5).AsTestingSequence();
		await using var ts2 = AsyncSeqExceptionAt(5).AsTestingSequence();
		await using var ts3 = AsyncSeqExceptionAt(5).AsTestingSequence();

		await using var seq = new[] { ts1, ts2, ts3 }
			.AsTestingSequence();

		var result = seq.Catch();

		_ = await Assert.ThrowsAsync<TestException>(async () =>
		{
			var i = 1;
			await foreach (var item in result)
			{
				Assert.Equal(i++, item);
				if (i == 5)
					i = 1;
			}
		});
	}
}
