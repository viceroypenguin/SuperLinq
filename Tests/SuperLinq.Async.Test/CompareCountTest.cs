namespace Test.Async;

public class CompareCountTest
{
	[Theory]
	[InlineData(0, 0, 0)]
	[InlineData(0, 1, -1)]
	[InlineData(1, 0, 1)]
	[InlineData(1, 1, 0)]
	public async ValueTask CompareCount(int xCount, int yCount, int expected)
	{
		Assert.Equal(expected, await AsyncEnumerable.Range(0, xCount).CompareCount(AsyncEnumerable.Range(0, yCount)));
	}

	[Fact]
	public async ValueTask CompareCountDisposesSequenceEnumerators()
	{
		await using var seq1 = TestingSequence.Of<int>();
		await using var seq2 = TestingSequence.Of<int>();

		Assert.Equal(0, await seq1.CompareCount(seq2));
	}

	[Fact]
	public async ValueTask CompareCountDoesNotIterateUnnecessaryElements()
	{
		var seq1 = AsyncSeqExceptionAt(5);

		var seq2 = AsyncEnumerable.Range(1, 3);

		Assert.Equal(1, await seq1.CompareCount(seq2));
		Assert.Equal(-1, await seq2.CompareCount(seq1));
	}
}
