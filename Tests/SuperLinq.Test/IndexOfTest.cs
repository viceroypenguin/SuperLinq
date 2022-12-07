namespace Test;

public class IndexOfTest
{
	[Fact]
	public void IndexOfWithNegativeStart()
	{
		using var sequence = Seq(1).AsTestingSequence();
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			sequence.IndexOf(1, -1));
	}

	[Fact]
	public void IndexOfWorksWithEmptySequence()
	{
		using var sequence = Array.Empty<int>().AsTestingSequence();
		Assert.Equal(-1, sequence.IndexOf(5));
	}

	[Fact]
	public void IndexOfFromStart()
	{
		using var sequence = Enumerable.Range(100, 5).AsTestingSequence();
		Assert.Equal(
			2,
			sequence.IndexOf(102));
	}

	[Fact]
	public void IndexOfFromStartIndex()
	{
		using var sequence = Enumerable.Range(100, 5).Concat(Enumerable.Range(100, 5)).AsTestingSequence();
		Assert.Equal(
			7,
			sequence.IndexOf(102, 5));
	}

	[Fact]
	public void IndexOfFromEndIndex()
	{
		using var sequence = Enumerable.Range(100, 5).Concat(Enumerable.Range(100, 5)).AsTestingSequence();
		Assert.Equal(
			7,
			sequence.IndexOf(102, ^5));
	}

	[Fact]
	public void IndexOfFromEndOfArray()
	{
		var array = new int[20];
		array[^1] = 3;
		Assert.Equal(
			19,
			array.IndexOf(3, ^5));
	}

	[Fact]
	public void IndexOfMissingValueFromStart()
	{
		using var sequence = Enumerable.Range(100, 5).AsTestingSequence();
		Assert.Equal(
			-1,
			sequence.IndexOf(95));
	}

	[Fact]
	public void IndexOfMissingValueFromEnd()
	{
		using var sequence = Enumerable.Range(100, 5).AsTestingSequence();
		Assert.Equal(
			-1,
			sequence.IndexOf(95, ^5));
	}

	[Fact]
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

		Assert.Equal(2, source.IndexOf("carla"));
	}
}
