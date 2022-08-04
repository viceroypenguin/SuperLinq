﻿namespace Test;

public class BindByIndexTest
{
	[Fact]
	public void BindByIndexIsLazy()
	{
		new BreakingSequence<int>().BindByIndex(new BreakingSequence<int>());
		new BreakingSequence<int>().BindByIndex(new BreakingSequence<int>(), BreakingFunc.Of<int, int, int>(), BreakingFunc.Of<int, int>());
	}

	[Fact]
	public void BindByIndexDisposesEnumerators()
	{
		using var seq1 = TestingSequence.Of<int>();
		using var seq2 = TestingSequence.Of<int>();
		Assert.Empty(seq1.BindByIndex(seq2));
	}

	[Fact]
	public void BindByIndexInOrder()
	{
		var seq1 = Enumerable.Range(1, 10);
		var seq2 = Seq(1, 3, 5, 7, 9);

		seq1.BindByIndex(seq2).AssertSequenceEqual(seq2.Select(x => x + 1));
	}

	[Fact]
	public void BindByIndexOutOfOrder()
	{
		var seq1 = Enumerable.Range(1, 10);
		var seq2 = Seq(9, 7, 5, 3, 1);

		seq1.BindByIndex(seq2).AssertSequenceEqual(seq2.Select(x => x + 1));
	}

	[Fact]
	public void BindByIndexComplex()
	{
		var seq1 = Enumerable.Range(1, 10);
		var seq2 = Seq(0, 1, 8, 9, 3, 4, 2);

		seq1.BindByIndex(seq2).AssertSequenceEqual(seq2.Select(x => x + 1));
	}

	[Theory]
	[InlineData(-1)]
	[InlineData(10)]
	[InlineData(100)]
	public void BindByIndexThrowExceptionInvalidIndex(int index)
	{
		var seq1 = Enumerable.Range(1, 10);
		var seq2 = Seq(index);

		Assert.Throws<ArgumentOutOfRangeException>("indices", () => seq1.BindByIndex(seq2).Consume());
	}

	[Fact]
	public void BindByIndexTransformInOrder()
	{
		var seq1 = Enumerable.Range(1, 10);
		var seq2 = Seq(1, 3, 5, 7, 9);

		seq1.BindByIndex(seq2, (e, i) => e, i => default(int?)).AssertSequenceEqual(seq2.Select(x => (int?)(x + 1)));
	}

	[Fact]
	public void BindByIndexTransformOutOfOrder()
	{
		var seq1 = Enumerable.Range(1, 10);
		var seq2 = Seq(9, 7, 5, 3, 1);

		seq1.BindByIndex(seq2, (e, i) => e, i => default(int?)).AssertSequenceEqual(seq2.Select(x => (int?)(x + 1)));
	}

	[Fact]
	public void BindByIndexTransformComplex()
	{
		var seq1 = Enumerable.Range(1, 10);
		var seq2 = Seq(0, 1, 8, 9, 3, 4, 2);

		seq1.BindByIndex(seq2, (e, i) => e, i => default(int?)).AssertSequenceEqual(seq2.Select(x => (int?)(x + 1)));
	}

	[Fact]
	public void BindByIndexTransformInvalidIndex()
	{
		var seq1 = Enumerable.Range(1, 10);
		var seq2 = Seq(1, 10, 3, 30);

		seq1.BindByIndex(seq2, (e, i) => e, i => default(int?)).AssertSequenceEqual(2, null, 4, null);
	}

	[Fact]
	public void BindByIndexTransformThrowExceptionNegativeIndex()
	{
		var seq1 = Enumerable.Range(1, 10);
		var seq2 = Seq(-1);

		Assert.Throws<ArgumentOutOfRangeException>("indices", () => seq1.BindByIndex(seq2, (e, i) => e, i => default(int?)).Consume());
	}
}
