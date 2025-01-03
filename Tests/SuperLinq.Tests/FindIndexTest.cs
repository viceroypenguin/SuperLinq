#if !NO_INDEX

namespace SuperLinq.Tests;

public sealed class FindIndexTest
{
	[Test]
	public void FindIndexWithNegativeCount()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new BreakingSequence<int>().FindIndex(BreakingFunc.Of<int, bool>(), 1, -1));
	}

	[Test]
	public void FindIndexWorksWithEmptySequence()
	{
		using var sequence = Array.Empty<int>().AsTestingSequence();
		Assert.Equal(-1, sequence.FindIndex(BreakingFunc.Of<int, bool>()));
	}

	[Test]
	public void FindIndexFromStart()
	{
		using var sequence = Enumerable.Range(100, 5)
			.AsTestingSequence();
		Assert.Equal(
			2,
			sequence.FindIndex(i => i == 102));
	}

	[Test]
	public void FindIndexFromStartCount()
	{
		using var sequence = Enumerable.Range(100, 5)
			.AsTestingSequence();
		Assert.Equal(
			2,
			sequence.FindIndex(i => i == 102, 0, 3));
	}

	[Test]
	public void FindIndexFromStartIndex()
	{
		using var sequence = Enumerable.Range(100, 5)
			.Concat(Enumerable.Range(100, 5))
			.AsTestingSequence();
		Assert.Equal(
			7,
			sequence.FindIndex(i => i == 102, 5));
	}

	[Test]
	public void FindIndexFromEndIndex()
	{
		using var sequence = Enumerable.Range(100, 5)
			.Concat(Enumerable.Range(100, 5))
			.AsTestingSequence();
		Assert.Equal(
			7,
			sequence.FindIndex(i => i == 102, ^5));
	}

	[Test]
	public void FindIndexUsesCollectionLengthCorrectly()
	{
		var array = new int[20];
		array[^1] = 3;
		Assert.Equal(
			19,
			array.FindIndex(i => i == 3, ^5));
	}

	[Test]
	public void FindIndexMissingValueFromStart()
	{
		using var sequence = Enumerable.Range(100, 5)
			.AsTestingSequence();
		Assert.Equal(
			-1,
			sequence.FindIndex(i => i == 95));
	}

	[Test]
	public void FindIndexMissingValueFromEnd()
	{
		using var sequence = Enumerable.Range(100, 5)
			.AsTestingSequence();
		Assert.Equal(
			-1,
			sequence.FindIndex(i => i == 95, ^5));
	}

	[Test]
	public void FindIndexMissingValueFromStartCount()
	{
		using var sequence = Enumerable.Range(100, 5)
			.AsTestingSequence();
		Assert.Equal(
			-1,
			sequence.FindIndex(i => i == 104, 0, 4));
	}

	[Test]
	public void FindIndexMissingValueFromEndCount()
	{
		using var sequence = Enumerable.Range(100, 5)
			.AsTestingSequence();
		Assert.Equal(
			-1,
			sequence.FindIndex(i => i == 104, ^5, 4));
	}

	[Test]
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

	[Test]
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

#endif
