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

		using var xs = ns.Select((n, i) => KeyValuePair.Create(i, n))
			.Repeat(2)
			.Reverse()
			.AsTestingSequence();

		var sorted = xs.DensePartialSortBy(3, e => e.Key);

		sorted.Select(e => e.Value).AssertSequenceEqual(
			ns.Take(3).SelectMany(x => new[] { x, x, }));
	}

	[Theory]
	[InlineData(OrderByDirection.Ascending)]
	[InlineData(OrderByDirection.Descending)]
	public void DensePartialSortWithOrder(OrderByDirection direction)
	{
		var ns = SuperEnumerable.RandomDouble()
			.Take(10)
			.ToArray().AsEnumerable();

		using var xs = ns.Select((n, i) => KeyValuePair.Create(i, n))
			.Repeat(2)
			.Reverse()
			.AsTestingSequence();

		var sorted = xs.DensePartialSortBy(3, e => e.Key, direction);

		if (direction == OrderByDirection.Descending)
			ns = ns.Reverse();

		sorted.Select(e => e.Value).AssertSequenceEqual(
			ns.Take(3).SelectMany(x => new[] { x, x, }));
	}

	[Fact]
	public void DensePartialSortWithComparer()
	{
		var alphabet = Enumerable.Range(0, 26)
			.Repeat(2)
			.Select((n, i) => ((char)((i % 2 == 0 ? 'A' : 'a') + n)).ToString())
			.ToArray();

		using var ns = alphabet
			.Zip(SuperEnumerable.RandomDouble(), KeyValuePair.Create)
			.AsTestingSequence();

		ns
			.DensePartialSortBy(3, e => e.Key, StringComparer.Ordinal)
			.Select(e => e.Key[0])
			.AssertSequenceEqual('A', 'A', 'C', 'C', 'E', 'E');
	}
}
