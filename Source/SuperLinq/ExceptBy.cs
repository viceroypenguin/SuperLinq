namespace SuperLinq;

public static partial class SuperEnumerable
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
	public static IEnumerable<TSource> ExceptByItems<TSource, TKey>(
		this IEnumerable<TSource> first,
		IEnumerable<TSource> second,
		Func<TSource, TKey> keySelector)
		=> ExceptByItems(first, second, keySelector, keyComparer: default);

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
	/// If null, the default equality comparer for <c>TSource</c> is used.</param>
	/// <returns>A sequence of elements from <paramref name="first"/> whose key was not also a key for
	/// any element in <paramref name="second"/>.</returns>
	public static IEnumerable<TSource> ExceptByItems<TSource, TKey>(
		this IEnumerable<TSource> first,
		IEnumerable<TSource> second,
		Func<TSource, TKey> keySelector,
		IEqualityComparer<TKey>? keyComparer)
	{
		first.ThrowIfNull();
		second.ThrowIfNull();
		keySelector.ThrowIfNull();

		return _(); IEnumerable<TSource> _()
		{
			var keys = second.Select(keySelector).ToHashSet(keyComparer);
			foreach (var element in first)
			{
				var key = keySelector(element);
				if (keys.Contains(key))
					continue;
				yield return element;
				keys.Add(key);
			}
		}
	}
}
