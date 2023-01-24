namespace Test.Async;

public class DensePartialSortTests
{
	[Fact]
	public void DensePartialSortIsLazy()
	{
		new AsyncBreakingSequence<object>().DensePartialSort(1);
	}

	[Fact]
	public async Task DensePartialSort()
	{
		await using var xs = AsyncEnumerable.Range(1, 10)
			.Repeat(2)
			.Reverse()
			.Append(0)
			.AsTestingSequence();

		await xs
			.DensePartialSort(3)
			.AssertSequenceEqual(0, 1, 1, 2, 2);
	}

	[Theory]
	[InlineData(OrderByDirection.Ascending)]
	[InlineData(OrderByDirection.Descending)]
	public async Task DensePartialSortWithOrder(OrderByDirection direction)
	{
		await using var xs = AsyncEnumerable.Range(1, 10)
			.Repeat(2)
			.Reverse()
			.Append(0)
			.AsTestingSequence();

		var sorted = xs.DensePartialSort(3, direction);
		if (direction == OrderByDirection.Descending)
			await sorted.AssertSequenceEqual(10, 10, 9, 9, 8, 8);
		else
			await sorted.AssertSequenceEqual(0, 1, 1, 2, 2);
	}

	[Fact]
	public async Task DensePartialSortWithComparer()
	{
		var alphabet = Enumerable.Range(0, 26)
			.Repeat(2)
			.Select((n, i) => ((char)((i % 2 == 0 ? 'A' : 'a') + n)).ToString())
			.ToArray();

		await using var xs = alphabet
			.AsTestingSequence();

		await xs
			.DensePartialSort(3, StringComparer.Ordinal)
			.Select(s => s[0])
			.AssertSequenceEqual('A', 'A', 'C', 'C', 'E', 'E');
	}
}
