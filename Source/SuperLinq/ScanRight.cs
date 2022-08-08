namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Peforms a right-associative scan (inclusive prefix) on a sequence of elements.
	/// This operator is the right-associative version of the
	/// <see cref="EnumerableEx.Scan{TSource}(IEnumerable{TSource}, Func{TSource, TSource, TSource})"/> LINQ operator.
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

	public static IEnumerable<TSource> ScanRight<TSource>(this IEnumerable<TSource> source, Func<TSource, TSource, TSource> func)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(func);

		return _(source, func);

		static IEnumerable<TSource> _(IEnumerable<TSource> source, Func<TSource, TSource, TSource> func)
		{
			using var e = source.Reverse().GetEnumerator();

			if (!e.MoveNext())
				yield break;

			var seed = e.Current;
			var stack = new Stack<TSource>();
			stack.Push(seed);

			while (e.MoveNext())
			{
				seed = func(e.Current, seed);
				stack.Push(seed);
			}

			foreach (var item in stack)
				yield return item;
		}
	}

	/// <summary>
	/// Peforms a right-associative scan (inclusive prefix) on a sequence of elements.
	/// The specified seed value is used as the initial accumulator value.
	/// This operator is the right-associative version of the
	/// <see cref="EnumerableEx.Scan{TSource, TState}(IEnumerable{TSource}, TState, Func{TState, TSource, TState})"/> LINQ operator.
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

	public static IEnumerable<TAccumulate> ScanRight<TSource, TAccumulate>(this IEnumerable<TSource> source, TAccumulate seed, Func<TSource, TAccumulate, TAccumulate> func)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(func);

		return _(source, seed, func);

		static IEnumerable<TAccumulate> _(IEnumerable<TSource> source, TAccumulate seed, Func<TSource, TAccumulate, TAccumulate> func)
		{
			var stack = new Stack<TAccumulate>();
			stack.Push(seed);

			foreach (var i in source.Reverse())
			{
				seed = func(i, seed);
				stack.Push(seed);
			}

			foreach (var item in stack)
				yield return item;
		}
	}
}
