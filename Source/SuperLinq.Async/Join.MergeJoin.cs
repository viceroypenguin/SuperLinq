using System.Diagnostics.CodeAnalysis;

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
	///	    This method is implemented using a `merge`-join. The sequences <paramref name="left"/> and <paramref
	///     name="right"/> are assumed to be already sorted. Results from using unsorted sequences with this method are
	///     undefined. Each sequence is enumerated exactly once in a parallel fashion, until both sequences are
	///     fully enumerated.
	/// </para>
	/// <para>
	///	    This method uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IAsyncEnumerable<(TLeft Left, TRight Right)> InnerMergeJoin<TLeft, TRight, TKey>(
		this IAsyncEnumerable<TLeft> left,
		IAsyncEnumerable<TRight> right,
		Func<TLeft, TKey> leftKeySelector,
		Func<TRight, TKey> rightKeySelector,
		IComparer<TKey>? comparer = null)
	{
		ArgumentNullException.ThrowIfNull(left);
		ArgumentNullException.ThrowIfNull(right);
		ArgumentNullException.ThrowIfNull(leftKeySelector);
		ArgumentNullException.ThrowIfNull(rightKeySelector);

		return JoinMerge(
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
	///	    This method is implemented using a `merge`-join. The sequences <paramref name="left"/> and <paramref
	///     name="right"/> are assumed to be already sorted. Results from using unsorted sequences with this method are
	///     undefined. Each sequence is enumerated exactly once in a parallel fashion, until both sequences are
	///     fully enumerated.
	/// </para>
	/// <para>
	///	    This method uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IAsyncEnumerable<TResult> InnerMergeJoin<TLeft, TRight, TKey, TResult>(
		this IAsyncEnumerable<TLeft> left,
		IAsyncEnumerable<TRight> right,
		Func<TLeft, TKey> leftKeySelector,
		Func<TRight, TKey> rightKeySelector,
		Func<TLeft, TRight, TResult> bothResultSelector,
		IComparer<TKey>? comparer = null)
	{
		ArgumentNullException.ThrowIfNull(left);
		ArgumentNullException.ThrowIfNull(right);
		ArgumentNullException.ThrowIfNull(leftKeySelector);
		ArgumentNullException.ThrowIfNull(rightKeySelector);
		ArgumentNullException.ThrowIfNull(bothResultSelector);

		return JoinMerge(
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
	///	    This method is implemented using a `merge`-join. The sequences <paramref name="left"/> and <paramref
	///     name="right"/> are assumed to be already sorted. Results from using unsorted sequences with this method are
	///     undefined. Each sequence is enumerated exactly once in a parallel fashion, until both sequences are
	///     fully enumerated.
	/// </para>
	/// <para>
	///	    This method uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IAsyncEnumerable<(TLeft Left, TRight? Right)> LeftOuterMergeJoin<TLeft, TRight, TKey>(
		this IAsyncEnumerable<TLeft> left,
		IAsyncEnumerable<TRight> right,
		Func<TLeft, TKey> leftKeySelector,
		Func<TRight, TKey> rightKeySelector,
		IComparer<TKey>? comparer = null)
	{
		ArgumentNullException.ThrowIfNull(left);
		ArgumentNullException.ThrowIfNull(right);
		ArgumentNullException.ThrowIfNull(leftKeySelector);
		ArgumentNullException.ThrowIfNull(rightKeySelector);

		return JoinMerge<TLeft, TRight, TKey, (TLeft, TRight?)>(
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
	///	    This method is implemented using a `merge`-join. The sequences <paramref name="left"/> and <paramref
	///     name="right"/> are assumed to be already sorted. Results from using unsorted sequences with this method are
	///     undefined. Each sequence is enumerated exactly once in a parallel fashion, until both sequences are
	///     fully enumerated.
	/// </para>
	/// <para>
	///	    This method uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IAsyncEnumerable<TResult> LeftOuterMergeJoin<TLeft, TRight, TKey, TResult>(
		this IAsyncEnumerable<TLeft> left,
		IAsyncEnumerable<TRight> right,
		Func<TLeft, TKey> leftKeySelector,
		Func<TRight, TKey> rightKeySelector,
		Func<TLeft, TResult> leftResultSelector,
		Func<TLeft, TRight, TResult> bothResultSelector,
		IComparer<TKey>? comparer = null)
	{
		ArgumentNullException.ThrowIfNull(left);
		ArgumentNullException.ThrowIfNull(right);
		ArgumentNullException.ThrowIfNull(leftKeySelector);
		ArgumentNullException.ThrowIfNull(rightKeySelector);
		ArgumentNullException.ThrowIfNull(leftResultSelector);
		ArgumentNullException.ThrowIfNull(bothResultSelector);

		return JoinMerge(
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
	///	    This method is implemented using a `merge`-join. The sequences <paramref name="left"/> and <paramref
	///     name="right"/> are assumed to be already sorted. Results from using unsorted sequences with this method are
	///     undefined. Each sequence is enumerated exactly once in a parallel fashion, until both sequences are
	///     fully enumerated.
	/// </para>
	/// <para>
	///	    This method uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IAsyncEnumerable<(TLeft? Left, TRight Right)> RightOuterMergeJoin<TLeft, TRight, TKey>(
		this IAsyncEnumerable<TLeft> left,
		IAsyncEnumerable<TRight> right,
		Func<TLeft, TKey> leftKeySelector,
		Func<TRight, TKey> rightKeySelector,
		IComparer<TKey>? comparer = null)
	{
		ArgumentNullException.ThrowIfNull(left);
		ArgumentNullException.ThrowIfNull(right);
		ArgumentNullException.ThrowIfNull(leftKeySelector);
		ArgumentNullException.ThrowIfNull(rightKeySelector);

		return JoinMerge<TLeft, TRight, TKey, (TLeft?, TRight)>(
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
	///	    This method is implemented using a `merge`-join. The sequences <paramref name="left"/> and <paramref
	///     name="right"/> are assumed to be already sorted. Results from using unsorted sequences with this method are
	///     undefined. Each sequence is enumerated exactly once in a parallel fashion, until both sequences are
	///     fully enumerated.
	/// </para>
	/// <para>
	///	    This method uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IAsyncEnumerable<TResult> RightOuterMergeJoin<TLeft, TRight, TKey, TResult>(
		this IAsyncEnumerable<TLeft> left,
		IAsyncEnumerable<TRight> right,
		Func<TLeft, TKey> leftKeySelector,
		Func<TRight, TKey> rightKeySelector,
		Func<TRight, TResult> rightResultSelector,
		Func<TLeft, TRight, TResult> bothResultSelector,
		IComparer<TKey>? comparer = null)
	{
		ArgumentNullException.ThrowIfNull(left);
		ArgumentNullException.ThrowIfNull(right);
		ArgumentNullException.ThrowIfNull(leftKeySelector);
		ArgumentNullException.ThrowIfNull(rightKeySelector);
		ArgumentNullException.ThrowIfNull(rightResultSelector);
		ArgumentNullException.ThrowIfNull(bothResultSelector);

		return JoinMerge(
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
	///	    This method is implemented using a `merge`-join. The sequences <paramref name="left"/> and <paramref
	///     name="right"/> are assumed to be already sorted. Results from using unsorted sequences with this method are
	///     undefined. Each sequence is enumerated exactly once in a parallel fashion, until both sequences are
	///     fully enumerated.
	/// </para>
	/// <para>
	///	    This method uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IAsyncEnumerable<(TLeft? Left, TRight? Right)> FullOuterMergeJoin<TLeft, TRight, TKey>(
		this IAsyncEnumerable<TLeft> left,
		IAsyncEnumerable<TRight> right,
		Func<TLeft, TKey> leftKeySelector,
		Func<TRight, TKey> rightKeySelector,
		IComparer<TKey>? comparer = null)
	{
		ArgumentNullException.ThrowIfNull(left);
		ArgumentNullException.ThrowIfNull(right);
		ArgumentNullException.ThrowIfNull(leftKeySelector);
		ArgumentNullException.ThrowIfNull(rightKeySelector);

		return JoinMerge<TLeft, TRight, TKey, (TLeft?, TRight?)>(
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
	///	    This method is implemented using a `merge`-join. The sequences <paramref name="left"/> and <paramref
	///     name="right"/> are assumed to be already sorted. Results from using unsorted sequences with this method are
	///     undefined. Each sequence is enumerated exactly once in a parallel fashion, until both sequences are
	///     fully enumerated.
	/// </para>
	/// <para>
	///	    This method uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IAsyncEnumerable<TResult> FullOuterMergeJoin<TLeft, TRight, TKey, TResult>(
		this IAsyncEnumerable<TLeft> left,
		IAsyncEnumerable<TRight> right,
		Func<TLeft, TKey> leftKeySelector,
		Func<TRight, TKey> rightKeySelector,
		Func<TLeft, TResult> leftResultSelector,
		Func<TRight, TResult> rightResultSelector,
		Func<TLeft, TRight, TResult> bothResultSelector,
		IComparer<TKey>? comparer = null)
	{
		ArgumentNullException.ThrowIfNull(left);
		ArgumentNullException.ThrowIfNull(right);
		ArgumentNullException.ThrowIfNull(leftKeySelector);
		ArgumentNullException.ThrowIfNull(rightKeySelector);
		ArgumentNullException.ThrowIfNull(leftResultSelector);
		ArgumentNullException.ThrowIfNull(rightResultSelector);
		ArgumentNullException.ThrowIfNull(bothResultSelector);

		return JoinMerge(
			left, right,
			leftKeySelector, rightKeySelector,
			leftResultSelector,
			rightResultSelector,
			bothResultSelector,
			comparer);
	}

	private static async IAsyncEnumerable<TResult> JoinMerge<TLeft, TRight, TKey, TResult>(
		IAsyncEnumerable<TLeft> left,
		IAsyncEnumerable<TRight> right,
		Func<TLeft, TKey> leftKeySelector,
		Func<TRight, TKey> rightKeySelector,
		Func<TLeft, TResult>? leftResultSelector,
		Func<TRight, TResult>? rightResultSelector,
		Func<TLeft, TRight, TResult> bothResultSelector,
		IComparer<TKey>? comparer = null,
		[EnumeratorCancellation] CancellationToken token = default)
	{
		comparer ??= Comparer<TKey>.Default;

		await using var e1 = left.GroupAdjacent(leftKeySelector, new ComparerEqualityComparer<TKey>(comparer))
			.GetConfiguredAsyncEnumerator(token);
		await using var e2 = right.GroupAdjacent(rightKeySelector, new ComparerEqualityComparer<TKey>(comparer))
			.GetConfiguredAsyncEnumerator(token);

		var gotLeft = await e1.MoveNextAsync();
		var gotRight = await e2.MoveNextAsync();

		while (gotLeft && gotRight)
		{
			var l = e1.Current;
			var r = e2.Current;
			var comparison = comparer.Compare(l.Key, r.Key);

			if (comparison < 0)
			{
				if (leftResultSelector is not null)
				{
					foreach (var e in l)
						yield return leftResultSelector(e);
				}

				gotLeft = await e1.MoveNextAsync();
			}
			else if (comparison > 0)
			{
				if (rightResultSelector is not null)
				{
					foreach (var e in r)
						yield return rightResultSelector(e);
				}

				gotRight = await e2.MoveNextAsync();
			}
			else
			{
				foreach (var el in l)
				{
					foreach (var er in r)
						yield return bothResultSelector(el, er);
				}

				gotLeft = await e1.MoveNextAsync();
				gotRight = await e2.MoveNextAsync();
			}
		}

		if (gotLeft && leftResultSelector is not null)
		{
			do
			{
				foreach (var e in e1.Current)
					yield return leftResultSelector(e);
			} while (await e1.MoveNextAsync());
			yield break;
		}

		if (gotRight && rightResultSelector is not null)
		{
			do
			{
				foreach (var e in e2.Current)
					yield return rightResultSelector(e);
			} while (await e2.MoveNextAsync());
			yield break;
		}
	}
}

file sealed class ComparerEqualityComparer<TKey>(
	IComparer<TKey> comparer
) : IEqualityComparer<TKey>
{
	public bool Equals([AllowNull] TKey x, [AllowNull] TKey y) => comparer.Compare(x, y) == 0;
	public int GetHashCode([DisallowNull] TKey obj) => ThrowHelper.ThrowNotSupportedException<int>();
}
