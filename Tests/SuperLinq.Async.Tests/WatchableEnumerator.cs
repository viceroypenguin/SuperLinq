namespace SuperLinq.Async.Tests;

internal sealed class WatchableEnumerator<T> : IAsyncEnumerator<T>
{
	private readonly IAsyncEnumerator<T> _source;

	public event EventHandler? Disposed;
	public event EventHandler? GetCurrentCalled;
	public event EventHandler<bool>? MoveNextCalled;

	public WatchableEnumerator(IAsyncEnumerator<T> source)
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

	public async ValueTask<bool> MoveNextAsync()
	{
		// force async behavior to be tested
		await Task.Yield();

		var moved = await _source.MoveNextAsync();
		MoveNextCalled?.Invoke(this, moved);
		return moved;
	}

	public async ValueTask DisposeAsync()
	{
		// force async behavior to be tested
		await Task.Yield();

		await _source.DisposeAsync();
		Disposed?.Invoke(this, EventArgs.Empty);
	}
}
