namespace Test.Async;

public class ScanExTest
{
	[Fact]
	public Task ScanExEmpty()
	{
		return AsyncEnumerable.Empty<int>().ScanEx((a, b) => a + b)
			.AssertEmpty();
	}

	[Fact]
	public Task ScanExSum()
	{
		var result = AsyncEnumerable.Range(1, 10).ScanEx((a, b) => a + b);
		return result.AssertSequenceEqual(1, 3, 6, 10, 15, 21, 28, 36, 45, 55);
	}

	[Fact]
	public void ScanExIsLazy()
	{
		new AsyncBreakingSequence<object>().ScanEx(BreakingFunc.Of<object, object, object>());
	}

	[Fact]
	public async Task ScanExDoesNotIterateExtra()
	{
		var sequence = AsyncEnumerable.Range(1, 3).Concat(new AsyncBreakingSequence<int>()).ScanEx((a, b) => a + b);
		await Assert.ThrowsAsync<NotSupportedException>(async () => await sequence.Consume());
		await sequence.Take(3).AssertSequenceEqual(1, 3, 6);
	}

	[Fact]
	public async Task SeededScanExEmpty()
	{
		Assert.Equal(-1, await AsyncEnumerable.Empty<int>().ScanEx(-1, (a, b) => a + b).SingleAsync());
	}

	[Fact]
	public Task SeededScanExSum()
	{
		var result = AsyncEnumerable.Range(1, 10).ScanEx(0, (a, b) => a + b);
		return result.AssertSequenceEqual(0, 1, 3, 6, 10, 15, 21, 28, 36, 45, 55);
	}

	[Fact]
	public void SeededScanExIsLazy()
	{
		new AsyncBreakingSequence<object>().ScanEx(seed: null,
			BreakingFunc.Of<object?, object, object>());
	}

	[Fact]
	public async Task SeededScanExDoesNotIterateExtra()
	{
		var sequence = AsyncEnumerable.Range(1, 3).Concat(new AsyncBreakingSequence<int>()).ScanEx(0, (a, b) => a + b);
		await Assert.ThrowsAsync<NotSupportedException>(async () => await sequence.Consume());
		await sequence.Take(4).AssertSequenceEqual(0, 1, 3, 6);
	}
}
