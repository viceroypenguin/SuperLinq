namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	///	    Performs an inner join on two heterogeneous sequences.
	/// </summary>
	/// <typeparam name="TLeft">
	///	    The type of elements in the first sequence.
	/// </typeparam>
	/// <typeparam name="TRight">
	///	    The type of elements in the second sequence.
	/// </typeparam>
	/// <typeparam name="TKey">
	///	    The type of the key returned by the key selector functions.
	/// </typeparam>
	/// <param name="left">
	///	    The first sequence.
	/// </param>
	/// <param name="right">
	///	    The second sequence.
	/// </param>
	/// <param name="leftKeySelector">
	///	    A function to extract the join key from each element of the first sequence.
	/// </param>
	/// <param name="rightKeySelector">
	///	    A function to extract the join key from each element of the second sequence.
	/// </param>
	/// <param name="comparer">
	///	    An <see cref="IEqualityComparer{T}"/> to hash and compare keys.
	/// </param>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="left"/>, <paramref name="right"/>, <paramref name="leftKeySelector"/>, or <paramref
	///     name="rightKeySelector"/> is <see langword="null" />.
	/// </exception>
	/// <returns>
	///	    A sequence containing values from an inner join of the two input sequences.
	/// </returns>
	/// <remarks>
	/// <para>
	///	    The result of this method is an `inner`-join. Values are only returned if matching elements are found in
	///     both <paramref name="left"/> and <paramref name="right"/> sequences.
	/// </para>
	/// <para>
	///	    This method is implemented using a `loop`-join. The sequence <paramref name="right"/> is cached, and
	///	    for-each element of <paramref name="left"/>, the cached sequence is enumerated for matching elements.
	/// </para>
	/// <para>
	///	    This method uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IAsyncEnumerable<(TLeft Left, TRight Right)> InnerLoopJoin<TLeft, TRight, TKey>(
		this IAsyncEnumerable<TLeft> left,
		IAsyncEnumerable<TRight> right,
		Func<TLeft, TKey> leftKeySelector,
		Func<TRight, TKey> rightKeySelector,
		IEqualityComparer<TKey>? comparer = null)
	{
		ArgumentNullException.ThrowIfNull(left);
		ArgumentNullException.ThrowIfNull(right);
		ArgumentNullException.ThrowIfNull(leftKeySelector);
		ArgumentNullException.ThrowIfNull(rightKeySelector);

		return LoopJoin(
			left, right,
			leftKeySelector, rightKeySelector,
			leftResultSelector: default,
			ValueTuple.Create,
			comparer);
	}

	/// <summary>
	///	    Performs an inner join on two heterogeneous sequences.
	/// </summary>
	/// <typeparam name="TLeft">
	///	    The type of elements in the first sequence.
	/// </typeparam>
	/// <typeparam name="TRight">
	///	    The type of elements in the second sequence.
	/// </typeparam>
	/// <typeparam name="TKey">
	///	    The type of the key returned by the key selector functions.
	/// </typeparam>
	/// <typeparam name="TResult">
	///	    The type of the result elements.
	/// </typeparam>
	/// <param name="left">
	///	    The first sequence.
	/// </param>
	/// <param name="right">
	///	    The second sequence.
	/// </param>
	/// <param name="leftKeySelector">
	///	    A function to extract the join key from each element of the first sequence.
	/// </param>
	/// <param name="rightKeySelector">
	///	    A function to extract the join key from each element of the second sequence.
	/// </param>
	/// <param name="bothResultSelector">
	///	    A function to create a result element from two matching elements.
	/// </param>
	/// <param name="comparer">
	///	    An <see cref="IEqualityComparer{T}"/> to hash and compare keys.
	/// </param>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="left"/>, <paramref name="right"/>, <paramref name="leftKeySelector"/>, <paramref
	///     name="rightKeySelector"/>, or <paramref name="bothResultSelector"/> is <see langword="null" />.
	/// </exception>
	/// <returns>
	///	    A sequence containing results projected from an inner join of the two input sequences.
	/// </returns>
	/// <remarks>
	/// <para>
	///	    The result of this method is an `inner`-join. Values are projected using <paramref
	///     name="bothResultSelector"/> only if matching elements are found in both <paramref name="left"/> and
	///     <paramref name="right"/> sequences.
	/// </para>
	/// <para>
	///	    This method is implemented using a `loop`-join. The sequence <paramref name="right"/> is cached, and
	///	    for-each element of <paramref name="left"/>, the cached sequence is enumerated for matching elements.
	/// </para>
	/// <para>
	///	    This method uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IAsyncEnumerable<TResult> InnerLoopJoin<TLeft, TRight, TKey, TResult>(
		this IAsyncEnumerable<TLeft> left,
		IAsyncEnumerable<TRight> right,
		Func<TLeft, TKey> leftKeySelector,
		Func<TRight, TKey> rightKeySelector,
		Func<TLeft, TRight, TResult> bothResultSelector,
		IEqualityComparer<TKey>? comparer = null)
	{
		ArgumentNullException.ThrowIfNull(left);
		ArgumentNullException.ThrowIfNull(right);
		ArgumentNullException.ThrowIfNull(leftKeySelector);
		ArgumentNullException.ThrowIfNull(rightKeySelector);
		ArgumentNullException.ThrowIfNull(bothResultSelector);

		return LoopJoin(
			left, right,
			leftKeySelector, rightKeySelector,
			leftResultSelector: default,
			bothResultSelector,
			comparer);
	}

	/// <summary>
	///	    Performs a left outer join on two heterogeneous sequences.
	/// </summary>
	/// <typeparam name="TLeft">
	///	    The type of elements in the first sequence.
	/// </typeparam>
	/// <typeparam name="TRight">
	///	    The type of elements in the second sequence.
	/// </typeparam>
	/// <typeparam name="TKey">
	///	    The type of the key returned by the key selector functions.
	/// </typeparam>
	/// <param name="left">
	///	    The first sequence.
	/// </param>
	/// <param name="right">
	///	    The second sequence.
	/// </param>
	/// <param name="leftKeySelector">
	///	    A function to extract the join key from each element of the first sequence.
	/// </param>
	/// <param name="rightKeySelector">
	///	    A function to extract the join key from each element of the second sequence.
	/// </param>
	/// <param name="comparer">
	///	    An <see cref="IEqualityComparer{T}"/> to hash and compare keys.
	/// </param>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="left"/>, <paramref name="right"/>, <paramref name="leftKeySelector"/>, or <paramref
	///     name="rightKeySelector"/> is <see langword="null" />.
	/// </exception>
	/// <returns>
	///	    A sequence containing values from a left outer join of the two input sequences.
	/// </returns>
	/// <remarks>
	/// <para>
	///	    The result of this method is a `left outer`-join. All elements from <paramref name="left"/> are returned,
	///	    along with any matching elements from <paramref name="right"/>, if any are present. If no values in
	///	    <paramref name="right"/> match, <see langword="default" /> is returned for the right element.
	/// </para>
	/// <para>
	///	    This method is implemented using a `loop`-join. The sequence <paramref name="right"/> is cached, and
	///	    for-each element of <paramref name="left"/>, the cached sequence is enumerated for matching elements.
	/// </para>
	/// <para>
	///	    This method uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IAsyncEnumerable<(TLeft Left, TRight? Right)> LeftOuterLoopJoin<TLeft, TRight, TKey>(
		this IAsyncEnumerable<TLeft> left,
		IAsyncEnumerable<TRight> right,
		Func<TLeft, TKey> leftKeySelector,
		Func<TRight, TKey> rightKeySelector,
		IEqualityComparer<TKey>? comparer = null)
	{
		ArgumentNullException.ThrowIfNull(left);
		ArgumentNullException.ThrowIfNull(right);
		ArgumentNullException.ThrowIfNull(leftKeySelector);
		ArgumentNullException.ThrowIfNull(rightKeySelector);

		return LoopJoin(
			left, right,
			leftKeySelector, rightKeySelector,
			static left => (left, default),
			ValueTuple.Create,
			comparer);
	}

	/// <summary>
	///	    Performs a left outer join on two heterogeneous sequences.
	/// </summary>
	/// <typeparam name="TLeft">
	///	    The type of elements in the first sequence.
	/// </typeparam>
	/// <typeparam name="TRight">
	///	    The type of elements in the second sequence.
	/// </typeparam>
	/// <typeparam name="TKey">
	///	    The type of the key returned by the key selector functions.
	/// </typeparam>
	/// <typeparam name="TResult">
	///	    The type of the result elements.
	/// </typeparam>
	/// <param name="left">
	///	    The first sequence.
	/// </param>
	/// <param name="right">
	///	    The second sequence.
	/// </param>
	/// <param name="leftKeySelector">
	///	    A function to extract the join key from each element of the first sequence.
	/// </param>
	/// <param name="rightKeySelector">
	///	    A function to extract the join key from each element of the second sequence.
	/// </param>
	/// <param name="leftResultSelector">
	///	    A function to create a result element from a <paramref name="left"/> value that does not have a matching
	///     value in <paramref name="right"/>.
	/// </param>
	/// <param name="bothResultSelector">
	///	    A function to create a result element from two matching elements.
	/// </param>
	/// <param name="comparer">
	///	    An <see cref="IEqualityComparer{T}"/> to hash and compare keys.
	/// </param>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="left"/>, <paramref name="right"/>, <paramref name="leftKeySelector"/>, <paramref
	///     name="rightKeySelector"/>, <paramref name="leftResultSelector"/>, or <paramref name="bothResultSelector"/>
	///     is <see langword="null" />.
	/// </exception>
	/// <returns>
	///	    A sequence containing values projected from a left outer join of the two input sequences.
	/// </returns>
	/// <remarks>
	/// <para>
	///	    The result of this method is a `left outer`-join. Values are projected using <paramref
	///     name="bothResultSelector"/> when matching elements are found in both <paramref name="left"/> and <paramref
	///     name="right"/> sequences. Values are projected using <paramref name="leftKeySelector"/> when elements in
	///     <paramref name="left"/> do not have a matching element in <paramref name="right"/>.
	/// </para>
	/// <para>
	///	    This method is implemented using a `loop`-join. The sequence <paramref name="right"/> is cached, and
	///	    for-each element of <paramref name="left"/>, the cached sequence is enumerated for matching elements.
	/// </para>
	/// <para>
	///	    This method uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IAsyncEnumerable<TResult> LeftOuterLoopJoin<TLeft, TRight, TKey, TResult>(
		this IAsyncEnumerable<TLeft> left,
		IAsyncEnumerable<TRight> right,
		Func<TLeft, TKey> leftKeySelector,
		Func<TRight, TKey> rightKeySelector,
		Func<TLeft, TResult> leftResultSelector,
		Func<TLeft, TRight, TResult> bothResultSelector,
		IEqualityComparer<TKey>? comparer = null)
	{
		ArgumentNullException.ThrowIfNull(left);
		ArgumentNullException.ThrowIfNull(right);
		ArgumentNullException.ThrowIfNull(leftKeySelector);
		ArgumentNullException.ThrowIfNull(rightKeySelector);
		ArgumentNullException.ThrowIfNull(leftResultSelector);
		ArgumentNullException.ThrowIfNull(bothResultSelector);

		return LoopJoin(
			left, right,
			leftKeySelector, rightKeySelector,
			leftResultSelector,
			bothResultSelector,
			comparer);
	}

	private static async IAsyncEnumerable<TResult> LoopJoin<TLeft, TRight, TKey, TResult>(
		this IAsyncEnumerable<TLeft> left,
		IAsyncEnumerable<TRight> right,
		Func<TLeft, TKey> leftKeySelector,
		Func<TRight, TKey> rightKeySelector,
		Func<TLeft, TResult>? leftResultSelector,
		Func<TLeft, TRight, TResult> bothResultSelector,
		IEqualityComparer<TKey>? comparer,
		[EnumeratorCancellation] CancellationToken token = default)
	{
		comparer ??= EqualityComparer<TKey>.Default;

		var rList = await right.ToListAsync(token).ConfigureAwait(false);
		await foreach (var l in left.WithCancellation(token).ConfigureAwait(false))
		{
			var lKey = leftKeySelector(l);
			var flag = false;

			foreach (var r in rList)
			{
				var rKey = rightKeySelector(r);
				if (comparer.Equals(lKey, rKey))
				{
					flag = true;
					yield return bothResultSelector(l, r);
				}
			}

			if (leftResultSelector != null
				&& !flag)
			{
				yield return leftResultSelector(l);
			}
		}
	}
}
