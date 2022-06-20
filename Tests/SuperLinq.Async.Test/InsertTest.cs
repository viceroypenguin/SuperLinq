namespace Test.Async;

public class InsertTest
{
	[Fact]
	public async ValueTask InsertWithNegativeIndex()
	{
		await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
			await AsyncEnumerable.Range(1, 10)
				.Insert(new[] { 97, 98, 99 }.ToAsyncEnumerable(), -1)
				.ToListAsync());
	}

	[Theory]
	[InlineData(7)]
	[InlineData(8)]
	[InlineData(9)]
	public async ValueTask InsertWithIndexGreaterThanSourceLengthMaterialized(int count)
	{
		var seq1 = AsyncEnumerable.Range(0, count);
		var seq2 = new[] { 97, 98, 99 }.ToAsyncEnumerable();

		var result = seq1.Insert(seq2, count + 1);

		await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
			await result.ForEachAsync((e, index) =>
				Assert.Equal(index, e)));
	}

	[Theory]
	[InlineData(7)]
	[InlineData(8)]
	[InlineData(9)]
	public async Task InsertWithIndexGreaterThanSourceLengthLazy(int count)
	{
		var seq1 = AsyncEnumerable.Range(0, count);
		var seq2 = new[] { 97, 98, 99 }.ToAsyncEnumerable();

		var result = seq1.Insert(seq2, count + 1).Take(count);

		Assert.Equal(await seq1.ToListAsync(), await result.ToListAsync());
	}

	[Theory]
	[InlineData(3, 0)]
	[InlineData(3, 1)]
	[InlineData(3, 2)]
	[InlineData(3, 3)]
	public async ValueTask Insert(int count, int index)
	{
		var seq1 = AsyncEnumerable.Range(1, count);
		var seq2 = new[] { 97, 98, 99 }.ToAsyncEnumerable();

		var result = seq1.Insert(seq2, index);

		var expectations = seq1.Take(index).Concat(seq2)
			.Concat(seq1.Skip(index));

		Assert.Equal(await expectations.ToListAsync(), await result.ToListAsync());
	}

	[Fact]
	public void InsertIsLazy()
	{
		new AsyncBreakingSequence<int>().Insert(new AsyncBreakingSequence<int>(), 0);
	}

	[Fact]
	public async ValueTask InsertDisposesEnumerators()
	{
		await using var seq1 = TestingSequence.Of(1);
		await using var seq2 = TestingSequence.Of(2);
		await seq1.Insert(seq2, 0).ToListAsync();
	}
}
