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
		_ = new BreakingSequence<int>().Lag(5, BreakingFunc.Of<int, int, int>());
		_ = new BreakingSequence<int>().Lag(5, -1, BreakingFunc.Of<int, int, int>());
	}

	/// <summary>
	/// Verify that lagging by a negative offset results in an exception.
	/// </summary>
	[Fact]
	public void TestLagNegativeOffsetException()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new BreakingSequence<int>().Lag(-10, (val, lagVal) => val));
	}

	/// <summary>
	/// Verify that attempting to lag by a zero offset will result in an exception
	/// </summary>
	[Fact]
	public void TestLagZeroOffset()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new BreakingSequence<int>().Lag(0, (val, lagVal) => val + lagVal));
	}

	public static IEnumerable<object[]> GetIntSequences() =>
		Enumerable.Range(1, 100)
			.GetListSequences()
			.Select(x => new object[] { x });

	[Theory]
	[MemberData(nameof(GetIntSequences))]
	public void TestLagExplicitDefaultValue(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.Lag(10, -1, (val, lagVal) => lagVal);
			result.AssertSequenceEqual(
				Enumerable.Repeat(-1, 10).Concat(Enumerable.Range(1, 90)));
		}
	}

	[Theory]
	[MemberData(nameof(GetIntSequences))]
	public void TestLagTuple(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.Lag(10);
			result.AssertSequenceEqual(
				Enumerable.Range(1, 100).Select(x => (x, x <= 10 ? default : x - 10)));
		}
	}

	[Theory]
	[MemberData(nameof(GetIntSequences))]
	public void TestLagImplicitDefaultValue(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.Lag(10, (val, lagVal) => lagVal);
			result.AssertSequenceEqual(
				Enumerable.Repeat(default(int), 10)
					.Concat(Enumerable.Range(1, 90)));
		}
	}

	[Theory]
	[MemberData(nameof(GetIntSequences))]
	public void TestLagOffsetGreaterThanSequenceLength(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.Lag(100 + 1, (a, b) => a);
			result.AssertSequenceEqual(
				Enumerable.Range(1, 100));
		}
	}

	[Theory]
	[MemberData(nameof(GetIntSequences))]
	public void TestLagPassesCorrectLagValueOffsetBy1(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.Lag(1);
			result.AssertSequenceEqual(
				Enumerable.Range(1, 100)
					.Select(x => (x, x - 1)));
		}
	}

	[Theory]
	[MemberData(nameof(GetIntSequences))]
	public void TestLagPassesCorrectLagValuesOffsetBy2(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.Lag(2);
			result.AssertSequenceEqual(
				Enumerable.Range(1, 100)
					.Select(x => (x, x <= 2 ? 0 : x - 2)));
		}
	}

	public static IEnumerable<object[]> GetStringSequences() =>
		Seq("foo", "bar", "baz", "qux")
			.GetListSequences()
			.Select(x => new object[] { x, });

	[Theory]
	[MemberData(nameof(GetStringSequences))]
	public void TestLagWithNullableReferences(IDisposableEnumerable<string> seq)
	{
		using (seq)
		{
			var result = seq.Lag(2, (a, b) => new { A = a, B = b });
			result.AssertSequenceEqual(
				new { A = "foo", B = (string?)null },
				new { A = "bar", B = (string?)null },
				new { A = "baz", B = (string?)"foo" },
				new { A = "qux", B = (string?)"bar" });
		}
	}

	[Theory]
	[MemberData(nameof(GetStringSequences))]
	public void TestLagWithNonNullableReferences(IDisposableEnumerable<string> seq)
	{
		using (seq)
		{
			var result = seq.Lag(2, string.Empty, (a, b) => new { A = a, B = b });
			result.AssertSequenceEqual(
				new { A = "foo", B = string.Empty, },
				new { A = "bar", B = string.Empty, },
				new { A = "baz", B = "foo" },
				new { A = "qux", B = "bar" });
		}
	}
}
