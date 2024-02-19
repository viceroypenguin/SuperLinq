namespace Test;

public class BatchTest
{
	[Fact]
	public void BatchIsLazy()
	{
		_ = new BreakingSequence<int>().Batch(1);
		_ = new BreakingSequence<int>().Buffer(1);

		_ = new BreakingSequence<int>()
			.Batch(1, BreakingFunc.Of<ArraySegment<int>, int>());
		_ = new BreakingSequence<int>()
			.Batch(new int[2], BreakingFunc.Of<ArraySegment<int>, int>());
		_ = new BreakingSequence<int>()
			.Batch(new int[2], 1, BreakingFunc.Of<ArraySegment<int>, int>());
	}

	[Fact]
	public void BatchValidatesSize()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>("size",
			() => new BreakingSequence<int>()
				.Batch(0));

		_ = Assert.Throws<ArgumentOutOfRangeException>("size",
			() => new BreakingSequence<int>()
				.Batch(0, BreakingFunc.Of<ArraySegment<int>, int>()));

		_ = Assert.Throws<ArgumentOutOfRangeException>("size",
			() => new BreakingSequence<int>()
				.Batch([], 0, BreakingFunc.Of<ArraySegment<int>, int>()));

		_ = Assert.Throws<ArgumentOutOfRangeException>("size",
			() => new BreakingSequence<int>()
				.Batch(new int[5], 6, BreakingFunc.Of<ArraySegment<int>, int>()));

		_ = new BreakingSequence<int>()
			.Batch(new int[5], 5, BreakingFunc.Of<ArraySegment<int>, int>());
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

	public enum BatchMethod
	{
		Traditional,
		BufferSize,
		BufferArray,
		BufferSizeArray,
	}

	private static IEnumerable<object[]> GetBatchTestSequences(IEnumerable<int> source)
	{
		foreach (var seq in source.GetListSequences())
			yield return new object[] { seq, BatchMethod.Traditional, };
		yield return new object[] { source.AsTestingSequence(), BatchMethod.BufferSize, };
		yield return new object[] { source.AsTestingSequence(), BatchMethod.BufferArray, };
		yield return new object[] { source.AsTestingSequence(), BatchMethod.BufferSizeArray, };
	}

	private static IEnumerable<IList<T>> GetBatches<T>(
			IEnumerable<T> seq,
			BatchMethod method,
			int size) =>
		method switch
		{
			BatchMethod.Traditional => seq.Batch(size),
			BatchMethod.BufferSize => seq.Batch(size, arr => arr.ToList()),
			BatchMethod.BufferArray => seq.Batch(new T[size], arr => arr.ToList()),
			BatchMethod.BufferSizeArray => seq.Batch(new T[size + 10], size, arr => arr.ToList()),
			_ => throw new NotSupportedException(),
		};

	public static IEnumerable<object[]> GetEmptySequences() =>
		GetBatchTestSequences([]);

	[Theory]
	[MemberData(nameof(GetEmptySequences))]
	public void BatchWithEmptySource(IDisposableEnumerable<int> seq, BatchMethod bm)
	{
		using (seq)
		{
			var result = GetBatches(seq, bm, 5);
			result.AssertSequenceEqual();
		}
	}

	public static IEnumerable<object[]> GetSequences() =>
		GetBatchTestSequences(Enumerable.Range(1, 9));

	[Theory]
	[MemberData(nameof(GetSequences))]
	public void BatchEvenlyDivisibleSequence(IDisposableEnumerable<int> seq, BatchMethod bm)
	{
		using (seq)
		{
			var result = GetBatches(seq, bm, 3);
			result.AssertSequenceEqual(
				[1, 2, 3],
				[4, 5, 6],
				[7, 8, 9]);
		}
	}

	[Theory]
	[MemberData(nameof(GetSequences))]
	public void BatchUnevenlyDivisibleSequence(IDisposableEnumerable<int> seq, BatchMethod bm)
	{
		using (seq)
		{
			var result = GetBatches(seq, bm, 4);
			result.AssertSequenceEqual(
				[1, 2, 3, 4],
				[5, 6, 7, 8],
				[9]);
		}
	}

	[Theory]
	[MemberData(nameof(GetSequences))]
	public void BatchSmallSequence(IDisposableEnumerable<int> seq, BatchMethod bm)
	{
		using (seq)
		{
			var result = GetBatches(seq, bm, 10);
			result.AssertSequenceEqual(
				[1, 2, 3, 4, 5, 6, 7, 8, 9]);
		}
	}

	public static IEnumerable<object[]> GetBreakingCollections(IEnumerable<int> source)
	{
		yield return new object[] { source.AsBreakingCollection(), BatchMethod.Traditional, };
		yield return new object[] { source.AsBreakingCollection(), BatchMethod.BufferSize, };
		yield return new object[] { source.AsBreakingCollection(), BatchMethod.BufferArray, };
		yield return new object[] { source.AsBreakingCollection(), BatchMethod.BufferSizeArray, };
	}

	[Theory]
	[MemberData(nameof(GetBreakingCollections), new int[] { })]
	public void BatchWithEmptyCollection(IDisposableEnumerable<int> seq, BatchMethod bm)
	{
		using (seq)
		{
			var result = GetBatches(seq, bm, 10);
			result.AssertSequenceEqual();
		}
	}

	[Theory]
	[MemberData(nameof(GetBreakingCollections), new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 })]
	public void BatchWithCollectionSmallerThanBatchSize(IDisposableEnumerable<int> seq, BatchMethod bm)
	{
		using (seq)
		{
			var result = GetBatches(seq, bm, 10);
			result.AssertSequenceEqual(
				[1, 2, 3, 4, 5, 6, 7, 8, 9]);
		}
	}

	[Theory]
	[InlineData(BatchMethod.Traditional)]
	[InlineData(BatchMethod.BufferSize)]
	[InlineData(BatchMethod.BufferArray)]
	[InlineData(BatchMethod.BufferSizeArray)]
	public void BatchCollectionSizeNotEvaluatedEarly(BatchMethod bm)
	{
		var list = new List<int>() { 1, 2, 3, };
		var result = GetBatches(list, bm, 3);
		list.Add(4);
		result.AssertCount(2).Consume();
	}

	[Theory]
	[InlineData(BatchMethod.Traditional)]
	[InlineData(BatchMethod.BufferSize)]
	[InlineData(BatchMethod.BufferArray)]
	[InlineData(BatchMethod.BufferSizeArray)]
	public void BatchUsesCollectionCountAtIterationTime(BatchMethod bm)
	{
		var list = new List<int>(Enumerable.Range(1, 3));
		using var ts = new BreakingCollection<int>(list);
		var result = GetBatches(ts, bm, 3);

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
}
