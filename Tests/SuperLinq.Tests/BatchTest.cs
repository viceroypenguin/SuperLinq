namespace SuperLinq.Tests;

public sealed class BatchTest
{
	[Test]
	public void BatchIsLazy()
	{
		_ = new BreakingSequence<int>().Batch(1);
		_ = new BreakingSequence<int>().Buffer(1);

		_ = new BreakingSequence<int>()
			.Batch(1, BreakingReadOnlySpanFunc.Of<int, int>());
		_ = new BreakingSequence<int>()
			.Batch(new int[2], BreakingReadOnlySpanFunc.Of<int, int>());
		_ = new BreakingSequence<int>()
			.Batch(new int[2], 1, BreakingReadOnlySpanFunc.Of<int, int>());
	}

	[Test]
	public void BatchValidatesSize()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>("size",
			() => new BreakingSequence<int>()
				.Batch(0));

		_ = Assert.Throws<ArgumentOutOfRangeException>("size",
			() => new BreakingSequence<int>()
				.Batch(0, BreakingReadOnlySpanFunc.Of<int, int>()));

		_ = Assert.Throws<ArgumentOutOfRangeException>("size",
			() => new BreakingSequence<int>()
				.Batch([], 0, BreakingReadOnlySpanFunc.Of<int, int>()));

		_ = Assert.Throws<ArgumentOutOfRangeException>("size",
			() => new BreakingSequence<int>()
				.Batch(new int[5], 6, BreakingReadOnlySpanFunc.Of<int, int>()));

		_ = new BreakingSequence<int>()
			.Batch(new int[5], 5, BreakingReadOnlySpanFunc.Of<int, int>());
	}

	public static IEnumerable<IDisposableEnumerable<int>> GetFourElementSequences() =>
		Enumerable.Range(0, 4)
			.GetListSequences();

	[Test]
	[MethodDataSource(nameof(GetFourElementSequences))]
	public void BatchDoesNotReturnSameArrayInstance(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			using var e = seq.Batch(2).GetEnumerator();

			_ = e.MoveNext();
			var batch1 = e.Current;
			_ = e.MoveNext();
			var batch2 = e.Current;

			Assert.NotEqual(batch1, batch2);
		}
	}

	[Test]
	[MethodDataSource(nameof(GetFourElementSequences))]
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

	[Test]
	[MethodDataSource(nameof(GetFourElementSequences))]
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

	[Test]
	[MethodDataSource(nameof(GetFourElementSequences))]
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

	private static IEnumerable<(IDisposableEnumerable<int> seq, BatchMethod bm)> GetBatchTestSequences(IEnumerable<int> source)
	{
		foreach (var seq in source.GetListSequences())
			yield return (seq, BatchMethod.Traditional);

		yield return (source.AsTestingSequence(), BatchMethod.BufferSize);
		yield return (source.AsTestingSequence(), BatchMethod.BufferArray);
		yield return (source.AsTestingSequence(), BatchMethod.BufferSizeArray);
	}

	private static IEnumerable<IList<T>> GetBatches<T>(
			IEnumerable<T> seq,
			BatchMethod method,
			int size) =>
		method switch
		{
			BatchMethod.Traditional => seq.Batch(size),
			BatchMethod.BufferSize => seq.Batch(size, arr => arr.ToArray()),
			BatchMethod.BufferArray => seq.Batch(new T[size], arr => arr.ToArray()),
			BatchMethod.BufferSizeArray => seq.Batch(new T[size + 10], size, arr => arr.ToArray()),
			_ => throw new NotSupportedException(),
		};

	public static IEnumerable<(IDisposableEnumerable<int>, BatchMethod)> GetEmptySequences() =>
		GetBatchTestSequences([]);

	[Test]
	[MethodDataSource(nameof(GetEmptySequences))]
	public void BatchWithEmptySource(IDisposableEnumerable<int> seq, BatchMethod batchMethod)
	{
		using (seq)
		{
			var result = GetBatches(seq, batchMethod, 5);
			result.AssertSequenceEqual();
		}
	}

	public static IEnumerable<(IDisposableEnumerable<int>, BatchMethod)> GetSequences() =>
		GetBatchTestSequences(Enumerable.Range(1, 9));

	[Test]
	[MethodDataSource(nameof(GetSequences))]
	public void BatchEvenlyDivisibleSequence(IDisposableEnumerable<int> seq, BatchMethod batchMethod)
	{
		using (seq)
		{
			var result = GetBatches(seq, batchMethod, 3);
			result.AssertSequenceEqual(
				[1, 2, 3],
				[4, 5, 6],
				[7, 8, 9]);
		}
	}

	[Test]
	[MethodDataSource(nameof(GetSequences))]
	public void BatchUnevenlyDivisibleSequence(IDisposableEnumerable<int> seq, BatchMethod batchMethod)
	{
		using (seq)
		{
			var result = GetBatches(seq, batchMethod, 4);
			result.AssertSequenceEqual(
				[1, 2, 3, 4],
				[5, 6, 7, 8],
				[9]);
		}
	}

	[Test]
	[MethodDataSource(nameof(GetSequences))]
	public void BatchSmallSequence(IDisposableEnumerable<int> seq, BatchMethod batchMethod)
	{
		using (seq)
		{
			var result = GetBatches(seq, batchMethod, 10);
			result.AssertSequenceEqual(
				[1, 2, 3, 4, 5, 6, 7, 8, 9]);
		}
	}

	private static IEnumerable<(IDisposableEnumerable<int>, BatchMethod)> GetBreakingCollections(IEnumerable<int> source)
	{
		yield return (source.AsBreakingCollection(), BatchMethod.Traditional);
		yield return (source.AsBreakingCollection(), BatchMethod.BufferSize);
		yield return (source.AsBreakingCollection(), BatchMethod.BufferArray);
		yield return (source.AsBreakingCollection(), BatchMethod.BufferSizeArray);
	}

	public static IEnumerable<(IDisposableEnumerable<int>, BatchMethod)> GetEmptyBreakingCollections() =>
		GetBreakingCollections([]);

	[Test]
	[MethodDataSource(nameof(GetEmptyBreakingCollections))]
	public void BatchWithEmptyCollection(IDisposableEnumerable<int> seq, BatchMethod batchMethod)
	{
		using (seq)
		{
			var result = GetBatches(seq, batchMethod, 10);
			result.AssertSequenceEqual();
		}
	}

	public static IEnumerable<(IDisposableEnumerable<int>, BatchMethod)> GetNonEmptyBreakingCollections() =>
		GetBreakingCollections(Enumerable.Range(1, 9));

	[Test]
	[MethodDataSource(nameof(GetNonEmptyBreakingCollections))]
	public void BatchWithCollectionSmallerThanBatchSize(IDisposableEnumerable<int> seq, BatchMethod batchMethod)
	{
		using (seq)
		{
			var result = GetBatches(seq, batchMethod, 10);
			result.AssertSequenceEqual(
				[1, 2, 3, 4, 5, 6, 7, 8, 9]);
		}
	}

	[Test]
	[Arguments(BatchMethod.Traditional)]
	[Arguments(BatchMethod.BufferSize)]
	[Arguments(BatchMethod.BufferArray)]
	[Arguments(BatchMethod.BufferSizeArray)]
	public void BatchCollectionSizeNotEvaluatedEarly(BatchMethod bm)
	{
		var list = new List<int>() { 1, 2, 3 };
		var result = GetBatches(list, bm, 3);
		list.Add(4);
		result.AssertCount(2).Consume();
	}

	[Test]
	[Arguments(BatchMethod.Traditional)]
	[Arguments(BatchMethod.BufferSize)]
	[Arguments(BatchMethod.BufferArray)]
	[Arguments(BatchMethod.BufferSizeArray)]
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

	[Test]
	public void BatchListEvenlyDivisibleBehavior()
	{
		using var seq = Enumerable.Range(0, 10_000).AsBreakingList();

		var result = seq.Batch(20);
		var length = 10_000 / 20;
		result.AssertCollectionErrorChecking(length);
		result.AssertListElementChecking(length);

		Assert.Equal(Enumerable.Range(1_000, 20), result.ElementAt(50));
#if !NO_INDEX
		Assert.Equal(Enumerable.Range(9_980, 20), result.ElementAt(^1));
#endif
	}

	[Test]
	public void BatchListUnevenlyDivisibleBehavior()
	{
		using var seq = Enumerable.Range(0, 10_002).AsBreakingList();

		var result = seq.Batch(20);
		var length = (10_002 / 20) + 1;
		result.AssertCollectionErrorChecking(length);
		result.AssertListElementChecking(length);

		Assert.Equal(Enumerable.Range(1_000, 20), result.ElementAt(50));
#if !NO_INDEX
		Assert.Equal(Enumerable.Range(9_980, 20), result.ElementAt(^2));
		Assert.Equal(Enumerable.Range(10_000, 2), result.ElementAt(^1));
#endif
	}
}
