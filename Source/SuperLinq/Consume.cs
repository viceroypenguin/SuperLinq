namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Completely consumes the given sequence.
	/// </summary>
	/// <typeparam name="T">
	///	    Element type of the sequence.
	///	</typeparam>
	/// <param name="source">
	///	    Source to consume.
	///	</param>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> is <see langword="null"/>.
	///	</exception>
	/// <remarks>
	/// <para>
	///	    The purpose of this method is to execute the operators for the provided <see cref="IEnumerable{T}"/>, in the
	///	    event that the operators have side-effects. 
	/// </para>
	/// <para>
	///	    This method executes immediately.
	/// </para>
	/// </remarks>
	public static void Consume<T>(this IEnumerable<T> source)
	{
		ArgumentNullException.ThrowIfNull(source);
		foreach (var _ in source)
		{
		}
	}
}
