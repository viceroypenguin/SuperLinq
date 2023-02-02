namespace Test;

public class LastIndexOfTest
{
	[Fact]
	public void LastIndexOfWithNegativeCount()
	{
		using var sequence = TestingSequence.Of(1);
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			sequence.LastIndexOf(1, 1, -1));
	}

	[Fact]
	public void LastIndexOfWorksWithEmptySequence()
	{
		using var sequence = Array.Empty<int>().AsTestingSequence();
		Assert.Equal(-1, sequence.LastIndexOf(5));
	}

	[Fact]
	public void LastIndexOfFromStart()
	{
		using var sequence = Enumerable.Range(100, 5)
			.Concat(Enumerable.Range(100, 5))
			.AsTestingSequence();
		Assert.Equal(
			7,
			sequence.LastIndexOf(102));
	}

	[Fact]
	public void LastIndexOfFromStartCount()
	{
		using var sequence = Enumerable.Range(100, 5)
			.Concat(Enumerable.Range(100, 5))
			.AsTestingSequence();
		Assert.Equal(
			7,
			sequence.LastIndexOf(102, int.MaxValue, 8));
	}

	[Fact]
	public void LastIndexOfFromStartIndex()
	{
		using var sequence = Enumerable.Range(100, 5)
			.Concat(Enumerable.Range(100, 5))
			.AsTestingSequence();
		Assert.Equal(
			7,
			sequence.LastIndexOf(102, 8));
	}

	[Fact]
	public void LastIndexOfFromEndIndex()
	{
		using var sequence = Enumerable.Range(100, 5)
			.Concat(Enumerable.Range(100, 5))
			.AsTestingSequence();
		Assert.Equal(
			7,
			sequence.LastIndexOf(102, ^3));
	}

	[Fact]
	public void LastIndexOfFromEndOfArray()
	{
		var array = new int[20];
		array[^7] = 3;
		array[^6] = 3;
		Assert.Equal(
			(^6).GetOffset(20),
			array.LastIndexOf(3, ^5));
	}

	[Fact]
	public void LastIndexOfMissingValueFromStart()
	{
		using var sequence = Enumerable.Range(100, 5).AsTestingSequence();
		Assert.Equal(
			-1,
			sequence.LastIndexOf(95));
	}

	[Fact]
	public void LastIndexOfMissingValueFromEnd()
	{
		using var sequence = Enumerable.Range(100, 5).AsTestingSequence();
		Assert.Equal(
			-1,
			sequence.LastIndexOf(95, ^5));
	}

	[Fact]
	public void LastIndexOfMissingValueFromStartCount()
	{
		using var sequence = Enumerable.Range(100, 5).AsTestingSequence();
		Assert.Equal(
			-1,
			sequence.LastIndexOf(100, int.MaxValue, 4));
	}

	[Fact]
	public void LastIndexOfMissingValueFromEndCount()
	{
		using var sequence = Enumerable.Range(100, 5).AsTestingSequence();
		Assert.Equal(
			-1,
			sequence.LastIndexOf(100, ^1, 3));
	}
}
