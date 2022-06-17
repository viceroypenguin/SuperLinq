namespace Test;

public class MoveTest
{
	[Fact]
	public void MoveWithNegativeFromIndex()
	{
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			new[] { 1 }.Move(-1, 0, 0));
	}

	[Fact]
	public void MoveWithNegativeCount()
	{
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			new[] { 1 }.Move(0, -1, 0));
	}

	[Fact]
	public void MoveWithNegativeToIndex()
	{
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			new[] { 1 }.Move(0, 0, -1));
	}

	[Fact]
	public void MoveIsLazy()
	{
		new BreakingSequence<int>().Move(0, 0, 0);
	}

	[Theory, MemberData(nameof(MoveSource))]
	public void Move(int length, int fromIndex, int count, int toIndex)
	{
		var source = Enumerable.Range(0, length);

		using var test = source.AsTestingSequence();

		var result = test.Move(fromIndex, count, toIndex);

		var slice = source.Slice(fromIndex, count);
		var exclude = source.Exclude(fromIndex, count);
		var expectations = exclude.Take(toIndex).Concat(slice).Concat(exclude.Skip(toIndex));
		Assert.Equal(expectations, result);
	}

	public static IEnumerable<object[]> MoveSource()
	{
		const int length = 10;
		return
			from index in Enumerable.Range(0, length)
			from count in Enumerable.Range(0, length + 1)
			from tcd in new[]
			{
				new object[] { length, index, count, Math.Max(0, index - 1), },
				new object[] { length, index, count, index + 1, },
			}
			select tcd;
	}

	[Theory, MemberData(nameof(MoveWithSequenceShorterThanToIndexSource))]
	public void MoveWithSequenceShorterThanToIndex(int length, int fromIndex, int count, int toIndex)
	{
		var source = Enumerable.Range(0, length);

		using var test = source.AsTestingSequence();

		var result = test.Move(fromIndex, count, toIndex);

		var expectations = source.Exclude(fromIndex, count).Concat(source.Slice(fromIndex, count));
		Assert.Equal(expectations, result);
	}

	public static IEnumerable<object[]> MoveWithSequenceShorterThanToIndexSource() =>
		Enumerable.Range(10, 10 + 5)
				  .Select(toIndex => new object[] { 10, 5, 2, toIndex, });

	[Fact]
	public void MoveIsRepeatable()
	{
		var source = Enumerable.Range(0, 10);
		var result = source.Move(0, 5, 10);

		Assert.Equal(result, result.ToArray());
	}

	[Fact]
	public void MoveWithFromIndexEqualsToIndex()
	{
		var source = Enumerable.Range(0, 10);
		var result = source.Move(5, 999, 5);

		Assert.Equal(result, source);
	}

	[Fact]
	public void MoveWithCountEqualsZero()
	{
		var source = Enumerable.Range(0, 10);
		var result = source.Move(5, 0, 999);

		Assert.Equal(source, result);
	}
}
