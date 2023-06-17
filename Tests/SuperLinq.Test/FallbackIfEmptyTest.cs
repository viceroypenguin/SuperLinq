namespace Test;

public class FallbackIfEmptyTest
{
	[Fact]
	public void FallbackIfEmptyWithEmptySequence()
	{
		using var source = Enumerable.Empty<int>().AsTestingSequence(maxEnumerations: 2);
		source.FallbackIfEmpty(12).AssertSequenceEqual(12);
	}

	[Fact]
	public void FallbackIfEmptyWithCollectionSequence()
	{
		using var source = Enumerable.Empty<int>().AsTestingCollection(maxEnumerations: 2);
		source.FallbackIfEmpty(12).AssertSequenceEqual(12);
	}

	[Fact]
	public void FallbackIfEmptyWithNotEmptySequence()
	{
		using var source = Seq(1).AsTestingSequence(maxEnumerations: 2);
		source.FallbackIfEmpty(new BreakingSequence<int>()).AssertSequenceEqual(1);
	}

	[Fact]
	public void FallbackIfEmptyWithNotEmptyCollectionSequence()
	{
		using var source = Seq(1).AsTestingCollection(maxEnumerations: 2);
		source.FallbackIfEmpty(new BreakingSequence<int>()).AssertSequenceEqual(1);
	}
}
