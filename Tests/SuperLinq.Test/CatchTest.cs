namespace Test;

public sealed class CatchTest
{
	[Fact]
	public void CatchIsLazy()
	{
		_ = new BreakingSequence<int>().Catch(BreakingFunc.Of<Exception, IEnumerable<int>>());
		_ = new BreakingSequence<int>().Catch(new BreakingSequence<int>());
		_ = SuperEnumerable.Catch(new BreakingSequence<IEnumerable<int>>());
		_ = new[] { new BreakingSequence<int>(), new BreakingSequence<int>() }.Catch();
	}

	[Fact]
	public void CatchThrowsDelayedExceptionOnNullSource()
	{
		var seq = SuperEnumerable.Catch(new IEnumerable<int>[] { null!, });
		_ = Assert.Throws<ArgumentNullException>(seq.Consume);
	}

	[Fact]
	public void CatchHandlerWithNoExceptions()
	{
		using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		var result = seq.Catch(BreakingFunc.Of<Exception, IEnumerable<int>>());
		result.AssertSequenceEqual(Enumerable.Range(1, 10));
	}

	[Fact]
	public void CatchHandlerWithException()
	{
		using var seq = SeqExceptionAt(5).AsTestingSequence();

		var ran = false;
		var result = seq
			.Catch((Exception _) =>
			{
				ran = true;
				return SuperEnumerable.Return(-5);
			});

		Assert.False(ran);
		result.AssertSequenceEqual(1, 2, 3, 4, -5);
		Assert.True(ran);
	}

	[Fact]
	public void CatchWithEmptySequenceList()
	{
		using var seq = Enumerable.Empty<IEnumerable<int>>().AsTestingSequence();

		var result = seq.Catch();
		result.AssertSequenceEqual();
	}

	[Theory]
	[InlineData(1)]
	[InlineData(2)]
	[InlineData(3)]
	public void CatchMultipleSequencesNoExceptions(int count)
	{
		using var ts1 = Enumerable.Range(1, 10).AsTestingSequence();
		using var ts2 = Enumerable.Range(1, 10).AsTestingSequence();
		using var ts3 = Enumerable.Range(1, 10).AsTestingSequence();

		using var seq = new[] { ts1, ts2, ts3 }
			.Take(count)
			.AsTestingSequence();

		var result = seq.Catch();

		// no matter what, only first sequence enumerated due to no exceptions
		result.AssertSequenceEqual(Enumerable.Range(1, 10));
	}

	[Theory]
	[InlineData(2)]
	[InlineData(3)]
	[InlineData(4)]
	public void CatchMultipleSequencesWithNoExceptionOnSequence(int sequenceNumber)
	{
		var cnt = 1;
		using var ts1 = (cnt++ == sequenceNumber ? Enumerable.Range(1, 10) : SeqExceptionAt(5)).AsTestingSequence();
		using var ts2 = (cnt++ == sequenceNumber ? Enumerable.Range(1, 10) : SeqExceptionAt(5)).AsTestingSequence();
		using var ts3 = (cnt++ == sequenceNumber ? Enumerable.Range(1, 10) : SeqExceptionAt(5)).AsTestingSequence();
		using var ts4 = (cnt++ == sequenceNumber ? Enumerable.Range(1, 10) : SeqExceptionAt(5)).AsTestingSequence();
		using var ts5 = (cnt++ == sequenceNumber ? Enumerable.Range(1, 10) : SeqExceptionAt(5)).AsTestingSequence();

		using var seq = new[] { ts1, ts2, ts3, ts4, ts5, }.AsTestingSequence();

		var result = seq.Catch();

		result.AssertSequenceEqual(
			Enumerable.Range(1, 4)
				.Repeat(sequenceNumber - 1)
				.Concat(Enumerable.Range(1, 10)));
	}

	[Fact]
	public void CatchMultipleSequencesThrowsIfNoFollowingSequence()
	{
		using var ts1 = SeqExceptionAt(5).AsTestingSequence();
		using var ts2 = SeqExceptionAt(5).AsTestingSequence();
		using var ts3 = SeqExceptionAt(5).AsTestingSequence();

		using var seq = new[] { ts1, ts2, ts3 }
			.AsTestingSequence();

		var result = seq.Catch();

		_ = Assert.Throws<TestException>(() =>
		{
			var i = 1;
			foreach (var item in result)
			{
				Assert.Equal(i++, item);
				if (i == 5)
					i = 1;
			}
		});
	}
}
