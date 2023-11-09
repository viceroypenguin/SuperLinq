namespace Test;

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

	public static IEnumerable<object[]> GetSimpleSequences() =>
		Enumerable.Repeat(1, 10)
			.GetTestingSequence()
			.Select(x => new object[] { x, });

	[Theory]
	[MemberData(nameof(GetSimpleSequences))]
	public void TestRankNullComparer(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			seq
				.DenseRank()
				.AssertSequenceEqual(
					Enumerable.Repeat((1, 1), 10));
		}
	}

	[Theory]
	[MemberData(nameof(GetSimpleSequences))]
	public void TestRankByNullComparer(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			seq
				.DenseRankBy(SuperEnumerable.Identity)
				.AssertSequenceEqual(
					Enumerable.Repeat((1, 1), 10));
		}
	}

	public static IEnumerable<object[]> GetDescendingIntSequences() =>
		Enumerable.Range(456, 100)
			.Reverse()
			.GetTestingSequence()
			.Select(x => new object[] { x, });

	[Theory]
	[MemberData(nameof(GetDescendingIntSequences))]
	public void TestRankDescendingSequence(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			seq
				.DenseRank()
				.AssertSequenceEqual(
					Enumerable.Range(456, 100)
						.Select((x, i) => (x, i + 1)));
		}
	}

	public static IEnumerable<object[]> GetAscendingIntSequences() =>
		Enumerable.Range(456, 100)
			.GetTestingSequence()
			.Select(x => new object[] { x, });

	[Theory]
	[MemberData(nameof(GetDescendingIntSequences))]
	public void TestRankAscendingSequence(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			seq
				.DenseRank()
				.AssertSequenceEqual(
					Enumerable.Range(456, 100)
						.Select((x, i) => (x, i + 1)));
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
		using (seq)
		{
			var expected =
				SuperEnumerable.Range(1, 10, 1)
					.SelectMany((x, i) => Enumerable.Repeat(x, 3)
						// should be 0-9, repeated three times, with ranks 1,2,...,10
						.Select(y => (item: i, index: y)));
			seq
				.DenseRank()
				.AssertSequenceEqual(expected);
		}
	}

	public sealed record Person(string Name, int Age, int ExpectedRank);
	public static IEnumerable<object[]> GetPersonSequences() =>
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

	/// <summary>
	/// Verify that we can rank items by an arbitrary key produced from the item.
	/// </summary>
	[Theory]
	[MemberData(nameof(GetPersonSequences))]
	public void TestRankByKeySelector(IDisposableEnumerable<Person> seq)
	{
		using (seq)
		{
			var result = seq.DenseRankBy(x => x.Age).ToArray();
			Assert.Equal(8, result.Length);
			Assert.True(result.All(x => x.rank == x.item.ExpectedRank));
		}
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
		using (seq)
		{
			// invert the CompareTo operation to Rank in reverse order
			var resultA = seq.DenseRank(Comparer<DateTime>.Create((a, b) => -a.CompareTo(b)));
			resultA
				.AssertSequenceEqual(
					Enumerable.Range(1, 10)
						.Select(x => new DateTime(2010, x, 20 - x))
						.OrderByDescending(SuperEnumerable.Identity)
						.Select((x, i) => (x, i + 1)));
		}
	}

	[Theory]
	[MemberData(nameof(GetDateTimeSequences))]
	public void TestRankCustomComparer2(IDisposableEnumerable<DateTime> seq)
	{
		using (seq)
		{
			var resultB = seq.DenseRankBy(x => x.Day, Comparer<int>.Create((a, b) => -a.CompareTo(b)));
			resultB
				.AssertSequenceEqual(
					Enumerable.Range(1, 10)
						.Select(x => new DateTime(2010, x, 20 - x))
						.OrderByDescending(x => x.Day)
						.Select((x, i) => (x, i + 1)));
		}
	}
}
