namespace Test;

public class BatchTest
{
	[Fact]
	public void BatchIsLazy()
	{
		_ = new BreakingSequence<int>().Batch(1);
		_ = new BreakingSequence<int>().Buffer(1);
	}

	[Fact]
	public void BatchValidatesSize()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>("size",
			() => new BreakingSequence<int>()
				.Batch(0));
	}

	public static IEnumerable<object[]> GetSequences(IEnumerable<int> seq) =>
		seq
			.ArrangeCollectionInlineDatas()
			.Select(x => new object[] { x });

	[Theory]
	[MemberData(nameof(GetSequences), new int[] { })]
	public void BatchWithEmptySource(IDisposableEnumerable<int> seq)
	{
		using (seq)
			Assert.Empty(seq.Batch(1));
	}

	[Fact]
	public void BatchWithEmptyIListProvider()
	{
		Enumerable.Range(0, 0)
			.Batch(1)
			.AssertSequenceEqual();
	}

	[Fact]
	public void BatchEvenlyDivisibleSequence()
	{
		using var seq = Enumerable.Range(1, 9).AsTestingSequence();

		var result = seq.Batch(3);
		using var reader = result.Read();
		reader.Read().AssertSequenceEqual(1, 2, 3);
		reader.Read().AssertSequenceEqual(4, 5, 6);
		reader.Read().AssertSequenceEqual(7, 8, 9);
		reader.ReadEnd();
	}

	[Fact]
	public void BatchUnevenlyDivisibleSequence()
	{
		using var seq = Enumerable.Range(1, 9).AsTestingSequence();

		var result = seq.Batch(4);
		using var reader = result.Read();
		reader.Read().AssertSequenceEqual(1, 2, 3, 4);
		reader.Read().AssertSequenceEqual(5, 6, 7, 8);
		reader.Read().AssertSequenceEqual(9);
		reader.ReadEnd();
	}

	[Fact]
	public void BatchWithCollectionSmallerThanBatchSize()
	{
		using var seq = new BreakingCollection<int>(Enumerable.Range(1, 9));
		seq.Batch(10).Consume();
	}

	[Fact]
	public void BatchCollectionSizeNotEvaluatedEarly()
	{
		var list = new List<int>(Enumerable.Range(1, 3));
		var result = list.Batch(3);
		list.Add(4);
		result.AssertCount(2).Consume();
	}

	[Fact]
	public void BatchUsesCollectionCountAtIterationTime()
	{
		var list = new List<int>(Enumerable.Range(1, 3));
		using var ts = new BreakingCollection<int>(list);
		var result = ts.Batch(3);

		// should use `CopyTo`
		result.AssertCount(1).Consume();

		list.Add(4);

		// should fail trying to enumerate
		_ = Assert.Throws<TestException>(
			() => result.AssertCount(2).Consume());
	}

	[Fact]
	public void BatchBufferedIsLazy()
	{
		_ = new BreakingSequence<int>()
			.Batch(1, BreakingFunc.Of<IReadOnlyList<int>, int>());
		_ = new BreakingSequence<int>()
			.Batch(new int[2], BreakingFunc.Of<IReadOnlyList<int>, int>());
		_ = new BreakingSequence<int>()
			.Batch(new int[2], 1, BreakingFunc.Of<IReadOnlyList<int>, int>());
	}

	[Fact]
	public void BatchBufferedValidatesSize()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>("size",
			() => new BreakingSequence<int>()
				.Batch(0, BreakingFunc.Of<IReadOnlyList<int>, int>()));
		_ = Assert.Throws<ArgumentOutOfRangeException>("size",
			() => new BreakingSequence<int>()
				.Batch(new int[2], 0, BreakingFunc.Of<IReadOnlyList<int>, int>()));
		_ = Assert.Throws<ArgumentOutOfRangeException>("size",
			() => new BreakingSequence<int>()
				.Batch(new int[2], 3, BreakingFunc.Of<IReadOnlyList<int>, int>()));
	}

	[Fact]
	public void BatchBufferedWithEmptySource()
	{
		using var xs = TestingSequence.Of<int>();
		Assert.Empty(xs.Batch(1, BreakingFunc.Of<IReadOnlyList<int>, int>()));
	}

	[Fact]
	public void BatchBufferedEvenlyDivisibleSequence()
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
	public void BatchBufferedUnevenlyDivisibleSequence()
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
	public void BatchBufferedWithCollectionSmallerThanBatchSize()
	{
		using var seq = new BreakingCollection<int>(Enumerable.Range(1, 9));
		seq.Batch(10, i => i.Sum()).Consume();
	}

	public static IEnumerable<object[]> GetCollection(IEnumerable<int> seq) =>
		seq
			.ArrangeCollectionInlineDatas()
			.Where(x => x is not TestingSequence<int>)
			.Select(x => new object[] { x });

	[Fact]
	public void BatchBufferedUsesCollectionCountAtIterationTime()
	{
		var list = new List<int>(Enumerable.Range(1, 3));
		using var ts = new BreakingCollection<int>(list);
		var result = ts.Batch(3, w => w[0]);

		// should use `CopyTo`
		result.AssertCount(1).Consume();

		list.Add(4);

		// should fail trying to enumerate
		_ = Assert.Throws<TestException>(
			() => result.AssertCount(2).Consume());
	}
}
