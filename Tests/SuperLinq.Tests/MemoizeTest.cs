using System.Collections;

namespace SuperLinq.Tests;

public sealed class MemoizeTest
{
	[Test]
	public void MemoizeIsLazy()
	{
		_ = new BreakingSequence<int>().Memoize();
	}

	public static IEnumerable<IDisposableEnumerable<int>> GetSequences() =>
		Enumerable.Range(1, 10)
			.GetBreakingCollectionSequences();

	[Test]
	[MethodDataSource(nameof(GetSequences))]
	public void MemoizeSimpleUse(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			using var buffer = seq.Memoize();
			Assert.Equal(0, buffer.Count);

			buffer.AssertSequenceEqual(Enumerable.Range(1, 10));
			Assert.Equal(10, buffer.Count);
		}
	}

	[Test]
	[MethodDataSource(nameof(GetSequences))]
	public void MemoizeReturningExpectedElementsWhenUsedAtInnerForEach(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var buffer = seq.Memoize();

			var flowArray = InnerForEach(Enumerable.Range(1, 10));
			var flowBuffer = InnerForEach(buffer);

			flowArray.AssertSequenceEqual(flowBuffer);

			static IEnumerable<object> InnerForEach(IEnumerable<int> source)
			{
				var firstVisitAtInnerLoopDone = false;

				//add 1-3 to cache (so enter inner loop)
				//consume 4-5 already cached
				//add 6-7 to cache (so enter inner loop)
				//consume 8-10 already cached

				yield return "enter outer loop";
				foreach (var i in source)
				{
					yield return i;

					if (i is 3 or 7)
					{
						//consume 1-3 already cached
						//add 4-5 to cache (so go to outer loop)
						//consume 1-7 already cached
						//add 8-10 to cache (so go to outer loop)

						yield return "enter inner loop";
						foreach (var j in source)
						{
							yield return j;

							if (!firstVisitAtInnerLoopDone && j == 5)
							{
								firstVisitAtInnerLoopDone = true;
								break;
							}
						}

						yield return "exit inner loop";
					}
				}

				yield return "exit outer loop";

				yield return "enter last loop";
				//consume 1-10 (all item were already cached)
				foreach (var k in source)
					yield return k;

				yield return "exit last loop";
			}
		}
	}

	[Test]
	[MethodDataSource(nameof(GetSequences))]
	public void MemoizeThrowsWhenCacheDisposedDuringIteration(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			using var buffer = seq.Memoize();

			using var reader = buffer.Read();

			Assert.Equal(1, reader.Read());
			buffer.Dispose();

			_ = Assert.Throws<ObjectDisposedException>(
				() => reader.Read());
		}
	}

	[Test]
	[MethodDataSource(nameof(GetSequences))]
	public void MemoizeThrowsWhenResetDuringIteration(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			using var buffer = seq.Memoize();

			using var reader = buffer.Read();
			Assert.Equal(1, reader.Read());

			buffer.Reset();

			var ex = Assert.Throws<InvalidOperationException>(
				() => reader.Read());
			Assert.Equal("Buffer reset during iteration.", ex.Message);
		}
	}

	[Test]
	[MethodDataSource(nameof(GetSequences))]
	public void MemoizeThrowsWhenGettingIteratorAfterDispose(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			using var buffer = seq.Memoize();

			buffer.Consume();
			buffer.Dispose();

			_ = Assert.Throws<ObjectDisposedException>(
				() => buffer.Consume());
		}
	}

	[Test]
	[MethodDataSource(nameof(GetSequences))]
	public void MemoizeThrowsWhenResettingAfterDispose(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			using var buffer = seq.Memoize();

			buffer.Consume();
			buffer.Dispose();

			_ = Assert.Throws<ObjectDisposedException>(buffer.Reset);
		}
	}

	[Test]
	public void MemoizeIteratorWithPartialIterationBeforeCompleteIteration()
	{
		using var seq = Enumerable.Range(0, 10).AsTestingSequence();

		using var buffer = seq.Memoize();
		Assert.Equal(0, buffer.Count);

		buffer.Take(5).AssertSequenceEqual(Enumerable.Range(0, 5));
		Assert.Equal(5, buffer.Count);

		buffer.AssertSequenceEqual(Enumerable.Range(0, 10));
		Assert.Equal(10, buffer.Count);
	}

	[Test]
	public void MemoizeIteratorWithDisposeOnEarlyExitTrue()
	{
		using var seq = Enumerable.Range(0, 10).AsTestingSequence();

		using var buffer = seq.Memoize();
		Assert.Equal(0, buffer.Count);

		using (buffer)
			buffer.Take(1).Consume();

		Assert.Equal(0, buffer.Count);
		Assert.True(seq.IsDisposed);
	}

	[Test]
	public void MemoizeIteratorDisposesAfterSourceIsIteratedEntirely()
	{
		using var seq = Enumerable.Range(0, 10).AsTestingSequence();

		using var buffer = seq.Memoize();
		buffer.Consume();

		Assert.True(seq.IsDisposed);
	}

	[Test]
	public void MemoizeIteratorEnumeratesOnlyOnce()
	{
		using var ts = Enumerable.Range(0, 10).AsTestingSequence();

		using var buffer = ts.Memoize();

		buffer.AssertSequenceEqual(Enumerable.Range(0, 10));
		buffer.AssertSequenceEqual(Enumerable.Range(0, 10));
	}

	[Test]
	public void MemoizeIteratorRestartsAfterReset()
	{
		var starts = 0;

		IEnumerable<int> TestSequence()
		{
			starts++;
			yield return 1;
			yield return 2;
		}

		using var seq = TestSequence().AsTestingSequence(maxEnumerations: 2);
		using var memoized = seq.Memoize();

		memoized.Take(1).Consume();
		Assert.Equal(1, starts);

		memoized.Reset();
		memoized.Take(1).Consume();
		Assert.Equal(2, starts);
	}

	[Test]
	public void MemoizeIteratorRethrowsErrorDuringIterationToAllIteratorsUntilReset()
	{
		using var xs = SeqExceptionAt(2).AsTestingSequence(maxEnumerations: 2);

		using var memoized = xs.Memoize();

		using (var r1 = memoized.Read())
		using (var r2 = memoized.Read())
		{
			Assert.True(r1.Read() == r2.Read());
			_ = Assert.Throws<TestException>(() => r1.Read());

			Assert.True(xs.IsDisposed);
			_ = Assert.Throws<TestException>(() => r2.Read());
		}

		memoized.Reset();

		using (var r1 = memoized.Read())
			Assert.Equal(1, r1.Read());
	}

	[Test]
	public void MemoizeIteratorRethrowsErrorDuringIterationStartToAllIteratorsUntilReset()
	{
		var i = 0;
		using var xs = SuperEnumerable
			.From(() => i++ == 0 ? throw new TestException() : 42)
			.AsTestingSequence(maxEnumerations: 2);

		using var buffer = xs.Memoize();
		Assert.Equal(0, buffer.Count);

		using (var r1 = buffer.Read())
		using (var r2 = buffer.Read())
		{
			_ = Assert.Throws<TestException>(() => r1.Read());
			Assert.True(xs.IsDisposed);
			_ = Assert.Throws<TestException>(() => r2.Read());
		}

		Assert.Equal(0, buffer.Count);

		buffer.Reset();
		Assert.Equal(0, buffer.Count);
		using (var r1 = buffer.Read())
		using (var r2 = buffer.Read())
			Assert.True(r1.Read() == r2.Read());

		Assert.Equal(1, buffer.Count);
	}

	[Test]
	public void MemoizeIteratorRethrowsErrorDuringFirstIterationStartToAllIterationsUntilReset()
	{
		using var seq = new FailingEnumerable().AsTestingSequence(maxEnumerations: 2);

		using var buffer = seq.Memoize();
		Assert.Equal(0, buffer.Count);

		for (var i = 0; i < 2; i++)
			_ = Assert.Throws<TestException>(() => buffer.First());

		buffer.Reset();
		buffer.AssertSequenceEqual(1);
		Assert.Equal(1, buffer.Count);
	}

	private sealed class FailingEnumerable : IEnumerable<int>
	{
		private bool _started;
		public IEnumerator<int> GetEnumerator()
		{
			if (!_started)
			{
				_started = true;
				throw new TestException();
			}

			return Enumerable.Range(1, 1).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}

	[Test]
	public void MemoizeCollectionsUseCopyTo()
	{
		using var list = new BreakingCollection<int>(Enumerable.Range(0, 10));

		using var buffer = list.Memoize();
		Assert.Equal(0, buffer.Count);

		buffer.Take(1).Consume();
		Assert.Equal(10, buffer.Count);
	}

	[Test]
	public void MemoizeCollectionEnumeratesOnlyOnce()
	{
		using var ts = new BreakingCollection<int>(Enumerable.Range(0, 10));

		using var buffer = ts.Memoize();
		Assert.Equal(0, buffer.Count);

		buffer.AssertSequenceEqual(Enumerable.Range(0, 10));
		buffer.AssertSequenceEqual(Enumerable.Range(0, 10));
		Assert.Equal(1, ts.CopyCount);
		Assert.Equal(10, buffer.Count);
	}

	[Test]
	public void MemoizeCollectionRestartsAfterReset()
	{
		using var ts = new BreakingCollection<int>(Enumerable.Range(0, 10));

		using var memoized = ts.Memoize();
		Assert.Equal(0, memoized.Count);

		memoized.Take(1).Consume();
		Assert.Equal(1, ts.CopyCount);
		Assert.Equal(10, memoized.Count);

		memoized.Reset();
		memoized.Take(1).Consume();
		Assert.Equal(2, ts.CopyCount);
		Assert.Equal(10, memoized.Count);
	}

	[Test]
	public void MemoizeCollectionRethrowsErrorDuringFirstIterationStartToAllIterationsUntilReset()
	{
		using var ts = new FailingCollection();
		using var memo = ts.Memoize();

		for (var i = 0; i < 2; i++)
			_ = Assert.Throws<TestException>(() => memo.First());

		memo.Reset();
		memo.AssertSequenceEqual(1);
		Assert.Equal(1, ts.CopyCount);
	}

	private sealed class FailingCollection : BreakingCollection<int>
	{
		public FailingCollection() : base(1) { }

		private bool _started;
		public override void CopyTo(int[] array, int arrayIndex)
		{
			if (!_started)
			{
				_started = true;
				throw new TestException();
			}

			base.CopyTo(array, arrayIndex);
		}
	}

	[Test]
	public void MemoizeProxySimpleUse()
	{
		var ts = Enumerable.Range(1, 10).ToArray();

		using var buffer = ts.Memoize(forceCache: false);
		Assert.Equal(10, buffer.Count);

		buffer.AssertSequenceEqual(Enumerable.Range(1, 10));
		Assert.Equal(10, buffer.Count);

		buffer.Reset();
		buffer.AssertSequenceEqual(Enumerable.Range(1, 10));
		Assert.Equal(10, buffer.Count);
	}

	[Test]
	public void MemoizeProxyThrowsExceptionAfterDisposal()
	{
		var ts = Enumerable.Range(1, 10).ToArray();
		var buffer = ts.Memoize(forceCache: false);

		buffer.Consume();
		buffer.Dispose();

		_ = Assert.Throws<ObjectDisposedException>(buffer.Consume);
	}

	[Test]
	public void MemoizeProxyThrowsExceptionWhenResettingAfterDisposal()
	{
		var ts = Enumerable.Range(1, 10).ToArray();
		var buffer = ts.Memoize(forceCache: false);

		buffer.Consume();
		buffer.Dispose();

		_ = Assert.Throws<ObjectDisposedException>(buffer.Reset);
	}

	[Test]
	public void MemoizeProxyReturnsCollectionIteratorDirectly()
	{
		var iterator = Enumerable.Empty<int>().GetEnumerator();
		var coll = new ProxyCollection(() => iterator, () => 42);
		var memo = coll.Memoize(forceCache: false);

		Assert.Same(iterator, memo.GetEnumerator());
		Assert.Equal(42, memo.Count);
	}

	private sealed class ProxyCollection(
		Func<IEnumerator<int>> enumeratorFunc,
		Func<int> countFunc
	) : ICollection<int>
	{
		public int Count => countFunc();

		public void Add(int item) => throw new TestException();
		public void Clear() => throw new TestException();
		public bool Contains(int item) => throw new TestException();
		public bool Remove(int item) => throw new TestException();
		public bool IsReadOnly => true;

		public void CopyTo(int[] array, int arrayIndex) => throw new TestException();

		public IEnumerator<int> GetEnumerator() => enumeratorFunc();
		IEnumerator IEnumerable.GetEnumerator() => enumeratorFunc();
	}

#if false
	[Test, Explicit]
	public static void MemoizeIsThreadSafe()
	{
		var sequence = Enumerable.Range(1, 50000);
		using var ts = sequence.AsTestingSequence();
		var memoized = ts.Memoize();

		var lists = Enumerable.Range(0, Environment.ProcessorCount * 2)
							  .Select(_ => new List<int>())
							  .ToArray();

		using var start = new Barrier(lists.Length);

		var threads =
			from list in lists
			select new Thread(() =>
			{
				start.SignalAndWait();
				list.AddRange(memoized);
			});

		threads.Pipe(t => t.Start())
			   .ToArray() // start all before joining
			   .ForEach(t => t.Join());

		Assert.That(sequence, Is.EqualTo(memoized));
		lists.ForEach(list => Assert.That(list, Is.EqualTo(memoized)));
	}
#endif
}
