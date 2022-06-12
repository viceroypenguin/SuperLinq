using System.Collections;
using NUnit.Framework;

namespace SuperLinq.Test;

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
/// Sequence that asserts whether its iterator has been disposed
/// when it is disposed itself and also whether GetEnumerator() is
/// called exactly once or not.
/// </summary>
sealed class TestingSequence<T> : IEnumerable<T>, IDisposable
{
	bool? _disposed;
	IEnumerable<T> _sequence;

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
		Assert.IsTrue(_disposed, "Expected sequence to be disposed.");
		_disposed = null;
	}

	public IEnumerator<T> GetEnumerator()
	{
		Assert.That(_sequence, Is.Not.Null, "LINQ operators should not enumerate a sequence more than once.");
		var enumerator = _sequence.GetEnumerator().AsWatchable();
		_disposed = false;
		enumerator.Disposed += delegate
		{
			Assert.That(_disposed, Is.False, "LINQ operators should not dispose a sequence more than once.");
			_disposed = true;
		};
		var ended = false;
		enumerator.MoveNextCalled += (_, moved) =>
		{
			Assert.That(ended, Is.False, "LINQ operators should not continue iterating a sequence that has terminated.");
			ended = !moved;
			MoveNextCallCount++;
		};
		_sequence = null;
		return enumerator;
	}

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

}
