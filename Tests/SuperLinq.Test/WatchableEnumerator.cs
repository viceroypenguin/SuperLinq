using System.Collections;

namespace Test;

partial class TestExtensions
{
	public static WatchableEnumerator<T> AsWatchable<T>(this IEnumerator<T> source) =>
		new WatchableEnumerator<T>(source);
}

sealed class WatchableEnumerator<T> : IEnumerator<T>
{
	readonly IEnumerator<T> _source;

	public event EventHandler Disposed;
	public event EventHandler<bool> MoveNextCalled;

	public WatchableEnumerator(IEnumerator<T> source) =>
		_source = source ?? throw new ArgumentNullException(nameof(source));

	public T Current => _source.Current;
	object IEnumerator.Current => Current;
	public void Reset() => _source.Reset();

	public bool MoveNext()
	{
		var moved = _source.MoveNext();
		MoveNextCalled?.Invoke(this, moved);
		return moved;
	}

	public void Dispose()
	{
		_source.Dispose();
		Disposed?.Invoke(this, EventArgs.Empty);
	}
}
