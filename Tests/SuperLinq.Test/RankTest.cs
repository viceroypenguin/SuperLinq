namespace Test;

public sealed class RankTests
{
	/// <summary>
	/// Verify that Rank uses deferred execution
	/// </summary>
	[Fact]
	public void TestRankIsLazy()
	{
		_ = new BreakingSequence<int>().Rank();
		_ = new BreakingSequence<int>().Rank(OrderByDirection.Ascending);
	}

	/// <summary>
	/// Verify that RankBy uses deferred execution
	/// </summary>
	[Fact]
	public void TestRankByIsLazy()
	{
		_ = new BreakingSequence<int>().RankBy(BreakingFunc.Of<int, int>());
		_ = new BreakingSequence<int>().RankBy(BreakingFunc.Of<int, int>(), OrderByDirection.Ascending);
	}

	public static IEnumerable<object[]> GetSimpleSequences() =>
		Enumerable.Repeat(1, 10)
			.GetTestingSequence(maxEnumerations: 2)
			.Select(x => new object[] { x, });

	/// <summary>
	/// Verify that calling Rank with null comparer results in a sequence
	/// ordered using the default comparer for the given element.
	/// </summary>
	[Theory]
	[MemberData(nameof(GetSimpleSequences))]
	public void TestRankNullComparer(IDisposableEnumerable<int> seq)
	{
		var expected = Enumerable.Repeat((1, 1), 10);
		using (seq)
		{
			seq
				.Rank()
				.AssertSequenceEqual(expected);

			seq
				.Rank(OrderByDirection.Ascending)
				.AssertSequenceEqual(expected);
		}
	}

	/// <summary>
	/// Verify that calling RankBy with null comparer results in a sequence
	/// ordered using the default comparer for the given element.
	/// </summary>
	[Theory]
	[MemberData(nameof(GetSimpleSequences))]
	public void TestRankByNullComparer(IDisposableEnumerable<int> seq)
	{
		var expected = Enumerable.Repeat((1, 1), 10);
		using (seq)
		{
			seq
				.RankBy(SuperEnumerable.Identity)
				.AssertSequenceEqual(expected);

			seq
				.RankBy(SuperEnumerable.Identity, OrderByDirection.Ascending)
				.AssertSequenceEqual(expected);
		}
	}

	public static IEnumerable<object[]> GetDescendingIntSequences() =>
		Enumerable.Range(456, 100)
			.Reverse()
			.GetTestingSequence(maxEnumerations: 2)
			.Select(x => new object[] { x, });

	/// <summary>
	/// Verify that calling Rank with null comparer on a source in reverse order
	/// results in a sequence in ascending order, using the default comparer for
	/// the given element.
	/// </summary>
	[Theory]
	[MemberData(nameof(GetDescendingIntSequences))]
	public void TestRankDescendingSequence(IDisposableEnumerable<int> seq)
	{
		var expected =
			Enumerable
				.Range(456, 100)
				.Select((x, i) => (x, i + 1));

		using (seq)
		{
			seq
				.Rank()
				.AssertSequenceEqual(expected);

			seq
				.Rank(OrderByDirection.Ascending)
				.AssertSequenceEqual(expected);
		}
	}

	public static IEnumerable<object[]> GetAscendingIntSequences() =>
		Enumerable.Range(456, 100)
			.GetTestingSequence(maxEnumerations: 2)
			.Select(x => new object[] { x, });

	/// <summary>
	/// Verify that calling Rank with null comparer on a source in ascending order
	/// results in a sequence in ascending order, using the default comparer for
	/// the given element.
	/// </summary>
	[Theory]
	[MemberData(nameof(GetAscendingIntSequences))]
	public void TestRankAscendingSequence(IDisposableEnumerable<int> seq)
	{
		var expected =
			Enumerable
				.Range(456, 100)
				.Select((x, i) => (x, i + 1));

		using (seq)
		{
			seq
				.Rank()
				.AssertSequenceEqual(expected);

			seq
				.Rank(OrderByDirection.Ascending)
				.AssertSequenceEqual(expected);
		}
	}

	/// <summary>
	/// Verify that calling Rank with null comparer on a source in ascending order
	/// results in a sequence in descending order, using OrderByDirection.Descending
	/// with the default comparer for the given element.
	/// </summary>
	[Theory]
	[MemberData(nameof(GetAscendingIntSequences))]
	public void TestRankOrderByDescending(IDisposableEnumerable<int> seq)
	{
		var expected =
			Enumerable
				.Range(456, 100)
				.Reverse()
				.Select((x, i) => (x, i + 1));

		using (seq)
		{
			seq
				.Rank(OrderByDirection.Descending)
				.AssertSequenceEqual(expected);
		}
	}

	public static IEnumerable<object[]> GetGroupedSequences() =>
		Enumerable.Range(0, 10)
			.Concat(Enumerable.Range(0, 10))
			.Concat(Enumerable.Range(0, 10))
			.GetTestingSequence(maxEnumerations: 2)
			.Select(x => new object[] { x, });

	/// <summary>
	/// Verify that the rank of equivalent items in a sequence is the same.
	/// </summary>
	[Theory]
	[MemberData(nameof(GetGroupedSequences))]
	public void TestRankGroupedItems(IDisposableEnumerable<int> seq)
	{
		var expected =
			SuperEnumerable
				.Range(1, 10, 3)
				.SelectMany((x, i) =>
					Enumerable
						.Repeat(x, 3)
						// should be 0-9, repeated three times, with ranks 1,2,...,10
						.Select(y => (item: i, index: y))
				);

		using (seq)
		{
			seq
				.Rank()
				.AssertSequenceEqual(expected);

			seq
				.Rank(OrderByDirection.Ascending)
				.AssertSequenceEqual(expected);
		}
	}

	public sealed record Person(string Name, int Age, int ExpectedRank);
	public static IEnumerable<object[]> GetPersonSequences1() =>
		new[]
		{
				new Person(Name: "Bob", Age: 24, ExpectedRank: 4),
				new Person(Name: "Sam", Age: 51, ExpectedRank: 7),
				new Person(Name: "Kim", Age: 18, ExpectedRank: 2),
				new Person(Name: "Tim", Age: 23, ExpectedRank: 3),
				new Person(Name: "Joe", Age: 31, ExpectedRank: 6),
				new Person(Name: "Mel", Age: 28, ExpectedRank: 5),
				new Person(Name: "Jim", Age: 74, ExpectedRank: 8),
				new Person(Name: "Jes", Age: 11, ExpectedRank: 1),
		}
			.GetTestingSequence(maxEnumerations: 2)
			.Select(x => new object[] { x, });

	public static IEnumerable<object[]> GetPersonSequences2() =>
		new[]
		{
				new Person(Name: "Tim", Age: 23, ExpectedRank: 4),
				new Person(Name: "Joe", Age: 23, ExpectedRank: 4),
				new Person(Name: "Jes", Age: 30, ExpectedRank: 8),
				new Person(Name: "Bob", Age: 11, ExpectedRank: 1),
				new Person(Name: "Sam", Age: 11, ExpectedRank: 1),
				new Person(Name: "Kim", Age: 11, ExpectedRank: 1),
				new Person(Name: "Mel", Age: 28, ExpectedRank: 6),
				new Person(Name: "Jim", Age: 28, ExpectedRank: 6),
		}
			.GetTestingSequence(maxEnumerations: 2)
			.Select(x => new object[] { x, });

	/// <summary>
	/// Verify that we can rank items by an arbitrary key produced from the item.
	/// </summary>
	[Theory]
	[MemberData(nameof(GetPersonSequences1))]
	[MemberData(nameof(GetPersonSequences2))]
	public void TestRankByKeySelector(IDisposableEnumerable<Person> seq)
	{
		var expectedLength = 8;

		using (seq)
		{
			var resultRankBy = seq.RankBy(x => x.Age).ToArray();
			Assert.Equal(expectedLength, resultRankBy.Length);
			Assert.True(HasExpectedRank(resultRankBy));

			var resultRankByWithSortDirection = seq.RankBy(x => x.Age, OrderByDirection.Ascending).ToArray();
			Assert.Equal(expectedLength, resultRankByWithSortDirection.Length);
			Assert.True(HasExpectedRank(resultRankByWithSortDirection));
		}

		static bool HasExpectedRank(IEnumerable<(Person item, int rank)> result)
			=> result.All(x => x.rank == x.item.ExpectedRank);
	}

	public static IEnumerable<object[]> GetDateTimeSequences() =>
		Enumerable.Range(1, 10)
			.Select(x => new DateTime(2010, x, 20 - x))
			.GetTestingSequence(maxEnumerations: 2)
			.Select(x => new object[] { x, });

	/// <summary>
	/// Verify that Rank can use a custom comparer
	/// </summary>
	[Theory]
	[MemberData(nameof(GetDateTimeSequences))]
	public void TestRankCustomComparer1(IDisposableEnumerable<DateTime> seq)
	{
		var expected =
			Enumerable
				.Range(1, 10)
				.Select(x => new DateTime(2010, x, 20 - x))
				.OrderByDescending(SuperEnumerable.Identity)
				.Select((x, i) => (x, i + 1));

		using (seq)
		{
			// invert the CompareTo operation to Rank in reverse order
			var resultRank = seq.Rank(Comparer<DateTime>.Create((a, b) => -a.CompareTo(b)));
			resultRank.AssertSequenceEqual(expected);

			// Rank called with a reverse comparer should be ordered correctly with OrderByDirection.Ascending
			var resultRankWithSortDirection =
				seq.Rank(
					Comparer<DateTime>.Create((a, b) => -a.CompareTo(b)),
					OrderByDirection.Ascending
				);
			resultRankWithSortDirection.AssertSequenceEqual(expected);
		}
	}

	/// <summary>
	/// Verify that RankBy can use a custom comparer with a key selector
	/// </summary>
	[Theory]
	[MemberData(nameof(GetDateTimeSequences))]
	public void TestRankCustomComparer2(IDisposableEnumerable<DateTime> seq)
	{
		var expected =
			Enumerable
				.Range(1, 10)
				.Select(x => new DateTime(2010, x, 20 - x))
				.OrderByDescending(x => x.Day)
				.Select((x, i) => (x, i + 1));

		using (seq)
		{
			// invert the CompareTo operation to Rank in reverse order and specify a key selector
			var resultRankBy = seq.RankBy(x => x.Day, Comparer<int>.Create((a, b) => -a.CompareTo(b)));
			resultRankBy.AssertSequenceEqual(expected);

			// Rank called with a reverse comparer and key selector should be ordered correctly with OrderByDirection.Ascending
			var resultRankByWithSortDirection =
				seq.RankBy(x => x.Day, Comparer<int>.Create((a, b) => -a.CompareTo(b)),
				OrderByDirection.Ascending
			);
			resultRankByWithSortDirection.AssertSequenceEqual(expected);
		}
	}
}
