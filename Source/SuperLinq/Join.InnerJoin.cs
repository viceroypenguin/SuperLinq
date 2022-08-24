namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Performs an inner join on two heterogeneous sequences,
	/// returning a <see cref="ValueTuple{T1, T2}"/> containing 
	/// the elements from <paramref name="left"/> and 
	/// <paramref name="right"/>.
	/// Additional arguments specify key selection functions.
	/// </summary>
	/// <typeparam name="TLeft">The type of elements in the first sequence.</typeparam>
	/// <typeparam name="TRight">The type of elements in the second sequence.</typeparam>
	/// <typeparam name="TKey">The type of the key returned by the key selector functions.</typeparam>
	/// <param name="left">The first sequence.</param>
	/// <param name="right">The second sequence.</param>
	/// <param name="joinType">A value indicating which type of join to use.</param>
	/// <param name="leftKeySelector">Function that projects the key given an element from <paramref name="left"/>.</param>
	/// <param name="rightKeySelector">Function that projects the key given an element from <paramref name="right"/>.</param>
	/// <returns>
	/// A sequence containing results projected from an inner join of the two input sequences.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="leftKeySelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="rightKeySelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentException"><paramref name="joinType"/> is <see cref="JoinType.Loop"/>.</exception>
	/// <remarks>
	/// <para>
	/// This method uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IEnumerable<(TLeft, TRight)> InnerJoin<TLeft, TRight, TKey>(
		this IEnumerable<TLeft> left,
		IEnumerable<TRight> right,
		JoinType joinType,
		Func<TLeft, TKey> leftKeySelector,
		Func<TRight, TKey> rightKeySelector)
	{
		return Join(
			left,
			right,
			joinType,
			JoinOperation.Inner,
			leftKeySelector,
			rightKeySelector,
			leftResultSelector: default,
			rightResultSelector: default,
			ValueTuple.Create);
	}

	/// <summary>
	/// Performs an inner join on two heterogeneous sequences,
	/// returning a <see cref="ValueTuple{T1, T2}"/> containing 
	/// the elements from <paramref name="left"/> and <paramref name="right"/>.
	/// Additional arguments specify key selection functions and a key comparer.
	/// </summary>
	/// <typeparam name="TLeft">The type of elements in the first sequence.</typeparam>
	/// <typeparam name="TRight">The type of elements in the second sequence.</typeparam>
	/// <typeparam name="TKey">The type of the key returned by the key selector functions.</typeparam>
	/// <typeparam name="TComparer">The type of the comparer used to compare keys.</typeparam>
	/// <param name="left">The first sequence.</param>
	/// <param name="right">The second sequence.</param>
	/// <param name="joinType">A value indicating which type of join to use.</param>
	/// <param name="leftKeySelector">Function that projects the key given an element from <paramref name="left"/>.</param>
	/// <param name="rightKeySelector">Function that projects the key given an element from <paramref name="right"/>.</param>
	/// <param name="comparer">
	/// The comparer implementing types <see cref="IEqualityComparer{T}"/> and 
	/// <see cref="IComparer{T}"/> used to compare keys.
	/// </param>
	/// <returns>
	/// A sequence containing results projected from an inner join of the two input sequences.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="leftKeySelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="rightKeySelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentException"><paramref name="joinType"/> is <see cref="JoinType.Loop"/>.</exception>
	/// <remarks>
	/// <para>
	/// This method uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IEnumerable<(TLeft, TRight)> InnerJoin<TLeft, TRight, TKey, TComparer>(
		this IEnumerable<TLeft> left,
		IEnumerable<TRight> right,
		JoinType joinType,
		Func<TLeft, TKey> leftKeySelector,
		Func<TRight, TKey> rightKeySelector,
		TComparer comparer)
		where TComparer : notnull, IComparer<TKey>, IEqualityComparer<TKey>
	{
		return Join(
			left,
			right,
			joinType,
			JoinOperation.Inner,
			leftKeySelector,
			rightKeySelector,
			leftResultSelector: default,
			rightResultSelector: default,
			ValueTuple.Create,
			comparer);
	}

	/// <summary>
	/// Performs an inner join on two heterogeneous sequences.
	/// Additional arguments specify key selection and result
	/// projection functions.
	/// </summary>
	/// <typeparam name="TLeft">The type of elements in the first sequence.</typeparam>
	/// <typeparam name="TRight">The type of elements in the second sequence.</typeparam>
	/// <typeparam name="TKey">The type of the key returned by the key selector functions.</typeparam>
	/// <typeparam name="TResult">The type of the result elements.</typeparam>
	/// <param name="left">The first sequence.</param>
	/// <param name="right">The second sequence.</param>
	/// <param name="joinType">A value indicating which type of join to use.</param>
	/// <param name="leftKeySelector">Function that projects the key given an element from <paramref name="left"/>.</param>
	/// <param name="rightKeySelector">Function that projects the key given an element from <paramref name="right"/>.</param>
	/// <param name="bothResultSelector">
	/// Function that projects the result given an element from
	/// <paramref name="left"/> and an element from <paramref name="right"/>
	/// that match on a common key.
	/// </param>
	/// <returns>
	/// A sequence containing results projected from an inner join of the two input sequences.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="leftKeySelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="rightKeySelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="bothResultSelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentException"><paramref name="joinType"/> is <see cref="JoinType.Loop"/>.</exception>
	/// <remarks>
	/// <para>
	/// This method uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IEnumerable<TResult> InnerJoin<TLeft, TRight, TKey, TResult>(
		this IEnumerable<TLeft> left,
		IEnumerable<TRight> right,
		JoinType joinType,
		Func<TLeft, TKey> leftKeySelector,
		Func<TRight, TKey> rightKeySelector,
		Func<TLeft, TRight, TResult> bothResultSelector)
	{
		return Join(
			left,
			right,
			joinType,
			JoinOperation.Inner,
			leftKeySelector,
			rightKeySelector,
			leftResultSelector: default,
			rightResultSelector: default,
			bothResultSelector);
	}

	/// <summary>
	/// Performs an inner join on two heterogeneous sequences.
	/// Additional arguments specify key selection functions, result
	/// projection functions and a key comparer.
	/// </summary>
	/// <typeparam name="TLeft">The type of elements in the first sequence.</typeparam>
	/// <typeparam name="TRight">The type of elements in the second sequence.</typeparam>
	/// <typeparam name="TKey">The type of the key returned by the key selector functions.</typeparam>
	/// <typeparam name="TComparer">The type of the comparer used to compare keys.</typeparam>
	/// <typeparam name="TResult">The type of the result elements.</typeparam>
	/// <param name="left">The first sequence.</param>
	/// <param name="right">The second sequence.</param>
	/// <param name="joinType">A value indicating which type of join to use.</param>
	/// <param name="leftKeySelector">Function that projects the key given an element from <paramref name="left"/>.</param>
	/// <param name="rightKeySelector">Function that projects the key given an element from <paramref name="right"/>.</param>
	/// <param name="bothResultSelector">
	/// Function that projects the result given an element from
	/// <paramref name="left"/> and an element from <paramref name="right"/>
	/// that match on a common key.
	/// </param>
	/// <param name="comparer">
	/// The comparer implementing types <see cref="IEqualityComparer{T}"/> and 
	/// <see cref="IComparer{T}"/> used to compare keys.
	/// </param>
	/// <returns>
	/// A sequence containing results projected from an inner join of the two input sequences.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="leftKeySelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="rightKeySelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="bothResultSelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentException"><paramref name="joinType"/> is <see cref="JoinType.Loop"/>.</exception>
	/// <remarks>
	/// <para>
	/// This method uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IEnumerable<TResult> InnerJoin<TLeft, TRight, TKey, TComparer, TResult>(
		this IEnumerable<TLeft> left,
		IEnumerable<TRight> right,
		JoinType joinType,
		Func<TLeft, TKey> leftKeySelector,
		Func<TRight, TKey> rightKeySelector,
		Func<TLeft, TRight, TResult> bothResultSelector,
		TComparer comparer)
		where TComparer : notnull, IComparer<TKey>, IEqualityComparer<TKey>
	{
		return Join(
			left,
			right,
			joinType,
			JoinOperation.Inner,
			leftKeySelector,
			rightKeySelector,
			leftResultSelector: default,
			rightResultSelector: default,
			bothResultSelector,
			comparer);
	}
}
