namespace SuperLinq.Tests;

public sealed class AssertCountTest
{
	[Test]
	public void AssertCountIsLazy()
	{
		_ = new BreakingSequence<object>().AssertCount(0);
	}

	[Test]
	public void AssertCountNegativeCount()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>("count",
			() => new BreakingSequence<int>().AssertCount(-1));
	}

	public static IEnumerable<IDisposableEnumerable<int>> GetSequences() =>
		Enumerable.Range(1, 10)
			.GetAllSequences();

	[Test]
	[MethodDataSource(nameof(GetSequences))]
	public void AssertCountShortSequence(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.AssertCount(11);
			_ = Assert.Throws<ArgumentOutOfRangeException>("source.Count()",
				() => result.Consume());
		}
	}

	[Test]
	[MethodDataSource(nameof(GetSequences))]
	public void AssertCountEqualSequence(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.AssertCount(10);
			result.AssertSequenceEqual(
				Enumerable.Range(1, 10),
				testCollectionEnumerable: true);
		}
	}

	[Test]
	[MethodDataSource(nameof(GetSequences))]
	public void AssertCountLongSequence(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.AssertCount(9);
			_ = Assert.Throws<ArgumentOutOfRangeException>("source.Count()",
				() => result.Consume());
		}
	}

	[Test]
	public void AssertCountCollectionBehavior()
	{
		using var seq = Enumerable.Range(0, 10_000).AsBreakingCollection();

		var result = seq.AssertCount(10_000);
		result.AssertCollectionErrorChecking(10_000);
		result.AssertSequenceEqual(Enumerable.Range(0, 10_000));
	}

	[Test]
	public void AssertCountListBehavior()
	{
		using var seq = Enumerable.Range(0, 10_000).AsBreakingList();

		var result = seq.AssertCount(10_000);
		result.AssertCollectionErrorChecking(10_000);
		result.AssertListElementChecking(10_000);

		Assert.Equal(200, result.ElementAt(200));
		Assert.Equal(1_200, result.ElementAt(1_200));
#if !NO_INDEX
		Assert.Equal(8_800, result.ElementAt(^1_200));
#endif
	}

	[Test]
	public void AssertCountUsesCollectionCountAtIterationTime()
	{
		var stack = new Stack<int>(Enumerable.Range(1, 3));
		var result = stack.AssertCount(4);
		_ = Assert.Throws<ArgumentOutOfRangeException>("source.Count()",
			() => result.Consume());
		stack.Push(4);
		result.Consume();
	}
}
