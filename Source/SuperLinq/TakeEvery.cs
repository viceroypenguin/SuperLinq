﻿namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Returns every N-th element of a sequence.
	/// </summary>
	/// <typeparam name="TSource">Type of the source sequence</typeparam>
	/// <param name="source">Source sequence</param>
	/// <param name="step">Number of elements to bypass before returning the next element.</param>
	/// <returns>
	/// A sequence with every N-th element of the input sequence.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="step"/> is negative.</exception>
	/// <remarks>
	/// This operator uses deferred execution and streams its results.
	/// </remarks>
	/// <example>
	/// <code><![CDATA[
	/// int[] numbers = { 1, 2, 3, 4, 5 };
	/// var result = numbers.TakeEvery(2);
	/// ]]></code>
	/// The <c>result</c> variable, when iterated over, will yield 1, 3 and 5, in turn.
	/// </example>
	public static IEnumerable<TSource> TakeEvery<TSource>(this IEnumerable<TSource> source, int step)
	{
		Guard.IsNotNull(source);
		Guard.IsGreaterThanOrEqualTo(step, 1);
		return source.Where((e, i) => i % step == 0);
	}
}
