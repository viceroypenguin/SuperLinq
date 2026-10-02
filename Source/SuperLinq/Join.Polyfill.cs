namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>Performs a left outer join on two sequences.</summary>
	/// <typeparam name="TOuter">The type of the elements of the first sequence.</typeparam>
	/// <typeparam name="TInner">The type of the elements of the second sequence.</typeparam>
	/// <typeparam name="TKey">The type of the keys returned by the key selector functions.</typeparam>
	/// <typeparam name="TResult">The type of the result elements.</typeparam>
	/// <param name="outer">The first sequence to join.</param>
	/// <param name="inner">The sequence to join to the first sequence.</param>
	/// <param name="outerKeySelector">A function to extract the join key from each element of the first sequence.</param>
	/// <param name="innerKeySelector">A function to extract the join key from each element of the second sequence.</param>
	/// <param name="resultSelector">A function to create a result element from two correlated elements.</param>
	/// <returns>A sequence containing the result of a left outer join.</returns>
	/// <remarks>This polyfill specifically is a proxy for <see cref="LeftOuterHashJoin{TLeft, TRight, TKey, TResult}(IEnumerable{TLeft}, IEnumerable{TRight}, Func{TLeft, TKey}, Func{TRight, TKey}, Func{TLeft, TResult}, Func{TLeft, TRight, TResult}, IEqualityComparer{TKey}?)"/></remarks>
#if NET10_0_OR_GREATER
	[Obsolete("This method has been implemented by the framework.")]
	public static IEnumerable<TResult> LeftJoin<TOuter, TInner, TKey, TResult>(
		IEnumerable<TOuter> outer,
#else
	public static IEnumerable<TResult> LeftJoin<TOuter, TInner, TKey, TResult>(
		this IEnumerable<TOuter> outer,
#endif
		IEnumerable<TInner> inner,
		Func<TOuter, TKey> outerKeySelector,
		Func<TInner, TKey> innerKeySelector,
		Func<TOuter, TInner?, TResult> resultSelector) =>
		LeftJoin(outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer: null);

	/// <summary>Performs a left outer join on two sequences using a specified key comparer.</summary>
	/// <typeparam name="TOuter">The type of the elements of the first sequence.</typeparam>
	/// <typeparam name="TInner">The type of the elements of the second sequence.</typeparam>
	/// <typeparam name="TKey">The type of the keys returned by the key selector functions.</typeparam>
	/// <typeparam name="TResult">The type of the result elements.</typeparam>
	/// <param name="outer">The first sequence to join.</param>
	/// <param name="inner">The sequence to join to the first sequence.</param>
	/// <param name="outerKeySelector">A function to extract the join key from each element of the first sequence.</param>
	/// <param name="innerKeySelector">A function to extract the join key from each element of the second sequence.</param>
	/// <param name="resultSelector">A function to create a result element from two correlated elements.</param>
	/// <param name="comparer">An equality comparer to compare keys.</param>
	/// <returns>A sequence containing the result of a left outer join.</returns>
	/// <remarks>This polyfill specifically is a proxy for <see cref="LeftOuterHashJoin{TLeft, TRight, TKey, TResult}(IEnumerable{TLeft}, IEnumerable{TRight}, Func{TLeft, TKey}, Func{TRight, TKey}, Func{TLeft, TResult}, Func{TLeft, TRight, TResult}, IEqualityComparer{TKey}?)"/></remarks>
#if NET10_0_OR_GREATER
	[Obsolete("This method has been implemented by the framework.")]
	public static IEnumerable<TResult> LeftJoin<TOuter, TInner, TKey, TResult>(
		IEnumerable<TOuter> outer,
#else
	public static IEnumerable<TResult> LeftJoin<TOuter, TInner, TKey, TResult>(
		this IEnumerable<TOuter> outer,
#endif
		IEnumerable<TInner> inner,
		Func<TOuter, TKey> outerKeySelector,
		Func<TInner, TKey> innerKeySelector,
		Func<TOuter, TInner?, TResult> resultSelector,
		IEqualityComparer<TKey>? comparer)
	{
		ArgumentNullException.ThrowIfNull(outer);
		ArgumentNullException.ThrowIfNull(inner);
		ArgumentNullException.ThrowIfNull(outerKeySelector);
		ArgumentNullException.ThrowIfNull(innerKeySelector);
		ArgumentNullException.ThrowIfNull(resultSelector);

		return outer.LeftOuterHashJoin(
			inner,
			outerKeySelector,
			innerKeySelector,
			outer => resultSelector(outer, default),
			resultSelector,
			comparer);
	}

	/// <summary>Performs a right outer join on two sequences.</summary>
	/// <typeparam name="TOuter">The type of the elements of the first sequence.</typeparam>
	/// <typeparam name="TInner">The type of the elements of the second sequence.</typeparam>
	/// <typeparam name="TKey">The type of the keys returned by the key selector functions.</typeparam>
	/// <typeparam name="TResult">The type of the result elements.</typeparam>
	/// <param name="outer">The first sequence to join.</param>
	/// <param name="inner">The sequence to join to the first sequence.</param>
	/// <param name="outerKeySelector">A function to extract the join key from each element of the first sequence.</param>
	/// <param name="innerKeySelector">A function to extract the join key from each element of the second sequence.</param>
	/// <param name="resultSelector">A function to create a result element from two correlated elements.</param>
	/// <returns>A sequence containing the result of a right outer join.</returns>
	/// <remarks>This polyfill specifically is a proxy for <see cref="RightOuterHashJoin{TLeft, TRight, TKey, TResult}(IEnumerable{TLeft}, IEnumerable{TRight}, Func{TLeft, TKey}, Func{TRight, TKey}, Func{TRight, TResult}, Func{TLeft, TRight, TResult}, IEqualityComparer{TKey}?)"/></remarks>
#if NET10_0_OR_GREATER
	[Obsolete("This method has been implemented by the framework.")]
	public static IEnumerable<TResult> RightJoin<TOuter, TInner, TKey, TResult>(
		IEnumerable<TOuter> outer,
#else
	public static IEnumerable<TResult> RightJoin<TOuter, TInner, TKey, TResult>(
		this IEnumerable<TOuter> outer,
#endif
		IEnumerable<TInner> inner,
		Func<TOuter, TKey> outerKeySelector,
		Func<TInner, TKey> innerKeySelector,
		Func<TOuter?, TInner, TResult> resultSelector) =>
		RightJoin(outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer: null);

	/// <summary>Performs a right outer join on two sequences using a specified key comparer.</summary>
	/// <typeparam name="TOuter">The type of the elements of the first sequence.</typeparam>
	/// <typeparam name="TInner">The type of the elements of the second sequence.</typeparam>
	/// <typeparam name="TKey">The type of the keys returned by the key selector functions.</typeparam>
	/// <typeparam name="TResult">The type of the result elements.</typeparam>
	/// <param name="outer">The first sequence to join.</param>
	/// <param name="inner">The sequence to join to the first sequence.</param>
	/// <param name="outerKeySelector">A function to extract the join key from each element of the first sequence.</param>
	/// <param name="innerKeySelector">A function to extract the join key from each element of the second sequence.</param>
	/// <param name="resultSelector">A function to create a result element from two correlated elements.</param>
	/// <param name="comparer">An equality comparer to compare keys.</param>
	/// <returns>A sequence containing the result of a right outer join.</returns>
	/// <remarks>This polyfill specifically is a proxy for <see cref="RightOuterHashJoin{TLeft, TRight, TKey, TResult}(IEnumerable{TLeft}, IEnumerable{TRight}, Func{TLeft, TKey}, Func{TRight, TKey}, Func{TRight, TResult}, Func{TLeft, TRight, TResult}, IEqualityComparer{TKey}?)"/></remarks>
#if NET10_0_OR_GREATER
	[Obsolete("This method has been implemented by the framework.")]
	public static IEnumerable<TResult> RightJoin<TOuter, TInner, TKey, TResult>(
		IEnumerable<TOuter> outer,
#else
	public static IEnumerable<TResult> RightJoin<TOuter, TInner, TKey, TResult>(
		this IEnumerable<TOuter> outer,
#endif
		IEnumerable<TInner> inner,
		Func<TOuter, TKey> outerKeySelector,
		Func<TInner, TKey> innerKeySelector,
		Func<TOuter?, TInner, TResult> resultSelector,
		IEqualityComparer<TKey>? comparer)
	{
		ArgumentNullException.ThrowIfNull(outer);
		ArgumentNullException.ThrowIfNull(inner);
		ArgumentNullException.ThrowIfNull(outerKeySelector);
		ArgumentNullException.ThrowIfNull(innerKeySelector);
		ArgumentNullException.ThrowIfNull(resultSelector);

		return outer.RightOuterHashJoin(
			inner,
			outerKeySelector,
			innerKeySelector,
			inner => resultSelector(default, inner),
			resultSelector,
			comparer);
	}

	/// <summary>Performs a full outer join on two sequences.</summary>
	/// <typeparam name="TOuter">The type of the elements of the first sequence.</typeparam>
	/// <typeparam name="TInner">The type of the elements of the second sequence.</typeparam>
	/// <typeparam name="TKey">The type of the keys returned by the key selector functions.</typeparam>
	/// <param name="outer">The first sequence to join.</param>
	/// <param name="inner">The sequence to join to the first sequence.</param>
	/// <param name="outerKeySelector">A function to extract the join key from each element of the first sequence.</param>
	/// <param name="innerKeySelector">A function to extract the join key from each element of the second sequence.</param>
	/// <param name="comparer">An equality comparer to compare keys.</param>
	/// <returns>A sequence containing the pairs produced by a full outer join.</returns>
	/// <remarks>This polyfill specifically is a proxy for <see cref="FullOuterHashJoin{TLeft, TRight, TKey, TResult}(IEnumerable{TLeft}, IEnumerable{TRight}, Func{TLeft, TKey}, Func{TRight, TKey}, Func{TLeft, TResult}, Func{TRight, TResult}, Func{TLeft, TRight, TResult}, IEqualityComparer{TKey}?)"/></remarks>
#if NET11_0_OR_GREATER
	[Obsolete("This method has been implemented by the framework.")]
	public static IEnumerable<(TOuter? Outer, TInner? Inner)> FullJoin<TOuter, TInner, TKey>(
		IEnumerable<TOuter> outer,
#else
	public static IEnumerable<(TOuter? Outer, TInner? Inner)> FullJoin<TOuter, TInner, TKey>(
		this IEnumerable<TOuter> outer,
#endif
		IEnumerable<TInner> inner,
		Func<TOuter, TKey> outerKeySelector,
		Func<TInner, TKey> innerKeySelector,
		IEqualityComparer<TKey>? comparer = null)
	{
		ArgumentNullException.ThrowIfNull(outer);
		ArgumentNullException.ThrowIfNull(inner);
		ArgumentNullException.ThrowIfNull(outerKeySelector);
		ArgumentNullException.ThrowIfNull(innerKeySelector);

		return outer.FullOuterHashJoin(inner, outerKeySelector, innerKeySelector, comparer);
	}

	/// <summary>Performs a full outer join on two sequences.</summary>
	/// <typeparam name="TOuter">The type of the elements of the first sequence.</typeparam>
	/// <typeparam name="TInner">The type of the elements of the second sequence.</typeparam>
	/// <typeparam name="TKey">The type of the keys returned by the key selector functions.</typeparam>
	/// <typeparam name="TResult">The type of the result elements.</typeparam>
	/// <param name="outer">The first sequence to join.</param>
	/// <param name="inner">The sequence to join to the first sequence.</param>
	/// <param name="outerKeySelector">A function to extract the join key from each element of the first sequence.</param>
	/// <param name="innerKeySelector">A function to extract the join key from each element of the second sequence.</param>
	/// <param name="resultSelector">A function to create a result element from two correlated elements.</param>
	/// <param name="comparer">An equality comparer to compare keys.</param>
	/// <returns>A sequence containing the result of a full outer join.</returns>
	/// <remarks>This polyfill specifically is a proxy for <see cref="FullOuterHashJoin{TLeft, TRight, TKey, TResult}(IEnumerable{TLeft}, IEnumerable{TRight}, Func{TLeft, TKey}, Func{TRight, TKey}, Func{TLeft, TResult}, Func{TRight, TResult}, Func{TLeft, TRight, TResult}, IEqualityComparer{TKey}?)"/></remarks>
#if NET11_0_OR_GREATER
	[Obsolete("This method has been implemented by the framework.")]
	public static IEnumerable<TResult> FullJoin<TOuter, TInner, TKey, TResult>(
		IEnumerable<TOuter> outer,
#else
	public static IEnumerable<TResult> FullJoin<TOuter, TInner, TKey, TResult>(
		this IEnumerable<TOuter> outer,
#endif
		IEnumerable<TInner> inner,
		Func<TOuter, TKey> outerKeySelector,
		Func<TInner, TKey> innerKeySelector,
		Func<TOuter?, TInner?, TResult> resultSelector,
		IEqualityComparer<TKey>? comparer = null)
	{
		ArgumentNullException.ThrowIfNull(outer);
		ArgumentNullException.ThrowIfNull(inner);
		ArgumentNullException.ThrowIfNull(outerKeySelector);
		ArgumentNullException.ThrowIfNull(innerKeySelector);
		ArgumentNullException.ThrowIfNull(resultSelector);

		return outer.FullOuterHashJoin(
			inner,
			outerKeySelector,
			innerKeySelector,
			outer => resultSelector(outer, default),
			inner => resultSelector(default, inner),
			resultSelector,
			comparer);
	}
}
