namespace Test.Async;

/// <summary>
/// Verify the behavior of the Lead operator.
/// </summary>
public sealed class LeadTests
{
	/// <summary>
	/// Verify that Lead() behaves in a lazy manner.
	/// </summary>
	[Fact]
	public void TestLeadIsLazy()
	{
		_ = new AsyncBreakingSequence<int>().Lead(5, BreakingFunc.Of<int, int, int>());
		_ = new AsyncBreakingSequence<int>().Lead(5, -1, BreakingFunc.Of<int, int, int>());
	}

	/// <summary>
	/// Verify that attempting to lead by a negative offset will result in an exception.
	/// </summary>
	[Fact]
	public void TestLeadNegativeOffset()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new AsyncBreakingSequence<int>().Lead(-5, (val, leadVal) => val + leadVal));
	}

	/// <summary>
	/// Verify that attempting to lead by a zero offset will result in an exception.
	/// </summary>
	[Fact]
	public void TestLeadZeroOffset()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new AsyncBreakingSequence<int>().Lead(0, (val, leadVal) => val + leadVal));
	}

	/// <summary>
	/// Verify that lead can accept and propagate a default value passed to it.
	/// </summary>
	[Fact]
	public async Task TestLeadExplicitDefaultValue()
	{
		await using var sequence = Enumerable.Range(1, 100).AsTestingSequence();

		var result = await sequence.Lead(10, -1, async (val, leadVal) => { await Task.Yield(); return leadVal; }).ToListAsync();
		Assert.Equal(100, result.Count);
		result.Skip(100 - 10).AssertSequenceEqual(Enumerable.Repeat(-1, 10));
	}

	[Fact]
	public async Task TestLeadTuple()
	{
		await using var sequence = Enumerable.Range(1, 100).AsTestingSequence();

		var result = await sequence.Lead(10).ToListAsync();
		Assert.Equal(100, result.Count);
		result.AssertSequenceEqual(
			Enumerable.Range(1, 100).Select(x => (x, x <= 100 - 10 ? x + 10 : default)));
	}

	/// <summary>
	/// Verify that Lead() will use default(T) if a specific default value is not supplied for the lead value.
	/// </summary>
	[Fact]
	public async Task TestLeadImplicitDefaultValue()
	{
		await using var sequence = Enumerable.Range(1, 100).AsTestingSequence();

		var result = await sequence.Lead(10, (val, leadVal) => leadVal).ToListAsync();
		Assert.Equal(100, result.Count);
		result.Skip(100 - 10).AssertSequenceEqual(Enumerable.Repeat(default(int), 10));
	}

	/// <summary>
	/// Verify that if the lead offset is greater than the length of the sequence
	/// Lead() still yield all of the elements of the source sequence.
	/// </summary>
	[Fact]
	public async Task TestLeadOffsetGreaterThanSequenceLength()
	{
		await using var sequence = Enumerable.Range(1, 100).AsTestingSequence();

		var result = await sequence
			.Lead(101, -1, (val, leadVal) => new { A = val, B = leadVal })
			.ToListAsync();
		Assert.Equal(100, result.Count);
		result.AssertSequenceEqual(Enumerable.Range(1, 100).Select(x => new { A = x, B = -1 }));
	}

	/// <summary>
	/// Verify that Lead() actually yields the correct pair of values from the sequence
	/// when the lead offset is 1.
	/// </summary>
	[Fact]
	public async Task TestLeadPassesCorrectValueOffsetBy1()
	{
		await using var sequence = Enumerable.Range(1, 100).AsTestingSequence();

		var result = await sequence
			.Lead(1, 101, (val, leadVal) => new { A = val, B = leadVal })
			.ToListAsync();
		Assert.Equal(100, result.Count);
		Assert.True(result.All(x => x.B == (x.A + 1)));
	}

	/// <summary>
	/// Verify that Lead() yields the correct pair of values from the sequence
	/// when the lead offset is greater than 1.
	/// </summary>
	[Fact]
	public async Task TestLeadPassesCorrectValueOffsetBy2()
	{
		await using var sequence = Enumerable.Range(1, 100).AsTestingSequence();

		var result = await sequence
			.Lead(2, 101, (val, leadVal) => new { A = val, B = leadVal })
			.ToListAsync();
		Assert.Equal(100, result.Count);
		Assert.True(result.Take(100 - 2).All(x => x.B == (x.A + 2)));
		Assert.True(result.Skip(100 - 2).All(x => x.B == 101 && (x.A is 99 or 100)));
	}

	[Fact]
	public async Task TestLagWithNullableReferences()
	{
		await using var words = TestingSequence.Of("foo", "bar", "baz", "qux");

		var result = words.Lead(2, (a, b) => new { A = a, B = b });
		await result.AssertSequenceEqual(
			new { A = "foo", B = (string?)"baz" },
			new { A = "bar", B = (string?)"qux" },
			new { A = "baz", B = (string?)null },
			new { A = "qux", B = (string?)null });
	}

	[Fact]
	public async Task TestLagWithNonNullableReferences()
	{
		await using var words = TestingSequence.Of("foo", "bar", "baz", "qux");

		var result = words.Lead(2, "", (a, b) => new { A = a, B = b });
		await result.AssertSequenceEqual(
			new { A = "foo", B = "baz" },
			new { A = "bar", B = "qux" },
			new { A = "baz", B = "" },
			new { A = "qux", B = "" });
	}
}
