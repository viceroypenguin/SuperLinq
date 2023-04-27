﻿namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Returns a sequence generated by applying a state to the generator function,
	/// and from its result, determines if the sequence should have a next element, its value,
	/// and the next state in the recursive call.
	/// </summary>
	/// <typeparam name="TState">Type of state elements.</typeparam>
	/// <typeparam name="T">Type of the elements generated by the generator function.</typeparam>
	/// <typeparam name="TResult">The type of the elements of the result sequence.</typeparam>
	/// <param name="state">The initial state.</param>
	/// <param name="generator">
	/// Function that takes a state and computes the next state and the next element of the sequence.
	/// </param>
	/// <param name="predicate">
	/// Function to determine if the unfolding should continue based the
	/// result of the <paramref name="generator"/> function.
	/// </param>
	/// <param name="stateSelector">
	/// Function to select the state from the output of the <paramref name="generator"/> function.
	/// </param>
	/// <param name="resultSelector">
	/// Function to select the result from the output of the <paramref name="generator"/> function.
	/// </param>
	/// <returns>A sequence containing the results generated by the <paramref name="resultSelector"/> function.</returns>
	/// <remarks>
	/// This operator uses deferred execution and streams its results.
	/// </remarks>
	[Obsolete("Will be removed in v6.0.0; better implemented as `SuperEnumerable.Generate().TakeWhile().Select()`")]
	public static IEnumerable<TResult> Unfold<TState, T, TResult>(
		TState state,
		Func<TState, T> generator,
		Func<T, bool> predicate,
		Func<T, TState> stateSelector,
		Func<T, TResult> resultSelector)
	{
		Guard.IsNotNull(generator);
		Guard.IsNotNull(predicate);
		Guard.IsNotNull(stateSelector);
		Guard.IsNotNull(resultSelector);

		return Core(state, generator, predicate, stateSelector, resultSelector);

		static IEnumerable<TResult> Core(
			TState state,
			Func<TState, T> generator,
			Func<T, bool> predicate,
			Func<T, TState> stateSelector,
			Func<T, TResult> resultSelector)
		{
			while (true)
			{
				var step = generator(state);

				if (!predicate(step))
					yield break;

				yield return resultSelector(step);
				state = stateSelector(step);
			}
		}
	}
}
