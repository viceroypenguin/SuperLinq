namespace Test.Async;

public class PartialSortByTests
{
	[Fact]
	public async Task PartialSortBy()
	{
		var ns = SuperLinq.SuperEnumerable.RandomDouble().Take(10).ToArray();

		const int Count = 5;
		var sorted = ns
			.ToAsyncEnumerable()
			.Select((n, i) => KeyValuePair.Create(i, n))
			.Reverse()
			.PartialSortBy(Count, e => e.Key);

		await sorted.Select(e => e.Value).AssertSequenceEqual(ns.Take(Count));
	}

	[Fact]
	public async Task PartialSortWithOrder()
	{
		var ns = SuperLinq.SuperEnumerable.RandomDouble().Take(10).ToArray();

		const int Count = 5;
		var sorted = ns
			.ToAsyncEnumerable()
			.Select((n, i) => KeyValuePair.Create(i, n))
			.Reverse()
			.PartialSortBy(Count, e => e.Key, OrderByDirection.Ascending);

		await sorted.Select(e => e.Value).AssertSequenceEqual(ns.Take(Count));

		sorted = ns
			.ToAsyncEnumerable()
			.Select((n, i) => KeyValuePair.Create(i, n))
			.Reverse()
			.PartialSortBy(Count, e => e.Key, OrderByDirection.Descending);

		await sorted.Select(e => e.Value).AssertSequenceEqual(ns.Reverse().Take(Count));
	}

	[Fact]
	public async Task PartialSortWithComparer()
	{
		var alphabet = AsyncEnumerable.Range(0, 26)
			.Select((n, i) => ((char)((i % 2 == 0 ? 'A' : 'a') + n)).ToString());

		var ns = alphabet.Zip(AsyncSuperEnumerable.RandomDouble(), KeyValuePair.Create);
		var sorted = ns.PartialSortBy(5, e => e.Key, StringComparer.Ordinal);

		await sorted.Select(e => e.Key[0]).AssertSequenceEqual('A', 'C', 'E', 'G', 'I');
	}

	[Fact]
	public void PartialSortByIsLazy()
	{
		new AsyncBreakingSequence<object>().PartialSortBy(1, BreakingFunc.Of<object, object>());
	}

	[Fact]
	public async Task PartialSortByIsStable()
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

		for (var i = 1; i <= 10; i++)
		{
			var sorted = list.PartialSortBy(i, x => x.key);
			await sorted.AssertSequenceEqual(stableSort.Take(i));
		}
	}
}
