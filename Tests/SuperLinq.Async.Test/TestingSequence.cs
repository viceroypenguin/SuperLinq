using System.Collections;
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
