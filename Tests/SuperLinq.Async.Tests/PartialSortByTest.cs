namespace SuperLinq.Async.Tests;

public sealed class PartialSortByTests
{
	[Test]
	public async Task PartialSortBy()
	{
		var ns = SuperEnumerable.RandomDouble()
			.Take(10).ToArray();
		await using var sequence = ns.Index()
			.Reverse().AsTestingSequence();

		await sequence
			.PartialSortBy(5, e => e.Index)
			.Select(e => e.Item)
			.AssertSequenceEqual(ns.Take(5));
	}

	[Test]
	public async Task PartialSortWithOrder()
	{
		var ns = SuperEnumerable.RandomDouble()
			.Take(10).ToArray();
		await using var sequence = ns.Index()
			.Reverse().AsTestingSequence(maxEnumerations: 5);

		await sequence
			.PartialSortBy(5, e => e.Index, OrderByDirection.Ascending)
			.Select(e => e.Item)
			.AssertSequenceEqual(ns.Take(5));

		await sequence
			.PartialSortBy(5, e => e.Index, OrderByDirection.Descending)
			.Select(e => e.Item)
			.AssertSequenceEqual(ns.Reverse().Take(5));
	}

	[Test]
	public async Task PartialSortWithComparer()
	{
		await using var alphabet = Enumerable.Range(0, 26)
			.Select((n, i) => ((char)((i % 2 == 0 ? 'A' : 'a') + n)).ToString())
			.Zip(SuperEnumerable.RandomDouble())
			.AsTestingSequence();

		await alphabet
			.PartialSortBy(5, e => e.First, StringComparer.Ordinal)
			.Select(x => x.First[0])
			.AssertSequenceEqual('A', 'C', 'E', 'G', 'I');
	}

	[Test]
	public void PartialSortByIsLazy()
	{
		_ = new AsyncBreakingSequence<object>().PartialSortBy(1, BreakingFunc.Of<object, object>());
	}

	[Test]
	public async Task PartialSortByIsStable()
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

		for (var i = 1; i <= 10; i++)
		{
			var sorted = list.PartialSortBy(i, x => x.key);
			await sorted.AssertSequenceEqual(stableSort.Take(i));
		}
	}
}
