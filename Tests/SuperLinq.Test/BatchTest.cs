using System.Globalization;

namespace Test;

public class BatchTest
{
	[Fact]
	public void BatchIsLazy()
	{
		new BreakingSequence<int>()
			.Batch(1, BreakingFunc.Of<IReadOnlyList<int>, int>());
		new BreakingSequence<int>()
			.Batch(new int[2], BreakingFunc.Of<IReadOnlyList<int>, int>());
		new BreakingSequence<int>()
			.Batch(new int[2], 1, BreakingFunc.Of<IReadOnlyList<int>, int>());
	}

	[Fact]
	public void BatchValidatesSize()
	{
		Assert.Throws<ArgumentOutOfRangeException>("size",
			() => new BreakingSequence<int>()
				.Batch(0, BreakingFunc.Of<IReadOnlyList<int>, int>()));
		Assert.Throws<ArgumentOutOfRangeException>("size",
			() => new BreakingSequence<int>()
				.Batch(new int[2], 0, BreakingFunc.Of<IReadOnlyList<int>, int>()));
		Assert.Throws<ArgumentOutOfRangeException>("size",
			() => new BreakingSequence<int>()
				.Batch(new int[2], 3, BreakingFunc.Of<IReadOnlyList<int>, int>()));
	}

	[Fact]
	public void BatchWithEmptySource()
	{
		using var xs = TestingSequence.Of<int>();
		Assert.Empty(xs.Batch(1, BreakingFunc.Of<IReadOnlyList<int>, int>()));
	}


	[Fact]
	public void BatchEvenlyDivisibleSequence()
	{
		using var seq = Enumerable.Range(1, 9).AsTestingSequence();

		var result = seq.Batch(3, l => string.Join(",", l));
		using var reader = result.Read();
		Assert.Equal("1,2,3", reader.Read());
		Assert.Equal("4,5,6", reader.Read());
		Assert.Equal("7,8,9", reader.Read());
		reader.ReadEnd();
	}

	[Fact]
	public void BatchUnevenlyDivisibleSequence()
	{
		using var seq = Enumerable.Range(1, 9).AsTestingSequence();

		var result = seq.Batch(4, l => string.Join(",", l));
		using var reader = result.Read();
		Assert.Equal("1,2,3,4", reader.Read());
		Assert.Equal("5,6,7,8", reader.Read());
		Assert.Equal("9", reader.Read());
		reader.ReadEnd();
	}

	[Fact]
	public void BatchWithCollectionSmallerThanBatchSize()
	{
		using var seq = new BreakingCollection<int>(Enumerable.Range(1, 9));
		seq.Batch(10, i => i.Sum()).Consume();
	}

	[Fact]
	public void BatchCollectionSizeNotEvaluatedEarly()
	{
		var stack = new Stack<int>(Enumerable.Range(1, 3));
		var result = stack.Batch(3);
		stack.Push(4);
		result.AssertCount(2).Consume();
	}
}
