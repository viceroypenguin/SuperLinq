namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Creates a left-aligned sliding window of a given size over the
	/// source sequence.
	/// </summary>
	/// <typeparam name="TSource">
	/// The type of the elements of <paramref name="source"/>.</typeparam>
	/// <param name="source">
	/// The sequence over which to create the sliding window.</param>
	/// <param name="size">Size of the sliding window.</param>
	/// <returns>A sequence representing each sliding window.</returns>
	/// <remarks>
	/// <para>
	/// A window can contain fewer elements than <paramref name="size"/>,
	/// especially as it slides over the end of the sequence.</para>
	/// <para>
	/// This operator uses deferred execution and streams its results.</para>
	/// </remarks>
	/// <example>
	/// <code><![CDATA[
	/// Console.WriteLine(
	///     Enumerable
	///         .Range(1, 5)
	///         .WindowLeft(3)
	///         .Select(w => "AVG(" + w.ToDelimitedString(",") + ") = " + w.Average())
	///         .ToDelimitedString(Environment.NewLine));
	///
	/// // Output:
	/// // AVG(1,2,3) = 2
	/// // AVG(2,3,4) = 3
	/// // AVG(3,4,5) = 4
	/// // AVG(4,5) = 4.5
	/// // AVG(5) = 5
	/// ]]></code>
	/// </example>
	public static IAsyncEnumerable<IList<TSource>> WindowLeft<TSource>(this IAsyncEnumerable<TSource> source, int size)
	{
		ArgumentNullException.ThrowIfNull(source);
		Guard.IsGreaterThanOrEqualTo(size, 1);

		return Core(source, size);

		static async IAsyncEnumerable<IList<TSource>> Core(
			IAsyncEnumerable<TSource> source, int size,
			[EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			await using var e = source.GetConfiguredAsyncEnumerator(cancellationToken);
			if (!await e.MoveNextAsync())
				yield break;

			var window = new TSource[size];
			window[0] = e.Current;

			for (var i = 1; i < size; i++)
			{
				if (!await e.MoveNextAsync())
				{
					Array.Resize(ref window, i);
					goto skipLoop;
				}
				window[i] = e.Current;
			}

			while (await e.MoveNextAsync())
			{
				var newWindow = new TSource[size];
				window.AsSpan()[1..].CopyTo(newWindow);
				newWindow[^1] = e.Current;

				yield return window;
				window = newWindow;
			}

skipLoop:
			while (window.Length > 1)
			{
				var newWindow = new TSource[window.Length - 1];
				window.AsSpan()[1..].CopyTo(newWindow);

				yield return window;
				window = newWindow;
			}

			yield return window;
		}
	}
}
