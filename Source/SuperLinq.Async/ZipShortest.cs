namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Returns a projection of tuples, where each tuple contains the N-th
	/// element from each of the argument sequences. The resulting sequence
	/// is as short as the shortest input sequence.
	/// </summary>
	/// <typeparam name="TFirst">Type of elements in first sequence.</typeparam>
	/// <typeparam name="TSecond">Type of elements in second sequence.</typeparam>
	/// <typeparam name="TResult">Type of elements in result sequence.</typeparam>
	/// <param name="first">The first sequence.</param>
	/// <param name="second">The second sequence.</param>
	/// <param name="resultSelector">
	/// Function to apply to each pair of elements.</param>
	/// <returns>
	/// A projection of tuples, where each tuple contains the N-th element
	/// from each of the argument sequences.</returns>
	/// <example>
	/// <code><![CDATA[
	/// var numbers = new[] { 1, 2, 3 };
	/// var letters = new[] { "A", "B", "C", "D" };
	/// var zipped = numbers.ZipShortest(letters, (n, l) => n + l);
	/// ]]></code>
	/// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
	/// </example>
	/// <remarks>
	/// <para>
	/// If the input sequences are of different lengths, the result sequence
	/// is terminated as soon as the shortest input sequence is exhausted
	/// and remainder elements from the longer sequences are never consumed.
	/// </para>
	/// <para>
	/// This operator uses deferred execution and streams its results.</para>
	/// </remarks>
	/// <exception cref="ArgumentNullException"><paramref name="first"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="resultSelector"/> is <see langword="null"/>.</exception>
	public static IAsyncEnumerable<TResult> ZipShortest<TFirst, TSecond, TResult>(
		this IAsyncEnumerable<TFirst> first,
		IAsyncEnumerable<TSecond> second,
		Func<TFirst, TSecond, TResult> resultSelector)
	{
		first.ThrowIfNull();
		second.ThrowIfNull();
		resultSelector.ThrowIfNull();

		return ZipShortestImpl(first, second, AsyncEnumerableEx.Repeat(default(object)), AsyncEnumerableEx.Repeat(default(object)), (a, b, c, d) => resultSelector(a, b));
	}

	/// <summary>
	/// Returns a projection of tuples, where each tuple contains the N-th
	/// element from each of the argument sequences. The resulting sequence
	/// is as short as the shortest input sequence.
	/// </summary>
	/// <typeparam name="T1">Type of elements in first sequence.</typeparam>
	/// <typeparam name="T2">Type of elements in second sequence.</typeparam>
	/// <typeparam name="T3">Type of elements in third sequence.</typeparam>
	/// <typeparam name="TResult">Type of elements in result sequence.</typeparam>
	/// <param name="first">First sequence</param>
	/// <param name="second">Second sequence</param>
	/// <param name="third">Third sequence</param>
	/// <param name="resultSelector">
	/// Function to apply to each triplet of elements.</param>
	/// <returns>
	/// A projection of tuples, where each tuple contains the N-th element
	/// from each of the argument sequences.</returns>
	/// <example>
	/// <code><![CDATA[
	/// var numbers = new[] { 1, 2, 3 };
	/// var letters = new[] { "A", "B", "C", "D" };
	/// var chars   = new[] { 'a', 'b', 'c', 'd', 'e' };
	/// var zipped  = numbers.ZipShortest(letters, chars, (n, l, c) => c + n + l);
	/// ]]></code>
	/// The <c>zipped</c> variable, when iterated over, will yield
	/// "98A", "100B", "102C", in turn.
	/// </example>
	/// <remarks>
	/// <para>
	/// If the input sequences are of different lengths, the result sequence
	/// is terminated as soon as the shortest input sequence is exhausted
	/// and remainder elements from the longer sequences are never consumed.
	/// </para>
	/// <para>
	/// This operator uses deferred execution and streams its results.</para>
	/// </remarks>
	/// <exception cref="ArgumentNullException"><paramref name="first"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="third"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="resultSelector"/> is <see langword="null"/>.</exception>
	public static IAsyncEnumerable<TResult> ZipShortest<T1, T2, T3, TResult>(
		this IAsyncEnumerable<T1> first,
		IAsyncEnumerable<T2> second,
		IAsyncEnumerable<T3> third,
		Func<T1, T2, T3, TResult> resultSelector)
	{
		first.ThrowIfNull();
		second.ThrowIfNull();
		third.ThrowIfNull();
		resultSelector.ThrowIfNull();

		return ZipShortestImpl(first, second, third, AsyncEnumerableEx.Repeat(default(object)), (a, b, c, _) => resultSelector(a, b, c));
	}

	/// <summary>
	/// Returns a projection of tuples, where each tuple contains the N-th
	/// element from each of the argument sequences. The resulting sequence
	/// is as short as the shortest input sequence.
	/// </summary>
	/// <typeparam name="T1">Type of elements in first sequence.</typeparam>
	/// <typeparam name="T2">Type of elements in second sequence.</typeparam>
	/// <typeparam name="T3">Type of elements in third sequence.</typeparam>
	/// <typeparam name="T4">Type of elements in fourth sequence.</typeparam>
	/// <typeparam name="TResult">Type of elements in result sequence.</typeparam>
	/// <param name="first">The first sequence.</param>
	/// <param name="second">The second sequence.</param>
	/// <param name="third">The third sequence.</param>
	/// <param name="fourth">The fourth sequence.</param>
	/// <param name="resultSelector">
	/// Function to apply to each quadruplet of elements.</param>
	/// <returns>
	/// A projection of tuples, where each tuple contains the N-th element
	/// from each of the argument sequences.</returns>
	/// <example>
	/// <code><![CDATA[
	/// var numbers = new[] { 1, 2, 3 };
	/// var letters = new[] { "A", "B", "C", "D" };
	/// var chars   = new[] { 'a', 'b', 'c', 'd', 'e' };
	/// var flags   = new[] { true, false };
	/// var zipped  = numbers.ZipShortest(letters, chars, flags, (n, l, c, f) => n + l + c + f);
	/// ]]></code>
	/// The <c>zipped</c> variable, when iterated over, will yield
	/// "1AaTrue", "2BbFalse" in turn.
	/// </example>
	/// <remarks>
	/// <para>
	/// If the input sequences are of different lengths, the result sequence
	/// is terminated as soon as the shortest input sequence is exhausted
	/// and remainder elements from the longer sequences are never consumed.
	/// </para>
	/// <para>
	/// This operator uses deferred execution and streams its results.</para>
	/// </remarks>
	/// <exception cref="ArgumentNullException"><paramref name="first"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="third"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="fourth"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="resultSelector"/> is <see langword="null"/>.</exception>
	public static IAsyncEnumerable<TResult> ZipShortest<T1, T2, T3, T4, TResult>(
		this IAsyncEnumerable<T1> first,
		IAsyncEnumerable<T2> second,
		IAsyncEnumerable<T3> third,
		IAsyncEnumerable<T4> fourth,
		Func<T1, T2, T3, T4, TResult> resultSelector)
	{
		first.ThrowIfNull();
		second.ThrowIfNull();
		third.ThrowIfNull();
		fourth.ThrowIfNull();
		resultSelector.ThrowIfNull();

		return ZipShortestImpl(first, second, third, fourth, resultSelector);
	}

	private static async IAsyncEnumerable<TResult> ZipShortestImpl<T1, T2, T3, T4, TResult>(
		IAsyncEnumerable<T1> s1,
		IAsyncEnumerable<T2> s2,
		IAsyncEnumerable<T3> s3,
		IAsyncEnumerable<T4> s4,
		Func<T1, T2, T3, T4, TResult> resultSelector,
		[EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		await using var e1 = s1.GetConfiguredAsyncEnumerator(cancellationToken);
		await using var e2 = s2.GetConfiguredAsyncEnumerator(cancellationToken);
		await using var e3 = s3.GetConfiguredAsyncEnumerator(cancellationToken);
		await using var e4 = s4.GetConfiguredAsyncEnumerator(cancellationToken);

		while (true)
		{
			if (!await e1.MoveNextAsync())
				yield break;
			if (!await e2.MoveNextAsync())
				yield break;
			if (!await e3.MoveNextAsync())
				yield break;
			if (!await e4.MoveNextAsync())
				yield break;

			yield return resultSelector(
				e1.Current,
				e2.Current,
				e3.Current,
				e4.Current);
		}
	}
}
