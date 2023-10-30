﻿namespace SuperLinq;

public partial class SuperEnumerable
{
	/// <summary>
	/// Generates an enumerable sequence by repeating a source sequence as long as the given loop postcondition holds.
	/// </summary>
	/// <typeparam name="TSource">Source sequence element type.</typeparam>
	/// <param name="source">Source sequence to repeat while the condition evaluates true.</param>
	/// <param name="condition">Loop condition.</param>
	/// <returns>Sequence generated by repeating the given sequence until the condition evaluates to false.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="condition"/> is <see
	/// langword="null"/>.</exception>
	/// <remarks>
	/// <para>
	/// <paramref name="condition"/> is evaluated lazily, once at the end of each loop of <paramref name="source"/>.
	/// </para>
	/// <para>
	/// <paramref name="source"/> is cached via <see cref="Memoize{TSource}(IEnumerable{TSource}, bool)"/>, so that it
	/// is only iterated once during the first loop. Successive loops will enumerate the cache instead of <paramref
	/// name="source"/>.
	/// </para>
	/// </remarks>
	public static IEnumerable<TSource> DoWhile<TSource>(this IEnumerable<TSource> source, Func<bool> condition)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(condition);

		return Core(source, condition);

		static IEnumerable<TSource> Core(IEnumerable<TSource> source, Func<bool> condition)
		{
			using var memo = source.Memoize();

			do
			{
				foreach (var item in memo)
					yield return item;
			} while (condition());
		}
	}
}
