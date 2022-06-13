using System.Runtime.CompilerServices;

namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Returns the minimal elements of the given sequence, based on
	/// the given projection.
	/// </summary>
	/// <remarks>
	/// This overload uses the default comparer for the projected type.
	/// This operator uses deferred execution. The results are evaluated
	/// and cached on first use to returned sequence.
	/// </remarks>
	/// <typeparam name="TSource">Type of the source sequence</typeparam>
	/// <typeparam name="TKey">Type of the projected element</typeparam>
	/// <param name="source">Source sequence</param>
	/// <param name="selector">Selector to use to pick the results to compare</param>
	/// <returns>The sequence of minimal elements, according to the projection.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null</exception>

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[Obsolete("MinBy() conflicts with new .NET Core method. Use MinElementsBy() instead.")]
#if NET6_0_OR_GREATER
	public static IExtremaEnumerable<TSource> MinBy<TSource, TKey>(
		IEnumerable<TSource> source,
		Func<TSource, TKey> selector)
#else
	public static IExtremaEnumerable<TSource> MinBy<TSource, TKey>(
		this IEnumerable<TSource> source,
		Func<TSource, TKey> selector)
#endif
		=> MinElementsBy(source, selector);

	/// <summary>
	/// Returns the minimal elements of the given sequence, based on
	/// the given projection and the specified comparer for projected values.
	/// </summary>
	/// <remarks>
	/// This operator uses deferred execution. The results are evaluated
	/// and cached on first use to returned sequence.
	/// </remarks>
	/// <typeparam name="TSource">Type of the source sequence</typeparam>
	/// <typeparam name="TKey">Type of the projected element</typeparam>
	/// <param name="source">Source sequence</param>
	/// <param name="selector">Selector to use to pick the results to compare</param>
	/// <param name="comparer">Comparer to use to compare projected values</param>
	/// <returns>The sequence of minimal elements, according to the projection.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/>, <paramref name="selector"/>
	/// or <paramref name="comparer"/> is null</exception>

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[Obsolete("MinBy() conflicts with new .NET Core method. Use MinElementsBy() instead.")]
#if NET6_0_OR_GREATER
	public static IExtremaEnumerable<TSource> MinBy<TSource, TKey>(
		IEnumerable<TSource> source,
		Func<TSource, TKey> selector, IComparer<TKey>? comparer)
#else
	public static IExtremaEnumerable<TSource> MinBy<TSource, TKey>(
		this IEnumerable<TSource> source,
		Func<TSource, TKey> selector,
		IComparer<TKey>? comparer)
#endif
		=> MinElementsBy(source, selector, comparer);
}
