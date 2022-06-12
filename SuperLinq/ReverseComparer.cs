namespace SuperLinq;

sealed class ReverseComparer<T> : IComparer<T>
{
	readonly IComparer<T> _underlying;

	public ReverseComparer(IComparer<T>? underlying)
	{
		_underlying = underlying ?? Comparer<T>.Default;
	}

#if NETSTANDARD2_0
	public int Compare(T x, T y)
#else
	public int Compare(T? x, T? y)
#endif
	{
		var result = _underlying.Compare(x, y);
		return result < 0 ? 1 : result > 0 ? -1 : 0;
	}
}
