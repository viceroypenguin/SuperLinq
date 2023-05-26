namespace Test;

public class BatchTest
{
	#region Regular
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

	public static IEnumerable<object[]> GetFourElementSequences() =>
		Enumerable.Range(0, 4)
			.GetListSequences()
			.Select(x => new object[] { x });

	[Theory]
	[MemberData(nameof(GetFourElementSequences))]
	public void BatchModifiedBeforeMoveNextDoesNotAffectNextBatch(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			using var e = seq.Batch(2).GetEnumerator();

			_ = e.MoveNext();
			var batch1 = e.Current;
			batch1[1] = -1;
			_ = e.MoveNext();
			var batch2 = e.Current;

			Assert.Equal(3, batch2[1]);
		}
	}

	[Theory]
	[MemberData(nameof(GetFourElementSequences))]
	public void BatchModifiedAfterMoveNextDoesNotAffectNextBatch(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			using var e = seq.Batch(2).GetEnumerator();

			_ = e.MoveNext();
			var batch1 = e.Current;
			_ = e.MoveNext();
			batch1[1] = -1;
			var batch2 = e.Current;

			Assert.Equal(3, batch2[1]);
		}
	}

	[Theory]
	[MemberData(nameof(GetFourElementSequences))]
	public void BatchModifiedDoesNotAffectPreviousBatch(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			using var e = seq.Batch(2).GetEnumerator();

			_ = e.MoveNext();
			var batch1 = e.Current;
			_ = e.MoveNext();
			var batch2 = e.Current;
			batch2[1] = -1;

			Assert.Equal(1, batch1[1]);
		}
	}

	public static IEnumerable<object[]> GetEmptySequences() =>
		Array.Empty<int>()
			.GetAllSequences()
			.Select(x => new object[] { x });

	[Theory]
	[MemberData(nameof(GetEmptySequences))]
	public void BatchWithEmptySource(IDisposableEnumerable<int> seq)
	{
		using (seq)
			Assert.Empty(seq.Batch(1));
	}

	[Fact]
	// branch not able to run with `BreakingList<>`
	public void BatchWithEmptyIListProvider()
	{
		Enumerable.Range(0, 0)
			.Batch(1)
			.AssertSequenceEqual();
	}

	public static IEnumerable<object[]> GetSequences() =>
		Enumerable.Range(1, 9)
			.GetListSequences()
			.Select(x => new object[] { x });

	[Theory]
	[MemberData(nameof(GetSequences))]
	public void BatchEvenlyDivisibleSequence(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.Batch(3);

			result.AssertSequenceEqual(
				Seq(1, 2, 3),
				Seq(4, 5, 6),
				Seq(7, 8, 9));
		}
	}

	[Theory]
	[MemberData(nameof(GetSequences))]
	public void BatchUnevenlyDivisibleSequence(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.Batch(4);

			result.AssertSequenceEqual(
				Seq(1, 2, 3, 4),
				Seq(5, 6, 7, 8),
				Seq(9));
		}
	}

	[Theory]
	[MemberData(nameof(GetSequences))]
	public void BatchSmallSequence(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.Batch(10);

			result.AssertSequenceEqual(
				Seq(1, 2, 3, 4, 5, 6, 7, 8, 9));
		}
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
	public void BatchListEvenlyDivisibleBehavior()
	{
		using var seq = Enumerable.Range(0, 10_000).AsBreakingList();

		var result = seq.Batch(20);
		var length = 10_000 / 20;
		result.AssertCollectionErrorChecking(length);
		result.AssertListElementChecking(length);

		Assert.Equal(Enumerable.Range(1_000, 20), result.ElementAt(50));
		Assert.Equal(Enumerable.Range(9_980, 20), result.ElementAt(^1));
	}

	[Fact]
	public void BatchListUnevenlyDivisibleBehavior()
	{
		using var seq = Enumerable.Range(0, 10_002).AsBreakingList();

		var result = seq.Batch(20);
		var length = (10_002 / 20) + 1;
		result.AssertCollectionErrorChecking(length);
		result.AssertListElementChecking(length);

		Assert.Equal(Enumerable.Range(1_000, 20), result.ElementAt(50));
		Assert.Equal(Enumerable.Range(9_980, 20), result.ElementAt(^2));
		Assert.Equal(Enumerable.Range(10_000, 2), result.ElementAt(^1));
	}
	#endregion

	#region Buffered
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
	#endregion
}
