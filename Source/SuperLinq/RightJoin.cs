namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Performs a right outer join on two homogeneous sequences.
	/// Additional arguments specify key selection functions and result
	/// projection functions.
	/// </summary>
	/// <typeparam name="TSource">The type of elements in the source sequence.</typeparam>
	/// <typeparam name="TKey">The type of the key returned by the key selector function.</typeparam>
	/// <typeparam name="TResult">The type of the result elements.</typeparam>
	/// <param name="first">The first sequence.</param>
	/// <param name="second">The second sequence.</param>
	/// <param name="keySelector">Function that projects the key given an element of one of the sequences to join.</param>
	/// <param name="secondSelector">
	/// Function that projects the result given just an element from
	/// <paramref name="second"/> where there is no corresponding element
	/// in <paramref name="first"/>.
	/// </param>
	/// <param name="bothSelector">
	/// Function that projects the result given an element from
	/// <paramref name="first"/> and an element from <paramref name="second"/>
	/// that match on a common key.
	/// </param>
	/// <returns>
	/// A sequence containing results projected from a full
	/// outer join of the two input sequences.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="first"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="keySelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="secondSelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="bothSelector"/> is <see langword="null"/>.</exception>
	/// <remarks>
	/// <para>
	/// This method uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	[Obsolete("RightJoin has been replaced by RightOuterJoin")]
	public static IEnumerable<TResult> RightJoin<TSource, TKey, TResult>(
		this IEnumerable<TSource> first,
		IEnumerable<TSource> second,
		Func<TSource, TKey> keySelector,
		Func<TSource, TResult> secondSelector,
		Func<TSource, TSource, TResult> bothSelector)
	{
		ArgumentNullException.ThrowIfNull(keySelector);

		return RightJoin(
			first, second,
			keySelector, keySelector,
			secondSelector, bothSelector);
	}

	/// <summary>
	/// Performs a right outer join on two homogeneous sequences.
	/// Additional arguments specify key selection functions, result
	/// projection functions and a key comparer.
	/// </summary>
	/// <typeparam name="TSource">The type of elements in the source sequence.</typeparam>
	/// <typeparam name="TKey">The type of the key returned by the key selector function.</typeparam>
	/// <typeparam name="TResult">The type of the result elements.</typeparam>
	/// <param name="first">The first sequence.</param>
	/// <param name="second">The second sequence.</param>
	/// <param name="keySelector">Function that projects the key given an element of one of the sequences to join.</param>
	/// <param name="secondSelector">
	/// Function that projects the result given just an element from
	/// <paramref name="second"/> where there is no corresponding element
	/// in <paramref name="first"/>.
	/// </param>
	/// <param name="bothSelector">
	/// Function that projects the result given an element from
	/// <paramref name="first"/> and an element from <paramref name="second"/>
	/// that match on a common key.
	/// </param>
	/// <param name="comparer">
	/// The <see cref="IEqualityComparer{T}"/> instance used to compare
	/// keys for equality.
	/// </param>
	/// <returns>
	/// A sequence containing results projected from a full
	/// outer join of the two input sequences.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="first"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="keySelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="secondSelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="bothSelector"/> is <see langword="null"/>.</exception>
	/// <remarks>
	/// <para>
	/// This method uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	[Obsolete("RightJoin has been replaced by RightOuterJoin")]
	public static IEnumerable<TResult> RightJoin<TSource, TKey, TResult>(
		this IEnumerable<TSource> first,
		IEnumerable<TSource> second,
		Func<TSource, TKey> keySelector,
		Func<TSource, TResult> secondSelector,
		Func<TSource, TSource, TResult> bothSelector,
		IEqualityComparer<TKey>? comparer)
	{
		ArgumentNullException.ThrowIfNull(keySelector);

		return RightJoin(
			first, second,
			keySelector, keySelector,
			secondSelector, bothSelector,
			comparer);
	}

	/// <summary>
	/// Performs a right outer join on two heterogeneous sequences.
	/// Additional arguments specify key selection functions and result
	/// projection functions.
	/// </summary>
	/// <typeparam name="TFirst">The type of elements in the first sequence.</typeparam>
	/// <typeparam name="TSecond">The type of elements in the second sequence.</typeparam>
	/// <typeparam name="TKey">The type of the key returned by the key selector functions.</typeparam>
	/// <typeparam name="TResult">The type of the result elements.</typeparam>
	/// <param name="first">The first sequence.</param>
	/// <param name="second">The second sequence.</param>
	/// <param name="firstKeySelector">Function that projects the key given an element from <paramref name="first"/>.</param>
	/// <param name="secondKeySelector">Function that projects the key given an element from <paramref name="second"/>.</param>
	/// <param name="secondSelector">
	/// Function that projects the result given just an element from
	/// <paramref name="second"/> where there is no corresponding element
	/// in <paramref name="first"/>.
	/// </param>
	/// <param name="bothSelector">
	/// Function that projects the result given an element from
	/// <paramref name="first"/> and an element from <paramref name="second"/>
	/// that match on a common key.
	/// </param>
	/// <returns>
	/// A sequence containing results projected from a full
	/// outer join of the two input sequences.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="first"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="firstKeySelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="secondKeySelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="secondSelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="bothSelector"/> is <see langword="null"/>.</exception>
	/// <remarks>
	/// <para>
	/// This method uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	[Obsolete("RightJoin has been replaced by RightOuterJoin")]
	public static IEnumerable<TResult> RightJoin<TFirst, TSecond, TKey, TResult>(
		this IEnumerable<TFirst> first,
		IEnumerable<TSecond> second,
		Func<TFirst, TKey> firstKeySelector,
		Func<TSecond, TKey> secondKeySelector,
		Func<TSecond, TResult> secondSelector,
		Func<TFirst, TSecond, TResult> bothSelector)
	{
		ArgumentNullException.ThrowIfNull(first);
		ArgumentNullException.ThrowIfNull(second);
		ArgumentNullException.ThrowIfNull(firstKeySelector);
		ArgumentNullException.ThrowIfNull(secondKeySelector);
		ArgumentNullException.ThrowIfNull(secondSelector);
		ArgumentNullException.ThrowIfNull(bothSelector);

		return HashJoin(
			second, first,
			JoinOperation.LeftOuter,
			secondKeySelector, firstKeySelector,
			leftResultSelector: secondSelector, rightResultSelector: default,
			(r, l) => bothSelector(l, r),
			EqualityComparer<TKey>.Default);
	}

	/// <summary>
	/// Performs a right outer join on two heterogeneous sequences.
	/// Additional arguments specify key selection functions, result
	/// projection functions and a key comparer.
	/// </summary>
	/// <typeparam name="TFirst">The type of elements in the first sequence.</typeparam>
	/// <typeparam name="TSecond">The type of elements in the second sequence.</typeparam>
	/// <typeparam name="TKey">The type of the key returned by the key selector functions.</typeparam>
	/// <typeparam name="TResult">The type of the result elements.</typeparam>
	/// <param name="first">The first sequence.</param>
	/// <param name="second">The second sequence.</param>
	/// <param name="firstKeySelector">Function that projects the key given an element from <paramref name="first"/>.</param>
	/// <param name="secondKeySelector">Function that projects the key given an element from <paramref name="second"/>.</param>
	/// <param name="secondSelector">
	/// Function that projects the result given just an element from
	/// <paramref name="second"/> where there is no corresponding element
	/// in <paramref name="first"/>.
	/// </param>
	/// <param name="bothSelector">
	/// Function that projects the result given an element from
	/// <paramref name="first"/> and an element from <paramref name="second"/>
	/// that match on a common key.
	/// </param>
	/// <param name="comparer">
	/// The <see cref="IEqualityComparer{T}"/> instance used to compare
	/// keys for equality.
	/// </param>
	/// <returns>
	/// A sequence containing results projected from a full
	/// outer join of the two input sequences.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="first"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="firstKeySelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="secondKeySelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="secondSelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="bothSelector"/> is <see langword="null"/>.</exception>
	/// <remarks>
	/// <para>
	/// This method uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	[Obsolete("RightJoin has been replaced by RightOuterJoin")]
	public static IEnumerable<TResult> RightJoin<TFirst, TSecond, TKey, TResult>(
		this IEnumerable<TFirst> first,
		IEnumerable<TSecond> second,
		Func<TFirst, TKey> firstKeySelector,
		Func<TSecond, TKey> secondKeySelector,
		Func<TSecond, TResult> secondSelector,
		Func<TFirst, TSecond, TResult> bothSelector,
		IEqualityComparer<TKey>? comparer)
	{
		ArgumentNullException.ThrowIfNull(first);
		ArgumentNullException.ThrowIfNull(second);
		ArgumentNullException.ThrowIfNull(firstKeySelector);
		ArgumentNullException.ThrowIfNull(secondKeySelector);
		ArgumentNullException.ThrowIfNull(secondSelector);
		ArgumentNullException.ThrowIfNull(bothSelector);

		return HashJoin(
				second, first,
				JoinOperation.LeftOuter,
				secondKeySelector, firstKeySelector,
				leftResultSelector: secondSelector, rightResultSelector: default,
				(r, l) => bothSelector(l, r),
				comparer ?? EqualityComparer<TKey>.Default);
	}
}
