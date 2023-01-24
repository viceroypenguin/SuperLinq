using CommunityToolkit.Diagnostics;
using Xunit.Sdk;

namespace Test.Async;

internal static class TestingSequence
{
	internal static TestingSequence<T> Of<T>(params T[] elements) => new(elements.ToAsyncEnumerable(), 1);

	internal static TestingSequence<T> AsTestingSequence<T>(this IEnumerable<T> source, int numEnumerations = 1) =>
		source != null
		? new TestingSequence<T>(source.ToAsyncEnumerable(), numEnumerations)
		: ThrowHelper.ThrowArgumentNullException<TestingSequence<T>>(nameof(source));

	internal static TestingSequence<T> AsTestingSequence<T>(this IAsyncEnumerable<T> source, int numEnumerations = 1) =>
		source != null
		? new TestingSequence<T>(source, numEnumerations)
		: ThrowHelper.ThrowArgumentNullException<TestingSequence<T>>(nameof(source));

	internal const string ExpectedDisposal = "Expected sequence to be disposed.";
	internal const string TooManyEnumerations = "Sequence should not be enumerated more than expected.";
	internal const string SimultaneousEnumerations = "Sequence should not have simultaneous enumeration.";
	internal const string MoveNextDisposed = "LINQ operators should not call MoveNext() on a disposed sequence.";
	internal const string CurrentDisposed = "LINQ operators should not attempt to get the Current value on a disposed sequence.";
	internal const string CurrentCompleted = "LINQ operators should not attempt to get the Current value on a completed sequence.";
}

internal sealed class TestingSequence<T> : IAsyncEnumerable<T>, IAsyncDisposable, IDisposable
{
	private IAsyncEnumerable<T>? _sequence;
	private readonly int _numEnumerations;

	private bool _hasEnumerated;
	private bool _currentlyEnumerating;
	private int _disposedCount;
	private int _enumerationCount;

	internal TestingSequence(IAsyncEnumerable<T> sequence, int numEnumerations)
	{
		_sequence = sequence;
		_numEnumerations = numEnumerations;
	}

	public int MoveNextCallCount { get; private set; }

	void IDisposable.Dispose()
	{
		if (_hasEnumerated)
			Assert.True(_disposedCount == _enumerationCount, TestingSequence.ExpectedDisposal);
	}

	ValueTask IAsyncDisposable.DisposeAsync()
	{
		if (_hasEnumerated)
			Assert.True(_disposedCount == _enumerationCount, TestingSequence.ExpectedDisposal);
		return default;
	}

	public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
	{
		Assert.False(_sequence is null, TestingSequence.TooManyEnumerations);
		Assert.False(_currentlyEnumerating, TestingSequence.SimultaneousEnumerations);

		_hasEnumerated = true;

		var enumerator = _sequence.GetAsyncEnumerator(cancellationToken).AsWatchable();
		_currentlyEnumerating = true;

		var disposed = false;
		enumerator.Disposed += delegate
		{
			if (!disposed)
			{
				_disposedCount++;
				_currentlyEnumerating = false;
				disposed = true;
			}
		};

		var ended = false;
		enumerator.MoveNextCalled += (_, moved) =>
		{
			Assert.True(_currentlyEnumerating, TestingSequence.MoveNextDisposed);
			ended = !moved;
			MoveNextCallCount++;
		};
		enumerator.GetCurrentCalled += delegate
		{
			Assert.True(_currentlyEnumerating, TestingSequence.CurrentDisposed);
			Assert.False(ended, TestingSequence.CurrentCompleted);
		};

		if (++_enumerationCount == _numEnumerations)
			_sequence = null;
		return enumerator;
	}
}

public class TestingSequenceTest
{
	[Fact]
	public async Task TestingSequenceShouldValidateDisposal()
	{
		static async IAsyncEnumerable<int> InvalidUsage(IAsyncEnumerable<int> enumerable)
		{
			var enumerator = enumerable.GetAsyncEnumerator();

			await Task.Yield();
			yield break;
		}

		var ex = await Assert.ThrowsAsync<TrueException>(async () =>
		{
			using var xs = Enumerable.Range(1, 10).AsTestingSequence();
			await InvalidUsage(xs).Consume();
		});
		Assert.StartsWith(TestingSequence.ExpectedDisposal, ex.Message);
	}

	[Fact]
	public async Task TestingSequenceShouldValidateNumberOfUsages()
	{
		static async IAsyncEnumerable<int> InvalidUsage(IAsyncEnumerable<int> enumerable)
		{
			await using (var enumerator = enumerable.GetAsyncEnumerator())
				yield return 1;
			await using (var enumerator = enumerable.GetAsyncEnumerator())
				yield return 2;
			await using (var enumerator = enumerable.GetAsyncEnumerator())
				yield return 3;

			await Task.Yield();
			yield break;
		}

		var ex = await Assert.ThrowsAsync<FalseException>(async () =>
		{
			using var xs = Enumerable.Range(1, 10).AsTestingSequence(2);
			await InvalidUsage(xs).Consume();
		});
		Assert.StartsWith(TestingSequence.TooManyEnumerations, ex.Message);
	}

	[Fact]
	public async Task TestingSequenceShouldValidateMoveNextOnDisposedSequence()
	{
		static async IAsyncEnumerable<int> InvalidUsage(IAsyncEnumerable<int> enumerable)
		{
			await using var enumerator = enumerable.GetAsyncEnumerator();
			await enumerator.DisposeAsync();
			await enumerator.MoveNextAsync();

			await Task.Yield();
			yield break;
		}

		var ex = await Assert.ThrowsAsync<TrueException>(async () =>
		{
			using var xs = Enumerable.Range(1, 10).AsTestingSequence();
			await InvalidUsage(xs).Consume();
		});
		Assert.StartsWith(TestingSequence.MoveNextDisposed, ex.Message);
	}

	[Fact]
	public async Task TestingSequenceShouldValidateCurrentOnDisposedSequence()
	{
		static async IAsyncEnumerable<int> InvalidUsage(IAsyncEnumerable<int> enumerable)
		{
			await using var enumerator = enumerable.GetAsyncEnumerator();
			await enumerator.DisposeAsync();
			yield return enumerator.Current;

			await Task.Yield();
			yield break;
		}

		var ex = await Assert.ThrowsAsync<TrueException>(async () =>
		{
			using var xs = Enumerable.Range(1, 10).AsTestingSequence();
			await InvalidUsage(xs).Consume();
		});
		Assert.StartsWith(TestingSequence.CurrentDisposed, ex.Message);
	}

	[Fact]
	public async Task TestingSequenceShouldValidateCurrentOnEndedSequence()
	{
		static async IAsyncEnumerable<int> InvalidUsage(IAsyncEnumerable<int> enumerable)
		{
			await using var enumerator = enumerable.GetAsyncEnumerator();
			while (await enumerator.MoveNextAsync())
				yield return enumerator.Current;
			yield return enumerator.Current;

			await Task.Yield();
			yield break;
		}

		var ex = await Assert.ThrowsAsync<FalseException>(async () =>
		{
			using var xs = Enumerable.Range(1, 10).AsTestingSequence();
			await InvalidUsage(xs).Consume();
		});
		Assert.StartsWith(TestingSequence.CurrentCompleted, ex.Message);
	}

	[Fact]
	public async Task TestingSequenceShouldValidateSimultaneousEnumeration()
	{
		static async IAsyncEnumerable<int> InvalidUsage(IAsyncEnumerable<int> enumerable)
		{
			await using var enum1 = enumerable.GetAsyncEnumerator();
			await using var enum2 = enumerable.GetAsyncEnumerator();

			await Task.Yield();
			yield break;
		}

		var ex = await Assert.ThrowsAsync<FalseException>(async () =>
		{
			using var xs = Enumerable.Range(1, 10).AsTestingSequence(2);
			await InvalidUsage(xs).Consume();
		});
		Assert.StartsWith(TestingSequence.SimultaneousEnumerations, ex.Message);
	}
}
