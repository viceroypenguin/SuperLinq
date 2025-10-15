namespace SuperLinq.Async.Tests;

public sealed class BatchTest
{
	[Fact]
	public void BatchIsLazy()
	{
		_ = new AsyncBreakingSequence<int>().Batch(1);
		_ = new AsyncBreakingSequence<int>().Buffer(1);
	}

	[Fact]
	public void BatchValidatesSize()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>("size",
			() => new AsyncBreakingSequence<int>()
				.Batch(0));
	}

	[Fact]
	public async Task BatchDoesNotReturnSameArrayInstance()
	{
		await using var seq = Enumerable.Range(1, 4).AsTestingSequence();
		await using var e = seq.Batch(2).GetAsyncEnumerator();

		_ = await e.MoveNextAsync();
		var batch1 = e.Current;
		_ = await e.MoveNextAsync();
		var batch2 = e.Current;

		Assert.NotEqual(batch1, batch2);
	}

	[Fact]
	public async Task BatchWithEmptySource()
	{
		await using var seq = Enumerable.Empty<int>().AsTestingSequence();
		var result = seq.Batch(1);
		await seq.AssertSequenceEqual();
	}

	[Fact]
	public async Task BatchEvenlyDivisibleSequence()
	{
		await using var seq = Enumerable.Range(1, 9).AsTestingSequence();

		var result = seq.Batch(3);
		await using var reader = result.Read();
		(await reader.Read()).AssertSequenceEqual(1, 2, 3);
		(await reader.Read()).AssertSequenceEqual(4, 5, 6);
		(await reader.Read()).AssertSequenceEqual(7, 8, 9);
		await reader.ReadEnd();
	}

	[Fact]
	public async Task BatchUnevenlyDivisibleSequence()
	{
		await using var seq = Enumerable.Range(1, 9).AsTestingSequence();

		var result = seq.Batch(4);
		await using var reader = result.Read();
		(await reader.Read()).AssertSequenceEqual(1, 2, 3, 4);
		(await reader.Read()).AssertSequenceEqual(5, 6, 7, 8);
		(await reader.Read()).AssertSequenceEqual(9);
		await reader.ReadEnd();
	}
}
