using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;

namespace Test;

internal static class SequenceReader
{
	public static SequenceReader<T> Read<T>(this IEnumerable<T> source)
	{
		Guard.IsNotNull(source);
		return new SequenceReader<T>(source);
	}
}

/// <summary>
/// Adds reader semantics to a sequence where <see cref="System.Collections.IEnumerator.MoveNext"/> and <see
/// cref="IEnumerator{T}.Current"/> are rolled into a single "read" operation.
/// </summary>
/// <typeparam name="T">Type of elements to read.</typeparam>
internal sealed class SequenceReader<T> : IDisposable
{
	private IEnumerator<T>? _enumerator;

	/// <summary>
	/// Initializes a <see cref="SequenceReader{T}" /> instance
	/// from an enumerable sequence.
	/// </summary>
	/// <param name="source">Source sequence.</param>

	public SequenceReader(IEnumerable<T> source) :
		this(GetEnumerator(source))
	{ }

	/// <summary>
	/// Initializes a <see cref="SequenceReader{T}" /> instance
	/// from an enumerator.
	/// </summary>
	/// <param name="enumerator">Source enumerator.</param>

	public SequenceReader(IEnumerator<T> enumerator)
	{
		Guard.IsNotNull(enumerator);
		_enumerator = enumerator;
	}

	private static IEnumerator<T> GetEnumerator(IEnumerable<T> source)
	{
		Guard.IsNotNull(source);
		return source.GetEnumerator();
	}

	/// <summary>
	/// Reads the next value. If the sequence is already finished, then it fails the unit test.
	/// </summary>
	public T Read()
	{
		EnsureNotDisposed();

		Assert.True(
			_enumerator.MoveNext(),
			"Sequence is expected to have another value.");

		return _enumerator.Current!;
	}

	/// <summary>
	/// Reads the end. If the end has not been reached, then it fails the unit test.
	/// </summary>
	public void ReadEnd()
	{
		EnsureNotDisposed();

		Assert.False(
			_enumerator.MoveNext(),
			"Sequence is expected to be completed.");
	}

	[MemberNotNull(nameof(_enumerator))]
	private void EnsureNotDisposed()
	{
		if (_enumerator == null)
			Assert.Fail("Sequence was disposed before completing.");
	}

	/// <summary>
	/// Disposes this object and enumerator with which it was initialized.
	/// </summary>
	public void Dispose()
	{
		var e = _enumerator;
		if (e == null) return;
		_enumerator = null;
		e.Dispose();
	}
}
