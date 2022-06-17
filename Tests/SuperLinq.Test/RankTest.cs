namespace Test;

/// <summary>
/// Verify the behavior of the Rank operator
/// </summary>
public class RankTests
{
	/// <summary>
	/// Verify that rank behaves in a lazy manner.
	/// </summary>
	[Fact]
	public void TestRankIsLazy()
	{
		new BreakingSequence<int>().Rank();
	}

	/// <summary>
	/// Verify that rank behaves in a lazy manner.
	/// </summary>
	[Fact]
	public void TestRankByIsLazy()
	{
		new BreakingSequence<int>().RankBy(BreakingFunc.Of<int, int>());
	}

	/// <summary>
	/// Verify that Rank uses the default comparer when comparer is <c>null</c>
	/// </summary>
	[Fact]
	public void TestRankNullComparer()
	{
		var sequence = Enumerable.Repeat(1, 10);
		sequence.AsTestingSequence().Rank(null).AssertSequenceEqual(sequence);
	}

	/// <summary>
	/// Verify that Rank uses the default comparer when comparer is <c>null</c>
	/// </summary>
	[Fact]
	public void TestRankByNullComparer()
	{
		var sequence = Enumerable.Repeat(1, 10);
		sequence.AsTestingSequence().RankBy(x => x, null).AssertSequenceEqual(sequence);
	}

	/// <summary>
	/// Verify that ranking a descending series of integers produces
	/// a linear, progressive rank for each value.
	/// </summary>
	[Fact]
	public void TestRankDescendingSequence()
	{
		var sequence = Enumerable.Range(456, 100).Reverse();
		var result = sequence.AsTestingSequence().Rank().ToArray();
		var expectedResult = Enumerable.Range(1, 100);

		Assert.Equal(100, result.Length);
		Assert.Equal(expectedResult, result);
	}

	/// <summary>
	/// Verify that ranking an ascending series of integers produces
	/// a linear, regressive rank for each value.
	/// </summary>
	[Fact]
	public void TestRankByAscendingSeries()
	{
		var sequence = Enumerable.Range(456, 100);
		var result = sequence.AsTestingSequence().Rank().ToArray();
		var expectedResult = Enumerable.Range(1, 100).Reverse();

		Assert.Equal(100, result.Length);
		Assert.Equal(expectedResult, result);
	}

	/// <summary>
	/// Verify that the rank of a sequence of the same item is always 1.
	/// </summary>
	[Fact]
	public void TestRankEquivalentItems()
	{
		var sequence = Enumerable.Repeat(1234, 100);
		var result = sequence.AsTestingSequence().Rank().ToArray();

		Assert.Equal(100, result.Length);
		Assert.Equal(Enumerable.Repeat(1, 100), result);
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
		var result = sequence.AsTestingSequence().Rank();

		Assert.Equal(10, result.Distinct().Count());
		Assert.Equal(sequence.Reverse().Select(x => x + 1), result);
	}

	/// <summary>
	/// Verify that the highest rank (that of the largest item) is 1 (not 0).
	/// </summary>
	[Fact]
	public void TestRankOfHighestItemIsOne()
	{
		var sequence = Enumerable.Range(1, 10);
		var result = sequence.AsTestingSequence().Rank();

		Assert.Equal(1, result.OrderBy(x => x).First());
	}

	/// <summary>
	/// Verify that we can rank items by an arbitrary key produced from the item.
	/// </summary>
	[Fact]
	public void TestRankByKeySelector()
	{
		var sequence = new[]
		{
			new { Name = "Bob", Age = 24, ExpectedRank = 5 },
			new { Name = "Sam", Age = 51, ExpectedRank = 2 },
			new { Name = "Kim", Age = 18, ExpectedRank = 7 },
			new { Name = "Tim", Age = 23, ExpectedRank = 6 },
			new { Name = "Joe", Age = 31, ExpectedRank = 3 },
			new { Name = "Mel", Age = 28, ExpectedRank = 4 },
			new { Name = "Jim", Age = 74, ExpectedRank = 1 },
			new { Name = "Jes", Age = 11, ExpectedRank = 8 },
		};
		var result = sequence.AsTestingSequence().RankBy(x => x.Age).ToArray();

		Assert.Equal(sequence.Length, result.Length);
		Assert.Equal(sequence.Select(x => x.ExpectedRank), result);
	}

	/// <summary>
	/// Verify that Rank can use a custom comparer
	/// </summary>
	[Fact]
	public void TestRankCustomComparer()
	{
		var ordinals = Enumerable.Range(1, 10);
		var sequence = ordinals.Select(x => new DateTime(2010, x, 20 - x));
		// invert the CompareTo operation to Rank in reverse order (ascening to descending)
		var resultA = sequence.AsTestingSequence().Rank(Comparer.Create<DateTime>((a, b) => -a.CompareTo(b)));
		var resultB = sequence.AsTestingSequence().RankBy(x => x.Day, Comparer.Create<int>((a, b) => -a.CompareTo(b)));

		Assert.Equal(ordinals, resultA);
		Assert.Equal(ordinals.Reverse(), resultB);
	}
}
