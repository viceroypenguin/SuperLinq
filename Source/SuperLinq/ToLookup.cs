namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Creates an <see cref="ILookup{TKey,TValue}" /> from a sequence of <see cref="KeyValuePair{TKey,TValue}" />
	///     elements.
	/// </summary>
	/// <typeparam name="TKey">
	///	    The type of the key.
	/// </typeparam>
	/// <typeparam name="TValue">
	///	    The type of the value.
	/// </typeparam>
	/// <param name="source">
	///	    The source sequence of key-value pairs.
	/// </param>
	/// <returns>
	///	    An <see cref="ILookup{TKey,TValue}"/> containing the values mapped to their keys.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> is <see langword="null"/>
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This method executes immediately.
	/// </para>
	/// </remarks>
	public static ILookup<TKey, TValue> ToLookup<TKey, TValue>(
		this IEnumerable<KeyValuePair<TKey, TValue>> source)
	{
		return source.ToLookup(null);
	}

	/// <summary>
	///	    Creates an <see cref="ILookup{TKey,TValue}" /> from a sequence of <see cref="KeyValuePair{TKey,TValue}" />
	///     elements. An additional parameter specifies a comparer for keys.
	/// </summary>
	/// <typeparam name="TKey">
	///	    The type of the key.
	/// </typeparam>
	/// <typeparam name="TValue">
	///	    The type of the value.
	/// </typeparam>
	/// <param name="source">
	///	    The source sequence of key-value pairs.
	/// </param>
	/// <param name="comparer">
	///	    The comparer for keys.
	/// </param>
	/// <returns>
	///	    An <see cref="ILookup{TKey,TValue}"/> containing the values mapped to their keys.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> is <see langword="null"/>
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This method executes immediately.
	/// </para>
	/// </remarks>
	public static ILookup<TKey, TValue> ToLookup<TKey, TValue>(
		this IEnumerable<KeyValuePair<TKey, TValue>> source,
		IEqualityComparer<TKey>? comparer)
	{
		ArgumentNullException.ThrowIfNull(source);
		return source.ToLookup(e => e.Key, e => e.Value, comparer);
	}

	/// <summary>
	///	    Creates an <see cref="ILookup{TKey,TValue}" /> /> from a sequence of tuples of 2 where the first item is the
	///     key and the second the value.
	/// </summary>
	/// <typeparam name="TKey">
	///	    The type of the key.
	/// </typeparam>
	/// <typeparam name="TValue">
	///	    The type of the value.
	/// </typeparam>
	/// <param name="source">
	///	    The source sequence of tuples of 2.
	/// </param>
	/// <returns>
	///	    An <see cref="ILookup{TKey,TValue}"/> containing the values mapped to their keys.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> is <see langword="null"/>
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This method executes immediately.
	/// </para>
	/// </remarks>
	public static ILookup<TKey, TValue> ToLookup<TKey, TValue>(
		this IEnumerable<(TKey Key, TValue Value)> source)
	{
		return source.ToLookup(null);
	}

	/// <summary>
	///	    Creates an <see cref="ILookup{TKey,TValue}" /> from a sequence of tuples of 2 where the first item is the
	///     key and the second the value. An additional parameter specifies a comparer for keys.
	/// </summary>
	/// <typeparam name="TKey">
	///	    The type of the key.
	/// </typeparam>
	/// <typeparam name="TValue">
	///	    The type of the value.
	/// </typeparam>
	/// <param name="source">
	///	    The source sequence of tuples of 2.
	/// </param>
	/// <param name="comparer">
	///	    The comparer for keys.
	/// </param>
	/// <returns>
	///	    An <see cref="ILookup{TKey,TValue}"/> containing the values mapped to their keys.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> is <see langword="null"/>
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This method executes immediately.
	/// </para>
	/// </remarks>
	public static ILookup<TKey, TValue> ToLookup<TKey, TValue>(
		this IEnumerable<(TKey Key, TValue Value)> source,
		IEqualityComparer<TKey>? comparer)
	{
		ArgumentNullException.ThrowIfNull(source);
		return source.ToLookup(e => e.Key, e => e.Value, comparer);
	}
}
