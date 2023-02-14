namespace Test;

public class FindIndexTest
{
	[Fact]
	public void FindIndexWithNegativeCount()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new BreakingSequence<int>().FindIndex(BreakingFunc.Of<int, bool>(), 1, -1));
	}

	[Fact]
	public void FindIndexWorksWithEmptySequence()
	{
		using var sequence = Array.Empty<int>().AsTestingSequence();
		Assert.Equal(-1, sequence.FindIndex(BreakingFunc.Of<int, bool>()));
	}

	[Fact]
	public void FindIndexFromStart()
	{
		using var sequence = Enumerable.Range(100, 5)
			.AsTestingSequence();
		Assert.Equal(
			2,
			sequence.FindIndex(i => i == 102));
	}

	[Fact]
	public void FindIndexFromStartCount()
	{
		using var sequence = Enumerable.Range(100, 5)
			.AsTestingSequence();
		Assert.Equal(
			2,
			sequence.FindIndex(i => i == 102, 0, 3));
	}

	[Fact]
	public void FindIndexFromStartIndex()
	{
		using var sequence = Enumerable.Range(100, 5)
			.Concat(Enumerable.Range(100, 5))
			.AsTestingSequence();
		Assert.Equal(
			7,
			sequence.FindIndex(i => i == 102, 5));
	}

	[Fact]
	public void FindIndexFromEndIndex()
	{
		using var sequence = Enumerable.Range(100, 5)
			.Concat(Enumerable.Range(100, 5))
			.AsTestingSequence();
		Assert.Equal(
			7,
			sequence.FindIndex(i => i == 102, ^5));
	}

	[Fact]
	public void FindIndexUsesCollectionLengthCorrectly()
	{
		var array = new int[20];
		array[^1] = 3;
		Assert.Equal(
			19,
			array.FindIndex(i => i == 3, ^5));
	}

	[Fact]
	public void FindIndexMissingValueFromStart()
	{
		using var sequence = Enumerable.Range(100, 5)
			.AsTestingSequence();
		Assert.Equal(
			-1,
			sequence.FindIndex(i => i == 95));
	}

	[Fact]
	public void FindIndexMissingValueFromEnd()
	{
		using var sequence = Enumerable.Range(100, 5)
			.AsTestingSequence();
		Assert.Equal(
			-1,
			sequence.FindIndex(i => i == 95, ^5));
	}

	[Fact]
	public void FindIndexMissingValueFromStartCount()
	{
		using var sequence = Enumerable.Range(100, 5)
			.AsTestingSequence();
		Assert.Equal(
			-1,
			sequence.FindIndex(i => i == 104, 0, 4));
	}

	[Fact]
	public void FindIndexMissingValueFromEndCount()
	{
		using var sequence = Enumerable.Range(100, 5)
			.AsTestingSequence();
		Assert.Equal(
			-1,
			sequence.FindIndex(i => i == 104, ^5, 4));
	}

	[Fact]
	public void FindIndexDoesNotIterateUnnecessaryElements()
	{
		using var source = SuperEnumerable
			.From(
				() => "ana",
				() => "beatriz",
				() => "carla",
				() => "bob",
				() => "davi",
				BreakingFunc.Of<string>(),
				() => "angelo",
				() => "carlos")
			.AsTestingSequence();

		Assert.Equal(4, source.FindIndex(i => i == "davi"));
	}

	[Fact]
	public void FindIndexDoesNotIterateUnnecessaryElementsCount()
	{
		using var source = SuperEnumerable
			.From(
				() => "ana",
				() => "beatriz",
				() => "carla",
				() => "bob",
				() => "davi",
				BreakingFunc.Of<string>(),
				() => "angelo",
				() => "carlos")
			.AsTestingSequence();

		Assert.Equal(-1, source.FindIndex(i => i == "carlos", 0, 5));
	}
}
