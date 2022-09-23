using SuperLinq;

namespace Test.Async;

public class DensePartialSortTests
{
	[Fact]
	public void DensePartialSortIsLazy()
	{
		new AsyncBreakingSequence<object>().DensePartialSort(1);
	}

	[Fact]
	public Task DensePartialSort()
	{
		var sorted = AsyncEnumerable.Range(1, 10)
			.Repeat(2)
			.Reverse()
			.Append(0)
			.DensePartialSort(3);

		return sorted.AssertSequenceEqual(0, 1, 1, 2, 2);
	}

	[Fact]
	public async Task DensePartialSortWithOrder()
	{
		var sorted = AsyncEnumerable.Range(1, 10)
			.Repeat(2)
			.Reverse()
			.Append(0)
			.DensePartialSort(3, OrderByDirection.Ascending);
		await sorted.AssertSequenceEqual(0, 1, 1, 2, 2);

		sorted = AsyncEnumerable.Range(1, 10)
			.Repeat(2)
			.Reverse()
			.Append(0)
			.DensePartialSort(3, OrderByDirection.Descending);
		await sorted.AssertSequenceEqual(10, 10, 9, 9, 8, 8);
	}

	[Fact]
	public Task DensePartialSortWithComparer()
	{
		var alphabet = Enumerable.Range(0, 26)
			.Repeat(2)
			.Select((n, i) => ((char)((i % 2 == 0 ? 'A' : 'a') + n)).ToString())
			.ToArray();

		var sorted = alphabet.ToAsyncEnumerable()
			.DensePartialSort(3, StringComparer.Ordinal);

		return sorted.Select(s => s[0]).AssertSequenceEqual('A', 'A', 'C', 'C', 'E', 'E');
	}
}
