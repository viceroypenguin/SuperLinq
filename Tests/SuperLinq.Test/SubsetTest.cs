namespace Test;

/// <summary>
/// Tests of the Subset() family of extension methods.
/// </summary>
public sealed class SubsetTest
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

	public static IEnumerable<object?[]> GetSubsetSequences() =>
		Enumerable.Range(1, 10)
			.GetCollectionSequences()
			.Select(x => new object?[] { x });

	/// <summary>
	/// Verify that requesting subsets larger than the original sequence length result in an exception.
	/// </summary>
	[Theory]
	[MemberData(nameof(GetSubsetSequences))]
	public void TestSubsetLargerThanSequence(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
				seq
					.Subsets(15)
					.Consume());
		}
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

	[Fact]
	public void TestSizeZeroSubsets()
	{
		using var sequence = TestingSequence.Of(1, 2, 3);

		var result = sequence.Subsets(0);
		result.Single().AssertSequenceEqual();
	}

	/// <summary>
	/// Verify that subsets are returned in increasing size, starting with the empty set.
	/// </summary>
	[Theory]
	[MemberData(nameof(GetSubsetSequences))]
	public void TestSubsetsInIncreasingOrder(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.Subsets();

			var prevSubsetCount = -1;
			foreach (var subset in result)
			{
				Assert.True(subset.Count >= prevSubsetCount);
				prevSubsetCount = subset.Count;
			}
		}
	}

	/// <summary>
	/// Verify that the number of subsets returned is correct, but don't verify the subset contents.
	/// </summary>
	[Theory]
	[MemberData(nameof(GetSubsetSequences))]
	public void TestAllSubsetsExpectedCount(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.Subsets().Count();
			Assert.Equal(Math.Pow(2, 10), result);
		}
	}

	/// <summary>
	/// Verify that the complete subset results for a known set are correct.
	/// </summary>
	[Fact]
	public void TestAllSubsetsExpectedResults()
	{
		using var sequence = Enumerable.Range(1, 4).AsTestingSequence();

		var result = sequence.Subsets();
		var expectedSubsets = new int[][]
		{
			[],
			[1], [2], [3], [4],
			[1,2], [1,3], [1,4], [2,3], [2,4], [3,4],
			[1,2,3], [1,2,4], [1,3,4], [2,3,4],
			[1,2,3,4],
		};

		foreach (var (actual, expected) in result.Zip(expectedSubsets))
			expected.AssertSequenceEqual(actual);
	}

	public static IEnumerable<object[]> SubsetSizes() =>
		Enumerable.Range(1, 20)
			.SelectMany(size =>
				Enumerable.Range(1, 20)
					.GetCollectionSequences()
					.Select(seq => new object[]
					{
						seq,
						size,
					}));

	/// <summary>
	/// Verify that the number of subsets for a given subset-size is correct.
	/// </summary>
	[Theory]
	[MemberData(nameof(SubsetSizes))]
	public void TestKSubsetExpectedCount(IDisposableEnumerable<int> seq, int subsetSize)
	{
		using (seq)
		{
			var result = seq.Subsets(subsetSize).Count();

			// number of subsets of a given size is defined by the binomial coefficient: c! / ((c-s)!*s!)
			var expected = Combinatorics.Binomial(20, subsetSize);
			Assert.Equal(expected, result);
		}
	}

	/// <summary>
	/// Verify that k-subsets of a given set are in the correct order and contain the correct elements.
	/// </summary>
	[Fact]
	public void TestKSubsetExpectedResult()
	{
		using var sequence = Enumerable.Range(1, 6).AsTestingSequence();

		var result = sequence.Subsets(4);

		var expectedSubsets = new int[][]
		{
			[1,2,3,4,],
			[1,2,3,5,],
			[1,2,3,6,],
			[1,2,4,5,],
			[1,2,4,6,],
			[1,2,5,6,],
			[1,3,4,5,],
			[1,3,4,6,],
			[1,3,5,6,],
			[1,4,5,6,],
			[2,3,4,5,],
			[2,3,4,6,],
			[2,3,5,6,],
			[2,4,5,6,],
			[3,4,5,6,],
		};

		foreach (var (actual, expected) in result.Zip(expectedSubsets))
			expected.AssertSequenceEqual(actual);
	}
}
