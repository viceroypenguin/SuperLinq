namespace SuperLinq;

internal class EnumeratorList<T> : IDisposable
{
	private readonly List<IEnumerator<T>> _iter = new();

	public EnumeratorList(IEnumerable<IEnumerable<T>> sources)
	{
		try
		{
			foreach (var source in sources)
				_iter.Add(source.GetEnumerator());
		}
		catch
		{
			foreach (var e in _iter)
				e.Dispose();

			_iter.Clear();

			throw;
		}
	}

	public int Count => _iter.Count;

	public bool Any() => _iter.Count != 0;

	public bool MoveNext(int i)
	{
		while (i < _iter.Count)
		{
			var e = _iter[i];
			if (e.MoveNext())
				return true;

			_iter.RemoveAt(i);
			e.Dispose();
		}

		return false;
	}

	public bool MoveNextOnce(int i)
	{
		if (i < _iter.Count)
		{
			var e = _iter[i];
			if (e.MoveNext())
				return true;

			_iter.RemoveAt(i);
			e.Dispose();
		}

		return false;
	}

	public T Current(int i) =>
		_iter[i].Current;

	public void Dispose()
	{
		foreach (var e in _iter)
			e.Dispose();
	}
}
