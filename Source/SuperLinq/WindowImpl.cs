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

	private static IEnumerable<TResult> WindowImpl<TSource, TResult>(
		IEnumerable<TSource> source,
		TSource[] array,
		int size,
		WindowType type,
		Func<IReadOnlyList<TSource>, TResult> projector)
	{
		using var iter = source.GetEnumerator();

		var n = 0;
		while (n < size && iter.MoveNext())
		{
			array[n++] = iter.Current;
			if (type == WindowType.Right)
				yield return projector(new ArraySegment<TSource>(array, 0, n));
		}

		if (type == WindowType.Normal && n == size)
			yield return projector(new ArraySegment<TSource>(array));
		else if (type == WindowType.Left && n > 0)
			yield return projector(new ArraySegment<TSource>(array, 0, n));

		while (iter.MoveNext())
		{
			array.AsSpan()[1..].CopyTo(array);
			array[^1] = iter.Current;

			yield return projector(new ArraySegment<TSource>(array));
		}

		if (type != WindowType.Left)
			yield break;

		for (var j = 1; j < n; j++)
		{
			yield return projector(new ArraySegment<TSource>(array, j, n - j));
		}
	}
}
