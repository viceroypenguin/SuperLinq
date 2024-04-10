namespace Test.Async;

public sealed class DensePartialSortTests
{
	[Fact]
	public void DensePartialSortIsLazy()
	{
		_ = new AsyncBreakingSequence<object>().DensePartialSort(1);
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

	[Fact]
	public async Task DensePartialSortIsStable()
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
		}.AsTestingSequence(maxEnumerations: 5);

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

		for (var i = 1; i <= 5; i++)
		{
			var sorted = list.DensePartialSort(i, comparer);
			await sorted.AssertSequenceEqual(
				stableSort.Take(i * 2));
		}
	}
}
