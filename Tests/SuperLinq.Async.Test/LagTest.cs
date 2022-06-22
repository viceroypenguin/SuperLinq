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
			AsyncEnumerable.Repeat(1, 10).Lag(-10, (val, lagVal) => val));
	}

	/// <summary>
	/// Verify that attempting to lag by a zero offset will result in an exception
	/// </summary>
	[Fact]
	public void TestLagZeroOffset()
	{
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			AsyncEnumerable.Range(1, 10).Lag(0, (val, lagVal) => val + lagVal));
	}

	/// <summary>
	/// Verify that lag can accept an propagate a default value passed to it.
	/// </summary>
	[Fact]
	public async Task TestLagExplicitDefaultValue()
	{
		const int count = 100;
		const int lagBy = 10;
		const int lagDefault = -1;
		var sequence = AsyncEnumerable.Range(1, count);
		var result = sequence.Lag(lagBy, lagDefault, (val, lagVal) => lagVal);

		Assert.Equal(count, await result.CountAsync());
		await result.Take(lagBy).AssertSequenceEqual(Enumerable.Repeat(lagDefault, lagBy));
	}

	[Fact]
	public async Task TestLagTuple()
	{
		const int Count = 100;
		const int LagBy = 10;
		var sequence = Enumerable.Range(1, Count);
		var result = sequence.ToAsyncEnumerable().Lag(LagBy);

		Assert.Equal(Count, await result.CountAsync());
		await result.AssertSequenceEqual(
			sequence.Select(x => (x, x <= LagBy ? default : x - LagBy)));
	}

	/// <summary>
	/// Verify that lag will use default(T) if a specific default value is not supplied for the lag value.
	/// </summary>
	[Fact]
	public async Task TestLagImplicitDefaultValue()
	{
		const int count = 100;
		const int lagBy = 10;
		var sequence = Enumerable.Range(1, count);
		var result = sequence.ToAsyncEnumerable().Lag(lagBy, (val, lagVal) => lagVal);

		Assert.Equal(count, await result.CountAsync());
		await result.Take(lagBy).AssertSequenceEqual(Enumerable.Repeat(default(int), lagBy));
	}

	/// <summary>
	/// Verify that if the lag offset is greater than the sequence length lag
	/// still yields all of the elements of the source sequence.
	/// </summary>
	[Fact]
	public async Task TestLagOffsetGreaterThanSequenceLength()
	{
		const int count = 100;
		var sequence = Enumerable.Range(1, count);
		var result = sequence.ToAsyncEnumerable().Lag(count + 1, (a, b) => a);

		Assert.Equal(count, await result.CountAsync());
		await result.AssertSequenceEqual(sequence);
	}

	/// <summary>
	/// Verify that lag actually yields the correct pair of values from the sequence
	/// when offsetting by a single item.
	/// </summary>
	[Fact]
	public async Task TestLagPassesCorrectLagValueOffsetBy1()
	{
		const int count = 100;
		var sequence = Enumerable.Range(1, count);
		var result = sequence.ToAsyncEnumerable().Lag(1, (a, b) => new { A = a, B = b });

		Assert.Equal(count, await result.CountAsync());
		Assert.True(await result.AllAsync(x => x.B == (x.A - 1)));
	}

	/// <summary>
	/// Verify that lag yields the correct pair of values from the sequence when
	/// offsetting by more than a single item.
	/// </summary>
	[Fact]
	public async Task TestLagPassesCorrectLagValuesOffsetBy2()
	{
		const int count = 100;
		var sequence = Enumerable.Range(1, count);
		var result = sequence.ToAsyncEnumerable().Lag(2, (a, b) => new { A = a, B = b });

		Assert.Equal(count, await result.CountAsync());
		Assert.True(await result.Skip(2).AllAsync(x => x.B == (x.A - 2)));
		Assert.True(await result.Take(2).AllAsync(x => (x.A - x.B) == x.A));
	}

	[Fact]
	public Task TestLagWithNullableReferences()
	{
		var words = AsyncSeq("foo", "bar", "baz", "qux");
		var result = words.Lag(2, (a, b) => new { A = a, B = b });
		return result.AssertSequenceEqual(
			new { A = "foo", B = (string?)null },
			new { A = "bar", B = (string?)null },
			new { A = "baz", B = (string?)"foo" },
			new { A = "qux", B = (string?)"bar" });
	}

	[Fact]
	public Task TestLagWithNonNullableReferences()
	{
		var words = AsyncSeq("foo", "bar", "baz", "qux");
		var empty = string.Empty;
		var result = words.Lag(2, empty, (a, b) => new { A = a, B = b });
		return result.AssertSequenceEqual(
			new { A = "foo", B = empty },
			new { A = "bar", B = empty },
			new { A = "baz", B = "foo" },
			new { A = "qux", B = "bar" });
	}
}
