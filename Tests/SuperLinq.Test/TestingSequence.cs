using System.Collections;

namespace Test;

static class TestingSequence
{
	internal static TestingSequence<T> Of<T>(params T[] elements) =>
		new TestingSequence<T>(elements);

	internal static TestingSequence<T> AsTestingSequence<T>(this IEnumerable<T> source) =>
		source != null
		? new TestingSequence<T>(source)
		: throw new ArgumentNullException(nameof(source));
}

/// <summary>
/// Sequence that asserts whether GetEnumerator() is
/// called exactly once or not.
/// </summary>
sealed class TestingSequence<T> : IEnumerable<T>, IDisposable
{
	private bool? _disposed;
	private IEnumerable<T>? _sequence;

	internal TestingSequence(IEnumerable<T> sequence) =>
		_sequence = sequence;

	public int MoveNextCallCount { get; private set; }

	void IDisposable.Dispose() =>
		AssertDisposed();

	/// <summary>
	/// Checks that the iterator was disposed, and then resets.
	/// </summary>
	void AssertDisposed()
	{
		if (_disposed == null)
			return;
		Assert.True(_disposed, "Expected sequence to be disposed.");
		_disposed = null;
	}

	public IEnumerator<T> GetEnumerator()
	{
		Assert.NotNull(_sequence);
		var enumerator = _sequence!.GetEnumerator().AsWatchable();
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

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

}
