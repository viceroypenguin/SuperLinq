namespace Test;

public class IndexOfTest
{
	[Fact]
	public void IndexOfWithNegativeStart()
	{
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			Seq(1).IndexOf(1, -1));
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
