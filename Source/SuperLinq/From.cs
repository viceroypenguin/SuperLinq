namespace SuperLinq;

partial class SuperEnumerable
{
	/// <summary>
	/// Returns a single-element sequence containing the result of invoking the function.
	/// </summary>
	/// <remarks>
	/// This operator uses deferred execution and streams the results.
	/// If the resulting sequence is enumerated multiple times, the function will be
	/// invoked multiple times too.
	/// </remarks>
	/// <typeparam name="T">The type of the object returned by the function.</typeparam>
	/// <param name="function">The function to evaluate.</param>
	/// <returns>A sequence with the value resulting from invoking <paramref name="function"/>.</returns>

	public static IEnumerable<T> From<T>(Func<T> function)
	{
		return _(); IEnumerable<T> _()
		{
			yield return function();
		}
	}

	/// <summary>
	/// Returns a sequence containing the result of invoking each parameter function in order.
	/// </summary>
	/// <remarks>
	/// This operator uses deferred execution and streams the results.
	/// If the resulting sequence is enumerated multiple times, the functions will be
	/// invoked multiple times too.
	/// </remarks>
	/// <typeparam name="T">The type of the object returned by the functions.</typeparam>
	/// <param name="function1">The first function to evaluate.</param>
	/// <param name="function2">The second function to evaluate.</param>
	/// <returns>A sequence with the values resulting from invoking <paramref name="function1"/> and <paramref name="function2"/>.</returns>

	public static IEnumerable<T> From<T>(Func<T> function1, Func<T> function2)
	{
		return _(); IEnumerable<T> _()
		{
			yield return function1();
			yield return function2();
		}
	}

	/// <summary>
	/// Returns a sequence containing the result of invoking each parameter function in order.
	/// </summary>
	/// <remarks>
	/// This operator uses deferred execution and streams the results.
	/// If the resulting sequence is enumerated multiple times, the functions will be
	/// invoked multiple times too.
	/// </remarks>
	/// <typeparam name="T">The type of the object returned by the functions.</typeparam>
	/// <param name="function1">The first function to evaluate.</param>
	/// <param name="function2">The second function to evaluate.</param>
	/// <param name="function3">The third function to evaluate.</param>
	/// <returns>A sequence with the values resulting from invoking <paramref name="function1"/>, <paramref name="function2"/> and <paramref name="function3"/>.</returns>

	public static IEnumerable<T> From<T>(Func<T> function1, Func<T> function2, Func<T> function3)
	{
		return _(); IEnumerable<T> _()
		{
			yield return function1();
			yield return function2();
			yield return function3();
		}
	}

	/// <summary>
	/// Returns a sequence containing the values resulting from invoking (in order) each function in the source sequence of functions.
	/// </summary>
	/// <remarks>
	/// This operator uses deferred execution and streams the results.
	/// If the resulting sequence is enumerated multiple times, the functions will be
	/// invoked multiple times too.
	/// </remarks>
	/// <typeparam name="T">The type of the object returned by the functions.</typeparam>
	/// <param name="functions">The functions to evaluate.</param>
	/// <returns>A sequence with the values resulting from invoking all of the <paramref name="functions"/>.</returns>
	/// <exception cref="ArgumentNullException">When <paramref name="functions"/> is <c>null</c>.</exception>

	public static IEnumerable<T> From<T>(params Func<T>[] functions)
	{
		functions.ThrowIfNull();
		return Evaluate(functions);
	}
}
