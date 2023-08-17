namespace SuperLinq;

public static partial class SuperEnumerable
{
	private enum WindowType { Normal, Left, Right, }
	private static IEnumerable<TResult> WindowImpl<TSource, TResult>(
		IEnumerable<TSource> source,
		TSource[] array,
		int size,
		WindowType type,
		Func<ArraySegment<TSource>, TResult> projector)
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
