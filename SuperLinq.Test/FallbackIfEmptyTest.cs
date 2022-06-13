using NUnit.Framework;

namespace Test;

[TestFixture]
public class FallbackIfEmptyTest
{
	[Test]
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

	[TestCase(SourceKind.BreakingCollection)]
	[TestCase(SourceKind.BreakingReadOnlyCollection)]
	public void FallbackIfEmptyPreservesSourceCollectionIfPossible(SourceKind sourceKind)
	{
		var source = new[] { 1 }.ToSourceKind(sourceKind);
		// ReSharper disable PossibleMultipleEnumeration
		Assert.AreSame(source.FallbackIfEmpty(12), source);
		Assert.AreSame(source.FallbackIfEmpty(12, 23), source);
		Assert.AreSame(source.FallbackIfEmpty(12, 23, 34), source);
		Assert.AreSame(source.FallbackIfEmpty(12, 23, 34, 45), source);
		Assert.AreSame(source.FallbackIfEmpty(12, 23, 34, 45, 56), source);
		Assert.AreSame(source.FallbackIfEmpty(12, 23, 34, 45, 56, 67), source);
		// ReSharper restore PossibleMultipleEnumeration
	}

	[TestCase(SourceKind.BreakingCollection)]
	[TestCase(SourceKind.BreakingReadOnlyCollection)]
	public void FallbackIfEmptyPreservesFallbackCollectionIfPossible(SourceKind sourceKind)
	{
		var source = Array.Empty<int>().ToSourceKind(sourceKind);
		var fallback = new[] { 1 };
		Assert.AreSame(source.FallbackIfEmpty(fallback), fallback);
		Assert.AreSame(source.FallbackIfEmpty(fallback.AsEnumerable()), fallback);
	}
}
