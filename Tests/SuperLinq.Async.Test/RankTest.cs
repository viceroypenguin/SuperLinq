namespace Test.Async;

public class RankTests
{
	[Fact]
	public void TestRankIsLazy()
	{
		new AsyncBreakingSequence<int>().Rank();
	}

	[Fact]
	public void TestRankByIsLazy()
	{
		new AsyncBreakingSequence<int>().RankBy(BreakingFunc.Of<int, int>());
	}

	[Fact]
	public async Task TestRankNullComparer()
	{
		await using var sequence = Enumerable.Repeat(1, 10)
			.AsTestingSequence();
		await sequence.Rank().AssertSequenceEqual(
			Enumerable.Repeat((1, 1), 10));
	}

	[Fact]
	public async Task TestRankByNullComparer()
	{
		await using var sequence = Enumerable.Repeat(1, 10)
			.AsTestingSequence();
		await sequence.RankBy(x => x).AssertSequenceEqual(
			Enumerable.Repeat((1, 1), 10));
	}

	[Fact]
	public async Task TestRankDescendingSequence()
	{
		await using var sequence = Enumerable.Range(456, 100).Reverse()
			.AsTestingSequence();
		var expectedResult = Enumerable.Range(456, 100)
			.Select((x, i) => (x, i + 1));

		var result = await sequence.Rank().ToArrayAsync();
		Assert.Equal(100, result.Length);
		result.AssertSequenceEqual(expectedResult);
	}

	[Fact]
	public async Task TestRankByAscendingSeries()
	{
		await using var sequence = Enumerable.Range(456, 100).AsTestingSequence();
		var expectedResult = Enumerable.Range(456, 100)
			.Select((x, i) => (x, i + 1));

		var result = await sequence.Rank().ToArrayAsync();
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

		var result = await sequence.Rank().ToListAsync();
		Assert.Equal(10, result.Distinct().Count());
		result.AssertSequenceEqual(
			SuperLinq.SuperEnumerable.Range(1, 10, 3)
				.SelectMany((x, i) => Enumerable.Repeat(x, 3)
					// should be 0-9, repeated three times, with ranks 1,4,...,28
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

		var result = sequence.Rank();
		Assert.Equal(1, (await result.OrderBy(x => x).FirstAsync()).rank);
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
		var result = await xs.RankBy(x => x.Age).ToArrayAsync();

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

		// invert the CompareTo operation to Rank in reverse order
		await using (var xs = sequence.AsTestingSequence())
		{
			var resultA = xs.Rank(Comparer<DateTime>.Create((a, b) => -a.CompareTo(b)));
			await resultA.AssertSequenceEqual(sequence
				.OrderByDescending(x => x)
				.Select((x, i) => (x, i + 1)));
		}

		await using (var xs = sequence.AsTestingSequence())
		{
			var resultB = xs.RankBy(x => x.Day, Comparer<int>.Create((a, b) => -a.CompareTo(b)));
			await resultB.AssertSequenceEqual(sequence
				.OrderByDescending(x => x.Day)
				.Select((x, i) => (x, i + 1)));
		}
	}
}
