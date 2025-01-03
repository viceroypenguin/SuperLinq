namespace SuperLinq.Tests;

public sealed class FillBackwardTest
{
	[Test]
	public void FillBackwardIsLazy()
	{
		_ = new BreakingSequence<object>().FillBackward();
		_ = new BreakingSequence<object>().FillBackward(BreakingFunc.Of<object, bool>());
		_ = new BreakingSequence<object>().FillBackward(BreakingFunc.Of<object, bool>(), BreakingFunc.Of<object, object, object>());
	}

	public static IEnumerable<IDisposableEnumerable<int?>> GetIntNullSequences() =>
		Seq<int?>(null, null, 1, 2, null, null, null, 3, 4, null, null)
			.GetBreakingCollectionSequences();

	[Test]
	[MethodDataSource(nameof(GetIntNullSequences))]
	public void FillBackwardBlank(IDisposableEnumerable<int?> seq)
	{
		using (seq)
		{
			seq
				.FillBackward(x => x != 200)
				.AssertSequenceEqual(default(int?), null, 1, 2, null, null, null, 3, 4, null, null);
		}
	}

	[Test]
	[MethodDataSource(nameof(GetIntNullSequences))]
	public void FillBackward(IDisposableEnumerable<int?> seq)
	{
		using (seq)
		{
			seq
				.FillBackward()
				.AssertSequenceEqual(1, 1, 1, 2, 3, 3, 3, 3, 4, null, null);
		}
	}

	public static IEnumerable<IDisposableEnumerable<int>> GetIntSequences() =>
		Enumerable.Range(1, 13)
			.GetBreakingCollectionSequences();

	[Test]
	[MethodDataSource(nameof(GetIntSequences))]
	public void FillBackwardWithFillSelector(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			seq
				.FillBackward(e => e % 5 != 0, (x, y) => x * y)
				.AssertSequenceEqual(5, 10, 15, 20, 5, 60, 70, 80, 90, 10, 11, 12, 13);
		}
	}

	[Test]
	public void FillBackwardCollectionCount()
	{
		using var sequence = Enumerable.Range(1, 10_000)
			.AsBreakingCollection();

		var result = sequence.FillBackward();
		result.AssertCollectionErrorChecking(10_000);
	}
}
