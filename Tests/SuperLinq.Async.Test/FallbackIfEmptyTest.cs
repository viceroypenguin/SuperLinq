﻿namespace Test.Async;

public class FallbackIfEmptyTest
{
	[Fact]
	public async Task FallbackIfEmptyWithEmptySequence()
	{
		await using (var source = AsyncEnumerable.Empty<int>().AsTestingSequence())
			await source.FallbackIfEmpty(12).AssertSequenceEqual(12);
		await using (var source = AsyncEnumerable.Empty<int>().AsTestingSequence())
			await source.FallbackIfEmpty(12, 23).AssertSequenceEqual(12, 23);
	}

	[Fact]
	public async Task FallbackIfEmptyWithNotEmptySequence()
	{
		await using (var source = AsyncSeq(1).AsTestingSequence())
			await source.FallbackIfEmpty(12).AssertSequenceEqual(1);
		await using (var source = AsyncSeq(1).AsTestingSequence())
			await source.FallbackIfEmpty(12, 23).AssertSequenceEqual(1);
	}
}
