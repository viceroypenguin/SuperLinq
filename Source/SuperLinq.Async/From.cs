using System.Runtime.CompilerServices;

namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
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

	public static IAsyncEnumerable<T> From<T>(Func<Task<T>> function)
	{
		function.ThrowIfNull();
		return _(function);

		static async IAsyncEnumerable<T> _(Func<Task<T>> function, [EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			cancellationToken.ThrowIfCancellationRequested();
			yield return await function().ConfigureAwait(false);
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

	public static IAsyncEnumerable<T> From<T>(Func<Task<T>> function1, Func<Task<T>> function2)
	{
		function1.ThrowIfNull();
		function2.ThrowIfNull();
		return _(function1, function2);

		static async IAsyncEnumerable<T> _(Func<Task<T>> function1, Func<Task<T>> function2, [EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			cancellationToken.ThrowIfCancellationRequested();
			yield return await function1().ConfigureAwait(false);
			cancellationToken.ThrowIfCancellationRequested();
			yield return await function2().ConfigureAwait(false);
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

	public static IAsyncEnumerable<T> From<T>(Func<Task<T>> function1, Func<Task<T>> function2, Func<Task<T>> function3)
	{
		function1.ThrowIfNull();
		function2.ThrowIfNull();
		function3.ThrowIfNull();
		return _(function1, function2, function3);

		static async IAsyncEnumerable<T> _(Func<Task<T>> function1, Func<Task<T>> function2, Func<Task<T>> function3, [EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			cancellationToken.ThrowIfCancellationRequested();
			yield return await function1().ConfigureAwait(false);
			cancellationToken.ThrowIfCancellationRequested();
			yield return await function2().ConfigureAwait(false);
			cancellationToken.ThrowIfCancellationRequested();
			yield return await function3().ConfigureAwait(false);
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

	public static IAsyncEnumerable<T> From<T>(params Func<Task<T>>[] functions) =>
		Evaluate(functions);

	/// <summary>
	/// Returns a sequence containing the values resulting from invoking (in order) each function in the source sequence of functions.
	/// </summary>
	/// <remarks>
	/// This operator uses deferred execution and streams the results.
	/// If the resulting sequence is enumerated multiple times, the functions will be
	/// evaluated multiple times too.
	/// </remarks>
	/// <typeparam name="T">The type of the object returned by the functions.</typeparam>
	/// <param name="functions">The functions to evaluate.</param>
	/// <returns>A sequence with results from invoking <paramref name="functions"/>.</returns>
	/// <exception cref="ArgumentNullException">When <paramref name="functions"/> is <c>null</c>.</exception>

	public static IAsyncEnumerable<T> Evaluate<T>(this IEnumerable<Func<Task<T>>> functions)
	{
		functions.ThrowIfNull();
		return _(functions);

		static async IAsyncEnumerable<T> _(IEnumerable<Func<Task<T>>> functions, [EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			foreach (var f in functions)
			{
				cancellationToken.ThrowIfCancellationRequested();
				yield return await f().ConfigureAwait(false);
			}
		}
	}

	/// <summary>
	/// Returns a sequence containing the values resulting from invoking (in order) each function in the source sequence of functions.
	/// </summary>
	/// <remarks>
	/// This operator uses deferred execution and streams the results.
	/// If the resulting sequence is enumerated multiple times, the functions will be
	/// evaluated multiple times too.
	/// </remarks>
	/// <typeparam name="T">The type of the object returned by the functions.</typeparam>
	/// <param name="functions">The functions to evaluate.</param>
	/// <returns>A sequence with results from invoking <paramref name="functions"/>.</returns>
	/// <exception cref="ArgumentNullException">When <paramref name="functions"/> is <c>null</c>.</exception>

	public static IAsyncEnumerable<T> Evaluate<T>(this IAsyncEnumerable<Func<Task<T>>> functions)
	{
		functions.ThrowIfNull();
		return _(functions);

		static async IAsyncEnumerable<T> _(IAsyncEnumerable<Func<Task<T>>> functions, [EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			await foreach (var f in functions.WithCancellation(cancellationToken).ConfigureAwait(false))
			{
				yield return await f().ConfigureAwait(false);
			}
		}
	}
}
