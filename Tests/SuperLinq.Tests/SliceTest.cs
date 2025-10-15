namespace SuperLinq.Tests;

/// <summary>
/// Verify the behavior of the Slice operator
/// </summary>
public sealed class SliceTests
{
	/// <summary>
	/// Verify that Slice evaluates in a lazy manner.
	/// </summary>
	[Fact]
	public void TestSliceIsLazy()
	{
		_ = new BreakingSequence<int>().Slice(10, 10);
	}

	public static IEnumerable<object[]> GetSequences() =>
		Enumerable.Range(1, 5)
			.GetCollectionSequences()
			.Select(x => new object[] { x });

	[Theory]
	[MemberData(nameof(GetSequences))]
	public void TestSliceIdentity(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.Slice(0, 5);
			result.AssertSequenceEqual(1, 2, 3, 4, 5);
		}
	}

	[Theory]
	[MemberData(nameof(GetSequences))]
	public void TestSliceFirstItem(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.Slice(0, 1);
			result.AssertSequenceEqual(1);
		}
	}

	[Theory]
	[MemberData(nameof(GetSequences))]
	public void TestSliceLastItem(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.Slice(4, 1);
			result.AssertSequenceEqual(5);
		}
	}

	[Theory]
	[MemberData(nameof(GetSequences))]
	public void TestSliceSmallerThanSequence(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.Slice(1, 2);
			result.AssertSequenceEqual(2, 3);
		}
	}

	[Theory]
	[MemberData(nameof(GetSequences))]
	public void TestSliceLongerThanSequence(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.Slice(3, 5);
			result.AssertSequenceEqual(4, 5);
		}
	}
}
