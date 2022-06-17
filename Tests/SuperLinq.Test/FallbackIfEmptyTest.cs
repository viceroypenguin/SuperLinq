namespace Test;

public class FallbackIfEmptyTest
{
	[Fact]
	public void FallbackIfEmptyWithEmptySequence()
	{
		var source = Enumerable.Empty<int>().Select(x => x);
		// ReSharper disable PossibleMultipleEnumeration
		source.FallbackIfEmpty(12).AssertSequenceEqual(12);
		source.FallbackIfEmpty(12, 23).AssertSequenceEqual(12, 23);
		source.FallbackIfEmpty(12, 23, 34).AssertSequenceEqual(12, 23, 34);
		source.FallbackIfEmpty(12, 23, 34, 45).AssertSequenceEqual(12, 23, 34, 45);
		source.FallbackIfEmpty(12, 23, 34, 45, 56).AssertSequenceEqual(12, 23, 34, 45, 56);
		source.FallbackIfEmpty(12, 23, 34, 45, 56, 67).AssertSequenceEqual(12, 23, 34, 45, 56, 67);
		// ReSharper restore PossibleMultipleEnumeration
	}

	[Theory]
	[InlineData(SourceKind.BreakingCollection)]
	[InlineData(SourceKind.BreakingReadOnlyCollection)]
	public void FallbackIfEmptyPreservesSourceCollectionIfPossible(SourceKind sourceKind)
	{
		var source = new[] { 1 }.ToSourceKind(sourceKind);
		Assert.StrictEqual(source, source.FallbackIfEmpty(12));
		Assert.StrictEqual(source, source.FallbackIfEmpty(12, 23));
		Assert.StrictEqual(source, source.FallbackIfEmpty(12, 23, 34));
		Assert.StrictEqual(source, source.FallbackIfEmpty(12, 23, 34, 45));
		Assert.StrictEqual(source, source.FallbackIfEmpty(12, 23, 34, 45, 56));
		Assert.StrictEqual(source, source.FallbackIfEmpty(12, 23, 34, 45, 56, 67));
	}

	[Theory]
	[InlineData(SourceKind.BreakingCollection)]
	[InlineData(SourceKind.BreakingReadOnlyCollection)]
	public void FallbackIfEmptyPreservesFallbackCollectionIfPossible(SourceKind sourceKind)
	{
		var source = Array.Empty<int>().ToSourceKind(sourceKind);
		var fallback = new[] { 1 };
		Assert.Equal(fallback, source.FallbackIfEmpty(fallback));
		Assert.Equal(fallback, source.FallbackIfEmpty(fallback.AsEnumerable()));
	}
}
