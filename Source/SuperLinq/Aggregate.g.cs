namespace SuperLinq;

public static partial class SuperEnumerable
{

    /// <summary>
    /// Applies two accumulators sequentially in a single pass over a
    /// sequence.
    /// </summary>
    /// <typeparam name="T">The type of elements in <paramref name="source"/>.</typeparam>
    /// <typeparam name="TAccumulate1">The type of first accumulator value.</typeparam>
    /// <typeparam name="TAccumulate2">The type of second accumulator value.</typeparam>
    /// <typeparam name="TResult">The type of the accumulated result.</typeparam>
    /// <param name="source">The source sequence</param>
    /// <param name="seed1">The seed value for the first accumulator.</param>
    /// <param name="accumulator1">The first accumulator.</param>
    /// <param name="seed2">The seed value for the second accumulator.</param>
    /// <param name="accumulator2">The second accumulator.</param>
    /// <param name="resultSelector">
    /// A function that projects a single result given the result of each
    /// accumulator.</param>
    /// <returns>The value returned by <paramref name="resultSelector"/>.</returns>
    /// <remarks>
    /// This operator executes immediately.
    /// </remarks>

    public static TResult Aggregate<T, TAccumulate1, TAccumulate2, TResult>(
        this IEnumerable<T> source,
        TAccumulate1 seed1, Func<TAccumulate1, T, TAccumulate1> accumulator1,
        TAccumulate2 seed2, Func<TAccumulate2, T, TAccumulate2> accumulator2,
        Func<TAccumulate1, TAccumulate2, TResult> resultSelector)
    {
        source.ThrowIfNull();
        accumulator1.ThrowIfNull();
        accumulator2.ThrowIfNull();
        resultSelector.ThrowIfNull();

        var a1 = seed1;
        var a2 = seed2;

        foreach (var item in source)
        {
            a1 = accumulator1(a1, item);
            a2 = accumulator2(a2, item);
        }

        return resultSelector(a1, a2);
    }

    /// <summary>
    /// Applies three accumulators sequentially in a single pass over a
    /// sequence.
    /// </summary>
    /// <typeparam name="T">The type of elements in <paramref name="source"/>.</typeparam>
    /// <typeparam name="TAccumulate1">The type of first accumulator value.</typeparam>
    /// <typeparam name="TAccumulate2">The type of second accumulator value.</typeparam>
    /// <typeparam name="TAccumulate3">The type of third accumulator value.</typeparam>
    /// <typeparam name="TResult">The type of the accumulated result.</typeparam>
    /// <param name="source">The source sequence</param>
    /// <param name="seed1">The seed value for the first accumulator.</param>
    /// <param name="accumulator1">The first accumulator.</param>
    /// <param name="seed2">The seed value for the second accumulator.</param>
    /// <param name="accumulator2">The second accumulator.</param>
    /// <param name="seed3">The seed value for the third accumulator.</param>
    /// <param name="accumulator3">The third accumulator.</param>
    /// <param name="resultSelector">
    /// A function that projects a single result given the result of each
    /// accumulator.</param>
    /// <returns>The value returned by <paramref name="resultSelector"/>.</returns>
    /// <remarks>
    /// This operator executes immediately.
    /// </remarks>

    public static TResult Aggregate<T, TAccumulate1, TAccumulate2, TAccumulate3, TResult>(
        this IEnumerable<T> source,
        TAccumulate1 seed1, Func<TAccumulate1, T, TAccumulate1> accumulator1,
        TAccumulate2 seed2, Func<TAccumulate2, T, TAccumulate2> accumulator2,
        TAccumulate3 seed3, Func<TAccumulate3, T, TAccumulate3> accumulator3,
        Func<TAccumulate1, TAccumulate2, TAccumulate3, TResult> resultSelector)
    {
        source.ThrowIfNull();
        accumulator1.ThrowIfNull();
        accumulator2.ThrowIfNull();
        accumulator3.ThrowIfNull();
        resultSelector.ThrowIfNull();

        var a1 = seed1;
        var a2 = seed2;
        var a3 = seed3;

        foreach (var item in source)
        {
            a1 = accumulator1(a1, item);
            a2 = accumulator2(a2, item);
            a3 = accumulator3(a3, item);
        }

        return resultSelector(a1, a2, a3);
    }

    /// <summary>
    /// Applies four accumulators sequentially in a single pass over a
    /// sequence.
    /// </summary>
    /// <typeparam name="T">The type of elements in <paramref name="source"/>.</typeparam>
    /// <typeparam name="TAccumulate1">The type of first accumulator value.</typeparam>
    /// <typeparam name="TAccumulate2">The type of second accumulator value.</typeparam>
    /// <typeparam name="TAccumulate3">The type of third accumulator value.</typeparam>
    /// <typeparam name="TAccumulate4">The type of fourth accumulator value.</typeparam>
    /// <typeparam name="TResult">The type of the accumulated result.</typeparam>
    /// <param name="source">The source sequence</param>
    /// <param name="seed1">The seed value for the first accumulator.</param>
    /// <param name="accumulator1">The first accumulator.</param>
    /// <param name="seed2">The seed value for the second accumulator.</param>
    /// <param name="accumulator2">The second accumulator.</param>
    /// <param name="seed3">The seed value for the third accumulator.</param>
    /// <param name="accumulator3">The third accumulator.</param>
    /// <param name="seed4">The seed value for the fourth accumulator.</param>
    /// <param name="accumulator4">The fourth accumulator.</param>
    /// <param name="resultSelector">
    /// A function that projects a single result given the result of each
    /// accumulator.</param>
    /// <returns>The value returned by <paramref name="resultSelector"/>.</returns>
    /// <remarks>
    /// This operator executes immediately.
    /// </remarks>

    public static TResult Aggregate<T, TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TResult>(
        this IEnumerable<T> source,
        TAccumulate1 seed1, Func<TAccumulate1, T, TAccumulate1> accumulator1,
        TAccumulate2 seed2, Func<TAccumulate2, T, TAccumulate2> accumulator2,
        TAccumulate3 seed3, Func<TAccumulate3, T, TAccumulate3> accumulator3,
        TAccumulate4 seed4, Func<TAccumulate4, T, TAccumulate4> accumulator4,
        Func<TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TResult> resultSelector)
    {
        source.ThrowIfNull();
        accumulator1.ThrowIfNull();
        accumulator2.ThrowIfNull();
        accumulator3.ThrowIfNull();
        accumulator4.ThrowIfNull();
        resultSelector.ThrowIfNull();

        var a1 = seed1;
        var a2 = seed2;
        var a3 = seed3;
        var a4 = seed4;

        foreach (var item in source)
        {
            a1 = accumulator1(a1, item);
            a2 = accumulator2(a2, item);
            a3 = accumulator3(a3, item);
            a4 = accumulator4(a4, item);
        }

        return resultSelector(a1, a2, a3, a4);
    }

    /// <summary>
    /// Applies five accumulators sequentially in a single pass over a
    /// sequence.
    /// </summary>
    /// <typeparam name="T">The type of elements in <paramref name="source"/>.</typeparam>
    /// <typeparam name="TAccumulate1">The type of first accumulator value.</typeparam>
    /// <typeparam name="TAccumulate2">The type of second accumulator value.</typeparam>
    /// <typeparam name="TAccumulate3">The type of third accumulator value.</typeparam>
    /// <typeparam name="TAccumulate4">The type of fourth accumulator value.</typeparam>
    /// <typeparam name="TAccumulate5">The type of fifth accumulator value.</typeparam>
    /// <typeparam name="TResult">The type of the accumulated result.</typeparam>
    /// <param name="source">The source sequence</param>
    /// <param name="seed1">The seed value for the first accumulator.</param>
    /// <param name="accumulator1">The first accumulator.</param>
    /// <param name="seed2">The seed value for the second accumulator.</param>
    /// <param name="accumulator2">The second accumulator.</param>
    /// <param name="seed3">The seed value for the third accumulator.</param>
    /// <param name="accumulator3">The third accumulator.</param>
    /// <param name="seed4">The seed value for the fourth accumulator.</param>
    /// <param name="accumulator4">The fourth accumulator.</param>
    /// <param name="seed5">The seed value for the fifth accumulator.</param>
    /// <param name="accumulator5">The fifth accumulator.</param>
    /// <param name="resultSelector">
    /// A function that projects a single result given the result of each
    /// accumulator.</param>
    /// <returns>The value returned by <paramref name="resultSelector"/>.</returns>
    /// <remarks>
    /// This operator executes immediately.
    /// </remarks>

    public static TResult Aggregate<T, TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TAccumulate5, TResult>(
        this IEnumerable<T> source,
        TAccumulate1 seed1, Func<TAccumulate1, T, TAccumulate1> accumulator1,
        TAccumulate2 seed2, Func<TAccumulate2, T, TAccumulate2> accumulator2,
        TAccumulate3 seed3, Func<TAccumulate3, T, TAccumulate3> accumulator3,
        TAccumulate4 seed4, Func<TAccumulate4, T, TAccumulate4> accumulator4,
        TAccumulate5 seed5, Func<TAccumulate5, T, TAccumulate5> accumulator5,
        Func<TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TAccumulate5, TResult> resultSelector)
    {
        source.ThrowIfNull();
        accumulator1.ThrowIfNull();
        accumulator2.ThrowIfNull();
        accumulator3.ThrowIfNull();
        accumulator4.ThrowIfNull();
        accumulator5.ThrowIfNull();
        resultSelector.ThrowIfNull();

        var a1 = seed1;
        var a2 = seed2;
        var a3 = seed3;
        var a4 = seed4;
        var a5 = seed5;

        foreach (var item in source)
        {
            a1 = accumulator1(a1, item);
            a2 = accumulator2(a2, item);
            a3 = accumulator3(a3, item);
            a4 = accumulator4(a4, item);
            a5 = accumulator5(a5, item);
        }

        return resultSelector(a1, a2, a3, a4, a5);
    }

    /// <summary>
    /// Applies six accumulators sequentially in a single pass over a
    /// sequence.
    /// </summary>
    /// <typeparam name="T">The type of elements in <paramref name="source"/>.</typeparam>
    /// <typeparam name="TAccumulate1">The type of first accumulator value.</typeparam>
    /// <typeparam name="TAccumulate2">The type of second accumulator value.</typeparam>
    /// <typeparam name="TAccumulate3">The type of third accumulator value.</typeparam>
    /// <typeparam name="TAccumulate4">The type of fourth accumulator value.</typeparam>
    /// <typeparam name="TAccumulate5">The type of fifth accumulator value.</typeparam>
    /// <typeparam name="TAccumulate6">The type of sixth accumulator value.</typeparam>
    /// <typeparam name="TResult">The type of the accumulated result.</typeparam>
    /// <param name="source">The source sequence</param>
    /// <param name="seed1">The seed value for the first accumulator.</param>
    /// <param name="accumulator1">The first accumulator.</param>
    /// <param name="seed2">The seed value for the second accumulator.</param>
    /// <param name="accumulator2">The second accumulator.</param>
    /// <param name="seed3">The seed value for the third accumulator.</param>
    /// <param name="accumulator3">The third accumulator.</param>
    /// <param name="seed4">The seed value for the fourth accumulator.</param>
    /// <param name="accumulator4">The fourth accumulator.</param>
    /// <param name="seed5">The seed value for the fifth accumulator.</param>
    /// <param name="accumulator5">The fifth accumulator.</param>
    /// <param name="seed6">The seed value for the sixth accumulator.</param>
    /// <param name="accumulator6">The sixth accumulator.</param>
    /// <param name="resultSelector">
    /// A function that projects a single result given the result of each
    /// accumulator.</param>
    /// <returns>The value returned by <paramref name="resultSelector"/>.</returns>
    /// <remarks>
    /// This operator executes immediately.
    /// </remarks>

    public static TResult Aggregate<T, TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TAccumulate5, TAccumulate6, TResult>(
        this IEnumerable<T> source,
        TAccumulate1 seed1, Func<TAccumulate1, T, TAccumulate1> accumulator1,
        TAccumulate2 seed2, Func<TAccumulate2, T, TAccumulate2> accumulator2,
        TAccumulate3 seed3, Func<TAccumulate3, T, TAccumulate3> accumulator3,
        TAccumulate4 seed4, Func<TAccumulate4, T, TAccumulate4> accumulator4,
        TAccumulate5 seed5, Func<TAccumulate5, T, TAccumulate5> accumulator5,
        TAccumulate6 seed6, Func<TAccumulate6, T, TAccumulate6> accumulator6,
        Func<TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TAccumulate5, TAccumulate6, TResult> resultSelector)
    {
        source.ThrowIfNull();
        accumulator1.ThrowIfNull();
        accumulator2.ThrowIfNull();
        accumulator3.ThrowIfNull();
        accumulator4.ThrowIfNull();
        accumulator5.ThrowIfNull();
        accumulator6.ThrowIfNull();
        resultSelector.ThrowIfNull();

        var a1 = seed1;
        var a2 = seed2;
        var a3 = seed3;
        var a4 = seed4;
        var a5 = seed5;
        var a6 = seed6;

        foreach (var item in source)
        {
            a1 = accumulator1(a1, item);
            a2 = accumulator2(a2, item);
            a3 = accumulator3(a3, item);
            a4 = accumulator4(a4, item);
            a5 = accumulator5(a5, item);
            a6 = accumulator6(a6, item);
        }

        return resultSelector(a1, a2, a3, a4, a5, a6);
    }

    /// <summary>
    /// Applies seven accumulators sequentially in a single pass over a
    /// sequence.
    /// </summary>
    /// <typeparam name="T">The type of elements in <paramref name="source"/>.</typeparam>
    /// <typeparam name="TAccumulate1">The type of first accumulator value.</typeparam>
    /// <typeparam name="TAccumulate2">The type of second accumulator value.</typeparam>
    /// <typeparam name="TAccumulate3">The type of third accumulator value.</typeparam>
    /// <typeparam name="TAccumulate4">The type of fourth accumulator value.</typeparam>
    /// <typeparam name="TAccumulate5">The type of fifth accumulator value.</typeparam>
    /// <typeparam name="TAccumulate6">The type of sixth accumulator value.</typeparam>
    /// <typeparam name="TAccumulate7">The type of seventh accumulator value.</typeparam>
    /// <typeparam name="TResult">The type of the accumulated result.</typeparam>
    /// <param name="source">The source sequence</param>
    /// <param name="seed1">The seed value for the first accumulator.</param>
    /// <param name="accumulator1">The first accumulator.</param>
    /// <param name="seed2">The seed value for the second accumulator.</param>
    /// <param name="accumulator2">The second accumulator.</param>
    /// <param name="seed3">The seed value for the third accumulator.</param>
    /// <param name="accumulator3">The third accumulator.</param>
    /// <param name="seed4">The seed value for the fourth accumulator.</param>
    /// <param name="accumulator4">The fourth accumulator.</param>
    /// <param name="seed5">The seed value for the fifth accumulator.</param>
    /// <param name="accumulator5">The fifth accumulator.</param>
    /// <param name="seed6">The seed value for the sixth accumulator.</param>
    /// <param name="accumulator6">The sixth accumulator.</param>
    /// <param name="seed7">The seed value for the seventh accumulator.</param>
    /// <param name="accumulator7">The seventh accumulator.</param>
    /// <param name="resultSelector">
    /// A function that projects a single result given the result of each
    /// accumulator.</param>
    /// <returns>The value returned by <paramref name="resultSelector"/>.</returns>
    /// <remarks>
    /// This operator executes immediately.
    /// </remarks>

    public static TResult Aggregate<T, TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TAccumulate5, TAccumulate6, TAccumulate7, TResult>(
        this IEnumerable<T> source,
        TAccumulate1 seed1, Func<TAccumulate1, T, TAccumulate1> accumulator1,
        TAccumulate2 seed2, Func<TAccumulate2, T, TAccumulate2> accumulator2,
        TAccumulate3 seed3, Func<TAccumulate3, T, TAccumulate3> accumulator3,
        TAccumulate4 seed4, Func<TAccumulate4, T, TAccumulate4> accumulator4,
        TAccumulate5 seed5, Func<TAccumulate5, T, TAccumulate5> accumulator5,
        TAccumulate6 seed6, Func<TAccumulate6, T, TAccumulate6> accumulator6,
        TAccumulate7 seed7, Func<TAccumulate7, T, TAccumulate7> accumulator7,
        Func<TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TAccumulate5, TAccumulate6, TAccumulate7, TResult> resultSelector)
    {
        source.ThrowIfNull();
        accumulator1.ThrowIfNull();
        accumulator2.ThrowIfNull();
        accumulator3.ThrowIfNull();
        accumulator4.ThrowIfNull();
        accumulator5.ThrowIfNull();
        accumulator6.ThrowIfNull();
        accumulator7.ThrowIfNull();
        resultSelector.ThrowIfNull();

        var a1 = seed1;
        var a2 = seed2;
        var a3 = seed3;
        var a4 = seed4;
        var a5 = seed5;
        var a6 = seed6;
        var a7 = seed7;

        foreach (var item in source)
        {
            a1 = accumulator1(a1, item);
            a2 = accumulator2(a2, item);
            a3 = accumulator3(a3, item);
            a4 = accumulator4(a4, item);
            a5 = accumulator5(a5, item);
            a6 = accumulator6(a6, item);
            a7 = accumulator7(a7, item);
        }

        return resultSelector(a1, a2, a3, a4, a5, a6, a7);
    }

    /// <summary>
    /// Applies eight accumulators sequentially in a single pass over a
    /// sequence.
    /// </summary>
    /// <typeparam name="T">The type of elements in <paramref name="source"/>.</typeparam>
    /// <typeparam name="TAccumulate1">The type of first accumulator value.</typeparam>
    /// <typeparam name="TAccumulate2">The type of second accumulator value.</typeparam>
    /// <typeparam name="TAccumulate3">The type of third accumulator value.</typeparam>
    /// <typeparam name="TAccumulate4">The type of fourth accumulator value.</typeparam>
    /// <typeparam name="TAccumulate5">The type of fifth accumulator value.</typeparam>
    /// <typeparam name="TAccumulate6">The type of sixth accumulator value.</typeparam>
    /// <typeparam name="TAccumulate7">The type of seventh accumulator value.</typeparam>
    /// <typeparam name="TAccumulate8">The type of eighth accumulator value.</typeparam>
    /// <typeparam name="TResult">The type of the accumulated result.</typeparam>
    /// <param name="source">The source sequence</param>
    /// <param name="seed1">The seed value for the first accumulator.</param>
    /// <param name="accumulator1">The first accumulator.</param>
    /// <param name="seed2">The seed value for the second accumulator.</param>
    /// <param name="accumulator2">The second accumulator.</param>
    /// <param name="seed3">The seed value for the third accumulator.</param>
    /// <param name="accumulator3">The third accumulator.</param>
    /// <param name="seed4">The seed value for the fourth accumulator.</param>
    /// <param name="accumulator4">The fourth accumulator.</param>
    /// <param name="seed5">The seed value for the fifth accumulator.</param>
    /// <param name="accumulator5">The fifth accumulator.</param>
    /// <param name="seed6">The seed value for the sixth accumulator.</param>
    /// <param name="accumulator6">The sixth accumulator.</param>
    /// <param name="seed7">The seed value for the seventh accumulator.</param>
    /// <param name="accumulator7">The seventh accumulator.</param>
    /// <param name="seed8">The seed value for the eighth accumulator.</param>
    /// <param name="accumulator8">The eighth accumulator.</param>
    /// <param name="resultSelector">
    /// A function that projects a single result given the result of each
    /// accumulator.</param>
    /// <returns>The value returned by <paramref name="resultSelector"/>.</returns>
    /// <remarks>
    /// This operator executes immediately.
    /// </remarks>

    public static TResult Aggregate<T, TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TAccumulate5, TAccumulate6, TAccumulate7, TAccumulate8, TResult>(
        this IEnumerable<T> source,
        TAccumulate1 seed1, Func<TAccumulate1, T, TAccumulate1> accumulator1,
        TAccumulate2 seed2, Func<TAccumulate2, T, TAccumulate2> accumulator2,
        TAccumulate3 seed3, Func<TAccumulate3, T, TAccumulate3> accumulator3,
        TAccumulate4 seed4, Func<TAccumulate4, T, TAccumulate4> accumulator4,
        TAccumulate5 seed5, Func<TAccumulate5, T, TAccumulate5> accumulator5,
        TAccumulate6 seed6, Func<TAccumulate6, T, TAccumulate6> accumulator6,
        TAccumulate7 seed7, Func<TAccumulate7, T, TAccumulate7> accumulator7,
        TAccumulate8 seed8, Func<TAccumulate8, T, TAccumulate8> accumulator8,
        Func<TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TAccumulate5, TAccumulate6, TAccumulate7, TAccumulate8, TResult> resultSelector)
    {
        source.ThrowIfNull();
        accumulator1.ThrowIfNull();
        accumulator2.ThrowIfNull();
        accumulator3.ThrowIfNull();
        accumulator4.ThrowIfNull();
        accumulator5.ThrowIfNull();
        accumulator6.ThrowIfNull();
        accumulator7.ThrowIfNull();
        accumulator8.ThrowIfNull();
        resultSelector.ThrowIfNull();

        var a1 = seed1;
        var a2 = seed2;
        var a3 = seed3;
        var a4 = seed4;
        var a5 = seed5;
        var a6 = seed6;
        var a7 = seed7;
        var a8 = seed8;

        foreach (var item in source)
        {
            a1 = accumulator1(a1, item);
            a2 = accumulator2(a2, item);
            a3 = accumulator3(a3, item);
            a4 = accumulator4(a4, item);
            a5 = accumulator5(a5, item);
            a6 = accumulator6(a6, item);
            a7 = accumulator7(a7, item);
            a8 = accumulator8(a8, item);
        }

        return resultSelector(a1, a2, a3, a4, a5, a6, a7, a8);
    }
}
