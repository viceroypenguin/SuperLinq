namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Determines whether two collections are equal by comparing the elements by using
	/// the default equality comparer for their type.
	/// </summary>
	/// <typeparam name="TSource">
	/// The type of the elements of the input sequences.</typeparam>
	/// <param name="first">
	/// An <see cref="IAsyncEnumerable{T}"/> to compare to <paramref name="second"/>.
	/// </param>
	/// <param name="second">
	/// An <see cref="IAsyncEnumerable{T}"/> to compare to the <paramref name="first"/> sequence.
	/// </param>
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
	/// <returns>
	/// <see langword="true"/> if the two source sequences are of equal length and their corresponding
	/// elements are equal according to the default equality comparer for their type;
	/// otherwise, <see langword="false"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="first"/> is null.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="second"/> is null.</exception>
	/// <remarks>
	/// <para>
	/// This method uses the default equality comparer for <typeparamref name="TSource"/>, <see cref="EqualityComparer{T}.Default"/>, to
	/// build a <see cref="HashSet{T}" /> of the items from <paramref name="first"/>; and 
	/// compares the collection to <paramref name="second"/> using <see cref="HashSet{T}.SetEquals(IEnumerable{T})"/>.
	/// </para>
	/// <para>
	/// This method executes immediately.
	/// </para>
	/// </remarks>
	public static ValueTask<bool> CollectionEqual<TSource>(
		this IAsyncEnumerable<TSource> first,
		IAsyncEnumerable<TSource> second,
		CancellationToken cancellationToken = default)
	{
		return CollectionEqual(first, second, comparer: null, cancellationToken);
	}

	/// <summary>
	/// Determines whether two collections are equal by comparing the elements by using
	///  a specified <see cref="IEqualityComparer{T}"/>.
	/// </summary>
	/// <typeparam name="TSource">
	/// The type of the elements of the input sequences.</typeparam>
	/// <param name="first">
	/// An <see cref="IAsyncEnumerable{T}"/> to compare to <paramref name="second"/>.
	/// </param>
	/// <param name="second">
	/// An <see cref="IAsyncEnumerable{T}"/> to compare to the <paramref name="first"/> sequence.
	/// </param>
	/// <param name="comparer">
	/// An <see cref="IEqualityComparer{T}"/> to use to compare elements.
	/// </param>
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
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
	/// build a <see cref="HashSet{T}" /> of the items from <paramref name="first"/>; and 
	/// compares the collection to <paramref name="second"/> using <see cref="HashSet{T}.SetEquals(IEnumerable{T})"/>.
	/// If <paramref name="comparer"/> is <see langword="null"/>, the default equality comparer,
	/// <see cref="EqualityComparer{T}.Default"/>, is used.
	/// </para>
	/// <para>
	/// This method executes immediately.
	/// </para>
	/// </remarks>
	public static ValueTask<bool> CollectionEqual<TSource>(
		this IAsyncEnumerable<TSource> first,
		IAsyncEnumerable<TSource> second,
		IEqualityComparer<TSource>? comparer,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(first);
		ArgumentNullException.ThrowIfNull(second);

		return Core(first, second, comparer, cancellationToken);

		static async ValueTask<bool> Core(
			IAsyncEnumerable<TSource> first,
			IAsyncEnumerable<TSource> second,
			IEqualityComparer<TSource>? comparer,
			CancellationToken cancellationToken = default)
		{
			var firstSet = await first.CountBy(Identity, comparer)
				.ToHashSetAsync(ValueTupleEqualityComparer.Create<TSource, int>(comparer, comparer2: null), cancellationToken)
				.ConfigureAwait(false);
			var secondSet = await second.CountBy(Identity, comparer)
				.ToListAsync(cancellationToken)
				.ConfigureAwait(false);
			return firstSet.SetEquals(secondSet);
		}
	}
}
