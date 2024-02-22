﻿namespace Test;

public class MoveTest
{
	[Fact]
	public void MoveWithNegativeFromIndex()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() => new[] { 1 }.Move(-1, 0, 0));
	}

	[Fact]
	public void MoveRangeWithNegativeStartIndex()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() => new[] { 1 }.Move(-1..-1, 0));
	}

	[Fact]
	public void MoveWithNegativeCount()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() => new[] { 1 }.Move(0, -1, 0));
	}

	[Fact]
	public void MoveRangeWithDecendingRange()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() => new[] { 1 }.Move(0..-1, 0));
	}

	[Fact]
	public void MoveWithNegativeToIndex()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() => new[] { 1 }.Move(0, 0, -1));
	}

	[Fact]
	public void MoveRangeWithNegativeToIndex()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() => new[] { 1 }.Move(0..0, -1));
	}

	[Fact]
	public void MoveIsLazy()
	{
		_ = new BreakingSequence<int>().Move(0, 0, 0);
	}

	[Theory, MemberData(nameof(MoveSource))]
	public void Move(int length, int fromIndex, int count, int toIndex)
	{
		var source = Enumerable.Range(0, length);

		using var test = source.AsTestingSequence();

		var result = test.Move(fromIndex, count, toIndex);

		var slice = source.Take(fromIndex..(fromIndex + count));
		var exclude = source.Exclude(fromIndex, count);
		var expectations = exclude.Take(toIndex).Concat(slice).Concat(exclude.Skip(toIndex));
		result.AssertSequenceEqual(expectations);
	}

	[Theory, MemberData(nameof(MoveRangeSource))]
	public void MoveRange(int length, Range range, int toIndex)
	{
		var source = Enumerable.Range(0, length);

		using var test = source.AsTestingSequence();

		var result = test.Move(range, toIndex);

		var slice = source.Take(range);
		var exclude = source.Exclude(range.Start.Value, range.End.Value - range.Start.Value);
		var expectations = exclude.Take(toIndex).Concat(slice).Concat(exclude.Skip(toIndex));
		result.AssertSequenceEqual(expectations);
	}

	public static IEnumerable<object[]> MoveSource()
	{
		const int Length = 10;
		return from index in Enumerable.Range(0, Length)
			from count in Enumerable.Range(0, Length + 1)
			from tcd in new object[][]
			{
				[Length, index, count, Math.Max(0, index - 1),],
				[Length, index, count, index + 1,],
			}
			select tcd;
	}

	public static IEnumerable<object[]> MoveRangeSource()
	{
		const int Length = 10;
		return from index in Enumerable.Range(0, Length)
			from count in Enumerable.Range(0, Length + 1)
			from tcd in new object[][]
			{
				[Length, index..(index + count), Math.Max(0, index - 1),],
				[Length, index..(index + count), index + 1,],
			}
			select tcd;
	}

	[Theory, MemberData(nameof(MoveWithSequenceShorterThanToIndexSource))]
	public void MoveWithSequenceShorterThanToIndex(
		int length,
		int fromIndex,
		int count,
		int toIndex
	)
	{
		var source = Enumerable.Range(0, length);

		using var test = source.AsTestingSequence();

		var result = test.Move(fromIndex, count, toIndex);

		var expectations = source
			.Exclude(fromIndex, count)
			.Concat(source.Take(fromIndex..(fromIndex + count)));
		Assert.Equal(expectations, result);
	}

	[Theory, MemberData(nameof(MoveRangeWithSequenceShorterThanToIndexSource))]
	public void MoveRangeWithSequenceShorterThanToIndex(int length, Range range, int toIndex)
	{
		var source = Enumerable.Range(0, length);

		using var test = source.AsTestingSequence();

		var result = test.Move(range, toIndex);

		var expectations = source
			.Exclude(range.Start.Value, range.End.Value - range.Start.Value)
			.Concat(source.Take(range));
		Assert.Equal(expectations, result);
	}

	public static IEnumerable<object[]> MoveWithSequenceShorterThanToIndexSource() =>
		Enumerable.Range(10, 10 + 5).Select(toIndex => new object[] { 10, 5, 2, toIndex, });

	public static IEnumerable<object[]> MoveRangeWithSequenceShorterThanToIndexSource() =>
		Enumerable.Range(10, 10 + 5).Select(toIndex => new object[] { 10, 5..7, toIndex, });

	[Fact]
	public void MoveIsRepeatable()
	{
		using var source = Enumerable.Range(0, 10).AsTestingSequence(maxEnumerations: 2);

		var result = source.Move(0, 5, 10);
		Assert.Equal(result, result.ToArray());
	}

	[Fact]
	public void MoveRangeIsRepeatable()
	{
		using var source = Enumerable.Range(0, 10).AsTestingSequence(maxEnumerations: 2);

		var result = source.Move(0..5, 10);
		Assert.Equal(result, result.ToArray());
	}

	[Fact]
	public void MoveWithFromIndexEqualsToIndex()
	{
		using var source = Enumerable.Range(0, 10).AsTestingSequence();

		var result = source.Move(5, 999, 5);
		result.AssertSequenceEqual(Enumerable.Range(0, 10));
	}

	[Fact]
	public void MoveRangeWithFomrIndexEqualsToIndex()
	{
		using var source = Enumerable.Range(0, 10).AsTestingSequence();

		var result = source.Move(5..1004, 5);
		result.AssertSequenceEqual(Enumerable.Range(0, 10));
	}

	[Fact]
	public void MoveWithCountEqualsZero()
	{
		using var source = Enumerable.Range(0, 10).AsTestingSequence();

		var result = source.Move(5, 0, 999);
		result.AssertSequenceEqual(Enumerable.Range(0, 10));
	}

	[Fact]
	public void MoveRngeWithCountEqualsZero()
	{
		using var source = Enumerable.Range(0, 10).AsTestingSequence();

		var result = source.Move(5..5, 999);
		result.AssertSequenceEqual(Enumerable.Range(0, 10));
	}

	[Fact]
	public void MoveRangeFromEndIndex_Forward()
	{
		using var source = Enumerable.Range(0, 8).AsTestingSequence();
		var result = source.Move(1..4, ^3);
		result.AssertSequenceEqual([0, 4, 1, 2, 3, 5, 6, 7]);
	}

	[Fact]
	public void MoveRangeFromEndIndex_Backward()
	{
		using var source = Enumerable.Range(0, 10).AsTestingSequence();
		var result = source.Move(3..4, ^9);
		result.AssertSequenceEqual([0, 3, 1, 2, 4, 5, 6, 7, 8, 9]);
	}

	[Fact]
	public void MoveRangeWithRangeEndFromEnd_Forward()
	{
		using var source = Enumerable.Range(0, 8).AsTestingSequence();
		var result = source.Move(1..^4, 2);
		result.AssertSequenceEqual([0, 4, 1, 2, 3, 5, 6, 7]);
	}

	[Fact]
	public void MoveRangeWithRangeEndFromEnd_Backward()
	{
		using var source = Enumerable.Range(0, 8).AsTestingSequence();
		var result = source.Move(1..^4, 0);
		result.AssertSequenceEqual([1, 2, 3, 0, 4, 5, 6, 7]);
	}
}
