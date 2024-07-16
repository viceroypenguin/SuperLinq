namespace Test.Async;

public sealed class PartialSortTests
{
	[Fact]
	public async Task PartialSort()
	{
		await using var sequence = Enumerable.Range(1, 10)
			.Reverse()
			.Concat([0])
			.AsTestingSequence();

		await sequence
			.PartialSort(5)
			.AssertSequenceEqual(Enumerable.Range(0, 5));
	}

	[Fact]
	public async Task PartialSortWithOrder()
	{
		await using var sequence = Enumerable.Range(1, 10)
			.Reverse()
			.Concat([0])
			.AsTestingSequence(maxEnumerations: 2);

		await sequence
			.PartialSort(5, OrderByDirection.Ascending)
			.AssertSequenceEqual(Enumerable.Range(0, 5));
		await sequence
			.PartialSort(5, OrderByDirection.Descending)
			.AssertSequenceEqual(Enumerable.Range(6, 5).Reverse());
	}

	[Fact]
	public async Task PartialSortWithDuplicates()
	{
		await using var sequence = Enumerable.Range(1, 10)
			.Reverse()
			.Concat(Enumerable.Repeat(3, 3))
			.AsTestingSequence();

		await sequence
			.PartialSort(5)
			.AssertSequenceEqual(1, 2, 3, 3, 3);
	}

	[Fact]
	public async Task PartialSortWithComparer()
	{
		await using var sequence = Enumerable.Range(0, 26)
			.Select((n, i) => ((char)((i % 2 == 0 ? 'A' : 'a') + n)).ToString())
			.AsTestingSequence();

		await sequence
			.PartialSort(5, StringComparer.Ordinal)
			.Select(s => s[0])
			.AssertSequenceEqual('A', 'C', 'E', 'G', 'I');
	}

	[Fact]
	public void PartialSortIsLazy()
	{
		_ = new AsyncBreakingSequence<object>().PartialSort(1);
	}

	[Fact]
	public async Task PartialSortIsStable()
	{
		await using var list = new[]
		{
			(key: 5, text: "1"),
			(key: 5, text: "2"),
			(key: 4, text: "3"),
			(key: 4, text: "4"),
			(key: 3, text: "5"),
			(key: 3, text: "6"),
			(key: 2, text: "7"),
			(key: 2, text: "8"),
			(key: 1, text: "9"),
			(key: 1, text: "10"),
		}.AsTestingSequence(maxEnumerations: 10);

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
