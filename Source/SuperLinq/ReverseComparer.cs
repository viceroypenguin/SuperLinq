namespace SuperLinq;

internal sealed class ReverseComparer<T> : IComparer<T>
{
	private readonly IComparer<T> _underlying;

	public ReverseComparer(IComparer<T>? underlying)
	{
		_underlying = underlying ?? Comparer<T>.Default;
	}

	public int Compare(T? x, T? y) =>
		-_underlying.Compare(x, y);
}
