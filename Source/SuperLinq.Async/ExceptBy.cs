namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Returns the set of elements in the first sequence which aren't
	/// in the second sequence, according to a given key selector.
	/// </summary>
	/// <remarks>
	/// This is a set operation; if multiple elements in <paramref name="first"/> have
	/// equal keys, only the first such element is returned.
	/// This operator uses deferred execution and streams the results, although
	/// a set of keys from <paramref name="second"/> is immediately selected and retained.
	/// </remarks>
	/// <typeparam name="TSource">The type of the elements in the input sequences.</typeparam>
	/// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector"/>.</typeparam>
	/// <param name="first">The sequence of potentially included elements.</param>
	/// <param name="second">The sequence of elements whose keys may prevent elements in
	/// <paramref name="first"/> from being returned.</param>
	/// <param name="keySelector">The mapping from source element to key.</param>
	/// <returns>A sequence of elements from <paramref name="first"/> whose key was not also a key for
	/// any element in <paramref name="second"/>.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="first"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="keySelector"/> is <see langword="null"/>.</exception>
	public static IAsyncEnumerable<TSource> ExceptBy<TSource, TKey>(
		this IAsyncEnumerable<TSource> first,
		IAsyncEnumerable<TSource> second,
		Func<TSource, TKey> keySelector
	)
	{
		return ExceptBy(first, second, keySelector, keyComparer: default);
	}

	/// <summary>
	/// Returns the set of elements in the first sequence which aren't
	/// in the second sequence, according to a given key selector.
	/// </summary>
	/// <remarks>
	/// This is a set operation; if multiple elements in <paramref name="first"/> have
	/// equal keys, only the first such element is returned.
	/// This operator uses deferred execution and streams the results, although
	/// a set of keys from <paramref name="second"/> is immediately selected and retained.
	/// </remarks>
	/// <typeparam name="TSource">The type of the elements in the input sequences.</typeparam>
	/// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector"/>.</typeparam>
	/// <param name="first">The sequence of potentially included elements.</param>
	/// <param name="second">The sequence of elements whose keys may prevent elements in
	/// <paramref name="first"/> from being returned.</param>
	/// <param name="keySelector">The mapping from source element to key.</param>
	/// <param name="keyComparer">The equality comparer to use to determine whether or not keys are equal.
	/// If null, the default equality comparer for <typeparamref name="TSource"/> is used.</param>
	/// <returns>A sequence of elements from <paramref name="first"/> whose key was not also a key for
	/// any element in <paramref name="second"/>.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="first"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="keySelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="keyComparer"/> is <see langword="null"/>.</exception>
	public static IAsyncEnumerable<TSource> ExceptBy<TSource, TKey>(
		this IAsyncEnumerable<TSource> first,
		IAsyncEnumerable<TSource> second,
		Func<TSource, TKey> keySelector,
		IEqualityComparer<TKey>? keyComparer)
	{
		ArgumentNullException.ThrowIfNull(first);
		ArgumentNullException.ThrowIfNull(second);
		ArgumentNullException.ThrowIfNull(keySelector);

		return Core(first, second, keySelector, keyComparer);

		static async IAsyncEnumerable<TSource> Core(
			IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second,
			Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? keyComparer,
			[EnumeratorCancellation] CancellationToken cancellationToken = default
		)
		{
			var keys = await second.Select(keySelector).ToHashSetAsync(keyComparer, cancellationToken).ConfigureAwait(false);
			await foreach (var element in first.WithCancellation(cancellationToken).ConfigureAwait(false))
			{
				var key = keySelector(element);
				if (keys.Add(key))
					yield return element;
			}
		}
	}
}
