namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Returns a sequence from a dictionary based on the result of evaluating a selector function.
	/// </summary>
	/// <typeparam name="TValue">
	///	    Type of the selector value.
	/// </typeparam>
	/// <typeparam name="TResult">
	///	    Result sequence element type.
	/// </typeparam>
	/// <param name="selector">
	///	    Selector function used to pick a sequence from the given sources.
	/// </param>
	/// <param name="sources">
	///	    Dictionary mapping selector values onto resulting sequences.
	/// </param>
	/// <returns>
	///	    The source sequence corresponding with the evaluated selector value; otherwise, an empty sequence.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="selector"/> or <paramref name="sources"/> is <see langword="null"/>.
	/// </exception>
	/// <exception cref="ArgumentNullException">
	///	    (Thrown lazily) The sequence in <paramref name="sources"/> selected by the result of <paramref
	///     name="selector"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    <paramref name="selector"/> is not evaluated until enumeration. The value returned will be used to select a
	///     sequence from <paramref name="sources"/>; enumeration will continue with items from that sequence.
	/// </para>
	/// <para>
	///	    If the value returned by <paramref name="selector"/> is not present in <paramref name="sources"/>, the
	///     resulting sequence will be empty.
	/// </para>
	/// </remarks>
	public static IEnumerable<TResult> Case<TValue, TResult>(
		Func<TValue> selector,
		IDictionary<TValue, IEnumerable<TResult>> sources)
		where TValue : notnull
	{
		return Case(selector, sources, Enumerable.Empty<TResult>());
	}

	/// <summary>
	///	    Returns a sequence from a dictionary based on the result of evaluating a selector function.
	/// </summary>
	/// <typeparam name="TValue">
	///	    Type of the selector value.
	/// </typeparam>
	/// <typeparam name="TResult">
	///	    Result sequence element type.
	/// </typeparam>
	/// <param name="selector">
	///	    Selector function used to pick a sequence from the given sources.
	/// </param>
	/// <param name="sources">
	///	    Dictionary mapping selector values onto resulting sequences.
	/// </param>
	/// <param name="defaultSource">
	///	    Default sequence to return in case there's no corresponding source for the computed selector value.
	/// </param>
	/// <returns>
	///	    The source sequence corresponding with the evaluated selector value; otherwise, the <paramref
	///     name="defaultSource"/> sequence.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="selector"/>, <paramref name="sources"/> or <paramref name="defaultSource"/> is <see
	///     langword="null"/>.
	/// </exception>
	/// <exception cref="ArgumentNullException">
	///	    (Thrown lazily) The sequence in <paramref name="sources"/> selected by the result of <paramref
	///     name="selector"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    <paramref name="selector"/> is not evaluated until enumeration. The value returned will be used to select a
	///     sequence from <paramref name="sources"/>; enumeration will continue with items from that sequence.
	/// </para>
	/// </remarks>
	public static IEnumerable<TResult> Case<TValue, TResult>(
		Func<TValue> selector,
		IDictionary<TValue, IEnumerable<TResult>> sources,
		IEnumerable<TResult> defaultSource)
		where TValue : notnull
	{
		Guard.IsNotNull(selector);
		Guard.IsNotNull(sources);
		Guard.IsNotNull(defaultSource);

		return Core(selector, sources, defaultSource);

		static IEnumerable<TResult> Core(
			Func<TValue> selector,
			IDictionary<TValue, IEnumerable<TResult>> sources,
			IEnumerable<TResult> defaultSource)
		{
			if (!sources.TryGetValue(selector(), out var source))
				source = defaultSource;

			Guard.IsNotNull(source);
			foreach (var el in source)
				yield return el;
		}
	}
}
