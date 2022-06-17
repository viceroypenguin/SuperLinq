namespace Test;

/// <summary>
/// Tests that verify the behavior of the RandomSubset() operator
/// </summary>
public class RandomSubsetTest
{
	/// <summary>
	/// Verify that RandomSubset() behaves in a lazy manner.
	/// </summary>
	[Fact]
	public void TestRandomSubsetIsLazy()
	{
		new BreakingSequence<int>().RandomSubset(10);
		new BreakingSequence<int>().RandomSubset(10, new Random());
	}

	/// <summary>
	/// Verify that involving RandomSubsets with a subset size less than 0 results in an exception.
	/// </summary>
	[Fact]
	public void TestRandomSubsetNegativeSubsetSize()
	{
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			Enumerable.Range(1, 10).RandomSubset(-5));
	}

	/// <summary>
	/// Verify that involving RandomSubsets with a subset size less than 0 results in an exception.
	/// </summary>
	[Fact]
	public void TestRandomSubsetNegativeSubsetSize2()
	{
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			Enumerable.Range(1, 10).RandomSubset(-1, new Random()));
	}

	/// <summary>
	/// Verify that the 0-size random subset of the empty set is the empty set.
	/// </summary>
	[Fact]
	public void TestRandomSubsetOfEmptySequence()
	{
		var sequence = Enumerable.Empty<int>();
		var result = sequence.RandomSubset(0); // we can only get subsets <= sequence.Count()

		Assert.Empty(result);
	}

	/// <summary>
	/// Verify that RandomSubset can produce a random subset of equal length to the original sequence.
	/// </summary>
	[Fact]
	public void TestRandomSubsetSameLengthAsSequence()
	{
		const int count = 100;
		var sequence = Enumerable.Range(1, count);
		var resultA = sequence.RandomSubset(count);
		var resultB = sequence.RandomSubset(count, new Random(12345));

		// ensure random subset is always a complete reordering of original sequence
		Assert.Equal(count, resultA.Distinct().Count());
		Assert.Equal(count, resultB.Distinct().Count());
	}

	/// <summary>
	/// Verify that RandomSubset can produce a random subset shorter than the original sequence.
	/// </summary>
	[Fact]
	public void TestRandomSubsetShorterThanSequence()
	{
		const int count = 100;
		const int subsetSize = 20;
		var sequence = Enumerable.Range(1, count);
		var resultA = sequence.RandomSubset(subsetSize);
		var resultB = sequence.RandomSubset(subsetSize, new Random(12345));

		// ensure random subset is always a distinct subset of original sequence
		Assert.Equal(subsetSize, resultA.Distinct().Count());
		Assert.Equal(subsetSize, resultB.Distinct().Count());
	}

	/// <summary>
	/// Verify that attempting to fetch a random subset longer than the original sequence
	/// results in an exception. Only thrown when the resulting random sequence is enumerated.
	/// </summary>
	[Fact]
	public void TestRandomSubsetLongerThanSequence()
	{
		const int count = 100;
		const int subsetSize = count + 5;
		var sequence = Enumerable.Range(1, count);

		Assert.Throws<ArgumentOutOfRangeException>(() =>
		{
			sequence.RandomSubset(subsetSize).Consume();
		});
	}

	/// <summary>
	/// Verify that attempting to fetch a random subset longer than the original sequence
	/// results in an exception. Only thrown when the resulting random sequence is enumerated.
	/// </summary>
	[Fact]
	public void TestRandomSubsetLongerThanSequence2()
	{
		const int count = 100;
		const int subsetSize = count + 5;
		var sequence = Enumerable.Range(1, count);

		Assert.Throws<ArgumentOutOfRangeException>(() =>
		{
			sequence.RandomSubset(subsetSize, new Random(1234)).Consume();
		});
	}

	/// <summary>
	/// Verify that RandomSubset does not exhibit selection bias in the subsets it returns.
	/// </summary>
	/// <remarks>
	/// It's actually a complicated matter to ensure that a random process does not exhibit
	/// any kind of bias. In this test, we want to make sure that the probability of any
	/// particular subset being returned is roughly the same as any other. Here's how.
	///
	/// This test selects a random subset of length N from an ascending sequence 1..N.
	/// It then adds up the values of the random result into an accumulator array. After many
	/// iterations, we would hope that each index of the accumulator array approach the same
	/// value. Of course, in the real world, there will be some variance, and the values will
	/// never be the same. However, we can compute the relative standard deviation (RSD) of the
	/// variance. As the number of trials increases, the RSD should continue decreasing
	/// asymptotically towards zero. By running several iterations of increasing trial size,
	/// we can assert that the RSD continually decreases, approaching zero.
	///
	/// For math geeks who read this:
	///   A decreasing RSD demonstrates that the random subsets form a cumulative distribution
	///   approaching unity (1.0). Which, given that the original sequence was monotonic, implies
	///   there cannot be a selection bias in the returned subsets - quod erat demonstrandum (QED).
	/// </remarks>
	[Fact(Skip = "Explicit")]
	public void TestRandomSubsetIsUnbiased()
	{
		const int count = 20;
		var sequence = Enumerable.Range(1, count);

		var rsdTrials = new[] { 1000, 10000, 100000, 500000, 10000000 };
		var rsdResults = new[] { 0.0, 0.0, 0.0, 0.0, 0.0 };

		var trialIndex = 0;
		foreach (var trialSize in rsdTrials)
		{
			var biasAccumulator = Enumerable.Repeat(0.0, count).ToArray();

			for (var i = 0; i < trialSize; i++)
			{
				var index = 0;
				var result = sequence.RandomSubset(count);
				foreach (var itemA in result)
					biasAccumulator[index++] += itemA;
			}

			rsdResults[trialIndex++] = RelativeStandardDeviation(biasAccumulator);
		}

		// ensure that wth increasing trial size the a RSD% continually decreases
		for (var j = 0; j < rsdResults.Length - 1; j++)
			Assert.True(rsdResults[j + 1] < rsdResults[j]);

		// ensure that the RSD% for the 5M trial size is < 1.0    (this is somewhat arbitrary)
		Assert.True(rsdResults[^1] < 1.0);

		// for sanity, we output the RSD% values as a cross-check, the expected result should be
		// that the RSD% rapidly decreases and eventually drops below 1.0
		Console.WriteLine("RSD% = {0:0.00000}, {1:0.00000}, {2:0.00000}, {3:0.00000}, {4:0.00000}",
						  rsdResults[0], rsdResults[1], rsdResults[2], rsdResults[3], rsdResults[4]);
	}

	/// <summary>
	/// Verify that RandomSubsets is idempotent with respect to the original sequence.
	/// </summary>
	/// <remarks>
	/// Internally, RandomSubsets perform some in-place operations on a copy of the sequence.
	/// This attempts to verify that the original sequence remains unaltered after a random
	/// subset is returned and enumerated.
	/// </remarks>
	[Fact]
	public void TestRandomSubsetIsIdempotent()
	{
		const int count = 100;
		const int subsetSize = count;
		var sequence = Enumerable.Range(1, count).ToArray();
		var sequenceClone = sequence.ToArray();
		var resultA = sequence.RandomSubset(subsetSize);
		var resultB = sequence.RandomSubset(subsetSize);

		// force complete enumeration of random subsets
		resultA.Consume();
		resultB.Consume();

		// verify the original sequence is untouched
		Assert.Equal(sequenceClone, sequence);
	}

	/// <summary>
	/// Verify that RandomSubset produces subset where all elements belongs to original sequence.
	/// </summary>
	[Fact]
	public void TestRandomSubsetReturnsOriginalSequenceElements()
	{
		const int count = 100;
		var sequence = Enumerable.Range(1, count);
		var result = sequence.RandomSubset(count, new Random(12345));

		// we do not test overload without seed because it can return original sequence
		Assert.NotEqual(result, sequence);

		// ensure random subset returns exactly the same elements of original sequence
		Assert.Equal(result.OrderBy(x => x), sequence);
	}

	static double RelativeStandardDeviation(IEnumerable<double> values)
	{
		var average = values.Average();
		var standardDeviation = StandardDeviationInternal(values, average);
		return (standardDeviation * 100.0) / average;
	}

	static double StandardDeviationInternal(IEnumerable<double> values, double average)
	{
		return Math.Sqrt(values.Select(value => Math.Pow(value - average, 2.0)).Average());
	}
}
