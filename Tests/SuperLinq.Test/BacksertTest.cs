namespace Test;

#pragma warning disable CS0618 // Type or member is obsolete

public class BacksertTest
{
	[Fact]
	public void BacksertIsLazy()
	{
		_ = new BreakingSequence<int>().Backsert(new BreakingSequence<int>(), 0);
	}

	[Fact]
	public void BacksertWithNegativeIndex()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			 new BreakingSequence<int>().Backsert([97, 98, 99], -1));
	}

	[Theory]
	[InlineData(new[] { 1, 2, 3 }, 4, new[] { 9 })]
	public void BacksertWithIndexGreaterThanSourceLength(int[] seq1, int index, int[] seq2)
	{
		using var test1 = seq1.AsTestingSequence();
		using var test2 = seq2.AsTestingSequence();

		var result = test1.Backsert(test2, index);

		_ = Assert.Throws<ArgumentOutOfRangeException>(() => result.ElementAt(0));
	}

	[Theory]
	[InlineData(new[] { 1, 2, 3 }, 0, new[] { 8, 9 }, new[] { 1, 2, 3, 8, 9 })]
	[InlineData(new[] { 1, 2, 3 }, 1, new[] { 8, 9 }, new[] { 1, 2, 8, 9, 3 })]
	[InlineData(new[] { 1, 2, 3 }, 2, new[] { 8, 9 }, new[] { 1, 8, 9, 2, 3 })]
	[InlineData(new[] { 1, 2, 3 }, 3, new[] { 8, 9 }, new[] { 8, 9, 1, 2, 3 })]
	public void Backsert(int[] seq1, int index, int[] seq2, int[] expected)
	{
		using var test1 = seq1.AsTestingSequence();
		using var test2 = seq2.AsTestingSequence();

		Assert.Equal(expected, test1.Backsert(test2, index).ToArray());
	}
}

#pragma warning restore CS0618 // Type or member is obsolete
