namespace Test;

public sealed class RetryTest
{
	[Fact]
	public void RetryIsLazy()
	{
		_ = new BreakingSequence<int>().Retry();
	}

	[Fact]
	public void RetryNoExceptions()
	{
		using var ts = Enumerable.Range(1, 10).AsTestingSequence();

		var result = ts.Retry();
		result.AssertSequenceEqual(Enumerable.Range(1, 10));
	}

	[Fact]
	public void RetryWithExceptions()
	{
		using var ts1 = SeqExceptionAt(2).AsTestingSequence();
		using var ts2 = SeqExceptionAt(5).AsTestingSequence();
		using var ts3 = Enumerable.Range(1, 10).AsTestingSequence();

		var starts = 0;
		var seq = SuperEnumerable.Case(
			() => starts++,
			new Dictionary<int, IEnumerable<int>>()
			{
				[0] = ts1,
				[1] = ts2,
				[2] = ts3,
			});
		using var ts = seq.AsTestingSequence(maxEnumerations: 3);

		var result = ts.Retry();
		result.AssertSequenceEqual(
			Enumerable.Range(1, 1)
				.Concat(Enumerable.Range(1, 4))
				.Concat(Enumerable.Range(1, 10)));
	}

	[Fact]
	public void RetryCountIsLazy()
	{
		_ = new BreakingSequence<int>().Retry(3);
	}

	[Fact]
	public void RetryCountNoExceptions()
	{
		using var ts = Enumerable.Range(1, 10).AsTestingSequence();

		var result = ts.Retry(3);
		result.AssertSequenceEqual(Enumerable.Range(1, 10));
	}

	[Fact]
	public void RetryCountWithExceptionsComplete()
	{
		using var ts1 = SeqExceptionAt(2).AsTestingSequence();
		using var ts2 = SeqExceptionAt(5).AsTestingSequence();
		using var ts3 = Enumerable.Range(1, 10).AsTestingSequence();

		var starts = 0;
		var seq = SuperEnumerable.Case(
			() => starts++,
			new Dictionary<int, IEnumerable<int>>()
			{
				[0] = ts1,
				[1] = ts2,
				[2] = ts3,
			});
		using var ts = seq.AsTestingSequence(maxEnumerations: 3);

		var result = ts.Retry(4);
		result.AssertSequenceEqual(
			Enumerable.Range(1, 1)
				.Concat(Enumerable.Range(1, 4))
				.Concat(Enumerable.Range(1, 10)));
	}

	[Fact]
	public void RetryCountWithExceptionsThrow()
	{
		using var ts1 = SeqExceptionAt(2).AsTestingSequence();
		using var ts2 = SeqExceptionAt(5).AsTestingSequence();
		using var ts3 = Enumerable.Range(1, 10).AsTestingSequence();

		var starts = 0;
		var seq = SuperEnumerable.Case(
			() => starts++,
			new Dictionary<int, IEnumerable<int>>()
			{
				[0] = ts1,
				[1] = ts2,
				[2] = ts3,
			});
		using var ts = seq.AsTestingSequence(maxEnumerations: 2);

		var result = ts.Retry(2);
		using var reader = result.Read();
		Assert.Equal(1, reader.Read());
		Assert.Equal(1, reader.Read());
		Assert.Equal(2, reader.Read());
		Assert.Equal(3, reader.Read());
		Assert.Equal(4, reader.Read());
		_ = Assert.Throws<TestException>(() => reader.Read());
	}
}
