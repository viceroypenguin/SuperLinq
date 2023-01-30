namespace SuperLinq;
#nullable enable
public static partial class SuperEnumerable
{
    private static bool DoRead<T>(bool flag, IEnumerator<T> iter, out T? value)
    {
        if (!flag || !iter.MoveNext())
        {
            value = default;
            return false;
        }

        value = iter.Current;
        return true;
    }

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
    public static global::System.Collections.Generic.IEnumerable<TResult> ZipLongest<T1, T2, TResult>(this global::System.Collections.Generic.IEnumerable<T1> first, global::System.Collections.Generic.IEnumerable<T2> second, global::System.Func<T1?, T2?, TResult> resultSelector)
    {
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(first);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(second);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(resultSelector);
        return _(first, second, resultSelector);
        static global::System.Collections.Generic.IEnumerable<TResult> _(global::System.Collections.Generic.IEnumerable<T1> first, global::System.Collections.Generic.IEnumerable<T2> second, global::System.Func<T1?, T2?, TResult> resultSelector)
        {
            using var e1 = first.GetEnumerator();
            var f1 = true;
            using var e2 = second.GetEnumerator();
            var f2 = true;
            while ((f1 = DoRead(f1, e1, out var v1)) | (f2 = DoRead(f2, e2, out var v2)))
            {
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
    public static global::System.Collections.Generic.IEnumerable<(T1? , T2? )> ZipLongest<T1, T2>(this global::System.Collections.Generic.IEnumerable<T1> first, global::System.Collections.Generic.IEnumerable<T2> second) => ZipLongest(first, second, global::System.ValueTuple.Create);
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
    public static global::System.Collections.Generic.IEnumerable<TResult> ZipLongest<T1, T2, T3, TResult>(this global::System.Collections.Generic.IEnumerable<T1> first, global::System.Collections.Generic.IEnumerable<T2> second, global::System.Collections.Generic.IEnumerable<T3> third, global::System.Func<T1?, T2?, T3?, TResult> resultSelector)
    {
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(first);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(second);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(third);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(resultSelector);
        return _(first, second, third, resultSelector);
        static global::System.Collections.Generic.IEnumerable<TResult> _(global::System.Collections.Generic.IEnumerable<T1> first, global::System.Collections.Generic.IEnumerable<T2> second, global::System.Collections.Generic.IEnumerable<T3> third, global::System.Func<T1?, T2?, T3?, TResult> resultSelector)
        {
            using var e1 = first.GetEnumerator();
            var f1 = true;
            using var e2 = second.GetEnumerator();
            var f2 = true;
            using var e3 = third.GetEnumerator();
            var f3 = true;
            while ((f1 = DoRead(f1, e1, out var v1)) | (f2 = DoRead(f2, e2, out var v2)) | (f3 = DoRead(f3, e3, out var v3)))
            {
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
    public static global::System.Collections.Generic.IEnumerable<(T1? , T2? , T3? )> ZipLongest<T1, T2, T3>(this global::System.Collections.Generic.IEnumerable<T1> first, global::System.Collections.Generic.IEnumerable<T2> second, global::System.Collections.Generic.IEnumerable<T3> third) => ZipLongest(first, second, third, global::System.ValueTuple.Create);
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
    public static global::System.Collections.Generic.IEnumerable<TResult> ZipLongest<T1, T2, T3, T4, TResult>(this global::System.Collections.Generic.IEnumerable<T1> first, global::System.Collections.Generic.IEnumerable<T2> second, global::System.Collections.Generic.IEnumerable<T3> third, global::System.Collections.Generic.IEnumerable<T4> fourth, global::System.Func<T1?, T2?, T3?, T4?, TResult> resultSelector)
    {
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(first);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(second);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(third);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(fourth);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(resultSelector);
        return _(first, second, third, fourth, resultSelector);
        static global::System.Collections.Generic.IEnumerable<TResult> _(global::System.Collections.Generic.IEnumerable<T1> first, global::System.Collections.Generic.IEnumerable<T2> second, global::System.Collections.Generic.IEnumerable<T3> third, global::System.Collections.Generic.IEnumerable<T4> fourth, global::System.Func<T1?, T2?, T3?, T4?, TResult> resultSelector)
        {
            using var e1 = first.GetEnumerator();
            var f1 = true;
            using var e2 = second.GetEnumerator();
            var f2 = true;
            using var e3 = third.GetEnumerator();
            var f3 = true;
            using var e4 = fourth.GetEnumerator();
            var f4 = true;
            while ((f1 = DoRead(f1, e1, out var v1)) | (f2 = DoRead(f2, e2, out var v2)) | (f3 = DoRead(f3, e3, out var v3)) | (f4 = DoRead(f4, e4, out var v4)))
            {
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
    public static global::System.Collections.Generic.IEnumerable<(T1? , T2? , T3? , T4? )> ZipLongest<T1, T2, T3, T4>(this global::System.Collections.Generic.IEnumerable<T1> first, global::System.Collections.Generic.IEnumerable<T2> second, global::System.Collections.Generic.IEnumerable<T3> third, global::System.Collections.Generic.IEnumerable<T4> fourth) => ZipLongest(first, second, third, fourth, global::System.ValueTuple.Create);
}