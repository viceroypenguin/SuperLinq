namespace SuperLinq;

#nullable enable

public static partial class SuperEnumerable
{
	/// <summary>
	/// Applies two accumulators sequentially in a single pass over a
	/// sequence.
	/// </summary>
	/// <typeparam name="T">The type of elements in <paramref name="source"/>.</typeparam>
	/// <typeparam name="TResult">The type of the accumulated result.</typeparam>
	/// <param name="source">The source sequence</param>
	/// <param name="resultSelector">
	/// A function that projects a single result given the result of each accumulator.
	/// </param>
	/// <returns>The value returned by <paramref name="resultSelector"/>.</returns>
	/// <remarks>
	/// This operator executes immediately.
	/// </remarks>
	/// <exception cref="global::System.ArgumentNullException">
	/// <paramref name="source"/>, <paramref name="resultSelector"/> or any of the accumulator functions is null.
	/// </exception>
	/// <typeparam name="TAccumulate1">The type of the first accumulator value.</typeparam>
	/// <param name="seed1">The seed value for the first accumulator.</param>
	/// <param name="accumulator1">The first accumulator.</param>
	/// <typeparam name="TAccumulate2">The type of the second accumulator value.</typeparam>
	/// <param name="seed2">The seed value for the second accumulator.</param>
	/// <param name="accumulator2">The second accumulator.</param>
	public static TResult Aggregate<T, TAccumulate1, TAccumulate2, TResult>(
		this global::System.Collections.Generic.IEnumerable<T> source,
		TAccumulate1 seed1, global::System.Func<TAccumulate1, T, TAccumulate1> accumulator1,
		TAccumulate2 seed2, global::System.Func<TAccumulate2, T, TAccumulate2> accumulator2,
		global::System.Func<TAccumulate1, TAccumulate2, TResult> resultSelector
	)
	{
		ArgumentNullException.ThrowIfNull(source);

		ArgumentNullException.ThrowIfNull(accumulator1);
		ArgumentNullException.ThrowIfNull(accumulator2);

		ArgumentNullException.ThrowIfNull(resultSelector);

		foreach (var item in source)
		{
			seed1 = accumulator1(seed1, item);
			seed2 = accumulator2(seed2, item);
		}

		return resultSelector(
			seed1,
			seed2
		);
	}

	/// <summary>
	/// Applies two accumulators sequentially in a single pass over a
	/// sequence.
	/// </summary>
	/// <typeparam name="T">The type of elements in <paramref name="source"/>.</typeparam>
	/// <param name="source">The source sequence</param>
	/// <returns>A 
	/// <see cref="global::System.ValueTuple{T1,T2 }" /> 
	/// containing the result of each accumulator.</returns>
	/// <remarks>
	/// This operator executes immediately.
	/// </remarks>
	/// <exception cref="global::System.ArgumentNullException">
	/// <paramref name="source"/> or any of the accumulator functions is null.
	/// </exception>
	/// <typeparam name="TAccumulate1">The type of the first accumulator value.</typeparam>
	/// <param name="seed1">The seed value for the first accumulator.</param>
	/// <param name="accumulator1">The first accumulator.</param>
	/// <typeparam name="TAccumulate2">The type of the second accumulator value.</typeparam>
	/// <param name="seed2">The seed value for the second accumulator.</param>
	/// <param name="accumulator2">The second accumulator.</param>
	public static (TAccumulate1,TAccumulate2) Aggregate<T, TAccumulate1, TAccumulate2>(
		this global::System.Collections.Generic.IEnumerable<T> source,
		TAccumulate1 seed1, global::System.Func<TAccumulate1, T, TAccumulate1> accumulator1,
		TAccumulate2 seed2, global::System.Func<TAccumulate2, T, TAccumulate2> accumulator2
	) => Aggregate(source,seed1, accumulator1, seed2, accumulator2, global::System.ValueTuple.Create);

	/// <summary>
	/// Applies three accumulators sequentially in a single pass over a
	/// sequence.
	/// </summary>
	/// <typeparam name="T">The type of elements in <paramref name="source"/>.</typeparam>
	/// <typeparam name="TResult">The type of the accumulated result.</typeparam>
	/// <param name="source">The source sequence</param>
	/// <param name="resultSelector">
	/// A function that projects a single result given the result of each accumulator.
	/// </param>
	/// <returns>The value returned by <paramref name="resultSelector"/>.</returns>
	/// <remarks>
	/// This operator executes immediately.
	/// </remarks>
	/// <exception cref="global::System.ArgumentNullException">
	/// <paramref name="source"/>, <paramref name="resultSelector"/> or any of the accumulator functions is null.
	/// </exception>
	/// <typeparam name="TAccumulate1">The type of the first accumulator value.</typeparam>
	/// <param name="seed1">The seed value for the first accumulator.</param>
	/// <param name="accumulator1">The first accumulator.</param>
	/// <typeparam name="TAccumulate2">The type of the second accumulator value.</typeparam>
	/// <param name="seed2">The seed value for the second accumulator.</param>
	/// <param name="accumulator2">The second accumulator.</param>
	/// <typeparam name="TAccumulate3">The type of the third accumulator value.</typeparam>
	/// <param name="seed3">The seed value for the third accumulator.</param>
	/// <param name="accumulator3">The third accumulator.</param>
	public static TResult Aggregate<T, TAccumulate1, TAccumulate2, TAccumulate3, TResult>(
		this global::System.Collections.Generic.IEnumerable<T> source,
		TAccumulate1 seed1, global::System.Func<TAccumulate1, T, TAccumulate1> accumulator1,
		TAccumulate2 seed2, global::System.Func<TAccumulate2, T, TAccumulate2> accumulator2,
		TAccumulate3 seed3, global::System.Func<TAccumulate3, T, TAccumulate3> accumulator3,
		global::System.Func<TAccumulate1, TAccumulate2, TAccumulate3, TResult> resultSelector
	)
	{
		ArgumentNullException.ThrowIfNull(source);

		ArgumentNullException.ThrowIfNull(accumulator1);
		ArgumentNullException.ThrowIfNull(accumulator2);
		ArgumentNullException.ThrowIfNull(accumulator3);

		ArgumentNullException.ThrowIfNull(resultSelector);

		foreach (var item in source)
		{
			seed1 = accumulator1(seed1, item);
			seed2 = accumulator2(seed2, item);
			seed3 = accumulator3(seed3, item);
		}

		return resultSelector(
			seed1,
			seed2,
			seed3
		);
	}

	/// <summary>
	/// Applies three accumulators sequentially in a single pass over a
	/// sequence.
	/// </summary>
	/// <typeparam name="T">The type of elements in <paramref name="source"/>.</typeparam>
	/// <param name="source">The source sequence</param>
	/// <returns>A 
	/// <see cref="global::System.ValueTuple{T1,T2,T3 }" /> 
	/// containing the result of each accumulator.</returns>
	/// <remarks>
	/// This operator executes immediately.
	/// </remarks>
	/// <exception cref="global::System.ArgumentNullException">
	/// <paramref name="source"/> or any of the accumulator functions is null.
	/// </exception>
	/// <typeparam name="TAccumulate1">The type of the first accumulator value.</typeparam>
	/// <param name="seed1">The seed value for the first accumulator.</param>
	/// <param name="accumulator1">The first accumulator.</param>
	/// <typeparam name="TAccumulate2">The type of the second accumulator value.</typeparam>
	/// <param name="seed2">The seed value for the second accumulator.</param>
	/// <param name="accumulator2">The second accumulator.</param>
	/// <typeparam name="TAccumulate3">The type of the third accumulator value.</typeparam>
	/// <param name="seed3">The seed value for the third accumulator.</param>
	/// <param name="accumulator3">The third accumulator.</param>
	public static (TAccumulate1,TAccumulate2,TAccumulate3) Aggregate<T, TAccumulate1, TAccumulate2, TAccumulate3>(
		this global::System.Collections.Generic.IEnumerable<T> source,
		TAccumulate1 seed1, global::System.Func<TAccumulate1, T, TAccumulate1> accumulator1,
		TAccumulate2 seed2, global::System.Func<TAccumulate2, T, TAccumulate2> accumulator2,
		TAccumulate3 seed3, global::System.Func<TAccumulate3, T, TAccumulate3> accumulator3
	) => Aggregate(source,seed1, accumulator1, seed2, accumulator2, seed3, accumulator3, global::System.ValueTuple.Create);

	/// <summary>
	/// Applies four accumulators sequentially in a single pass over a
	/// sequence.
	/// </summary>
	/// <typeparam name="T">The type of elements in <paramref name="source"/>.</typeparam>
	/// <typeparam name="TResult">The type of the accumulated result.</typeparam>
	/// <param name="source">The source sequence</param>
	/// <param name="resultSelector">
	/// A function that projects a single result given the result of each accumulator.
	/// </param>
	/// <returns>The value returned by <paramref name="resultSelector"/>.</returns>
	/// <remarks>
	/// This operator executes immediately.
	/// </remarks>
	/// <exception cref="global::System.ArgumentNullException">
	/// <paramref name="source"/>, <paramref name="resultSelector"/> or any of the accumulator functions is null.
	/// </exception>
	/// <typeparam name="TAccumulate1">The type of the first accumulator value.</typeparam>
	/// <param name="seed1">The seed value for the first accumulator.</param>
	/// <param name="accumulator1">The first accumulator.</param>
	/// <typeparam name="TAccumulate2">The type of the second accumulator value.</typeparam>
	/// <param name="seed2">The seed value for the second accumulator.</param>
	/// <param name="accumulator2">The second accumulator.</param>
	/// <typeparam name="TAccumulate3">The type of the third accumulator value.</typeparam>
	/// <param name="seed3">The seed value for the third accumulator.</param>
	/// <param name="accumulator3">The third accumulator.</param>
	/// <typeparam name="TAccumulate4">The type of the fourth accumulator value.</typeparam>
	/// <param name="seed4">The seed value for the fourth accumulator.</param>
	/// <param name="accumulator4">The fourth accumulator.</param>
	public static TResult Aggregate<T, TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TResult>(
		this global::System.Collections.Generic.IEnumerable<T> source,
		TAccumulate1 seed1, global::System.Func<TAccumulate1, T, TAccumulate1> accumulator1,
		TAccumulate2 seed2, global::System.Func<TAccumulate2, T, TAccumulate2> accumulator2,
		TAccumulate3 seed3, global::System.Func<TAccumulate3, T, TAccumulate3> accumulator3,
		TAccumulate4 seed4, global::System.Func<TAccumulate4, T, TAccumulate4> accumulator4,
		global::System.Func<TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TResult> resultSelector
	)
	{
		ArgumentNullException.ThrowIfNull(source);

		ArgumentNullException.ThrowIfNull(accumulator1);
		ArgumentNullException.ThrowIfNull(accumulator2);
		ArgumentNullException.ThrowIfNull(accumulator3);
		ArgumentNullException.ThrowIfNull(accumulator4);

		ArgumentNullException.ThrowIfNull(resultSelector);

		foreach (var item in source)
		{
			seed1 = accumulator1(seed1, item);
			seed2 = accumulator2(seed2, item);
			seed3 = accumulator3(seed3, item);
			seed4 = accumulator4(seed4, item);
		}

		return resultSelector(
			seed1,
			seed2,
			seed3,
			seed4
		);
	}

	/// <summary>
	/// Applies four accumulators sequentially in a single pass over a
	/// sequence.
	/// </summary>
	/// <typeparam name="T">The type of elements in <paramref name="source"/>.</typeparam>
	/// <param name="source">The source sequence</param>
	/// <returns>A 
	/// <see cref="global::System.ValueTuple{T1,T2,T3,T4 }" /> 
	/// containing the result of each accumulator.</returns>
	/// <remarks>
	/// This operator executes immediately.
	/// </remarks>
	/// <exception cref="global::System.ArgumentNullException">
	/// <paramref name="source"/> or any of the accumulator functions is null.
	/// </exception>
	/// <typeparam name="TAccumulate1">The type of the first accumulator value.</typeparam>
	/// <param name="seed1">The seed value for the first accumulator.</param>
	/// <param name="accumulator1">The first accumulator.</param>
	/// <typeparam name="TAccumulate2">The type of the second accumulator value.</typeparam>
	/// <param name="seed2">The seed value for the second accumulator.</param>
	/// <param name="accumulator2">The second accumulator.</param>
	/// <typeparam name="TAccumulate3">The type of the third accumulator value.</typeparam>
	/// <param name="seed3">The seed value for the third accumulator.</param>
	/// <param name="accumulator3">The third accumulator.</param>
	/// <typeparam name="TAccumulate4">The type of the fourth accumulator value.</typeparam>
	/// <param name="seed4">The seed value for the fourth accumulator.</param>
	/// <param name="accumulator4">The fourth accumulator.</param>
	public static (TAccumulate1,TAccumulate2,TAccumulate3,TAccumulate4) Aggregate<T, TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4>(
		this global::System.Collections.Generic.IEnumerable<T> source,
		TAccumulate1 seed1, global::System.Func<TAccumulate1, T, TAccumulate1> accumulator1,
		TAccumulate2 seed2, global::System.Func<TAccumulate2, T, TAccumulate2> accumulator2,
		TAccumulate3 seed3, global::System.Func<TAccumulate3, T, TAccumulate3> accumulator3,
		TAccumulate4 seed4, global::System.Func<TAccumulate4, T, TAccumulate4> accumulator4
	) => Aggregate(source,seed1, accumulator1, seed2, accumulator2, seed3, accumulator3, seed4, accumulator4, global::System.ValueTuple.Create);

	/// <summary>
	/// Applies five accumulators sequentially in a single pass over a
	/// sequence.
	/// </summary>
	/// <typeparam name="T">The type of elements in <paramref name="source"/>.</typeparam>
	/// <typeparam name="TResult">The type of the accumulated result.</typeparam>
	/// <param name="source">The source sequence</param>
	/// <param name="resultSelector">
	/// A function that projects a single result given the result of each accumulator.
	/// </param>
	/// <returns>The value returned by <paramref name="resultSelector"/>.</returns>
	/// <remarks>
	/// This operator executes immediately.
	/// </remarks>
	/// <exception cref="global::System.ArgumentNullException">
	/// <paramref name="source"/>, <paramref name="resultSelector"/> or any of the accumulator functions is null.
	/// </exception>
	/// <typeparam name="TAccumulate1">The type of the first accumulator value.</typeparam>
	/// <param name="seed1">The seed value for the first accumulator.</param>
	/// <param name="accumulator1">The first accumulator.</param>
	/// <typeparam name="TAccumulate2">The type of the second accumulator value.</typeparam>
	/// <param name="seed2">The seed value for the second accumulator.</param>
	/// <param name="accumulator2">The second accumulator.</param>
	/// <typeparam name="TAccumulate3">The type of the third accumulator value.</typeparam>
	/// <param name="seed3">The seed value for the third accumulator.</param>
	/// <param name="accumulator3">The third accumulator.</param>
	/// <typeparam name="TAccumulate4">The type of the fourth accumulator value.</typeparam>
	/// <param name="seed4">The seed value for the fourth accumulator.</param>
	/// <param name="accumulator4">The fourth accumulator.</param>
	/// <typeparam name="TAccumulate5">The type of the fifth accumulator value.</typeparam>
	/// <param name="seed5">The seed value for the fifth accumulator.</param>
	/// <param name="accumulator5">The fifth accumulator.</param>
	public static TResult Aggregate<T, TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TAccumulate5, TResult>(
		this global::System.Collections.Generic.IEnumerable<T> source,
		TAccumulate1 seed1, global::System.Func<TAccumulate1, T, TAccumulate1> accumulator1,
		TAccumulate2 seed2, global::System.Func<TAccumulate2, T, TAccumulate2> accumulator2,
		TAccumulate3 seed3, global::System.Func<TAccumulate3, T, TAccumulate3> accumulator3,
		TAccumulate4 seed4, global::System.Func<TAccumulate4, T, TAccumulate4> accumulator4,
		TAccumulate5 seed5, global::System.Func<TAccumulate5, T, TAccumulate5> accumulator5,
		global::System.Func<TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TAccumulate5, TResult> resultSelector
	)
	{
		ArgumentNullException.ThrowIfNull(source);

		ArgumentNullException.ThrowIfNull(accumulator1);
		ArgumentNullException.ThrowIfNull(accumulator2);
		ArgumentNullException.ThrowIfNull(accumulator3);
		ArgumentNullException.ThrowIfNull(accumulator4);
		ArgumentNullException.ThrowIfNull(accumulator5);

		ArgumentNullException.ThrowIfNull(resultSelector);

		foreach (var item in source)
		{
			seed1 = accumulator1(seed1, item);
			seed2 = accumulator2(seed2, item);
			seed3 = accumulator3(seed3, item);
			seed4 = accumulator4(seed4, item);
			seed5 = accumulator5(seed5, item);
		}

		return resultSelector(
			seed1,
			seed2,
			seed3,
			seed4,
			seed5
		);
	}

	/// <summary>
	/// Applies five accumulators sequentially in a single pass over a
	/// sequence.
	/// </summary>
	/// <typeparam name="T">The type of elements in <paramref name="source"/>.</typeparam>
	/// <param name="source">The source sequence</param>
	/// <returns>A 
	/// <see cref="global::System.ValueTuple{T1,T2,T3,T4,T5 }" /> 
	/// containing the result of each accumulator.</returns>
	/// <remarks>
	/// This operator executes immediately.
	/// </remarks>
	/// <exception cref="global::System.ArgumentNullException">
	/// <paramref name="source"/> or any of the accumulator functions is null.
	/// </exception>
	/// <typeparam name="TAccumulate1">The type of the first accumulator value.</typeparam>
	/// <param name="seed1">The seed value for the first accumulator.</param>
	/// <param name="accumulator1">The first accumulator.</param>
	/// <typeparam name="TAccumulate2">The type of the second accumulator value.</typeparam>
	/// <param name="seed2">The seed value for the second accumulator.</param>
	/// <param name="accumulator2">The second accumulator.</param>
	/// <typeparam name="TAccumulate3">The type of the third accumulator value.</typeparam>
	/// <param name="seed3">The seed value for the third accumulator.</param>
	/// <param name="accumulator3">The third accumulator.</param>
	/// <typeparam name="TAccumulate4">The type of the fourth accumulator value.</typeparam>
	/// <param name="seed4">The seed value for the fourth accumulator.</param>
	/// <param name="accumulator4">The fourth accumulator.</param>
	/// <typeparam name="TAccumulate5">The type of the fifth accumulator value.</typeparam>
	/// <param name="seed5">The seed value for the fifth accumulator.</param>
	/// <param name="accumulator5">The fifth accumulator.</param>
	public static (TAccumulate1,TAccumulate2,TAccumulate3,TAccumulate4,TAccumulate5) Aggregate<T, TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TAccumulate5>(
		this global::System.Collections.Generic.IEnumerable<T> source,
		TAccumulate1 seed1, global::System.Func<TAccumulate1, T, TAccumulate1> accumulator1,
		TAccumulate2 seed2, global::System.Func<TAccumulate2, T, TAccumulate2> accumulator2,
		TAccumulate3 seed3, global::System.Func<TAccumulate3, T, TAccumulate3> accumulator3,
		TAccumulate4 seed4, global::System.Func<TAccumulate4, T, TAccumulate4> accumulator4,
		TAccumulate5 seed5, global::System.Func<TAccumulate5, T, TAccumulate5> accumulator5
	) => Aggregate(source,seed1, accumulator1, seed2, accumulator2, seed3, accumulator3, seed4, accumulator4, seed5, accumulator5, global::System.ValueTuple.Create);

	/// <summary>
	/// Applies six accumulators sequentially in a single pass over a
	/// sequence.
	/// </summary>
	/// <typeparam name="T">The type of elements in <paramref name="source"/>.</typeparam>
	/// <typeparam name="TResult">The type of the accumulated result.</typeparam>
	/// <param name="source">The source sequence</param>
	/// <param name="resultSelector">
	/// A function that projects a single result given the result of each accumulator.
	/// </param>
	/// <returns>The value returned by <paramref name="resultSelector"/>.</returns>
	/// <remarks>
	/// This operator executes immediately.
	/// </remarks>
	/// <exception cref="global::System.ArgumentNullException">
	/// <paramref name="source"/>, <paramref name="resultSelector"/> or any of the accumulator functions is null.
	/// </exception>
	/// <typeparam name="TAccumulate1">The type of the first accumulator value.</typeparam>
	/// <param name="seed1">The seed value for the first accumulator.</param>
	/// <param name="accumulator1">The first accumulator.</param>
	/// <typeparam name="TAccumulate2">The type of the second accumulator value.</typeparam>
	/// <param name="seed2">The seed value for the second accumulator.</param>
	/// <param name="accumulator2">The second accumulator.</param>
	/// <typeparam name="TAccumulate3">The type of the third accumulator value.</typeparam>
	/// <param name="seed3">The seed value for the third accumulator.</param>
	/// <param name="accumulator3">The third accumulator.</param>
	/// <typeparam name="TAccumulate4">The type of the fourth accumulator value.</typeparam>
	/// <param name="seed4">The seed value for the fourth accumulator.</param>
	/// <param name="accumulator4">The fourth accumulator.</param>
	/// <typeparam name="TAccumulate5">The type of the fifth accumulator value.</typeparam>
	/// <param name="seed5">The seed value for the fifth accumulator.</param>
	/// <param name="accumulator5">The fifth accumulator.</param>
	/// <typeparam name="TAccumulate6">The type of the sixth accumulator value.</typeparam>
	/// <param name="seed6">The seed value for the sixth accumulator.</param>
	/// <param name="accumulator6">The sixth accumulator.</param>
	public static TResult Aggregate<T, TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TAccumulate5, TAccumulate6, TResult>(
		this global::System.Collections.Generic.IEnumerable<T> source,
		TAccumulate1 seed1, global::System.Func<TAccumulate1, T, TAccumulate1> accumulator1,
		TAccumulate2 seed2, global::System.Func<TAccumulate2, T, TAccumulate2> accumulator2,
		TAccumulate3 seed3, global::System.Func<TAccumulate3, T, TAccumulate3> accumulator3,
		TAccumulate4 seed4, global::System.Func<TAccumulate4, T, TAccumulate4> accumulator4,
		TAccumulate5 seed5, global::System.Func<TAccumulate5, T, TAccumulate5> accumulator5,
		TAccumulate6 seed6, global::System.Func<TAccumulate6, T, TAccumulate6> accumulator6,
		global::System.Func<TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TAccumulate5, TAccumulate6, TResult> resultSelector
	)
	{
		ArgumentNullException.ThrowIfNull(source);

		ArgumentNullException.ThrowIfNull(accumulator1);
		ArgumentNullException.ThrowIfNull(accumulator2);
		ArgumentNullException.ThrowIfNull(accumulator3);
		ArgumentNullException.ThrowIfNull(accumulator4);
		ArgumentNullException.ThrowIfNull(accumulator5);
		ArgumentNullException.ThrowIfNull(accumulator6);

		ArgumentNullException.ThrowIfNull(resultSelector);

		foreach (var item in source)
		{
			seed1 = accumulator1(seed1, item);
			seed2 = accumulator2(seed2, item);
			seed3 = accumulator3(seed3, item);
			seed4 = accumulator4(seed4, item);
			seed5 = accumulator5(seed5, item);
			seed6 = accumulator6(seed6, item);
		}

		return resultSelector(
			seed1,
			seed2,
			seed3,
			seed4,
			seed5,
			seed6
		);
	}

	/// <summary>
	/// Applies six accumulators sequentially in a single pass over a
	/// sequence.
	/// </summary>
	/// <typeparam name="T">The type of elements in <paramref name="source"/>.</typeparam>
	/// <param name="source">The source sequence</param>
	/// <returns>A 
	/// <see cref="global::System.ValueTuple{T1,T2,T3,T4,T5,T6 }" /> 
	/// containing the result of each accumulator.</returns>
	/// <remarks>
	/// This operator executes immediately.
	/// </remarks>
	/// <exception cref="global::System.ArgumentNullException">
	/// <paramref name="source"/> or any of the accumulator functions is null.
	/// </exception>
	/// <typeparam name="TAccumulate1">The type of the first accumulator value.</typeparam>
	/// <param name="seed1">The seed value for the first accumulator.</param>
	/// <param name="accumulator1">The first accumulator.</param>
	/// <typeparam name="TAccumulate2">The type of the second accumulator value.</typeparam>
	/// <param name="seed2">The seed value for the second accumulator.</param>
	/// <param name="accumulator2">The second accumulator.</param>
	/// <typeparam name="TAccumulate3">The type of the third accumulator value.</typeparam>
	/// <param name="seed3">The seed value for the third accumulator.</param>
	/// <param name="accumulator3">The third accumulator.</param>
	/// <typeparam name="TAccumulate4">The type of the fourth accumulator value.</typeparam>
	/// <param name="seed4">The seed value for the fourth accumulator.</param>
	/// <param name="accumulator4">The fourth accumulator.</param>
	/// <typeparam name="TAccumulate5">The type of the fifth accumulator value.</typeparam>
	/// <param name="seed5">The seed value for the fifth accumulator.</param>
	/// <param name="accumulator5">The fifth accumulator.</param>
	/// <typeparam name="TAccumulate6">The type of the sixth accumulator value.</typeparam>
	/// <param name="seed6">The seed value for the sixth accumulator.</param>
	/// <param name="accumulator6">The sixth accumulator.</param>
	public static (TAccumulate1,TAccumulate2,TAccumulate3,TAccumulate4,TAccumulate5,TAccumulate6) Aggregate<T, TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TAccumulate5, TAccumulate6>(
		this global::System.Collections.Generic.IEnumerable<T> source,
		TAccumulate1 seed1, global::System.Func<TAccumulate1, T, TAccumulate1> accumulator1,
		TAccumulate2 seed2, global::System.Func<TAccumulate2, T, TAccumulate2> accumulator2,
		TAccumulate3 seed3, global::System.Func<TAccumulate3, T, TAccumulate3> accumulator3,
		TAccumulate4 seed4, global::System.Func<TAccumulate4, T, TAccumulate4> accumulator4,
		TAccumulate5 seed5, global::System.Func<TAccumulate5, T, TAccumulate5> accumulator5,
		TAccumulate6 seed6, global::System.Func<TAccumulate6, T, TAccumulate6> accumulator6
	) => Aggregate(source,seed1, accumulator1, seed2, accumulator2, seed3, accumulator3, seed4, accumulator4, seed5, accumulator5, seed6, accumulator6, global::System.ValueTuple.Create);

	/// <summary>
	/// Applies seven accumulators sequentially in a single pass over a
	/// sequence.
	/// </summary>
	/// <typeparam name="T">The type of elements in <paramref name="source"/>.</typeparam>
	/// <typeparam name="TResult">The type of the accumulated result.</typeparam>
	/// <param name="source">The source sequence</param>
	/// <param name="resultSelector">
	/// A function that projects a single result given the result of each accumulator.
	/// </param>
	/// <returns>The value returned by <paramref name="resultSelector"/>.</returns>
	/// <remarks>
	/// This operator executes immediately.
	/// </remarks>
	/// <exception cref="global::System.ArgumentNullException">
	/// <paramref name="source"/>, <paramref name="resultSelector"/> or any of the accumulator functions is null.
	/// </exception>
	/// <typeparam name="TAccumulate1">The type of the first accumulator value.</typeparam>
	/// <param name="seed1">The seed value for the first accumulator.</param>
	/// <param name="accumulator1">The first accumulator.</param>
	/// <typeparam name="TAccumulate2">The type of the second accumulator value.</typeparam>
	/// <param name="seed2">The seed value for the second accumulator.</param>
	/// <param name="accumulator2">The second accumulator.</param>
	/// <typeparam name="TAccumulate3">The type of the third accumulator value.</typeparam>
	/// <param name="seed3">The seed value for the third accumulator.</param>
	/// <param name="accumulator3">The third accumulator.</param>
	/// <typeparam name="TAccumulate4">The type of the fourth accumulator value.</typeparam>
	/// <param name="seed4">The seed value for the fourth accumulator.</param>
	/// <param name="accumulator4">The fourth accumulator.</param>
	/// <typeparam name="TAccumulate5">The type of the fifth accumulator value.</typeparam>
	/// <param name="seed5">The seed value for the fifth accumulator.</param>
	/// <param name="accumulator5">The fifth accumulator.</param>
	/// <typeparam name="TAccumulate6">The type of the sixth accumulator value.</typeparam>
	/// <param name="seed6">The seed value for the sixth accumulator.</param>
	/// <param name="accumulator6">The sixth accumulator.</param>
	/// <typeparam name="TAccumulate7">The type of the seventh accumulator value.</typeparam>
	/// <param name="seed7">The seed value for the seventh accumulator.</param>
	/// <param name="accumulator7">The seventh accumulator.</param>
	public static TResult Aggregate<T, TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TAccumulate5, TAccumulate6, TAccumulate7, TResult>(
		this global::System.Collections.Generic.IEnumerable<T> source,
		TAccumulate1 seed1, global::System.Func<TAccumulate1, T, TAccumulate1> accumulator1,
		TAccumulate2 seed2, global::System.Func<TAccumulate2, T, TAccumulate2> accumulator2,
		TAccumulate3 seed3, global::System.Func<TAccumulate3, T, TAccumulate3> accumulator3,
		TAccumulate4 seed4, global::System.Func<TAccumulate4, T, TAccumulate4> accumulator4,
		TAccumulate5 seed5, global::System.Func<TAccumulate5, T, TAccumulate5> accumulator5,
		TAccumulate6 seed6, global::System.Func<TAccumulate6, T, TAccumulate6> accumulator6,
		TAccumulate7 seed7, global::System.Func<TAccumulate7, T, TAccumulate7> accumulator7,
		global::System.Func<TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TAccumulate5, TAccumulate6, TAccumulate7, TResult> resultSelector
	)
	{
		ArgumentNullException.ThrowIfNull(source);

		ArgumentNullException.ThrowIfNull(accumulator1);
		ArgumentNullException.ThrowIfNull(accumulator2);
		ArgumentNullException.ThrowIfNull(accumulator3);
		ArgumentNullException.ThrowIfNull(accumulator4);
		ArgumentNullException.ThrowIfNull(accumulator5);
		ArgumentNullException.ThrowIfNull(accumulator6);
		ArgumentNullException.ThrowIfNull(accumulator7);

		ArgumentNullException.ThrowIfNull(resultSelector);

		foreach (var item in source)
		{
			seed1 = accumulator1(seed1, item);
			seed2 = accumulator2(seed2, item);
			seed3 = accumulator3(seed3, item);
			seed4 = accumulator4(seed4, item);
			seed5 = accumulator5(seed5, item);
			seed6 = accumulator6(seed6, item);
			seed7 = accumulator7(seed7, item);
		}

		return resultSelector(
			seed1,
			seed2,
			seed3,
			seed4,
			seed5,
			seed6,
			seed7
		);
	}

	/// <summary>
	/// Applies seven accumulators sequentially in a single pass over a
	/// sequence.
	/// </summary>
	/// <typeparam name="T">The type of elements in <paramref name="source"/>.</typeparam>
	/// <param name="source">The source sequence</param>
	/// <returns>A 
	/// <see cref="global::System.ValueTuple{T1,T2,T3,T4,T5,T6,T7 }" /> 
	/// containing the result of each accumulator.</returns>
	/// <remarks>
	/// This operator executes immediately.
	/// </remarks>
	/// <exception cref="global::System.ArgumentNullException">
	/// <paramref name="source"/> or any of the accumulator functions is null.
	/// </exception>
	/// <typeparam name="TAccumulate1">The type of the first accumulator value.</typeparam>
	/// <param name="seed1">The seed value for the first accumulator.</param>
	/// <param name="accumulator1">The first accumulator.</param>
	/// <typeparam name="TAccumulate2">The type of the second accumulator value.</typeparam>
	/// <param name="seed2">The seed value for the second accumulator.</param>
	/// <param name="accumulator2">The second accumulator.</param>
	/// <typeparam name="TAccumulate3">The type of the third accumulator value.</typeparam>
	/// <param name="seed3">The seed value for the third accumulator.</param>
	/// <param name="accumulator3">The third accumulator.</param>
	/// <typeparam name="TAccumulate4">The type of the fourth accumulator value.</typeparam>
	/// <param name="seed4">The seed value for the fourth accumulator.</param>
	/// <param name="accumulator4">The fourth accumulator.</param>
	/// <typeparam name="TAccumulate5">The type of the fifth accumulator value.</typeparam>
	/// <param name="seed5">The seed value for the fifth accumulator.</param>
	/// <param name="accumulator5">The fifth accumulator.</param>
	/// <typeparam name="TAccumulate6">The type of the sixth accumulator value.</typeparam>
	/// <param name="seed6">The seed value for the sixth accumulator.</param>
	/// <param name="accumulator6">The sixth accumulator.</param>
	/// <typeparam name="TAccumulate7">The type of the seventh accumulator value.</typeparam>
	/// <param name="seed7">The seed value for the seventh accumulator.</param>
	/// <param name="accumulator7">The seventh accumulator.</param>
	public static (TAccumulate1,TAccumulate2,TAccumulate3,TAccumulate4,TAccumulate5,TAccumulate6,TAccumulate7) Aggregate<T, TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TAccumulate5, TAccumulate6, TAccumulate7>(
		this global::System.Collections.Generic.IEnumerable<T> source,
		TAccumulate1 seed1, global::System.Func<TAccumulate1, T, TAccumulate1> accumulator1,
		TAccumulate2 seed2, global::System.Func<TAccumulate2, T, TAccumulate2> accumulator2,
		TAccumulate3 seed3, global::System.Func<TAccumulate3, T, TAccumulate3> accumulator3,
		TAccumulate4 seed4, global::System.Func<TAccumulate4, T, TAccumulate4> accumulator4,
		TAccumulate5 seed5, global::System.Func<TAccumulate5, T, TAccumulate5> accumulator5,
		TAccumulate6 seed6, global::System.Func<TAccumulate6, T, TAccumulate6> accumulator6,
		TAccumulate7 seed7, global::System.Func<TAccumulate7, T, TAccumulate7> accumulator7
	) => Aggregate(source,seed1, accumulator1, seed2, accumulator2, seed3, accumulator3, seed4, accumulator4, seed5, accumulator5, seed6, accumulator6, seed7, accumulator7, global::System.ValueTuple.Create);

	/// <summary>
	/// Applies eight accumulators sequentially in a single pass over a
	/// sequence.
	/// </summary>
	/// <typeparam name="T">The type of elements in <paramref name="source"/>.</typeparam>
	/// <typeparam name="TResult">The type of the accumulated result.</typeparam>
	/// <param name="source">The source sequence</param>
	/// <param name="resultSelector">
	/// A function that projects a single result given the result of each accumulator.
	/// </param>
	/// <returns>The value returned by <paramref name="resultSelector"/>.</returns>
	/// <remarks>
	/// This operator executes immediately.
	/// </remarks>
	/// <exception cref="global::System.ArgumentNullException">
	/// <paramref name="source"/>, <paramref name="resultSelector"/> or any of the accumulator functions is null.
	/// </exception>
	/// <typeparam name="TAccumulate1">The type of the first accumulator value.</typeparam>
	/// <param name="seed1">The seed value for the first accumulator.</param>
	/// <param name="accumulator1">The first accumulator.</param>
	/// <typeparam name="TAccumulate2">The type of the second accumulator value.</typeparam>
	/// <param name="seed2">The seed value for the second accumulator.</param>
	/// <param name="accumulator2">The second accumulator.</param>
	/// <typeparam name="TAccumulate3">The type of the third accumulator value.</typeparam>
	/// <param name="seed3">The seed value for the third accumulator.</param>
	/// <param name="accumulator3">The third accumulator.</param>
	/// <typeparam name="TAccumulate4">The type of the fourth accumulator value.</typeparam>
	/// <param name="seed4">The seed value for the fourth accumulator.</param>
	/// <param name="accumulator4">The fourth accumulator.</param>
	/// <typeparam name="TAccumulate5">The type of the fifth accumulator value.</typeparam>
	/// <param name="seed5">The seed value for the fifth accumulator.</param>
	/// <param name="accumulator5">The fifth accumulator.</param>
	/// <typeparam name="TAccumulate6">The type of the sixth accumulator value.</typeparam>
	/// <param name="seed6">The seed value for the sixth accumulator.</param>
	/// <param name="accumulator6">The sixth accumulator.</param>
	/// <typeparam name="TAccumulate7">The type of the seventh accumulator value.</typeparam>
	/// <param name="seed7">The seed value for the seventh accumulator.</param>
	/// <param name="accumulator7">The seventh accumulator.</param>
	/// <typeparam name="TAccumulate8">The type of the eighth accumulator value.</typeparam>
	/// <param name="seed8">The seed value for the eighth accumulator.</param>
	/// <param name="accumulator8">The eighth accumulator.</param>
	public static TResult Aggregate<T, TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TAccumulate5, TAccumulate6, TAccumulate7, TAccumulate8, TResult>(
		this global::System.Collections.Generic.IEnumerable<T> source,
		TAccumulate1 seed1, global::System.Func<TAccumulate1, T, TAccumulate1> accumulator1,
		TAccumulate2 seed2, global::System.Func<TAccumulate2, T, TAccumulate2> accumulator2,
		TAccumulate3 seed3, global::System.Func<TAccumulate3, T, TAccumulate3> accumulator3,
		TAccumulate4 seed4, global::System.Func<TAccumulate4, T, TAccumulate4> accumulator4,
		TAccumulate5 seed5, global::System.Func<TAccumulate5, T, TAccumulate5> accumulator5,
		TAccumulate6 seed6, global::System.Func<TAccumulate6, T, TAccumulate6> accumulator6,
		TAccumulate7 seed7, global::System.Func<TAccumulate7, T, TAccumulate7> accumulator7,
		TAccumulate8 seed8, global::System.Func<TAccumulate8, T, TAccumulate8> accumulator8,
		global::System.Func<TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TAccumulate5, TAccumulate6, TAccumulate7, TAccumulate8, TResult> resultSelector
	)
	{
		ArgumentNullException.ThrowIfNull(source);

		ArgumentNullException.ThrowIfNull(accumulator1);
		ArgumentNullException.ThrowIfNull(accumulator2);
		ArgumentNullException.ThrowIfNull(accumulator3);
		ArgumentNullException.ThrowIfNull(accumulator4);
		ArgumentNullException.ThrowIfNull(accumulator5);
		ArgumentNullException.ThrowIfNull(accumulator6);
		ArgumentNullException.ThrowIfNull(accumulator7);
		ArgumentNullException.ThrowIfNull(accumulator8);

		ArgumentNullException.ThrowIfNull(resultSelector);

		foreach (var item in source)
		{
			seed1 = accumulator1(seed1, item);
			seed2 = accumulator2(seed2, item);
			seed3 = accumulator3(seed3, item);
			seed4 = accumulator4(seed4, item);
			seed5 = accumulator5(seed5, item);
			seed6 = accumulator6(seed6, item);
			seed7 = accumulator7(seed7, item);
			seed8 = accumulator8(seed8, item);
		}

		return resultSelector(
			seed1,
			seed2,
			seed3,
			seed4,
			seed5,
			seed6,
			seed7,
			seed8
		);
	}

	/// <summary>
	/// Applies eight accumulators sequentially in a single pass over a
	/// sequence.
	/// </summary>
	/// <typeparam name="T">The type of elements in <paramref name="source"/>.</typeparam>
	/// <param name="source">The source sequence</param>
	/// <returns>A 
	/// <see cref="global::System.ValueTuple{T1,T2,T3,T4,T5,T6,T7,T8 }" /> 
	/// containing the result of each accumulator.</returns>
	/// <remarks>
	/// This operator executes immediately.
	/// </remarks>
	/// <exception cref="global::System.ArgumentNullException">
	/// <paramref name="source"/> or any of the accumulator functions is null.
	/// </exception>
	/// <typeparam name="TAccumulate1">The type of the first accumulator value.</typeparam>
	/// <param name="seed1">The seed value for the first accumulator.</param>
	/// <param name="accumulator1">The first accumulator.</param>
	/// <typeparam name="TAccumulate2">The type of the second accumulator value.</typeparam>
	/// <param name="seed2">The seed value for the second accumulator.</param>
	/// <param name="accumulator2">The second accumulator.</param>
	/// <typeparam name="TAccumulate3">The type of the third accumulator value.</typeparam>
	/// <param name="seed3">The seed value for the third accumulator.</param>
	/// <param name="accumulator3">The third accumulator.</param>
	/// <typeparam name="TAccumulate4">The type of the fourth accumulator value.</typeparam>
	/// <param name="seed4">The seed value for the fourth accumulator.</param>
	/// <param name="accumulator4">The fourth accumulator.</param>
	/// <typeparam name="TAccumulate5">The type of the fifth accumulator value.</typeparam>
	/// <param name="seed5">The seed value for the fifth accumulator.</param>
	/// <param name="accumulator5">The fifth accumulator.</param>
	/// <typeparam name="TAccumulate6">The type of the sixth accumulator value.</typeparam>
	/// <param name="seed6">The seed value for the sixth accumulator.</param>
	/// <param name="accumulator6">The sixth accumulator.</param>
	/// <typeparam name="TAccumulate7">The type of the seventh accumulator value.</typeparam>
	/// <param name="seed7">The seed value for the seventh accumulator.</param>
	/// <param name="accumulator7">The seventh accumulator.</param>
	/// <typeparam name="TAccumulate8">The type of the eighth accumulator value.</typeparam>
	/// <param name="seed8">The seed value for the eighth accumulator.</param>
	/// <param name="accumulator8">The eighth accumulator.</param>
	public static (TAccumulate1,TAccumulate2,TAccumulate3,TAccumulate4,TAccumulate5,TAccumulate6,TAccumulate7,TAccumulate8) Aggregate<T, TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TAccumulate5, TAccumulate6, TAccumulate7, TAccumulate8>(
		this global::System.Collections.Generic.IEnumerable<T> source,
		TAccumulate1 seed1, global::System.Func<TAccumulate1, T, TAccumulate1> accumulator1,
		TAccumulate2 seed2, global::System.Func<TAccumulate2, T, TAccumulate2> accumulator2,
		TAccumulate3 seed3, global::System.Func<TAccumulate3, T, TAccumulate3> accumulator3,
		TAccumulate4 seed4, global::System.Func<TAccumulate4, T, TAccumulate4> accumulator4,
		TAccumulate5 seed5, global::System.Func<TAccumulate5, T, TAccumulate5> accumulator5,
		TAccumulate6 seed6, global::System.Func<TAccumulate6, T, TAccumulate6> accumulator6,
		TAccumulate7 seed7, global::System.Func<TAccumulate7, T, TAccumulate7> accumulator7,
		TAccumulate8 seed8, global::System.Func<TAccumulate8, T, TAccumulate8> accumulator8
	) => Aggregate(source,seed1, accumulator1, seed2, accumulator2, seed3, accumulator3, seed4, accumulator4, seed5, accumulator5, seed6, accumulator6, seed7, accumulator7, seed8, accumulator8, global::System.ValueTuple.Create);

}
