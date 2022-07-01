namespace Test;

public class FallbackIfEmptyTest
{
	[Fact]
	public void FallbackIfEmptyWithEmptySequence()
	{
		var source = Enumerable.Empty<int>().Select(x => x);
		source.FallbackIfEmpty(12).AssertSequenceEqual(12);
		source.FallbackIfEmpty(12, 23).AssertSequenceEqual(12, 23);
	}

	[Fact]
	public void FallbackIfEmptyWithNotEmptySequence()
	{
		var source = Seq(1);
		Assert.Equal(source, source.FallbackIfEmpty(12));
		Assert.Equal(source, source.FallbackIfEmpty(12, 23));
	}
}
