namespace Test.Async;

public class CountBetweenTest
{
	[Fact]
	public Task CountBetweenWithNegativeMin()
	{
		return Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
			await new AsyncBreakingSequence<int>().CountBetween(-1, 0));
	}

	[Fact]
	public Task CountBetweenWithNegativeMax()
	{
		return Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
			await new AsyncBreakingSequence<int>().CountBetween(0, -1));
	}

	[Fact]
	public Task CountBetweenWithMaxLesserThanMin()
	{
		return Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
			await new AsyncBreakingSequence<int>().CountBetween(1, 0));
	}

	[Fact]
	public async Task CountBetweenWithMaxEqualsMin()
	{
		await using var xs = TestingSequence.Of(1);
		Assert.True(await xs.CountBetween(1, 1));
	}

	[Theory]
	[InlineData(1, 2, 4, false)]
	[InlineData(2, 2, 4, true)]
	[InlineData(3, 2, 4, true)]
	[InlineData(4, 2, 4, true)]
	[InlineData(5, 2, 4, false)]
	public async Task CountBetweenRangeTests(int count, int min, int max, bool expecting)
	{
		await using var xs = AsyncEnumerable.Range(1, count).AsTestingSequence();
		Assert.Equal(expecting, await xs.CountBetween(min, max));
	}

	[Fact]
	public async Task CountBetweenDoesNotIterateUnnecessaryElements()
	{
		await using var xs = AsyncSeqExceptionAt(5).AsTestingSequence();
		Assert.False(await xs.CountBetween(2, 3));
	}
}
