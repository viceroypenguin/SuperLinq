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
/// Adds reader semantics to a sequence where <see cref="IAsyncEnumerable{T}.MoveNext"/>
/// and <see cref="IAsyncEnumerable{T}.Current"/> are rolled into a single
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
	/// Reads a value otherwise throws <see cref="InvalidOperationException"/>
	/// if no more values are available.
	/// </summary>
	/// <returns>
	/// Returns the read value;
	/// </returns>

	public async ValueTask<T> Read()
	{
		EnsureNotDisposed();

		if (!await _enumerator.MoveNextAsync())
			throw new InvalidOperationException();

		return _enumerator.Current!;
	}

	/// <summary>
	/// Reads the end. If the end has not been reached then it
	/// throws <see cref="InvalidOperationException"/>.
	/// </summary>

	public virtual async ValueTask ReadEnd()
	{
		EnsureNotDisposed();

		if (await _enumerator.MoveNextAsync())
			throw new InvalidOperationException();
	}

	/// <summary>
	/// Ensures that this object has not been disposed, that
	/// <see cref="Dispose"/> has not been previously called.
	/// </summary>

	[MemberNotNull(nameof(_enumerator))]
	protected void EnsureNotDisposed()
	{
		if (_enumerator == null)
			throw new ObjectDisposedException(GetType().FullName);
	}

	/// <summary>
	/// Disposes this object and enumerator with which is was
	/// initialized.
	/// </summary>

	public virtual async ValueTask DisposeAsync()
	{
		var e = _enumerator;
		if (e == null) return;
		_enumerator = null;
		await e.DisposeAsync();
	}
}
