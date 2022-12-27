using SuperLinq;

namespace Test.Async;

public class DensePartialSortByTests
{
	[Fact]
	public void DensePartialSortByIsLazy()
	{
		new AsyncBreakingSequence<object>().DensePartialSortBy(1, BreakingFunc.Of<object, object>());
	}

	[Fact]
	public async Task DensePartialSortBy()
	{
		var ns = SuperEnumerable.RandomDouble().Take(10).ToArray();

		await using var xs = ns.Select((n, i) => KeyValuePair.Create(i, n))
			.Repeat(2)
			.Reverse()
			.ToAsyncEnumerable()
			.AsTestingSequence();
		var sorted = xs
			.DensePartialSortBy(3, e => e.Key);

		await sorted.Select(e => e.Value).AssertSequenceEqual(
			ns.Take(3).SelectMany(x => new[] { x, x, }));
	}

	[Theory]
	[InlineData(OrderByDirection.Ascending)]
	[InlineData(OrderByDirection.Descending)]
	public async Task DensePartialSortWithOrder(OrderByDirection direction)
	{
		var ns = SuperEnumerable.RandomDouble()
			.Take(10)
			.ToArray()
			.AsEnumerable();

		await using var xs = ns.Select((n, i) => KeyValuePair.Create(i, n))
			.Repeat(2)
			.Reverse()
			.ToAsyncEnumerable()
			.AsTestingSequence();
		var sorted = xs
			.DensePartialSortBy(3, e => e.Key, direction);

		if (direction == OrderByDirection.Descending)
			ns = ns.Reverse();

		await sorted.Select(e => e.Value).AssertSequenceEqual(
			ns.Take(3).SelectMany(x => new[] { x, x, }));
	}

	[Fact]
	public async Task DensePartialSortWithComparer()
	{
		var alphabet = Enumerable.Range(0, 26)
			.Repeat(2)
			.Select((n, i) => ((char)((i % 2 == 0 ? 'A' : 'a') + n)).ToString())
			.ToArray();

		var ns = alphabet.Zip(SuperEnumerable.RandomDouble(), KeyValuePair.Create).ToArray();
		await using var xs = ns.ToAsyncEnumerable().AsTestingSequence();

		var sorted = xs.DensePartialSortBy(3, e => e.Key, StringComparer.Ordinal);

		await sorted.Select(e => e.Key[0])
			.AssertSequenceEqual('A', 'A', 'C', 'C', 'E', 'E');
	}
}
