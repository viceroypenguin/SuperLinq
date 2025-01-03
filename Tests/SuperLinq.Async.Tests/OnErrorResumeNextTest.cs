namespace SuperLinq.Async.Tests;

public sealed class OnErrorResumeNextTest
{
	[Test]
	public void OnErrorResumeNextIsLazy()
	{
		_ = new AsyncBreakingSequence<int>().OnErrorResumeNext(new AsyncBreakingSequence<int>());
		_ = AsyncSuperEnumerable.OnErrorResumeNext(new AsyncBreakingSequence<int>(), new AsyncBreakingSequence<int>());
		_ = new[] { new AsyncBreakingSequence<int>(), new AsyncBreakingSequence<int>() }.OnErrorResumeNext();
		_ = new AsyncBreakingSequence<IAsyncEnumerable<int>>().OnErrorResumeNext();
	}

	[Test]
	public async Task OnErrorResumeNextMultipleSequencesNoExceptions()
	{
		await using var ts1 = Enumerable.Range(1, 10).AsTestingSequence();
		await using var ts2 = Enumerable.Range(1, 10).AsTestingSequence();
		await using var ts3 = Enumerable.Range(1, 10).AsTestingSequence();

		await using var seq = new[] { ts1, ts2, ts3 }
			.AsTestingSequence();

		var result = seq.OnErrorResumeNext();

		await result.AssertSequenceEqual(
			Enumerable.Range(1, 10)
				.Concat(Enumerable.Range(1, 10))
				.Concat(Enumerable.Range(1, 10)));
	}

	[Test]
	[Arguments(1)]
	[Arguments(2)]
	[Arguments(3)]
	[Arguments(4)]
	[Arguments(5)]
	public async Task OnErrorResumeNextMultipleSequencesWithNoExceptionOnSequence(int sequenceNumber)
	{
		var cnt = 1;
		await using var ts1 = (cnt++ == sequenceNumber ? AsyncEnumerable.Range(1, 10) : AsyncSeqExceptionAt(5)).AsTestingSequence();
		await using var ts2 = (cnt++ == sequenceNumber ? AsyncEnumerable.Range(1, 10) : AsyncSeqExceptionAt(5)).AsTestingSequence();
		await using var ts3 = (cnt++ == sequenceNumber ? AsyncEnumerable.Range(1, 10) : AsyncSeqExceptionAt(5)).AsTestingSequence();
		await using var ts4 = (cnt++ == sequenceNumber ? AsyncEnumerable.Range(1, 10) : AsyncSeqExceptionAt(5)).AsTestingSequence();
		await using var ts5 = (cnt++ == sequenceNumber ? AsyncEnumerable.Range(1, 10) : AsyncSeqExceptionAt(5)).AsTestingSequence();

		await using var seq = new[] { ts1, ts2, ts3, ts4, ts5 }.AsTestingSequence();

		var result = seq.OnErrorResumeNext();

		var expected = Enumerable.Empty<int>();
		for (cnt = 1; cnt <= 5; cnt++)
			expected = expected.Concat(Enumerable.Range(1, cnt == sequenceNumber ? 10 : 4));

		await result.AssertSequenceEqual(expected);
	}
}
