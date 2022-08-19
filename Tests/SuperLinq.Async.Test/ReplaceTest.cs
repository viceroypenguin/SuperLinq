namespace Test.Async;

public class ReplaceTest
{
	[Fact]
	public void ReplaceIsLazy()
	{
		new AsyncBreakingSequence<int>().Replace(0, 10);
		new AsyncBreakingSequence<int>().Replace(^0, 10);
	}

	[Fact]
	public async Task ReplaceEmptySequence()
	{
		await AsyncEnumerable.Empty<int>().Replace(0, 10).AssertEmpty();
		await AsyncEnumerable.Empty<int>().Replace(10, 10).AssertEmpty();
		await AsyncEnumerable.Empty<int>().Replace(^0, 10).AssertEmpty();
		await AsyncEnumerable.Empty<int>().Replace(^10, 10).AssertEmpty();
	}

	[Fact]
	public async Task ReplaceStartIndex()
	{
		for (var i = 0; i < 10; i++)
			await AsyncEnumerable.Range(1, 10).Replace(i, 30)
				.AssertSequenceEqual(
					Enumerable.Range(1, i)
						.Append(30)
						.Concat(Enumerable.Range(i + 2, 9 - i)));
	}

	[Fact]
	public async Task ReplaceEndIndex()
	{
		for (var i = 0; i < 10; i++)
			await AsyncEnumerable.Range(1, 10).Replace(^i, 30)
				.AssertSequenceEqual(
					Enumerable.Range(1, 9 - i)
						.Append(30)
						.Concat(Enumerable.Range(11 - i, i)));
	}

	[Fact]
	public async Task ReplaceStartIndexPastSequenceLength()
	{
		await AsyncEnumerable.Range(1, 10).Replace(10, 30)
			.AssertSequenceEqual(Enumerable.Range(1, 10));
	}

	[Fact]
	public async Task ReplaceEndIndexPastSequenceLength()
	{
		await AsyncEnumerable.Range(1, 10).Replace(^10, 30)
			.AssertSequenceEqual(Enumerable.Range(1, 10));
	}
}
