namespace SuperLinq.Tests;

/// <summary>
/// Verify the behavior of the Lag operator
/// </summary>
public sealed class LagTest
{
	/// <summary>
	/// Verify that lag behaves in a lazy manner.
	/// </summary>
	[Test]
	public void TestLagIsLazy()
	{
		_ = new BreakingSequence<int>().Lag(5, BreakingFunc.Of<int, int, int>());
		_ = new BreakingSequence<int>().Lag(5, -1, BreakingFunc.Of<int, int, int>());
	}

	/// <summary>
	/// Verify that lagging by a negative offset results in an exception.
	/// </summary>
	[Test]
	public void TestLagNegativeOffsetException()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new BreakingSequence<int>().Lag(-10, (val, lagVal) => val));
	}

	/// <summary>
	/// Verify that attempting to lag by a zero offset will result in an exception
	/// </summary>
	[Test]
	public void TestLagZeroOffset()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new BreakingSequence<int>().Lag(0, (val, lagVal) => val + lagVal));
	}

	public static IEnumerable<IDisposableEnumerable<int>> GetIntSequences() =>
		Enumerable.Range(1, 100)
			.GetListSequences();

	[Test]
	[MethodDataSource(nameof(GetIntSequences))]
	public void TestLagExplicitDefaultValue(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.Lag(10, -1, (val, lagVal) => lagVal);
			result.AssertSequenceEqual(
				Enumerable.Repeat(-1, 10).Concat(Enumerable.Range(1, 90)));
		}
	}

	[Test]
	[MethodDataSource(nameof(GetIntSequences))]
	public void TestLagTuple(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.Lag(10);
			result.AssertSequenceEqual(
				Enumerable.Range(1, 100).Select(x => (x, x <= 10 ? default : x - 10)));
		}
	}

	[Test]
	[MethodDataSource(nameof(GetIntSequences))]
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

	[Test]
	[MethodDataSource(nameof(GetIntSequences))]
	public void TestLagOffsetGreaterThanSequenceLength(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.Lag(100 + 1, -1, ValueTuple.Create);
			result.AssertSequenceEqual(
				Enumerable.Range(1, 100).Select(x => (x, -1)));
		}
	}

	[Test]
	[MethodDataSource(nameof(GetIntSequences))]
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

	[Test]
	[MethodDataSource(nameof(GetIntSequences))]
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

	public static IEnumerable<IDisposableEnumerable<string>> GetStringSequences() =>
		Seq("foo", "bar", "baz", "qux")
			.GetListSequences();

	[Test]
	[MethodDataSource(nameof(GetStringSequences))]
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

	[Test]
	[MethodDataSource(nameof(GetStringSequences))]
	public void TestLagWithNonNullableReferences(IDisposableEnumerable<string> seq)
	{
		using (seq)
		{
			var result = seq.Lag(2, "", (a, b) => new { A = a, B = b });
			result.AssertSequenceEqual(
				new { A = "foo", B = "" },
				new { A = "bar", B = "" },
				new { A = "baz", B = "foo" },
				new { A = "qux", B = "bar" });
		}
	}

	[Test]
	public void LagListBehavior()
	{
		using var seq = Enumerable.Range(0, 10_000).AsBreakingList();

		var result = seq.Lag(20);
		result.AssertCollectionErrorChecking(10_000);
		result.AssertListElementChecking(10_000);

		Assert.Equal((10, 0), result.ElementAt(10));
		Assert.Equal((50, 30), result.ElementAt(50));
#if !NO_INDEX
		Assert.Equal((9_950, 9_930), result.ElementAt(^50));
#endif
	}
}
