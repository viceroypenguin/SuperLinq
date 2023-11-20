namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Returns a sequence of values based on indexes.
	/// </summary>
	/// <typeparam name="TResult">
	/// The type of the value returned by <paramref name="generator"/>
	/// and therefore the elements of the generated sequence.</typeparam>
	/// <param name="generator">
	/// Generation function to apply to each index.</param>
	/// <returns>A sequence of generated results.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="generator"/> is <see langword="null"/>.</exception>
	/// <remarks>
	/// <para>
	/// The sequence is (practically) infinite where the index ranges from
	/// zero to <see cref="int.MaxValue"/> inclusive.</para>
	/// <para>
	/// This function defers execution and streams the results.</para>
	/// </remarks>
	public static IAsyncEnumerable<TResult> GenerateByIndex<TResult>(Func<int, TResult> generator)
	{
		ArgumentNullException.ThrowIfNull(generator);

		return Sequence(0, int.MaxValue)
			.Select(generator);
	}

	/// <summary>
	/// Returns a sequence of values based on indexes.
	/// </summary>
	/// <typeparam name="TResult">
	/// The type of the value returned by <paramref name="generator"/>
	/// and therefore the elements of the generated sequence.</typeparam>
	/// <param name="generator">
	/// Generation function to apply to each index.</param>
	/// <returns>A sequence of generated results.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="generator"/> is <see langword="null"/>.</exception>
	/// <remarks>
	/// <para>
	/// The sequence is (practically) infinite where the index ranges from
	/// zero to <see cref="int.MaxValue"/> inclusive.</para>
	/// <para>
	/// This function defers execution and streams the results.</para>
	/// </remarks>
	public static IAsyncEnumerable<TResult> GenerateByIndex<TResult>(Func<int, ValueTask<TResult>> generator)
	{
		ArgumentNullException.ThrowIfNull(generator);

		return Sequence(0, int.MaxValue)
			.SelectAwait(generator);
	}

	/// <summary>
	/// Returns a sequence of values based on indexes.
	/// </summary>
	/// <typeparam name="TResult">
	/// The type of the value returned by <paramref name="generator"/>
	/// and therefore the elements of the generated sequence.</typeparam>
	/// <param name="generator">
	/// Generation function to apply to each index.</param>
	/// <returns>A sequence of generated results.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="generator"/> is <see langword="null"/>.</exception>
	/// <remarks>
	/// <para>
	/// The sequence is (practically) infinite where the index ranges from
	/// zero to <see cref="int.MaxValue"/> inclusive.</para>
	/// <para>
	/// This function defers execution and streams the results.</para>
	/// </remarks>
	public static IAsyncEnumerable<TResult> GenerateByIndex<TResult>(Func<int, CancellationToken, ValueTask<TResult>> generator)
	{
		ArgumentNullException.ThrowIfNull(generator);

		return Sequence(0, int.MaxValue)
			.SelectAwaitWithCancellation(generator);
	}
}
