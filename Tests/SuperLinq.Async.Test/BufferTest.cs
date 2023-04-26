namespace Test.Async;

public class BufferTest
{
	[Fact]
	public void BufferIsLazy()
	{
		_ = new AsyncBreakingSequence<int>().Buffer(2, 1);
	}

	[Fact]
	public void BufferValidatesCount()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>("count",
			() => new AsyncBreakingSequence<int>().Buffer(0, 2));
	}

	[Fact]
	public void BufferValidatesSkip()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>("skip",
			() => new AsyncBreakingSequence<int>().Buffer(2, 0));
	}

	[Fact]
	public async Task BufferEmptySequence()
	{
		await using var seq = Enumerable.Empty<int>().AsTestingSequence();

		var result = seq.Buffer(1, 2);
		await result.AssertSequenceEqual();
	}

	[Theory]
	[InlineData(3)]
	[InlineData(5)]
	[InlineData(7)]
	public async Task BufferNonOverlappingBuffers(int count)
	{
		await using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		var result = seq.Buffer(count, count);
		await foreach (var (actual, expected) in result
			.EquiZip(AsyncEnumerable.Range(1, 10).Batch(count)))
		{
			actual.AssertSequenceEqual(expected);
		}
	}

	[Fact]
	public async Task BufferOverlappingBuffers()
	{
		await using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		var result = seq.Buffer(5, 3);
		await using var reader = result.Read();
		(await reader.Read()).AssertSequenceEqual(Enumerable.Range(1, 5));
		(await reader.Read()).AssertSequenceEqual(Enumerable.Range(4, 5));
		(await reader.Read()).AssertSequenceEqual(Enumerable.Range(7, 4));
		(await reader.Read()).AssertSequenceEqual(Enumerable.Range(10, 1));
		await reader.ReadEnd();
	}

	[Fact]
	public async Task BufferSkippingBuffers()
	{
		await using var seq = Enumerable.Range(1, 10).AsTestingSequence();

		var result = seq.Buffer(5, 7);
		await using var reader = result.Read();
		(await reader.Read()).AssertSequenceEqual(Enumerable.Range(1, 5));
		(await reader.Read()).AssertSequenceEqual(Enumerable.Range(8, 3));
		await reader.ReadEnd();
	}
}
