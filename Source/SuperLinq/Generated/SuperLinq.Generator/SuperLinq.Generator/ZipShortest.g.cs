namespace SuperLinq;
#nullable enable
public static partial class SuperEnumerable
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
    public static global::System.Collections.Generic.IEnumerable<TResult> ZipShortest<TFirst, TSecond, TResult>(this global::System.Collections.Generic.IEnumerable<TFirst> first, global::System.Collections.Generic.IEnumerable<TSecond> second, global::System.Func<TFirst, TSecond, TResult> resultSelector)
    {
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(first);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(second);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(resultSelector);
        return Core(first, second, resultSelector);
        static global::System.Collections.Generic.IEnumerable<TResult> Core(global::System.Collections.Generic.IEnumerable<TFirst> first, global::System.Collections.Generic.IEnumerable<TSecond> second, global::System.Func<TFirst, TSecond, TResult> resultSelector)
        {
            using var e1 = first.GetEnumerator();
            using var e2 = second.GetEnumerator();
            while (e1.MoveNext() && e2.MoveNext())
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
    public static global::System.Collections.Generic.IEnumerable<(TFirst, TSecond)> ZipShortest<TFirst, TSecond>(this global::System.Collections.Generic.IEnumerable<TFirst> first, global::System.Collections.Generic.IEnumerable<TSecond> second) => ZipShortest(first, second, global::System.ValueTuple.Create);
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
    public static global::System.Collections.Generic.IEnumerable<TResult> ZipShortest<TFirst, TSecond, TThird, TResult>(this global::System.Collections.Generic.IEnumerable<TFirst> first, global::System.Collections.Generic.IEnumerable<TSecond> second, global::System.Collections.Generic.IEnumerable<TThird> third, global::System.Func<TFirst, TSecond, TThird, TResult> resultSelector)
    {
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(first);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(second);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(third);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(resultSelector);
        return Core(first, second, third, resultSelector);
        static global::System.Collections.Generic.IEnumerable<TResult> Core(global::System.Collections.Generic.IEnumerable<TFirst> first, global::System.Collections.Generic.IEnumerable<TSecond> second, global::System.Collections.Generic.IEnumerable<TThird> third, global::System.Func<TFirst, TSecond, TThird, TResult> resultSelector)
        {
            using var e1 = first.GetEnumerator();
            using var e2 = second.GetEnumerator();
            using var e3 = third.GetEnumerator();
            while (e1.MoveNext() && e2.MoveNext() && e3.MoveNext())
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
    public static global::System.Collections.Generic.IEnumerable<(TFirst, TSecond, TThird)> ZipShortest<TFirst, TSecond, TThird>(this global::System.Collections.Generic.IEnumerable<TFirst> first, global::System.Collections.Generic.IEnumerable<TSecond> second, global::System.Collections.Generic.IEnumerable<TThird> third) => ZipShortest(first, second, third, global::System.ValueTuple.Create);
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
    public static global::System.Collections.Generic.IEnumerable<TResult> ZipShortest<TFirst, TSecond, TThird, TFourth, TResult>(this global::System.Collections.Generic.IEnumerable<TFirst> first, global::System.Collections.Generic.IEnumerable<TSecond> second, global::System.Collections.Generic.IEnumerable<TThird> third, global::System.Collections.Generic.IEnumerable<TFourth> fourth, global::System.Func<TFirst, TSecond, TThird, TFourth, TResult> resultSelector)
    {
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(first);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(second);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(third);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(fourth);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(resultSelector);
        return Core(first, second, third, fourth, resultSelector);
        static global::System.Collections.Generic.IEnumerable<TResult> Core(global::System.Collections.Generic.IEnumerable<TFirst> first, global::System.Collections.Generic.IEnumerable<TSecond> second, global::System.Collections.Generic.IEnumerable<TThird> third, global::System.Collections.Generic.IEnumerable<TFourth> fourth, global::System.Func<TFirst, TSecond, TThird, TFourth, TResult> resultSelector)
        {
            using var e1 = first.GetEnumerator();
            using var e2 = second.GetEnumerator();
            using var e3 = third.GetEnumerator();
            using var e4 = fourth.GetEnumerator();
            while (e1.MoveNext() && e2.MoveNext() && e3.MoveNext() && e4.MoveNext())
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
    public static global::System.Collections.Generic.IEnumerable<(TFirst, TSecond, TThird, TFourth)> ZipShortest<TFirst, TSecond, TThird, TFourth>(this global::System.Collections.Generic.IEnumerable<TFirst> first, global::System.Collections.Generic.IEnumerable<TSecond> second, global::System.Collections.Generic.IEnumerable<TThird> third, global::System.Collections.Generic.IEnumerable<TFourth> fourth) => ZipShortest(first, second, third, fourth, global::System.ValueTuple.Create);
}