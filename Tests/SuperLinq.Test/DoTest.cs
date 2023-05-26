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

	public static IEnumerable<object[]> GetSequences() =>
		Enumerable.Range(1, 10)
			.GetCollectionSequences()
			.Select(x => new object[] { x });

	[Theory]
	[MemberData(nameof(GetSequences))]
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

	[Theory]
	[MemberData(nameof(GetSequences))]
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

	[Theory]
	[MemberData(nameof(GetSequences))]
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

	[Theory]
	[MemberData(nameof(GetSequences))]
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

	[Fact]
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

	[Fact]
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

	[Fact]
	public void DoCollectionCount()
	{
		using var sequence = Enumerable.Range(1, 10_000)
			.AsBreakingCollection();

		var result = sequence.Do(delegate { });
		result.AssertCollectionErrorChecking(10_000);

		result = sequence.Do(delegate { }, onError: delegate { });
		result.AssertCollectionErrorChecking(10_000);
	}
}
