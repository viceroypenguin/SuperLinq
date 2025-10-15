namespace SuperLinq.Tests;

public sealed class FallbackIfEmptyTest
{
	[Fact]
	public void FallbackIfEmptyWithEmptySequence()
	{
		using var source = Seq<int>().AsTestingSequence();
		source.FallbackIfEmpty(12).AssertSequenceEqual(12);
	}

	[Fact]
	public void FallbackIfEmptyWithCollectionSequence()
	{
		using var source = Seq<int>().AsTestingCollection();
		source.FallbackIfEmpty(12).AssertSequenceEqual(12);
	}

	[Fact]
	public void FallbackIfEmptyWithNotEmptySequence()
	{
		using var source = Seq(1).AsTestingSequence();
		source.FallbackIfEmpty(new BreakingSequence<int>()).AssertSequenceEqual(1);
	}

	[Fact]
	public void FallbackIfEmptyWithNotEmptyCollectionSequence()
	{
		using var source = Seq(1).AsTestingCollection();
		source.FallbackIfEmpty(new BreakingSequence<int>()).AssertSequenceEqual(1);
	}

	[Fact]
	public void AssertFallbackIfEmptyCollectionBehaviorOnEmptyCollection()
	{
		using var source = Seq<int>().AsBreakingCollection();
		using var fallback = Enumerable.Range(0, 10_000).AsBreakingCollection();

		var result = source.FallbackIfEmpty(fallback);
		result.AssertCollectionErrorChecking(10_000);
	}

	[Fact]
	public void AssertFallbackIfEmptyCollectionBehaviorOnNonEmptyCollection()
	{
		using var source = Enumerable.Range(0, 10_000).AsBreakingCollection();
		using var fallback = new BreakingCollection<int>();

		var result = source.FallbackIfEmpty(fallback);
		result.AssertCollectionErrorChecking(10_000);
	}

	[Fact]
	public void FallbackIfEmptyUsesCollectionCountAtIterationTime()
	{
		var stack = new Stack<int>();

		var result = stack.FallbackIfEmpty(4);
		result.AssertSequenceEqual(4);

		stack.Push(1);
		result.AssertSequenceEqual(1);
	}
}
