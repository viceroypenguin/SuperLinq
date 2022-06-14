using NUnit.Framework;

namespace Test;

[TestFixture]
public class PartialSortByTests
{
	[Test]
	public void PartialSortBy()
	{
		var ns = SuperEnumerable.RandomDouble().Take(10).ToArray();

		const int count = 5;
		var sorted = ns.Select((n, i) => KeyValuePair.Create(i, n))
					   .Reverse()
					   .PartialSortBy(count, e => e.Key);

		sorted.Select(e => e.Value).AssertSequenceEqual(ns.Take(count));
	}

	[Test]
	public void PartialSortWithOrder()
	{
		var ns = SuperEnumerable.RandomDouble().Take(10).ToArray();

		const int count = 5;
		var sorted = ns.Select((n, i) => KeyValuePair.Create(i, n))
						.Reverse()
						.PartialSortBy(count, e => e.Key, OrderByDirection.Ascending);

		sorted.Select(e => e.Value).AssertSequenceEqual(ns.Take(count));

		sorted = ns.Select((n, i) => KeyValuePair.Create(i, n))
					.Reverse()
					.PartialSortBy(count, e => e.Key, OrderByDirection.Descending);

		sorted.Select(e => e.Value).AssertSequenceEqual(ns.Reverse().Take(count));
	}

	[Test]
	public void PartialSortWithComparer()
	{
		var alphabet = Enumerable.Range(0, 26)
								 .Select((n, i) => ((char)((i % 2 == 0 ? 'A' : 'a') + n)).ToString())
								 .ToArray();

		var ns = alphabet.Zip(SuperEnumerable.RandomDouble(), KeyValuePair.Create).ToArray();
		var sorted = ns.PartialSortBy(5, e => e.Key, StringComparer.Ordinal);

		sorted.Select(e => e.Key[0]).AssertSequenceEqual('A', 'C', 'E', 'G', 'I');
	}

	[Test]
	public void PartialSortByIsLazy()
	{
		new BreakingSequence<object>().PartialSortBy(1, BreakingFunc.Of<object, object>());
	}

	[Test]
	public void PartialSortByIsStable()
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

		for (var i = 1; i <= 10; i++)
		{
			var sorted = list.PartialSortBy(i, x => x.key);
			Assert.True(sorted.SequenceEqual(stableSort.Take(i)));
		}
	}
}
