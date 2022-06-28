namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	private enum WindowType { Normal, Left, Right, }
	private static async IAsyncEnumerable<IList<TSource>> WindowImpl<TSource>(IAsyncEnumerable<TSource> source, int size, WindowType type, [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		await using var iter = source.GetConfiguredAsyncEnumerator(cancellationToken);

		if (!await iter.MoveNextAsync())
			yield break;

		var window = new TSource[1] { iter.Current };
		while (window.Length < size && await iter.MoveNextAsync())
		{
			var newWindow = new TSource[window.Length + 1];
			window.AsSpan().CopyTo(newWindow);
			if (type == WindowType.Right)
				yield return window;
			window = newWindow;

			window[^1] = iter.Current;
		}

		while (await iter.MoveNextAsync())
		{
			var newWindow = new TSource[size];
			window.AsSpan()[1..].CopyTo(newWindow);
			yield return window;
			window = newWindow;

			window[^1] = iter.Current;
		}

		if (type == WindowType.Normal && window.Length < size)
			yield break;
		if (type != WindowType.Left)
		{
			yield return window;
			yield break;
		}

		while (window.Length > 0)
		{
			var newWindow = new TSource[window.Length - 1];
			window.AsSpan()[1..].CopyTo(newWindow);
			yield return window;
			window = newWindow;
		}
	}
}
