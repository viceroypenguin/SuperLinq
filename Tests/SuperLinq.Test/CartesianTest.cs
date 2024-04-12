namespace Test;

/// <summary>
/// Verify the behavior of the Cartesian operator
/// </summary>
public sealed class CartesianTests
{
	/// <summary>
	/// Verify that the Cartesian product is evaluated in a lazy fashion on demand.
	/// </summary>
	[Fact]
	public void TestCartesianIsLazy()
	{
		_ = new BreakingSequence<string>()
			.Cartesian(
				new BreakingSequence<int>(),
				BreakingFunc.Of<string, int, bool>()
			);
	}

	/// <summary>
	/// Verify that the Cartesian product of two empty sequences is an empty sequence
	/// </summary>
	[Fact]
	public void TestCartesianOfEmptySequences()
	{
		using var sequenceA = Enumerable.Empty<int>().AsTestingSequence();
		using var sequenceB = Enumerable.Empty<int>().AsTestingSequence();

		var result = sequenceA.Cartesian(sequenceB, (a, b) => a + b);

		Assert.Empty(result);
	}

	/// <summary>
	/// Verify that the Cartesian product of an empty and non-empty sequence is an empty sequence
	/// </summary>
	[Fact]
	public void TestCartesianOfEmptyAndNonEmpty()
	{
		var sequenceA = Enumerable.Empty<int>();
		var sequenceB = Enumerable.Repeat(1, 10);

		using (var tsA = sequenceA.AsTestingSequence())
		using (var tsB = sequenceB.AsTestingSequence())
		{
			var result = tsA.Cartesian(tsB, (a, b) => a + b);
			result.AssertSequenceEqual(sequenceA);
		}

		using (var tsA = sequenceA.AsTestingSequence())
		using (var tsB = sequenceB.AsTestingSequence())
		{
			var result = tsB.Cartesian(tsA, (a, b) => a + b);
			result.AssertSequenceEqual(sequenceA);
		}
	}

	/// <summary>
	/// Verify that the number of elements in a Cartesian product is the product of the number of elements of each sequence
	/// </summary>
	[Fact]
	public void TestCartesianProductCount()
	{
		using var sequenceA = Enumerable.Range(1, 100).AsTestingSequence();
		using var sequenceB = Enumerable.Range(1, 75).AsTestingSequence();

		var result = sequenceA.Cartesian(sequenceB, (a, b) => a + b);

		Assert.Equal(100 * 75, result.Count());
	}

	/// <summary>
	/// Verify that the number of elements in a Cartesian product is the product of the number of elements of each sequence,
	/// even when there are more than two sequences
	/// </summary>
	[Fact]
	public void TestCartesianProductCountMultidimensional()
	{
		const int CountA = 10;
		const int CountB = 9;
		const int CountC = 8;
		const int CountD = 7;

		using var sequenceA = Enumerable.Range(1, CountA).AsTestingSequence();
		using var sequenceB = Enumerable.Range(1, CountB).AsTestingSequence();
		using var sequenceC = Enumerable.Range(1, CountC).AsTestingSequence();
		using var sequenceD = Enumerable.Range(1, CountD).AsTestingSequence();

		var result = sequenceA.Cartesian(sequenceB, sequenceC, sequenceD, (a, b, c, d) => a + b + c + d);

		const int ExpectedCount = CountA * CountB * CountC * CountD;
		Assert.Equal(ExpectedCount, result.Count());
	}

	/// <summary>
	/// Verify that each combination is produced in the Cartesian product
	/// </summary>
	[Fact]
	public void TestCartesianProductCombinations()
	{
		var sequenceA = Enumerable.Range(0, 5);
		var sequenceB = Enumerable.Range(0, 5);

		var expectedSet = new[]
		{
			Enumerable.Repeat(element: false, 5).ToArray(),
			Enumerable.Repeat(element: false, 5).ToArray(),
			Enumerable.Repeat(element: false, 5).ToArray(),
			Enumerable.Repeat(element: false, 5).ToArray(),
			Enumerable.Repeat(element: false, 5).ToArray(),
		};

		using var tsA = sequenceA.AsTestingSequence();
		using var tsB = sequenceB.AsTestingSequence();

		var result = tsA
			.Cartesian(tsB, (a, b) => new { A = a, B = b })
			.ToArray();

		// verify that the expected number of results is correct
		Assert.Equal(sequenceA.Count() * sequenceB.Count(), result.Length);

		// ensure that all "cells" were visited by the cartesian product
		foreach (var coord in result)
			expectedSet[coord.A][coord.B] = true;

		Assert.True(expectedSet.SelectMany(SuperEnumerable.Identity).All(SuperEnumerable.Identity));
	}

	[Fact]
	public void TestAllCartesianMethods()
	{
		using var ts1 = Enumerable.Range(0, 1).AsTestingSequence(maxEnumerations: 7);
		using var ts2 = Enumerable.Range(0, 1).AsTestingSequence(maxEnumerations: 7);
		using var ts3 = Enumerable.Range(0, 1).AsTestingSequence(maxEnumerations: 6);
		using var ts4 = Enumerable.Range(0, 1).AsTestingSequence(maxEnumerations: 5);
		using var ts5 = Enumerable.Range(0, 1).AsTestingSequence(maxEnumerations: 4);
		using var ts6 = Enumerable.Range(0, 1).AsTestingSequence(maxEnumerations: 3);
		using var ts7 = Enumerable.Range(0, 1).AsTestingSequence(maxEnumerations: 2);
		using var ts8 = Enumerable.Range(0, 1).AsTestingSequence();

		ts1.Cartesian(ts2).AssertSequenceEqual((0, 0));
		ts1.Cartesian(ts2, ts3).AssertSequenceEqual((0, 0, 0));
		ts1.Cartesian(ts2, ts3, ts4).AssertSequenceEqual((0, 0, 0, 0));
		ts1.Cartesian(ts2, ts3, ts4, ts5).AssertSequenceEqual((0, 0, 0, 0, 0));
		ts1.Cartesian(ts2, ts3, ts4, ts5, ts6).AssertSequenceEqual((0, 0, 0, 0, 0, 0));
		ts1.Cartesian(ts2, ts3, ts4, ts5, ts6, ts7).AssertSequenceEqual((0, 0, 0, 0, 0, 0, 0));
		ts1.Cartesian(ts2, ts3, ts4, ts5, ts6, ts7, ts8).AssertSequenceEqual((0, 0, 0, 0, 0, 0, 0, 0));
	}
}
