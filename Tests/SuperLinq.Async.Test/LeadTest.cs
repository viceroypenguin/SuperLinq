﻿namespace Test.Async;

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
		new AsyncBreakingSequence<int>().Lead(5, BreakingFunc.Of<int, int, int>());
		new AsyncBreakingSequence<int>().Lead(5, -1, BreakingFunc.Of<int, int, int>());
	}

	/// <summary>
	/// Verify that attempting to lead by a negative offset will result in an exception.
	/// </summary>
	[Fact]
	public void TestLeadNegativeOffset()
	{
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			AsyncEnumerable.Range(1, 100).Lead(-5, (val, leadVal) => val + leadVal));
	}

	/// <summary>
	/// Verify that attempting to lead by a zero offset will result in an exception.
	/// </summary>
	[Fact]
	public void TestLeadZeroOffset()
	{
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			AsyncEnumerable.Range(1, 100).Lead(0, (val, leadVal) => val + leadVal));
	}

	/// <summary>
	/// Verify that lead can accept and propagate a default value passed to it.
	/// </summary>
	[Fact]
	public async Task TestLeadExplicitDefaultValue()
	{
		const int count = 100;
		const int leadBy = 10;
		const int leadDefault = -1;
		var sequence = Enumerable.Range(1, count);
		var result = sequence.ToAsyncEnumerable().Lead(leadBy, leadDefault, (val, leadVal) => leadVal);

		Assert.Equal(count, await result.CountAsync());
		await result.Skip(count - leadBy).AssertSequenceEqual(Enumerable.Repeat(leadDefault, leadBy));
	}

	[Fact]
	public async Task TestLeadTuple()
	{
		const int Count = 100;
		const int LeadBy = 10;
		var sequence = Enumerable.Range(1, Count);
		var result = sequence.ToAsyncEnumerable().Lead(LeadBy);

		Assert.Equal(Count, await result.CountAsync());
		await result.AssertSequenceEqual(
			sequence.Select(x => (x, x <= Count - LeadBy ? x + LeadBy : default)));
	}

	/// <summary>
	/// Verify that Lead() will use default(T) if a specific default value is not supplied for the lead value.
	/// </summary>
	[Fact]
	public async Task TestLeadImplicitDefaultValue()
	{
		const int count = 100;
		const int leadBy = 10;
		var sequence = Enumerable.Range(1, count);
		var result = sequence.ToAsyncEnumerable().Lead(leadBy, (val, leadVal) => leadVal);

		Assert.Equal(count, await result.CountAsync());
		await result.Skip(count - leadBy).AssertSequenceEqual(Enumerable.Repeat(default(int), leadBy));
	}

	/// <summary>
	/// Verify that if the lead offset is greater than the length of the sequence
	/// Lead() still yield all of the elements of the source sequence.
	/// </summary>
	[Fact]
	public async Task TestLeadOffsetGreaterThanSequenceLength()
	{
		const int count = 100;
		const int leadDefault = -1;
		var sequence = Enumerable.Range(1, count);
		var result = sequence.ToAsyncEnumerable().Lead(count + 1, leadDefault, (val, leadVal) => new { A = val, B = leadVal });

		Assert.Equal(count, await result.CountAsync());
		await result.AssertSequenceEqual(sequence.Select(x => new { A = x, B = leadDefault }));
	}

	/// <summary>
	/// Verify that Lead() actually yields the correct pair of values from the sequence
	/// when the lead offset is 1.
	/// </summary>
	[Fact]
	public async Task TestLeadPassesCorrectValueOffsetBy1()
	{
		const int count = 100;
		var sequence = Enumerable.Range(1, count);
		var result = sequence.ToAsyncEnumerable().Lead(1, count + 1, (val, leadVal) => new { A = val, B = leadVal });

		Assert.Equal(count, await result.CountAsync());
		Assert.True(await result.AllAsync(x => x.B == (x.A + 1)));
	}

	/// <summary>
	/// Verify that Lead() yields the correct pair of values from the sequence
	/// when the lead offset is greater than 1.
	/// </summary>
	[Fact]
	public async Task TestLeadPassesCorrectValueOffsetBy2()
	{
		const int count = 100;
		const int leadDefault = count + 1;
		var sequence = Enumerable.Range(1, count);
		var result = sequence.ToAsyncEnumerable().Lead(2, leadDefault, (val, leadVal) => new { A = val, B = leadVal });

		Assert.Equal(count, await result.CountAsync());
		Assert.True(await result.Take(count - 2).AllAsync(x => x.B == (x.A + 2)));
		Assert.True(await result.Skip(count - 2).AllAsync(x => x.B == leadDefault && (x.A == count || x.A == count - 1)));
	}

	[Fact]
	public Task TestLagWithNullableReferences()
	{
		var words = AsyncSeq("foo", "bar", "baz", "qux");
		var result = words.Lead(2, (a, b) => new { A = a, B = b });
		return result.AssertSequenceEqual(
			new { A = "foo", B = (string?)"baz" },
			new { A = "bar", B = (string?)"qux" },
			new { A = "baz", B = (string?)null },
			new { A = "qux", B = (string?)null });
	}

	[Fact]
	public Task TestLagWithNonNullableReferences()
	{
		var words = AsyncSeq("foo", "bar", "baz", "qux");
		var empty = string.Empty;
		var result = words.Lead(2, empty, (a, b) => new { A = a, B = b });
		return result.AssertSequenceEqual(
			new { A = "foo", B = "baz" },
			new { A = "bar", B = "qux" },
			new { A = "baz", B = empty },
			new { A = "qux", B = empty });
	}
}