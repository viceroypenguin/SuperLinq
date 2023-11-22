namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Creates a sequence whose termination or disposal of an enumerator causes a finally action to be executed.
	/// </summary>
	/// <typeparam name="TSource">
	///	    Source sequence element type.
	/// </typeparam>
	/// <param name="source">
	///	    Source sequence.
	/// </param>
	/// <param name="finallyAction">
	///	    Action to run upon termination of the sequence, or when an enumerator is disposed.
	/// </param>
	/// <returns>
	///	    Source sequence with guarantees on the invocation of the finally action.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="finallyAction"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This method uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IEnumerable<TSource> Finally<TSource>(this IEnumerable<TSource> source, Action finallyAction)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(finallyAction);

		return Core(source, finallyAction);

		static IEnumerable<TSource> Core(IEnumerable<TSource> source, Action finallyAction)
		{
			try
			{
				foreach (var item in source)
					yield return item;
			}
			finally
			{
				finallyAction();
			}
		}
	}
}
