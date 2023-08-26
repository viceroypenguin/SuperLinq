namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///		Returns consecutive distinct elements by using the default equality comparer to compare values.
	/// </summary>
	/// <typeparam name="TSource">
	///		Source sequence element type.
	/// </typeparam>
	/// <param name="source">
	///		Source sequence.
	/// </param>
	/// <returns>
	///		Sequence without adjacent non-distinct elements.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///		<paramref name="source"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///		This method uses deferred execution semantics and streams its results.
	/// </para>
	/// </remarks>
	public static IEnumerable<TSource> DistinctUntilChanged<TSource>(this IEnumerable<TSource> source)
	{
		return DistinctUntilChanged(source, Identity, comparer: null);
	}

	/// <summary>
	///		Returns consecutive distinct elements by using the specified equality comparer to compare values.
	/// </summary>
	/// <typeparam name="TSource">
	///		Source sequence element type.
	/// </typeparam>
	/// <param name="source">
	///		Source sequence.
	/// </param>
	/// <param name="comparer">
	///		Comparer used to compare values.
	/// </param>
	///	<returns>
	///		Sequence without adjacent non-distinct elements.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///		<paramref name="source"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///		This method uses deferred execution semantics and streams its results.
	/// </para>
	/// </remarks>
	public static IEnumerable<TSource> DistinctUntilChanged<TSource>(this IEnumerable<TSource> source, IEqualityComparer<TSource>? comparer)
	{
		Guard.IsNotNull(source);
		return DistinctUntilChanged(source, Identity, comparer);
	}

	/// <summary>
	///	    Returns consecutive distinct elements based on a key value by using the specified equality comparer to
	///	    compare key values.
	/// </summary>
	/// <typeparam name="TSource">
	///	    Source sequence element type.
	/// </typeparam>
	/// <typeparam name="TKey">
	///	    Key type.
	/// </typeparam>
	/// <param name="source">
	///	    Source sequence.
	/// </param>
	/// <param name="keySelector">
	///	    Key selector.
	/// </param>
	/// <returns>
	///	    Sequence without adjacent non-distinct elements.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="keySelector"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This method uses deferred execution semantics and streams its results.
	/// </para>
	/// </remarks>
	public static IEnumerable<TSource> DistinctUntilChanged<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
	{
		return DistinctUntilChanged(source, keySelector, comparer: null);
	}

	/// <summary>
	///	    Returns consecutive distinct elements based on a key value by using the specified equality comparer to
	///     compare key values.
	/// </summary>
	/// <typeparam name="TSource">
	///	    Source sequence element type.
	/// </typeparam>
	/// <typeparam name="TKey">
	///	    Key type.
	/// </typeparam>
	/// <param name="source">
	///	    Source sequence.
	/// </param>
	/// <param name="keySelector">
	///	    Key selector.
	/// </param>
	/// <param name="comparer">
	///	    Comparer used to compare key values.
	/// </param>
	/// <returns>
	///	    Sequence without adjacent non-distinct elements.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="keySelector"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This method uses deferred execution semantics and streams its results.
	/// </para>
	/// </remarks>
	public static IEnumerable<TSource> DistinctUntilChanged<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(keySelector);

		return Core(source, keySelector, comparer ?? EqualityComparer<TKey>.Default);

		static IEnumerable<TSource> Core(IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
		{
			using var e = source.GetEnumerator();
			if (!e.MoveNext())
				yield break;

			yield return e.Current;
			var lastKey = keySelector(e.Current);

			while (e.MoveNext())
			{
				var nextKey = keySelector(e.Current);
				if (!comparer.Equals(lastKey, nextKey))
				{
					yield return e.Current;
					lastKey = nextKey;
				}
			}
		}
	}
}
