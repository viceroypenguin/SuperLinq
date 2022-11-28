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
	public static ValueTask<(IAsyncEnumerable<T> True, IAsyncEnumerable<T> False)>
		Partition<T>(this IAsyncEnumerable<T> source, Func<T, bool> predicate, CancellationToken cancellationToken = default)
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
		Func<IAsyncEnumerable<T>, IAsyncEnumerable<T>, TResult> resultSelector,
		CancellationToken cancellationToken = default)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(predicate);
		Guard.IsNotNull(resultSelector);

		return source.GroupBy(predicate).Partition(resultSelector, cancellationToken: cancellationToken);
	}

	/// <summary>
	/// Partitions a grouping by Boolean keys into a projection of true
	/// elements and false elements, respectively.
	/// </summary>
	/// <typeparam name="T">Type of elements in source groupings.</typeparam>
	/// <typeparam name="TResult">Type of the result.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="resultSelector">
	/// Function that projects the result from sequences of true elements
	/// and false elements, respectively, passed as arguments.
	/// </param>
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
	/// <returns>
	/// The return value from <paramref name="resultSelector"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
	/// <exception cref="ArgumentNullException"><paramref name="resultSelector"/> is null</exception>
	public static ValueTask<TResult> Partition<T, TResult>(
		this IAsyncEnumerable<IAsyncGrouping<bool, T>> source,
		Func<IAsyncEnumerable<T>, IAsyncEnumerable<T>, TResult> resultSelector,
		CancellationToken cancellationToken = default)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(resultSelector);

		return source.Partition(
			key1: true, key2: false,
			(t, f, _) => resultSelector(t, f),
			cancellationToken: cancellationToken);
	}

	/// <summary>
	/// Partitions a grouping by nullable Boolean keys into a projection of
	/// true elements, false elements and null elements, respectively.
	/// </summary>
	/// <typeparam name="T">Type of elements in source groupings.</typeparam>
	/// <typeparam name="TResult">Type of the result.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="resultSelector">
	/// Function that projects the result from sequences of true elements,
	/// false elements and null elements, respectively, passed as
	/// arguments.
	/// </param>
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
	/// <returns>
	/// The return value from <paramref name="resultSelector"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
	/// <exception cref="ArgumentNullException"><paramref name="resultSelector"/> is null</exception>
	public static ValueTask<TResult> Partition<T, TResult>(
		this IAsyncEnumerable<IAsyncGrouping<bool?, T>> source,
		Func<IAsyncEnumerable<T>, IAsyncEnumerable<T>, IAsyncEnumerable<T>, TResult> resultSelector,
		CancellationToken cancellationToken = default)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(resultSelector);

		return source.Partition(
			key1: true, key2: false, key3: null,
			(t, f, n, _) => resultSelector(t, f, n),
			cancellationToken: cancellationToken);
	}

	/// <summary>
	/// Partitions a grouping and projects a result from group elements
	/// matching a key and those groups that do not.
	/// </summary>
	/// <typeparam name="TKey">Type of keys in source groupings.</typeparam>
	/// <typeparam name="TElement">Type of elements in source groupings.</typeparam>
	/// <typeparam name="TResult">Type of the result.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="key">The key to partition.</param>
	/// <param name="resultSelector">
	/// Function that projects the result from sequences of elements
	/// matching <paramref name="key"/> and those groups that do not (in
	/// the order in which they appear in <paramref name="source"/>),
	/// passed as arguments.
	/// </param>
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
	/// <returns>
	/// The return value from <paramref name="resultSelector"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
	/// <exception cref="ArgumentNullException"><paramref name="resultSelector"/> is null</exception>
	public static ValueTask<TResult> Partition<TKey, TElement, TResult>(
		this IAsyncEnumerable<IAsyncGrouping<TKey, TElement>> source,
		TKey key,
		Func<IAsyncEnumerable<TElement>, IAsyncEnumerable<IAsyncGrouping<TKey, TElement>>, TResult> resultSelector,
		CancellationToken cancellationToken = default)
	{
		return Partition(source, key, comparer: default, resultSelector, cancellationToken: cancellationToken);
	}

	/// <summary>
	/// Partitions a grouping and projects a result from group elements
	/// matching a key and those groups that do not. An additional parameter
	/// specifies how to compare keys for equality.
	/// </summary>
	/// <typeparam name="TKey">Type of keys in source groupings.</typeparam>
	/// <typeparam name="TElement">Type of elements in source groupings.</typeparam>
	/// <typeparam name="TResult">Type of the result.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="key">The key to partition on.</param>
	/// <param name="comparer">The comparer for keys.</param>
	/// <param name="resultSelector">
	/// Function that projects the result from elements of the group
	/// matching <paramref name="key"/> and those groups that do not (in
	/// the order in which they appear in <paramref name="source"/>),
	/// passed as arguments.
	/// </param>
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
	/// <returns>
	/// The return value from <paramref name="resultSelector"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
	/// <exception cref="ArgumentNullException"><paramref name="resultSelector"/> is null</exception>
	public static ValueTask<TResult> Partition<TKey, TElement, TResult>(
		this IAsyncEnumerable<IAsyncGrouping<TKey, TElement>> source,
		TKey key, IEqualityComparer<TKey>? comparer,
		Func<IAsyncEnumerable<TElement>, IAsyncEnumerable<IAsyncGrouping<TKey, TElement>>, TResult> resultSelector,
		CancellationToken cancellationToken = default)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(resultSelector);

		return PartitionImpl(
			source, 1, key, key2: default, key3: default, comparer,
			(a, _, _, rest) => resultSelector(a, rest),
			cancellationToken);
	}

	/// <summary>
	/// Partitions a grouping and projects a result from elements of
	/// groups matching a set of two keys and those groups that do not.
	/// </summary>
	/// <typeparam name="TKey">Type of keys in source groupings.</typeparam>
	/// <typeparam name="TElement">Type of elements in source groupings.</typeparam>
	/// <typeparam name="TResult">Type of the result.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="key1">The first key to partition on.</param>
	/// <param name="key2">The second key to partition on.</param>
	/// <param name="resultSelector">
	/// Function that projects the result from elements of the group
	/// matching <paramref name="key1"/>, elements of the group matching
	/// <paramref name="key2"/> and those groups that do not (in the order
	/// in which they appear in <paramref name="source"/>), passed as
	/// arguments.
	/// </param>
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
	/// <returns>
	/// The return value from <paramref name="resultSelector"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
	/// <exception cref="ArgumentNullException"><paramref name="resultSelector"/> is null</exception>
	public static ValueTask<TResult> Partition<TKey, TElement, TResult>(
		this IAsyncEnumerable<IAsyncGrouping<TKey, TElement>> source,
		TKey key1, TKey key2,
		Func<IAsyncEnumerable<TElement>, IAsyncEnumerable<TElement>, IAsyncEnumerable<IAsyncGrouping<TKey, TElement>>, TResult> resultSelector,
		CancellationToken cancellationToken = default)
	{
		return Partition(source, key1, key2, comparer: default, resultSelector, cancellationToken: cancellationToken);
	}

	/// <summary>
	/// Partitions a grouping and projects a result from elements of
	/// groups matching a set of two keys and those groups that do not.
	/// An additional parameter specifies how to compare keys for equality.
	/// </summary>
	/// <typeparam name="TKey">Type of keys in source groupings.</typeparam>
	/// <typeparam name="TElement">Type of elements in source groupings.</typeparam>
	/// <typeparam name="TResult">Type of the result.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="key1">The first key to partition on.</param>
	/// <param name="key2">The second key to partition on.</param>
	/// <param name="comparer">The comparer for keys.</param>
	/// <param name="resultSelector">
	/// Function that projects the result from elements of the group
	/// matching <paramref name="key1"/>, elements of the group matching
	/// <paramref name="key2"/> and those groups that do not (in the order
	/// in which they appear in <paramref name="source"/>), passed as
	/// arguments.
	/// </param>
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
	/// <returns>
	/// The return value from <paramref name="resultSelector"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
	/// <exception cref="ArgumentNullException"><paramref name="resultSelector"/> is null</exception>
	public static ValueTask<TResult> Partition<TKey, TElement, TResult>(
		this IAsyncEnumerable<IAsyncGrouping<TKey, TElement>> source,
		TKey key1, TKey key2, IEqualityComparer<TKey>? comparer,
		Func<IAsyncEnumerable<TElement>, IAsyncEnumerable<TElement>, IAsyncEnumerable<IAsyncGrouping<TKey, TElement>>, TResult> resultSelector,
		CancellationToken cancellationToken = default)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(resultSelector);

		return PartitionImpl(
			source, 2, key1, key2, key3: default, comparer,
			(a, b, c, rest) => resultSelector(a, b, rest),
			cancellationToken);
	}

	/// <summary>
	/// Partitions a grouping and projects a result from elements groups
	/// matching a set of three keys and those groups that do not.
	/// </summary>
	/// <typeparam name="TKey">Type of keys in source groupings.</typeparam>
	/// <typeparam name="TElement">Type of elements in source groupings.</typeparam>
	/// <typeparam name="TResult">Type of the result.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="key1">The first key to partition on.</param>
	/// <param name="key2">The second key to partition on.</param>
	/// <param name="key3">The third key to partition on.</param>
	/// <param name="resultSelector">
	/// Function that projects the result from elements of groups
	/// matching <paramref name="key1"/>, <paramref name="key2"/> and
	/// <paramref name="key3"/> and those groups that do not (in the order
	/// in which they appear in <paramref name="source"/>), passed as
	/// arguments.
	/// </param>
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
	/// <returns>
	/// The return value from <paramref name="resultSelector"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
	/// <exception cref="ArgumentNullException"><paramref name="resultSelector"/> is null</exception>
	public static ValueTask<TResult> Partition<TKey, TElement, TResult>(
		this IAsyncEnumerable<IAsyncGrouping<TKey, TElement>> source,
		TKey key1, TKey key2, TKey key3,
		Func<IAsyncEnumerable<TElement>, IAsyncEnumerable<TElement>, IAsyncEnumerable<TElement>, IAsyncEnumerable<IAsyncGrouping<TKey, TElement>>, TResult> resultSelector,
		CancellationToken cancellationToken = default)
	{
		return Partition(source, key1, key2, key3, comparer: default, resultSelector, cancellationToken: cancellationToken);
	}

	/// <summary>
	/// Partitions a grouping and projects a result from elements groups
	/// matching a set of three keys and those groups that do not. An
	/// additional parameter specifies how to compare keys for equality.
	/// </summary>
	/// <typeparam name="TKey">Type of keys in source groupings.</typeparam>
	/// <typeparam name="TElement">Type of elements in source groupings.</typeparam>
	/// <typeparam name="TResult">Type of the result.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="key1">The first key to partition on.</param>
	/// <param name="key2">The second key to partition on.</param>
	/// <param name="key3">The third key to partition on.</param>
	/// <param name="comparer">The comparer for keys.</param>
	/// <param name="resultSelector">
	/// Function that projects the result from elements of groups
	/// matching <paramref name="key1"/>, <paramref name="key2"/> and
	/// <paramref name="key3"/> and those groups that do not (in
	/// the order in which they appear in <paramref name="source"/>),
	/// passed as arguments.
	/// </param>
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
	/// <returns>
	/// The return value from <paramref name="resultSelector"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
	/// <exception cref="ArgumentNullException"><paramref name="resultSelector"/> is null</exception>
	public static ValueTask<TResult> Partition<TKey, TElement, TResult>(
		this IAsyncEnumerable<IAsyncGrouping<TKey, TElement>> source,
		TKey key1, TKey key2, TKey key3, IEqualityComparer<TKey>? comparer,
		Func<IAsyncEnumerable<TElement>, IAsyncEnumerable<TElement>, IAsyncEnumerable<TElement>, IAsyncEnumerable<IAsyncGrouping<TKey, TElement>>, TResult> resultSelector,
		CancellationToken cancellationToken = default)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(resultSelector);

		return PartitionImpl(source, 3, key1, key2, key3, comparer, resultSelector, cancellationToken);
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
