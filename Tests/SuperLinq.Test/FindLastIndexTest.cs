namespace Test;

public class FindLastIndexTest
{
	[Fact]
	public void FindLastIndexWithNegativeCount()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new BreakingSequence<int>().FindLastIndex(i => i == 1, 1, -1));
	}

	[Fact]
	public void FindLastIndexWorksWithEmptySequence()
	{
		using var sequence = Array.Empty<int>().AsTestingSequence();
		Assert.Equal(-1, sequence.FindLastIndex(i => i == 5));
	}

	[Fact]
	public void FindLastIndexFromStart()
	{
		using var sequence = Enumerable.Range(100, 5)
			.Concat(Enumerable.Range(100, 5))
			.AsTestingSequence();
		Assert.Equal(
			7,
			sequence.FindLastIndex(i => i == 102));
	}

	[Fact]
	public void FindLastIndexFromStartCount()
	{
		using var sequence = Enumerable.Range(100, 5)
			.Concat(Enumerable.Range(100, 5))
			.AsTestingSequence();
		Assert.Equal(
			7,
			sequence.FindLastIndex(i => i == 102, int.MaxValue, 8));
	}

	[Fact]
	public void FindLastIndexFromStartIndex()
	{
		using var sequence = Enumerable.Range(100, 5)
			.Concat(Enumerable.Range(100, 5))
			.AsTestingSequence();
		Assert.Equal(
			7,
			sequence.FindLastIndex(i => i == 102, 8));
	}

	[Fact]
	public void FindLastIndexFromEndIndex()
	{
		using var sequence = Enumerable.Range(100, 5)
			.Concat(Enumerable.Range(100, 5))
			.AsTestingSequence();
		Assert.Equal(
			7,
			sequence.FindLastIndex(i => i == 102, ^3));
	}

	[Fact]
	public void FindLastIndexUsesCollectionLengthCorrectly()
	{
		var array = new int[20];
		array[^7] = 3;
		array[^6] = 3;
		Assert.Equal(
			(^6).GetOffset(20),
			array.FindLastIndex(i => i == 3, ^5));
	}

	[Fact]
	public void FindLastIndexMissingValueFromStart()
	{
		using var sequence = Enumerable.Range(100, 5)
			.AsTestingSequence();
		Assert.Equal(
			-1,
			sequence.FindLastIndex(i => i == 95));
	}

	[Fact]
	public void FindLastIndexMissingValueFromEnd()
	{
		using var sequence = Enumerable.Range(100, 5)
			.AsTestingSequence();
		Assert.Equal(
			-1,
			sequence.FindLastIndex(i => i == 95, ^5));
	}

	[Fact]
	public void FindLastIndexMissingValueFromStartCount()
	{
		using var sequence = Enumerable.Range(100, 5)
			.AsTestingSequence();
		Assert.Equal(
			-1,
			sequence.FindLastIndex(i => i == 100, int.MaxValue, 4));
	}

	[Fact]
	public void FindLastIndexMissingValueFromEndCount()
	{
		using var sequence = Enumerable.Range(100, 5)
			.AsTestingSequence();
		Assert.Equal(
			-1,
			sequence.FindLastIndex(i => i == 100, ^1, 3));
	}
}
