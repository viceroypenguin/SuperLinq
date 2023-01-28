namespace SuperLinq;

public static partial class SuperEnumerable
{
	private enum WindowType { Normal, Left, Right, }
	private static IEnumerable<IList<TSource>> WindowImpl<TSource>(IEnumerable<TSource> source, int size, WindowType type)
	{
		var window = Array.Empty<TSource>();

		foreach (var i in source)
		{
			if (window.Length < size)
			{
				var newWindow = new TSource[window.Length + 1];
				window.CopyTo(newWindow, 0);

				if (window.Length > 0
					&& type == WindowType.Right)
				{
					yield return window;
				}

				window = newWindow;
				window[^1] = i;
			}
			else
			{
				var newWindow = new TSource[size];
				window.AsSpan()[1..].CopyTo(newWindow);
				yield return window;

				window = newWindow;
				window[^1] = i;
			}
		}

		// foreach will always return one loop behind, so if necessary, return the last value
		switch (type)
		{
			case WindowType.Normal:
			{
				if (window.Length == size)
					yield return window;
				yield break;
			}

			case WindowType.Right:
			{
				if (window.Length > 0)
					yield return window;
				yield break;
			}
		}

		while (window.Length > 1)
		{
			var newWindow = new TSource[window.Length - 1];
			window.AsSpan()[1..].CopyTo(newWindow);
			yield return window;
			window = newWindow;
		}

		// intentionally break out length == 1 to remove new TSource[0] allocation 
		if (window.Length > 0)
			yield return window;
	}

	private static IEnumerable<TResult> WindowImpl<TSource, TResult>(
		IEnumerable<TSource> source,
		TSource[] array,
		int size,
		WindowType type,
		Func<IReadOnlyList<TSource>, TResult> projector)
	{
		var n = 0;

		foreach (var i in source)
		{
			if (n < size)
			{
				array[n++] = i;
				if (type == WindowType.Right || n == size)
					yield return projector(new ArraySegment<TSource>(array, 0, n));
			}
			else
			{
				array.AsSpan()[1..].CopyTo(array);
				array[^1] = i;

				yield return projector(new ArraySegment<TSource>(array));
			}
		}

		if (type != WindowType.Left)
			yield break;

		for (var j = n == size ? 1 : 0; j < n; j++)
		{
			yield return projector(new ArraySegment<TSource>(array, j, n - j));
		}
	}
}
