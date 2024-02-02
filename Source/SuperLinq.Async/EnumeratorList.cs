namespace SuperLinq.Async;

internal sealed class EnumeratorList<T>(
	List<ConfiguredCancelableAsyncEnumerable<T>.Enumerator> iter
) : IAsyncDisposable
{
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

	public int Count => iter.Count;

	public bool Any() => iter.Count != 0;

	public async ValueTask<bool> MoveNext(int i)
	{
		while (i < iter.Count)
		{
			var e = iter[i];
			if (await e.MoveNextAsync())
				return true;

			iter.RemoveAt(i);
			await e.DisposeAsync();
		}

		return false;
	}

	public async ValueTask<bool> MoveNextOnce(int i)
	{
		if (i < iter.Count)
		{
			var e = iter[i];
			if (await e.MoveNextAsync())
				return true;

			iter.RemoveAt(i);
			await e.DisposeAsync();
		}

		return false;
	}

	public T Current(int i) =>
		iter[i].Current;

	public async ValueTask DisposeAsync()
	{
		foreach (var e in iter)
			await e.DisposeAsync();
	}
}
