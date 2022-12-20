namespace Test.Async;

public class CompareCountTest
{
	[Theory]
	[InlineData(0, 0, 0)]
	[InlineData(0, 1, -1)]
	[InlineData(1, 0, 1)]
	[InlineData(1, 1, 0)]
	public async Task CompareCount(int xCount, int yCount, int expected)
	{
		await using var xs = AsyncEnumerable.Range(0, xCount).AsTestingSequence();
		await using var ys = AsyncEnumerable.Range(0, yCount).AsTestingSequence();
		Assert.Equal(expected, await xs.CompareCount(ys));
	}

	[Fact]
	public async Task CompareCountDisposesSequenceEnumerators()
	{
		await using var seq1 = TestingSequence.Of<int>();
		await using var seq2 = TestingSequence.Of<int>();

		Assert.Equal(0, await seq1.CompareCount(seq2));
	}

	[Fact]
	public async Task CompareCountDoesNotIterateUnnecessaryElements()
	{
		await using var seq1 = AsyncSeqExceptionAt(5).AsTestingSequence();
		await using var seq2 = AsyncEnumerable.Range(1, 3).AsTestingSequence();

		Assert.Equal(1, await seq1.CompareCount(seq2));
	}
}
