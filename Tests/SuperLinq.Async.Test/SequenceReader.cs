using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;

namespace Test.Async;

internal static class SequenceReader
{
	public static SequenceReader<T> Read<T>(this IAsyncEnumerable<T> source)
	{
		Guard.IsNotNull(source);
		return new SequenceReader<T>(source);
	}
}

/// <summary>
/// Adds reader semantics to a sequence where <see cref="IAsyncEnumerator{T}.MoveNextAsync"/>
/// and <see cref="IAsyncEnumerator{T}.Current"/> are rolled into a single
/// "read" operation.
/// </summary>
/// <typeparam name="T">Type of elements to read.</typeparam>
internal sealed class SequenceReader<T> : IAsyncDisposable
{
	private IAsyncEnumerator<T>? _enumerator;

	/// <summary>
	/// Initializes a <see cref="SequenceReader{T}" /> instance
	/// from an enumerable sequence.
	/// </summary>
	/// <param name="source">Source sequence.</param>

	public SequenceReader(IAsyncEnumerable<T> source) :
		this(GetEnumerator(source))
	{ }

	/// <summary>
	/// Initializes a <see cref="SequenceReader{T}" /> instance
	/// from an enumerator.
	/// </summary>
	/// <param name="enumerator">Source enumerator.</param>

	public SequenceReader(IAsyncEnumerator<T> enumerator)
	{
		Guard.IsNotNull(enumerator);
		_enumerator = enumerator;
	}

	private static IAsyncEnumerator<T> GetEnumerator(IAsyncEnumerable<T> source)
	{
		Guard.IsNotNull(source);
		return source.GetAsyncEnumerator();
	}

	/// <summary>
	/// Reads the next value. If the sequence is already finished, then it fails the unit test.
	/// </summary>
	public async ValueTask<T> Read()
	{
		EnsureNotDisposed();

		Assert.True(
			await _enumerator.MoveNextAsync(),
			"Sequence is expected to have another value.");

		return _enumerator.Current!;
	}

	/// <summary>
	/// Reads the end. If the end has not been reached, then it fails the unit test.
	/// </summary>
	public async ValueTask ReadEnd()
	{
		EnsureNotDisposed();

		Assert.False(
			await _enumerator.MoveNextAsync(),
			"Sequence is expected to be completed.");
	}

	[MemberNotNull(nameof(_enumerator))]
	private void EnsureNotDisposed()
	{
		if (_enumerator is null)
			Assert.Fail("Sequence was disposed before completing.");
	}

	/// <summary>
	/// Disposes this object and enumerator with which it was initialized.
	/// </summary>
	public async ValueTask DisposeAsync()
	{
		var e = _enumerator;
		if (e is null) return;
		_enumerator = null;
		await e.DisposeAsync();
	}
}
