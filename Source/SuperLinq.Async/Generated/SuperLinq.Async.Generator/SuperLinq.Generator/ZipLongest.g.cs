namespace SuperLinq.Async;
#nullable enable
public static partial class AsyncSuperEnumerable
{
    /// <summary>
    /// Returns a projection of tuples, where each tuple contains the N-th
    /// element from each of the argument sequences. The resulting sequence
    /// will always be as long as the longest of input sequences where the
    /// default value of each of the shorter sequence element types is used
    /// for padding.
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
    /// <typeparam name = "T1">The type of the elements of <paramref name = "first"/>.</typeparam>
    /// <param name = "first">The first sequence of elements.</param>
    /// <typeparam name = "T2">The type of the elements of <paramref name = "second"/>.</typeparam>
    /// <param name = "second">The second sequence of elements.</param>
    public static global::System.Collections.Generic.IAsyncEnumerable<TResult> ZipLongest<T1, T2, TResult>(this global::System.Collections.Generic.IAsyncEnumerable<T1> first, global::System.Collections.Generic.IAsyncEnumerable<T2> second, global::System.Func<T1?, T2?, TResult> resultSelector)
    {
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(first);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(second);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(resultSelector);
        return _(first, second, resultSelector);
        static async global::System.Collections.Generic.IAsyncEnumerable<TResult> _(global::System.Collections.Generic.IAsyncEnumerable<T1> first, global::System.Collections.Generic.IAsyncEnumerable<T2> second, global::System.Func<T1?, T2?, TResult> resultSelector, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await using var e1 = first.ConfigureAwait(false).WithCancellation(cancellationToken).GetAsyncEnumerator();
            var f1 = true;
            await using var e2 = second.ConfigureAwait(false).WithCancellation(cancellationToken).GetAsyncEnumerator();
            var f2 = true;
            while (true)
            {
                var v1 = (f1 && (f1 = await e1.MoveNextAsync())) ? e1.Current : default(T1);
                var v2 = (f2 && (f2 = await e2.MoveNextAsync())) ? e2.Current : default(T2);
                if (!f1 && !f2)
                    yield break;
                yield return resultSelector(v1, v2);
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
    /// <typeparam name = "T1">The type of the elements of <paramref name = "first"/>.</typeparam>
    /// <param name = "first">The first sequence of elements.</param>
    /// <typeparam name = "T2">The type of the elements of <paramref name = "second"/>.</typeparam>
    /// <param name = "second">The second sequence of elements.</param>
    public static global::System.Collections.Generic.IAsyncEnumerable<(T1? , T2? )> ZipLongest<T1, T2>(this global::System.Collections.Generic.IAsyncEnumerable<T1> first, global::System.Collections.Generic.IAsyncEnumerable<T2> second) => ZipLongest(first, second, global::System.ValueTuple.Create);
    /// <summary>
    /// Returns a projection of tuples, where each tuple contains the N-th
    /// element from each of the argument sequences. The resulting sequence
    /// will always be as long as the longest of input sequences where the
    /// default value of each of the shorter sequence element types is used
    /// for padding.
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
    /// <typeparam name = "T1">The type of the elements of <paramref name = "first"/>.</typeparam>
    /// <param name = "first">The first sequence of elements.</param>
    /// <typeparam name = "T2">The type of the elements of <paramref name = "second"/>.</typeparam>
    /// <param name = "second">The second sequence of elements.</param>
    /// <typeparam name = "T3">The type of the elements of <paramref name = "third"/>.</typeparam>
    /// <param name = "third">The third sequence of elements.</param>
    public static global::System.Collections.Generic.IAsyncEnumerable<TResult> ZipLongest<T1, T2, T3, TResult>(this global::System.Collections.Generic.IAsyncEnumerable<T1> first, global::System.Collections.Generic.IAsyncEnumerable<T2> second, global::System.Collections.Generic.IAsyncEnumerable<T3> third, global::System.Func<T1?, T2?, T3?, TResult> resultSelector)
    {
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(first);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(second);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(third);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(resultSelector);
        return _(first, second, third, resultSelector);
        static async global::System.Collections.Generic.IAsyncEnumerable<TResult> _(global::System.Collections.Generic.IAsyncEnumerable<T1> first, global::System.Collections.Generic.IAsyncEnumerable<T2> second, global::System.Collections.Generic.IAsyncEnumerable<T3> third, global::System.Func<T1?, T2?, T3?, TResult> resultSelector, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await using var e1 = first.ConfigureAwait(false).WithCancellation(cancellationToken).GetAsyncEnumerator();
            var f1 = true;
            await using var e2 = second.ConfigureAwait(false).WithCancellation(cancellationToken).GetAsyncEnumerator();
            var f2 = true;
            await using var e3 = third.ConfigureAwait(false).WithCancellation(cancellationToken).GetAsyncEnumerator();
            var f3 = true;
            while (true)
            {
                var v1 = (f1 && (f1 = await e1.MoveNextAsync())) ? e1.Current : default(T1);
                var v2 = (f2 && (f2 = await e2.MoveNextAsync())) ? e2.Current : default(T2);
                var v3 = (f3 && (f3 = await e3.MoveNextAsync())) ? e3.Current : default(T3);
                if (!f1 && !f2 && !f3)
                    yield break;
                yield return resultSelector(v1, v2, v3);
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
    /// <typeparam name = "T1">The type of the elements of <paramref name = "first"/>.</typeparam>
    /// <param name = "first">The first sequence of elements.</param>
    /// <typeparam name = "T2">The type of the elements of <paramref name = "second"/>.</typeparam>
    /// <param name = "second">The second sequence of elements.</param>
    /// <typeparam name = "T3">The type of the elements of <paramref name = "third"/>.</typeparam>
    /// <param name = "third">The third sequence of elements.</param>
    public static global::System.Collections.Generic.IAsyncEnumerable<(T1? , T2? , T3? )> ZipLongest<T1, T2, T3>(this global::System.Collections.Generic.IAsyncEnumerable<T1> first, global::System.Collections.Generic.IAsyncEnumerable<T2> second, global::System.Collections.Generic.IAsyncEnumerable<T3> third) => ZipLongest(first, second, third, global::System.ValueTuple.Create);
    /// <summary>
    /// Returns a projection of tuples, where each tuple contains the N-th
    /// element from each of the argument sequences. The resulting sequence
    /// will always be as long as the longest of input sequences where the
    /// default value of each of the shorter sequence element types is used
    /// for padding.
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
    /// <typeparam name = "T1">The type of the elements of <paramref name = "first"/>.</typeparam>
    /// <param name = "first">The first sequence of elements.</param>
    /// <typeparam name = "T2">The type of the elements of <paramref name = "second"/>.</typeparam>
    /// <param name = "second">The second sequence of elements.</param>
    /// <typeparam name = "T3">The type of the elements of <paramref name = "third"/>.</typeparam>
    /// <param name = "third">The third sequence of elements.</param>
    /// <typeparam name = "T4">The type of the elements of <paramref name = "fourth"/>.</typeparam>
    /// <param name = "fourth">The fourth sequence of elements.</param>
    public static global::System.Collections.Generic.IAsyncEnumerable<TResult> ZipLongest<T1, T2, T3, T4, TResult>(this global::System.Collections.Generic.IAsyncEnumerable<T1> first, global::System.Collections.Generic.IAsyncEnumerable<T2> second, global::System.Collections.Generic.IAsyncEnumerable<T3> third, global::System.Collections.Generic.IAsyncEnumerable<T4> fourth, global::System.Func<T1?, T2?, T3?, T4?, TResult> resultSelector)
    {
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(first);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(second);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(third);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(fourth);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(resultSelector);
        return _(first, second, third, fourth, resultSelector);
        static async global::System.Collections.Generic.IAsyncEnumerable<TResult> _(global::System.Collections.Generic.IAsyncEnumerable<T1> first, global::System.Collections.Generic.IAsyncEnumerable<T2> second, global::System.Collections.Generic.IAsyncEnumerable<T3> third, global::System.Collections.Generic.IAsyncEnumerable<T4> fourth, global::System.Func<T1?, T2?, T3?, T4?, TResult> resultSelector, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await using var e1 = first.ConfigureAwait(false).WithCancellation(cancellationToken).GetAsyncEnumerator();
            var f1 = true;
            await using var e2 = second.ConfigureAwait(false).WithCancellation(cancellationToken).GetAsyncEnumerator();
            var f2 = true;
            await using var e3 = third.ConfigureAwait(false).WithCancellation(cancellationToken).GetAsyncEnumerator();
            var f3 = true;
            await using var e4 = fourth.ConfigureAwait(false).WithCancellation(cancellationToken).GetAsyncEnumerator();
            var f4 = true;
            while (true)
            {
                var v1 = (f1 && (f1 = await e1.MoveNextAsync())) ? e1.Current : default(T1);
                var v2 = (f2 && (f2 = await e2.MoveNextAsync())) ? e2.Current : default(T2);
                var v3 = (f3 && (f3 = await e3.MoveNextAsync())) ? e3.Current : default(T3);
                var v4 = (f4 && (f4 = await e4.MoveNextAsync())) ? e4.Current : default(T4);
                if (!f1 && !f2 && !f3 && !f4)
                    yield break;
                yield return resultSelector(v1, v2, v3, v4);
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
    /// <typeparam name = "T1">The type of the elements of <paramref name = "first"/>.</typeparam>
    /// <param name = "first">The first sequence of elements.</param>
    /// <typeparam name = "T2">The type of the elements of <paramref name = "second"/>.</typeparam>
    /// <param name = "second">The second sequence of elements.</param>
    /// <typeparam name = "T3">The type of the elements of <paramref name = "third"/>.</typeparam>
    /// <param name = "third">The third sequence of elements.</param>
    /// <typeparam name = "T4">The type of the elements of <paramref name = "fourth"/>.</typeparam>
    /// <param name = "fourth">The fourth sequence of elements.</param>
    public static global::System.Collections.Generic.IAsyncEnumerable<(T1? , T2? , T3? , T4? )> ZipLongest<T1, T2, T3, T4>(this global::System.Collections.Generic.IAsyncEnumerable<T1> first, global::System.Collections.Generic.IAsyncEnumerable<T2> second, global::System.Collections.Generic.IAsyncEnumerable<T3> third, global::System.Collections.Generic.IAsyncEnumerable<T4> fourth) => ZipLongest(first, second, third, fourth, global::System.ValueTuple.Create);
}