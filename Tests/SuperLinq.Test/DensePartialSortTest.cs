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
		var sorted = Enumerable.Range(1, 10)
			.Repeat(2)
			.Reverse()
			.Append(0)
			.DensePartialSort(3);

		sorted.AssertSequenceEqual(0, 1, 1, 2, 2);
	}

	[Fact]
	public void DensePartialSortWithOrder()
	{
		var sorted = Enumerable.Range(1, 10)
			.Repeat(2)
			.Reverse()
			.Append(0)
			.DensePartialSort(3, OrderByDirection.Ascending);
		sorted.AssertSequenceEqual(0, 1, 1, 2, 2);

		sorted = Enumerable.Range(1, 10)
			.Repeat(2)
			.Reverse()
			.Append(0)
			.DensePartialSort(3, OrderByDirection.Descending);
		sorted.AssertSequenceEqual(10, 10, 9, 9, 8, 8);
	}

	[Fact]
	public void DensePartialSortWithComparer()
	{
		var alphabet = Enumerable.Range(0, 26)
			.Repeat(2)
			.Select((n, i) => ((char)((i % 2 == 0 ? 'A' : 'a') + n)).ToString())
			.ToArray();

		var sorted = alphabet.DensePartialSort(3, StringComparer.Ordinal);

		sorted.Select(s => s[0]).AssertSequenceEqual('A', 'A', 'C', 'C', 'E', 'E');
	}
}
