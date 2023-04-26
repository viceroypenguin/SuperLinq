using System.Xml.Linq;

namespace Test.Async;

public class AmbTest
{
	[Fact]
	public void AmbIsLazy()
	{
		_ = new AsyncBreakingSequence<int>().Amb(new AsyncBreakingSequence<int>());
		_ = AsyncSuperEnumerable.Amb(new AsyncBreakingSequence<int>(), new AsyncBreakingSequence<int>());
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
	public async Task AmbSyncEmptyReturnsFirst(int sequenceNumber)
	{
		var sync = AsyncEnumerable.Empty<int>();
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

	[Theory]
	[InlineData(1)]
	[InlineData(2)]
	[InlineData(3)]
	public async Task AmbAsyncEmptyComesFirst(int sequenceNumber)
	{
		var shorter = EmptyAsyncIterator<int>.Instance;
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

	private sealed class EmptyAsyncIterator<TValue> : IAsyncEnumerable<TValue>, IAsyncEnumerator<TValue>
	{
		public static readonly EmptyAsyncIterator<TValue> Instance = new();

		public TValue Current => default!;
		public async ValueTask<bool> MoveNextAsync()
		{
			await Task.Yield();
			return false;
		}

		public IAsyncEnumerator<TValue> GetAsyncEnumerator(CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested(); // NB: [LDM-2018-11-28] Equivalent to async iterator behavior.

			return this;
		}

		public ValueTask DisposeAsync() => default;
	}
}
