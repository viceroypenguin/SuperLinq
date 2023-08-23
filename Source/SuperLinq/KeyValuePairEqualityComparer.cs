namespace SuperLinq;

/// <summary>
/// A utility class to easily compose a custom <see cref="IEqualityComparer{T}"/>
/// for <see cref="KeyValuePair{TKey, TValue}"/>s.
/// </summary>
internal static class KeyValuePairEqualityComparer
{
	/// <summary>
	/// Creates a custom <see cref="IEqualityComparer{T}"/> for a <see cref="KeyValuePair{TKey, TValue}"/> based on custom
	/// comparers for each component.
	/// </summary>
	/// <typeparam name="TKey">The type of the first element of the <see cref="KeyValuePair{TKey, TValue}"/></typeparam>
	/// <typeparam name="TValue">The type of the second element of the <see cref="KeyValuePair{TKey, TValue}"/></typeparam>
	/// <param name="keyComparer">The custom comparer for <typeparamref name="TKey"/>. If <see langword="null"/>, then <see
	/// cref="EqualityComparer{T}.Default" /> will be used.</param>
	/// <param name="valueComparer">The custom comparer for <typeparamref name="TValue"/>. If <see langword="null"/>, then <see
	/// cref="EqualityComparer{T}.Default" /> will be used.</param>
	/// <returns>An <see cref="IEqualityComparer{T}"/> that can be used to compare two <see cref="KeyValuePair{TKey, TValue}"/>
	/// using the provided comparers for each component.</returns>
	public static IEqualityComparer<KeyValuePair<TKey, TValue>> Create<TKey, TValue>(
			IEqualityComparer<TKey>? keyComparer,
			IEqualityComparer<TValue>? valueComparer) =>
		new ItemEqualityComparer<TKey, TValue>(
			keyComparer,
			valueComparer);

	private sealed class ItemEqualityComparer<TKey, TValue> : IEqualityComparer<KeyValuePair<TKey, TValue>>
	{
		private readonly IEqualityComparer<TKey> _keyComparer;
		private readonly IEqualityComparer<TValue> _valueComparer;

		public ItemEqualityComparer(
			IEqualityComparer<TKey>? keyComparer,
			IEqualityComparer<TValue>? valueComparer)
		{
			_keyComparer = keyComparer ?? EqualityComparer<TKey>.Default;
			_valueComparer = valueComparer ?? EqualityComparer<TValue>.Default;
		}

		public bool Equals(KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y)
			=> _keyComparer.Equals(x.Key, y.Key)
			   && _valueComparer.Equals(x.Value, y.Value);

		public int GetHashCode(KeyValuePair<TKey, TValue> obj)
			=> HashCode.Combine(
				_keyComparer.GetHashCode(obj.Key!),
				_valueComparer.GetHashCode(obj.Value!));
	}
}
