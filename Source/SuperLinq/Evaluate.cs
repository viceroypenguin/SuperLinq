namespace SuperLinq;

public partial class SuperEnumerable
{
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
	///	    A sequence with results from invoking each function in the <paramref name="functions"/> sequence.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="functions"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	///	    This operator uses deferred execution and streams the results. If the resulting sequence is enumerated
	///     multiple times, the functions will be evaluated multiple times too.
	/// </remarks>
	public static IEnumerable<T> Evaluate<T>(this IEnumerable<Func<T>> functions)
	{
		ArgumentNullException.ThrowIfNull(functions);
		return functions.Select(f => f());
	}
}
