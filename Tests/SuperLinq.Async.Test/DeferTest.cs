﻿namespace Test.Async;

public sealed class DeferTest
{
	[Fact]
	public void DeferIsLazy()
	{
		_ = AsyncSuperEnumerable.Defer(BreakingFunc.Of<IAsyncEnumerable<int>>());
	}

	[Fact]
	public async Task DeferBehavior()
	{
		var starts = 0;
		var length = 5;

		var seq = AsyncSuperEnumerable.Defer(() =>
		{
			starts++;
			return AsyncEnumerable.Range(1, length);
		});

		Assert.Equal(0, starts);

		await seq.AssertSequenceEqual(Enumerable.Range(1, length));
		Assert.Equal(1, starts);

		length = 10;
		await seq.AssertSequenceEqual(Enumerable.Range(1, length));
		Assert.Equal(2, starts);
	}
}
