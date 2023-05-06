namespace Test;

public class DoTest
{
	[Fact]
	public void DoOverloadsAreLazy()
	{
		_ = new BreakingSequence<int>().Do(BreakingAction.Of<int>());
		_ = new BreakingSequence<int>().Do(BreakingAction.Of<int>(), BreakingAction.Of());
		_ = new BreakingSequence<int>().Do(BreakingAction.Of<int>(), BreakingAction.Of<Exception>(), BreakingAction.Of());
	}

	[Fact]
	public void DoBehavior()
	{
		using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		var count = 0;
		var result = seq.Do(i => count += i);

		result.AssertSequenceEqual(Enumerable.Range(1, 10));
		Assert.Equal(55, count);
	}

	[Fact]
	public void DoCompletedBehavior()
	{
		using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		var count = 0;
		var result = seq.Do(i => count += i, () => count += 10);

		result.AssertSequenceEqual(Enumerable.Range(1, 10));
		Assert.Equal(65, count);
	}

	[Fact]
	public void DoBehaviorNoError()
	{
		using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		var count = 0;
		var result = seq.Do(i => count += i, BreakingAction.Of<Exception>());

		result.AssertSequenceEqual(Enumerable.Range(1, 10));
		Assert.Equal(55, count);
	}

	[Fact]
	public void DoCompletedBehaviorNoError()
	{
		using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		var count = 0;
		var result = seq.Do(i => count += i, BreakingAction.Of<Exception>(), () => count += 10);

		result.AssertSequenceEqual(Enumerable.Range(1, 10));
		Assert.Equal(65, count);
	}

	[Fact]
	public void DoBehaviorError()
	{
		using var seq = Enumerable.Range(1, 10)
			.Concat(SuperEnumerable.From(BreakingFunc.Of<int>()))
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

	[Fact]
	public void DoCompletedBehaviorError()
	{
		using var seq = Enumerable.Range(1, 10)
			.Concat(SuperEnumerable.From(BreakingFunc.Of<int>()))
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

	[Fact]
	public void DoCollectionCount()
	{
		using var sequence = Enumerable.Range(1, 10_000)
			.AsBreakingCollection();

		var result = sequence.Do(delegate { });
		Assert.Equal(10_000, result.Count());

		result = sequence.Do(delegate { }, onError: delegate { });
		Assert.Equal(10_000, result.Count());
	}
}
