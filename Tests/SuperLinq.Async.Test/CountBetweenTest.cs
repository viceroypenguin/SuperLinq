namespace Test.Async;

public class CountBetweenTest
{
	[Fact]
	public Task CountBetweenWithNegativeMin()
	{
		return Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
			await AsyncSeq(1).CountBetween(-1, 0));
	}

	[Fact]
	public Task CountBetweenWithNegativeMax()
	{
		return Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
			await AsyncSeq(1).CountBetween(0, -1));
	}

	[Fact]
	public Task CountBetweenWithMaxLesserThanMin()
	{
		return Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
			await AsyncSeq(1).CountBetween(1, 0));
	}

	[Fact]
	public async ValueTask CountBetweenWithMaxEqualsMin()
	{
		Assert.True(await AsyncSeq(1).CountBetween(1, 1));
	}

	[Theory]
	[InlineData(1, 2, 4, false)]
	[InlineData(2, 2, 4, true)]
	[InlineData(3, 2, 4, true)]
	[InlineData(4, 2, 4, true)]
	[InlineData(5, 2, 4, false)]
	public async ValueTask CountBetweenRangeTests(int count, int min, int max, bool expecting)
	{
		Assert.Equal(expecting, await AsyncEnumerable.Range(1, count).CountBetween(min, max));
	}

	[Fact]
	public async ValueTask CountBetweenDoesNotIterateUnnecessaryElements()
	{
		Assert.False(await AsyncSeqExceptionAt(5).CountBetween(2, 3));
	}
}
