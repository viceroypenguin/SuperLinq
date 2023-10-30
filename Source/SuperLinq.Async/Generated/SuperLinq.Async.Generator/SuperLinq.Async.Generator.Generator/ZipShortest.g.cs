namespace SuperLinq.Async;
#nullable enable
public static partial class AsyncSuperEnumerable
{
    /// <summary>
    /// Returns a projection of tuples, where each tuple contains the N-th
    /// element from each of the argument sequences. The resulting sequence
    /// is as short as the shortest input sequence.
    /// </summary>
    /// <typeparam name = "TResult">
    /// The type of the elements of the result sequence.</typeparam>
    /// <param name = "resultSelector">A projection function that combines
    /// elements from all of the sequences.</param>
    /// <returns>A sequence of elements returned by <paramref name = "resultSelector"/>.</returns>
    /// <remarks>
    /// This method uses deferred execution and streams its results.
    /// </remarks>
    /// <exception cref = "global::System.ArgumentNullException"><paramref name = "resultSelector"/> or any of the input sequences is null.</exception>
    /// <typeparam name = "TFirst">The type of the elements of <paramref name = "first"/>.</typeparam>
    /// <param name = "first">The first sequence of elements.</param>
    /// <typeparam name = "TSecond">The type of the elements of <paramref name = "second"/>.</typeparam>
    /// <param name = "second">The second sequence of elements.</param>
    public static global::System.Collections.Generic.IAsyncEnumerable<TResult> ZipShortest<TFirst, TSecond, TResult>(this global::System.Collections.Generic.IAsyncEnumerable<TFirst> first, global::System.Collections.Generic.IAsyncEnumerable<TSecond> second, global::System.Func<TFirst, TSecond, TResult> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(second);
        ArgumentNullException.ThrowIfNull(resultSelector);
        return Core(first, second, resultSelector);
        static async global::System.Collections.Generic.IAsyncEnumerable<TResult> Core(global::System.Collections.Generic.IAsyncEnumerable<TFirst> first, global::System.Collections.Generic.IAsyncEnumerable<TSecond> second, global::System.Func<TFirst, TSecond, TResult> resultSelector, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await using var e1 = first.ConfigureAwait(false).WithCancellation(cancellationToken).GetAsyncEnumerator();
            await using var e2 = second.ConfigureAwait(false).WithCancellation(cancellationToken).GetAsyncEnumerator();
            while (await e1.MoveNextAsync() && await e2.MoveNextAsync())
            {
                yield return resultSelector(e1.Current, e2.Current);
            }
        }
    }

    /// <summary>
    /// Returns a projection of tuples, where each tuple contains the N-th
    /// element from each of the argument sequences. The resulting sequence
    /// will always be as long as the longest of input sequences where the
    /// default value of each of the shorter sequence element types is used
    /// for padding.
    /// </summary>
    /// <returns>A sequence of 
    /// <see cref = "global::System.ValueTuple{T1, T2}"/> 
    /// containing corresponding elements from each of the sequences.</returns>
    /// <remarks>
    /// This method uses deferred execution and streams its results.
    /// </remarks>
    /// <exception cref = "global::System.ArgumentNullException">Any of the input sequences is null.</exception>
    /// <typeparam name = "TFirst">The type of the elements of <paramref name = "first"/>.</typeparam>
    /// <param name = "first">The first sequence of elements.</param>
    /// <typeparam name = "TSecond">The type of the elements of <paramref name = "second"/>.</typeparam>
    /// <param name = "second">The second sequence of elements.</param>
    public static global::System.Collections.Generic.IAsyncEnumerable<(TFirst, TSecond)> ZipShortest<TFirst, TSecond>(this global::System.Collections.Generic.IAsyncEnumerable<TFirst> first, global::System.Collections.Generic.IAsyncEnumerable<TSecond> second) => ZipShortest(first, second, global::System.ValueTuple.Create);
    /// <summary>
    /// Returns a projection of tuples, where each tuple contains the N-th
    /// element from each of the argument sequences. The resulting sequence
    /// is as short as the shortest input sequence.
    /// </summary>
    /// <typeparam name = "TResult">
    /// The type of the elements of the result sequence.</typeparam>
    /// <param name = "resultSelector">A projection function that combines
    /// elements from all of the sequences.</param>
    /// <returns>A sequence of elements returned by <paramref name = "resultSelector"/>.</returns>
    /// <remarks>
    /// This method uses deferred execution and streams its results.
    /// </remarks>
    /// <exception cref = "global::System.ArgumentNullException"><paramref name = "resultSelector"/> or any of the input sequences is null.</exception>
    /// <typeparam name = "TFirst">The type of the elements of <paramref name = "first"/>.</typeparam>
    /// <param name = "first">The first sequence of elements.</param>
    /// <typeparam name = "TSecond">The type of the elements of <paramref name = "second"/>.</typeparam>
    /// <param name = "second">The second sequence of elements.</param>
    /// <typeparam name = "TThird">The type of the elements of <paramref name = "third"/>.</typeparam>
    /// <param name = "third">The third sequence of elements.</param>
    public static global::System.Collections.Generic.IAsyncEnumerable<TResult> ZipShortest<TFirst, TSecond, TThird, TResult>(this global::System.Collections.Generic.IAsyncEnumerable<TFirst> first, global::System.Collections.Generic.IAsyncEnumerable<TSecond> second, global::System.Collections.Generic.IAsyncEnumerable<TThird> third, global::System.Func<TFirst, TSecond, TThird, TResult> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(second);
        ArgumentNullException.ThrowIfNull(third);
        ArgumentNullException.ThrowIfNull(resultSelector);
        return Core(first, second, third, resultSelector);
        static async global::System.Collections.Generic.IAsyncEnumerable<TResult> Core(global::System.Collections.Generic.IAsyncEnumerable<TFirst> first, global::System.Collections.Generic.IAsyncEnumerable<TSecond> second, global::System.Collections.Generic.IAsyncEnumerable<TThird> third, global::System.Func<TFirst, TSecond, TThird, TResult> resultSelector, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await using var e1 = first.ConfigureAwait(false).WithCancellation(cancellationToken).GetAsyncEnumerator();
            await using var e2 = second.ConfigureAwait(false).WithCancellation(cancellationToken).GetAsyncEnumerator();
            await using var e3 = third.ConfigureAwait(false).WithCancellation(cancellationToken).GetAsyncEnumerator();
            while (await e1.MoveNextAsync() && await e2.MoveNextAsync() && await e3.MoveNextAsync())
            {
                yield return resultSelector(e1.Current, e2.Current, e3.Current);
            }
        }
    }

    /// <summary>
    /// Returns a projection of tuples, where each tuple contains the N-th
    /// element from each of the argument sequences. The resulting sequence
    /// will always be as long as the longest of input sequences where the
    /// default value of each of the shorter sequence element types is used
    /// for padding.
    /// </summary>
    /// <returns>A sequence of 
    /// <see cref = "global::System.ValueTuple{T1, T2, T3}"/> 
    /// containing corresponding elements from each of the sequences.</returns>
    /// <remarks>
    /// This method uses deferred execution and streams its results.
    /// </remarks>
    /// <exception cref = "global::System.ArgumentNullException">Any of the input sequences is null.</exception>
    /// <typeparam name = "TFirst">The type of the elements of <paramref name = "first"/>.</typeparam>
    /// <param name = "first">The first sequence of elements.</param>
    /// <typeparam name = "TSecond">The type of the elements of <paramref name = "second"/>.</typeparam>
    /// <param name = "second">The second sequence of elements.</param>
    /// <typeparam name = "TThird">The type of the elements of <paramref name = "third"/>.</typeparam>
    /// <param name = "third">The third sequence of elements.</param>
    public static global::System.Collections.Generic.IAsyncEnumerable<(TFirst, TSecond, TThird)> ZipShortest<TFirst, TSecond, TThird>(this global::System.Collections.Generic.IAsyncEnumerable<TFirst> first, global::System.Collections.Generic.IAsyncEnumerable<TSecond> second, global::System.Collections.Generic.IAsyncEnumerable<TThird> third) => ZipShortest(first, second, third, global::System.ValueTuple.Create);
    /// <summary>
    /// Returns a projection of tuples, where each tuple contains the N-th
    /// element from each of the argument sequences. The resulting sequence
    /// is as short as the shortest input sequence.
    /// </summary>
    /// <typeparam name = "TResult">
    /// The type of the elements of the result sequence.</typeparam>
    /// <param name = "resultSelector">A projection function that combines
    /// elements from all of the sequences.</param>
    /// <returns>A sequence of elements returned by <paramref name = "resultSelector"/>.</returns>
    /// <remarks>
    /// This method uses deferred execution and streams its results.
    /// </remarks>
    /// <exception cref = "global::System.ArgumentNullException"><paramref name = "resultSelector"/> or any of the input sequences is null.</exception>
    /// <typeparam name = "TFirst">The type of the elements of <paramref name = "first"/>.</typeparam>
    /// <param name = "first">The first sequence of elements.</param>
    /// <typeparam name = "TSecond">The type of the elements of <paramref name = "second"/>.</typeparam>
    /// <param name = "second">The second sequence of elements.</param>
    /// <typeparam name = "TThird">The type of the elements of <paramref name = "third"/>.</typeparam>
    /// <param name = "third">The third sequence of elements.</param>
    /// <typeparam name = "TFourth">The type of the elements of <paramref name = "fourth"/>.</typeparam>
    /// <param name = "fourth">The fourth sequence of elements.</param>
    public static global::System.Collections.Generic.IAsyncEnumerable<TResult> ZipShortest<TFirst, TSecond, TThird, TFourth, TResult>(this global::System.Collections.Generic.IAsyncEnumerable<TFirst> first, global::System.Collections.Generic.IAsyncEnumerable<TSecond> second, global::System.Collections.Generic.IAsyncEnumerable<TThird> third, global::System.Collections.Generic.IAsyncEnumerable<TFourth> fourth, global::System.Func<TFirst, TSecond, TThird, TFourth, TResult> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(second);
        ArgumentNullException.ThrowIfNull(third);
        ArgumentNullException.ThrowIfNull(fourth);
        ArgumentNullException.ThrowIfNull(resultSelector);
        return Core(first, second, third, fourth, resultSelector);
        static async global::System.Collections.Generic.IAsyncEnumerable<TResult> Core(global::System.Collections.Generic.IAsyncEnumerable<TFirst> first, global::System.Collections.Generic.IAsyncEnumerable<TSecond> second, global::System.Collections.Generic.IAsyncEnumerable<TThird> third, global::System.Collections.Generic.IAsyncEnumerable<TFourth> fourth, global::System.Func<TFirst, TSecond, TThird, TFourth, TResult> resultSelector, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await using var e1 = first.ConfigureAwait(false).WithCancellation(cancellationToken).GetAsyncEnumerator();
            await using var e2 = second.ConfigureAwait(false).WithCancellation(cancellationToken).GetAsyncEnumerator();
            await using var e3 = third.ConfigureAwait(false).WithCancellation(cancellationToken).GetAsyncEnumerator();
            await using var e4 = fourth.ConfigureAwait(false).WithCancellation(cancellationToken).GetAsyncEnumerator();
            while (await e1.MoveNextAsync() && await e2.MoveNextAsync() && await e3.MoveNextAsync() && await e4.MoveNextAsync())
            {
                yield return resultSelector(e1.Current, e2.Current, e3.Current, e4.Current);
            }
        }
    }

    /// <summary>
    /// Returns a projection of tuples, where each tuple contains the N-th
    /// element from each of the argument sequences. The resulting sequence
    /// will always be as long as the longest of input sequences where the
    /// default value of each of the shorter sequence element types is used
    /// for padding.
    /// </summary>
    /// <returns>A sequence of 
    /// <see cref = "global::System.ValueTuple{T1, T2, T3, T4}"/> 
    /// containing corresponding elements from each of the sequences.</returns>
    /// <remarks>
    /// This method uses deferred execution and streams its results.
    /// </remarks>
    /// <exception cref = "global::System.ArgumentNullException">Any of the input sequences is null.</exception>
    /// <typeparam name = "TFirst">The type of the elements of <paramref name = "first"/>.</typeparam>
    /// <param name = "first">The first sequence of elements.</param>
    /// <typeparam name = "TSecond">The type of the elements of <paramref name = "second"/>.</typeparam>
    /// <param name = "second">The second sequence of elements.</param>
    /// <typeparam name = "TThird">The type of the elements of <paramref name = "third"/>.</typeparam>
    /// <param name = "third">The third sequence of elements.</param>
    /// <typeparam name = "TFourth">The type of the elements of <paramref name = "fourth"/>.</typeparam>
    /// <param name = "fourth">The fourth sequence of elements.</param>
    public static global::System.Collections.Generic.IAsyncEnumerable<(TFirst, TSecond, TThird, TFourth)> ZipShortest<TFirst, TSecond, TThird, TFourth>(this global::System.Collections.Generic.IAsyncEnumerable<TFirst> first, global::System.Collections.Generic.IAsyncEnumerable<TSecond> second, global::System.Collections.Generic.IAsyncEnumerable<TThird> third, global::System.Collections.Generic.IAsyncEnumerable<TFourth> fourth) => ZipShortest(first, second, third, fourth, global::System.ValueTuple.Create);
}