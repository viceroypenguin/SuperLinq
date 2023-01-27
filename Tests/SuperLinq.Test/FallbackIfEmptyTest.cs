namespace Test;

public class FallbackIfEmptyTest
{
	[Fact]
	public void FallbackIfEmptyWithEmptySequence()
	{
		using var source = Enumerable.Empty<int>().AsTestingSequence(maxEnumerations: 2);
		source.FallbackIfEmpty(12).AssertSequenceEqual(12);
		source.FallbackIfEmpty(12, 23).AssertSequenceEqual(12, 23);
	}

	[Fact]
	public void FallbackIfEmptyWithNotEmptySequence()
	{
		using var source = Seq(1).AsTestingSequence(maxEnumerations: 2);
		source.FallbackIfEmpty(12).AssertSequenceEqual(1);
		source.FallbackIfEmpty(12, 23).AssertSequenceEqual(1);
	}
}
