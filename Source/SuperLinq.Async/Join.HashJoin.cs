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
	///	    This method is implemented using a `hash`-join. The sequence <paramref name="right"/> is cached as an <see
	///	    cref="ILookup{TKey, TElement}"/>, and for-each element of <paramref name="left"/>, the lookup is accessed to
	///	    find any matching elements.
	/// </para>
	/// <para>
	///	    This method uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IAsyncEnumerable<(TLeft Left, TRight Right)> InnerHashJoin<TLeft, TRight, TKey>(
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

		return JoinHash(
			left, right,
			leftKeySelector, rightKeySelector,
			leftResultSelector: default,
			rightResultSelector: default,
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
	///	    This method is implemented using a `hash`-join. The sequence <paramref name="right"/> is cached as an <see
	///	    cref="ILookup{TKey, TElement}"/>, and for-each element of <paramref name="left"/>, the lookup is accessed to
	///	    find any matching elements.
	/// </para>
	/// <para>
	///	    This method uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IAsyncEnumerable<TResult> InnerHashJoin<TLeft, TRight, TKey, TResult>(
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

		return JoinHash(
			left, right,
			leftKeySelector, rightKeySelector,
			leftResultSelector: default,
			rightResultSelector: default,
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
	///	    This method is implemented using a `hash`-join. The sequence <paramref name="right"/> is cached as an <see
	///	    cref="ILookup{TKey, TElement}"/>, and for-each element of <paramref name="left"/>, the lookup is accessed to
	///	    find any matching elements.
	/// </para>
	/// <para>
	///	    This method uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IAsyncEnumerable<(TLeft Left, TRight? Right)> LeftOuterHashJoin<TLeft, TRight, TKey>(
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

		return JoinHash(
			left, right,
			leftKeySelector, rightKeySelector,
			static left => (left, default),
			rightResultSelector: default,
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
	///	    This method is implemented using a `hash`-join. The sequence <paramref name="right"/> is cached as an <see
	///	    cref="ILookup{TKey, TElement}"/>, and for-each element of <paramref name="left"/>, the lookup is accessed to
	///	    find any matching elements.
	/// </para>
	/// <para>
	///	    This method uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IAsyncEnumerable<TResult> LeftOuterHashJoin<TLeft, TRight, TKey, TResult>(
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

		return JoinHash(
			left, right,
			leftKeySelector, rightKeySelector,
			leftResultSelector,
			rightResultSelector: default,
			bothResultSelector,
			comparer);
	}

	/// <summary>
	///	    Performs a right outer join on two heterogeneous sequences.
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
	///	    A sequence containing values from a right outer join of the two input sequences.
	/// </returns>
	/// <remarks>
	/// <para>
	///	    The result of this method is a `right outer`-join. All elements from <paramref name="right"/> are returned,
	///	    along with any matching elements from <paramref name="left"/>, if any are present. If no values in <paramref
	///	    name="left"/> match, <see langword="default" /> is returned for the left element.
	/// </para>
	/// <para>
	///	    This method is implemented using a `hash`-join. The sequence <paramref name="right"/> is cached as an <see
	///	    cref="ILookup{TKey, TElement}"/>, and for-each element of <paramref name="left"/>, the lookup is accessed to
	///	    find any matching elements.
	/// </para>
	/// <para>
	///	    This method uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IAsyncEnumerable<(TLeft? Left, TRight Right)> RightOuterHashJoin<TLeft, TRight, TKey>(
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

		return JoinHash(
			left, right,
			leftKeySelector, rightKeySelector,
			leftResultSelector: default,
			static right => (default, right),
			ValueTuple.Create,
			comparer);
	}

	/// <summary>
	///	    Performs a right outer join on two heterogeneous sequences.
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
	/// <param name="rightResultSelector">
	///	    A function to create a result element from a <paramref name="right"/> value that does not have a matching
	///     value in <paramref name="left"/>.
	/// </param>
	/// <param name="bothResultSelector">
	///	    A function to create a result element from two matching elements.
	/// </param>
	/// <param name="comparer">
	///	    An <see cref="IEqualityComparer{T}"/> to hash and compare keys.
	/// </param>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="left"/>, <paramref name="right"/>, <paramref name="leftKeySelector"/>, <paramref
	///     name="rightKeySelector"/>, <paramref name="rightResultSelector"/>, or <paramref name="bothResultSelector"/>
	///     is <see langword="null" />.
	/// </exception>
	/// <returns>
	///	    A sequence containing values projected from a right outer join of the two input sequences.
	/// </returns>
	/// <remarks>
	/// <para>
	///	    The result of this method is a `left outer`-join. Values are projected using <paramref
	///     name="bothResultSelector"/> when matching elements are found in both <paramref name="left"/> and <paramref
	///     name="right"/> sequences. Values are projected using <paramref name="rightResultSelector"/> when elements in
	///     <paramref name="right"/> do not have a matching element in <paramref name="left"/>.
	/// </para>
	/// <para>
	///	    This method is implemented using a `hash`-join. The sequence <paramref name="right"/> is cached as an <see
	///	    cref="ILookup{TKey, TElement}"/>, and for-each element of <paramref name="left"/>, the lookup is accessed to
	///	    find any matching elements.
	/// </para>
	/// <para>
	///	    This method uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IAsyncEnumerable<TResult> RightOuterHashJoin<TLeft, TRight, TKey, TResult>(
		this IAsyncEnumerable<TLeft> left,
		IAsyncEnumerable<TRight> right,
		Func<TLeft, TKey> leftKeySelector,
		Func<TRight, TKey> rightKeySelector,
		Func<TRight, TResult> rightResultSelector,
		Func<TLeft, TRight, TResult> bothResultSelector,
		IEqualityComparer<TKey>? comparer = null)
	{
		ArgumentNullException.ThrowIfNull(left);
		ArgumentNullException.ThrowIfNull(right);
		ArgumentNullException.ThrowIfNull(leftKeySelector);
		ArgumentNullException.ThrowIfNull(rightKeySelector);
		ArgumentNullException.ThrowIfNull(rightResultSelector);
		ArgumentNullException.ThrowIfNull(bothResultSelector);

		return JoinHash(
			left, right,
			leftKeySelector, rightKeySelector,
			leftResultSelector: default,
			rightResultSelector,
			bothResultSelector,
			comparer);
	}

	/// <summary>
	///	    Performs a full outer join on two heterogeneous sequences.
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
	///	    A sequence containing values from a full outer join of the two input sequences.
	/// </returns>
	/// <remarks>
	/// <para>
	///	    The result of this method is a `full outer`-join. All elements from both <paramref name="left"/> and
	///	    <paramref name="right"/> are returned; matching elements are returned together, and elements that do not
	///	    match are returned with a <see langword="default"/> value for the other.
	/// </para>
	/// <para>
	///	    This method is implemented using a `hash`-join. The sequence <paramref name="right"/> is cached as an <see
	///	    cref="ILookup{TKey, TElement}"/>, and for-each element of <paramref name="left"/>, the lookup is accessed to
	///	    find any matching elements.
	/// </para>
	/// <para>
	///	    This method uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IAsyncEnumerable<(TLeft? Left, TRight? Right)> FullOuterHashJoin<TLeft, TRight, TKey>(
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

		return JoinHash(
			left, right,
			leftKeySelector, rightKeySelector,
			static left => (left, default),
			static right => (default, right),
			ValueTuple.Create,
			comparer);
	}

	/// <summary>
	///	    Performs a full outer join on two heterogeneous sequences.
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
	/// <param name="rightResultSelector">
	///	    A function to create a result element from a <paramref name="right"/> value that does not have a matching
	///     value in <paramref name="left"/>.
	/// </param>
	/// <param name="bothResultSelector">
	///	    A function to create a result element from two matching elements.
	/// </param>
	/// <param name="comparer">
	///	    An <see cref="IEqualityComparer{T}"/> to hash and compare keys.
	/// </param>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="left"/>, <paramref name="right"/>, <paramref name="leftKeySelector"/>, <paramref
	///     name="rightKeySelector"/>, <paramref name="leftResultSelector"/>, <paramref name="rightResultSelector"/>, or
	///     <paramref name="bothResultSelector"/> is <see langword="null" />.
	/// </exception>
	/// <returns>
	///	    A sequence containing values projected from a right outer join of the two input sequences.
	/// </returns>
	/// <remarks>
	/// <para>
	///	    The result of this method is a `full outer`-join. Values are projected using <paramref
	///     name="bothResultSelector"/> when matching elements are found in both <paramref name="left"/> and <paramref
	///     name="right"/> sequences. Values are projected using <paramref name="leftKeySelector"/> when elements in
	///     <paramref name="left"/> do not have a matching element in <paramref name="right"/>. Values are projected
	///     using <paramref name="rightResultSelector"/> when elements in <paramref name="right"/> do not have a
	///     matching element in <paramref name="left"/>.
	/// </para>
	/// <para>
	///	    This method is implemented using a `hash`-join. The sequence <paramref name="right"/> is cached as an <see
	///	    cref="ILookup{TKey, TElement}"/>, and for-each element of <paramref name="left"/>, the lookup is accessed to
	///	    find any matching elements.
	/// </para>
	/// <para>
	///	    This method uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IAsyncEnumerable<TResult> FullOuterHashJoin<TLeft, TRight, TKey, TResult>(
		this IAsyncEnumerable<TLeft> left,
		IAsyncEnumerable<TRight> right,
		Func<TLeft, TKey> leftKeySelector,
		Func<TRight, TKey> rightKeySelector,
		Func<TLeft, TResult> leftResultSelector,
		Func<TRight, TResult> rightResultSelector,
		Func<TLeft, TRight, TResult> bothResultSelector,
		IEqualityComparer<TKey>? comparer = null)
	{
		ArgumentNullException.ThrowIfNull(left);
		ArgumentNullException.ThrowIfNull(right);
		ArgumentNullException.ThrowIfNull(leftKeySelector);
		ArgumentNullException.ThrowIfNull(rightKeySelector);
		ArgumentNullException.ThrowIfNull(leftResultSelector);
		ArgumentNullException.ThrowIfNull(rightResultSelector);
		ArgumentNullException.ThrowIfNull(bothResultSelector);

		return JoinHash(
			left, right,
			leftKeySelector, rightKeySelector,
			leftResultSelector,
			rightResultSelector,
			bothResultSelector,
			comparer);
	}

	private static async IAsyncEnumerable<TResult> JoinHash<TLeft, TRight, TKey, TResult>(
		IAsyncEnumerable<TLeft> left,
		IAsyncEnumerable<TRight> right,
		Func<TLeft, TKey> leftKeySelector,
		Func<TRight, TKey> rightKeySelector,
		Func<TLeft, TResult>? leftResultSelector,
		Func<TRight, TResult>? rightResultSelector,
		Func<TLeft, TRight, TResult> bothResultSelector,
		IEqualityComparer<TKey>? comparer,
		[EnumeratorCancellation] CancellationToken token = default)
	{
		comparer ??= EqualityComparer<TKey>.Default;

		var rLookup = await right.ToLookupAsync(rightKeySelector, comparer, token).ConfigureAwait(false);
		var used = new HashSet<TKey>(comparer);
		await foreach (var l in left.WithCancellation(token).ConfigureAwait(false))
		{
			var lKey = leftKeySelector(l);

			if (!rLookup.Contains(lKey))
			{
				if (leftResultSelector != null)
					yield return leftResultSelector(l);
				continue;
			}

			_ = used.Add(lKey);
			foreach (var r in rLookup[lKey])
				yield return bothResultSelector(l, r);
		}

		if (rightResultSelector != null)
		{
			foreach (var g in rLookup)
			{
				if (used.Contains(g.Key))
					continue;

				foreach (var r in g)
					yield return rightResultSelector(r);
			}
		}
	}
}
