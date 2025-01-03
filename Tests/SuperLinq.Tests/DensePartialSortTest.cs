namespace SuperLinq.Tests;

public sealed class DensePartialSortTests
{
	[Test]
	public void DensePartialSortIsLazy()
	{
		_ = new BreakingSequence<object>().DensePartialSort(1);
	}

	[Test]
	public void DensePartialSort()
	{
		using var xs = Enumerable.Range(1, 10)
			.Repeat(2)
			.Reverse()
			.Concat([0])
			.AsTestingSequence();

		xs
			.DensePartialSort(3)
			.AssertSequenceEqual(0, 1, 1, 2, 2);
	}

	[Test]
	[Arguments(OrderByDirection.Ascending)]
	[Arguments(OrderByDirection.Descending)]
	public void DensePartialSortWithOrder(OrderByDirection direction)
	{
		using var xs = Enumerable.Range(1, 10)
			.Repeat(2)
			.Reverse()
			.Concat([0])
			.AsTestingSequence();

		var sorted = xs.DensePartialSort(3, direction);
		if (direction == OrderByDirection.Descending)
			sorted.AssertSequenceEqual(10, 10, 9, 9, 8, 8);
		else
			sorted.AssertSequenceEqual(0, 1, 1, 2, 2);
	}

	[Test]
	public void DensePartialSortWithComparer()
	{
		var alphabet = Enumerable.Range(0, 26)
			.Repeat(2)
			.Select((n, i) => ((char)((i % 2 == 0 ? 'A' : 'a') + n)).ToString())
			.ToArray();

		using var xs = alphabet
			.AsTestingSequence();

		xs
			.DensePartialSort(3, StringComparer.Ordinal)
			.Select(s => s[0])
			.AssertSequenceEqual('A', 'A', 'C', 'C', 'E', 'E');
	}

	[Test]
	public void DensePartialSortIsStable()
	{
		using var list = new[]
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
			sorted.AssertSequenceEqual(
				stableSort.Take(i * 2));
		}
	}
}
