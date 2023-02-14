namespace Test.Async;

/// <summary>
/// Tests of the various overloads of <see cref="SuperEnumerable"/>.Random()
/// </summary>
public class RandomTest
{
	private const int RandomTrials = 10000;

	/// <summary>
	/// Verify that passing a negative maximum value yields an exception
	/// </summary>
	[Fact]
	public void TestNegativeMaxValueException()
	{
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			AsyncSuperEnumerable.Random(-10));
	}

	/// <summary>
	/// Verify that passing lower bound that is greater than the upper bound results
	/// in an exception.
	/// </summary>
	[Fact]
	public void TestMinValueGreaterThanMaxValueException()
	{
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			AsyncSuperEnumerable.Random(100, 10));
	}

	/// <summary>
	/// Verify that we can produce a valid sequence or random doubles between 0.0 and 1.0
	/// </summary>
	[Fact]
	public async Task TestRandomDouble()
	{
		var resultA = AsyncSuperEnumerable.RandomDouble().Take(RandomTrials);
		var resultB = AsyncSuperEnumerable.RandomDouble(new Random()).Take(RandomTrials);

		// NOTE: Unclear what should actually be verified here... some additional thought needed.
		Assert.Equal(RandomTrials, await resultA.CountAsync());
		Assert.Equal(RandomTrials, await resultB.CountAsync());
		Assert.True(await resultA.AllAsync(x => x >= 0.0 && x < 1.0));
		Assert.True(await resultB.AllAsync(x => x >= 0.0 && x < 1.0));
	}

	/// <summary>
	/// Verify that the max constraint is preserved by the sequence generator.
	/// </summary>
	[Fact]
	public async Task TestRandomMaxConstraint()
	{
		var resultA = AsyncSuperEnumerable.Random(100).Take(RandomTrials);
		var resultB = AsyncSuperEnumerable.Random(new Random(), 100).Take(RandomTrials);

		Assert.Equal(RandomTrials, await resultA.CountAsync());
		Assert.Equal(RandomTrials, await resultB.CountAsync());
		Assert.True(await resultA.AllAsync(x => x < 100));
		Assert.True(await resultB.AllAsync(x => x < 100));
	}

	/// <summary>
	/// Verify that the min/max constraints are preserved by the sequence generator.
	/// </summary>
	[Fact]
	public async Task TestRandomMinMaxConstraint()
	{
		var resultA = AsyncSuperEnumerable.Random(0, 100).Take(RandomTrials);
		var resultB = AsyncSuperEnumerable.Random(new Random(), 0, 100).Take(RandomTrials);

		Assert.Equal(RandomTrials, await resultA.CountAsync());
		Assert.Equal(RandomTrials, await resultB.CountAsync());
		Assert.True(await resultA.AllAsync(x => x >= 0 && x < 100));
		Assert.True(await resultB.AllAsync(x => x >= 0 && x < 100));
	}

	/// <summary>
	/// Evaluate that using a random sequence (with a given generator)
	/// is equivalent to a for loop accessing the same random generator.
	/// </summary>
	[Fact]
	public Task TestRandomEquivalence()
	{
		// must use a specific seed to ensure sequences will be identical
		var randA = new Random(12345);
		var randB = new Random(12345);

		var valuesA = new List<int>();
		for (var i = 0; i < RandomTrials; i++)
			valuesA.Add(randA.Next());

		var valuesB = AsyncSuperEnumerable.Random(randB)
			.Take(RandomTrials);

		return valuesB.AssertSequenceEqual(valuesA);
	}
}
