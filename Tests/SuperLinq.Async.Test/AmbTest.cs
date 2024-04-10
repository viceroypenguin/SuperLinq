namespace Test.Async;

public sealed class AmbTest
{
	[Fact]
	public void AmbIsLazy()
	{
		_ = new AsyncBreakingSequence<int>().Amb(new AsyncBreakingSequence<int>());
		_ = AsyncSuperEnumerable.Amb(new AsyncBreakingSequence<int>(), new AsyncBreakingSequence<int>());
	}

	[Theory]
	[InlineData(1, false)]
	[InlineData(2, false)]
	[InlineData(3, false)]
	[InlineData(1, true)]
	[InlineData(2, true)]
	[InlineData(3, true)]
	public async Task AmbEmptyReturnsFirst(int sequenceNumber, bool asyncEmpty)
	{
		var empty = AsyncEnumerable.Empty<int>();
		await using var tsEmpty = empty.AsTestingSequence();
		if (asyncEmpty) empty = tsEmpty;

		var async = AsyncEnumerable.Range(6, 5)
			.SelectAwaitWithCancellation(async (i, ct) =>
			{
				await Task.Delay(250, ct);
				return i;
			});

		// will switch to using `TestingSequence` once I turn on sync/async versions of it
		var seq1 = sequenceNumber == 1 ? empty : async;
		var seq2 = sequenceNumber == 2 ? empty : async;
		var seq3 = sequenceNumber == 3 ? empty : async;

		var ts = new[] { seq1, seq2, seq3, };

		var result = ts.Amb();

		await result.AssertSequenceEqual();
	}

	[Theory]
	[InlineData(1)]
	[InlineData(2)]
	[InlineData(3)]
	public async Task AmbSyncReturnsFirst(int sequenceNumber)
	{
		var sync = AsyncEnumerable.Range(1, 5);
		var async = AsyncEnumerable.Range(6, 5)
			.SelectAwaitWithCancellation(async (i, ct) =>
			{
				await Task.Delay(250, ct);
				return i;
			});

		// will switch to using `TestingSequence` once I turn on sync/async versions of it
		var seq1 = sequenceNumber == 1 ? sync : async;
		var seq2 = sequenceNumber == 2 ? sync : async;
		var seq3 = sequenceNumber == 3 ? sync : async;

		var ts = new[] { seq1, seq2, seq3, };

		var result = ts.Amb();

		await result.AssertSequenceEqual(Enumerable.Range(1, 5));
	}

	[Theory]
	[InlineData(1)]
	[InlineData(2)]
	[InlineData(3)]
	public async Task AmbAsyncShortestComesFirst(int sequenceNumber)
	{
		var shorter = AsyncEnumerable.Range(1, 5)
			.SelectAwaitWithCancellation(async (i, ct) =>
			{
				await Task.Delay(10, ct);
				return i;
			});
		var longer = AsyncEnumerable.Range(6, 5)
			.SelectAwaitWithCancellation(async (i, ct) =>
			{
				await Task.Delay(250, ct);
				return i;
			});
		await using var seq1 = (sequenceNumber == 1 ? shorter : longer).AsTestingSequence();
		await using var seq2 = (sequenceNumber == 2 ? shorter : longer).AsTestingSequence();
		await using var seq3 = (sequenceNumber == 3 ? shorter : longer).AsTestingSequence();

		var ts = new[] { seq1, seq2, seq3, };

		var result = ts.Amb();

		await result.AssertSequenceEqual(Enumerable.Range(1, 5));
	}
}
