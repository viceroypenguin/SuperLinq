namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Lazily invokes an action for each value in the sequence.
	/// </summary>
	/// <typeparam name="TSource">
	///	    Source sequence element type.
	/// </typeparam>
	/// <param name="source">
	///	    Source sequence.
	/// </param>
	/// <param name="action">
	///	    Action to invoke for each element.
	/// </param>
	/// <returns>
	///	    Sequence exhibiting the specified side-effects upon enumeration.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="action"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///		This method is a synonym for <see cref="Do{TSource}(IEnumerable{TSource}, Action{TSource})"/>.
	/// </para>
	/// <para>
	///	    This method uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IEnumerable<TSource> Pipe<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(action);

		return Do(source, action);
	}
}
