namespace Test;

public class DensePartialSortByTests
{
	[Fact]
	public void DensePartialSortByIsLazy()
	{
		new BreakingSequence<object>().DensePartialSortBy(1, BreakingFunc.Of<object, object>());
	}

	[Fact]
	public void DensePartialSortBy()
	{
		var ns = SuperEnumerable.RandomDouble().Take(10).ToArray();

		var sorted = ns.Select((n, i) => KeyValuePair.Create(i, n))
			.Repeat(2)
			.Reverse()
			.DensePartialSortBy(3, e => e.Key);

		sorted.Select(e => e.Value).AssertSequenceEqual(
			ns.Take(3).SelectMany(x => new[] { x, x, }));
	}

	[Fact]
	public void DensePartialSortWithOrder()
	{
		var ns = SuperEnumerable.RandomDouble().Take(10).ToArray();

		var sorted = ns.Select((n, i) => KeyValuePair.Create(i, n))
			.Repeat(2)
			.Reverse()
			.DensePartialSortBy(3, e => e.Key, OrderByDirection.Ascending);

		sorted.Select(e => e.Value).AssertSequenceEqual(
			ns.Take(3).SelectMany(x => new[] { x, x, }));

		sorted = ns.Select((n, i) => KeyValuePair.Create(i, n))
			.Repeat(2)
			.Reverse()
			.DensePartialSortBy(3, e => e.Key, OrderByDirection.Descending);

		sorted.Select(e => e.Value).AssertSequenceEqual(
			ns.Reverse().Take(3).SelectMany(x => new[] { x, x, }));
	}

	[Fact]
	public void DensePartialSortWithComparer()
	{
		var alphabet = Enumerable.Range(0, 26)
			.Repeat(2)
			.Select((n, i) => ((char)((i % 2 == 0 ? 'A' : 'a') + n)).ToString())
			.ToArray();

		var ns = alphabet.Zip(SuperEnumerable.RandomDouble(), KeyValuePair.Create).ToArray();
		var sorted = ns.DensePartialSortBy(3, e => e.Key, StringComparer.Ordinal);

		sorted.Select(e => e.Key[0]).AssertSequenceEqual('A', 'A', 'C', 'C', 'E', 'E');
	}
}
