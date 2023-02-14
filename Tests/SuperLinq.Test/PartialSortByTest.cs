namespace Test;

public class PartialSortByTests
{
	[Fact]
	public void PartialSortBy()
	{
		var ns = SuperEnumerable.RandomDouble()
			.Take(10).ToArray();
		using var sequence = ns.Index()
			.Reverse().AsTestingSequence();

		sequence
			.PartialSortBy(5, e => e.index)
			.Select(e => e.item)
			.AssertSequenceEqual(ns.Take(5));
	}

	[Fact]
	public void PartialSortWithOrder()
	{
		var ns = SuperEnumerable.RandomDouble()
			.Take(10).ToArray();
		using var sequence = ns.Index()
			.Reverse().AsTestingSequence(maxEnumerations: 5);

		sequence
			.PartialSortBy(5, e => e.index, OrderByDirection.Ascending)
			.Select(e => e.item)
			.AssertSequenceEqual(ns.Take(5));

		sequence
			.PartialSortBy(5, e => e.index, OrderByDirection.Descending)
			.Select(e => e.item)
			.AssertSequenceEqual(ns.Reverse().Take(5));
	}

	[Fact]
	public void PartialSortWithComparer()
	{
		using var alphabet = Enumerable.Range(0, 26)
			.Select((n, i) => ((char)((i % 2 == 0 ? 'A' : 'a') + n)).ToString())
			.Zip(SuperEnumerable.RandomDouble())
			.AsTestingSequence();

		alphabet
			.PartialSortBy(5, e => e.First, StringComparer.Ordinal)
			.Select(x => x.First[0])
			.AssertSequenceEqual('A', 'C', 'E', 'G', 'I');
	}

	[Fact]
	public void PartialSortByIsLazy()
	{
		_ = new BreakingSequence<object>().PartialSortBy(1, BreakingFunc.Of<object, object>());
	}

	[Fact]
	public void PartialSortByIsStable()
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

		for (var i = 1; i <= 10; i++)
		{
			var sorted = list.PartialSortBy(i, x => x.key);
			Assert.Equal(stableSort.Take(i), sorted);
		}
	}
}
