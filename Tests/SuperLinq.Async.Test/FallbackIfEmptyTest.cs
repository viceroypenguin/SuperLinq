namespace Test.Async;

public class FallbackIfEmptyTest
{
	[Fact]
	public async Task FallbackIfEmptyWithEmptySequence()
	{
		var source = AsyncEnumerable.Empty<int>().Select(x => x);
		await source.FallbackIfEmpty(12).AssertSequenceEqual(12);
		await source.FallbackIfEmpty(12, 23).AssertSequenceEqual(12, 23);
	}

	[Fact]
	public async Task FallbackIfEmptyWithNotEmptySequence()
	{
		var source = Seq(1);
		Assert.Equal(source, await source.ToAsyncEnumerable().FallbackIfEmpty(12).ToListAsync());
		Assert.Equal(source, await source.ToAsyncEnumerable().FallbackIfEmpty(12, 23).ToListAsync());
	}
}
