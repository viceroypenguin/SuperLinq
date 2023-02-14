namespace Test.Async;

public class DenseRankTests
{
	[Fact]
	public void TestDenseRankIsLazy()
	{
		_ = new AsyncBreakingSequence<int>().DenseRank();
	}

	[Fact]
	public void TestDenseRankByIsLazy()
	{
		_ = new AsyncBreakingSequence<int>().DenseRankBy(BreakingFunc.Of<int, int>());
	}

	[Fact]
	public async Task TestRankNullComparer()
	{
		await using var sequence = AsyncEnumerable.Repeat(1, 10)
			.AsTestingSequence();
		await sequence
			.DenseRank()
			.AssertSequenceEqual(Enumerable.Repeat((1, 1), 10));
	}

	[Fact]
	public async Task TestRankByNullComparer()
	{
		await using var sequence = AsyncEnumerable.Repeat(1, 10)
			.AsTestingSequence();
		await sequence
			.DenseRankBy(SuperLinq.SuperEnumerable.Identity)
			.AssertSequenceEqual(Enumerable.Repeat((1, 1), 10));
	}

	[Fact]
	public async Task TestRankDescendingSequence()
	{
		await using var sequence = Enumerable.Range(456, 100).Reverse()
			.AsTestingSequence();
		var expectedResult = Enumerable.Range(456, 100)
			.Select((x, i) => (x, i + 1));

		var result = await sequence.DenseRank().ToArrayAsync();
		Assert.Equal(100, result.Length);
		result.AssertSequenceEqual(expectedResult);
	}

	[Fact]
	public async Task TestRankByAscendingSeries()
	{
		await using var sequence = Enumerable.Range(456, 100)
			.AsTestingSequence();
		var expectedResult = Enumerable.Range(456, 100)
			.Select((x, i) => (x, i + 1));

		var result = await sequence.DenseRank().ToArrayAsync();
		Assert.Equal(100, result.Length);
		result.AssertSequenceEqual(expectedResult);
	}

	/// <summary>
	/// Verify that the rank of equivalent items in a sequence is the same.
	/// </summary>
	[Fact]
	public async Task TestRankGroupedItems()
	{
		await using var sequence = Enumerable.Range(0, 10)
			.Concat(Enumerable.Range(0, 10))
			.Concat(Enumerable.Range(0, 10))
			.AsTestingSequence();

		var result = await sequence.DenseRank().ToListAsync();
		Assert.Equal(10, result.Distinct().Count());
		result.AssertSequenceEqual(
			SuperLinq.SuperEnumerable.Range(1, 10, 1)
				.SelectMany((x, i) => Enumerable.Repeat(x, 3)
					// should be 0-9, repeated three times, with ranks 1,2,...,10
					.Select(y => (item: i, index: y))));
	}

	/// <summary>
	/// Verify that the highest rank (that of the largest item) is 1 (not 0).
	/// </summary>
	[Fact]
	public async Task TestRankOfHighestItemIsOne()
	{
		await using var sequence = Enumerable.Range(1, 10)
			.AsTestingSequence();

		var result = sequence.DenseRank();
		Assert.Equal(1,
			(await result
				.OrderBy(SuperLinq.SuperEnumerable.Identity)
				.FirstAsync()).rank);
	}

	/// <summary>
	/// Verify that we can rank items by an arbitrary key produced from the item.
	/// </summary>
	[Fact]
	public async Task TestRankByKeySelector()
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

		await using var xs = sequence.AsTestingSequence();
		var result = await xs.DenseRankBy(x => x.Age).ToArrayAsync();

		Assert.Equal(sequence.Length, result.Length);
		result.AssertSequenceEqual(sequence
			.OrderBy(x => x.ExpectedRank)
			.Select(x => (x, x.ExpectedRank)));
	}

	/// <summary>
	/// Verify that Rank can use a custom comparer
	/// </summary>
	[Fact]
	public async Task TestRankCustomComparer()
	{
		var ordinals = Enumerable.Range(1, 10);
		var sequence = ordinals.Select(x => new DateTime(2010, x, 20 - x));

		await using var xs = sequence.AsTestingSequence(maxEnumerations: 2);

		// invert the CompareTo operation to Rank in reverse order
		var resultA = xs.DenseRank(Comparer<DateTime>.Create((a, b) => -a.CompareTo(b)));
		await resultA.AssertSequenceEqual(sequence
			.OrderByDescending(SuperLinq.SuperEnumerable.Identity)
			.Select((x, i) => (x, i + 1)));

		var resultB = xs.DenseRankBy(x => x.Day, Comparer<int>.Create((a, b) => -a.CompareTo(b)));
		await resultB.AssertSequenceEqual(sequence
			.OrderByDescending(x => x.Day)
			.Select((x, i) => (x, i + 1)));
	}
}
