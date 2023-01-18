namespace Test;

public class FallbackIfEmptyTest
{
	[Fact]
	public void FallbackIfEmptyWithEmptySequence()
	{
		using (var source = Enumerable.Empty<int>().AsTestingSequence())
			source.FallbackIfEmpty(12).AssertSequenceEqual(12);
		using (var source = Enumerable.Empty<int>().AsTestingSequence())
			source.FallbackIfEmpty(12, 23).AssertSequenceEqual(12, 23);
	}

	[Fact]
	public void FallbackIfEmptyWithNotEmptySequence()
	{
		using (var source = Seq(1).AsTestingSequence())
			source.FallbackIfEmpty(12).AssertSequenceEqual(1);
		using (var source = Seq(1).AsTestingSequence())
			source.FallbackIfEmpty(12, 23).AssertSequenceEqual(1);
	}
}
