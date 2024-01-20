namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Partitions or splits a sequence in two using a predicate.
	/// </summary>
	/// <param name="source">The source sequence.</param>
	/// <param name="predicate">The predicate function.</param>
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
	/// <typeparam name="T">Type of source elements.</typeparam>
	/// <returns>
	/// A tuple of elements satisfying the predicate and those that do not,
	/// respectively.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
	/// <example>
	/// <code><![CDATA[
	/// var (evens, odds) =
	///     Enumerable.Range(0, 10).Partition(x => x % 2 == 0);
	/// ]]></code>
	/// The <c>evens</c> variable, when iterated over, will yield 0, 2, 4, 6
	/// and then 8. The <c>odds</c> variable, when iterated over, will yield
	/// 1, 3, 5, 7 and then 9.
	/// </example>
	public static ValueTask<(IEnumerable<T> True, IEnumerable<T> False)> Partition<T>(
		this IAsyncEnumerable<T> source,
		Func<T, bool> predicate,
		CancellationToken cancellationToken = default)
	{
		return source.Partition(predicate, ValueTuple.Create, cancellationToken: cancellationToken);
	}

	/// <summary>
	/// Partitions or splits a sequence in two using a predicate and then
	/// projects a result from the two.
	/// </summary>
	/// <param name="source">The source sequence.</param>
	/// <param name="predicate">The predicate function.</param>
	/// <param name="resultSelector">
	/// Function that projects the result from sequences of elements that
	/// satisfy the predicate and those that do not, respectively, passed as
	/// arguments.
	/// </param>
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
	/// <typeparam name="T">Type of source elements.</typeparam>
	/// <typeparam name="TResult">Type of the result.</typeparam>
	/// <returns>
	/// The return value from <paramref name="resultSelector"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
	/// <exception cref="ArgumentNullException"><paramref name="predicate"/> is null</exception>
	/// <exception cref="ArgumentNullException"><paramref name="resultSelector"/> is null</exception>
	/// <example>
	/// <code><![CDATA[
	/// var (evens, odds) =
	///     Enumerable.Range(0, 10)
	///               .Partition(x => x % 2 == 0, ValueTuple.Create);
	/// ]]></code>
	/// The <c>evens</c> variable, when iterated over, will yield 0, 2, 4, 6
	/// and then 8. The <c>odds</c> variable, when iterated over, will yield
	/// 1, 3, 5, 7 and then 9.
	/// </example>
	public static ValueTask<TResult> Partition<T, TResult>(
		this IAsyncEnumerable<T> source,
		Func<T, bool> predicate,
		Func<IEnumerable<T>, IEnumerable<T>, TResult> resultSelector,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(predicate);
		ArgumentNullException.ThrowIfNull(resultSelector);

		return Core(source, predicate, resultSelector, cancellationToken);

		static async ValueTask<TResult> Core(
			IAsyncEnumerable<T> source,
			Func<T, bool> predicate,
			Func<IEnumerable<T>, IEnumerable<T>, TResult> resultSelector,
			CancellationToken cancellationToken)
		{
			var lookup = await source.ToLookupAsync(predicate, cancellationToken).ConfigureAwait(false);
			return resultSelector(lookup[true], lookup[false]);
		}
	}

	private static async ValueTask<TResult> PartitionImpl<TKey, TElement, TResult>(
		IAsyncEnumerable<IAsyncGrouping<TKey, TElement>> source,
		int count, TKey? key1, TKey? key2, TKey? key3, IEqualityComparer<TKey>? comparer,
		Func<IAsyncEnumerable<TElement>, IAsyncEnumerable<TElement>, IAsyncEnumerable<TElement>, IAsyncEnumerable<IAsyncGrouping<TKey, TElement>>, TResult> resultSelector,
		CancellationToken cancellationToken)
	{
		comparer ??= EqualityComparer<TKey>.Default;

		List<IAsyncGrouping<TKey, TElement>>? etc = null;

		var groups = new[]
		{
			AsyncEnumerable.Empty<TElement>(),
			AsyncEnumerable.Empty<TElement>(),
			AsyncEnumerable.Empty<TElement>(),
		};

		await foreach (var e in source.WithCancellation(cancellationToken).ConfigureAwait(false))
		{
			var i = count > 0 && comparer.Equals(e.Key, key1) ? 0
				  : count > 1 && comparer.Equals(e.Key, key2) ? 1
				  : count > 2 && comparer.Equals(e.Key, key3) ? 2
				  : -1;

			if (i < 0)
			{
				etc ??= new List<IAsyncGrouping<TKey, TElement>>();
				etc.Add(e);
			}
			else
			{
				groups[i] = e;
			}
		}

		return resultSelector(groups[0], groups[1], groups[2], etc?.ToAsyncEnumerable() ?? AsyncEnumerable.Empty<IAsyncGrouping<TKey, TElement>>());
	}
}
