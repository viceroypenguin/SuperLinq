﻿namespace SuperLinq.Tests;

public sealed class BindByIndexTest
{
	[Test]
	public void BindByIndexIsLazy()
	{
		_ = new BreakingSequence<int>().BindByIndex(new BreakingSequence<int>());
		_ = new BreakingSequence<int>().BindByIndex(new BreakingSequence<int>(), BreakingFunc.Of<int, int, int>(), BreakingFunc.Of<int, int>());
	}

	[Test]
	public void BindByIndexDisposesEnumerators()
	{
		using var seq1 = TestingSequence.Of<int>();
		using var seq2 = TestingSequence.Of<int>();
		Assert.Empty(seq1.BindByIndex(seq2));
	}

	[Test]
	public void BindByIndexInOrder()
	{
		var indexes = Seq(1, 3, 5, 7, 9);
		using var seq1 = Enumerable.Range(1, 10).AsTestingSequence();
		using var seq2 = indexes.AsTestingSequence();

		seq1.BindByIndex(seq2).AssertSequenceEqual(indexes.Select(x => x + 1));
	}

	[Test]
	public void BindByIndexOutOfOrder()
	{
		var indexes = Seq(9, 7, 5, 3, 1);
		using var seq1 = Enumerable.Range(1, 10).AsTestingSequence();
		using var seq2 = indexes.AsTestingSequence();

		seq1.BindByIndex(seq2).AssertSequenceEqual(indexes.Select(x => x + 1));
	}

	[Test]
	public void BindByIndexComplex()
	{
		var indexes = Seq(0, 1, 8, 9, 3, 4, 2);
		using var seq1 = Enumerable.Range(1, 10).AsTestingSequence();
		using var seq2 = indexes.AsTestingSequence();

		seq1.BindByIndex(seq2).AssertSequenceEqual(indexes.Select(x => x + 1));
	}

	[Test]
	[Arguments(-1)]
	[Arguments(10)]
	[Arguments(100)]
	public void BindByIndexThrowExceptionInvalidIndex(int index)
	{
		using var seq1 = Enumerable.Range(1, 10).AsTestingSequence();
		using var seq2 = TestingSequence.Of(index);

		_ = Assert.Throws<ArgumentOutOfRangeException>("indices", () =>
			seq1.BindByIndex(seq2).Consume());
	}

	[Test]
	public void BindByIndexTransformInOrder()
	{
		var indexes = Seq(1, 3, 5, 7, 9);
		using var seq1 = Enumerable.Range(1, 10).AsTestingSequence();
		using var seq2 = indexes.AsTestingSequence();

		seq1.BindByIndex(seq2, (e, i) => e, i => default(int?))
			.AssertSequenceEqual(indexes.Select(x => (int?)(x + 1)));
	}

	[Test]
	public void BindByIndexTransformOutOfOrder()
	{
		var indexes = Seq(9, 7, 5, 3, 1);
		using var seq1 = Enumerable.Range(1, 10).AsTestingSequence();
		using var seq2 = indexes.AsTestingSequence();

		seq1.BindByIndex(seq2, (e, i) => e, i => default(int?))
			.AssertSequenceEqual(indexes.Select(x => (int?)(x + 1)));
	}

	[Test]
	public void BindByIndexTransformComplex()
	{
		var indexes = Seq(0, 1, 8, 9, 3, 4, 2);
		using var seq1 = Enumerable.Range(1, 10).AsTestingSequence();
		using var seq2 = indexes.AsTestingSequence();

		seq1.BindByIndex(seq2, (e, i) => e, i => default(int?))
			.AssertSequenceEqual(indexes.Select(x => (int?)(x + 1)));
	}

	[Test]
	public void BindByIndexTransformInvalidIndex()
	{
		using var seq1 = Enumerable.Range(1, 10).AsTestingSequence();
		using var seq2 = TestingSequence.Of(1, 10, 3, 30);

		seq1.BindByIndex(seq2, (e, i) => e, i => default(int?))
			.AssertSequenceEqual(2, null, 4, null);
	}

	[Test]
	public void BindByIndexTransformThrowExceptionNegativeIndex()
	{
		using var seq1 = Enumerable.Range(1, 10).AsTestingSequence();
		using var seq2 = TestingSequence.Of(-1);

		_ = Assert.Throws<ArgumentOutOfRangeException>("indices", () =>
			seq1.BindByIndex(seq2, (e, i) => e, i => default(int?)).Consume());
	}
}
