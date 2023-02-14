namespace Test;

/// <summary>
/// Verify the behavior of the Cartesian operator
/// </summary>
public class CartesianTests
{
	/// <summary>
	/// Verify that the Cartesian product is evaluated in a lazy fashion on demand.
	/// </summary>
	[Fact]
	public void TestCartesianIsLazy()
	{
		_ = new BreakingSequence<string>()
			.Cartesian(new BreakingSequence<int>(),
					   BreakingFunc.Of<string, int, bool>());
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
			Assert.Equal(sequenceA, result);
		}

		using (var tsA = sequenceA.AsTestingSequence(TestingSequence.Options.AllowRepeatedMoveNexts))
		using (var tsB = sequenceB.AsTestingSequence(TestingSequence.Options.AllowRepeatedMoveNexts))
		{
			var result = tsB.Cartesian(tsA, (a, b) => a + b);
			Assert.Equal(sequenceA, result);
		}
	}

	/// <summary>
	/// Verify that the number of elements in a Cartesian product is the product of the number of elements of each sequence
	/// </summary>
	[Fact]
	public void TestCartesianProductCount()
	{
		const int countA = 100;
		const int countB = 75;
		const int expectedCount = countA * countB;
		using var sequenceA = Enumerable.Range(1, countA).AsTestingSequence(TestingSequence.Options.AllowRepeatedMoveNexts);
		using var sequenceB = Enumerable.Range(1, countB).AsTestingSequence(TestingSequence.Options.AllowRepeatedMoveNexts);

		var result = sequenceA.Cartesian(sequenceB, (a, b) => a + b);

		Assert.Equal(expectedCount, result.Count());
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

		using var sequenceA = Enumerable.Range(1, CountA).AsTestingSequence(TestingSequence.Options.AllowRepeatedMoveNexts);
		using var sequenceB = Enumerable.Range(1, CountB).AsTestingSequence(TestingSequence.Options.AllowRepeatedMoveNexts);
		using var sequenceC = Enumerable.Range(1, CountC).AsTestingSequence(TestingSequence.Options.AllowRepeatedMoveNexts);
		using var sequenceD = Enumerable.Range(1, CountD).AsTestingSequence(TestingSequence.Options.AllowRepeatedMoveNexts);

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

		using var tsA = sequenceA.AsTestingSequence(TestingSequence.Options.AllowRepeatedMoveNexts);
		using var tsB = sequenceB.AsTestingSequence(TestingSequence.Options.AllowRepeatedMoveNexts);

		var result = tsA.Cartesian(tsB, (a, b) => new { A = a, B = b })
						.ToArray();

		// verify that the expected number of results is correct
		Assert.Equal(sequenceA.Count() * sequenceB.Count(), result.Length);

		// ensure that all "cells" were visited by the cartesian product
		foreach (var coord in result)
			expectedSet[coord.A][coord.B] = true;
		Assert.True(expectedSet.SelectMany(SuperEnumerable.Identity).All(SuperEnumerable.Identity));
	}

	/// <summary>
	/// Verify that if either sequence passed to Cartesian is empty, the result
	/// is an empty sequence.
	/// </summary>
	[Fact]
	public void TestEmptyCartesianEvaluation()
	{
		var sequence = Enumerable.Range(0, 5);

		var resultA = sequence.Cartesian(Enumerable.Empty<int>(), (a, b) => new { A = a, B = b });
		var resultB = Enumerable.Empty<int>().Cartesian(sequence, (a, b) => new { A = a, B = b });
		var resultC = Enumerable.Empty<int>().Cartesian(Enumerable.Empty<int>(), (a, b) => new { A = a, B = b });

		Assert.Empty(resultA);
		Assert.Empty(resultB);
		Assert.Empty(resultC);
	}
}
