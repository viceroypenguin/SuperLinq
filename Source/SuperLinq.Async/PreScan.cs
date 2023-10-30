﻿namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Performs a pre-scan (exclusive prefix sum) on a sequence of elements.
	/// </summary>
	/// <remarks>
	/// <para>
	/// An exclusive prefix sum returns an equal-length sequence where the
	/// N-th element is the sum of the first N-1 input elements (the first
	/// element is a special case, it is set to the identity). More
	/// generally, the pre-scan allows any commutative binary operation,
	/// not just a sum.
	/// </para>
	/// <para>
	/// The inclusive version of PreScan is <see cref="ScanEx{TSource}"/>.
	/// </para>
	/// <para>
	/// This operator uses deferred execution and streams its result.
	/// </para>
	/// <example>
	/// <code><![CDATA[
	/// int[] values = { 1, 2, 3, 4 };
	/// var prescan = values.PreScan((a, b) => a + b, 0);
	/// var scan = values.Scan((a, b) => a + b);
	/// ]]></code>
	/// <c>prescan</c> will yield <c>{ 0, 1, 3, 6 }</c>, while <c>scan</c>
	/// will yield <c>{ 1, 3, 6, 10 }</c>. This shows the relationship 
	/// between the inclusive and exclusive prefix sum.
	/// </example>
	/// </remarks>
	/// <typeparam name="TSource">Type of elements in source sequence</typeparam>
	/// <param name="source">Source sequence</param>
	/// <param name="transformation">Transformation operation</param>
	/// <param name="identity">Identity element (see remarks)</param>
	/// <returns>The scanned sequence</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
	/// <exception cref="ArgumentNullException"><paramref name="transformation"/> is null</exception>
	public static IAsyncEnumerable<TSource> PreScan<TSource>(
		this IAsyncEnumerable<TSource> source,
		Func<TSource, TSource, TSource> transformation,
		TSource identity)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(transformation);

		return Core(source, transformation, identity);

		static async IAsyncEnumerable<TSource> Core(
			IAsyncEnumerable<TSource> source,
			Func<TSource, TSource, TSource> transformation,
			TSource identity,
			[EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			var aggregator = identity;
			await using var e = source.GetConfiguredAsyncEnumerator(cancellationToken);

			if (!await e.MoveNextAsync())
				yield break;

			yield return aggregator;
			var current = e.Current;

			while (await e.MoveNextAsync())
			{
				aggregator = transformation(aggregator, current);
				yield return aggregator;
				current = e.Current;
			}
		}
	}
}
