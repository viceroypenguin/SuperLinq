using System.Collections;

namespace Test;

internal static class TestingSequence
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
internal sealed class TestingSequence<T> : IEnumerable<T>, IDisposable
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
	private void AssertDisposed()
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
		var ended = false;
		enumerator.MoveNextCalled += (_, moved) =>
		{
			Assert.False(_disposed, "LINQ operators should not call MoveNext() on a disposed sequence.");
			ended = !moved;
			MoveNextCallCount++;
		};
		enumerator.GetCurrentCalled += delegate
		{
			Assert.False(_disposed, "LINQ operators should not attempt to get the Current value on a disposed sequence.");
			Assert.False(ended, "LINQ operators should not attempt to get the Current value on a completed sequence.");
		};
		_sequence = null;
		return enumerator;
	}

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

}
