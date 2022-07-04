namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Determines whether the end of the first sequence is equivalent to
	/// the second sequence, using the default equality comparer.
	/// </summary>
	/// <typeparam name="T">Type of elements.</typeparam>
	/// <param name="first">The sequence to check.</param>
	/// <param name="second">The sequence to compare to.</param>
	/// <returns>
	/// <see langword="true"/> if <paramref name="first" /> ends with elements
	/// equivalent to <paramref name="second" />.
	/// </returns>
	/// <remarks>
	/// This is the <see cref="IAsyncEnumerable{T}" /> equivalent of
	/// <see cref="string.EndsWith(string)" /> and
	/// it calls <see cref="IEqualityComparer{T}.Equals(T,T)" /> using
	/// <see cref="EqualityComparer{T}.Default" /> on pairs of elements at
	/// the same index.
	/// </remarks>

	public static ValueTask<bool> EndsWith<T>(this IAsyncEnumerable<T> first, IEnumerable<T> second)
	{
		return EndsWith(first, second.ToAsyncEnumerable(), comparer: null);
	}

	/// <summary>
	/// Determines whether the end of the first sequence is equivalent to
	/// the second sequence, using the default equality comparer.
	/// </summary>
	/// <typeparam name="T">Type of elements.</typeparam>
	/// <param name="first">The sequence to check.</param>
	/// <param name="second">The sequence to compare to.</param>
	/// <returns>
	/// <see langword="true"/> if <paramref name="first" /> ends with elements
	/// equivalent to <paramref name="second" />.
	/// </returns>
	/// <remarks>
	/// This is the <see cref="IAsyncEnumerable{T}" /> equivalent of
	/// <see cref="string.EndsWith(string)" /> and
	/// it calls <see cref="IEqualityComparer{T}.Equals(T,T)" /> using
	/// <see cref="EqualityComparer{T}.Default" /> on pairs of elements at
	/// the same index.
	/// </remarks>

	public static ValueTask<bool> EndsWith<T>(this IAsyncEnumerable<T> first, IAsyncEnumerable<T> second)
	{
		return EndsWith(first, second, comparer: null);
	}

	/// <summary>
	/// Determines whether the end of the first sequence is equivalent to
	/// the second sequence, using the specified element equality comparer.
	/// </summary>
	/// <typeparam name="T">Type of elements.</typeparam>
	/// <param name="first">The sequence to check.</param>
	/// <param name="second">The sequence to compare to.</param>
	/// <param name="comparer">Equality comparer to use.</param>
	/// <returns>
	/// <see langword="true"/> if <paramref name="first" /> ends with elements
	/// equivalent to <paramref name="second" />.
	/// </returns>
	/// <remarks>
	/// This is the <see cref="IAsyncEnumerable{T}" /> equivalent of
	/// <see cref="string.EndsWith(string)" /> and it calls
	/// <see cref="IEqualityComparer{T}.Equals(T,T)" /> on pairs of
	/// elements at the same index.
	/// </remarks>

	public static ValueTask<bool> EndsWith<T>(this IAsyncEnumerable<T> first, IEnumerable<T> second, IEqualityComparer<T>? comparer)
	{
		return EndsWith(first, second.ToAsyncEnumerable(), comparer);
	}

	/// <summary>
	/// Determines whether the end of the first sequence is equivalent to
	/// the second sequence, using the specified element equality comparer.
	/// </summary>
	/// <typeparam name="T">Type of elements.</typeparam>
	/// <param name="first">The sequence to check.</param>
	/// <param name="second">The sequence to compare to.</param>
	/// <param name="comparer">Equality comparer to use.</param>
	/// <returns>
	/// <see langword="true"/> if <paramref name="first" /> ends with elements
	/// equivalent to <paramref name="second" />.
	/// </returns>
	/// <remarks>
	/// This is the <see cref="IAsyncEnumerable{T}" /> equivalent of
	/// <see cref="string.EndsWith(string)" /> and it calls
	/// <see cref="IEqualityComparer{T}.Equals(T,T)" /> on pairs of
	/// elements at the same index.
	/// </remarks>

	public static async ValueTask<bool> EndsWith<T>(this IAsyncEnumerable<T> first, IAsyncEnumerable<T> second, IEqualityComparer<T>? comparer)
	{
		first.ThrowIfNull();
		second.ThrowIfNull();

		comparer ??= EqualityComparer<T>.Default;

		var snd = await second.ToListAsync().ConfigureAwait(false);
		return await first.TakeLast(snd.Count)
			.SequenceEqualAsync(snd.ToAsyncEnumerable(), comparer)
			.ConfigureAwait(false);
	}
}
