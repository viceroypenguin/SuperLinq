﻿namespace Test;

/// <summary>
/// Verify the behavior of the Exclude operator
/// </summary>
public class ExcludeTests
{
	[Fact]
	public void TestExcludeIsLazy()
	{
		_ = new BreakingSequence<int>().Exclude(0, 10);
	}

	[Fact]
	public void TestExcludeNegativeStartIndexException()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new BreakingSequence<int>().Exclude(-10, 10));
	}

	[Fact]
	public void TestExcludeNegativeCountException()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new BreakingSequence<int>().Exclude(0, -5));
	}

	[Fact]
	public void TestExcludeWithCountEqualsZero()
	{
		using var sequence = Enumerable.Range(1, 10).AsTestingSequence();

		var result = sequence.Exclude(5, 0);
		result.AssertSequenceEqual(Enumerable.Range(1, 10));
	}

	public static IEnumerable<object[]> GetIntSequences() =>
		Enumerable.Range(1, 100)
			.GetListSequences()
			.Select(x => new object[] { x });

	[Theory]
	[MemberData(nameof(GetIntSequences))]
	public void TestExcludeSequenceHead(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.Exclude(0, 5);
			result.AssertSequenceEqual(Enumerable.Range(6, 95));
		}
	}

	[Theory]
	[MemberData(nameof(GetIntSequences))]
	public void TestExcludeSequenceTail(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.Exclude(95, 10);
			result.AssertSequenceEqual(Enumerable.Range(1, 95));
		}
	}

	[Theory]
	[MemberData(nameof(GetIntSequences))]
	public void TestExcludeSequenceMiddle(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.Exclude(30, 50);
			result.AssertSequenceEqual(
				Enumerable.Range(1, 30)
					.Concat(Enumerable.Range(81, 20)));
		}
	}

	[Theory]
	[MemberData(nameof(GetIntSequences))]
	public void TestExcludeEntireSequence(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.Exclude(0, 101);
			result.AssertSequenceEqual();
		}
	}

	[Theory]
	[MemberData(nameof(GetIntSequences))]
	public void TestExcludeCountGreaterThanSequenceLength(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.Exclude(1, 10 * 10);
			result.AssertSequenceEqual(1);
		}
	}

	[Theory]
	[MemberData(nameof(GetIntSequences))]
	public void TestExcludeStartIndexGreaterThanSequenceLength(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.Exclude(101, 10);
			result.AssertSequenceEqual(
				Enumerable.Range(1, 100));
		}
	}

	[Fact]
	public void ExcludeListBehaviorMid()
	{
		using var seq = Enumerable.Range(0, 10_000).AsBreakingList();

		var result = seq.Exclude(1_001, 1_000);
		Assert.Equal(9_000, result.Count());
		Assert.Equal(10, result.ElementAt(10));
		Assert.Equal(1_000, result.ElementAt(1_000));
		Assert.Equal(2_001, result.ElementAt(1_001));
		Assert.Equal(9_950, result.ElementAt(^50));
		Assert.Equal(9_950, result.ElementAt(8_950));
	}

	[Fact]
	public void ExcludeListBehaviorEnd()
	{
		using var seq = Enumerable.Range(0, 10_000).AsBreakingList();

		var result = seq.Exclude(9_500, 1_000);
		Assert.Equal(9_500, result.Count());
		Assert.Equal(10, result.ElementAt(10));
		Assert.Equal(9_450, result.ElementAt(^50));
	}

	[Fact]
	public void ExcludeListBehaviorAfter()
	{
		using var seq = Enumerable.Range(0, 10_000).AsBreakingList();

		var result = seq.Exclude(15_000, 1_000);
		Assert.Equal(10_000, result.Count());
		Assert.Equal(10, result.ElementAt(10));
		Assert.Equal(9_950, result.ElementAt(^50));
	}

	[Fact]
	public void ExcludeListBehaviorEntire()
	{
		using var seq = Enumerable.Range(0, 10_000).AsBreakingList();

		var result = seq.Exclude(0, 10_000);

#pragma warning disable xUnit2013 // Do not use equality check to check for collection size.
		Assert.Equal(0, result.Count());
#pragma warning restore xUnit2013 // Do not use equality check to check for collection size.

		_ = Assert.Throws<ArgumentOutOfRangeException>("index", () => result.ElementAt(0));
	}
}
