namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Returns a projection of tuples, where each tuple contains the N-th
	/// element from each of the argument sequences. The resulting sequence
	/// will always be as long as the longest of input sequences where the
	/// default value of each of the shorter sequence element types is used
	/// for padding.
	/// </summary>
	/// <typeparam name="TFirst">Type of elements in first sequence.</typeparam>
	/// <typeparam name="TSecond">Type of elements in second sequence.</typeparam>
	/// <typeparam name="TResult">Type of elements in result sequence.</typeparam>
	/// <param name="first">The first sequence.</param>
	/// <param name="second">The second sequence.</param>
	/// <param name="resultSelector">
	/// Function to apply to each pair of elements.</param>
	/// <returns>
	/// A sequence that contains elements of the two input sequences,
	/// combined by <paramref name="resultSelector"/>.
	/// </returns>
	/// <example>
	/// <code><![CDATA[
	/// var numbers = { 1, 2, 3 };
	/// var letters = { "A", "B", "C", "D" };
	/// var zipped = numbers.ZipLongest(letters, (n, l) => n + l);
	/// ]]></code>
	/// The <c>zipped</c> variable, when iterated over, will yield "1A",
	/// "2B", "3C", "0D" in turn.
	/// </example>
	/// <remarks>
	/// This operator uses deferred execution and streams its results.
	/// </remarks>
	/// <exception cref="ArgumentNullException"><paramref name="first"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="resultSelector"/> is <see langword="null"/>.</exception>
	public static IAsyncEnumerable<TResult> ZipLongest<TFirst, TSecond, TResult>(
		this IAsyncEnumerable<TFirst> first,
		IAsyncEnumerable<TSecond> second,
		Func<TFirst, TSecond, TResult> resultSelector)
	{
		Guard.IsNotNull(first);
		Guard.IsNotNull(second);
		Guard.IsNotNull(resultSelector);

		return ZipLongestImpl(first, second, AsyncEnumerableEx.Repeat(default(object)), AsyncEnumerableEx.Repeat(default(object)), (a, b, c, d) => resultSelector(a, b), 2);
	}

	/// <summary>
	/// Returns a projection of tuples, where each tuple contains the N-th
	/// element from each of the argument sequences. The resulting sequence
	/// will always be as long as the longest of input sequences where the
	/// default value of each of the shorter sequence element types is used
	/// for padding.
	/// </summary>
	/// <typeparam name="T1">Type of elements in first sequence.</typeparam>
	/// <typeparam name="T2">Type of elements in second sequence.</typeparam>
	/// <typeparam name="T3">Type of elements in third sequence.</typeparam>
	/// <typeparam name="TResult">Type of elements in result sequence.</typeparam>
	/// <param name="first">The first sequence.</param>
	/// <param name="second">The second sequence.</param>
	/// <param name="third">The third sequence.</param>
	/// <param name="resultSelector">
	/// Function to apply to each triplet of elements.</param>
	/// <returns>
	/// A sequence that contains elements of the three input sequences,
	/// combined by <paramref name="resultSelector"/>.
	/// </returns>
	/// <example>
	/// <code><![CDATA[
	/// var numbers = new[] { 1, 2, 3 };
	/// var letters = new[] { "A", "B", "C", "D" };
	/// var chars   = new[] { 'a', 'b', 'c', 'd', 'e' };
	/// var zipped  = numbers.ZipLongest(letters, chars, (n, l, c) => n + l + c);
	/// ]]></code>
	/// The <c>zipped</c> variable, when iterated over, will yield "1Aa",
	/// "2Bb", "3Cc", "0Dd", "0e" in turn.
	/// </example>
	/// <remarks>
	/// This operator uses deferred execution and streams its results.
	/// </remarks>
	/// <exception cref="ArgumentNullException"><paramref name="first"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="third"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="resultSelector"/> is <see langword="null"/>.</exception>
	public static IAsyncEnumerable<TResult> ZipLongest<T1, T2, T3, TResult>(
		this IAsyncEnumerable<T1> first,
		IAsyncEnumerable<T2> second,
		IAsyncEnumerable<T3> third,
		Func<T1, T2, T3, TResult> resultSelector)
	{
		Guard.IsNotNull(first);
		Guard.IsNotNull(second);
		Guard.IsNotNull(third);
		Guard.IsNotNull(resultSelector);

		return ZipLongestImpl(first, second, third, AsyncEnumerableEx.Repeat(default(object)), (a, b, c, d) => resultSelector(a, b, c), 3);
	}

	/// <summary>
	/// Returns a projection of tuples, where each tuple contains the N-th
	/// element from each of the argument sequences. The resulting sequence
	/// will always be as long as the longest of input sequences where the
	/// default value of each of the shorter sequence element types is used
	/// for padding.
	/// </summary>
	/// <typeparam name="T1">Type of elements in first sequence</typeparam>
	/// <typeparam name="T2">Type of elements in second sequence</typeparam>
	/// <typeparam name="T3">Type of elements in third sequence</typeparam>
	/// <typeparam name="T4">Type of elements in fourth sequence</typeparam>
	/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
	/// <param name="first">The first sequence.</param>
	/// <param name="second">The second sequence.</param>
	/// <param name="third">The third sequence.</param>
	/// <param name="fourth">The fourth sequence.</param>
	/// <param name="resultSelector">
	/// Function to apply to each quadruplet of elements.</param>
	/// <returns>
	/// A sequence that contains elements of the four input sequences,
	/// combined by <paramref name="resultSelector"/>.
	/// </returns>
	/// <example>
	/// <code><![CDATA[
	/// var numbers = new[] { 1, 2, 3 };
	/// var letters = new[] { "A", "B", "C", "D" };
	/// var chars   = new[] { 'a', 'b', 'c', 'd', 'e' };
	/// var flags   = new[] { true, false, true, false, true, false };
	/// var zipped  = numbers.ZipLongest(letters, chars, flags, (n, l, c, f) => n + l + c + f);
	/// ]]></code>
	/// The <c>zipped</c> variable, when iterated over, will yield "1AaTrue",
	/// "2BbFalse", "3CcTrue", "0DdFalse", "0eTrue", "0\0False" in turn.
	/// </example>
	/// <remarks>
	/// This operator uses deferred execution and streams its results.
	/// </remarks>
	/// <exception cref="ArgumentNullException"><paramref name="first"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="third"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="fourth"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="resultSelector"/> is <see langword="null"/>.</exception>
	public static IAsyncEnumerable<TResult> ZipLongest<T1, T2, T3, T4, TResult>(
		this IAsyncEnumerable<T1> first,
		IAsyncEnumerable<T2> second,
		IAsyncEnumerable<T3> third,
		IAsyncEnumerable<T4> fourth,
		Func<T1, T2, T3, T4, TResult> resultSelector)
	{
		Guard.IsNotNull(first);
		Guard.IsNotNull(second);
		Guard.IsNotNull(third);
		Guard.IsNotNull(fourth);
		Guard.IsNotNull(resultSelector);

		return ZipLongestImpl(first, second, third, fourth, resultSelector, 4);
	}

	private static async IAsyncEnumerable<TResult> ZipLongestImpl<T1, T2, T3, T4, TResult>(
		IAsyncEnumerable<T1> s1,
		IAsyncEnumerable<T2> s2,
		IAsyncEnumerable<T3> s3,
		IAsyncEnumerable<T4> s4,
		Func<T1, T2, T3, T4, TResult> resultSelector,
		int limit,
		[EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		await using var e1 = s1.GetConfiguredAsyncEnumerator(cancellationToken);
		await using var e2 = s2.GetConfiguredAsyncEnumerator(cancellationToken);
		await using var e3 = s3.GetConfiguredAsyncEnumerator(cancellationToken);
		await using var e4 = s4.GetConfiguredAsyncEnumerator(cancellationToken);

		while (true)
		{
			var terms = 4;

			var v1 = default(T1)!;
			if (await e1.MoveNextAsync())
			{
				v1 = e1.Current;
				terms--;
			}

			var v2 = default(T2)!;
			if (await e2.MoveNextAsync())
			{
				v2 = e2.Current;
				terms--;
			}

			var v3 = default(T3)!;
			if (await e3.MoveNextAsync())
			{
				v3 = e3.Current;
				terms--;
			}

			var v4 = default(T4)!;
			if (await e4.MoveNextAsync())
			{
				v4 = e4.Current;
				terms--;
			}

			if (terms >= limit)
				yield break;

			yield return resultSelector(v1, v2, v3, v4);
		}
	}
}
