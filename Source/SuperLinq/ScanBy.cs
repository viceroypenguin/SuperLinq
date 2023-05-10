using System.Diagnostics.CodeAnalysis;

namespace SuperLinq;

public static partial class SuperEnumerable
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
	public static IEnumerable<(TKey key, TState state)> ScanBy<TSource, TKey, TState>(
		this IEnumerable<TSource> source,
		Func<TSource, TKey> keySelector,
		Func<TKey, TState> seedSelector,
		Func<TState, TKey, TSource, TState> accumulator)
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
	public static IEnumerable<(TKey key, TState state)> ScanBy<TSource, TKey, TState>(
		this IEnumerable<TSource> source,
		Func<TSource, TKey> keySelector,
		Func<TKey, TState> seedSelector,
		Func<TState, TKey, TSource, TState> accumulator,
		IEqualityComparer<TKey>? comparer)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(keySelector);
		Guard.IsNotNull(seedSelector);
		Guard.IsNotNull(accumulator);

		comparer ??= EqualityComparer<TKey>.Default;

		if (source is ICollection<TSource> coll)
			return new ScanByIterator<TSource, TKey, TState>(
				coll, keySelector, seedSelector, accumulator, comparer);

		return ScanByCore(source, keySelector, seedSelector, accumulator, comparer);
	}

	private static IEnumerable<(TKey key, TState state)> ScanByCore<TSource, TKey, TState>(
		IEnumerable<TSource> source,
		Func<TSource, TKey> keySelector,
		Func<TKey, TState> seedSelector,
		Func<TState, TKey, TSource, TState> accumulator,
		IEqualityComparer<TKey> comparer)
	{
		var stateByKey = new Collections.NullKeyDictionary<TKey, TState>(comparer);

		foreach (var item in source)
		{
			var key = keySelector(item);

			var state = stateByKey.TryGetValue(key, out var ps)
				? ps
				: seedSelector(key);

			state = accumulator(state, key, item);

			stateByKey[key] = state;

			yield return (key, state);
		}
	}

	private sealed class ScanByIterator<TSource, TKey, TState> : CollectionIterator<(TKey key, TState state)>
	{
		private readonly ICollection<TSource> _source;
		private readonly Func<TSource, TKey> _keySelector;
		private readonly Func<TKey, TState> _seedSelector;
		private readonly Func<TState, TKey, TSource, TState> _accumulator;
		private readonly IEqualityComparer<TKey> _comparer;

		public ScanByIterator(
			ICollection<TSource> source,
			Func<TSource, TKey> keySelector,
			Func<TKey, TState> seedSelector,
			Func<TState, TKey, TSource, TState> accumulator,
			IEqualityComparer<TKey> comparer)
		{
			_source = source;
			_keySelector = keySelector;
			_seedSelector = seedSelector;
			_accumulator = accumulator;
			_comparer = comparer;
		}

		public override int Count => _source.Count;

		[ExcludeFromCodeCoverage]
		protected override IEnumerable<(TKey key, TState state)> GetEnumerable() =>
			ScanByCore(_source, _keySelector, _seedSelector, _accumulator, _comparer);
	}
}
