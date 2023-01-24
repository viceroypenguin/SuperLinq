namespace Test;

public class DensePartialSortTests
{
	[Fact]
	public void DensePartialSortIsLazy()
	{
		new BreakingSequence<object>().DensePartialSort(1);
	}

	[Fact]
	public void DensePartialSort()
	{
		using var xs = Enumerable.Range(1, 10)
			.Repeat(2)
			.Reverse()
			.Append(0)
			.AsTestingSequence();

		xs
			.DensePartialSort(3)
			.AssertSequenceEqual(0, 1, 1, 2, 2);
	}

	[Theory]
	[InlineData(OrderByDirection.Ascending)]
	[InlineData(OrderByDirection.Descending)]
	public void DensePartialSortWithOrder(OrderByDirection direction)
	{
		using var xs = Enumerable.Range(1, 10)
			.Repeat(2)
			.Reverse()
			.Append(0)
			.AsTestingSequence();

		var sorted = xs.DensePartialSort(3, direction);
		if (direction == OrderByDirection.Descending)
			sorted.AssertSequenceEqual(10, 10, 9, 9, 8, 8);
		else
			sorted.AssertSequenceEqual(0, 1, 1, 2, 2);
	}

	[Fact]
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
}
