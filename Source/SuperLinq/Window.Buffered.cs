namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Processes a sequence into a series of subsequences representing a windowed subset of the original, and then
	/// projecting them into a new form.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements of <paramref name="selector"/>.</typeparam>
	/// <typeparam name="TResult">The type of the value return by <paramref name="selector"/>.</typeparam>
	/// <param name="source">The sequence to evaluate a sliding window over.</param>
	/// <param name="size">The size (number of elements) in each window.</param>
	/// <param name="selector">A transform function to apply to each window.</param>
	/// <returns>An <see cref="IEnumerable{T}"/> whose elements are the result of invoking the transform function on
	/// each window of <paramref name="source"/>.</returns>
	/// <remarks>
	/// <para>
	/// The number of sequences returned is: <c>Max(0, <paramref name="source"/>.Count() - <paramref name="size"/> +
	/// 1)</c><br/> Returned subsequences are buffered, but the overall operation is streamed.<br/>
	/// </para>
	/// <para>
	/// In this overload of <c>Window</c>, a single array of length <paramref name="size"/> is allocated as a buffer for
	/// all subsequences.</para>
	/// </remarks>
	public static IEnumerable<TResult> Window<TSource, TResult>(
		this IEnumerable<TSource> source,
		int size,
		Func<IReadOnlyList<TSource>, TResult> selector)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(selector);
		Guard.IsGreaterThanOrEqualTo(size, 1);

		return WindowImpl(source, new TSource[size], size, WindowType.Normal, selector);
	}

	/// <summary>
	/// Processes a sequence into a series of subsequences representing a windowed subset of the original, and then
	/// projecting them into a new form.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements of <paramref name="selector"/>.</typeparam>
	/// <typeparam name="TResult">The type of the value return by <paramref name="selector"/>.</typeparam>
	/// <param name="source">The sequence to evaluate a sliding window over.</param>
	/// <param name="array">An array to use as a buffer for each subsequence.</param>
	/// <param name="selector">A transform function to apply to each window.</param>
	/// <returns>An <see cref="IEnumerable{T}"/> whose elements are the result of invoking the transform function on
	/// each window of <paramref name="source"/>.</returns>
	/// <remarks>
	/// <para>
	/// The number of sequences returned is: <c>Max(0, <paramref name="source"/>.Count() - <paramref
	/// name="array"/>.Length + 1)</c><br/> Returned subsequences are buffered, but the overall operation is
	/// streamed.<br/>
	/// </para>
	/// <para>
	/// In this overload of <c>Window</c>, <paramref name="array"/> is used as a common buffer for all
	/// subsequences.
	/// </para>
	/// </remarks>
	public static IEnumerable<TResult> Window<TSource, TResult>(
		this IEnumerable<TSource> source,
		TSource[] array,
		Func<IReadOnlyList<TSource>, TResult> selector)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(array);
		ArgumentNullException.ThrowIfNull(selector);

		return WindowImpl(source, array, array.Length, WindowType.Normal, selector);
	}

	/// <summary>
	/// Processes a sequence into a series of subsequences representing a windowed subset of the original, and then
	/// projecting them into a new form.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements of <paramref name="selector"/>.</typeparam>
	/// <typeparam name="TResult">The type of the value return by <paramref name="selector"/>.</typeparam>
	/// <param name="source">The sequence to evaluate a sliding window over.</param>
	/// <param name="array">An array to use as a buffer for each subsequence.</param>
	/// <param name="size">The size (number of elements) in each window.</param>
	/// <param name="selector">A transform function to apply to each window.</param>
	/// <returns>An <see cref="IEnumerable{T}"/> whose elements are the result of invoking the transform function on
	/// each window of <paramref name="source"/>.</returns>
	/// <remarks>
	/// <para>
	/// The number of sequences returned is: <c>Max(0, <paramref name="source"/>.Count() - <paramref name="size" /> +
	/// 1)</c><br/> Returned subsequences are buffered, but the overall operation is streamed.<br/>
	/// </para>
	/// <para>
	/// In this overload of <c>Window</c>, <paramref name="array"/> is used as a common buffer for all subsequences.
	/// </para>
	/// <para>
	/// This overload is provided to ease usage of common buffers, such as those rented from <see
	/// cref="System.Buffers.ArrayPool{T}"/>, which may return an array larger than requested.
	/// </para>
	/// </remarks>
	public static IEnumerable<TResult> Window<TSource, TResult>(
		this IEnumerable<TSource> source,
		TSource[] array,
		int size,
		Func<IReadOnlyList<TSource>, TResult> selector)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(array);
		ArgumentNullException.ThrowIfNull(selector);
		Guard.IsBetweenOrEqualTo(size, 1, array.Length);

		return WindowImpl(source, array, size, WindowType.Normal, selector);
	}
}
