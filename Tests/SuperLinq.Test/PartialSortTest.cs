namespace Test;

public class PartialSortTests
{
	[Fact]
	public void PartialSort()
	{
		var sorted = Enumerable.Range(1, 10)
							   .Reverse()
							   .Append(0)
							   .PartialSort(5);

		sorted.AssertSequenceEqual(Enumerable.Range(0, 5));
	}

	[Fact]
	public void PartialSortWithOrder()
	{
		var sorted = Enumerable.Range(1, 10)
								.Reverse()
								.Append(0)
								.PartialSort(5, OrderByDirection.Ascending);

		sorted.AssertSequenceEqual(Enumerable.Range(0, 5));
		sorted = Enumerable.Range(1, 10)
							.Reverse()
							.Append(0)
							.PartialSort(5, OrderByDirection.Descending);
		sorted.AssertSequenceEqual(Enumerable.Range(6, 5).Reverse());
	}

	[Fact]
	public void PartialSortWithDuplicates()
	{
		var sorted = Enumerable.Range(1, 10)
							   .Reverse()
							   .Concat(Enumerable.Repeat(3, 3))
							   .PartialSort(5);

		sorted.AssertSequenceEqual(1, 2, 3, 3, 3);
	}

	[Fact]
	public void PartialSortWithComparer()
	{
		var alphabet = Enumerable.Range(0, 26)
								 .Select((n, i) => ((char)((i % 2 == 0 ? 'A' : 'a') + n)).ToString())
								 .ToArray();

		var sorted = alphabet.PartialSort(5, StringComparer.Ordinal);

		sorted.Select(s => s[0]).AssertSequenceEqual('A', 'C', 'E', 'G', 'I');
	}

	[Fact]
	public void PartialSortIsLazy()
	{
		new BreakingSequence<object>().PartialSort(1);
	}

	[Fact]
	public void PartialSortIsStable()
	{
		var list = new[]
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
		};

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

		var comparer = Comparer.Create<(int key, string text)>((a, b) => a.key.CompareTo(b.key));

		for (var i = 1; i <= 10; i++)
		{
			var sorted = list.PartialSort(i, comparer);
			Assert.Equal(stableSort.Take(i), sorted);
		}
	}
}
