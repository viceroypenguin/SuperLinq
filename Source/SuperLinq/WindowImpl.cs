namespace SuperLinq;

public static partial class SuperEnumerable
{
	private enum WindowType { Normal, Left, Right, }
	private static IEnumerable<IList<TSource>> WindowImpl<TSource>(IEnumerable<TSource> source, int size, WindowType type)
	{
		using var iter = source.GetEnumerator();

		if (!iter.MoveNext())
			yield break;

		var window = new TSource[1] { iter.Current };
		while (window.Length < size && iter.MoveNext())
		{
			var newWindow = new TSource[window.Length + 1];
			window.AsSpan().CopyTo(newWindow);
			if (type == WindowType.Right)
				yield return window;
			window = newWindow;

			window[^1] = iter.Current;
		}

		while (iter.MoveNext())
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
