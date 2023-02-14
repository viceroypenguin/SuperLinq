namespace Test;

/// <summary>
/// Tests of the Subset() family of extension methods.
/// </summary>
public class SubsetTest
{
	/// <summary>
	/// Verify that Subsets() behaves in a lazy manner.
	/// </summary>
	[Fact]
	public void TestSubsetsIsLazy()
	{
		_ = new BreakingSequence<int>().Subsets();
		_ = new BreakingSequence<int>().Subsets(5);
	}

	/// <summary>
	/// Verify that negative subset sizes result in an exception.
	/// </summary>
	[Fact]
	public void TestNegativeSubsetSize()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new BreakingSequence<int>().Subsets(-5));
	}

	/// <summary>
	/// Verify that requesting subsets larger than the original sequence length result in an exception.
	/// </summary>
	[Fact]
	public void TestSubsetLargerThanSequence()
	{
		using var sequence = Enumerable.Range(1, 10).AsTestingSequence();

		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			sequence
				.Subsets(15)
				.Consume());
	}

	/// <summary>
	/// Verify that the only subset of an empty sequence is the empty sequence.
	/// </summary>
	[Fact]
	public void TestEmptySequenceSubsets()
	{
		using var sequence = TestingSequence.Of<int>();

		var result = sequence.Subsets();
		result.Single().AssertSequenceEqual();
	}

	/// <summary>
	/// Verify that subsets are returned in increasing size, starting with the empty set.
	/// </summary>
	[Fact]
	public void TestSubsetsInIncreasingOrder()
	{
		using var sequence = Enumerable.Range(1, 10).AsTestingSequence();

		var result = sequence.Subsets();
		var prevSubsetCount = -1;
		foreach (var subset in result)
		{
			Assert.True(subset.Count >= prevSubsetCount);
			prevSubsetCount = subset.Count;
		}
	}

	/// <summary>
	/// Verify that the number of subsets returned is correct, but don't verify the subset contents.
	/// </summary>
	[Fact]
	public void TestAllSubsetsExpectedCount()
	{
		using var sequence = Enumerable.Range(1, 20).AsTestingSequence();

		var result = sequence.Subsets();
		Assert.Equal(Math.Pow(2, 20), result.Count());
	}

	/// <summary>
	/// Verify that the complete subset results for a known set are correct.
	/// </summary>
	[Fact]
	public void TestAllSubsetsExpectedResults()
	{
		using var sequence = Enumerable.Range(1, 4).AsTestingSequence();

		var result = sequence.Subsets();
		var expectedSubsets = new[]
		{
			Array.Empty<int>(),
			new[] {1}, new[] {2}, new[] {3}, new[] {4},
			new[] {1,2}, new[] {1,3}, new[] {1,4}, new[] {2,3}, new[] {2,4}, new[] {3,4},
			new[] {1,2,3}, new[] {1,2,4}, new[] {1,3,4}, new[] {2,3,4},
			new[] {1,2,3,4}
		};

		foreach (var (actual, expected) in result.Zip(expectedSubsets))
			expected.AssertSequenceEqual(actual);
	}

	public static IEnumerable<object[]> SubsetSizes() =>
		Enumerable.Range(1, 20).Select(i => new object[] { i, });

	/// <summary>
	/// Verify that the number of subsets for a given subset-size is correct.
	/// </summary>
	[Theory, MemberData(nameof(SubsetSizes))]
	public void TestKSubsetExpectedCount(int subsetSize)
	{
		using var sequence = Enumerable.Range(1, 20).AsTestingSequence();

		var result = sequence.Subsets(subsetSize);

		// number of subsets of a given size is defined by the binomial coefficient: c! / ((c-s)!*s!)
		Assert.Equal(Combinatorics.Binomial(20, subsetSize), result.Count());
	}

	/// <summary>
	/// Verify that k-subsets of a given set are in the correct order and contain the correct elements.
	/// </summary>
	[Fact]
	public void TestKSubsetExpectedResult()
	{
		using var sequence = Enumerable.Range(1, 6).AsTestingSequence();

		var result = sequence.Subsets(4);

		var expectedSubsets = new[]
		{
			new[] {1,2,3,4},
			new[] {1,2,3,5},
			new[] {1,2,3,6},
			new[] {1,2,4,5},
			new[] {1,2,4,6},
			new[] {1,2,5,6},
			new[] {1,3,4,5},
			new[] {1,3,4,6},
			new[] {1,3,5,6},
			new[] {1,4,5,6},
			new[] {2,3,4,5},
			new[] {2,3,4,6},
			new[] {2,3,5,6},
			new[] {2,4,5,6},
			new[] {3,4,5,6},
		};

		foreach (var (actual, expected) in result.Zip(expectedSubsets))
			expected.AssertSequenceEqual(actual);
	}
}
