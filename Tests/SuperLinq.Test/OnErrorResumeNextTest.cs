namespace Test;

public sealed class OnErrorResumeNextTest
{
	[Fact]
	public void OnErrorResumeNextIsLazy()
	{
		_ = new BreakingSequence<int>().OnErrorResumeNext(new BreakingSequence<int>());
		_ = new[] { new BreakingSequence<int>(), new BreakingSequence<int>() }.OnErrorResumeNext();
		_ = new BreakingSequence<IEnumerable<int>>().OnErrorResumeNext();
	}

	[Fact]
	public void OnErrorResumeNextMultipleSequencesNoExceptions()
	{
		using var ts1 = Enumerable.Range(1, 10).AsTestingSequence();
		using var ts2 = Enumerable.Range(1, 10).AsTestingSequence();
		using var ts3 = Enumerable.Range(1, 10).AsTestingSequence();

		using var seq = new[] { ts1, ts2, ts3 }
			.AsTestingSequence();

		var result = seq.OnErrorResumeNext();

		result.AssertSequenceEqual(
			Enumerable.Range(1, 10)
				.Concat(Enumerable.Range(1, 10))
				.Concat(Enumerable.Range(1, 10)));
	}

	[Theory]
	[InlineData(1)]
	[InlineData(2)]
	[InlineData(3)]
	[InlineData(4)]
	[InlineData(5)]
	public void OnErrorResumeNextMultipleSequencesWithNoExceptionOnSequence(int sequenceNumber)
	{
		var cnt = 1;
		using var ts1 = (cnt++ == sequenceNumber ? Enumerable.Range(1, 10) : SeqExceptionAt(5)).AsTestingSequence();
		using var ts2 = (cnt++ == sequenceNumber ? Enumerable.Range(1, 10) : SeqExceptionAt(5)).AsTestingSequence();
		using var ts3 = (cnt++ == sequenceNumber ? Enumerable.Range(1, 10) : SeqExceptionAt(5)).AsTestingSequence();
		using var ts4 = (cnt++ == sequenceNumber ? Enumerable.Range(1, 10) : SeqExceptionAt(5)).AsTestingSequence();
		using var ts5 = (cnt++ == sequenceNumber ? Enumerable.Range(1, 10) : SeqExceptionAt(5)).AsTestingSequence();

		using var seq = new[] { ts1, ts2, ts3, ts4, ts5, }.AsTestingSequence();

		var result = seq.OnErrorResumeNext();

		var expected = Enumerable.Empty<int>();
		for (cnt = 1; cnt <= 5; cnt++)
			expected = expected.Concat(Enumerable.Range(1, cnt == sequenceNumber ? 10 : 4));

		result.AssertSequenceEqual(expected);
	}
}
