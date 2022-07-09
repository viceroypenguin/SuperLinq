namespace Test;

public class DenseRankTests
{
	[Fact]
	public void TestDenseRankIsLazy()
	{
		new BreakingSequence<int>().DenseRank();
	}

	[Fact]
	public void TestenseRankByIsLazy()
	{
		new BreakingSequence<int>().DenseRankBy(BreakingFunc.Of<int, int>());
	}

	[Fact]
	public void TestRankNullComparer()
	{
		var sequence = Enumerable.Repeat(1, 10);
		sequence.AsTestingSequence().DenseRank().AssertSequenceEqual(
			Enumerable.Repeat((1, 1), 10));
	}

	[Fact]
	public void TestRankByNullComparer()
	{
		var sequence = Enumerable.Repeat(1, 10);
		sequence.AsTestingSequence().DenseRankBy(x => x).AssertSequenceEqual(
			Enumerable.Repeat((1, 1), 10));
	}

	[Fact]
	public void TestRankDescendingSequence()
	{
		var sequence = Enumerable.Range(456, 100).Reverse();
		var expectedResult = Enumerable.Range(456, 100)
			.Select((x, i) => (x, i + 1));

		var result = sequence.AsTestingSequence().DenseRank().ToArray();
		Assert.Equal(100, result.Length);
		result.AssertSequenceEqual(expectedResult);
	}

	[Fact]
	public void TestRankByAscendingSeries()
	{
		var sequence = Enumerable.Range(456, 100);
		var expectedResult = Enumerable.Range(456, 100)
			.Select((x, i) => (x, i + 1));

		var result = sequence.AsTestingSequence().DenseRank().ToArray();
		Assert.Equal(100, result.Length);
		result.AssertSequenceEqual(expectedResult);
	}

	/// <summary>
	/// Verify that the rank of equivalent items in a sequence is the same.
	/// </summary>
	[Fact]
	public void TestRankGroupedItems()
	{
		var sequence = Enumerable.Range(0, 10)
			.Concat(Enumerable.Range(0, 10))
			.Concat(Enumerable.Range(0, 10));

		var result = sequence.AsTestingSequence().DenseRank().ToList();
		Assert.Equal(10, result.Distinct().Count());
		result.AssertSequenceEqual(
			SuperEnumerable.Range(1, 10, 1)
				.SelectMany((x, i) => Enumerable.Repeat(x, 3)
					// should be 0-9, repeated three times, with ranks 1,2,...,10
					.Select(y => (item: i, index: y))));
	}

	/// <summary>
	/// Verify that the highest rank (that of the largest item) is 1 (not 0).
	/// </summary>
	[Fact]
	public void TestRankOfHighestItemIsOne()
	{
		var sequence = Enumerable.Range(1, 10);

		var result = sequence.AsTestingSequence().DenseRank();
		Assert.Equal(1, result.OrderBy(x => x).First().rank);
	}

	/// <summary>
	/// Verify that we can rank items by an arbitrary key produced from the item.
	/// </summary>
	[Fact]
	public void TestRankByKeySelector()
	{
		var sequence = new[]
		{
			new { Name = "Bob", Age = 24, ExpectedRank = 4 },
			new { Name = "Sam", Age = 51, ExpectedRank = 7 },
			new { Name = "Kim", Age = 18, ExpectedRank = 2 },
			new { Name = "Tim", Age = 23, ExpectedRank = 3 },
			new { Name = "Joe", Age = 31, ExpectedRank = 6 },
			new { Name = "Mel", Age = 28, ExpectedRank = 5 },
			new { Name = "Jim", Age = 74, ExpectedRank = 8 },
			new { Name = "Jes", Age = 11, ExpectedRank = 1 },
		};

		var result = sequence.AsTestingSequence().DenseRankBy(x => x.Age).ToArray();
		Assert.Equal(sequence.Length, result.Length);
		result.AssertSequenceEqual(sequence
			.OrderBy(x => x.ExpectedRank)
			.Select(x => (x, x.ExpectedRank)));
	}

	/// <summary>
	/// Verify that Rank can use a custom comparer
	/// </summary>
	[Fact]
	public void TestRankCustomComparer()
	{
		var ordinals = Enumerable.Range(1, 10);
		var sequence = ordinals.Select(x => new DateTime(2010, x, 20 - x));

		// invert the CompareTo operation to Rank in reverse order
		var resultA = sequence.AsTestingSequence().DenseRank(Comparer<DateTime>.Create((a, b) => -a.CompareTo(b)));
		resultA.AssertSequenceEqual(sequence
			.OrderByDescending(x => x)
			.Select((x, i) => (x, i + 1)));
		var resultB = sequence.AsTestingSequence().DenseRankBy(x => x.Day, Comparer<int>.Create((a, b) => -a.CompareTo(b)));
		resultB.AssertSequenceEqual(sequence
			.OrderByDescending(x => x.Day)
			.Select((x, i) => (x, i + 1)));
	}
}
