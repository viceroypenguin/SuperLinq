namespace SuperLinq;
#nullable enable
public static partial class SuperEnumerable
{
    /// <summary>
    /// <para>
    /// Applies a specified function to the corresponding elements of second sequences,
    /// producing a sequence of the results.</para>
    /// <para>
    /// The resulting sequence has the same length as the input sequences.
    /// If the input sequences are of different lengths, an exception is thrown.</para>
    /// </summary>
    /// <typeparam name = "TResult">
    /// The type of the elements of the result sequence.</typeparam>
    /// <param name = "resultSelector">A projection function that combines
    /// elements from all of the sequences.</param>
    /// <returns>A sequence of elements returned by <paramref name = "resultSelector"/>.</returns>
    /// <remarks>
    /// This method uses deferred execution and stream its results.
    /// </remarks>
    /// <exception cref = "global::System.ArgumentNullException"><paramref name = "resultSelector"/> or any of the input sequences is null.</exception>
    /// <exception cref = "global::System.InvalidOperationException">
    /// Any of the input sequences are shorter than the others.
    /// </exception>
    /// <typeparam name = "TFirst">The type of the elements of <paramref name = "first"/>.</typeparam>
    /// <param name = "first">The first sequence of elements.</param>
    /// <typeparam name = "TSecond">The type of the elements of <paramref name = "second"/>.</typeparam>
    /// <param name = "second">The second sequence of elements.</param>
    public static global::System.Collections.Generic.IEnumerable<TResult> EquiZip<TFirst, TSecond, TResult>(this global::System.Collections.Generic.IEnumerable<TFirst> first, global::System.Collections.Generic.IEnumerable<TSecond> second, global::System.Func<TFirst, TSecond, TResult> resultSelector)
    {
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(first);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(second);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(resultSelector);
        return _(first, second, resultSelector);
        static global::System.Collections.Generic.IEnumerable<TResult> _(global::System.Collections.Generic.IEnumerable<TFirst> first, global::System.Collections.Generic.IEnumerable<TSecond> second, global::System.Func<TFirst, TSecond, TResult> resultSelector)
        {
            using var e1 = first.GetEnumerator();
            using var e2 = second.GetEnumerator();
            while (true)
            {
                if (!e1.MoveNext())
                {
                    if (e2.MoveNext())
                    {
                        global::CommunityToolkit.Diagnostics.ThrowHelper.ThrowInvalidOperationException("First sequence too short.");
                    }

                    yield break;
                }

                if (!e2.MoveNext())
                    global::CommunityToolkit.Diagnostics.ThrowHelper.ThrowInvalidOperationException("Second sequence too short.");
                yield return resultSelector(e1.Current, e2.Current);
            }
        }
    }

    /// <summary>
    /// Joins the corresponding elements of second sequences,
    /// producing a sequence of tuples containing them.
    /// </summary>
    /// <returns>A sequence of 
    /// <see cref = "global::System.ValueTuple{T1, T2}"/> 
    /// containing corresponding elements from each of the sequences.</returns>
    /// <remarks>
    /// This method uses deferred execution and stream its results.
    /// </remarks>
    /// <exception cref = "global::System.ArgumentNullException">Any of the input sequences is null.</exception>
    /// <exception cref = "global::System.InvalidOperationException">
    /// Any of the input sequences are shorter than the others.
    /// </exception>
    /// <typeparam name = "TFirst">The type of the elements of <paramref name = "first"/>.</typeparam>
    /// <param name = "first">The first sequence of elements.</param>
    /// <typeparam name = "TSecond">The type of the elements of <paramref name = "second"/>.</typeparam>
    /// <param name = "second">The second sequence of elements.</param>
    public static global::System.Collections.Generic.IEnumerable<(TFirst, TSecond)> EquiZip<TFirst, TSecond>(this global::System.Collections.Generic.IEnumerable<TFirst> first, global::System.Collections.Generic.IEnumerable<TSecond> second) => EquiZip(first, second, global::System.ValueTuple.Create);
    /// <summary>
    /// <para>
    /// Applies a specified function to the corresponding elements of third sequences,
    /// producing a sequence of the results.</para>
    /// <para>
    /// The resulting sequence has the same length as the input sequences.
    /// If the input sequences are of different lengths, an exception is thrown.</para>
    /// </summary>
    /// <typeparam name = "TResult">
    /// The type of the elements of the result sequence.</typeparam>
    /// <param name = "resultSelector">A projection function that combines
    /// elements from all of the sequences.</param>
    /// <returns>A sequence of elements returned by <paramref name = "resultSelector"/>.</returns>
    /// <remarks>
    /// This method uses deferred execution and stream its results.
    /// </remarks>
    /// <exception cref = "global::System.ArgumentNullException"><paramref name = "resultSelector"/> or any of the input sequences is null.</exception>
    /// <exception cref = "global::System.InvalidOperationException">
    /// Any of the input sequences are shorter than the others.
    /// </exception>
    /// <typeparam name = "TFirst">The type of the elements of <paramref name = "first"/>.</typeparam>
    /// <param name = "first">The first sequence of elements.</param>
    /// <typeparam name = "TSecond">The type of the elements of <paramref name = "second"/>.</typeparam>
    /// <param name = "second">The second sequence of elements.</param>
    /// <typeparam name = "TThird">The type of the elements of <paramref name = "third"/>.</typeparam>
    /// <param name = "third">The third sequence of elements.</param>
    public static global::System.Collections.Generic.IEnumerable<TResult> EquiZip<TFirst, TSecond, TThird, TResult>(this global::System.Collections.Generic.IEnumerable<TFirst> first, global::System.Collections.Generic.IEnumerable<TSecond> second, global::System.Collections.Generic.IEnumerable<TThird> third, global::System.Func<TFirst, TSecond, TThird, TResult> resultSelector)
    {
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(first);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(second);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(third);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(resultSelector);
        return _(first, second, third, resultSelector);
        static global::System.Collections.Generic.IEnumerable<TResult> _(global::System.Collections.Generic.IEnumerable<TFirst> first, global::System.Collections.Generic.IEnumerable<TSecond> second, global::System.Collections.Generic.IEnumerable<TThird> third, global::System.Func<TFirst, TSecond, TThird, TResult> resultSelector)
        {
            using var e1 = first.GetEnumerator();
            using var e2 = second.GetEnumerator();
            using var e3 = third.GetEnumerator();
            while (true)
            {
                if (!e1.MoveNext())
                {
                    if (e2.MoveNext() || e3.MoveNext())
                    {
                        global::CommunityToolkit.Diagnostics.ThrowHelper.ThrowInvalidOperationException("First sequence too short.");
                    }

                    yield break;
                }

                if (!e2.MoveNext())
                    global::CommunityToolkit.Diagnostics.ThrowHelper.ThrowInvalidOperationException("Second sequence too short.");
                if (!e3.MoveNext())
                    global::CommunityToolkit.Diagnostics.ThrowHelper.ThrowInvalidOperationException("Third sequence too short.");
                yield return resultSelector(e1.Current, e2.Current, e3.Current);
            }
        }
    }

    /// <summary>
    /// Joins the corresponding elements of third sequences,
    /// producing a sequence of tuples containing them.
    /// </summary>
    /// <returns>A sequence of 
    /// <see cref = "global::System.ValueTuple{T1, T2, T3}"/> 
    /// containing corresponding elements from each of the sequences.</returns>
    /// <remarks>
    /// This method uses deferred execution and stream its results.
    /// </remarks>
    /// <exception cref = "global::System.ArgumentNullException">Any of the input sequences is null.</exception>
    /// <exception cref = "global::System.InvalidOperationException">
    /// Any of the input sequences are shorter than the others.
    /// </exception>
    /// <typeparam name = "TFirst">The type of the elements of <paramref name = "first"/>.</typeparam>
    /// <param name = "first">The first sequence of elements.</param>
    /// <typeparam name = "TSecond">The type of the elements of <paramref name = "second"/>.</typeparam>
    /// <param name = "second">The second sequence of elements.</param>
    /// <typeparam name = "TThird">The type of the elements of <paramref name = "third"/>.</typeparam>
    /// <param name = "third">The third sequence of elements.</param>
    public static global::System.Collections.Generic.IEnumerable<(TFirst, TSecond, TThird)> EquiZip<TFirst, TSecond, TThird>(this global::System.Collections.Generic.IEnumerable<TFirst> first, global::System.Collections.Generic.IEnumerable<TSecond> second, global::System.Collections.Generic.IEnumerable<TThird> third) => EquiZip(first, second, third, global::System.ValueTuple.Create);
    /// <summary>
    /// <para>
    /// Applies a specified function to the corresponding elements of fourth sequences,
    /// producing a sequence of the results.</para>
    /// <para>
    /// The resulting sequence has the same length as the input sequences.
    /// If the input sequences are of different lengths, an exception is thrown.</para>
    /// </summary>
    /// <typeparam name = "TResult">
    /// The type of the elements of the result sequence.</typeparam>
    /// <param name = "resultSelector">A projection function that combines
    /// elements from all of the sequences.</param>
    /// <returns>A sequence of elements returned by <paramref name = "resultSelector"/>.</returns>
    /// <remarks>
    /// This method uses deferred execution and stream its results.
    /// </remarks>
    /// <exception cref = "global::System.ArgumentNullException"><paramref name = "resultSelector"/> or any of the input sequences is null.</exception>
    /// <exception cref = "global::System.InvalidOperationException">
    /// Any of the input sequences are shorter than the others.
    /// </exception>
    /// <typeparam name = "TFirst">The type of the elements of <paramref name = "first"/>.</typeparam>
    /// <param name = "first">The first sequence of elements.</param>
    /// <typeparam name = "TSecond">The type of the elements of <paramref name = "second"/>.</typeparam>
    /// <param name = "second">The second sequence of elements.</param>
    /// <typeparam name = "TThird">The type of the elements of <paramref name = "third"/>.</typeparam>
    /// <param name = "third">The third sequence of elements.</param>
    /// <typeparam name = "TFourth">The type of the elements of <paramref name = "fourth"/>.</typeparam>
    /// <param name = "fourth">The fourth sequence of elements.</param>
    public static global::System.Collections.Generic.IEnumerable<TResult> EquiZip<TFirst, TSecond, TThird, TFourth, TResult>(this global::System.Collections.Generic.IEnumerable<TFirst> first, global::System.Collections.Generic.IEnumerable<TSecond> second, global::System.Collections.Generic.IEnumerable<TThird> third, global::System.Collections.Generic.IEnumerable<TFourth> fourth, global::System.Func<TFirst, TSecond, TThird, TFourth, TResult> resultSelector)
    {
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(first);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(second);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(third);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(fourth);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(resultSelector);
        return _(first, second, third, fourth, resultSelector);
        static global::System.Collections.Generic.IEnumerable<TResult> _(global::System.Collections.Generic.IEnumerable<TFirst> first, global::System.Collections.Generic.IEnumerable<TSecond> second, global::System.Collections.Generic.IEnumerable<TThird> third, global::System.Collections.Generic.IEnumerable<TFourth> fourth, global::System.Func<TFirst, TSecond, TThird, TFourth, TResult> resultSelector)
        {
            using var e1 = first.GetEnumerator();
            using var e2 = second.GetEnumerator();
            using var e3 = third.GetEnumerator();
            using var e4 = fourth.GetEnumerator();
            while (true)
            {
                if (!e1.MoveNext())
                {
                    if (e2.MoveNext() || e3.MoveNext() || e4.MoveNext())
                    {
                        global::CommunityToolkit.Diagnostics.ThrowHelper.ThrowInvalidOperationException("First sequence too short.");
                    }

                    yield break;
                }

                if (!e2.MoveNext())
                    global::CommunityToolkit.Diagnostics.ThrowHelper.ThrowInvalidOperationException("Second sequence too short.");
                if (!e3.MoveNext())
                    global::CommunityToolkit.Diagnostics.ThrowHelper.ThrowInvalidOperationException("Third sequence too short.");
                if (!e4.MoveNext())
                    global::CommunityToolkit.Diagnostics.ThrowHelper.ThrowInvalidOperationException("Fourth sequence too short.");
                yield return resultSelector(e1.Current, e2.Current, e3.Current, e4.Current);
            }
        }
    }

    /// <summary>
    /// Joins the corresponding elements of fourth sequences,
    /// producing a sequence of tuples containing them.
    /// </summary>
    /// <returns>A sequence of 
    /// <see cref = "global::System.ValueTuple{T1, T2, T3, T4}"/> 
    /// containing corresponding elements from each of the sequences.</returns>
    /// <remarks>
    /// This method uses deferred execution and stream its results.
    /// </remarks>
    /// <exception cref = "global::System.ArgumentNullException">Any of the input sequences is null.</exception>
    /// <exception cref = "global::System.InvalidOperationException">
    /// Any of the input sequences are shorter than the others.
    /// </exception>
    /// <typeparam name = "TFirst">The type of the elements of <paramref name = "first"/>.</typeparam>
    /// <param name = "first">The first sequence of elements.</param>
    /// <typeparam name = "TSecond">The type of the elements of <paramref name = "second"/>.</typeparam>
    /// <param name = "second">The second sequence of elements.</param>
    /// <typeparam name = "TThird">The type of the elements of <paramref name = "third"/>.</typeparam>
    /// <param name = "third">The third sequence of elements.</param>
    /// <typeparam name = "TFourth">The type of the elements of <paramref name = "fourth"/>.</typeparam>
    /// <param name = "fourth">The fourth sequence of elements.</param>
    public static global::System.Collections.Generic.IEnumerable<(TFirst, TSecond, TThird, TFourth)> EquiZip<TFirst, TSecond, TThird, TFourth>(this global::System.Collections.Generic.IEnumerable<TFirst> first, global::System.Collections.Generic.IEnumerable<TSecond> second, global::System.Collections.Generic.IEnumerable<TThird> third, global::System.Collections.Generic.IEnumerable<TFourth> fourth) => EquiZip(first, second, third, fourth, global::System.ValueTuple.Create);
}