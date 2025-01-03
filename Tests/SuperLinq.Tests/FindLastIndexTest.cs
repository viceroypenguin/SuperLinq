#if !NO_INDEX

namespace SuperLinq.Tests;

public sealed class FindLastIndexTest
{
	[Test]
	public void FindLastIndexWithNegativeCount()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new BreakingSequence<int>().FindLastIndex(i => i == 1, 1, -1));
	}

	[Test]
	public void FindLastIndexWorksWithEmptySequence()
	{
		using var sequence = Array.Empty<int>().AsTestingSequence();
		Assert.Equal(-1, sequence.FindLastIndex(i => i == 5));
	}

	[Test]
	public void FindLastIndexFromStart()
	{
		using var sequence = Enumerable.Range(100, 5)
			.Concat(Enumerable.Range(100, 5))
			.AsTestingSequence();
		Assert.Equal(
			7,
			sequence.FindLastIndex(i => i == 102));
	}

	[Test]
	public void FindLastIndexFromStartCount()
	{
		using var sequence = Enumerable.Range(100, 5)
			.Concat(Enumerable.Range(100, 5))
			.AsTestingSequence();
		Assert.Equal(
			7,
			sequence.FindLastIndex(i => i == 102, int.MaxValue, 8));
	}

	[Test]
	public void FindLastIndexFromStartIndex()
	{
		using var sequence = Enumerable.Range(100, 5)
			.Concat(Enumerable.Range(100, 5))
			.AsTestingSequence();
		Assert.Equal(
			7,
			sequence.FindLastIndex(i => i == 102, 8));
	}

	[Test]
	public void FindLastIndexFromEndIndex()
	{
		using var sequence = Enumerable.Range(100, 5)
			.Concat(Enumerable.Range(100, 5))
			.AsTestingSequence();
		Assert.Equal(
			7,
			sequence.FindLastIndex(i => i == 102, ^3));
	}

	[Test]
	public void FindLastIndexUsesCollectionLengthCorrectly()
	{
		var array = new int[20];
		array[^7] = 3;
		array[^6] = 3;
		Assert.Equal(
			(^6).GetOffset(20),
			array.FindLastIndex(i => i == 3, ^6));
	}

	[Test]
	public void FindLastIndexMissingValueFromStart()
	{
		using var sequence = Enumerable.Range(100, 5)
			.AsTestingSequence();
		Assert.Equal(
			-1,
			sequence.FindLastIndex(i => i == 95));
	}

	[Test]
	public void FindLastIndexMissingValueFromEnd()
	{
		using var sequence = Enumerable.Range(100, 5)
			.AsTestingSequence();
		Assert.Equal(
			-1,
			sequence.FindLastIndex(i => i == 95, ^5));
	}

	[Test]
	public void FindLastIndexMissingValueFromStartCount()
	{
		using var sequence = Enumerable.Range(100, 5)
			.AsTestingSequence();
		Assert.Equal(
			-1,
			sequence.FindLastIndex(i => i == 100, int.MaxValue, 4));
	}

	[Test]
	public void FindLastIndexMissingValueFromEndCount()
	{
		using var sequence = Enumerable.Range(100, 5)
			.AsTestingSequence();
		Assert.Equal(
			-1,
			sequence.FindLastIndex(i => i == 100, ^1, 3));
	}
}

#endif
