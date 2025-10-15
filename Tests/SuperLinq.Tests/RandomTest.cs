namespace SuperLinq.Tests;

/// <summary>
/// Tests of the various overloads of <see cref="SuperEnumerable"/>.Random()
/// </summary>
public sealed class RandomTest
{
	private const int RandomTrials = 10000;

	/// <summary>
	/// Verify that passing a negative maximum value yields an exception
	/// </summary>
	[Fact]
	public void TestNegativeMaxValueException()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			SuperEnumerable.Random(-10));
	}

	/// <summary>
	/// Verify that passing lower bound that is greater than the upper bound results
	/// in an exception.
	/// </summary>
	[Fact]
	public void TestMinValueGreaterThanMaxValueException()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			SuperEnumerable.Random(100, 10));
	}

	/// <summary>
	/// Verify that we can produce a valid sequence or random doubles between 0.0 and 1.0
	/// </summary>
	[Fact]
	public void TestRandomDouble()
	{
		var resultA = SuperEnumerable.RandomDouble().Take(RandomTrials);
		var resultB = SuperEnumerable.RandomDouble(new Random()).Take(RandomTrials);

		// NOTE: Unclear what should actually be verified here... some additional thought needed.
		Assert.Equal(RandomTrials, resultA.Count());
		Assert.Equal(RandomTrials, resultB.Count());
		Assert.True(resultA.All(x => x is >= 0.0 and < 1.0));
		Assert.True(resultB.All(x => x is >= 0.0 and < 1.0));
	}

	/// <summary>
	/// Verify that the max constraint is preserved by the sequence generator.
	/// </summary>
	[Fact]
	public void TestRandomMaxConstraint()
	{
		var resultA = SuperEnumerable.Random(100).Take(RandomTrials);
		var resultB = SuperEnumerable.Random(new Random(), 100).Take(RandomTrials);

		Assert.Equal(RandomTrials, resultA.Count());
		Assert.Equal(RandomTrials, resultB.Count());
		Assert.True(resultA.All(x => x < 100));
		Assert.True(resultB.All(x => x < 100));
	}

	/// <summary>
	/// Verify that the min/max constraints are preserved by the sequence generator.
	/// </summary>
	[Fact]
	public void TestRandomMinMaxConstraint()
	{
		var resultA = SuperEnumerable.Random(0, 100).Take(RandomTrials);
		var resultB = SuperEnumerable.Random(new Random(), 0, 100).Take(RandomTrials);

		Assert.Equal(RandomTrials, resultA.Count());
		Assert.Equal(RandomTrials, resultB.Count());
		Assert.True(resultA.All(x => x is >= 0 and < 100));
		Assert.True(resultB.All(x => x is >= 0 and < 100));
	}

	/// <summary>
	/// Evaluate that using a random sequence (with a given generator)
	/// is equivalent to a for loop accessing the same random generator.
	/// </summary>
	[Fact]
	public void TestRandomEquivalence()
	{
		// must use a specific seed to ensure sequences will be identical
		var randA = new Random(12345);
		var randB = new Random(12345);

		var valuesA = new List<int>();
		for (var i = 0; i < RandomTrials; i++)
			valuesA.Add(randA.Next());

		var randomSeq = SuperEnumerable.Random(randB);
		var valuesB = randomSeq.Take(RandomTrials);

		Assert.Equal(valuesB, valuesA);
	}
}
