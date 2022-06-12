using NUnit.Framework;

namespace SuperLinq.Test;

[TestFixture]
public class PairwiseTest
{
	[Test]
	public void PairwiseIsLazy()
	{
		new BreakingSequence<object>().Pairwise(BreakingFunc.Of<object, object, int>());
	}

	[TestCase(0)]
	[TestCase(1)]
	public void PairwiseWithSequenceShorterThanTwo(int count)
	{
		var source = Enumerable.Range(0, count);
		var result = source.Pairwise(BreakingFunc.Of<int, int, int>());

		Assert.That(result, Is.Empty);
	}

	[Test]
	public void PairwiseWideSourceSequence()
	{
		var result = new[] { "a", "b", "c", "d" }.Pairwise((x, y) => x + y);
		result.AssertSequenceEqual("ab", "bc", "cd");
	}
}
