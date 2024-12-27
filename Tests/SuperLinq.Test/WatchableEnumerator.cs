using System.Collections;

namespace Test;

internal partial class TestExtensions
{
	public static WatchableEnumerator<T> AsWatchable<T>(this IEnumerator<T> source) => new(source);
}

internal sealed class WatchableEnumerator<T> : IEnumerator<T>
{
	private readonly IEnumerator<T> _source;

	public event EventHandler? Disposed;
	public event EventHandler? GetCurrentCalled;
	public event EventHandler<bool>? MoveNextCalled;

	public WatchableEnumerator(IEnumerator<T> source)
	{
		Assert.NotNull(source);
		_source = source;
	}

	public T Current
	{
		get
		{
			GetCurrentCalled?.Invoke(this, EventArgs.Empty);
			return _source.Current;
		}
	}

	object? IEnumerator.Current => Current;
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
