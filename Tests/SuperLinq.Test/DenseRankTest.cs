namespace Test;

public class DenseRankTests
{
	/// <summary>
	/// Verify that DenseRank uses deferred execution with lazy evaluation.
	/// </summary>
	[Fact]
	public void TestDenseRankIsLazy()
	{
		_ = new BreakingSequence<int>().DenseRank();
		_ = new BreakingSequence<int>().DenseRank(OrderByDirection.Ascending);
	}

	/// <summary>
	/// Verify that DenseRankBy uses deferred execution with lazy evaluation.
	/// </summary>
	[Fact]
	public void TestDenseRankByIsLazy()
	{
		_ = new BreakingSequence<int>().DenseRankBy(BreakingFunc.Of<int, int>());
		_ = new BreakingSequence<int>().DenseRankBy(BreakingFunc.Of<int, int>(), OrderByDirection.Ascending);
	}

	public static IEnumerable<object[]> GetSimpleSequences() =>
		Enumerable.Repeat(1, 10)
			.GetTestingSequence(maxEnumerations: 3)
			.Select(x => new object[] { x, });

	/// <summary>
	/// Verify that calling DenseRank with null comparer results in a sequence
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
				.DenseRank()
				.AssertSequenceEqual(expected);

			seq
				.DenseRank(OrderByDirection.Ascending)
				.AssertSequenceEqual(expected);
		}
	}

	/// <summary>
	/// Verify that calling DenseRankBy with null comparer results in a sequence
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
				.DenseRankBy(SuperEnumerable.Identity)
				.AssertSequenceEqual(expected);

			seq
				.DenseRankBy(SuperEnumerable.Identity, OrderByDirection.Ascending)
				.AssertSequenceEqual(expected);
		}
	}

	public static IEnumerable<object[]> GetDescendingIntSequences() =>
		Enumerable.Range(456, 100)
			.Reverse()
			.GetTestingSequence(maxEnumerations: 3)
			.Select(x => new object[] { x, });

	/// <summary>
	/// Verify that calling DenseRank with null comparer on a source in reverse order
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
				.DenseRank()
				.AssertSequenceEqual(expected);

			seq
				.DenseRank(OrderByDirection.Ascending)
				.AssertSequenceEqual(expected);
		}
	}

	public static IEnumerable<object[]> GetAscendingIntSequences() =>
		Enumerable.Range(456, 100)
			.GetTestingSequence(maxEnumerations: 3)
			.Select(x => new object[] { x, });

	/// <summary>
	/// Verify that calling DenseRank with null comparer on a source in ascending order
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
				.DenseRank()
				.AssertSequenceEqual(expected);

			seq
				.DenseRank(OrderByDirection.Ascending)
				.AssertSequenceEqual(expected);
		}
	}

	/// <summary>
	/// Verify that calling DenseRank with null comparer on a source in ascending order
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
				.DenseRank(OrderByDirection.Descending)
				.AssertSequenceEqual(expected);
		}
	}

	public static IEnumerable<object[]> GetGroupedSequences() =>
		Enumerable.Range(0, 10)
			.Concat(Enumerable.Range(0, 10))
			.Concat(Enumerable.Range(0, 10))
			.GetTestingSequence()
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
				.Range(1, 10, 1)
				.SelectMany((x, i) =>
					Enumerable
						.Repeat(x, 3)
						// should be 0-9, repeated three times, with ranks 1,2,...,10
						.Select(y => (item: i, index: y))
				);

		using (seq)
		{
			seq
				.DenseRank()
				.AssertSequenceEqual(expected);

			seq
				.DenseRank(OrderByDirection.Ascending)
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
			.GetTestingSequence()
			.Select(x => new object[] { x, });

	public static IEnumerable<object[]> GetPersonSequences2() =>
		new[]
		{
				new Person(Name: "Bob", Age: 11, ExpectedRank: 1),
				new Person(Name: "Sam", Age: 11, ExpectedRank: 1),
				new Person(Name: "Kim", Age: 11, ExpectedRank: 1),
				new Person(Name: "Tim", Age: 23, ExpectedRank: 2),
				new Person(Name: "Joe", Age: 23, ExpectedRank: 2),
				new Person(Name: "Mel", Age: 28, ExpectedRank: 3),
				new Person(Name: "Jim", Age: 28, ExpectedRank: 3),
				new Person(Name: "Jes", Age: 30, ExpectedRank: 4),
		}
			.GetTestingSequence(maxEnumerations: 3)
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
			var resultDenseRankBy = seq.DenseRankBy(x => x.Age).ToArray();
			Assert.Equal(expectedLength, resultDenseRankBy.Length);
			Assert.True(HasExpectedRank(resultDenseRankBy));

			var resultDenseRankByWithSortDirection = seq.DenseRankBy(x => x.Age).ToArray();
			Assert.Equal(expectedLength, resultDenseRankByWithSortDirection.Length);
			Assert.True(HasExpectedRank(resultDenseRankByWithSortDirection));
		}

		static bool HasExpectedRank(IEnumerable<(Person item, int rank)> result)
			=> result.All(x => x.rank == x.item.ExpectedRank);
	}

	public static IEnumerable<object[]> GetDateTimeSequences() =>
		Enumerable.Range(1, 10)
			.Select(x => new DateTime(2010, x, 20 - x))
			.GetTestingSequence()
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
			// invert the CompareTo operation to DenseRank in reverse order
			var resultDenseRank = seq.DenseRank(Comparer<DateTime>.Create((a, b) => -a.CompareTo(b)));
			resultDenseRank.AssertSequenceEqual(expected);

			// DenseRank called with a reverse comparer should be ordered correctly with OrderByDirection.Ascending
			var resultDenseRankWithSortDirection =
				seq.DenseRank(
					Comparer<DateTime>.Create((a, b) => -a.CompareTo(b)),
					OrderByDirection.Ascending
				);
			resultDenseRankWithSortDirection.AssertSequenceEqual(expected);
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
			var resultDenseRankBy = seq.DenseRankBy(x => x.Day, Comparer<int>.Create((a, b) => -a.CompareTo(b)));
			resultDenseRankBy.AssertSequenceEqual(expected);

			// Rank called with a reverse comparer and key selector should be ordered correctly with OrderByDirection.Ascending
			var resultRankByWithSortDirection =
				seq.RankBy(x => x.Day, Comparer<int>.Create((a, b) => -a.CompareTo(b)),
				OrderByDirection.Ascending
			);
			resultRankByWithSortDirection.AssertSequenceEqual(expected);
		}
	}
}
