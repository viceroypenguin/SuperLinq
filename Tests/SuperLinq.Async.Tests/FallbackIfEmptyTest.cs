namespace SuperLinq.Async.Tests;

public sealed class FallbackIfEmptyTest
{
	[Fact]
	public async Task FallbackIfEmptyWithEmptySequence()
	{
		await using var source = AsyncEnumerable.Empty<int>().AsTestingSequence(maxEnumerations: 2);
		await source.FallbackIfEmpty(12).AssertSequenceEqual(12);
		await source.FallbackIfEmpty(12, 23).AssertSequenceEqual(12, 23);
	}

	[Fact]
	public async Task FallbackIfEmptyWithNotEmptySequence()
	{
		await using var source = AsyncSeq(1).AsTestingSequence(maxEnumerations: 2);
		await source.FallbackIfEmpty(12).AssertSequenceEqual(1);
		await source.FallbackIfEmpty(12, 23).AssertSequenceEqual(1);
	}
}
