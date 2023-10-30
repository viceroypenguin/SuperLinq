namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Filters a sequence of values based on a predicate evaluated on the current value and a lagging value.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
	/// <param name="source">An <see cref="IEnumerable{T}"/> to filter.</param>
	/// <param name="offset">The offset (expressed as a positive number) by which to lag each element of the
	/// sequence.</param>
	/// <param name="predicate">A function which accepts the current and lagged element (in that order) to
	/// test for a condition.</param>
	/// <returns>An <see cref="IEnumerable{T}"/> that contains elements from the input sequence that satisfy the
	/// condition.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="predicate"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is below 1.</exception>
	/// <remarks>
	/// <para>
	/// For elements of the sequence that are less than <paramref name="offset"/> items from the end, <see
	/// langword="default"/>(<typeparamref name="TSource"/>?) is used as the Lag value.
	/// </para>
	/// <para>
	/// This operator evaluates in a deferred and streaming manner.
	/// </para>
	/// </remarks>
	public static IEnumerable<TSource> WhereLag<TSource>(this IEnumerable<TSource> source, int offset, Func<TSource, TSource?, bool> predicate)
	{
		return source.WhereLag(offset, default!, predicate);
	}

	/// <summary>
	/// Filters a sequence of values based on a predicate evaluated on the current value and a lagging value.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
	/// <param name="source">An <see cref="IEnumerable{T}"/> to filter.</param>
	/// <param name="offset">The offset (expressed as a positive number) by which to lag each element of the
	/// sequence.</param>
	/// <param name="defaultLagValue">A default value supplied for the lagged element when none is available</param>
	/// <param name="predicate">A function which accepts the current and lagged element (in that order) to
	/// test for a condition.</param>
	/// <returns>An <see cref="IEnumerable{T}"/> that contains elements from the input sequence that satisfy the
	/// condition.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="predicate"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is below 1.</exception>
	/// <remarks>
	/// <para>
	/// This operator evaluates in a deferred and streaming manner.
	/// </para>
	/// </remarks>
	public static IEnumerable<TSource> WhereLag<TSource>(this IEnumerable<TSource> source, int offset, TSource defaultLagValue, Func<TSource, TSource, bool> predicate)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(predicate);
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(offset);

		return Core(source, offset, defaultLagValue, predicate);

		static IEnumerable<TSource> Core(IEnumerable<TSource> source, int offset, TSource defaultLagValue, Func<TSource, TSource, bool> predicate)
		{
			var lagQueue = new Queue<TSource>(offset + 1);
			foreach (var item in source)
			{
				lagQueue.Enqueue(item);
				var deq = lagQueue.Count > offset ? lagQueue.Dequeue() : defaultLagValue;
				if (predicate(item, deq))
					yield return item;
			}
		}
	}
}
