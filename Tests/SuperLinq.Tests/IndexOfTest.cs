#if !NO_INDEX

namespace SuperLinq.Tests;

public sealed class IndexOfTest
{
	[Test]
	public void IndexOfWithNegativeCount()
	{
		using var sequence = TestingSequence.Of(1);
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			sequence.IndexOf(1, 1, -1));
	}

	[Test]
	public void IndexOfWorksWithEmptySequence()
	{
		using var sequence = Array.Empty<int>().AsTestingSequence();
		Assert.Equal(-1, sequence.IndexOf(5));
	}

	[Test]
	public void IndexOfFromStart()
	{
		using var sequence = Enumerable.Range(100, 5).AsTestingSequence();
		Assert.Equal(
			2,
			sequence.IndexOf(102));
	}

	[Test]
	public void IndexOfFromStartCount()
	{
		using var sequence = Enumerable.Range(100, 5).AsTestingSequence();
		Assert.Equal(
			2,
			sequence.IndexOf(102, 0, 3));
	}

	[Test]
	public void IndexOfFromStartIndex()
	{
		using var sequence = Enumerable.Range(100, 5).Concat(Enumerable.Range(100, 5)).AsTestingSequence();
		Assert.Equal(
			7,
			sequence.IndexOf(102, 5));
	}

	[Test]
	public void IndexOfFromEndIndex()
	{
		using var sequence = Enumerable.Range(100, 5).Concat(Enumerable.Range(100, 5)).AsTestingSequence();
		Assert.Equal(
			7,
			sequence.IndexOf(102, ^5));
	}

	[Test]
	public void IndexOfFromEndOfArray()
	{
		var array = new int[20];
		array[^1] = 3;
		Assert.Equal(
			19,
			array.IndexOf(3, ^5));
	}

	[Test]
	public void IndexOfMissingValueFromStart()
	{
		using var sequence = Enumerable.Range(100, 5).AsTestingSequence();
		Assert.Equal(
			-1,
			sequence.IndexOf(95));
	}

	[Test]
	public void IndexOfMissingValueFromEnd()
	{
		using var sequence = Enumerable.Range(100, 5).AsTestingSequence();
		Assert.Equal(
			-1,
			sequence.IndexOf(95, ^5));
	}

	[Test]
	public void IndexOfMissingValueFromStartCount()
	{
		using var sequence = Enumerable.Range(100, 5).AsTestingSequence();
		Assert.Equal(
			-1,
			sequence.IndexOf(104, 0, 4));
	}

	[Test]
	public void IndexOfMissingValueFromEndCount()
	{
		using var sequence = Enumerable.Range(100, 5).AsTestingSequence();
		Assert.Equal(
			-1,
			sequence.IndexOf(104, ^5, 4));
	}

	[Test]
	public void IndexOfDoesNotIterateUnnecessaryElements()
	{
		using var source = SuperEnumerable
			.From(
				() => "ana",
				() => "beatriz",
				() => "carla",
				() => "bob",
				() => "davi",
				() => throw new TestException(),
				() => "angelo",
				() => "carlos")
			.AsTestingSequence();

		Assert.Equal(4, source.IndexOf("davi"));
	}

	[Test]
	public void IndexOfDoesNotIterateUnnecessaryElementsCount()
	{
		using var source = SuperEnumerable
			.From(
				() => "ana",
				() => "beatriz",
				() => "carla",
				() => "bob",
				() => "davi",
				() => throw new TestException(),
				() => "angelo",
				() => "carlos")
			.AsTestingSequence();

		Assert.Equal(-1, source.IndexOf("carlos", 0, 5));
	}
}

#endif
