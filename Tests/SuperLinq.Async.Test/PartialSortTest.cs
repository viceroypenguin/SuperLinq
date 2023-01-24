namespace Test.Async;

public class PartialSortTests
{
	[Fact]
	public async Task PartialSort()
	{
		var sorted = AsyncEnumerable.Range(1, 10)
			.Reverse()
			.Append(0)
			.PartialSort(5);

		await sorted.AssertSequenceEqual(Enumerable.Range(0, 5));
	}

	[Fact]
	public async Task PartialSortWithOrder()
	{
		var sorted = AsyncEnumerable.Range(1, 10)
			.Reverse()
			.Append(0)
			.PartialSort(5, OrderByDirection.Ascending);
		await sorted.AssertSequenceEqual(Enumerable.Range(0, 5));

		sorted = AsyncEnumerable.Range(1, 10)
			.Reverse()
			.Append(0)
			.PartialSort(5, OrderByDirection.Descending);
		await sorted.AssertSequenceEqual(Enumerable.Range(6, 5).Reverse());
	}

	[Fact]
	public async Task PartialSortWithDuplicates()
	{
		var sorted = AsyncEnumerable.Range(1, 10)
			.Reverse()
			.Concat(AsyncEnumerable.Repeat(3, 3))
			.PartialSort(5);

		await sorted.AssertSequenceEqual(1, 2, 3, 3, 3);
	}

	[Fact]
	public async Task PartialSortWithComparer()
	{
		var sorted = AsyncEnumerable.Range(0, 26)
			.Select((n, i) => ((char)((i % 2 == 0 ? 'A' : 'a') + n)).ToString())
			.PartialSort(5, StringComparer.Ordinal);

		await sorted.Select(s => s[0]).AssertSequenceEqual('A', 'C', 'E', 'G', 'I');
	}

	[Fact]
	public void PartialSortIsLazy()
	{
		new AsyncBreakingSequence<object>().PartialSort(1);
	}

	[Fact]
	public async Task PartialSortIsStable()
	{
		var list = AsyncSeq(
			(key: 5, text: "1"),
			(key: 5, text: "2"),
			(key: 4, text: "3"),
			(key: 4, text: "4"),
			(key: 3, text: "5"),
			(key: 3, text: "6"),
			(key: 2, text: "7"),
			(key: 2, text: "8"),
			(key: 1, text: "9"),
			(key: 1, text: "10"));

		var stableSort = new[]
		{
			(key: 1, text: "9"),
			(key: 1, text: "10"),
			(key: 2, text: "7"),
			(key: 2, text: "8"),
			(key: 3, text: "5"),
			(key: 3, text: "6"),
			(key: 4, text: "3"),
			(key: 4, text: "4"),
			(key: 5, text: "1"),
			(key: 5, text: "2"),
		};

		var comparer = Comparer<(int key, string text)>.Create((a, b) => a.key.CompareTo(b.key));

		for (var i = 1; i <= 10; i++)
		{
			var sorted = list.PartialSort(i, comparer);
			await sorted.AssertSequenceEqual(stableSort.Take(i));
		}
	}
}
