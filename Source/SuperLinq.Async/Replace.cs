namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Replaces a single value in a sequence at a specified index with the given replacement value.
	/// </summary>
	/// <typeparam name="TSource">Type of item in the sequence</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="index">The index of the value to replace.</param>
	/// <param name="value">The replacement value to use at <paramref name="index"/>.</param>
	/// <returns>
	/// A sequence with the original values from <paramref name="source"/>, except for position <paramref name="index"/>
	/// which has the value <paramref name="value"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <remarks>
	/// This operator evaluates in a deferred and streaming manner.
	/// </remarks>
	public static IAsyncEnumerable<TSource> Replace<TSource>(
		this IAsyncEnumerable<TSource> source,
		int index,
		TSource value)
	{
		ArgumentNullException.ThrowIfNull(source);

		return Core(source, value, index);

		static async IAsyncEnumerable<TSource> Core(
			IAsyncEnumerable<TSource> source, TSource value, int index,
			[EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			var i = 0;
			await foreach (var e in source.WithCancellation(cancellationToken).ConfigureAwait(false))
				yield return i++ == index ? value : e;
		}
	}

	/// <summary>
	/// Replaces a single value in a sequence at a specified index with the given replacement value.
	/// </summary>
	/// <typeparam name="TSource">Type of item in the sequence</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="index">The index of the value to replace.</param>
	/// <param name="value">The replacement value to use at <paramref name="index"/>.</param>
	/// <returns>
	/// A sequence with the original values from <paramref name="source"/>, except for position <paramref name="index"/>
	/// which has the value <paramref name="value"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <remarks>
	/// This operator evaluates in a deferred and streaming manner.
	/// </remarks>
#if NETCOREAPP
	public static IAsyncEnumerable<TSource> Replace<TSource>(
#else
	internal static IAsyncEnumerable<TSource> Replace<TSource>(
#endif
		this IAsyncEnumerable<TSource> source,
		Index index,
		TSource value)
	{
		ArgumentNullException.ThrowIfNull(source);

		return Core(source, value, index);

		static async IAsyncEnumerable<TSource> Core(
			IAsyncEnumerable<TSource> source, TSource value, Index index,
			[EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			if (!index.IsFromEnd)
			{
				var cnt = index.Value;
				var i = 0;
				await foreach (var e in source.WithCancellation(cancellationToken).ConfigureAwait(false))
					yield return i++ == cnt ? value : e;
			}
			else
			{
				var cnt = index.Value + 1;
				var queue = new Queue<TSource>();

				await foreach (var e in source.WithCancellation(cancellationToken).ConfigureAwait(false))
				{
					queue.Enqueue(e);
					if (queue.Count > cnt)
						yield return queue.Dequeue();
				}

				if (queue.Count == 0)
					yield break;

				if (queue.Count == cnt)
				{
					yield return value;
					_ = queue.Dequeue();
				}

				while (queue.Count != 0)
					yield return queue.Dequeue();
			}
		}
	}
}
