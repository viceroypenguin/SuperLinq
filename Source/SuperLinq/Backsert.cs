namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Inserts the elements of a sequence into another sequence at a specified index from the tail of the sequence,
	///     where zero always represents the last position, one represents the second-last element, two represents the
	///     third-last element and so on.
	/// </summary>
	/// <typeparam name="T">
	///	    Type of elements in all sequences.
	/// </typeparam>
	/// <param name="first">
	///	    The source sequence.
	///	</param>
	/// <param name="second">
	///	    The sequence that will be inserted.
	///	</param>
	/// <param name="index">
	///	    The zero-based index from the end of <paramref name="first"/> where elements from <paramref name="second"/>
	///     should be inserted. <paramref name="second"/>.
	/// </param>
	/// <returns>
	///	    A sequence that contains the elements of <paramref name="first"/> plus the elements of <paramref
	///     name="second"/> inserted at the given index from the end of <paramref name="first"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="first" /> or <paramref name="second" /> is <see langword="null" />.
	///	</exception>
	/// <exception cref="ArgumentOutOfRangeException">
	///	    <paramref name="index"/> is negative.
	/// </exception>
	/// <exception cref="ArgumentOutOfRangeException">
	///	    Thrown lazily if <paramref name="index"/> is greater than the length of <paramref name="first"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///		This operator uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	[Obsolete("Backsert has been replaced by Insert(second, Index index)")]
	public static IEnumerable<T> Backsert<T>(this IEnumerable<T> first, IEnumerable<T> second, int index)
	{
		return first.Insert(second, ^index);
	}
}
