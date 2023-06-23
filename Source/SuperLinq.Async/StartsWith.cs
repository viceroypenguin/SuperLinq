namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Determines whether the beginning of the first sequence is
	/// equivalent to the second sequence, using the default equality
	/// comparer.
	/// </summary>
	/// <typeparam name="T">Type of elements.</typeparam>
	/// <param name="first">The sequence to check.</param>
	/// <param name="second">The sequence to compare to.</param>
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
	/// <returns>
	/// <see langword="true"/> if <paramref name="first" /> begins with elements
	/// equivalent to <paramref name="second" />.
	/// </returns>
	/// <remarks>
	/// This is the <see cref="IAsyncEnumerable{T}" /> equivalent of
	/// <see cref="string.StartsWith(string)" /> and it calls
	/// <see cref="IEqualityComparer{T}.Equals(T,T)" /> using
	/// <see cref="EqualityComparer{T}.Default"/> on pairs of elements at
	/// the same index.
	/// </remarks>

	public static ValueTask<bool> StartsWith<T>(this IAsyncEnumerable<T> first, IEnumerable<T> second, CancellationToken cancellationToken = default)
	{
		Guard.IsNotNull(first);
		Guard.IsNotNull(second);

		return StartsWith(first, second.ToAsyncEnumerable(), comparer: null, cancellationToken);
	}

	/// <summary>
	/// Determines whether the beginning of the first sequence is
	/// equivalent to the second sequence, using the default equality
	/// comparer.
	/// </summary>
	/// <typeparam name="T">Type of elements.</typeparam>
	/// <param name="first">The sequence to check.</param>
	/// <param name="second">The sequence to compare to.</param>
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
	/// <returns>
	/// <see langword="true"/> if <paramref name="first" /> begins with elements
	/// equivalent to <paramref name="second" />.
	/// </returns>
	/// <remarks>
	/// This is the <see cref="IAsyncEnumerable{T}" /> equivalent of
	/// <see cref="string.StartsWith(string)" /> and it calls
	/// <see cref="IEqualityComparer{T}.Equals(T,T)" /> using
	/// <see cref="EqualityComparer{T}.Default"/> on pairs of elements at
	/// the same index.
	/// </remarks>

	public static ValueTask<bool> StartsWith<T>(this IAsyncEnumerable<T> first, IAsyncEnumerable<T> second, CancellationToken cancellationToken = default)
	{
		return StartsWith(first, second, comparer: null, cancellationToken);
	}

	/// <summary>
	/// Determines whether the beginning of the first sequence is
	/// equivalent to the second sequence, using the specified element
	/// equality comparer.
	/// </summary>
	/// <typeparam name="T">Type of elements.</typeparam>
	/// <param name="first">The sequence to check.</param>
	/// <param name="second">The sequence to compare to.</param>
	/// <param name="comparer">Equality comparer to use.</param>
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
	/// <returns>
	/// <see langword="true"/> if <paramref name="first" /> begins with elements
	/// equivalent to <paramref name="second" />.
	/// </returns>
	/// <remarks>
	/// This is the <see cref="IAsyncEnumerable{T}" /> equivalent of
	/// <see cref="string.StartsWith(string)" /> and
	/// it calls <see cref="IEqualityComparer{T}.Equals(T,T)" /> on pairs
	/// of elements at the same index.
	/// </remarks>

	public static ValueTask<bool> StartsWith<T>(this IAsyncEnumerable<T> first, IEnumerable<T> second, IEqualityComparer<T>? comparer, CancellationToken cancellationToken = default)
	{
		Guard.IsNotNull(first);
		Guard.IsNotNull(second);

		return StartsWith(first, second.ToAsyncEnumerable(), comparer, cancellationToken);
	}

	/// <summary>
	/// Determines whether the beginning of the first sequence is
	/// equivalent to the second sequence, using the specified element
	/// equality comparer.
	/// </summary>
	/// <typeparam name="T">Type of elements.</typeparam>
	/// <param name="first">The sequence to check.</param>
	/// <param name="second">The sequence to compare to.</param>
	/// <param name="comparer">Equality comparer to use.</param>
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
	/// <returns>
	/// <see langword="true"/> if <paramref name="first" /> begins with elements
	/// equivalent to <paramref name="second" />.
	/// </returns>
	/// <remarks>
	/// This is the <see cref="IAsyncEnumerable{T}" /> equivalent of
	/// <see cref="string.StartsWith(string)" /> and
	/// it calls <see cref="IEqualityComparer{T}.Equals(T,T)" /> on pairs
	/// of elements at the same index.
	/// </remarks>

	public static ValueTask<bool> StartsWith<T>(this IAsyncEnumerable<T> first, IAsyncEnumerable<T> second, IEqualityComparer<T>? comparer, CancellationToken cancellationToken = default)
	{
		Guard.IsNotNull(first);
		Guard.IsNotNull(second);

		comparer ??= EqualityComparer<T>.Default;

		return Core(first, second, comparer, cancellationToken);

		static async ValueTask<bool> Core(IAsyncEnumerable<T> first, IAsyncEnumerable<T> second, IEqualityComparer<T>? comparer, CancellationToken cancellationToken)
		{
			var snd = await second.ToListAsync(cancellationToken).ConfigureAwait(false);
			return await first.Take(snd.Count)
				.SequenceEqualAsync(
					snd.ToAsyncEnumerable(),
					comparer,
					cancellationToken)
				.ConfigureAwait(false);
		}
	}
}
