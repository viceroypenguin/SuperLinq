using System.Collections;
using CommunityToolkit.Diagnostics;

namespace Test;

/// <summary>
/// Tests that verify the behavior of the Permutations() operator.
/// </summary>
public sealed class PermutationsTest
{
	/// <summary>
	/// Verify that the permutation of the empty set is the empty set.
	/// </summary>
	[Fact]
	public void TestCardinalityZeroPermutation()
	{
		using var emptySet = TestingSequence.Of<int>();

		var permutations = emptySet.Permutations();

		// should contain a single result: the empty set itself
		permutations.Single().AssertSequenceEqual();
	}

	public static IEnumerable<object[]> GetTooLongSequences() =>
		Enumerable.Range(1, 22)
			.GetBreakingCollectionSequences()
			.Select(x => new object[] { x, });

	[Theory]
	[MemberData(nameof(GetTooLongSequences))]
	public void TestExceptionWhenTooLong(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var ex = Assert.Throws<ArgumentException>(
				"sequence",
				() => seq.Permutations().Consume());

			Assert.Equal("Input set is too large to permute properly. (Parameter 'sequence')", ex.Message);
		}
	}

	/// <summary>
	/// Verify that there is one permutation of a set of one item
	/// </summary>
	[Fact]
	public void TestCardinalityOnePermutation()
	{
		using var set = TestingSequence.Of(42);

		var permutations = set.Permutations();

		// should contain a single result: the set itself
		permutations.Single().AssertSequenceEqual(42);
	}

	/// <summary>
	/// Verify that there are two permutations of a set of two items
	/// and confirm that the permutations are correct.
	/// </summary>
	[Fact]
	public void TestCardinalityTwoPermutation()
	{
		using var set = TestingSequence.Of(42, 37);

		var permutations = set.Permutations().ToList();

		// should contain two results: the set itself and its reverse
		Assert.Equal(2, permutations.Count);
		permutations[0].AssertSequenceEqual(42, 37);
		permutations[1].AssertSequenceEqual(37, 42);
	}

	/// <summary>
	/// Verify that there are six (3!) permutations of a set of three items
	/// and confirm the permutations are correct.
	/// </summary>
	[Fact]
	public void TestCardinalityThreePermutation()
	{
		using var set = TestingSequence.Of(42, 11, 100);

		var permutations = set.Permutations();
		var expectedPermutations = new int[][]
		{
			[42, 11, 100,],
			[42, 100, 11,],
			[11, 100, 42,],
			[11, 42, 100,],
			[100, 11, 42,],
			[100, 42, 11,],
		};

		// should contain six permutations (as defined above)
		permutations.AssertCollectionEqual(
			expectedPermutations,
			EqualityComparer.Create<IList<int>>(
				(x, y) => x.SequenceEqual(y),
				StructuralComparisons.StructuralEqualityComparer.GetHashCode));
	}

	/// <summary>
	/// Verify there are 24 (4!) permutations of a set of four items
	/// and confirm the permutations are correct.
	/// </summary>
	[Fact]
	public void TestCardinalityFourPermutation()
	{
		using var set = TestingSequence.Of(42, 11, 100, 89);

		var permutations = set.Permutations();
		var expectedPermutations = new int[][]
		{
			[42, 11, 100, 89,],
			[42, 100, 11, 89,],
			[11, 100, 42, 89,],
			[11, 42, 100, 89,],
			[100, 11, 42, 89,],
			[100, 42, 11, 89,],
			[42, 11, 89, 100,],
			[42, 100, 89, 11,],
			[11, 100, 89, 42,],
			[11, 42, 89, 100,],
			[100, 11, 89, 42,],
			[100, 42, 89, 11,],
			[42, 89, 11, 100,],
			[42, 89, 100, 11,],
			[11, 89, 100, 42,],
			[11, 89, 42, 100,],
			[100, 89, 11, 42,],
			[100, 89, 42, 11,],
			[89, 42, 11, 100,],
			[89, 42, 100, 11,],
			[89, 11, 100, 42,],
			[89, 11, 42, 100,],
			[89, 100, 11, 42,],
			[89, 100, 42, 11,],
		};

		permutations.AssertCollectionEqual(
			expectedPermutations,
			EqualityComparer.Create<IList<int>>(
				(x, y) => x.SequenceEqual(y),
				StructuralComparisons.StructuralEqualityComparer.GetHashCode));
	}

	/// <summary>
	/// Verify that the number of expected permutations of sets of size 5 through 10
	/// are equal to the factorial of the set size.
	/// </summary>
	[Fact]
	public void TestHigherCardinalityPermutations()
	{
		// NOTE: Testing higher cardinality permutations by exhaustive comparison becomes tedious
		//       above cardiality 4 sets, as the number of permutations is N! (factorial). To provide
		//       some level of verification, though, we will simply test the count of items in the
		//       permuted sets, and verify they are equal to the expected number (count!).

		// NOTE: Generating all permutations for sets larger than about 10 items is computationally
		//       expensive and generally impractical - especially since each additional step adds
		//       less and less to our confidence in the underlying implementation.
		//       We will assume that if the algorithm scales to sets of up to 10 items, it will work
		//       with any size set.
		var setsToPermute = Enumerable.Range(5, 6).Select(s => Enumerable.Range(1, s));

		foreach (var set in setsToPermute)
		{
			using var xs = set.AsTestingSequence();
			var permutedSet = xs.Permutations();
			Assert.Equal(
				Combinatorics.Factorial(set.Count()),
				permutedSet.Count());
		}
	}

	/// <summary>
	/// Verify that the Permutations() extension does not begin evaluation until the
	/// resulting sequence is iterated.
	/// </summary>
	[Fact]
	public void TestPermutationsIsLazy()
	{
		_ = new BreakingSequence<int>().Permutations();
	}

	/// <summary>
	/// Verify that each permutation produced is a new object, this ensures that callers
	/// can request permutations and cache or store them without them being overwritten.
	/// </summary>
	[Fact]
	public void TestPermutationsAreIndependent()
	{
		using var set = Enumerable.Range(1, 4).Select(i => i * 10).AsTestingSequence();

		var permutedSets = set.Permutations().ToList();

		for (var i = 0; i < permutedSets.Count; i++)
		{
			for (var j = i + 1; j < permutedSets.Count; j++)
			{
				Guard.IsFalse(permutedSets[i].SequenceEqual(permutedSets[j]));
			}
		}
	}
}
