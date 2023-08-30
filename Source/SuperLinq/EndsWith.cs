namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Determines whether the end of the first sequence is equivalent to the second sequence, using the default
	///     equality comparer.
	/// </summary>
	/// <typeparam name="T">
	///	    Type of elements.
	/// </typeparam>
	/// <param name="first">
	///	    The sequence to check.
	/// </param>
	/// <param name="second">
	///	    The sequence to compare to.
	/// </param>
	/// <returns>
	///	    <see langword="true"/> if <paramref name="first" /> ends with elements equivalent to <paramref name="second"
	///     />.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///		<paramref name="first"/> or <paramref name="second"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	///	    This is the <see cref="IEnumerable{T}" /> equivalent of <see cref="string.EndsWith(string)" /> and it calls
	///     <see cref="IEqualityComparer{T}.Equals(T,T)" /> using <see cref="EqualityComparer{T}.Default" /> on pairs of
	///     elements at the same index.
	/// </remarks>
	public static bool EndsWith<T>(this IEnumerable<T> first, IEnumerable<T> second)
	{
		return EndsWith(first, second, comparer: null);
	}

	/// <summary>
	///	    Determines whether the end of the first sequence is equivalent to the second sequence, using the specified
	///     element equality comparer.
	/// </summary>
	/// <typeparam name="T">
	///	    Type of elements.
	/// </typeparam>
	/// <param name="first">
	///	    The sequence to check.
	/// </param>
	/// <param name="second">
	///	    The sequence to compare to.
	/// </param>
	/// <param name="comparer">
	///	    Equality comparer to use.
	/// </param>
	/// <returns>
	///	    <see langword="true"/> if <paramref name="first" /> ends with elements equivalent to <paramref name="second"
	///     />.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="first"/> or <paramref name="second"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	///	    This is the <see cref="IEnumerable{T}" /> equivalent of <see cref="string.EndsWith(string)" /> and it calls
	///     <see cref="IEqualityComparer{T}.Equals(T,T)" /> using <see cref="EqualityComparer{T}.Default" /> on pairs of
	///     elements at the same index.
	/// </remarks>
	public static bool EndsWith<T>(this IEnumerable<T> first, IEnumerable<T> second, IEqualityComparer<T>? comparer)
	{
		Guard.IsNotNull(first);
		Guard.IsNotNull(second);

		if (first.TryGetCollectionCount() is int firstCount &&
			second.TryGetCollectionCount() is int secondCount &&
			secondCount > firstCount)
		{
			return false;
		}

		comparer ??= EqualityComparer<T>.Default;

		var snd = second.ToList();
		return first.TakeLast(snd.Count)
			.SequenceEqual(snd, comparer);
	}
}
