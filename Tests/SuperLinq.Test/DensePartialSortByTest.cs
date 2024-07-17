namespace Test;

public sealed class DensePartialSortByTests
{
	[Fact]
	public void DensePartialSortByIsLazy()
	{
		_ = new BreakingSequence<object>().DensePartialSortBy(1, BreakingFunc.Of<object, object>());
	}

	[Fact]
	public void DensePartialSortBy()
	{
		var ns = SuperEnumerable.RandomDouble().Take(10).ToArray();

		using var xs = ns.Select((n, i) => CreatePair(i, n))
			.Repeat(2)
			.Reverse()
			.AsTestingSequence();

		var sorted = xs.DensePartialSortBy(3, e => e.Key);

		sorted.Select(e => e.Value).AssertSequenceEqual(
			ns.Take(3).SelectMany(x => new[] { x, x }));
	}

	[Theory]
	[InlineData(OrderByDirection.Ascending)]
	[InlineData(OrderByDirection.Descending)]
	public void DensePartialSortWithOrder(OrderByDirection direction)
	{
		var ns = SuperEnumerable.RandomDouble()
			.Take(10)
			.ToArray().AsEnumerable();

		using var xs = ns.Select((n, i) => CreatePair(i, n))
			.Repeat(2)
			.Reverse()
			.AsTestingSequence();

		var sorted = xs.DensePartialSortBy(3, e => e.Key, direction);

		if (direction == OrderByDirection.Descending)
			ns = ns.Reverse();

		sorted.Select(e => e.Value).AssertSequenceEqual(
			ns.Take(3).SelectMany(x => new[] { x, x }));
	}

	[Fact]
	public void DensePartialSortWithComparer()
	{
		var alphabet = Enumerable.Range(0, 26)
			.Repeat(2)
			.Select((n, i) => ((char)((i % 2 == 0 ? 'A' : 'a') + n)).ToString())
			.ToArray();

		using var ns = alphabet
			.Zip(SuperEnumerable.RandomDouble(), CreatePair)
			.AsTestingSequence();

		ns
			.DensePartialSortBy(3, e => e.Key, StringComparer.Ordinal)
			.Select(e => e.Key[0])
			.AssertSequenceEqual('A', 'A', 'C', 'C', 'E', 'E');
	}

	[Fact]
	public void DensePartialSortByIsStable()
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

		for (var i = 1; i <= 5; i++)
		{
			var sorted = list.DensePartialSortBy(i, x => x.key);
			sorted.AssertSequenceEqual(
				stableSort.Take(i * 2));
		}
	}

	private static KeyValuePair<TKey, TValue> CreatePair<TKey, TValue>(TKey key, TValue value) => new(key, value);
}
