namespace Test;

public class PairwiseTest
{
	[Fact]
	public void PairwiseIsLazy()
	{
		new BreakingSequence<object>().Pairwise(BreakingFunc.Of<object, object, int>());
	}

	[Theory]
	[InlineData(0)]
	[InlineData(1)]
	public void PairwiseWithSequenceShorterThanTwo(int count)
	{
		var source = Enumerable.Range(0, count);
		var result = source.Pairwise(BreakingFunc.Of<int, int, int>());

		Assert.Empty(result);
	}

	[Fact]
	public void PairwiseWideSourceSequence()
	{
		var result = new[] { "a", "b", "c", "d" }.Pairwise((x, y) => x + y);
		result.AssertSequenceEqual("ab", "bc", "cd");
	}
}
