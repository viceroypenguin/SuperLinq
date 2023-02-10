namespace Test;

public class InsertTest
{
	[Fact]
	public void InsertWithNegativeIndex()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			 new BreakingSequence<int>().Insert(new[] { 97, 98, 99 }, -1));
	}

	[Fact]
	public void InsertWithIndexGreaterThanSourceLengthMaterialized()
	{
		var seq1 = Enumerable.Range(0, 10).ToList();
		var seq2 = new[] { 97, 98, 99 };

		using var test1 = seq1.AsTestingSequence();
		using var test2 = seq2.AsTestingSequence();

		var result = test1.Insert(test2, 11);

		_ = Assert.Throws<ArgumentOutOfRangeException>(delegate
		{
			foreach (var (index, e) in result.Index())
				Assert.Equal(seq1[index], e);
		});
	}

	[Fact]
	public void InsertWithIndexGreaterThanSourceLengthLazy()
	{
		var seq1 = Enumerable.Range(0, 10);
		var seq2 = new[] { 97, 98, 99 };

		using var test1 = seq1.AsTestingSequence();
		using var test2 = seq2.AsTestingSequence();

		var result = test1.Insert(test2, 11).Take(10);

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
