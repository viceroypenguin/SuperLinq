namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Determines whether or not the number of elements in the sequence is greater than
	/// or equal to the given integer.
	/// </summary>
	/// <typeparam name="T">Element type of sequence</typeparam>
	/// <param name="source">The source sequence</param>
	/// <param name="count">The minimum number of items a sequence must have for this
	/// function to return true</param>
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is negative</exception>
	/// <returns><see langword="true"/> if the number of elements in the sequence is greater than
	/// or equal to the given integer or <see langword="false"/> otherwise.</returns>
	/// <example>
	/// <code><![CDATA[
	/// var numbers = new[] { 123, 456, 789 };
	/// var result = numbers.AtLeast(2);
	/// ]]></code>
	/// The <c>result</c> variable will contain <see langword="true"/>.
	/// </example>

	public static ValueTask<bool> AtLeast<T>(this IAsyncEnumerable<T> source, int count, CancellationToken cancellationToken = default)
	{
		Guard.IsNotNull(source);
		Guard.IsGreaterThanOrEqualTo(count, 0);

		return QuantityIterator(source, limit: count, min: count, max: int.MaxValue, cancellationToken);
	}

	/// <summary>
	/// Determines whether or not the number of elements in the sequence is lesser than
	/// or equal to the given integer.
	/// </summary>
	/// <typeparam name="T">Element type of sequence</typeparam>
	/// <param name="source">The source sequence</param>
	/// <param name="count">The maximum number of items a sequence must have for this
	/// function to return true</param>
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is negative</exception>
	/// <returns><see langword="true"/> if the number of elements in the sequence is lesser than
	/// or equal to the given integer or <see langword="false"/> otherwise.</returns>
	/// <example>
	/// <code><![CDATA[
	/// var numbers = new[] { 123, 456, 789 };
	/// var result = numbers.AtMost(2);
	/// ]]></code>
	/// The <c>result</c> variable will contain <see langword="false"/>.
	/// </example>

	public static ValueTask<bool> AtMost<T>(this IAsyncEnumerable<T> source, int count, CancellationToken cancellationToken = default)
	{
		Guard.IsNotNull(source);
		Guard.IsGreaterThanOrEqualTo(count, 0);

		return QuantityIterator(source, limit: count + 1, min: 0, max: count, cancellationToken);
	}

	/// <summary>
	/// Determines whether or not the number of elements in the sequence is equals to the given integer.
	/// </summary>
	/// <typeparam name="T">Element type of sequence</typeparam>
	/// <param name="source">The source sequence</param>
	/// <param name="count">The exactly number of items a sequence must have for this
	/// function to return true</param>
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is negative</exception>
	/// <returns><see langword="true"/> if the number of elements in the sequence is equals
	/// to the given integer or <see langword="false"/> otherwise.</returns>
	/// <example>
	/// <code><![CDATA[
	/// var numbers = new[] { 123, 456, 789 };
	/// var result = numbers.Exactly(3);
	/// ]]></code>
	/// The <c>result</c> variable will contain <see langword="true"/>.
	/// </example>

	public static ValueTask<bool> Exactly<T>(this IAsyncEnumerable<T> source, int count, CancellationToken cancellationToken = default)
	{
		Guard.IsNotNull(source);
		Guard.IsGreaterThanOrEqualTo(count, 0);

		return QuantityIterator(source, limit: count + 1, min: count, max: count, cancellationToken);
	}

	/// <summary>
	/// Determines whether or not the number of elements in the sequence is between
	/// an inclusive range of minimum and maximum integers.
	/// </summary>
	/// <typeparam name="T">Element type of sequence</typeparam>
	/// <param name="source">The source sequence</param>
	/// <param name="min">The minimum number of items a sequence must have for this
	/// function to return true</param>
	/// <param name="max">The maximum number of items a sequence must have for this
	/// function to return true</param>
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="min"/> is negative or <paramref name="max"/> is less than min</exception>
	/// <returns><see langword="true"/> if the number of elements in the sequence is between (inclusive)
	/// the min and max given integers or <see langword="false"/> otherwise.</returns>
	/// <example>
	/// <code><![CDATA[
	/// var numbers = new[] { 123, 456, 789 };
	/// var result = numbers.CountBetween(1, 2);
	/// ]]></code>
	/// The <c>result</c> variable will contain <see langword="false"/>.
	/// </example>

	public static ValueTask<bool> CountBetween<T>(this IAsyncEnumerable<T> source, int min, int max, CancellationToken cancellationToken = default)
	{
		Guard.IsNotNull(source);
		Guard.IsGreaterThanOrEqualTo(min, 0);
		Guard.IsGreaterThanOrEqualTo(max, min);

		return QuantityIterator(source, limit: max + 1, min: min, max: max, cancellationToken);
	}

	private static async ValueTask<bool> QuantityIterator<T>(IAsyncEnumerable<T> source, int limit, int min, int max, CancellationToken cancellationToken)
	{
		var count = 0;
		await foreach (var i in source.WithCancellation(cancellationToken).ConfigureAwait(false))
		{
			if (++count >= limit)
				break;
		}

		return count >= min && count <= max;
	}

	/// <summary>
	/// Compares two sequences and returns an integer that indicates whether the first sequence
	/// has fewer, the same or more elements than the second sequence.
	/// </summary>
	/// <typeparam name="TFirst">Element type of the first sequence</typeparam>
	/// <typeparam name="TSecond">Element type of the second sequence</typeparam>
	/// <param name="first">The first sequence</param>
	/// <param name="second">The second sequence</param>
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
	/// <exception cref="ArgumentNullException"><paramref name="first"/> is null</exception>
	/// <exception cref="ArgumentNullException"><paramref name="second"/> is null</exception>
	/// <returns><c>-1</c> if the first sequence has the fewest elements, <c>0</c> if the two sequences have the same number of elements
	/// or <c>1</c> if the first sequence has the most elements.</returns>
	/// <example>
	/// <code><![CDATA[
	/// var first = new[] { 123, 456 };
	/// var second = new[] { 789 };
	/// var result = first.CompareCount(second);
	/// ]]></code>
	/// The <c>result</c> variable will contain <c>1</c>.
	/// </example>

	public static async ValueTask<int> CompareCount<TFirst, TSecond>(this IAsyncEnumerable<TFirst> first, IAsyncEnumerable<TSecond> second, CancellationToken cancellationToken = default)
	{
		Guard.IsNotNull(first);
		Guard.IsNotNull(second);

		bool firstHasNext;
		bool secondHasNext;

		await using var e1 = first.GetConfiguredAsyncEnumerator(cancellationToken);
		await using var e2 = second.GetConfiguredAsyncEnumerator(cancellationToken);

		{
			do
			{
				firstHasNext = await e1.MoveNextAsync();
				secondHasNext = await e2.MoveNextAsync();
			}
			while (firstHasNext && secondHasNext);
		}

		return firstHasNext.CompareTo(secondHasNext);
	}
}
