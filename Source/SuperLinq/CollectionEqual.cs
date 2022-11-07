namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Determines whether two collections are equal by comparing the elements by using
	/// the default equality comparer for their type.
	/// </summary>
	/// <typeparam name="TSource">
	/// The type of the elements of the input sequences.</typeparam>
	/// <param name="first">
	/// An <see cref="IEnumerable{T}"/> to compare to <paramref name="second"/>.
	/// </param>
	/// <param name="second">
	/// An <see cref="IEnumerable{T}"/> to compare to the <paramref name="first"/> sequence.
	/// </param>
	/// <returns>
	/// <see langword="true"/> if the two source sequences are of equal length and their corresponding
	/// elements are equal according to the default equality comparer for their type;
	/// otherwise, <see langword="false"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="first"/> is null.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="second"/> is null.</exception>
	/// <remarks>
	/// <para>
	/// This method uses the default equality comparer for <typeparamref name="TSource"/>, <see cref="EqualityComparer{T}.Default"/>, 
	/// determine whether two sequences have the same collection of elements.
	/// A collection may have more than one of the same element, so this method
	/// will compare the value and count of each element between both sequences.
	/// </para>
	/// <para>
	/// This method executes immediately.
	/// </para>
	/// </remarks>
	public static bool CollectionEqual<TSource>(
		this IEnumerable<TSource> first,
		IEnumerable<TSource> second)
	{
		return CollectionEqual(first, second, comparer: null);
	}

	/// <summary>
	/// Determines whether two collections are equal by comparing the elements by using
	/// a specified <see cref="IEqualityComparer{T}"/>.
	/// </summary>
	/// <typeparam name="TSource">
	/// The type of the elements of the input sequences.</typeparam>
	/// <param name="first">
	/// An <see cref="IEnumerable{T}"/> to compare to <paramref name="second"/>.
	/// </param>
	/// <param name="second">
	/// An <see cref="IEnumerable{T}"/> to compare to the <paramref name="first"/> sequence.
	/// </param>
	/// <param name="comparer">
	/// An <see cref="IEqualityComparer{T}"/> to use to compare elements.
	/// </param>
	/// <returns>
	/// <see langword="true"/> if the two source sequences are of equal length and their corresponding
	/// elements are equal according to the default equality comparer for their type;
	/// otherwise, <see langword="false"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="first"/> is null.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="second"/> is null.</exception>
	/// <remarks>
	/// <para>
	/// This method uses the provided equality comparer for <typeparamref name="TSource"/> to
	/// determine whether two sequences have the same collection of elements.
	/// A collection may have more than one of the same element, so this method
	/// will compare the value and count of each element between both sequences.
	/// If <paramref name="comparer"/> is <see langword="null"/>, the default equality comparer,
	/// <see cref="EqualityComparer{T}.Default"/>, is used.
	/// </para>
	/// <para>
	/// This method executes immediately.
	/// </para>
	/// </remarks>
	public static bool CollectionEqual<TSource>(
		this IEnumerable<TSource> first,
		IEnumerable<TSource> second,
		IEqualityComparer<TSource>? comparer)
	{
		Guard.IsNotNull(first);
		Guard.IsNotNull(second);

		var cmp = ValueTupleEqualityComparer.Create<TSource, int>(comparer, comparer2: null);
		var firstSet = first.CountBy(Identity, comparer)
			.ToHashSet(cmp);
		var secondSet = second.CountBy(Identity, comparer);
		return firstSet.SetEquals(secondSet);
	}
}
