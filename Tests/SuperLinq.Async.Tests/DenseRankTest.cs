namespace SuperLinq.Async.Tests;

public sealed class DenseRankTests
{
	/// <summary>
	/// Verify that DenseRank uses deferred execution
	/// </summary>
	[Test]
	public void TestDenseRankIsLazy()
	{
		_ = new AsyncBreakingSequence<int>().DenseRank();
		_ = new AsyncBreakingSequence<int>().DenseRank(OrderByDirection.Ascending);
	}

	/// <summary>
	/// Verify that DenseRankBy uses deferred execution
	/// </summary>
	[Test]
	public void TestDenseRankByIsLazy()
	{
		_ = new AsyncBreakingSequence<int>().DenseRankBy(BreakingFunc.Of<int, int>());
		_ = new AsyncBreakingSequence<int>().DenseRankBy(BreakingFunc.Of<int, int>(), OrderByDirection.Ascending);
	}

	/// <summary>
	/// Verify that calling DenseRank with null comparer results in a sequence
	/// ordered using the default comparer for the given element.
	/// </summary>
	[Test]
	public async Task TestRankNullComparer()
	{
		await using var sequence =
			AsyncEnumerable
				.Repeat(1, 10)
				.AsTestingSequence(maxEnumerations: 2);

		var expected = Enumerable.Repeat((1, 1), 10);

		await sequence
			.DenseRank()
			.AssertSequenceEqual(expected);

		await sequence
			.DenseRank(OrderByDirection.Ascending)
			.AssertSequenceEqual(expected);
	}

	/// <summary>
	/// Verify that calling DenseRankBy with null comparer results in a sequence
	/// ordered using the default comparer for the given element.
	/// </summary>
	[Test]
	public async Task TestRankByNullComparer()
	{
		await using var sequence =
			AsyncEnumerable
				.Repeat(1, 10)
				.AsTestingSequence(maxEnumerations: 2);

		var expected = Enumerable.Repeat((1, 1), 10);

		await sequence
			.DenseRankBy(SuperEnumerable.Identity)
			.AssertSequenceEqual(expected);

		await sequence
			.DenseRankBy(SuperEnumerable.Identity, OrderByDirection.Ascending)
			.AssertSequenceEqual(expected);
	}

	/// <summary>
	/// Verify that calling DenseRank with null comparer on a source in reverse order
	/// results in a sequence in ascending order, using the default comparer for
	/// the given element.
	/// </summary>
	[Test]
	public async Task TestRankDescendingSequence()
	{
		await using var sequence =
			Enumerable
				.Range(456, 100)
				.Reverse()
				.AsTestingSequence(maxEnumerations: 2);

		var expectedLength = 100;
		var expectedSequence =
			Enumerable
				.Range(456, 100)
				.Select((x, i) => (x, i + 1));

		var resultDenseRank = await sequence.DenseRank().ToArrayAsync();
		Assert.Equal(expectedLength, resultDenseRank.Length);
		resultDenseRank.AssertSequenceEqual(expectedSequence);

		var resultDenseRankWithSortDirection = await sequence.DenseRank(OrderByDirection.Ascending).ToArrayAsync();
		Assert.Equal(expectedLength, resultDenseRankWithSortDirection.Length);
		resultDenseRankWithSortDirection.AssertSequenceEqual(expectedSequence);
	}

	/// <summary>
	/// Verify that calling DenseRank with null comparer on a source in ascending order
	/// results in a sequence in ascending order, using the default comparer for
	/// the given element.
	/// </summary>
	[Test]
	public async Task TestRankByAscendingSeries()
	{
		await using var sequence =
			Enumerable
				.Range(456, 100)
				.AsTestingSequence(maxEnumerations: 2);

		var expectedLength = 100;
		var expectedSequence =
			Enumerable
				.Range(456, 100)
				.Select((x, i) => (x, i + 1));

		var resultDenseRank = await sequence.DenseRank().ToArrayAsync();
		Assert.Equal(expectedLength, resultDenseRank.Length);
		resultDenseRank.AssertSequenceEqual(expectedSequence);

		var resultDenseRankWithSortDirection = await sequence.DenseRank().ToArrayAsync();
		Assert.Equal(expectedLength, resultDenseRankWithSortDirection.Length);
		resultDenseRankWithSortDirection.AssertSequenceEqual(expectedSequence);
	}

	/// <summary>
	/// Verify that calling DenseRank with null comparer on a source in ascending order
	/// results in a sequence in descending order, using OrderByDirection.Descending
	/// with the default comparer for the given element.
	/// </summary>
	[Test]
	public async Task TestRankOrderByDescending()
	{
		await using var sequence =
			Enumerable
				.Range(456, 100)
				.AsTestingSequence();

		var expected =
			Enumerable
				.Range(456, 100)
				.Reverse()
				.Select((x, i) => (x, i + 1));

		await sequence
			.DenseRank(OrderByDirection.Descending)
			.AssertSequenceEqual(expected);
	}

	/// <summary>
	/// Verify that the rank of equivalent items in a sequence is the same.
	/// </summary>
	[Test]
	public async Task TestRankGroupedItems()
	{
		await using var sequence =
			Enumerable
				.Range(0, 10)
				.Concat(Enumerable.Range(0, 10))
				.Concat(Enumerable.Range(0, 10))
				.AsTestingSequence(maxEnumerations: 2);

		var expectedLength = 10;
		var expectedSequence =
			SuperEnumerable
				.Range(1, 10, 1)
				.SelectMany((x, i) =>
					Enumerable
						.Repeat(x, 3)
						// should be 0-9, repeated three times, with ranks 1,2,...,10
						.Select(y => (item: i, index: y))
				);

		var resultDenseRank = await sequence.DenseRank().ToListAsync();
		Assert.Equal(expectedLength, resultDenseRank.Distinct().Count());
		resultDenseRank.AssertSequenceEqual(expectedSequence);

		var resultDenseRankWithSortDirection = await sequence.DenseRank(OrderByDirection.Ascending).ToListAsync();
		Assert.Equal(expectedLength, resultDenseRankWithSortDirection.Distinct().Count());
		resultDenseRankWithSortDirection.AssertSequenceEqual(expectedSequence);
	}

	/// <summary>
	/// Verify that the highest rank (that of the largest item) is 1 (not 0).
	/// </summary>
	[Test]
	public async Task TestRankOfHighestItemIsOne()
	{
		await using var sequence =
			Enumerable
				.Range(1, 10)
				.AsTestingSequence(maxEnumerations: 2);

		var expected = 1;

		var resultDenseRank = sequence.DenseRank();
		Assert.Equal(expected, (await resultDenseRank.OrderBy(SuperEnumerable.Identity).FirstAsync()).rank);

		var resultDenseRankWithSortDirection = sequence.DenseRank(OrderByDirection.Ascending);
		Assert.Equal(expected, (await resultDenseRankWithSortDirection.OrderBy(SuperEnumerable.Identity).FirstAsync()).rank);
	}

	public record Person(string Name, int Age, int ExpectedRank);

	/// <summary>
	/// Verify that we can rank items by an arbitrary key produced from the item.
	/// </summary>
	[Test]
	public async Task TestRankByKeySelector()
	{
		var sequences = new List<Person[]>
		{
			new Person[]
			{
				new(Name: "Bob", Age: 24, ExpectedRank: 4),
				new(Name: "Sam", Age: 51, ExpectedRank: 7),
				new(Name: "Kim", Age: 18, ExpectedRank: 2),
				new(Name: "Tim", Age: 23, ExpectedRank: 3),
				new(Name: "Joe", Age: 31, ExpectedRank: 6),
				new(Name: "Mel", Age: 28, ExpectedRank: 5),
				new(Name: "Jim", Age: 74, ExpectedRank: 8),
				new(Name: "Jes", Age: 11, ExpectedRank: 1),
			},
			new Person[]
			{
				new(Name: "Jes", Age: 30, ExpectedRank: 4),
				new(Name: "Tim", Age: 23, ExpectedRank: 2),
				new(Name: "Joe", Age: 23, ExpectedRank: 2),
				new(Name: "Bob", Age: 11, ExpectedRank: 1),
				new(Name: "Sam", Age: 11, ExpectedRank: 1),
				new(Name: "Kim", Age: 11, ExpectedRank: 1),
				new(Name: "Mel", Age: 28, ExpectedRank: 3),
				new(Name: "Jim", Age: 28, ExpectedRank: 3),
			},
		};

		foreach (var seq in sequences)
		{
			await using var xs = seq.AsTestingSequence(maxEnumerations: 2);

			var expected = seq
				.OrderBy(x => x.ExpectedRank)
				.Select(x => (x, x.ExpectedRank));

			var resultDenseRankBy = await xs.DenseRankBy(x => x.Age).ToArrayAsync();
			Assert.Equal(seq.Length, resultDenseRankBy.Length);
			resultDenseRankBy.AssertSequenceEqual(expected);

			var resultDenseRankByWithSortDirection = await xs.DenseRankBy(x => x.Age, OrderByDirection.Ascending).ToArrayAsync();
			Assert.Equal(seq.Length, resultDenseRankByWithSortDirection.Length);
			resultDenseRankByWithSortDirection.AssertSequenceEqual(expected);
		}
	}

	/// <summary>
	/// Verify that Rank can use a custom comparer
	/// </summary>
	[Test]
	public async Task TestRankCustomComparer()
	{
		var ordinals = Enumerable.Range(1, 10);
		var sequence = ordinals.Select(x => new DateTime(2010, x, 20 - x));

		var expectedWithIdentityFunction =
			sequence
				.OrderByDescending(SuperEnumerable.Identity)
				.Select((x, i) => (x, i + 1));

		var expectedWithKeySelector =
			sequence
				.OrderByDescending(x => x.Day)
				.Select((x, i) => (x, i + 1));

		// invert the CompareTo operation to Rank in reverse order
		await using (var xs = sequence.AsTestingSequence(maxEnumerations: 2))
		{
			// with identity function
			var resultA = xs.DenseRank(Comparer<DateTime>.Create((a, b) => -a.CompareTo(b)));
			await resultA.AssertSequenceEqual(expectedWithIdentityFunction);

			// with key selector
			var resultB = xs.DenseRankBy(x => x.Day, Comparer<int>.Create((a, b) => -a.CompareTo(b)));
			await resultB.AssertSequenceEqual(expectedWithKeySelector);
		}

		// Rank called with a reverse comparer should be ordered correctly with OrderByDirection.Ascending
		await using (var xs = sequence.AsTestingSequence(maxEnumerations: 2))
		{
			// with identity function
			var resultA = xs.DenseRank(
				Comparer<DateTime>.Create((a, b) => -a.CompareTo(b)),
				OrderByDirection.Ascending
			);
			await resultA.AssertSequenceEqual(expectedWithIdentityFunction);

			// with key selector
			var resultB =
				xs.DenseRankBy(
					x => x.Day, Comparer<int>.Create((a, b) => -a.CompareTo(b)),
					OrderByDirection.Ascending
				);
			await resultB.AssertSequenceEqual(expectedWithKeySelector);
		}
	}
}
