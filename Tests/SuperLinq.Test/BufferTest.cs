namespace Test;

public sealed class BufferTest
{
	[Fact]
	public void BufferIsLazy()
	{
		_ = new BreakingSequence<int>().Buffer(2, 1);
	}

	[Fact]
	public void BufferValidatesCount()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>("count",
			() => new BreakingSequence<int>().Buffer(0, 2));
	}

	[Fact]
	public void BufferValidatesSkip()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>("skip",
			() => new BreakingSequence<int>().Buffer(2, 0));
	}

	[Fact]
	public void BufferEmptySequence()
	{
		using var seq = Enumerable.Empty<int>().AsTestingSequence();

		var result = seq.Buffer(1, 2);
		result.AssertSequenceEqual();
	}

	[Theory]
	[InlineData(3)]
	[InlineData(5)]
	[InlineData(7)]
	public void BufferNonOverlappingBuffers(int count)
	{
		using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		var result = seq.Buffer(count, count);
		foreach (var (actual, expected) in result.EquiZip(Enumerable.Range(1, 10).Batch(count)))
			actual.AssertSequenceEqual(expected);
	}

	[Fact]
	public void BufferOverlappingBuffers()
	{
		using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		var result = seq.Buffer(5, 3);
		using var reader = result.Read();
		reader.Read().AssertSequenceEqual(Enumerable.Range(1, 5));
		reader.Read().AssertSequenceEqual(Enumerable.Range(4, 5));
		reader.Read().AssertSequenceEqual(Enumerable.Range(7, 4));
		reader.Read().AssertSequenceEqual(Enumerable.Range(10, 1));
		reader.ReadEnd();
	}

	[Fact]
	public void BufferSkippingBuffers()
	{
		using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		var result = seq.Buffer(5, 7);
		using var reader = result.Read();
		reader.Read().AssertSequenceEqual(Enumerable.Range(1, 5));
		reader.Read().AssertSequenceEqual(Enumerable.Range(8, 3));
		reader.ReadEnd();
	}
}
