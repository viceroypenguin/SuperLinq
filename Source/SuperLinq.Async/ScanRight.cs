﻿namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Performs a right-associative scan (inclusive prefix) on a sequence of elements.
	/// This operator is the right-associative version of the
	/// <see cref="AsyncEnumerableEx.Scan{TSource}(IAsyncEnumerable{TSource}, Func{TSource, TSource, TSource})"/> LINQ operator.
	/// </summary>
	/// <typeparam name="TSource">Type of elements in source sequence.</typeparam>
	/// <param name="source">Source sequence.</param>
	/// <param name="func">
	/// A right-associative accumulator function to be invoked on each element.
	/// Its first argument is the current value in the sequence; second argument is the previous accumulator value.
	/// </param>
	/// <returns>The scanned sequence.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="func"/> is <see langword="null"/>.</exception>
	/// <example>
	/// <code><![CDATA[
	/// var result = Enumerable.Range(1, 5).Select(i => i.ToString()).ScanRight((a, b) => $"({a}+{b})");
	/// ]]></code>
	/// The <c>result</c> variable will contain <c>[ "(1+(2+(3+(4+5))))", "(2+(3+(4+5)))", "(3+(4+5))", "(4+5)", "5" ]</c>.
	/// </example>
	/// <remarks>
	/// This operator uses deferred execution and streams its results.
	/// Source sequence is consumed greedily when an iteration of the resulting sequence begins.
	/// </remarks>
	public static IAsyncEnumerable<TSource> ScanRight<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, TSource, TSource> func)
	{
		return source.ScanRight((a, b, ct) => new ValueTask<TSource>(func(a, b)));
	}

	/// <summary>
	/// Performs a right-associative scan (inclusive prefix) on a sequence of elements.
	/// This operator is the right-associative version of the
	/// <see cref="AsyncEnumerableEx.Scan{TSource}(IAsyncEnumerable{TSource}, Func{TSource, TSource, TSource})"/> LINQ operator.
	/// </summary>
	/// <typeparam name="TSource">Type of elements in source sequence.</typeparam>
	/// <param name="source">Source sequence.</param>
	/// <param name="func">
	/// A right-associative accumulator function to be invoked on each element.
	/// Its first argument is the current value in the sequence; second argument is the previous accumulator value.
	/// </param>
	/// <returns>The scanned sequence.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="func"/> is <see langword="null"/>.</exception>
	/// <example>
	/// <code><![CDATA[
	/// var result = Enumerable.Range(1, 5).Select(i => i.ToString()).ScanRight((a, b) => $"({a}+{b})");
	/// ]]></code>
	/// The <c>result</c> variable will contain <c>[ "(1+(2+(3+(4+5))))", "(2+(3+(4+5)))", "(3+(4+5))", "(4+5)", "5" ]</c>.
	/// </example>
	/// <remarks>
	/// This operator uses deferred execution and streams its results.
	/// Source sequence is consumed greedily when an iteration of the resulting sequence begins.
	/// </remarks>
	public static IAsyncEnumerable<TSource> ScanRight<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, TSource, ValueTask<TSource>> func)
	{
		return source.ScanRight((a, b, ct) => func(a, b));
	}

	/// <summary>
	/// Performs a right-associative scan (inclusive prefix) on a sequence of elements.
	/// This operator is the right-associative version of the
	/// <see cref="AsyncEnumerableEx.Scan{TSource}(IAsyncEnumerable{TSource}, Func{TSource, TSource, TSource})"/> LINQ operator.
	/// </summary>
	/// <typeparam name="TSource">Type of elements in source sequence.</typeparam>
	/// <param name="source">Source sequence.</param>
	/// <param name="func">
	/// A right-associative accumulator function to be invoked on each element.
	/// Its first argument is the current value in the sequence; second argument is the previous accumulator value.
	/// </param>
	/// <returns>The scanned sequence.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="func"/> is <see langword="null"/>.</exception>
	/// <example>
	/// <code><![CDATA[
	/// var result = Enumerable.Range(1, 5).Select(i => i.ToString()).ScanRight((a, b) => $"({a}+{b})");
	/// ]]></code>
	/// The <c>result</c> variable will contain <c>[ "(1+(2+(3+(4+5))))", "(2+(3+(4+5)))", "(3+(4+5))", "(4+5)", "5" ]</c>.
	/// </example>
	/// <remarks>
	/// This operator uses deferred execution and streams its results.
	/// Source sequence is consumed greedily when an iteration of the resulting sequence begins.
	/// </remarks>
	public static IAsyncEnumerable<TSource> ScanRight<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, TSource, CancellationToken, ValueTask<TSource>> func)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(func);

		return Core(source, func);

		static async IAsyncEnumerable<TSource> Core(IAsyncEnumerable<TSource> source, Func<TSource, TSource, CancellationToken, ValueTask<TSource>> func, [EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			var list = await source.ToListAsync(cancellationToken).ConfigureAwait(false);

			if (list.Count == 0)
				yield break;

			var seed = list[^1];
			var stack = new Stack<TSource>(list.Count);
			stack.Push(seed);

			for (var i = list.Count - 2; i >= 0; i--)
			{
				seed = await func(list[i], seed, cancellationToken).ConfigureAwait(false);
				stack.Push(seed);
			}

			foreach (var item in stack)
				yield return item;
		}
	}

	/// <summary>
	/// Performs a right-associative scan (inclusive prefix) on a sequence of elements.
	/// The specified seed value is used as the initial accumulator value.
	/// This operator is the right-associative version of the
	/// <see cref="AsyncEnumerableEx.Scan{TSource, TAccumulate}(IAsyncEnumerable{TSource}, TAccumulate, Func{TAccumulate, TSource, TAccumulate})"/> LINQ operator.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements of source.</typeparam>
	/// <typeparam name="TAccumulate">The type of the accumulator value.</typeparam>
	/// <param name="source">Source sequence.</param>
	/// <param name="seed">The initial accumulator value.</param>
	/// <param name="func">A right-associative accumulator function to be invoked on each element.</param>
	/// <returns>The scanned sequence.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="seed"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="func"/> is <see langword="null"/>.</exception>
	/// <example>
	/// <code><![CDATA[
	/// var result = Enumerable.Range(1, 4).ScanRight("5", (a, b) => string.Format("({0}/{1})", a, b));
	/// ]]></code>
	/// The <c>result</c> variable will contain <c>[ "(1+(2+(3+(4+5))))", "(2+(3+(4+5)))", "(3+(4+5))", "(4+5)", "5" ]</c>.
	/// </example>
	/// <remarks>
	/// This operator uses deferred execution and streams its results.
	/// Source sequence is consumed greedily when an iteration of the resulting sequence begins.
	/// </remarks>
	public static IAsyncEnumerable<TAccumulate> ScanRight<TSource, TAccumulate>(this IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TSource, TAccumulate, TAccumulate> func)
	{
		return source.ScanRight(seed, (a, b, ct) => new ValueTask<TAccumulate>(func(a, b)));
	}

	/// <summary>
	/// Performs a right-associative scan (inclusive prefix) on a sequence of elements.
	/// The specified seed value is used as the initial accumulator value.
	/// This operator is the right-associative version of the
	/// <see cref="AsyncEnumerableEx.Scan{TSource, TAccumulate}(IAsyncEnumerable{TSource}, TAccumulate, Func{TAccumulate, TSource, TAccumulate})"/> LINQ operator.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements of source.</typeparam>
	/// <typeparam name="TAccumulate">The type of the accumulator value.</typeparam>
	/// <param name="source">Source sequence.</param>
	/// <param name="seed">The initial accumulator value.</param>
	/// <param name="func">A right-associative accumulator function to be invoked on each element.</param>
	/// <returns>The scanned sequence.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="seed"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="func"/> is <see langword="null"/>.</exception>
	/// <example>
	/// <code><![CDATA[
	/// var result = Enumerable.Range(1, 4).ScanRight("5", (a, b) => string.Format("({0}/{1})", a, b));
	/// ]]></code>
	/// The <c>result</c> variable will contain <c>[ "(1+(2+(3+(4+5))))", "(2+(3+(4+5)))", "(3+(4+5))", "(4+5)", "5" ]</c>.
	/// </example>
	/// <remarks>
	/// This operator uses deferred execution and streams its results.
	/// Source sequence is consumed greedily when an iteration of the resulting sequence begins.
	/// </remarks>
	public static IAsyncEnumerable<TAccumulate> ScanRight<TSource, TAccumulate>(this IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TSource, TAccumulate, ValueTask<TAccumulate>> func)
	{
		return source.ScanRight(seed, (a, b, ct) => func(a, b));
	}

	/// <summary>
	/// Performs a right-associative scan (inclusive prefix) on a sequence of elements.
	/// The specified seed value is used as the initial accumulator value.
	/// This operator is the right-associative version of the
	/// <see cref="AsyncEnumerableEx.Scan{TSource, TAccumulate}(IAsyncEnumerable{TSource}, TAccumulate, Func{TAccumulate, TSource, TAccumulate})"/> LINQ operator.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements of source.</typeparam>
	/// <typeparam name="TAccumulate">The type of the accumulator value.</typeparam>
	/// <param name="source">Source sequence.</param>
	/// <param name="seed">The initial accumulator value.</param>
	/// <param name="func">A right-associative accumulator function to be invoked on each element.</param>
	/// <returns>The scanned sequence.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="seed"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="func"/> is <see langword="null"/>.</exception>
	/// <example>
	/// <code><![CDATA[
	/// var result = Enumerable.Range(1, 4).ScanRight("5", (a, b) => string.Format("({0}/{1})", a, b));
	/// ]]></code>
	/// The <c>result</c> variable will contain <c>[ "(1+(2+(3+(4+5))))", "(2+(3+(4+5)))", "(3+(4+5))", "(4+5)", "5" ]</c>.
	/// </example>
	/// <remarks>
	/// This operator uses deferred execution and streams its results.
	/// Source sequence is consumed greedily when an iteration of the resulting sequence begins.
	/// </remarks>
	public static IAsyncEnumerable<TAccumulate> ScanRight<TSource, TAccumulate>(this IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TSource, TAccumulate, CancellationToken, ValueTask<TAccumulate>> func)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(func);

		return Core(source, seed, func);

		static async IAsyncEnumerable<TAccumulate> Core(IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TSource, TAccumulate, CancellationToken, ValueTask<TAccumulate>> func, [EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			var list = await source.ToListAsync(cancellationToken).ConfigureAwait(false);

			var stack = new Stack<TAccumulate>(list.Count + 1);
			stack.Push(seed);

			for (var i = list.Count - 1; i >= 0; i--)
			{
				seed = await func(list[i], seed, cancellationToken).ConfigureAwait(false);
				stack.Push(seed);
			}

			foreach (var item in stack)
				yield return item;
		}
	}
}
