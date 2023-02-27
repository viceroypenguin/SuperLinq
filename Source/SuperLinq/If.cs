namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Returns an enumerable sequence if the evaluation result of the given condition is <see langword="true"/>,
	/// otherwise returns an empty sequence.
	/// </summary>
	/// <typeparam name="TResult">Result sequence element type.</typeparam>
	/// <param name="condition">Condition to evaluate.</param>
	/// <param name="thenSource">Sequence to return in case the condition evaluates true.</param>
	/// <returns>The given input sequence if the condition evaluates true; otherwise, an empty sequence.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="condition"/> or <paramref name="thenSource"/> is <see
	/// langword="null"/>.</exception>
	/// <remarks>
	/// <para>
	/// <paramref name="condition"/> is not evaluated until enumeration. If the value is <see langword="true"/>, then
	/// <paramref name="thenSource"/> is enumerated. Otherwise, the sequence will be empty.
	/// </para>
	/// </remarks>
	public static IEnumerable<TResult> If<TResult>(Func<bool> condition, IEnumerable<TResult> thenSource)
	{
		return If(condition, thenSource, Enumerable.Empty<TResult>());
	}

	/// <summary>
	/// Returns an enumerable sequence based on the evaluation result of the given condition.
	/// </summary>
	/// <typeparam name="TResult">Result sequence element type.</typeparam>
	/// <param name="condition">Condition to evaluate.</param>
	/// <param name="thenSource">Sequence to return in case the condition evaluates <see langword="true"/>.</param>
	/// <param name="elseSource">Sequence to return in case the condition evaluates <see langword="false"/>.</param>
	/// <returns>Either of the two input sequences based on the result of evaluating the condition.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="condition"/>, <paramref name="thenSource"/>, or
	/// <paramref name="elseSource"/> is <see langword="null"/>.</exception>
	/// <remarks>
	/// <para>
	/// <paramref name="condition"/> is not evaluated until enumeration. If the value is <see langword="true"/>, then
	/// <paramref name="thenSource"/> is enumerated. Otherwise, <paramref name="elseSource"/> will be enumerated.
	/// </para>
	/// </remarks>
	public static IEnumerable<TResult> If<TResult>(
		Func<bool> condition,
		IEnumerable<TResult> thenSource,
		IEnumerable<TResult> elseSource)
	{
		Guard.IsNotNull(condition);
		Guard.IsNotNull(thenSource);
		Guard.IsNotNull(elseSource);

		return Case(
			condition,
			new Dictionary<bool, IEnumerable<TResult>>
			{
				[true] = thenSource,
				[false] = elseSource,
			});
	}
}
