namespace Test.Async;

public class ConcurrentMergeTest
{
	[Fact]
	public void ConcurrentMergeIsLazy()
	{
		new AsyncBreakingSequence<int>().ConcurrentMerge(new AsyncBreakingSequence<int>());
	}

	[Fact]
	public void ConcurrentMergeNegativeMaxConcurrency()
	{
		Assert.Throws<ArgumentOutOfRangeException>("maxConcurrency", () =>
			new[] { new AsyncBreakingSequence<int>() }.ConcurrentMerge(-1));
	}

	[Fact]
	public async Task ConcurrentMergeDisposesOnErrorAtGetEnumerator()
	{
		await using var sequenceA = TestingSequence.Of<int>();
		var sequenceB = new AsyncBreakingSequence<int>();

		// Expected and thrown by BreakingSequence
		await Assert.ThrowsAsync<TestException>(async () =>
			await sequenceA.ConcurrentMerge(sequenceB).Consume());
	}

	[Fact]
	public async Task ConcurrentMergeDisposesOnErrorAtMoveNext()
	{
		await using var sequenceA = TestingSequence.Of<int>();
		await using var sequenceB = AsyncSuperEnumerable.From<int>(() =>
			throw new TestException()).AsTestingSequence();

		// Expected and thrown by sequenceB
		await Assert.ThrowsAsync<TestException>(async () =>
			await sequenceA.ConcurrentMerge(sequenceB).Consume());
	}

	[Fact]
	public async Task ConcurrentMergeTwoBalancedSequences()
	{
		await using var sequenceA = AsyncEnumerable.Range(1, 10).AsTestingSequence();
		await using var sequenceB = AsyncEnumerable.Range(1, 10).AsTestingSequence();
		var result = sequenceA.ConcurrentMerge(sequenceB);

		// variability of `Task.Yield()` means no "perfect" order can be asserted
		await result.AssertCollectionEqual(
			Enumerable.Range(1, 10).SelectMany(x => Enumerable.Repeat(x, 2)));
	}

	[Fact]
	public async Task ConcurrentMergeTwoEmptySequences()
	{
		await using var sequenceA = AsyncEnumerable.Empty<int>().AsTestingSequence();
		await using var sequenceB = AsyncEnumerable.Empty<int>().AsTestingSequence();
		var result = sequenceA.ConcurrentMerge(sequenceB);

		await result.AssertSequenceEqual(Enumerable.Empty<int>());
	}

	[Fact]
	public async Task ConcurrentMergeTwoImbalanceStrategySkip()
	{
		await using var sequenceA = TestingSequence.Of(0, 0, 0, 0, 0, 0);
		await using var sequenceB = TestingSequence.Of(1, 1, 1, 1);
		var result = sequenceA.ConcurrentMerge(sequenceB);

		// variability of `Task.Yield()` means no "perfect" order can be asserted
		await result.AssertCollectionEqual(0, 1, 0, 1, 0, 1, 0, 1, 0, 0);
	}

	[Fact]
	public async Task ConcurrentMergeManyEmptySequences()
	{
		await using var sequenceA = AsyncEnumerable.Empty<int>().AsTestingSequence();
		await using var sequenceB = AsyncEnumerable.Empty<int>().AsTestingSequence();
		await using var sequenceC = AsyncEnumerable.Empty<int>().AsTestingSequence();
		await using var sequenceD = AsyncEnumerable.Empty<int>().AsTestingSequence();
		await using var sequenceE = AsyncEnumerable.Empty<int>().AsTestingSequence();
		var result = sequenceA.ConcurrentMerge(sequenceB, sequenceC, sequenceD, sequenceE);

		await result.AssertEmpty();
	}

	// excess time consumption - avoid unless explicit
	// shorter times introduce variability in ordering
	[Fact(Skip = "Explicit")]
	public async Task ConcurrentMergeReturnsInOrderOfDelay()
	{
		await using var seqA = AsyncSuperEnumerable
			.From(
				async () => { await Task.Delay(100); return 1; },  //  100
				async () => { await Task.Delay(200); return 2; },  //  300
				async () => { await Task.Delay(300); return 3; },  //  600
				async () => { await Task.Delay(400); return 4; })  // 1000
			.AsTestingSequence();
		await using var seqB = AsyncSuperEnumerable
			.From(
				async () => { await Task.Delay(400); return 1; },  //  400
				async () => { await Task.Delay(300); return 2; },  //  700
				async () => { await Task.Delay(200); return 3; },  //  900
				async () => { await Task.Delay(100); return 4; })  // 1000
			.AsTestingSequence();
		var result = seqA.ConcurrentMerge(seqB);

		await result.AssertSequenceEqual(1, 2, 1, 3, 2, 3, 4, 4);
	}

	// excess time consumption - avoid unless explicit
	// shorter times introduce variability in ordering
	[Fact(Skip = "Explicit")]
	public async Task ConcurrentMergeReturnsInOrderOfDelayUnbounded()
	{
		await using var seqA = AsyncSuperEnumerable
			.From(
				async () => { await Task.Delay(200); return 11; },  //  200
				async () => { await Task.Delay(400); return 12; },  //  600
				async () => { await Task.Delay(600); return 13; },  // 1200
				async () => { await Task.Delay(800); return 14; })  // 2000
			.AsTestingSequence();
		await using var seqB = AsyncSuperEnumerable
			.From(
				async () => { await Task.Delay(800); return 21; },  //  800
				async () => { await Task.Delay(600); return 22; },  // 1400
				async () => { await Task.Delay(400); return 23; },  // 1800
				async () => { await Task.Delay(100); return 24; })  // 1900
			.AsTestingSequence();
		await using var seqC = AsyncSuperEnumerable
			.From(
				async () => { await Task.Delay(100); return 31; },  //  100
				async () => { await Task.Delay(300); return 32; },  //  400
				async () => { await Task.Delay(300); return 33; },  //  700
				async () => { await Task.Delay(900); return 34; })  // 1600
			.AsTestingSequence();
		await using var seqD = AsyncSuperEnumerable
			.From(
				async () => { await Task.Delay(300); return 41; },  //  300
				async () => { await Task.Delay(600); return 42; },  //  900
				async () => { await Task.Delay(100); return 43; },  // 1000
				async () => { await Task.Delay(500); return 44; })  // 1500
			.AsTestingSequence();
		var result = new[] { seqA, seqB, seqC, seqD }.ConcurrentMerge();

		await result.AssertSequenceEqual(31, 11, 41, 32, 12, 33, 21, 42, 43, 13, 22, 44, 34, 23, 24, 14);
	}

	[Fact]
	public async Task ConcurrentMergeReturnsWithDelayReturnsAllElementsUnbounded()
	{
		await using var seqA = AsyncSuperEnumerable
			.From(
				async () => { await Task.Delay(20); return 11; },  //  20
				async () => { await Task.Delay(40); return 12; },  //  60
				async () => { await Task.Delay(60); return 13; },  // 120
				async () => { await Task.Delay(80); return 14; })  // 200
			.AsTestingSequence();
		await using var seqB = AsyncSuperEnumerable
			.From(
				async () => { await Task.Delay(80); return 21; },  //  80
				async () => { await Task.Delay(60); return 22; },  // 140
				async () => { await Task.Delay(40); return 23; },  // 180
				async () => { await Task.Delay(10); return 24; })  // 190
			.AsTestingSequence();
		await using var seqC = AsyncSuperEnumerable
			.From(
				async () => { await Task.Delay(10); return 31; },  //  10
				async () => { await Task.Delay(30); return 32; },  //  40
				async () => { await Task.Delay(30); return 33; },  //  70
				async () => { await Task.Delay(90); return 34; })  // 160
			.AsTestingSequence();
		await using var seqD = AsyncSuperEnumerable
			.From(
				async () => { await Task.Delay(30); return 41; },  //  30
				async () => { await Task.Delay(60); return 42; },  //  90
				async () => { await Task.Delay(10); return 43; },  // 100
				async () => { await Task.Delay(50); return 44; })  // 150
			.AsTestingSequence();
		var result = new[] { seqA, seqB, seqC, seqD }.ConcurrentMerge();

		await result.AssertCollectionEqual(11, 12, 13, 14, 21, 22, 23, 24, 31, 32, 33, 34, 41, 42, 43, 44);
	}

	[Fact]
	public async Task ConcurrentMergeReturnsWithDelayReturnsAllElementsBounded()
	{
		await using var seqA = AsyncSuperEnumerable
			.From(
				async () => { await Task.Delay(20); return 11; },  //  20
				async () => { await Task.Delay(40); return 12; },  //  60
				async () => { await Task.Delay(60); return 13; },  // 120
				async () => { await Task.Delay(80); return 14; })  // 200
			.AsTestingSequence();
		await using var seqB = AsyncSuperEnumerable
			.From(
				async () => { await Task.Delay(80); return 21; },  //  80
				async () => { await Task.Delay(60); return 22; },  // 140
				async () => { await Task.Delay(40); return 23; },  // 180
				async () => { await Task.Delay(10); return 24; })  // 190
			.AsTestingSequence();
		await using var seqC = AsyncSuperEnumerable
			.From(
				async () => { await Task.Delay(10); return 31; },  //  10
				async () => { await Task.Delay(30); return 32; },  //  40
				async () => { await Task.Delay(30); return 33; },  //  70
				async () => { await Task.Delay(90); return 34; })  // 160
			.AsTestingSequence();
		await using var seqD = AsyncSuperEnumerable
			.From(
				async () => { await Task.Delay(30); return 41; },  //  30
				async () => { await Task.Delay(60); return 42; },  //  90
				async () => { await Task.Delay(10); return 43; },  // 100
				async () => { await Task.Delay(50); return 44; })  // 150
			.AsTestingSequence();
		var result = new[] { seqA, seqB, seqC, seqD }.ConcurrentMerge(2);

		await result.AssertCollectionEqual(11, 12, 13, 14, 21, 22, 23, 24, 31, 32, 33, 34, 41, 42, 43, 44);
	}

	[Fact]
	public async Task ConcurrentMergeSingleConcurrencyOperatesLikeInterleave()
	{
		await using var seqA = AsyncSuperEnumerable
			.From(
				async () => { await Task.Delay(10); return 1; },
				async () => { await Task.Delay(20); return 2; },
				async () => { await Task.Delay(30); return 3; },
				async () => { await Task.Delay(40); return 4; })
			.AsTestingSequence();
		await using var seqB = AsyncSuperEnumerable
			.From(
				async () => { await Task.Delay(40); return 1; },
				async () => { await Task.Delay(30); return 2; },
				async () => { await Task.Delay(20); return 3; },
				async () => { await Task.Delay(10); return 4; })
			.AsTestingSequence();
		var result = new[] { seqA, seqB }.ConcurrentMerge(1);

		await result.AssertSequenceEqual(1, 1, 2, 2, 3, 3, 4, 4);
	}
}
