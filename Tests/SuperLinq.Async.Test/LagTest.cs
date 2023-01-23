namespace Test.Async;

/// <summary>
/// Verify the behavior of the Lag operator
/// </summary>
public class LagTests
{
	/// <summary>
	/// Verify that lag behaves in a lazy manner.
	/// </summary>
	[Fact]
	public void TestLagIsLazy()
	{
		new AsyncBreakingSequence<int>().Lag(5, BreakingFunc.Of<int, int, int>());
		new AsyncBreakingSequence<int>().Lag(5, -1, BreakingFunc.Of<int, int, int>());
	}

	/// <summary>
	/// Verify that lagging by a negative offset results in an exception.
	/// </summary>
	[Fact]
	public void TestLagNegativeOffsetException()
	{
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			new AsyncBreakingSequence<int>().Lag(-10, (val, lagVal) => val));
	}

	/// <summary>
	/// Verify that attempting to lag by a zero offset will result in an exception
	/// </summary>
	[Fact]
	public void TestLagZeroOffset()
	{
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			new AsyncBreakingSequence<int>().Lag(0, (val, lagVal) => val + lagVal));
	}

	/// <summary>
	/// Verify that lag can accept an propagate a default value passed to it.
	/// </summary>
	[Fact]
	public async Task TestLagExplicitDefaultValue()
	{
		await using var sequence = Enumerable.Range(1, 100).AsTestingSequence();

		var result = await sequence.Lag(10, -1, (val, lagVal) => lagVal).ToListAsync();
		Assert.Equal(100, result.Count);
		result.Take(10).AssertSequenceEqual(Enumerable.Repeat(-1, 10));
	}

	[Fact]
	public async Task TestLagTuple()
	{
		await using var sequence = Enumerable.Range(1, 100).AsTestingSequence();

		var result = await sequence.Lag(10).ToListAsync();
		Assert.Equal(100, result.Count);
		result.AssertSequenceEqual(
			Enumerable.Range(1, 100).Select(x => (x, x <= 10 ? default : x - 10)));
	}

	/// <summary>
	/// Verify that lag will use default(T) if a specific default value is not supplied for the lag value.
	/// </summary>
	[Fact]
	public async Task TestLagImplicitDefaultValue()
	{
		await using var sequence = Enumerable.Range(1, 100).AsTestingSequence();

		var result = await sequence.Lag(10, (val, lagVal) => lagVal).ToListAsync();
		Assert.Equal(100, result.Count);
		result.Take(10).AssertSequenceEqual(Enumerable.Repeat(default(int), 10));
	}

	/// <summary>
	/// Verify that if the lag offset is greater than the sequence length lag
	/// still yields all of the elements of the source sequence.
	/// </summary>
	[Fact]
	public async Task TestLagOffsetGreaterThanSequenceLength()
	{
		await using var sequence = Enumerable.Range(1, 100).AsTestingSequence();

		var result = await sequence.Lag(100 + 1, (a, b) => a).ToListAsync();
		Assert.Equal(100, result.Count);
		result.AssertSequenceEqual(Enumerable.Range(1, 100));
	}

	/// <summary>
	/// Verify that lag actually yields the correct pair of values from the sequence
	/// when offsetting by a single item.
	/// </summary>
	[Fact]
	public async Task TestLagPassesCorrectLagValueOffsetBy1()
	{
		await using var sequence = Enumerable.Range(1, 100).AsTestingSequence();

		var result = await sequence.Lag(1, (a, b) => (A: a, B: b)).ToListAsync();
		Assert.Equal(100, result.Count);
		Assert.True(result.All(x => x.B == (x.A - 1)));
	}

	/// <summary>
	/// Verify that lag yields the correct pair of values from the sequence when
	/// offsetting by more than a single item.
	/// </summary>
	[Fact]
	public async Task TestLagPassesCorrectLagValuesOffsetBy2()
	{
		await using var sequence = Enumerable.Range(1, 100).AsTestingSequence();

		var result = await sequence.Lag(2, (a, b) => (A: a, B: b)).ToListAsync();
		Assert.Equal(100, result.Count);
		Assert.True(result.Skip(2).All(x => x.B == (x.A - 2)));
		Assert.True(result.Take(2).All(x => (x.A - x.B) == x.A));
	}

	[Fact]
	public async Task TestLagWithNullableReferences()
	{
		await using var words = TestingSequence.Of("foo", "bar", "baz", "qux");
		var result = words.Lag(2, (a, b) => new { A = a, B = b });
		await result.AssertSequenceEqual(
			new { A = "foo", B = (string?)null },
			new { A = "bar", B = (string?)null },
			new { A = "baz", B = (string?)"foo" },
			new { A = "qux", B = (string?)"bar" });
	}

	[Fact]
	public async Task TestLagWithNonNullableReferences()
	{
		await using var words = TestingSequence.Of("foo", "bar", "baz", "qux");
		var empty = string.Empty;
		var result = words.Lag(2, empty, (a, b) => new { A = a, B = b });
		await result.AssertSequenceEqual(
			new { A = "foo", B = empty },
			new { A = "bar", B = empty },
			new { A = "baz", B = "foo" },
			new { A = "qux", B = "bar" });
	}
}
