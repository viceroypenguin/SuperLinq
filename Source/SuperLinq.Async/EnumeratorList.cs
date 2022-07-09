namespace SuperLinq.Async;

internal class EnumeratorList<T> : IAsyncDisposable
{
	private readonly List<ConfiguredCancelableAsyncEnumerable<T>.Enumerator> _iter;

	internal static async ValueTask<EnumeratorList<T>> Create(IEnumerable<IAsyncEnumerable<T>> sources, CancellationToken cancellationToken)
	{
		var list = new List<ConfiguredCancelableAsyncEnumerable<T>.Enumerator>();
		try
		{
			foreach (var source in sources)
				list.Add(source.GetConfiguredAsyncEnumerator(cancellationToken));
			return new(list);
		}
		catch
		{
			foreach (var e in list)
				await e.DisposeAsync();

			list.Clear();

			throw;
		}
	}

	public EnumeratorList(List<ConfiguredCancelableAsyncEnumerable<T>.Enumerator> iter)
	{
		_iter = iter;
	}

	public int Count => _iter.Count;

	public bool Any() => _iter.Count != 0;

	public async ValueTask<bool> MoveNext(int i)
	{
		while (i < _iter.Count)
		{
			var e = _iter[i];
			if (await e.MoveNextAsync())
				return true;

			_iter.RemoveAt(i);
			await e.DisposeAsync();
		}

		return false;
	}

	public async ValueTask<bool> MoveNextOnce(int i)
	{
		if (i < _iter.Count)
		{
			var e = _iter[i];
			if (await e.MoveNextAsync())
				return true;

			_iter.RemoveAt(i);
			await e.DisposeAsync();
		}

		return false;
	}

	public T Current(int i) =>
		_iter[i].Current;

	public async ValueTask DisposeAsync()
	{
		foreach (var e in _iter)
			await e.DisposeAsync();
	}
}
