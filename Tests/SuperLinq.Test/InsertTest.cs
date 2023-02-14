namespace Test;

public class InsertTest
{
	[Fact]
	public void InsertWithNegativeIndex()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			 new BreakingSequence<int>().Insert(new[] { 97, 98, 99 }, -1));
	}

	[Theory]
	[InlineData(7)]
	[InlineData(8)]
	[InlineData(9)]
	public void InsertWithIndexGreaterThanSourceLengthMaterialized(int count)
	{
		var seq1 = Enumerable.Range(0, count).ToList();
		var seq2 = new[] { 97, 98, 99 };

		using var test1 = seq1.AsTestingSequence();
		using var test2 = seq2.AsTestingSequence();

		var result = test1.Insert(test2, count + 1);

		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			result.ForEach((e, index) =>
				Assert.Equal(seq1[index], e)));
	}

	[Theory]
	[InlineData(7)]
	[InlineData(8)]
	[InlineData(9)]
	public void InsertWithIndexGreaterThanSourceLengthLazy(int count)
	{
		var seq1 = Enumerable.Range(0, count);
		var seq2 = new[] { 97, 98, 99 };

		using var test1 = seq1.AsTestingSequence();
		using var test2 = seq2.AsTestingSequence();

		var result = test1.Insert(test2, count + 1).Take(count);

		Assert.Equal(result, seq1);
	}

	[Theory]
	[InlineData(3, 0)]
	[InlineData(3, 1)]
	[InlineData(3, 2)]
	[InlineData(3, 3)]
	public void Insert(int count, int index)
	{
		var seq1 = Enumerable.Range(1, count);
		var seq2 = new[] { 97, 98, 99 };

		using var test1 = seq1.AsTestingSequence();
		using var test2 = seq2.AsTestingSequence();

		var result = test1.Insert(test2, index);

		var expectations = seq1.Take(index).Concat(seq2).Concat(seq1.Skip(index));
		Assert.Equal(expectations, result);
	}

	[Fact]
	public void InsertIsLazy()
	{
		_ = new BreakingSequence<int>().Insert(new BreakingSequence<int>(), 0);
	}
}
