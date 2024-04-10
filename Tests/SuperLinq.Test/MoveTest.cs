namespace Test;

public sealed class MoveTest
{
	[Fact]
	public void MoveWithNegativeFromIndex()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new[] { 1 }.Move(-1, 0, 0));
	}

	[Fact]
	public void MoveWithNegativeCount()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new[] { 1 }.Move(0, -1, 0));
	}

	[Fact]
	public void MoveWithNegativeToIndex()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new[] { 1 }.Move(0, 0, -1));
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

	public static IEnumerable<object[]> MoveSource()
	{
		const int Length = 10;
		return
			from index in Enumerable.Range(0, Length)
			from count in Enumerable.Range(0, Length + 1)
			from tcd in new object[][]
			{
				[Length, index, count, Math.Max(0, index - 1),],
				[Length, index, count, index + 1,],
			}
			select tcd;
	}

	[Theory, MemberData(nameof(MoveWithSequenceShorterThanToIndexSource))]
	public void MoveWithSequenceShorterThanToIndex(int length, int fromIndex, int count, int toIndex)
	{
		var source = Enumerable.Range(0, length);

		using var test = source.AsTestingSequence();

		var result = test.Move(fromIndex, count, toIndex);

		var expectations = source.Exclude(fromIndex, count).Concat(source.Take(fromIndex..(fromIndex + count)));
		Assert.Equal(expectations, result);
	}

	public static IEnumerable<object[]> MoveWithSequenceShorterThanToIndexSource() =>
		Enumerable.Range(10, 10 + 5)
				  .Select(toIndex => new object[] { 10, 5, 2, toIndex, });

	[Fact]
	public void MoveIsRepeatable()
	{
		using var source = Enumerable.Range(0, 10).AsTestingSequence(maxEnumerations: 2);

		var result = source.Move(0, 5, 10);
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
	public void MoveWithCountEqualsZero()
	{
		using var source = Enumerable.Range(0, 10).AsTestingSequence();

		var result = source.Move(5, 0, 999);
		result.AssertSequenceEqual(Enumerable.Range(0, 10));
	}
}
