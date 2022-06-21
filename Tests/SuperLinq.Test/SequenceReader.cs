using System.Diagnostics.CodeAnalysis;

namespace Test;

static class SequenceReader
{
	public static SequenceReader<T> Read<T>(this IEnumerable<T> source)
	{
		if (source == null) throw new ArgumentNullException(nameof(source));
		return new SequenceReader<T>(source);
	}
}

/// <summary>
/// Adds reader semantics to a sequence where <see cref="IEnumerator{T}.MoveNext"/>
/// and <see cref="IEnumerator{T}.Current"/> are rolled into a single
/// "read" operation.
/// </summary>
/// <typeparam name="T">Type of elements to read.</typeparam>
class SequenceReader<T> : IDisposable
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

	public SequenceReader(IEnumerator<T> enumerator) =>
		_enumerator = enumerator ?? throw new ArgumentNullException(nameof(enumerator));

	static IEnumerator<T> GetEnumerator(IEnumerable<T> source)
	{
		if (source == null) throw new ArgumentNullException(nameof(source));
		return source.GetEnumerator();
	}

	/// <summary>
	/// Tires to read the next value.
	/// </summary>
	/// <param name="value">
	/// When this method returns, contains the value read on success.
	/// </param>
	/// <returns>
	/// Returns true if a value was successfully read; otherwise, false.
	/// </returns>

	public virtual bool TryRead([NotNullWhen(true)] out T? value)
	{
		EnsureNotDisposed();

		value = default;

		var e = _enumerator;
		if (!e!.MoveNext())
			return false;

		value = e.Current!;
		return true;
	}

	/// <summary>
	/// Tires to read the next value otherwise return the default.
	/// </summary>

	public T? TryRead() => TryRead(default);

	/// <summary>
	/// Tires to read the next value otherwise return a given default.
	/// </summary>

	public T? TryRead(T? defaultValue) =>
		TryRead(out var result) ? result : defaultValue;

	/// <summary>
	/// Reads a value otherwise throws <see cref="InvalidOperationException"/>
	/// if no more values are available.
	/// </summary>
	/// <returns>
	/// Returns the read value;
	/// </returns>

	public T Read() =>
		TryRead(out var result) ? result : throw new InvalidOperationException();

	/// <summary>
	/// Reads the end. If the end has not been reached then it
	/// throws <see cref="InvalidOperationException"/>.
	/// </summary>

	public virtual void ReadEnd()
	{
		EnsureNotDisposed();

		if (_enumerator.MoveNext())
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

	public virtual void Dispose()
	{
		var e = _enumerator;
		if (e == null) return;
		_enumerator = null;
		e.Dispose();
	}
}
