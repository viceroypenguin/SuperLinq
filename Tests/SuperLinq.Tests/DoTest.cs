namespace SuperLinq.Tests;

public sealed class DoTest
{
	[Test]
	public void DoOverloadsAreLazy()
	{
		_ = new BreakingSequence<int>().Do(BreakingAction.Of<int>());
		_ = new BreakingSequence<int>().Do(BreakingAction.Of<int>(), BreakingAction.Of());
		_ = new BreakingSequence<int>().Do(BreakingAction.Of<int>(), BreakingAction.Of<Exception>(), BreakingAction.Of());
	}

	public static IEnumerable<IDisposableEnumerable<int>> GetSequences() =>
		Enumerable.Range(1, 10)
			.GetTestingSequence();

	[Test]
	[MethodDataSource(nameof(GetSequences))]
	public void DoBehavior(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var count = 0;
			var result = seq.Do(i => count += i);

			result.AssertSequenceEqual(Enumerable.Range(1, 10));
			Assert.Equal(55, count);
		}
	}

	[Test]
	[MethodDataSource(nameof(GetSequences))]
	public void DoCompletedBehavior(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var count = 0;
			var result = seq.Do(i => count += i, () => count += 10);

			result.AssertSequenceEqual(Enumerable.Range(1, 10));
			Assert.Equal(65, count);
		}
	}

	[Test]
	[MethodDataSource(nameof(GetSequences))]
	public void DoBehaviorNoError(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var count = 0;
			var result = seq.Do(i => count += i, BreakingAction.Of<Exception>());

			result.AssertSequenceEqual(Enumerable.Range(1, 10));
			Assert.Equal(55, count);
		}
	}

	[Test]
	[MethodDataSource(nameof(GetSequences))]
	public void DoCompletedBehaviorNoError(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var count = 0;
			var result = seq.Do(i => count += i, BreakingAction.Of<Exception>(), () => count += 10);

			result.AssertSequenceEqual(Enumerable.Range(1, 10));
			Assert.Equal(65, count);
		}
	}

	[Test]
	public void DoBehaviorError()
	{
		using var seq = SeqExceptionAt(11)
			.AsTestingSequence();

		var count = 0;
		_ = Assert.Throws<TestException>(() =>
			seq
				.Do(
					i => count += i,
					ex => { _ = Assert.IsType<TestException>(ex); count += 100; })
				.Consume());

		Assert.Equal(155, count);
	}

	[Test]
	public void DoCompletedBehaviorError()
	{
		using var seq = SeqExceptionAt(11)
			.AsTestingSequence();

		var count = 0;
		_ = Assert.Throws<TestException>(() =>
			seq
				.Do(
					i => count += i,
					ex => { _ = Assert.IsType<TestException>(ex); count += 100; },
					BreakingAction.Of())
				.Consume());

		Assert.Equal(155, count);
	}
}
