namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Applies a right-associative accumulator function over a sequence.
	/// This operator is the right-associative version of the
	/// <see cref="Enumerable.Aggregate{TSource}(IEnumerable{TSource}, Func{TSource, TSource, TSource})"/> LINQ operator.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements of source.</typeparam>
	/// <param name="source">Source sequence.</param>
	/// <param name="func">A right-associative accumulator function to be invoked on each element.</param>
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
	/// <returns>The final accumulator value.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source" /> is <see langword="null" />.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="func" /> is <see langword="null" />.</exception>
	/// <example>
	/// <code><![CDATA[
	/// string result = Enumerable.Range(1, 5).Select(i => i.ToString()).AggregateRight((a, b) => string.Format("({0}/{1})", a, b));
	/// ]]></code>
	/// The <c>result</c> variable will contain <c>"(1/(2/(3/(4/5))))"</c>.
	/// </example>
	/// <remarks>
	/// This operator executes immediately.
	/// </remarks>
	public static ValueTask<TSource> AggregateRight<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, TSource, TSource> func, CancellationToken cancellationToken = default)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(func);

		return source.AggregateRight((a, b, ct) => new ValueTask<TSource>(func(a, b)), cancellationToken);
	}

	/// <summary>
	/// Applies a right-associative accumulator function over a sequence.
	/// This operator is the right-associative version of the
	/// <see cref="Enumerable.Aggregate{TSource}(IEnumerable{TSource}, Func{TSource, TSource, TSource})"/> LINQ operator.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements of source.</typeparam>
	/// <param name="source">Source sequence.</param>
	/// <param name="func">A right-associative accumulator function to be invoked on each element.</param>
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
	/// <returns>The final accumulator value.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source" /> is <see langword="null" />.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="func" /> is <see langword="null" />.</exception>
	/// <example>
	/// <code><![CDATA[
	/// string result = Enumerable.Range(1, 5).Select(i => i.ToString()).AggregateRight((a, b) => string.Format("({0}/{1})", a, b));
	/// ]]></code>
	/// The <c>result</c> variable will contain <c>"(1/(2/(3/(4/5))))"</c>.
	/// </example>
	/// <remarks>
	/// This operator executes immediately.
	/// </remarks>
	public static ValueTask<TSource> AggregateRight<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, TSource, ValueTask<TSource>> func, CancellationToken cancellationToken = default)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(func);

		return source.AggregateRight((a, b, ct) => func(a, b), cancellationToken);
	}

	/// <summary>
	/// Applies a right-associative accumulator function over a sequence.
	/// This operator is the right-associative version of the
	/// <see cref="Enumerable.Aggregate{TSource}(IEnumerable{TSource}, Func{TSource, TSource, TSource})"/> LINQ operator.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements of source.</typeparam>
	/// <param name="source">Source sequence.</param>
	/// <param name="func">A right-associative accumulator function to be invoked on each element.</param>
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
	/// <returns>The final accumulator value.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source" /> is <see langword="null" />.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="func" /> is <see langword="null" />.</exception>
	/// <example>
	/// <code><![CDATA[
	/// string result = Enumerable.Range(1, 5).Select(i => i.ToString()).AggregateRight((a, b) => string.Format("({0}/{1})", a, b));
	/// ]]></code>
	/// The <c>result</c> variable will contain <c>"(1/(2/(3/(4/5))))"</c>.
	/// </example>
	/// <remarks>
	/// This operator executes immediately.
	/// </remarks>
	public static ValueTask<TSource> AggregateRight<TSource>(
		this IAsyncEnumerable<TSource> source,
		Func<TSource, TSource, CancellationToken, ValueTask<TSource>> func,
		CancellationToken cancellationToken = default)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(func);

		return Core(source, func, cancellationToken);

		static async ValueTask<TSource> Core(
			IAsyncEnumerable<TSource> source,
			Func<TSource, TSource, CancellationToken, ValueTask<TSource>> func,
			CancellationToken cancellationToken = default)
		{
			await using var e = source.Reverse().GetConfiguredAsyncEnumerator(cancellationToken);

			if (!await e.MoveNextAsync())
				ThrowHelper.ThrowInvalidOperationException("Sequence contains no elements");

			var seed = e.Current;

			while (await e.MoveNextAsync())
				seed = await func(e.Current, seed, cancellationToken).ConfigureAwait(false);

			return seed;
		}
	}

	/// <summary>
	/// Applies a right-associative accumulator function over a sequence.
	/// The specified seed value is used as the initial accumulator value.
	/// This operator is the right-associative version of the
	/// <see cref="Enumerable.Aggregate{TSource, TAccumulate}(IEnumerable{TSource}, TAccumulate, Func{TAccumulate, TSource, TAccumulate})"/> LINQ operator.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements of source.</typeparam>
	/// <typeparam name="TAccumulate">The type of the accumulator value.</typeparam>
	/// <param name="source">Source sequence.</param>
	/// <param name="seed">The initial accumulator value.</param>
	/// <param name="func">A right-associative accumulator function to be invoked on each element.</param>
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
	/// <returns>The final accumulator value.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source" /> is <see langword="null" />.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="func" /> is <see langword="null" />.</exception>
	/// <example>
	/// <code><![CDATA[
	/// var numbers = Enumerable.Range(1, 5);
	/// string result = numbers.AggregateRight("6", (a, b) => string.Format("({0}/{1})", a, b));
	/// ]]></code>
	/// The <c>result</c> variable will contain <c>"(1/(2/(3/(4/(5/6)))))"</c>.
	/// </example>
	/// <remarks>
	/// This operator executes immediately.
	/// </remarks>

	public static ValueTask<TAccumulate> AggregateRight<TSource, TAccumulate>(this IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TSource, TAccumulate, TAccumulate> func, CancellationToken cancellationToken = default)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(func);

		return source.AggregateRight(seed, (a, b, ct) => new ValueTask<TAccumulate>(func(a, b)), cancellationToken);
	}

	/// <summary>
	/// Applies a right-associative accumulator function over a sequence.
	/// The specified seed value is used as the initial accumulator value.
	/// This operator is the right-associative version of the
	/// <see cref="Enumerable.Aggregate{TSource, TAccumulate}(IEnumerable{TSource}, TAccumulate, Func{TAccumulate, TSource, TAccumulate})"/> LINQ operator.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements of source.</typeparam>
	/// <typeparam name="TAccumulate">The type of the accumulator value.</typeparam>
	/// <param name="source">Source sequence.</param>
	/// <param name="seed">The initial accumulator value.</param>
	/// <param name="func">A right-associative accumulator function to be invoked on each element.</param>
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
	/// <returns>The final accumulator value.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source" /> is <see langword="null" />.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="func" /> is <see langword="null" />.</exception>
	/// <example>
	/// <code><![CDATA[
	/// var numbers = Enumerable.Range(1, 5);
	/// string result = numbers.AggregateRight("6", (a, b) => string.Format("({0}/{1})", a, b));
	/// ]]></code>
	/// The <c>result</c> variable will contain <c>"(1/(2/(3/(4/(5/6)))))"</c>.
	/// </example>
	/// <remarks>
	/// This operator executes immediately.
	/// </remarks>

	public static ValueTask<TAccumulate> AggregateRight<TSource, TAccumulate>(this IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TSource, TAccumulate, ValueTask<TAccumulate>> func, CancellationToken cancellationToken = default)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(func);

		return source.AggregateRight(seed, (a, b, ct) => func(a, b), cancellationToken);
	}

	/// <summary>
	/// Applies a right-associative accumulator function over a sequence.
	/// The specified seed value is used as the initial accumulator value.
	/// This operator is the right-associative version of the
	/// <see cref="Enumerable.Aggregate{TSource, TAccumulate}(IEnumerable{TSource}, TAccumulate, Func{TAccumulate, TSource, TAccumulate})"/> LINQ operator.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements of source.</typeparam>
	/// <typeparam name="TAccumulate">The type of the accumulator value.</typeparam>
	/// <param name="source">Source sequence.</param>
	/// <param name="seed">The initial accumulator value.</param>
	/// <param name="func">A right-associative accumulator function to be invoked on each element.</param>
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
	/// <returns>The final accumulator value.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source" /> is <see langword="null" />.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="func" /> is <see langword="null" />.</exception>
	/// <example>
	/// <code><![CDATA[
	/// var numbers = Enumerable.Range(1, 5);
	/// string result = numbers.AggregateRight("6", (a, b) => string.Format("({0}/{1})", a, b));
	/// ]]></code>
	/// The <c>result</c> variable will contain <c>"(1/(2/(3/(4/(5/6)))))"</c>.
	/// </example>
	/// <remarks>
	/// This operator executes immediately.
	/// </remarks>

	public static ValueTask<TAccumulate> AggregateRight<TSource, TAccumulate>(this IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TSource, TAccumulate, CancellationToken, ValueTask<TAccumulate>> func, CancellationToken cancellationToken = default)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(func);

		return Core(source, seed, func, cancellationToken);

		static async ValueTask<TAccumulate> Core(
			IAsyncEnumerable<TSource> source,
			TAccumulate seed,
			Func<TSource, TAccumulate, CancellationToken, ValueTask<TAccumulate>> func,
			CancellationToken cancellationToken = default)
		{
			await foreach (var i in source.Reverse().WithCancellation(cancellationToken).ConfigureAwait(false))
				seed = await func(i, seed, cancellationToken).ConfigureAwait(false);

			return seed;
		}
	}

	/// <summary>
	/// Applies a right-associative accumulator function over a sequence.
	/// The specified seed value is used as the initial accumulator value,
	/// and the specified function is used to select the result value.
	/// This operator is the right-associative version of the
	/// <see cref="Enumerable.Aggregate{TSource, TAccumulate, TResult}(IEnumerable{TSource}, TAccumulate, Func{TAccumulate, TSource, TAccumulate}, Func{TAccumulate, TResult})"/> LINQ operator.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements of source.</typeparam>
	/// <typeparam name="TAccumulate">The type of the accumulator value.</typeparam>
	/// <typeparam name="TResult">The type of the resulting value.</typeparam>
	/// <param name="source">Source sequence.</param>
	/// <param name="seed">The initial accumulator value.</param>
	/// <param name="func">A right-associative accumulator function to be invoked on each element.</param>
	/// <param name="resultSelector">A function to transform the final accumulator value into the result value.</param>
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
	/// <returns>The transformed final accumulator value.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source" /> is <see langword="null" />.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="func" /> is <see langword="null" />.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="resultSelector" /> is <see langword="null" />.</exception>
	/// <example>
	/// <code><![CDATA[
	/// var numbers = Enumerable.Range(1, 5);
	/// int result = numbers.AggregateRight("6", (a, b) => string.Format("({0}/{1})", a, b), str => str.Length);
	/// ]]></code>
	/// The <c>result</c> variable will contain <c>21</c>.
	/// </example>
	/// <remarks>
	/// This operator executes immediately.
	/// </remarks>

	public static ValueTask<TResult> AggregateRight<TSource, TAccumulate, TResult>(this IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TSource, TAccumulate, TAccumulate> func, Func<TAccumulate, TResult> resultSelector, CancellationToken cancellationToken = default)
	{
		Guard.IsNotNull(func);
		Guard.IsNotNull(resultSelector);

		return source.AggregateRight(seed, (a, b, ct) => new ValueTask<TAccumulate>(func(a, b)), (a, ct) => new ValueTask<TResult>(resultSelector(a)), cancellationToken);
	}

	/// <summary>
	/// Applies a right-associative accumulator function over a sequence.
	/// The specified seed value is used as the initial accumulator value,
	/// and the specified function is used to select the result value.
	/// This operator is the right-associative version of the
	/// <see cref="Enumerable.Aggregate{TSource, TAccumulate, TResult}(IEnumerable{TSource}, TAccumulate, Func{TAccumulate, TSource, TAccumulate}, Func{TAccumulate, TResult})"/> LINQ operator.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements of source.</typeparam>
	/// <typeparam name="TAccumulate">The type of the accumulator value.</typeparam>
	/// <typeparam name="TResult">The type of the resulting value.</typeparam>
	/// <param name="source">Source sequence.</param>
	/// <param name="seed">The initial accumulator value.</param>
	/// <param name="func">A right-associative accumulator function to be invoked on each element.</param>
	/// <param name="resultSelector">A function to transform the final accumulator value into the result value.</param>
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
	/// <returns>The transformed final accumulator value.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source" /> is <see langword="null" />.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="func" /> is <see langword="null" />.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="resultSelector" /> is <see langword="null" />.</exception>
	/// <example>
	/// <code><![CDATA[
	/// var numbers = Enumerable.Range(1, 5);
	/// int result = numbers.AggregateRight("6", (a, b) => string.Format("({0}/{1})", a, b), str => str.Length);
	/// ]]></code>
	/// The <c>result</c> variable will contain <c>21</c>.
	/// </example>
	/// <remarks>
	/// This operator executes immediately.
	/// </remarks>

	public static ValueTask<TResult> AggregateRight<TSource, TAccumulate, TResult>(this IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TSource, TAccumulate, ValueTask<TAccumulate>> func, Func<TAccumulate, ValueTask<TResult>> resultSelector, CancellationToken cancellationToken = default)
	{
		Guard.IsNotNull(func);
		Guard.IsNotNull(resultSelector);

		return source.AggregateRight(seed, (a, b, ct) => func(a, b), (a, ct) => resultSelector(a), cancellationToken);
	}

	/// <summary>
	/// Applies a right-associative accumulator function over a sequence.
	/// The specified seed value is used as the initial accumulator value,
	/// and the specified function is used to select the result value.
	/// This operator is the right-associative version of the
	/// <see cref="Enumerable.Aggregate{TSource, TAccumulate, TResult}(IEnumerable{TSource}, TAccumulate, Func{TAccumulate, TSource, TAccumulate}, Func{TAccumulate, TResult})"/> LINQ operator.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements of source.</typeparam>
	/// <typeparam name="TAccumulate">The type of the accumulator value.</typeparam>
	/// <typeparam name="TResult">The type of the resulting value.</typeparam>
	/// <param name="source">Source sequence.</param>
	/// <param name="seed">The initial accumulator value.</param>
	/// <param name="func">A right-associative accumulator function to be invoked on each element.</param>
	/// <param name="resultSelector">A function to transform the final accumulator value into the result value.</param>
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
	/// <returns>The transformed final accumulator value.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source" /> is <see langword="null" />.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="func" /> is <see langword="null" />.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="resultSelector" /> is <see langword="null" />.</exception>
	/// <example>
	/// <code><![CDATA[
	/// var numbers = Enumerable.Range(1, 5);
	/// int result = numbers.AggregateRight("6", (a, b) => string.Format("({0}/{1})", a, b), str => str.Length);
	/// ]]></code>
	/// The <c>result</c> variable will contain <c>21</c>.
	/// </example>
	/// <remarks>
	/// This operator executes immediately.
	/// </remarks>

	public static ValueTask<TResult> AggregateRight<TSource, TAccumulate, TResult>(this IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TSource, TAccumulate, CancellationToken, ValueTask<TAccumulate>> func, Func<TAccumulate, CancellationToken, ValueTask<TResult>> resultSelector, CancellationToken cancellationToken = default)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(func);
		Guard.IsNotNull(resultSelector);

		return Core(source, seed, func, resultSelector, cancellationToken);

		static async ValueTask<TResult> Core(
			IAsyncEnumerable<TSource> source,
			TAccumulate seed,
			Func<TSource, TAccumulate, CancellationToken, ValueTask<TAccumulate>> func,
			Func<TAccumulate, CancellationToken, ValueTask<TResult>> resultSelector,
			CancellationToken cancellationToken = default) =>
				await resultSelector(await source.AggregateRight(seed, func, cancellationToken).ConfigureAwait(false), cancellationToken).ConfigureAwait(false);
	}
}
