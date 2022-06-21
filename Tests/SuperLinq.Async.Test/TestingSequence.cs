namespace Test.Async;

internal static class TestingSequence
{
	internal static TestingSequence<T> Of<T>(params T[] elements) => new(elements.ToAsyncEnumerable());

	internal static TestingSequence<T> AsTestingSequence<T>(this IEnumerable<T> source) =>
		source != null
		? new TestingSequence<T>(source.ToAsyncEnumerable())
		: throw new ArgumentNullException(nameof(source));

	internal static TestingSequence<T> AsTestingSequence<T>(this IAsyncEnumerable<T> source) =>
		source != null
		? new TestingSequence<T>(source)
		: throw new ArgumentNullException(nameof(source));
}

/// <summary>
/// Sequence that asserts whether GetEnumerator() is
/// called exactly once or not.
/// </summary>
internal sealed class TestingSequence<T> : IAsyncEnumerable<T>, IAsyncDisposable
{
	private bool? _disposed;
	private IAsyncEnumerable<T>? _sequence;

	internal TestingSequence(IAsyncEnumerable<T> sequence) =>
		_sequence = sequence;

	public int MoveNextCallCount { get; private set; }

	public ValueTask DisposeAsync()
	{
		if (_disposed != null)
		{
			Assert.True(_disposed, "Expected sequence to be disposed.");
			_disposed = null;
		}
		return default;
	}

	public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
	{
		Assert.NotNull(_sequence);
		var enumerator = _sequence!.GetAsyncEnumerator(cancellationToken).AsWatchable();
		_disposed = false;
		enumerator.Disposed += delegate
		{
			_disposed = true;
		};
		enumerator.MoveNextCalled += (_, moved) =>
		{
			MoveNextCallCount++;
		};
		_sequence = null;
		return enumerator;
	}
}
