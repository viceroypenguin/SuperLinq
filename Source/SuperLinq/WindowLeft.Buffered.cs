namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Creates a left-aligned sliding window over the source sequence of a given size.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
	/// <typeparam name="TResult">The type of the value return by <paramref name="selector"/>.</typeparam>
	/// <param name="source">The sequence over which to create the sliding window.</param>
	/// <param name="size">Size of the sliding window.</param>
	/// <param name="selector">A transform function to apply to each window.</param>
	/// <returns>A sequence representing each sliding window.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is
	/// null.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="size"/> is below 1.</exception>
	/// <remarks>
	/// <para>
	/// A window can contain fewer elements than <paramref name="size"/>, especially as it slides over the start of the
	/// sequence.
	/// </para>
	/// <para>
	/// In this overload of <c>WindowLeft</c>, a single array of length <paramref name="size"/> is allocated as a buffer for
	/// all subsequences.
	/// </para>
	/// <para>
	/// This operator uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	/// <example>
	/// <code><![CDATA[
	/// Console.WriteLine(
	///     Enumerable
	///         .Range(1, 5)
	///         .WindowLeft(3)
	///         .Select(w => "AVG(" + w.ToDelimitedString(",") + ") = " + w.Average())
	///         .ToDelimitedString(Environment.NewLine));
	///
	/// // Output:
	/// // AVG(1,2,3) = 2
	/// // AVG(2,3,4) = 3
	/// // AVG(3,4,5) = 4
	/// // AVG(4,5) = 4.5
	/// // AVG(5) = 5
	/// ]]></code>
	/// </example>
	public static IEnumerable<TResult> WindowLeft<TSource, TResult>(
		this IEnumerable<TSource> source,
		int size,
		Func<IReadOnlyList<TSource>, TResult> selector)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(selector);
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(size);

		return WindowImpl(source, new TSource[size], size, WindowType.Left, selector);
	}

	/// <summary>
	/// Creates a right-aligned sliding window over the source sequence of a given size.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
	/// <typeparam name="TResult">The type of the value return by <paramref name="selector"/>.</typeparam>
	/// <param name="source">The sequence over which to create the sliding window.</param>
	/// <param name="array">An array to use as a buffer for each subsequence.</param>
	/// <param name="selector">A transform function to apply to each window.</param>
	/// <returns>A sequence representing each sliding window.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/>, <paramref name="selector"/>, or <paramref
	/// name="array"/> is null.</exception>
	/// <remarks>
	/// <para>
	/// A window can contain fewer elements than <c><paramref name="array"/>.Length</c>, especially as it slides over
	/// the start of the sequence.
	/// </para>
	/// <para>
	/// In this overload of <c>WindowLeft</c>, <paramref name="array"/> is used as a common buffer for all
	/// subsequences.
	/// </para>
	/// <para>
	/// This operator uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	/// <example>
	/// <code><![CDATA[
	/// Console.WriteLine(
	///     Enumerable
	///         .Range(1, 5)
	///         .WindowLeft(3)
	///         .Select(w => "AVG(" + w.ToDelimitedString(",") + ") = " + w.Average())
	///         .ToDelimitedString(Environment.NewLine));
	///
	/// // Output:
	/// // AVG(1,2,3) = 2
	/// // AVG(2,3,4) = 3
	/// // AVG(3,4,5) = 4
	/// // AVG(4,5) = 4.5
	/// // AVG(5) = 5
	/// ]]></code>
	/// </example>
	public static IEnumerable<TResult> WindowLeft<TSource, TResult>(
		this IEnumerable<TSource> source,
		TSource[] array,
		Func<IReadOnlyList<TSource>, TResult> selector)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(array);
		ArgumentNullException.ThrowIfNull(selector);

		return WindowImpl(source, array, array.Length, WindowType.Left, selector);
	}

	/// <summary>
	/// Creates a right-aligned sliding window over the source sequence of a given size.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
	/// <typeparam name="TResult">The type of the value return by <paramref name="selector"/>.</typeparam>
	/// <param name="source">The sequence over which to create the sliding window.</param>
	/// <param name="size">Size of the sliding window.</param>
	/// <param name="array">An array to use as a buffer for each subsequence.</param>
	/// <param name="selector">A transform function to apply to each window.</param>
	/// <returns>A sequence representing each sliding window.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/>, <paramref name="selector"/>, or <paramref
	/// name="array"/> is null.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="size"/> is below 1 or above <c><paramref
	/// name="array"/>.Length</c>.</exception>
	/// <remarks>
	/// <para>
	/// A window can contain fewer elements than <paramref name="size"/>, especially as it slides over the start of the
	/// sequence.
	/// </para>
	/// <para>
	/// In this overload of <c>WindowLeft</c>, <paramref name="array"/> is used as a common buffer for all
	/// subsequences.<br/> This overload is provided to ease usage of common buffers, such as those rented from <see
	/// cref="System.Buffers.ArrayPool{T}"/>, which may return an array larger than requested.
	/// </para>
	/// <para>
	/// This operator uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	/// <example>
	/// <code><![CDATA[
	/// Console.WriteLine(
	///     Enumerable
	///         .Range(1, 5)
	///         .WindowLeft(3)
	///         .Select(w => "AVG(" + w.ToDelimitedString(",") + ") = " + w.Average())
	///         .ToDelimitedString(Environment.NewLine));
	///
	/// // Output:
	/// // AVG(1,2,3) = 2
	/// // AVG(2,3,4) = 3
	/// // AVG(3,4,5) = 4
	/// // AVG(4,5) = 4.5
	/// // AVG(5) = 5
	/// ]]></code>
	/// </example>
	public static IEnumerable<TResult> WindowLeft<TSource, TResult>(
		this IEnumerable<TSource> source,
		TSource[] array,
		int size,
		Func<IReadOnlyList<TSource>, TResult> selector)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(array);
		ArgumentNullException.ThrowIfNull(selector);
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(size);
		ArgumentOutOfRangeException.ThrowIfGreaterThan(size, array.Length);

		return WindowImpl(source, array, size, WindowType.Left, selector);
	}
}
