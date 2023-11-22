namespace SuperLinq;
#nullable enable
public static partial class SuperEnumerable
{
    /// <summary>
    ///	    Returns the Cartesian product of two sequences by enumerating all possible combinations of one item from
    ///	    each sequence, and applying a user-defined projection to the items in a given combination.
    /// </summary>
    /// <typeparam name = "TResult">
    ///		The type of the elements of the result sequence.
    /// </typeparam>
    /// <param name = "resultSelector">
    ///		A projection function that combines elements from all of the sequences.
    /// </param>
    /// <returns>
    ///		A sequence of elements returned by <paramref name = "resultSelector"/>.
    /// </returns>
    /// <remarks>
    /// <para>
    ///	    The method returns items in the same order as a nested foreach loop, but all sequences cached when iterated
    ///	    over. The cache is then re-used for any subsequent iterations.
    /// </para>
    /// <para>
    ///		This method uses deferred execution and stream its results.
    /// </para>
    /// </remarks>
    /// <exception cref = "global::System.ArgumentNullException">
    ///		<paramref name = "resultSelector"/> or any of the input sequences is <see langword="null"/>.
    /// </exception>
    /// <typeparam name = "T1">
    ///		The type of the elements of <paramref name = "first"/>.
    /// </typeparam>
    /// <param name = "first">
    ///		The first sequence of elements.
    /// </param>
    /// <typeparam name = "T2">
    ///		The type of the elements of <paramref name = "second"/>.
    /// </typeparam>
    /// <param name = "second">
    ///		The second sequence of elements.
    /// </param>
    public static global::System.Collections.Generic.IEnumerable<TResult> Cartesian<T1, T2, TResult>(this global::System.Collections.Generic.IEnumerable<T1> first, global::System.Collections.Generic.IEnumerable<T2> second, global::System.Func<T1, T2, TResult> resultSelector)
    {
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(first);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(second);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(resultSelector);
        return Core(first, second, resultSelector);
        static global::System.Collections.Generic.IEnumerable<TResult> Core(global::System.Collections.Generic.IEnumerable<T1> first, global::System.Collections.Generic.IEnumerable<T2> second, global::System.Func<T1, T2, TResult> resultSelector)
        {
            using var firstMemo = first.Memoize();
            using var secondMemo = second.Memoize();
            foreach (var item1 in firstMemo)
                foreach (var item2 in secondMemo)
                    yield return resultSelector(item1, item2);
        }
    }

    /// <summary>
    ///	    Returns the Cartesian product of two sequences by enumerating all possible combinations of one item from
    ///     each sequence.
    /// </summary>
    /// <returns>
    ///		A sequence of <see cref = "global::System.ValueTuple{T1, T2}"/> containing elements from each of the 
    ///		sequences.
    /// </returns>
    /// <remarks>
    /// <para>
    ///	    The method returns items in the same order as a nested foreach loop, but all sequences are cached when
    ///     iterated over. The cache is then re-used for any subsequent iterations.
    /// </para>
    /// <para>
    ///		This method uses deferred execution and stream its results.
    /// </para>
    /// </remarks>
    /// <exception cref = "global::System.ArgumentNullException">
    ///		Any of the input sequences is <see langword="null"/>.
    /// </exception>
    /// <typeparam name = "T1">
    ///		The type of the elements of <paramref name = "first"/>.
    /// </typeparam>
    /// <param name = "first">
    ///		The first sequence of elements.
    /// </param>
    /// <typeparam name = "T2">
    ///		The type of the elements of <paramref name = "second"/>.
    /// </typeparam>
    /// <param name = "second">
    ///		The second sequence of elements.
    /// </param>
    public static global::System.Collections.Generic.IEnumerable<(T1, T2)> Cartesian<T1, T2>(this global::System.Collections.Generic.IEnumerable<T1> first, global::System.Collections.Generic.IEnumerable<T2> second) => Cartesian(first, second, global::System.ValueTuple.Create);
    /// <summary>
    ///	    Returns the Cartesian product of three sequences by enumerating all possible combinations of one item from
    ///	    each sequence, and applying a user-defined projection to the items in a given combination.
    /// </summary>
    /// <typeparam name = "TResult">
    ///		The type of the elements of the result sequence.
    /// </typeparam>
    /// <param name = "resultSelector">
    ///		A projection function that combines elements from all of the sequences.
    /// </param>
    /// <returns>
    ///		A sequence of elements returned by <paramref name = "resultSelector"/>.
    /// </returns>
    /// <remarks>
    /// <para>
    ///	    The method returns items in the same order as a nested foreach loop, but all sequences cached when iterated
    ///	    over. The cache is then re-used for any subsequent iterations.
    /// </para>
    /// <para>
    ///		This method uses deferred execution and stream its results.
    /// </para>
    /// </remarks>
    /// <exception cref = "global::System.ArgumentNullException">
    ///		<paramref name = "resultSelector"/> or any of the input sequences is <see langword="null"/>.
    /// </exception>
    /// <typeparam name = "T1">
    ///		The type of the elements of <paramref name = "first"/>.
    /// </typeparam>
    /// <param name = "first">
    ///		The first sequence of elements.
    /// </param>
    /// <typeparam name = "T2">
    ///		The type of the elements of <paramref name = "second"/>.
    /// </typeparam>
    /// <param name = "second">
    ///		The second sequence of elements.
    /// </param>
    /// <typeparam name = "T3">
    ///		The type of the elements of <paramref name = "third"/>.
    /// </typeparam>
    /// <param name = "third">
    ///		The third sequence of elements.
    /// </param>
    public static global::System.Collections.Generic.IEnumerable<TResult> Cartesian<T1, T2, T3, TResult>(this global::System.Collections.Generic.IEnumerable<T1> first, global::System.Collections.Generic.IEnumerable<T2> second, global::System.Collections.Generic.IEnumerable<T3> third, global::System.Func<T1, T2, T3, TResult> resultSelector)
    {
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(first);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(second);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(third);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(resultSelector);
        return Core(first, second, third, resultSelector);
        static global::System.Collections.Generic.IEnumerable<TResult> Core(global::System.Collections.Generic.IEnumerable<T1> first, global::System.Collections.Generic.IEnumerable<T2> second, global::System.Collections.Generic.IEnumerable<T3> third, global::System.Func<T1, T2, T3, TResult> resultSelector)
        {
            using var firstMemo = first.Memoize();
            using var secondMemo = second.Memoize();
            using var thirdMemo = third.Memoize();
            foreach (var item1 in firstMemo)
                foreach (var item2 in secondMemo)
                    foreach (var item3 in thirdMemo)
                        yield return resultSelector(item1, item2, item3);
        }
    }

    /// <summary>
    ///	    Returns the Cartesian product of three sequences by enumerating all possible combinations of one item from
    ///     each sequence.
    /// </summary>
    /// <returns>
    ///		A sequence of <see cref = "global::System.ValueTuple{T1, T2, T3}"/> containing elements from each of the 
    ///		sequences.
    /// </returns>
    /// <remarks>
    /// <para>
    ///	    The method returns items in the same order as a nested foreach loop, but all sequences are cached when
    ///     iterated over. The cache is then re-used for any subsequent iterations.
    /// </para>
    /// <para>
    ///		This method uses deferred execution and stream its results.
    /// </para>
    /// </remarks>
    /// <exception cref = "global::System.ArgumentNullException">
    ///		Any of the input sequences is <see langword="null"/>.
    /// </exception>
    /// <typeparam name = "T1">
    ///		The type of the elements of <paramref name = "first"/>.
    /// </typeparam>
    /// <param name = "first">
    ///		The first sequence of elements.
    /// </param>
    /// <typeparam name = "T2">
    ///		The type of the elements of <paramref name = "second"/>.
    /// </typeparam>
    /// <param name = "second">
    ///		The second sequence of elements.
    /// </param>
    /// <typeparam name = "T3">
    ///		The type of the elements of <paramref name = "third"/>.
    /// </typeparam>
    /// <param name = "third">
    ///		The third sequence of elements.
    /// </param>
    public static global::System.Collections.Generic.IEnumerable<(T1, T2, T3)> Cartesian<T1, T2, T3>(this global::System.Collections.Generic.IEnumerable<T1> first, global::System.Collections.Generic.IEnumerable<T2> second, global::System.Collections.Generic.IEnumerable<T3> third) => Cartesian(first, second, third, global::System.ValueTuple.Create);
    /// <summary>
    ///	    Returns the Cartesian product of four sequences by enumerating all possible combinations of one item from
    ///	    each sequence, and applying a user-defined projection to the items in a given combination.
    /// </summary>
    /// <typeparam name = "TResult">
    ///		The type of the elements of the result sequence.
    /// </typeparam>
    /// <param name = "resultSelector">
    ///		A projection function that combines elements from all of the sequences.
    /// </param>
    /// <returns>
    ///		A sequence of elements returned by <paramref name = "resultSelector"/>.
    /// </returns>
    /// <remarks>
    /// <para>
    ///	    The method returns items in the same order as a nested foreach loop, but all sequences cached when iterated
    ///	    over. The cache is then re-used for any subsequent iterations.
    /// </para>
    /// <para>
    ///		This method uses deferred execution and stream its results.
    /// </para>
    /// </remarks>
    /// <exception cref = "global::System.ArgumentNullException">
    ///		<paramref name = "resultSelector"/> or any of the input sequences is <see langword="null"/>.
    /// </exception>
    /// <typeparam name = "T1">
    ///		The type of the elements of <paramref name = "first"/>.
    /// </typeparam>
    /// <param name = "first">
    ///		The first sequence of elements.
    /// </param>
    /// <typeparam name = "T2">
    ///		The type of the elements of <paramref name = "second"/>.
    /// </typeparam>
    /// <param name = "second">
    ///		The second sequence of elements.
    /// </param>
    /// <typeparam name = "T3">
    ///		The type of the elements of <paramref name = "third"/>.
    /// </typeparam>
    /// <param name = "third">
    ///		The third sequence of elements.
    /// </param>
    /// <typeparam name = "T4">
    ///		The type of the elements of <paramref name = "fourth"/>.
    /// </typeparam>
    /// <param name = "fourth">
    ///		The fourth sequence of elements.
    /// </param>
    public static global::System.Collections.Generic.IEnumerable<TResult> Cartesian<T1, T2, T3, T4, TResult>(this global::System.Collections.Generic.IEnumerable<T1> first, global::System.Collections.Generic.IEnumerable<T2> second, global::System.Collections.Generic.IEnumerable<T3> third, global::System.Collections.Generic.IEnumerable<T4> fourth, global::System.Func<T1, T2, T3, T4, TResult> resultSelector)
    {
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(first);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(second);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(third);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(fourth);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(resultSelector);
        return Core(first, second, third, fourth, resultSelector);
        static global::System.Collections.Generic.IEnumerable<TResult> Core(global::System.Collections.Generic.IEnumerable<T1> first, global::System.Collections.Generic.IEnumerable<T2> second, global::System.Collections.Generic.IEnumerable<T3> third, global::System.Collections.Generic.IEnumerable<T4> fourth, global::System.Func<T1, T2, T3, T4, TResult> resultSelector)
        {
            using var firstMemo = first.Memoize();
            using var secondMemo = second.Memoize();
            using var thirdMemo = third.Memoize();
            using var fourthMemo = fourth.Memoize();
            foreach (var item1 in firstMemo)
                foreach (var item2 in secondMemo)
                    foreach (var item3 in thirdMemo)
                        foreach (var item4 in fourthMemo)
                            yield return resultSelector(item1, item2, item3, item4);
        }
    }

    /// <summary>
    ///	    Returns the Cartesian product of four sequences by enumerating all possible combinations of one item from
    ///     each sequence.
    /// </summary>
    /// <returns>
    ///		A sequence of <see cref = "global::System.ValueTuple{T1, T2, T3, T4}"/> containing elements from each of the 
    ///		sequences.
    /// </returns>
    /// <remarks>
    /// <para>
    ///	    The method returns items in the same order as a nested foreach loop, but all sequences are cached when
    ///     iterated over. The cache is then re-used for any subsequent iterations.
    /// </para>
    /// <para>
    ///		This method uses deferred execution and stream its results.
    /// </para>
    /// </remarks>
    /// <exception cref = "global::System.ArgumentNullException">
    ///		Any of the input sequences is <see langword="null"/>.
    /// </exception>
    /// <typeparam name = "T1">
    ///		The type of the elements of <paramref name = "first"/>.
    /// </typeparam>
    /// <param name = "first">
    ///		The first sequence of elements.
    /// </param>
    /// <typeparam name = "T2">
    ///		The type of the elements of <paramref name = "second"/>.
    /// </typeparam>
    /// <param name = "second">
    ///		The second sequence of elements.
    /// </param>
    /// <typeparam name = "T3">
    ///		The type of the elements of <paramref name = "third"/>.
    /// </typeparam>
    /// <param name = "third">
    ///		The third sequence of elements.
    /// </param>
    /// <typeparam name = "T4">
    ///		The type of the elements of <paramref name = "fourth"/>.
    /// </typeparam>
    /// <param name = "fourth">
    ///		The fourth sequence of elements.
    /// </param>
    public static global::System.Collections.Generic.IEnumerable<(T1, T2, T3, T4)> Cartesian<T1, T2, T3, T4>(this global::System.Collections.Generic.IEnumerable<T1> first, global::System.Collections.Generic.IEnumerable<T2> second, global::System.Collections.Generic.IEnumerable<T3> third, global::System.Collections.Generic.IEnumerable<T4> fourth) => Cartesian(first, second, third, fourth, global::System.ValueTuple.Create);
    /// <summary>
    ///	    Returns the Cartesian product of five sequences by enumerating all possible combinations of one item from
    ///	    each sequence, and applying a user-defined projection to the items in a given combination.
    /// </summary>
    /// <typeparam name = "TResult">
    ///		The type of the elements of the result sequence.
    /// </typeparam>
    /// <param name = "resultSelector">
    ///		A projection function that combines elements from all of the sequences.
    /// </param>
    /// <returns>
    ///		A sequence of elements returned by <paramref name = "resultSelector"/>.
    /// </returns>
    /// <remarks>
    /// <para>
    ///	    The method returns items in the same order as a nested foreach loop, but all sequences cached when iterated
    ///	    over. The cache is then re-used for any subsequent iterations.
    /// </para>
    /// <para>
    ///		This method uses deferred execution and stream its results.
    /// </para>
    /// </remarks>
    /// <exception cref = "global::System.ArgumentNullException">
    ///		<paramref name = "resultSelector"/> or any of the input sequences is <see langword="null"/>.
    /// </exception>
    /// <typeparam name = "T1">
    ///		The type of the elements of <paramref name = "first"/>.
    /// </typeparam>
    /// <param name = "first">
    ///		The first sequence of elements.
    /// </param>
    /// <typeparam name = "T2">
    ///		The type of the elements of <paramref name = "second"/>.
    /// </typeparam>
    /// <param name = "second">
    ///		The second sequence of elements.
    /// </param>
    /// <typeparam name = "T3">
    ///		The type of the elements of <paramref name = "third"/>.
    /// </typeparam>
    /// <param name = "third">
    ///		The third sequence of elements.
    /// </param>
    /// <typeparam name = "T4">
    ///		The type of the elements of <paramref name = "fourth"/>.
    /// </typeparam>
    /// <param name = "fourth">
    ///		The fourth sequence of elements.
    /// </param>
    /// <typeparam name = "T5">
    ///		The type of the elements of <paramref name = "fifth"/>.
    /// </typeparam>
    /// <param name = "fifth">
    ///		The fifth sequence of elements.
    /// </param>
    public static global::System.Collections.Generic.IEnumerable<TResult> Cartesian<T1, T2, T3, T4, T5, TResult>(this global::System.Collections.Generic.IEnumerable<T1> first, global::System.Collections.Generic.IEnumerable<T2> second, global::System.Collections.Generic.IEnumerable<T3> third, global::System.Collections.Generic.IEnumerable<T4> fourth, global::System.Collections.Generic.IEnumerable<T5> fifth, global::System.Func<T1, T2, T3, T4, T5, TResult> resultSelector)
    {
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(first);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(second);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(third);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(fourth);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(fifth);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(resultSelector);
        return Core(first, second, third, fourth, fifth, resultSelector);
        static global::System.Collections.Generic.IEnumerable<TResult> Core(global::System.Collections.Generic.IEnumerable<T1> first, global::System.Collections.Generic.IEnumerable<T2> second, global::System.Collections.Generic.IEnumerable<T3> third, global::System.Collections.Generic.IEnumerable<T4> fourth, global::System.Collections.Generic.IEnumerable<T5> fifth, global::System.Func<T1, T2, T3, T4, T5, TResult> resultSelector)
        {
            using var firstMemo = first.Memoize();
            using var secondMemo = second.Memoize();
            using var thirdMemo = third.Memoize();
            using var fourthMemo = fourth.Memoize();
            using var fifthMemo = fifth.Memoize();
            foreach (var item1 in firstMemo)
                foreach (var item2 in secondMemo)
                    foreach (var item3 in thirdMemo)
                        foreach (var item4 in fourthMemo)
                            foreach (var item5 in fifthMemo)
                                yield return resultSelector(item1, item2, item3, item4, item5);
        }
    }

    /// <summary>
    ///	    Returns the Cartesian product of five sequences by enumerating all possible combinations of one item from
    ///     each sequence.
    /// </summary>
    /// <returns>
    ///		A sequence of <see cref = "global::System.ValueTuple{T1, T2, T3, T4, T5}"/> containing elements from each of the 
    ///		sequences.
    /// </returns>
    /// <remarks>
    /// <para>
    ///	    The method returns items in the same order as a nested foreach loop, but all sequences are cached when
    ///     iterated over. The cache is then re-used for any subsequent iterations.
    /// </para>
    /// <para>
    ///		This method uses deferred execution and stream its results.
    /// </para>
    /// </remarks>
    /// <exception cref = "global::System.ArgumentNullException">
    ///		Any of the input sequences is <see langword="null"/>.
    /// </exception>
    /// <typeparam name = "T1">
    ///		The type of the elements of <paramref name = "first"/>.
    /// </typeparam>
    /// <param name = "first">
    ///		The first sequence of elements.
    /// </param>
    /// <typeparam name = "T2">
    ///		The type of the elements of <paramref name = "second"/>.
    /// </typeparam>
    /// <param name = "second">
    ///		The second sequence of elements.
    /// </param>
    /// <typeparam name = "T3">
    ///		The type of the elements of <paramref name = "third"/>.
    /// </typeparam>
    /// <param name = "third">
    ///		The third sequence of elements.
    /// </param>
    /// <typeparam name = "T4">
    ///		The type of the elements of <paramref name = "fourth"/>.
    /// </typeparam>
    /// <param name = "fourth">
    ///		The fourth sequence of elements.
    /// </param>
    /// <typeparam name = "T5">
    ///		The type of the elements of <paramref name = "fifth"/>.
    /// </typeparam>
    /// <param name = "fifth">
    ///		The fifth sequence of elements.
    /// </param>
    public static global::System.Collections.Generic.IEnumerable<(T1, T2, T3, T4, T5)> Cartesian<T1, T2, T3, T4, T5>(this global::System.Collections.Generic.IEnumerable<T1> first, global::System.Collections.Generic.IEnumerable<T2> second, global::System.Collections.Generic.IEnumerable<T3> third, global::System.Collections.Generic.IEnumerable<T4> fourth, global::System.Collections.Generic.IEnumerable<T5> fifth) => Cartesian(first, second, third, fourth, fifth, global::System.ValueTuple.Create);
    /// <summary>
    ///	    Returns the Cartesian product of six sequences by enumerating all possible combinations of one item from
    ///	    each sequence, and applying a user-defined projection to the items in a given combination.
    /// </summary>
    /// <typeparam name = "TResult">
    ///		The type of the elements of the result sequence.
    /// </typeparam>
    /// <param name = "resultSelector">
    ///		A projection function that combines elements from all of the sequences.
    /// </param>
    /// <returns>
    ///		A sequence of elements returned by <paramref name = "resultSelector"/>.
    /// </returns>
    /// <remarks>
    /// <para>
    ///	    The method returns items in the same order as a nested foreach loop, but all sequences cached when iterated
    ///	    over. The cache is then re-used for any subsequent iterations.
    /// </para>
    /// <para>
    ///		This method uses deferred execution and stream its results.
    /// </para>
    /// </remarks>
    /// <exception cref = "global::System.ArgumentNullException">
    ///		<paramref name = "resultSelector"/> or any of the input sequences is <see langword="null"/>.
    /// </exception>
    /// <typeparam name = "T1">
    ///		The type of the elements of <paramref name = "first"/>.
    /// </typeparam>
    /// <param name = "first">
    ///		The first sequence of elements.
    /// </param>
    /// <typeparam name = "T2">
    ///		The type of the elements of <paramref name = "second"/>.
    /// </typeparam>
    /// <param name = "second">
    ///		The second sequence of elements.
    /// </param>
    /// <typeparam name = "T3">
    ///		The type of the elements of <paramref name = "third"/>.
    /// </typeparam>
    /// <param name = "third">
    ///		The third sequence of elements.
    /// </param>
    /// <typeparam name = "T4">
    ///		The type of the elements of <paramref name = "fourth"/>.
    /// </typeparam>
    /// <param name = "fourth">
    ///		The fourth sequence of elements.
    /// </param>
    /// <typeparam name = "T5">
    ///		The type of the elements of <paramref name = "fifth"/>.
    /// </typeparam>
    /// <param name = "fifth">
    ///		The fifth sequence of elements.
    /// </param>
    /// <typeparam name = "T6">
    ///		The type of the elements of <paramref name = "sixth"/>.
    /// </typeparam>
    /// <param name = "sixth">
    ///		The sixth sequence of elements.
    /// </param>
    public static global::System.Collections.Generic.IEnumerable<TResult> Cartesian<T1, T2, T3, T4, T5, T6, TResult>(this global::System.Collections.Generic.IEnumerable<T1> first, global::System.Collections.Generic.IEnumerable<T2> second, global::System.Collections.Generic.IEnumerable<T3> third, global::System.Collections.Generic.IEnumerable<T4> fourth, global::System.Collections.Generic.IEnumerable<T5> fifth, global::System.Collections.Generic.IEnumerable<T6> sixth, global::System.Func<T1, T2, T3, T4, T5, T6, TResult> resultSelector)
    {
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(first);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(second);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(third);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(fourth);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(fifth);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(sixth);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(resultSelector);
        return Core(first, second, third, fourth, fifth, sixth, resultSelector);
        static global::System.Collections.Generic.IEnumerable<TResult> Core(global::System.Collections.Generic.IEnumerable<T1> first, global::System.Collections.Generic.IEnumerable<T2> second, global::System.Collections.Generic.IEnumerable<T3> third, global::System.Collections.Generic.IEnumerable<T4> fourth, global::System.Collections.Generic.IEnumerable<T5> fifth, global::System.Collections.Generic.IEnumerable<T6> sixth, global::System.Func<T1, T2, T3, T4, T5, T6, TResult> resultSelector)
        {
            using var firstMemo = first.Memoize();
            using var secondMemo = second.Memoize();
            using var thirdMemo = third.Memoize();
            using var fourthMemo = fourth.Memoize();
            using var fifthMemo = fifth.Memoize();
            using var sixthMemo = sixth.Memoize();
            foreach (var item1 in firstMemo)
                foreach (var item2 in secondMemo)
                    foreach (var item3 in thirdMemo)
                        foreach (var item4 in fourthMemo)
                            foreach (var item5 in fifthMemo)
                                foreach (var item6 in sixthMemo)
                                    yield return resultSelector(item1, item2, item3, item4, item5, item6);
        }
    }

    /// <summary>
    ///	    Returns the Cartesian product of six sequences by enumerating all possible combinations of one item from
    ///     each sequence.
    /// </summary>
    /// <returns>
    ///		A sequence of <see cref = "global::System.ValueTuple{T1, T2, T3, T4, T5, T6}"/> containing elements from each of the 
    ///		sequences.
    /// </returns>
    /// <remarks>
    /// <para>
    ///	    The method returns items in the same order as a nested foreach loop, but all sequences are cached when
    ///     iterated over. The cache is then re-used for any subsequent iterations.
    /// </para>
    /// <para>
    ///		This method uses deferred execution and stream its results.
    /// </para>
    /// </remarks>
    /// <exception cref = "global::System.ArgumentNullException">
    ///		Any of the input sequences is <see langword="null"/>.
    /// </exception>
    /// <typeparam name = "T1">
    ///		The type of the elements of <paramref name = "first"/>.
    /// </typeparam>
    /// <param name = "first">
    ///		The first sequence of elements.
    /// </param>
    /// <typeparam name = "T2">
    ///		The type of the elements of <paramref name = "second"/>.
    /// </typeparam>
    /// <param name = "second">
    ///		The second sequence of elements.
    /// </param>
    /// <typeparam name = "T3">
    ///		The type of the elements of <paramref name = "third"/>.
    /// </typeparam>
    /// <param name = "third">
    ///		The third sequence of elements.
    /// </param>
    /// <typeparam name = "T4">
    ///		The type of the elements of <paramref name = "fourth"/>.
    /// </typeparam>
    /// <param name = "fourth">
    ///		The fourth sequence of elements.
    /// </param>
    /// <typeparam name = "T5">
    ///		The type of the elements of <paramref name = "fifth"/>.
    /// </typeparam>
    /// <param name = "fifth">
    ///		The fifth sequence of elements.
    /// </param>
    /// <typeparam name = "T6">
    ///		The type of the elements of <paramref name = "sixth"/>.
    /// </typeparam>
    /// <param name = "sixth">
    ///		The sixth sequence of elements.
    /// </param>
    public static global::System.Collections.Generic.IEnumerable<(T1, T2, T3, T4, T5, T6)> Cartesian<T1, T2, T3, T4, T5, T6>(this global::System.Collections.Generic.IEnumerable<T1> first, global::System.Collections.Generic.IEnumerable<T2> second, global::System.Collections.Generic.IEnumerable<T3> third, global::System.Collections.Generic.IEnumerable<T4> fourth, global::System.Collections.Generic.IEnumerable<T5> fifth, global::System.Collections.Generic.IEnumerable<T6> sixth) => Cartesian(first, second, third, fourth, fifth, sixth, global::System.ValueTuple.Create);
    /// <summary>
    ///	    Returns the Cartesian product of seven sequences by enumerating all possible combinations of one item from
    ///	    each sequence, and applying a user-defined projection to the items in a given combination.
    /// </summary>
    /// <typeparam name = "TResult">
    ///		The type of the elements of the result sequence.
    /// </typeparam>
    /// <param name = "resultSelector">
    ///		A projection function that combines elements from all of the sequences.
    /// </param>
    /// <returns>
    ///		A sequence of elements returned by <paramref name = "resultSelector"/>.
    /// </returns>
    /// <remarks>
    /// <para>
    ///	    The method returns items in the same order as a nested foreach loop, but all sequences cached when iterated
    ///	    over. The cache is then re-used for any subsequent iterations.
    /// </para>
    /// <para>
    ///		This method uses deferred execution and stream its results.
    /// </para>
    /// </remarks>
    /// <exception cref = "global::System.ArgumentNullException">
    ///		<paramref name = "resultSelector"/> or any of the input sequences is <see langword="null"/>.
    /// </exception>
    /// <typeparam name = "T1">
    ///		The type of the elements of <paramref name = "first"/>.
    /// </typeparam>
    /// <param name = "first">
    ///		The first sequence of elements.
    /// </param>
    /// <typeparam name = "T2">
    ///		The type of the elements of <paramref name = "second"/>.
    /// </typeparam>
    /// <param name = "second">
    ///		The second sequence of elements.
    /// </param>
    /// <typeparam name = "T3">
    ///		The type of the elements of <paramref name = "third"/>.
    /// </typeparam>
    /// <param name = "third">
    ///		The third sequence of elements.
    /// </param>
    /// <typeparam name = "T4">
    ///		The type of the elements of <paramref name = "fourth"/>.
    /// </typeparam>
    /// <param name = "fourth">
    ///		The fourth sequence of elements.
    /// </param>
    /// <typeparam name = "T5">
    ///		The type of the elements of <paramref name = "fifth"/>.
    /// </typeparam>
    /// <param name = "fifth">
    ///		The fifth sequence of elements.
    /// </param>
    /// <typeparam name = "T6">
    ///		The type of the elements of <paramref name = "sixth"/>.
    /// </typeparam>
    /// <param name = "sixth">
    ///		The sixth sequence of elements.
    /// </param>
    /// <typeparam name = "T7">
    ///		The type of the elements of <paramref name = "seventh"/>.
    /// </typeparam>
    /// <param name = "seventh">
    ///		The seventh sequence of elements.
    /// </param>
    public static global::System.Collections.Generic.IEnumerable<TResult> Cartesian<T1, T2, T3, T4, T5, T6, T7, TResult>(this global::System.Collections.Generic.IEnumerable<T1> first, global::System.Collections.Generic.IEnumerable<T2> second, global::System.Collections.Generic.IEnumerable<T3> third, global::System.Collections.Generic.IEnumerable<T4> fourth, global::System.Collections.Generic.IEnumerable<T5> fifth, global::System.Collections.Generic.IEnumerable<T6> sixth, global::System.Collections.Generic.IEnumerable<T7> seventh, global::System.Func<T1, T2, T3, T4, T5, T6, T7, TResult> resultSelector)
    {
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(first);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(second);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(third);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(fourth);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(fifth);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(sixth);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(seventh);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(resultSelector);
        return Core(first, second, third, fourth, fifth, sixth, seventh, resultSelector);
        static global::System.Collections.Generic.IEnumerable<TResult> Core(global::System.Collections.Generic.IEnumerable<T1> first, global::System.Collections.Generic.IEnumerable<T2> second, global::System.Collections.Generic.IEnumerable<T3> third, global::System.Collections.Generic.IEnumerable<T4> fourth, global::System.Collections.Generic.IEnumerable<T5> fifth, global::System.Collections.Generic.IEnumerable<T6> sixth, global::System.Collections.Generic.IEnumerable<T7> seventh, global::System.Func<T1, T2, T3, T4, T5, T6, T7, TResult> resultSelector)
        {
            using var firstMemo = first.Memoize();
            using var secondMemo = second.Memoize();
            using var thirdMemo = third.Memoize();
            using var fourthMemo = fourth.Memoize();
            using var fifthMemo = fifth.Memoize();
            using var sixthMemo = sixth.Memoize();
            using var seventhMemo = seventh.Memoize();
            foreach (var item1 in firstMemo)
                foreach (var item2 in secondMemo)
                    foreach (var item3 in thirdMemo)
                        foreach (var item4 in fourthMemo)
                            foreach (var item5 in fifthMemo)
                                foreach (var item6 in sixthMemo)
                                    foreach (var item7 in seventhMemo)
                                        yield return resultSelector(item1, item2, item3, item4, item5, item6, item7);
        }
    }

    /// <summary>
    ///	    Returns the Cartesian product of seven sequences by enumerating all possible combinations of one item from
    ///     each sequence.
    /// </summary>
    /// <returns>
    ///		A sequence of <see cref = "global::System.ValueTuple{T1, T2, T3, T4, T5, T6, T7}"/> containing elements from each of the 
    ///		sequences.
    /// </returns>
    /// <remarks>
    /// <para>
    ///	    The method returns items in the same order as a nested foreach loop, but all sequences are cached when
    ///     iterated over. The cache is then re-used for any subsequent iterations.
    /// </para>
    /// <para>
    ///		This method uses deferred execution and stream its results.
    /// </para>
    /// </remarks>
    /// <exception cref = "global::System.ArgumentNullException">
    ///		Any of the input sequences is <see langword="null"/>.
    /// </exception>
    /// <typeparam name = "T1">
    ///		The type of the elements of <paramref name = "first"/>.
    /// </typeparam>
    /// <param name = "first">
    ///		The first sequence of elements.
    /// </param>
    /// <typeparam name = "T2">
    ///		The type of the elements of <paramref name = "second"/>.
    /// </typeparam>
    /// <param name = "second">
    ///		The second sequence of elements.
    /// </param>
    /// <typeparam name = "T3">
    ///		The type of the elements of <paramref name = "third"/>.
    /// </typeparam>
    /// <param name = "third">
    ///		The third sequence of elements.
    /// </param>
    /// <typeparam name = "T4">
    ///		The type of the elements of <paramref name = "fourth"/>.
    /// </typeparam>
    /// <param name = "fourth">
    ///		The fourth sequence of elements.
    /// </param>
    /// <typeparam name = "T5">
    ///		The type of the elements of <paramref name = "fifth"/>.
    /// </typeparam>
    /// <param name = "fifth">
    ///		The fifth sequence of elements.
    /// </param>
    /// <typeparam name = "T6">
    ///		The type of the elements of <paramref name = "sixth"/>.
    /// </typeparam>
    /// <param name = "sixth">
    ///		The sixth sequence of elements.
    /// </param>
    /// <typeparam name = "T7">
    ///		The type of the elements of <paramref name = "seventh"/>.
    /// </typeparam>
    /// <param name = "seventh">
    ///		The seventh sequence of elements.
    /// </param>
    public static global::System.Collections.Generic.IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> Cartesian<T1, T2, T3, T4, T5, T6, T7>(this global::System.Collections.Generic.IEnumerable<T1> first, global::System.Collections.Generic.IEnumerable<T2> second, global::System.Collections.Generic.IEnumerable<T3> third, global::System.Collections.Generic.IEnumerable<T4> fourth, global::System.Collections.Generic.IEnumerable<T5> fifth, global::System.Collections.Generic.IEnumerable<T6> sixth, global::System.Collections.Generic.IEnumerable<T7> seventh) => Cartesian(first, second, third, fourth, fifth, sixth, seventh, global::System.ValueTuple.Create);
    /// <summary>
    ///	    Returns the Cartesian product of eight sequences by enumerating all possible combinations of one item from
    ///	    each sequence, and applying a user-defined projection to the items in a given combination.
    /// </summary>
    /// <typeparam name = "TResult">
    ///		The type of the elements of the result sequence.
    /// </typeparam>
    /// <param name = "resultSelector">
    ///		A projection function that combines elements from all of the sequences.
    /// </param>
    /// <returns>
    ///		A sequence of elements returned by <paramref name = "resultSelector"/>.
    /// </returns>
    /// <remarks>
    /// <para>
    ///	    The method returns items in the same order as a nested foreach loop, but all sequences cached when iterated
    ///	    over. The cache is then re-used for any subsequent iterations.
    /// </para>
    /// <para>
    ///		This method uses deferred execution and stream its results.
    /// </para>
    /// </remarks>
    /// <exception cref = "global::System.ArgumentNullException">
    ///		<paramref name = "resultSelector"/> or any of the input sequences is <see langword="null"/>.
    /// </exception>
    /// <typeparam name = "T1">
    ///		The type of the elements of <paramref name = "first"/>.
    /// </typeparam>
    /// <param name = "first">
    ///		The first sequence of elements.
    /// </param>
    /// <typeparam name = "T2">
    ///		The type of the elements of <paramref name = "second"/>.
    /// </typeparam>
    /// <param name = "second">
    ///		The second sequence of elements.
    /// </param>
    /// <typeparam name = "T3">
    ///		The type of the elements of <paramref name = "third"/>.
    /// </typeparam>
    /// <param name = "third">
    ///		The third sequence of elements.
    /// </param>
    /// <typeparam name = "T4">
    ///		The type of the elements of <paramref name = "fourth"/>.
    /// </typeparam>
    /// <param name = "fourth">
    ///		The fourth sequence of elements.
    /// </param>
    /// <typeparam name = "T5">
    ///		The type of the elements of <paramref name = "fifth"/>.
    /// </typeparam>
    /// <param name = "fifth">
    ///		The fifth sequence of elements.
    /// </param>
    /// <typeparam name = "T6">
    ///		The type of the elements of <paramref name = "sixth"/>.
    /// </typeparam>
    /// <param name = "sixth">
    ///		The sixth sequence of elements.
    /// </param>
    /// <typeparam name = "T7">
    ///		The type of the elements of <paramref name = "seventh"/>.
    /// </typeparam>
    /// <param name = "seventh">
    ///		The seventh sequence of elements.
    /// </param>
    /// <typeparam name = "T8">
    ///		The type of the elements of <paramref name = "eighth"/>.
    /// </typeparam>
    /// <param name = "eighth">
    ///		The eighth sequence of elements.
    /// </param>
    public static global::System.Collections.Generic.IEnumerable<TResult> Cartesian<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this global::System.Collections.Generic.IEnumerable<T1> first, global::System.Collections.Generic.IEnumerable<T2> second, global::System.Collections.Generic.IEnumerable<T3> third, global::System.Collections.Generic.IEnumerable<T4> fourth, global::System.Collections.Generic.IEnumerable<T5> fifth, global::System.Collections.Generic.IEnumerable<T6> sixth, global::System.Collections.Generic.IEnumerable<T7> seventh, global::System.Collections.Generic.IEnumerable<T8> eighth, global::System.Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> resultSelector)
    {
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(first);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(second);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(third);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(fourth);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(fifth);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(sixth);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(seventh);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(eighth);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(resultSelector);
        return Core(first, second, third, fourth, fifth, sixth, seventh, eighth, resultSelector);
        static global::System.Collections.Generic.IEnumerable<TResult> Core(global::System.Collections.Generic.IEnumerable<T1> first, global::System.Collections.Generic.IEnumerable<T2> second, global::System.Collections.Generic.IEnumerable<T3> third, global::System.Collections.Generic.IEnumerable<T4> fourth, global::System.Collections.Generic.IEnumerable<T5> fifth, global::System.Collections.Generic.IEnumerable<T6> sixth, global::System.Collections.Generic.IEnumerable<T7> seventh, global::System.Collections.Generic.IEnumerable<T8> eighth, global::System.Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> resultSelector)
        {
            using var firstMemo = first.Memoize();
            using var secondMemo = second.Memoize();
            using var thirdMemo = third.Memoize();
            using var fourthMemo = fourth.Memoize();
            using var fifthMemo = fifth.Memoize();
            using var sixthMemo = sixth.Memoize();
            using var seventhMemo = seventh.Memoize();
            using var eighthMemo = eighth.Memoize();
            foreach (var item1 in firstMemo)
                foreach (var item2 in secondMemo)
                    foreach (var item3 in thirdMemo)
                        foreach (var item4 in fourthMemo)
                            foreach (var item5 in fifthMemo)
                                foreach (var item6 in sixthMemo)
                                    foreach (var item7 in seventhMemo)
                                        foreach (var item8 in eighthMemo)
                                            yield return resultSelector(item1, item2, item3, item4, item5, item6, item7, item8);
        }
    }

    /// <summary>
    ///	    Returns the Cartesian product of eight sequences by enumerating all possible combinations of one item from
    ///     each sequence.
    /// </summary>
    /// <returns>
    ///		A sequence of <see cref = "global::System.ValueTuple{T1, T2, T3, T4, T5, T6, T7, T8}"/> containing elements from each of the 
    ///		sequences.
    /// </returns>
    /// <remarks>
    /// <para>
    ///	    The method returns items in the same order as a nested foreach loop, but all sequences are cached when
    ///     iterated over. The cache is then re-used for any subsequent iterations.
    /// </para>
    /// <para>
    ///		This method uses deferred execution and stream its results.
    /// </para>
    /// </remarks>
    /// <exception cref = "global::System.ArgumentNullException">
    ///		Any of the input sequences is <see langword="null"/>.
    /// </exception>
    /// <typeparam name = "T1">
    ///		The type of the elements of <paramref name = "first"/>.
    /// </typeparam>
    /// <param name = "first">
    ///		The first sequence of elements.
    /// </param>
    /// <typeparam name = "T2">
    ///		The type of the elements of <paramref name = "second"/>.
    /// </typeparam>
    /// <param name = "second">
    ///		The second sequence of elements.
    /// </param>
    /// <typeparam name = "T3">
    ///		The type of the elements of <paramref name = "third"/>.
    /// </typeparam>
    /// <param name = "third">
    ///		The third sequence of elements.
    /// </param>
    /// <typeparam name = "T4">
    ///		The type of the elements of <paramref name = "fourth"/>.
    /// </typeparam>
    /// <param name = "fourth">
    ///		The fourth sequence of elements.
    /// </param>
    /// <typeparam name = "T5">
    ///		The type of the elements of <paramref name = "fifth"/>.
    /// </typeparam>
    /// <param name = "fifth">
    ///		The fifth sequence of elements.
    /// </param>
    /// <typeparam name = "T6">
    ///		The type of the elements of <paramref name = "sixth"/>.
    /// </typeparam>
    /// <param name = "sixth">
    ///		The sixth sequence of elements.
    /// </param>
    /// <typeparam name = "T7">
    ///		The type of the elements of <paramref name = "seventh"/>.
    /// </typeparam>
    /// <param name = "seventh">
    ///		The seventh sequence of elements.
    /// </param>
    /// <typeparam name = "T8">
    ///		The type of the elements of <paramref name = "eighth"/>.
    /// </typeparam>
    /// <param name = "eighth">
    ///		The eighth sequence of elements.
    /// </param>
    public static global::System.Collections.Generic.IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> Cartesian<T1, T2, T3, T4, T5, T6, T7, T8>(this global::System.Collections.Generic.IEnumerable<T1> first, global::System.Collections.Generic.IEnumerable<T2> second, global::System.Collections.Generic.IEnumerable<T3> third, global::System.Collections.Generic.IEnumerable<T4> fourth, global::System.Collections.Generic.IEnumerable<T5> fifth, global::System.Collections.Generic.IEnumerable<T6> sixth, global::System.Collections.Generic.IEnumerable<T7> seventh, global::System.Collections.Generic.IEnumerable<T8> eighth) => Cartesian(first, second, third, fourth, fifth, sixth, seventh, eighth, global::System.ValueTuple.Create);
}