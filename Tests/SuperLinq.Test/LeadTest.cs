namespace Test;

/// <summary>
/// Verify the behavior of the Lead operator.
/// </summary>
public class LeadTests
{
	/// <summary>
	/// Verify that Lead() behaves in a lazy manner.
	/// </summary>
	[Fact]
	public void TestLeadIsLazy()
	{
		_ = new BreakingSequence<int>().Lead(5, BreakingFunc.Of<int, int, int>());
		_ = new BreakingSequence<int>().Lead(5, -1, BreakingFunc.Of<int, int, int>());
	}

	/// <summary>
	/// Verify that attempting to lead by a negative offset will result in an exception.
	/// </summary>
	[Fact]
	public void TestLeadNegativeOffset()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new BreakingSequence<int>().Lead(-5, (val, leadVal) => val + leadVal));
	}

	/// <summary>
	/// Verify that attempting to lead by a zero offset will result in an exception.
	/// </summary>
	[Fact]
	public void TestLeadZeroOffset()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new BreakingSequence<int>().Lead(0, (val, leadVal) => val + leadVal));
	}

	/// <summary>
	/// Verify that lead can accept and propagate a default value passed to it.
	/// </summary>
	[Fact]
	public void TestLeadExplicitDefaultValue()
	{
		using var sequence = Enumerable.Range(1, 100).AsTestingSequence();

		var result = sequence.Lead(10, -1, (val, leadVal) => leadVal).ToList();
		Assert.Equal(100, result.Count);
		Assert.Equal(Enumerable.Repeat(-1, 10), result.Skip(100 - 10));
	}

	[Fact]
	public void TestLeadTuple()
	{
		using var sequence = Enumerable.Range(1, 100).AsTestingSequence();

		var result = sequence.Lead(10).ToList();
		Assert.Equal(100, result.Count);
		result.AssertSequenceEqual(
			Enumerable.Range(1, 100).Select(x => (x, x <= 100 - 10 ? x + 10 : default)));
	}

	/// <summary>
	/// Verify that Lead() will use default(T) if a specific default value is not supplied for the lead value.
	/// </summary>
	[Fact]
	public void TestLeadImplicitDefaultValue()
	{
		using var sequence = Enumerable.Range(1, 100).AsTestingSequence();

		var result = sequence.Lead(10, (val, leadVal) => leadVal).ToList();
		Assert.Equal(100, result.Count);
		Assert.Equal(Enumerable.Repeat(default(int), 10), result.Skip(100 - 10));
	}

	/// <summary>
	/// Verify that if the lead offset is greater than the length of the sequence
	/// Lead() still yield all of the elements of the source sequence.
	/// </summary>
	[Fact]
	public void TestLeadOffsetGreaterThanSequenceLength()
	{
		using var sequence = Enumerable.Range(1, 100).AsTestingSequence();

		var result = sequence
			.Lead(101, -1, (val, leadVal) => new { A = val, B = leadVal })
			.ToList();
		Assert.Equal(100, result.Count);
		Assert.Equal(Enumerable.Range(1, 100).Select(x => new { A = x, B = -1 }), result);
	}

	/// <summary>
	/// Verify that Lead() actually yields the correct pair of values from the sequence
	/// when the lead offset is 1.
	/// </summary>
	[Fact]
	public void TestLeadPassesCorrectValueOffsetBy1()
	{
		using var sequence = Enumerable.Range(1, 100).AsTestingSequence();

		var result = sequence
			.Lead(1, 101, (val, leadVal) => new { A = val, B = leadVal })
			.ToList();
		Assert.Equal(100, result.Count);
		Assert.True(result.All(x => x.B == (x.A + 1)));
	}

	/// <summary>
	/// Verify that Lead() yields the correct pair of values from the sequence
	/// when the lead offset is greater than 1.
	/// </summary>
	[Fact]
	public void TestLeadPassesCorrectValueOffsetBy2()
	{
		using var sequence = Enumerable.Range(1, 100).AsTestingSequence();

		var result = sequence
			.Lead(2, 101, (val, leadVal) => new { A = val, B = leadVal })
			.ToList();
		Assert.Equal(100, result.Count);
		Assert.True(result.Take(100 - 2).All(x => x.B == (x.A + 2)));
		Assert.True(result.Skip(100 - 2).All(x => x.B == 101 && (x.A is 99 or 100)));
	}

	[Fact]
	public void TestLagWithNullableReferences()
	{
		using var words = TestingSequence.Of("foo", "bar", "baz", "qux");

		var result = words.Lead(2, (a, b) => new { A = a, B = b });
		result.AssertSequenceEqual(
			new { A = "foo", B = (string?)"baz" },
			new { A = "bar", B = (string?)"qux" },
			new { A = "baz", B = (string?)null },
			new { A = "qux", B = (string?)null });
	}

	[Fact]
	public void TestLagWithNonNullableReferences()
	{
		using var words = TestingSequence.Of("foo", "bar", "baz", "qux");

		var result = words.Lead(2, string.Empty, (a, b) => new { A = a, B = b });
		result.AssertSequenceEqual(
			new { A = "foo", B = "baz" },
			new { A = "bar", B = "qux" },
			new { A = "baz", B = string.Empty },
			new { A = "qux", B = string.Empty });
	}
}
