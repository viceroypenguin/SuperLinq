using System.Diagnostics;

namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	private enum WindowType { Normal, Left, Right, }
	private static async IAsyncEnumerable<IList<TSource>> WindowImpl<TSource>(IAsyncEnumerable<TSource> source, int size, WindowType type, [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var window = Array.Empty<TSource>();

		await foreach (var i in source.WithCancellation(cancellationToken).ConfigureAwait(false))
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

			case WindowType.Left:
			{
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
				yield break;
			}

			default:
				throw new UnreachableException();
		}
	}
}
