﻿namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Applies an accumulator function over sequence element keys,
	/// returning the keys along with intermediate accumulator states.
	/// </summary>
	/// <typeparam name="TSource">Type of the elements of the source sequence.</typeparam>
	/// <typeparam name="TKey">The type of the key.</typeparam>
	/// <typeparam name="TState">Type of the state.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="keySelector">
	/// A function that returns the key given an element.</param>
	/// <param name="seedSelector">
	/// A function to determine the initial value for the accumulator that is
	/// invoked once per key encountered.</param>
	/// <param name="accumulator">
	/// An accumulator function invoked for each element.</param>
	/// <returns>
	/// A sequence of keys paired with intermediate accumulator states.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="keySelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="seedSelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="accumulator"/> is <see langword="null"/>.</exception>
	public static IAsyncEnumerable<(TKey key, TState state)> ScanBy<TSource, TKey, TState>(
		this IAsyncEnumerable<TSource> source,
		Func<TSource, TKey> keySelector,
		Func<TKey, TState> seedSelector,
		Func<TState, TKey, TSource, TState> accumulator)
	{
		return source.ScanBy(
			(s, ct) => new ValueTask<TKey>(keySelector(s)),
			(k, ct) => new ValueTask<TState>(seedSelector(k)),
			(s, k, i, ct) => new ValueTask<TState>(accumulator(s, k, i)));
	}

	/// <summary>
	/// Applies an accumulator function over sequence element keys,
	/// returning the keys along with intermediate accumulator states.
	/// </summary>
	/// <typeparam name="TSource">Type of the elements of the source sequence.</typeparam>
	/// <typeparam name="TKey">The type of the key.</typeparam>
	/// <typeparam name="TState">Type of the state.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="keySelector">
	/// A function that returns the key given an element.</param>
	/// <param name="seedSelector">
	/// A function to determine the initial value for the accumulator that is
	/// invoked once per key encountered.</param>
	/// <param name="accumulator">
	/// An accumulator function invoked for each element.</param>
	/// <returns>
	/// A sequence of keys paired with intermediate accumulator states.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="keySelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="seedSelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="accumulator"/> is <see langword="null"/>.</exception>
	public static IAsyncEnumerable<(TKey key, TState state)> ScanBy<TSource, TKey, TState>(
		this IAsyncEnumerable<TSource> source,
		Func<TSource, ValueTask<TKey>> keySelector,
		Func<TKey, ValueTask<TState>> seedSelector,
		Func<TState, TKey, TSource, ValueTask<TState>> accumulator)
	{
		return source.ScanBy(
			(s, ct) => keySelector(s),
			(k, ct) => seedSelector(k),
			(s, k, i, ct) => accumulator(s, k, i),
			comparer: null);
	}

	/// <summary>
	/// Applies an accumulator function over sequence element keys,
	/// returning the keys along with intermediate accumulator states.
	/// </summary>
	/// <typeparam name="TSource">Type of the elements of the source sequence.</typeparam>
	/// <typeparam name="TKey">The type of the key.</typeparam>
	/// <typeparam name="TState">Type of the state.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="keySelector">
	/// A function that returns the key given an element.</param>
	/// <param name="seedSelector">
	/// A function to determine the initial value for the accumulator that is
	/// invoked once per key encountered.</param>
	/// <param name="accumulator">
	/// An accumulator function invoked for each element.</param>
	/// <returns>
	/// A sequence of keys paired with intermediate accumulator states.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="keySelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="seedSelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="accumulator"/> is <see langword="null"/>.</exception>
	public static IAsyncEnumerable<(TKey key, TState state)> ScanBy<TSource, TKey, TState>(
		this IAsyncEnumerable<TSource> source,
		Func<TSource, CancellationToken, ValueTask<TKey>> keySelector,
		Func<TKey, CancellationToken, ValueTask<TState>> seedSelector,
		Func<TState, TKey, TSource, CancellationToken, ValueTask<TState>> accumulator)
	{
		return source.ScanBy(keySelector, seedSelector, accumulator, comparer: null);
	}

	/// <summary>
	/// Applies an accumulator function over sequence element keys,
	/// returning the keys along with intermediate accumulator states. An
	/// additional parameter specifies the comparer to use to compare keys.
	/// </summary>
	/// <typeparam name="TSource">Type of the elements of the source sequence.</typeparam>
	/// <typeparam name="TKey">The type of the key.</typeparam>
	/// <typeparam name="TState">Type of the state.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="keySelector">
	/// A function that returns the key given an element.</param>
	/// <param name="seedSelector">
	/// A function to determine the initial value for the accumulator that is
	/// invoked once per key encountered.</param>
	/// <param name="accumulator">
	/// An accumulator function invoked for each element.</param>
	/// <param name="comparer">The equality comparer to use to determine
	/// whether or not keys are equal. If <see langword="null"/>, the default equality
	/// comparer for <typeparamref name="TSource"/> is used.</param>
	/// <returns>
	/// A sequence of keys paired with intermediate accumulator states.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="keySelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="seedSelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="accumulator"/> is <see langword="null"/>.</exception>
	public static IAsyncEnumerable<(TKey key, TState state)> ScanBy<TSource, TKey, TState>(
		this IAsyncEnumerable<TSource> source,
		Func<TSource, TKey> keySelector,
		Func<TKey, TState> seedSelector,
		Func<TState, TKey, TSource, TState> accumulator,
		IEqualityComparer<TKey>? comparer)
	{
		return source.ScanBy(
			(s, ct) => new ValueTask<TKey>(keySelector(s)),
			(k, ct) => new ValueTask<TState>(seedSelector(k)),
			(s, k, i, ct) => new ValueTask<TState>(accumulator(s, k, i)),
			comparer);
	}

	/// <summary>
	/// Applies an accumulator function over sequence element keys,
	/// returning the keys along with intermediate accumulator states. An
	/// additional parameter specifies the comparer to use to compare keys.
	/// </summary>
	/// <typeparam name="TSource">Type of the elements of the source sequence.</typeparam>
	/// <typeparam name="TKey">The type of the key.</typeparam>
	/// <typeparam name="TState">Type of the state.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="keySelector">
	/// A function that returns the key given an element.</param>
	/// <param name="seedSelector">
	/// A function to determine the initial value for the accumulator that is
	/// invoked once per key encountered.</param>
	/// <param name="accumulator">
	/// An accumulator function invoked for each element.</param>
	/// <param name="comparer">The equality comparer to use to determine
	/// whether or not keys are equal. If <see langword="null"/>, the default equality
	/// comparer for <typeparamref name="TSource"/> is used.</param>
	/// <returns>
	/// A sequence of keys paired with intermediate accumulator states.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="keySelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="seedSelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="accumulator"/> is <see langword="null"/>.</exception>
	public static IAsyncEnumerable<(TKey key, TState state)> ScanBy<TSource, TKey, TState>(
		this IAsyncEnumerable<TSource> source,
		Func<TSource, ValueTask<TKey>> keySelector,
		Func<TKey, ValueTask<TState>> seedSelector,
		Func<TState, TKey, TSource, ValueTask<TState>> accumulator,
		IEqualityComparer<TKey>? comparer)
	{
		return source.ScanBy(
			(s, ct) => keySelector(s),
			(k, ct) => seedSelector(k),
			(s, k, i, ct) => accumulator(s, k, i),
			comparer);
	}

	/// <summary>
	/// Applies an accumulator function over sequence element keys,
	/// returning the keys along with intermediate accumulator states. An
	/// additional parameter specifies the comparer to use to compare keys.
	/// </summary>
	/// <typeparam name="TSource">Type of the elements of the source sequence.</typeparam>
	/// <typeparam name="TKey">The type of the key.</typeparam>
	/// <typeparam name="TState">Type of the state.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="keySelector">
	/// A function that returns the key given an element.</param>
	/// <param name="seedSelector">
	/// A function to determine the initial value for the accumulator that is
	/// invoked once per key encountered.</param>
	/// <param name="accumulator">
	/// An accumulator function invoked for each element.</param>
	/// <param name="comparer">The equality comparer to use to determine
	/// whether or not keys are equal. If <see langword="null"/>, the default equality
	/// comparer for <typeparamref name="TSource"/> is used.</param>
	/// <returns>
	/// A sequence of keys paired with intermediate accumulator states.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="keySelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="seedSelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="accumulator"/> is <see langword="null"/>.</exception>
	public static IAsyncEnumerable<(TKey key, TState state)> ScanBy<TSource, TKey, TState>(
		this IAsyncEnumerable<TSource> source,
		Func<TSource, CancellationToken, ValueTask<TKey>> keySelector,
		Func<TKey, CancellationToken, ValueTask<TState>> seedSelector,
		Func<TState, TKey, TSource, CancellationToken, ValueTask<TState>> accumulator,
		IEqualityComparer<TKey>? comparer)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(keySelector);
		Guard.IsNotNull(seedSelector);
		Guard.IsNotNull(accumulator);

		comparer ??= EqualityComparer<TKey>.Default;

		return Core(source, keySelector, seedSelector, accumulator, comparer);

		static async IAsyncEnumerable<(TKey key, TState state)> Core(
			IAsyncEnumerable<TSource> source,
			Func<TSource, CancellationToken, ValueTask<TKey>> keySelector,
			Func<TKey, CancellationToken, ValueTask<TState>> seedSelector,
			Func<TState, TKey, TSource, CancellationToken, ValueTask<TState>> accumulator,
			IEqualityComparer<TKey> comparer,
			[EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			var stateByKey = new Collections.NullKeyDictionary<TKey, TState>(comparer);

			await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
			{
				var key = await keySelector(item, cancellationToken).ConfigureAwait(false);

				var state = stateByKey.TryGetValue(key, out var ps)
					? ps
					: await seedSelector(key, cancellationToken).ConfigureAwait(false);

				state = await accumulator(state, key, item, cancellationToken).ConfigureAwait(false);

				stateByKey[key] = state;

				yield return (key, state);
			}
		}
	}
}
