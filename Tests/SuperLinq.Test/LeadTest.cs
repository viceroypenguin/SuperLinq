namespace Test;

/// <summary>
/// Verify the behavior of the Lead operator.
/// </summary>
public sealed class LeadTest
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

	public static IEnumerable<object[]> GetIntSequences() =>
		Enumerable.Range(1, 100)
			.GetListSequences()
			.Select(x => new object[] { x });

	[Theory]
	[MemberData(nameof(GetIntSequences))]
	public void TestLeadExplicitDefaultValue(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.Lead(10, -1, (val, leadVal) => leadVal);
			result.AssertSequenceEqual(
				Enumerable.Range(11, 90)
					.Concat(Enumerable.Repeat(-1, 10)));
		}
	}

	[Theory]
	[MemberData(nameof(GetIntSequences))]
	public void TestLeadTuple(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.Lead(10);
			result.AssertSequenceEqual(
				Enumerable.Range(1, 100)
					.Select(x => (x, x <= 100 - 10 ? x + 10 : default)));
		}
	}

	[Theory]
	[MemberData(nameof(GetIntSequences))]
	public void TestLeadImplicitDefaultValue(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.Lead(10, (val, leadVal) => leadVal);
			result.AssertSequenceEqual(
				Enumerable.Range(11, 90)
					.Concat(Enumerable.Repeat(0, 10)));
		}
	}

	[Theory]
	[MemberData(nameof(GetIntSequences))]
	public void TestLeadOffsetGreaterThanSequenceLength(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq
				.Lead(101, -1, ValueTuple.Create);
			result.AssertSequenceEqual(
				Enumerable.Range(1, 100).Select(x => (x, -1)));
		}
	}

	[Theory]
	[MemberData(nameof(GetIntSequences))]
	public void TestLeadPassesCorrectValueOffsetBy1(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.Lead(1);
			result.AssertSequenceEqual(
				Enumerable.Range(1, 100)
					.Select(x => (x, x == 100 ? 0 : x + 1)));
		}
	}

	[Theory]
	[MemberData(nameof(GetIntSequences))]
	public void TestLeadPassesCorrectValueOffsetBy2(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.Lead(2);
			result.AssertSequenceEqual(
				Enumerable.Range(1, 100)
					.Select(x => (x, x >= 99 ? 0 : x + 2)));
		}
	}

	public static IEnumerable<object[]> GetStringSequences() =>
		Seq("foo", "bar", "baz", "qux")
			.GetListSequences()
			.Select(x => new object[] { x });

	[Theory]
	[MemberData(nameof(GetStringSequences))]
	public void TestLagWithNullableReferences(IDisposableEnumerable<string> seq)
	{
		using (seq)
		{
			var result = seq.Lead(2, (a, b) => new { A = a, B = b });
			result.AssertSequenceEqual(
				new { A = "foo", B = (string?)"baz" },
				new { A = "bar", B = (string?)"qux" },
				new { A = "baz", B = (string?)null },
				new { A = "qux", B = (string?)null });
		}
	}

	[Theory]
	[MemberData(nameof(GetStringSequences))]
	public void TestLagWithNonNullableReferences(IDisposableEnumerable<string> seq)
	{
		using (seq)
		{
			var result = seq.Lead(2, "", (a, b) => new { A = a, B = b });
			result.AssertSequenceEqual(
				new { A = "foo", B = "baz" },
				new { A = "bar", B = "qux" },
				new { A = "baz", B = "" },
				new { A = "qux", B = "" });
		}
	}

	[Fact]
	public void LeadListBehavior()
	{
		using var seq = Enumerable.Range(0, 10_000).AsBreakingList();

		var result = seq.Lead(20);
		result.AssertCollectionErrorChecking(10_000);
		result.AssertListElementChecking(10_000);

		Assert.Equal((50, 70), result.ElementAt(50));
		Assert.Equal((9_950, 9_970), result.ElementAt(^50));
		Assert.Equal((9_990, 0), result.ElementAt(^10));
	}
}
