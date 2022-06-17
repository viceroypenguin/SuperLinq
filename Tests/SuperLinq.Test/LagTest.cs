namespace Test;

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
		new BreakingSequence<int>().Lag(5, BreakingFunc.Of<int, int, int>());
		new BreakingSequence<int>().Lag(5, -1, BreakingFunc.Of<int, int, int>());
	}

	/// <summary>
	/// Verify that lagging by a negative offset results in an exception.
	/// </summary>
	[Fact]
	public void TestLagNegativeOffsetException()
	{
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			Enumerable.Repeat(1, 10).Lag(-10, (val, lagVal) => val));
	}

	/// <summary>
	/// Verify that attempting to lag by a zero offset will result in an exception
	/// </summary>
	[Fact]
	public void TestLagZeroOffset()
	{
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			Enumerable.Range(1, 10).Lag(0, (val, lagVal) => val + lagVal));
	}

	/// <summary>
	/// Verify that lag can accept an propagate a default value passed to it.
	/// </summary>
	[Fact]
	public void TestLagExplicitDefaultValue()
	{
		const int count = 100;
		const int lagBy = 10;
		const int lagDefault = -1;
		var sequence = Enumerable.Range(1, count);
		var result = sequence.Lag(lagBy, lagDefault, (val, lagVal) => lagVal);

		Assert.Equal(count, result.Count());
		Assert.Equal(Enumerable.Repeat(lagDefault, lagBy), result.Take(lagBy));
	}

	/// <summary>
	/// Verify that lag will use default(T) if a specific default value is not supplied for the lag value.
	/// </summary>
	[Fact]
	public void TestLagImplicitDefaultValue()
	{
		const int count = 100;
		const int lagBy = 10;
		var sequence = Enumerable.Range(1, count);
		var result = sequence.Lag(lagBy, (val, lagVal) => lagVal);

		Assert.Equal(count, result.Count());
		Assert.Equal(Enumerable.Repeat(default(int), lagBy), result.Take(lagBy));
	}

	/// <summary>
	/// Verify that if the lag offset is greater than the sequence length lag
	/// still yields all of the elements of the source sequence.
	/// </summary>
	[Fact]
	public void TestLagOffsetGreaterThanSequenceLength()
	{
		const int count = 100;
		var sequence = Enumerable.Range(1, count);
		var result = sequence.Lag(count + 1, (a, b) => a);

		Assert.Equal(count, result.Count());
		Assert.Equal(sequence, result);
	}

	/// <summary>
	/// Verify that lag actually yields the correct pair of values from the sequence
	/// when offsetting by a single item.
	/// </summary>
	[Fact]
	public void TestLagPassesCorrectLagValueOffsetBy1()
	{
		const int count = 100;
		var sequence = Enumerable.Range(1, count);
		var result = sequence.Lag(1, (a, b) => new { A = a, B = b });

		Assert.Equal(count, result.Count());
		Assert.True(result.All(x => x.B == (x.A - 1)));
	}

	/// <summary>
	/// Verify that lag yields the correct pair of values from the sequence when
	/// offsetting by more than a single item.
	/// </summary>
	[Fact]
	public void TestLagPassesCorrectLagValuesOffsetBy2()
	{
		const int count = 100;
		var sequence = Enumerable.Range(1, count);
		var result = sequence.Lag(2, (a, b) => new { A = a, B = b });

		Assert.Equal(count, result.Count());
		Assert.True(result.Skip(2).All(x => x.B == (x.A - 2)));
		Assert.True(result.Take(2).All(x => (x.A - x.B) == x.A));
	}

	[Fact]
	public void TestLagWithNullableReferences()
	{
		var words = new[] { "foo", "bar", "baz", "qux" };
		var result = words.Lag(2, (a, b) => new { A = a, B = b });
		result.AssertSequenceEqual(
			new { A = "foo", B = (string?)null },
			new { A = "bar", B = (string?)null },
			new { A = "baz", B = (string?)"foo" },
			new { A = "qux", B = (string?)"bar" });
	}

	[Fact]
	public void TestLagWithNonNullableReferences()
	{
		var words = new[] { "foo", "bar", "baz", "qux" };
		var empty = string.Empty;
		var result = words.Lag(2, empty, (a, b) => new { A = a, B = b });
		result.AssertSequenceEqual(
			new { A = "foo", B = empty },
			new { A = "bar", B = empty },
			new { A = "baz", B = "foo" },
			new { A = "qux", B = "bar" });
	}
}
