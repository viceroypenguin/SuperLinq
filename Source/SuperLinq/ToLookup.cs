namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Creates a <see cref="ILookup{TKey,TValue}" /> from a sequence of
	/// <see cref="KeyValuePair{TKey,TValue}" /> elements.
	/// </summary>
	/// <typeparam name="TKey">The type of the key.</typeparam>
	/// <typeparam name="TValue">The type of the value.</typeparam>
	/// <param name="source">The source sequence of key-value pairs.</param>
	/// <returns>
	/// A <see cref="ILookup{TKey,TValue}"/> containing the values
	/// mapped to their keys.
	/// </returns>

	public static ILookup<TKey, TValue> ToLookup<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source) =>
		source.ToLookup(null);

	/// <summary>
	/// Creates a <see cref="ILookup{TKey,TValue}" /> from a sequence of
	/// <see cref="KeyValuePair{TKey,TValue}" /> elements. An additional
	/// parameter specifies a comparer for keys.
	/// </summary>
	/// <typeparam name="TKey">The type of the key.</typeparam>
	/// <typeparam name="TValue">The type of the value.</typeparam>
	/// <param name="source">The source sequence of key-value pairs.</param>
	/// <param name="comparer">The comparer for keys.</param>
	/// <returns>
	/// A <see cref="ILookup{TKey,TValue}"/> containing the values
	/// mapped to their keys.
	/// </returns>

	public static ILookup<TKey, TValue> ToLookup<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source,
		IEqualityComparer<TKey>? comparer)
	{
		source.ThrowIfNull();
		return source.ToLookup(e => e.Key, e => e.Value, comparer);
	}

	/// <summary>
	/// Creates a <see cref="Lookup{TKey,TValue}" /> from a sequence of
	/// tuples of 2 where the first item is the key and the second the
	/// value.
	/// </summary>
	/// <typeparam name="TKey">The type of the key.</typeparam>
	/// <typeparam name="TValue">The type of the value.</typeparam>
	/// <param name="source">The source sequence of tuples of 2.</param>
	/// <returns>
	/// A <see cref="Lookup{TKey, TValue}"/> containing the values
	/// mapped to their keys.
	/// </returns>

	public static ILookup<TKey, TValue> ToLookup<TKey, TValue>(this IEnumerable<(TKey Key, TValue Value)> source) =>
		source.ToLookup(null);

	/// <summary>
	/// Creates a <see cref="Lookup{TKey,TValue}" /> from a sequence of
	/// tuples of 2 where the first item is the key and the second the
	/// value. An additional parameter specifies a comparer for keys.
	/// </summary>
	/// <typeparam name="TKey">The type of the key.</typeparam>
	/// <typeparam name="TValue">The type of the value.</typeparam>
	/// <param name="source">The source sequence of tuples of 2.</param>
	/// <param name="comparer">The comparer for keys.</param>
	/// <returns>
	/// A <see cref="Lookup{TKey, TValue}"/> containing the values
	/// mapped to their keys.
	/// </returns>

	public static ILookup<TKey, TValue> ToLookup<TKey, TValue>(this IEnumerable<(TKey Key, TValue Value)> source,
		IEqualityComparer<TKey>? comparer)
	{
		source.ThrowIfNull();
		return source.ToLookup(e => e.Key, e => e.Value, comparer);
	}
}
