namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Filters a sequence of values based on a predicate evaluated on the current value and a leading value.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
	/// <param name="source">The sequence over which to evaluate Lead.</param>
	/// <param name="offset">The offset (expressed as a positive number) by which to lead each element of the
	/// sequence.</param>
	/// <param name="predicate">A function which accepts the current and subsequent (lead) element (in that order) to
	/// test for a condition.</param>
	/// <returns>An <see cref="IAsyncEnumerable{T}"/> that contains elements from the input sequence that satisfy the
	/// condition.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="predicate"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is below 1.</exception>
	/// <remarks>
	/// <para>
	/// For elements of the sequence that are less than <paramref name="offset"/> items from the end, <see
	/// langword="default"/>(<typeparamref name="TSource"/>?) is used as the lead value.
	/// </para>
	/// <para>
	/// This operator evaluates in a deferred and streaming manner.
	/// </para>
	/// </remarks>
	public static IAsyncEnumerable<TSource> WhereLead<TSource>(this IAsyncEnumerable<TSource> source, int offset, Func<TSource, TSource?, bool> predicate)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(predicate);
		Guard.IsGreaterThanOrEqualTo(offset, 1);

		return source.WhereLead(offset, default!, predicate.ToAsync());
	}

	/// <summary>
	/// Filters a sequence of values based on a predicate evaluated on the current value and a leading value.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
	/// <param name="source">The sequence over which to evaluate Lead.</param>
	/// <param name="offset">The offset (expressed as a positive number) by which to lead each element of the
	/// sequence.</param>
	/// <param name="predicate">A function which accepts the current and subsequent (lead) element (in that order) to
	/// test for a condition.</param>
	/// <returns>An <see cref="IAsyncEnumerable{T}"/> that contains elements from the input sequence that satisfy the
	/// condition.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="predicate"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is below 1.</exception>
	/// <remarks>
	/// <para>
	/// For elements of the sequence that are less than <paramref name="offset"/> items from the end, <see
	/// langword="default"/>(<typeparamref name="TSource"/>?) is used as the lead value.
	/// </para>
	/// <para>
	/// This operator evaluates in a deferred and streaming manner.
	/// </para>
	/// </remarks>
	public static IAsyncEnumerable<TSource> WhereLead<TSource>(this IAsyncEnumerable<TSource> source, int offset, Func<TSource, TSource?, ValueTask<bool>> predicate)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(predicate);
		Guard.IsGreaterThanOrEqualTo(offset, 1);

		return source.WhereLead(offset, default!, predicate);
	}

	/// <summary>
	/// Filters a sequence of values based on a predicate evaluated on the current value and a leading value.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
	/// <param name="source">The sequence over which to evaluate Lead.</param>
	/// <param name="offset">The offset (expressed as a positive number) by which to lead each element of the
	/// sequence.</param>
	/// <param name="defaultLeadValue">A default value supplied for the leading element when none is available</param>
	/// <param name="predicate">A function which accepts the current and subsequent (lead) element (in that order) to
	/// test for a condition.</param>
	/// <returns>An <see cref="IAsyncEnumerable{T}"/> that contains elements from the input sequence that satisfy the
	/// condition.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="predicate"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is below 1.</exception>
	/// <remarks>
	/// <para>
	/// This operator evaluates in a deferred and streaming manner.
	/// </para>
	/// </remarks>
	public static IAsyncEnumerable<TSource> WhereLead<TSource>(this IAsyncEnumerable<TSource> source, int offset, TSource defaultLeadValue, Func<TSource, TSource, bool> predicate)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(predicate);
		Guard.IsGreaterThanOrEqualTo(offset, 1);

		return source.WhereLead(offset, defaultLeadValue, predicate.ToAsync());
	}

	/// <summary>
	/// Filters a sequence of values based on a predicate evaluated on the current value and a leading value.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
	/// <param name="source">The sequence over which to evaluate Lead.</param>
	/// <param name="offset">The offset (expressed as a positive number) by which to lead each element of the
	/// sequence.</param>
	/// <param name="defaultLeadValue">A default value supplied for the leading element when none is available</param>
	/// <param name="predicate">A function which accepts the current and subsequent (lead) element (in that order) to
	/// test for a condition.</param>
	/// <returns>An <see cref="IAsyncEnumerable{T}"/> that contains elements from the input sequence that satisfy the
	/// condition.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="predicate"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is below 1.</exception>
	/// <remarks>
	/// <para>
	/// This operator evaluates in a deferred and streaming manner.
	/// </para>
	/// </remarks>
	public static IAsyncEnumerable<TSource> WhereLead<TSource>(this IAsyncEnumerable<TSource> source, int offset, TSource defaultLeadValue, Func<TSource, TSource, ValueTask<bool>> predicate)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(predicate);
		Guard.IsGreaterThanOrEqualTo(offset, 1);

		return Core(source, offset, defaultLeadValue, predicate);

		static async IAsyncEnumerable<TSource> Core(
			IAsyncEnumerable<TSource> source, int offset, TSource defaultLeadValue, Func<TSource, TSource, ValueTask<bool>> predicate,
			[EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			var queue = new Queue<TSource>(offset + 1);

			await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
			{
				queue.Enqueue(item);
				if (queue.Count > offset)
				{
					var deq = queue.Dequeue();
					if (await predicate(deq, item).ConfigureAwait(false))
						yield return deq;
				}
			}

			while (queue.Count > 0)
			{
				var deq = queue.Dequeue();
				if (await predicate(deq, defaultLeadValue).ConfigureAwait(false))
					yield return deq;
			}
		}
	}
}
