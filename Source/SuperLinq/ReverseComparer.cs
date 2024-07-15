namespace SuperLinq;

internal sealed class ReverseComparer<T>(
	IComparer<T> underlying
) : IComparer<T>
{
	public int Compare(T? x, T? y) =>
		-underlying.Compare(x!, y!);
}
