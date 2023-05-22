namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Creates a <see cref="Dictionary{TKey,TValue}" /> from a sequence of
	/// <see cref="KeyValuePair{TKey,TValue}" /> elements.
	/// </summary>
	/// <typeparam name="TKey">The type of the key.</typeparam>
	/// <typeparam name="TValue">The type of the value.</typeparam>
	/// <param name="source">The source sequence of key-value pairs.</param>
	/// <returns>
	/// A <see cref="Dictionary{TKey, TValue}"/> containing the values
	/// mapped to their keys.
	/// </returns>
#if NET8_0_OR_GREATER
	[Obsolete("This method has been implemented by the framework.")]
	public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(
		IEnumerable<KeyValuePair<TKey, TValue>> source)
#else
	public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(
		this IEnumerable<KeyValuePair<TKey, TValue>> source)
#endif
		where TKey : notnull
	{
		return ToDictionary(source, comparer: default);
	}

	/// <summary>
	/// Creates a <see cref="Dictionary{TKey,TValue}" /> from a sequence of
	/// <see cref="KeyValuePair{TKey,TValue}" /> elements. An additional
	/// parameter specifies a comparer for keys.
	/// </summary>
	/// <typeparam name="TKey">The type of the key.</typeparam>
	/// <typeparam name="TValue">The type of the value.</typeparam>
	/// <param name="source">The source sequence of key-value pairs.</param>
	/// <param name="comparer">The comparer for keys.</param>
	/// <returns>
	/// A <see cref="Dictionary{TKey, TValue}"/> containing the values
	/// mapped to their keys.
	/// </returns>
#if NET8_0_OR_GREATER
	[Obsolete("This method has been implemented by the framework.")]
	public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(
		IEnumerable<KeyValuePair<TKey, TValue>> source,
		IEqualityComparer<TKey>? comparer)
#else
	public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(
		this IEnumerable<KeyValuePair<TKey, TValue>> source,
		IEqualityComparer<TKey>? comparer)
#endif
		where TKey : notnull
	{
		Guard.IsNotNull(source);
		return source.ToDictionary(e => e.Key, e => e.Value, comparer);
	}

	/// <summary>
	/// Creates a <see cref="Dictionary{TKey,TValue}" /> from a sequence of
	/// tuples of 2 where the first item is the key and the second the
	/// value.
	/// </summary>
	/// <typeparam name="TKey">The type of the key.</typeparam>
	/// <typeparam name="TValue">The type of the value.</typeparam>
	/// <param name="source">The source sequence of couples (tuple of 2).</param>
	/// <returns>
	/// A <see cref="Dictionary{TKey, TValue}"/> containing the values
	/// mapped to their keys.
	/// </returns>
#if NET8_0_OR_GREATER
	[Obsolete("This method has been implemented by the framework.")]
	public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(
		IEnumerable<(TKey Key, TValue Value)> source)
#else
	public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(
		this IEnumerable<(TKey Key, TValue Value)> source)
#endif
		where TKey : notnull
	{
		return ToDictionary(source, comparer: default);
	}

	/// <summary>
	/// Creates a <see cref="Dictionary{TKey,TValue}" /> from a sequence of
	/// tuples of 2 where the first item is the key and the second the
	/// value. An additional parameter specifies a comparer for keys.
	/// </summary>
	/// <typeparam name="TKey">The type of the key.</typeparam>
	/// <typeparam name="TValue">The type of the value.</typeparam>
	/// <param name="source">The source sequence of couples (tuple of 2).</param>
	/// <param name="comparer">The comparer for keys.</param>
	/// <returns>
	/// A <see cref="Dictionary{TKey, TValue}"/> containing the values
	/// mapped to their keys.
	/// </returns>
#if NET8_0_OR_GREATER
	[Obsolete("This method has been implemented by the framework.")]
	public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(
		IEnumerable<(TKey Key, TValue Value)> source,
		IEqualityComparer<TKey>? comparer)
#else
	public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(
		this IEnumerable<(TKey Key, TValue Value)> source,
		IEqualityComparer<TKey>? comparer)
#endif
		where TKey : notnull
	{
		Guard.IsNotNull(source);
		return source.ToDictionary(e => e.Key, e => e.Value, comparer);
	}
}
