﻿namespace SuperLinq.Async;

public partial class AsyncSuperEnumerable
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
	/// <paramref name="source"/> is cached via <see cref="Memoize{TSource}(IAsyncEnumerable{TSource})"/>, so that it
	/// is only iterated once during the first loop. Successive loops will enumerate the cache instead of <paramref
	/// name="source"/>.
	/// </para>
	/// </remarks>
	public static IAsyncEnumerable<TSource> DoWhile<TSource>(this IAsyncEnumerable<TSource> source, Func<bool> condition)
	{
		ArgumentNullException.ThrowIfNull(condition);

		return DoWhile(source, condition.ToAsync());
	}

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
	/// <paramref name="source"/> is cached via <see cref="Memoize{TSource}(IAsyncEnumerable{TSource})"/>, so that it
	/// is only iterated once during the first loop. Successive loops will enumerate the cache instead of <paramref
	/// name="source"/>.
	/// </para>
	/// </remarks>
	public static IAsyncEnumerable<TSource> DoWhile<TSource>(this IAsyncEnumerable<TSource> source, Func<ValueTask<bool>> condition)
	{

		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(condition);

		return Core(source, condition);

		static async IAsyncEnumerable<TSource> Core(
			IAsyncEnumerable<TSource> source, Func<ValueTask<bool>> condition,
			[EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			await using var memo = source.Memoize();

			do
			{
				await foreach (var item in memo.WithCancellation(cancellationToken).ConfigureAwait(false))
					yield return item;
			} while (await condition().ConfigureAwait(false));
		}
	}
}
