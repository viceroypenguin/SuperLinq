using NUnit.Framework;

namespace SuperLinq.Test;

[TestFixture]
public class BacksertTest
{
	[Test]
	public void BacksertIsLazy()
	{
		new BreakingSequence<int>().Backsert(new BreakingSequence<int>(), 0);
	}

	[Test]
	public void BacksertWithNegativeIndex()
	{
		AssertThrowsArgument.OutOfRangeException("index", () =>
			 Enumerable.Range(1, 10).Backsert(new[] { 97, 98, 99 }, -1));
	}

	[TestCase(new[] { 1, 2, 3 }, 4, new[] { 9 })]
	public void BacksertWithIndexGreaterThanSourceLength(int[] seq1, int index, int[] seq2)
	{
		using var test1 = seq1.AsTestingSequence();
		using var test2 = seq2.AsTestingSequence();

		var result = test1.Backsert(test2, index);

		Assert.Throws<ArgumentOutOfRangeException>(() => result.ElementAt(0));
	}

	[TestCase(new[] { 1, 2, 3 }, 0, new[] { 8, 9 }, ExpectedResult = new[] { 1, 2, 3, 8, 9 })]
	[TestCase(new[] { 1, 2, 3 }, 1, new[] { 8, 9 }, ExpectedResult = new[] { 1, 2, 8, 9, 3 })]
	[TestCase(new[] { 1, 2, 3 }, 2, new[] { 8, 9 }, ExpectedResult = new[] { 1, 8, 9, 2, 3 })]
	[TestCase(new[] { 1, 2, 3 }, 3, new[] { 8, 9 }, ExpectedResult = new[] { 8, 9, 1, 2, 3 })]
	public IEnumerable<int> Backsert(int[] seq1, int index, int[] seq2)
	{
		using var test1 = seq1.AsTestingSequence();
		using var test2 = seq2.AsTestingSequence();

		return test1.Backsert(test2, index).ToArray();
	}
}
