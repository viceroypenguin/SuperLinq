namespace Test.Async;

public class FallbackIfEmptyTest
{
	[Fact]
	public async Task FallbackIfEmptyWithEmptySequence()
	{
		await using var source = AsyncEnumerable.Empty<int>().AsTestingSequence(2);
		await source.FallbackIfEmpty(12).AssertSequenceEqual(12);
		await source.FallbackIfEmpty(12, 23).AssertSequenceEqual(12, 23);
	}

	[Fact]
	public async Task FallbackIfEmptyWithNotEmptySequence()
	{
		await using var source = AsyncSeq(1).AsTestingSequence(2);
		await source.FallbackIfEmpty(12).AssertSequenceEqual(1);
		await source.FallbackIfEmpty(12, 23).AssertSequenceEqual(1);
	}
}
