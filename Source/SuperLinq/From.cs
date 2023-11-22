﻿namespace SuperLinq;

public partial class SuperEnumerable
{
	/// <summary>
	///	    Returns a single-element sequence containing the result of invoking the function.
	/// </summary>
	/// <typeparam name="T">
	///	    The type of the object returned by the function.
	/// </typeparam>
	/// <param name="function">
	///	    The function to evaluate.
	/// </param>
	/// <returns>
	///	    A sequence with the value resulting from invoking <paramref name="function"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="function"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	///	    This operator uses deferred execution and streams the results. If the resulting sequence is enumerated
	///     multiple times, the function will be invoked multiple times too.
	/// </remarks>
	public static IEnumerable<T> From<T>(Func<T> function)
	{
		ArgumentNullException.ThrowIfNull(function);
		return Core(function);

		static IEnumerable<T> Core(Func<T> function)
		{
			yield return function();
		}
	}

	/// <summary>
	///	    Returns a sequence containing the result of invoking each parameter function in order.
	/// </summary>
	/// <typeparam name="T">
	///	    The type of the object returned by the functions.
	/// </typeparam>
	/// <param name="function1">
	///	    The first function to evaluate.
	/// </param>
	/// <param name="function2">
	///	    The second function to evaluate.
	/// </param>
	/// <returns>
	///	    A sequence with the values resulting from invoking <paramref name="function1"/> and <paramref
	///     name="function2"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="function1"/> or <paramref name="function2"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	///	    This operator uses deferred execution and streams the results. If the resulting sequence is enumerated
	///     multiple times, the functions will be invoked multiple times too.
	/// </remarks>
	public static IEnumerable<T> From<T>(Func<T> function1, Func<T> function2)
	{
		ArgumentNullException.ThrowIfNull(function1);
		ArgumentNullException.ThrowIfNull(function2);
		return Core(function1, function2);

		static IEnumerable<T> Core(Func<T> function1, Func<T> function2)
		{
			yield return function1();
			yield return function2();
		}
	}

	/// <summary>
	///	    Returns a sequence containing the result of invoking each parameter function in order.
	/// </summary>
	/// <typeparam name="T">
	///	    The type of the object returned by the functions.
	/// </typeparam>
	/// <param name="function1">
	///	    The first function to evaluate.
	/// </param>
	/// <param name="function2">
	///	    The second function to evaluate.
	/// </param>
	/// <param name="function3">
	///	    The third function to evaluate.
	/// </param>
	/// <returns>
	///	    A sequence with the values resulting from invoking <paramref name="function1"/>, <paramref
	///     name="function2"/> and <paramref name="function3"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="function1"/>, <paramref name="function2"/>, or <paramref name="function3"/> is <see
	///     langword="null"/>.
	/// </exception>
	/// <remarks>
	///	    This operator uses deferred execution and streams the results. If the resulting sequence is enumerated
	///     multiple times, the functions will be invoked multiple times too.
	/// </remarks>
	public static IEnumerable<T> From<T>(Func<T> function1, Func<T> function2, Func<T> function3)
	{
		ArgumentNullException.ThrowIfNull(function1);
		ArgumentNullException.ThrowIfNull(function2);
		ArgumentNullException.ThrowIfNull(function3);
		return Core(function1, function2, function3);

		static IEnumerable<T> Core(Func<T> function1, Func<T> function2, Func<T> function3)
		{
			yield return function1();
			yield return function2();
			yield return function3();
		}
	}

	/// <summary>
	///	    Returns a sequence containing the values resulting from invoking (in order) each function in the source
	///     sequence of functions.
	/// </summary>
	/// <typeparam name="T">
	///	    The type of the object returned by the functions.
	/// </typeparam>
	/// <param name="functions">
	///	    The functions to evaluate.
	/// </param>
	/// <returns>
	///	    A sequence with the values resulting from invoking all of the <paramref name="functions"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="functions"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	///	    This operator uses deferred execution and streams the results. If the resulting sequence is enumerated
	///     multiple times, the functions will be invoked multiple times too.
	/// </remarks>
	public static IEnumerable<T> From<T>(params Func<T>[] functions) =>
		functions.Evaluate();
}
