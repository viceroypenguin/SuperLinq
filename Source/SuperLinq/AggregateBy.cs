namespace SuperLinq;

public static partial class SuperEnumerable
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
	public static IEnumerable<KeyValuePair<TKey, TAccumulate>> AggregateBy<TSource, TKey, TAccumulate>(
		this IEnumerable<TSource> source,
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
	///	    <paramref name="source"/>, <paramref name="seedSelector"/>, <paramref name="keySelector"/>, or <paramref
	///     name="func"/> is <see langword="null" />.
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
	public static IEnumerable<KeyValuePair<TKey, TAccumulate>> AggregateBy<TSource, TKey, TAccumulate>(
		this IEnumerable<TSource> source,
		Func<TSource, TKey> keySelector,
		Func<TKey, TAccumulate> seedSelector,
		Func<TAccumulate, TSource, TAccumulate> func,
		IEqualityComparer<TKey>? comparer = null) where TKey : notnull
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(keySelector);
		Guard.IsNotNull(seedSelector);
		Guard.IsNotNull(func);

		return Core(source, keySelector, seedSelector, func, comparer ?? EqualityComparer<TKey>.Default);

		static IEnumerable<KeyValuePair<TKey, TAccumulate>> Core(
			IEnumerable<TSource> source,
			Func<TSource, TKey> keySelector,
			Func<TKey, TAccumulate> seedSelector,
			Func<TAccumulate, TSource, TAccumulate> func,
			IEqualityComparer<TKey> comparer)
		{
			foreach (var kvp in Loop(source, keySelector, seedSelector, func, comparer))
				yield return kvp;
		}

		static Dictionary<TKey, TAccumulate> Loop(
			IEnumerable<TSource> source,
			Func<TSource, TKey> keySelector,
			Func<TKey, TAccumulate> seedSelector,
			Func<TAccumulate, TSource, TAccumulate> func,
			IEqualityComparer<TKey> cmp)
		{
			var dict = new Dictionary<TKey, TAccumulate>(cmp);

			foreach (var item in source)
			{
				var key = keySelector(item);

#if NET5_0_OR_GREATER
				ref var acc = ref System.Runtime.InteropServices.CollectionsMarshal.GetValueRefOrAddDefault(dict, key, out var exists);
				acc = func(exists ? acc! : seedSelector(key), item);
#else
				dict[key] = func(
					dict.TryGetValue(key, out var acc)
						? acc
						: seedSelector(key),
					item);
#endif
			}

			return dict;
		}
	}
}
