﻿namespace Test;

public class DenseRankTests
{
	[Fact]
	public void TestDenseRankIsLazy()
	{
		_ = new BreakingSequence<int>().DenseRank();
	}

	[Fact]
	public void TestDenseRankByIsLazy()
	{
		_ = new BreakingSequence<int>().DenseRankBy(BreakingFunc.Of<int, int>());
	}

	[Fact]
	public void TestRankNullComparer()
	{
		using var sequence = Enumerable.Repeat(1, 10)
			.AsTestingSequence();
		sequence
			.DenseRank()
			.AssertSequenceEqual(Enumerable.Repeat((1, 1), 10));
	}

	[Fact]
	public void TestRankByNullComparer()
	{
		using var sequence = Enumerable.Repeat(1, 10)
			.AsTestingSequence();
		sequence
			.DenseRankBy(SuperEnumerable.Identity)
			.AssertSequenceEqual(
				Enumerable.Repeat((1, 1), 10));
	}

	[Fact]
	public void TestRankDescendingSequence()
	{
		using var sequence = Enumerable.Range(456, 100)
			.Reverse()
			.AsTestingSequence();
		var expectedResult = Enumerable.Range(456, 100)
			.Select((x, i) => (x, i + 1));

		var result = sequence.DenseRank().ToArray();
		Assert.Equal(100, result.Length);
		result.AssertSequenceEqual(expectedResult);
	}

	[Fact]
	public void TestRankByAscendingSeries()
	{
		using var sequence = Enumerable.Range(456, 100)
			.AsTestingSequence();
		var expectedResult = Enumerable.Range(456, 100)
			.Select((x, i) => (x, i + 1));

		var result = sequence.DenseRank().ToArray();
		Assert.Equal(100, result.Length);
		result.AssertSequenceEqual(expectedResult);
	}

	/// <summary>
	/// Verify that the rank of equivalent items in a sequence is the same.
	/// </summary>
	[Fact]
	public void TestRankGroupedItems()
	{
		using var sequence = Enumerable.Range(0, 10)
			.Concat(Enumerable.Range(0, 10))
			.Concat(Enumerable.Range(0, 10))
			.AsTestingSequence();

		var result = sequence.DenseRank().ToList();
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
		using var sequence = Enumerable.Range(1, 10)
			.AsTestingSequence();

		var result = sequence.DenseRank();
		Assert.Equal(1, result.OrderBy(SuperEnumerable.Identity).First().rank);
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

		using var xs = sequence.AsTestingSequence();
		var result = xs.DenseRankBy(x => x.Age).ToArray();

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

		using var xs = sequence.AsTestingSequence(maxEnumerations: 2);

		// invert the CompareTo operation to Rank in reverse order
		var resultA = xs.DenseRank(Comparer<DateTime>.Create((a, b) => -a.CompareTo(b)));
		resultA.AssertSequenceEqual(sequence
			.OrderByDescending(SuperEnumerable.Identity)
			.Select((x, i) => (x, i + 1)));

		var resultB = xs.DenseRankBy(x => x.Day, Comparer<int>.Create((a, b) => -a.CompareTo(b)));
		resultB.AssertSequenceEqual(sequence
			.OrderByDescending(x => x.Day)
			.Select((x, i) => (x, i + 1)));
	}

	[Fact]
	public void TestRankCollectionCount()
	{
		using var sequence = Enumerable.Range(1, 10_000)
			.AsBreakingCollection();

		var result = sequence.DenseRank();
		Assert.Equal(10_000, result.Count());

		result = sequence.DenseRank(comparer: Comparer<int>.Default);
		Assert.Equal(10_000, result.Count());
	}
}
