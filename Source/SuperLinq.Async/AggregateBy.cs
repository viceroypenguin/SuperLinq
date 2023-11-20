namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	///	    Applies a key-generating function to each element of a sequence and returns an aggregate value for each key.
	///     An additional argument specifies a comparer to use for testing equivalence of keys.
	/// </summary>
	/// <typeparam name="TSource">
	///	    Type of the elements of the source sequence.
	/// </typeparam>
	/// <typeparam name="TKey">
	///	    Type of the projected element.
	/// </typeparam>
	/// <typeparam name="TAccumulate">
	///	    Type of the accumulator value.
	/// </typeparam>
	/// <param name="source">
	///	    Source sequence.
	/// </param>
	/// <param name="keySelector">
	///	    Function that transforms each item of source sequence into a key to be compared against the others.
	/// </param>
	/// <param name="seed">
	///	    The initial accumulator value for each key-group.
	/// </param>
	/// <param name="func">
	///	    An accumulator function to be invoked on each element. The accumulator value is tracked separately for each
	///     key.
	/// </param>
	/// <param name="comparer">
	///	    The equality comparer to use to determine whether or not keys are equal. If <see langword="null" />, the
	///     default equality comparer for <typeparamref name="TKey"/> is used.
	/// </param>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/>, <paramref name="keySelector"/>, or <paramref name="func"/> is <see
	///     langword="null" />.
	/// </exception>
	/// <returns>
	///	    A sequence of unique keys and their accumulated value.
	/// </returns>
	/// <remarks>
	/// <para>
	///	    This method is implemented by using deferred execution. The operator will be executed in it's entirety
	///	    immediately when the sequence is first enumerated.
	/// </para>
	/// </remarks>
	public static IAsyncEnumerable<KeyValuePair<TKey, TAccumulate>> AggregateBy<TSource, TKey, TAccumulate>(
		this IAsyncEnumerable<TSource> source,
		Func<TSource, TKey> keySelector,
		TAccumulate seed,
		Func<TAccumulate, TSource, TAccumulate> func,
		IEqualityComparer<TKey>? comparer = null) where TKey : notnull
	{
		return AggregateBy(source, keySelector, _ => seed, func, comparer);
	}

	/// <summary>
	///	    Applies a key-generating function to each element of a sequence and returns an aggregate value for each key.
	///     An additional argument specifies a comparer to use for testing equivalence of keys.
	/// </summary>
	/// <typeparam name="TSource">
	///	    Type of the elements of the source sequence.
	/// </typeparam>
	/// <typeparam name="TKey">
	///	    Type of the projected element.
	/// </typeparam>
	/// <typeparam name="TAccumulate">
	///	    Type of the accumulator value.
	/// </typeparam>
	/// <param name="source">
	///	    Source sequence.
	/// </param>
	/// <param name="keySelector">
	///	    Function that transforms each item of source sequence into a key to be compared against the others.
	/// </param>
	/// <param name="seedSelector">
	///	    A function that returns the initial seed for each key.
	/// </param>
	/// <param name="func">
	///	    An accumulator function to be invoked on each element. The accumulator value is tracked separately for each
	///     key.
	/// </param>
	/// <param name="comparer">
	///	    The equality comparer to use to determine whether or not keys are equal. If <see langword="null" />, the
	///     default equality comparer for <typeparamref name="TKey"/> is used.
	/// </param>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/>, <paramref name="keySelector"/>, or <paramref name="func"/> is <see
	///     langword="null" />.
	/// </exception>
	/// <returns>
	///	    A sequence of unique keys and their accumulated value.
	/// </returns>
	/// <remarks>
	/// <para>
	///	    This method is implemented by using deferred execution. The operator will be executed in it's entirety
	///	    immediately when the sequence is first enumerated.
	/// </para>
	/// </remarks>
	public static IAsyncEnumerable<KeyValuePair<TKey, TAccumulate>> AggregateBy<TSource, TKey, TAccumulate>(
		this IAsyncEnumerable<TSource> source,
		Func<TSource, TKey> keySelector,
		Func<TKey, TAccumulate> seedSelector,
		Func<TAccumulate, TSource, TAccumulate> func,
		IEqualityComparer<TKey>? comparer = null) where TKey : notnull
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(keySelector);
		ArgumentNullException.ThrowIfNull(seedSelector);
		ArgumentNullException.ThrowIfNull(func);

		return Core(source, keySelector, seedSelector, func, comparer ?? EqualityComparer<TKey>.Default);

		static async IAsyncEnumerable<KeyValuePair<TKey, TAccumulate>> Core(
			IAsyncEnumerable<TSource> source,
			Func<TSource, TKey> keySelector,
			Func<TKey, TAccumulate> seedSelector,
			Func<TAccumulate, TSource, TAccumulate> func,
			IEqualityComparer<TKey> comparer,
			[EnumeratorCancellation] CancellationToken token = default)
		{
			foreach (var kvp in await Loop(source, keySelector, seedSelector, func, comparer, token).ConfigureAwait(false))
				yield return kvp;
		}

		async static Task<Dictionary<TKey, TAccumulate>> Loop(
			IAsyncEnumerable<TSource> source,
			Func<TSource, TKey> keySelector,
			Func<TKey, TAccumulate> seedSelector,
			Func<TAccumulate, TSource, TAccumulate> func,
			IEqualityComparer<TKey> cmp,
			CancellationToken token)
		{
			var dict = new Dictionary<TKey, TAccumulate>(cmp);

			await foreach (var item in source.WithCancellation(token).ConfigureAwait(false))
			{
				var key = keySelector(item);

				dict[key] = func(
					dict.TryGetValue(key, out var acc)
						? acc
						: seedSelector(key),
					item);
			}

			return dict;
		}
	}
}
