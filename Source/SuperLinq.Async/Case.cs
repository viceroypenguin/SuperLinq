namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Returns a sequence from a dictionary based on the result of evaluating a selector function.
	/// </summary>
	/// <typeparam name="TValue">Type of the selector value.</typeparam>
	/// <typeparam name="TResult">Result sequence element type.</typeparam>
	/// <param name="selector">Selector function used to pick a sequence from the given sources.</param>
	/// <param name="sources">Dictionary mapping selector values onto resulting sequences.</param>
	/// <returns>The source sequence corresponding with the evaluated selector value; otherwise, an empty
	/// sequence.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="selector"/> or <paramref name="sources"/> is <see
	/// langword="null"/>.</exception>
	/// <remarks>
	/// <para>
	/// <paramref name="selector"/> is not evaluated until enumeration. The value returned will be used to select a
	/// sequence from <paramref name="sources"/>; enumeration will continue with items from that sequence.
	/// </para>
	/// <para>
	/// If the value returned by <paramref name="selector"/> is not present in <paramref name="sources"/>, the resulting
	/// sequence will be empty.
	/// </para>
	/// </remarks>
	public static IAsyncEnumerable<TResult> Case<TValue, TResult>(
		Func<TValue> selector,
		IDictionary<TValue, IAsyncEnumerable<TResult>> sources)
		where TValue : notnull
	{
		ArgumentNullException.ThrowIfNull(selector);
		ArgumentNullException.ThrowIfNull(sources);

		return Case(selector.ToAsync(), sources, AsyncEnumerable.Empty<TResult>());
	}

	/// <summary>
	/// Returns a sequence from a dictionary based on the result of evaluating a selector function.
	/// </summary>
	/// <typeparam name="TValue">Type of the selector value.</typeparam>
	/// <typeparam name="TResult">Result sequence element type.</typeparam>
	/// <param name="selector">Selector function used to pick a sequence from the given sources.</param>
	/// <param name="sources">Dictionary mapping selector values onto resulting sequences.</param>
	/// <returns>The source sequence corresponding with the evaluated selector value; otherwise, an empty
	/// sequence.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="selector"/> or <paramref name="sources"/> is <see
	/// langword="null"/>.</exception>
	/// <remarks>
	/// <para>
	/// <paramref name="selector"/> is not evaluated until enumeration. The value returned will be used to select a
	/// sequence from <paramref name="sources"/>; enumeration will continue with items from that sequence.
	/// </para>
	/// <para>
	/// If the value returned by <paramref name="selector"/> is not present in <paramref name="sources"/>, the resulting
	/// sequence will be empty.
	/// </para>
	/// </remarks>
	public static IAsyncEnumerable<TResult> Case<TValue, TResult>(
		Func<ValueTask<TValue>> selector,
		IDictionary<TValue, IAsyncEnumerable<TResult>> sources)
		where TValue : notnull
	{
		ArgumentNullException.ThrowIfNull(selector);
		ArgumentNullException.ThrowIfNull(sources);

		return Case(selector, sources, AsyncEnumerable.Empty<TResult>());
	}

	/// <summary>
	/// Returns a sequence from a dictionary based on the result of evaluating a selector function.
	/// </summary>
	/// <typeparam name="TValue">Type of the selector value.</typeparam>
	/// <typeparam name="TResult">Result sequence element type.</typeparam>
	/// <param name="selector">Selector function used to pick a sequence from the given sources.</param>
	/// <param name="sources">Dictionary mapping selector values onto resulting sequences.</param>
	/// <param name="defaultSource">Default sequence to return in case there's no corresponding source for the computed
	/// selector value.</param>
	/// <returns>The source sequence corresponding with the evaluated selector value; otherwise, an empty
	/// sequence.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="selector"/>, <paramref name="sources"/>, or <paramref
	/// name="defaultSource"/> is <see langword="null"/>.</exception>
	/// <remarks>
	/// <paramref name="selector"/> is not evaluated until enumeration. The value returned will be used to select a
	/// sequence from <paramref name="sources"/>; enumeration will continue with items from that sequence.
	/// </remarks>
	public static IAsyncEnumerable<TResult> Case<TValue, TResult>(
		Func<TValue> selector,
		IDictionary<TValue, IAsyncEnumerable<TResult>> sources,
		IAsyncEnumerable<TResult> defaultSource)
		where TValue : notnull
	{
		ArgumentNullException.ThrowIfNull(selector);
		ArgumentNullException.ThrowIfNull(sources);

		return Case(selector.ToAsync(), sources, defaultSource);
	}

	/// <summary>
	/// Returns a sequence from a dictionary based on the result of evaluating a selector function.
	/// </summary>
	/// <typeparam name="TValue">Type of the selector value.</typeparam>
	/// <typeparam name="TResult">Result sequence element type.</typeparam>
	/// <param name="selector">Selector function used to pick a sequence from the given sources.</param>
	/// <param name="sources">Dictionary mapping selector values onto resulting sequences.</param>
	/// <param name="defaultSource">Default sequence to return in case there's no corresponding source for the computed
	/// selector value.</param>
	/// <returns>The source sequence corresponding with the evaluated selector value; otherwise, an empty
	/// sequence.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="selector"/>, <paramref name="sources"/>, or <paramref
	/// name="defaultSource"/> is <see langword="null"/>.</exception>
	/// <remarks>
	/// <paramref name="selector"/> is not evaluated until enumeration. The value returned will be used to select a
	/// sequence from <paramref name="sources"/>; enumeration will continue with items from that sequence.
	/// </remarks>
	public static IAsyncEnumerable<TResult> Case<TValue, TResult>(
		Func<ValueTask<TValue>> selector,
		IDictionary<TValue, IAsyncEnumerable<TResult>> sources,
		IAsyncEnumerable<TResult> defaultSource)
		where TValue : notnull
	{
		ArgumentNullException.ThrowIfNull(selector);
		ArgumentNullException.ThrowIfNull(sources);
		ArgumentNullException.ThrowIfNull(defaultSource);

		return Core(selector, sources, defaultSource);

		static async IAsyncEnumerable<TResult> Core(
			Func<ValueTask<TValue>> selector,
			IDictionary<TValue, IAsyncEnumerable<TResult>> sources,
			IAsyncEnumerable<TResult> defaultSource,
			[EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			var selection = await selector().ConfigureAwait(false);
			if (!sources.TryGetValue(selection, out var source))
				source = defaultSource;

			await foreach (var el in source.WithCancellation(cancellationToken).ConfigureAwait(false))
				yield return el;
		}
	}
}
