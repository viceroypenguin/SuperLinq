namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Creates a left-aligned sliding window of a given size over the
	/// source sequence.
	/// </summary>
	/// <typeparam name="TSource">
	/// The type of the elements of <paramref name="source"/>.</typeparam>
	/// <param name="source">
	/// The sequence over which to create the sliding window.</param>
	/// <param name="size">Size of the sliding window.</param>
	/// <returns>A sequence representing each sliding window.</returns>
	/// <remarks>
	/// <para>
	/// A window can contain fewer elements than <paramref name="size"/>,
	/// especially as it slides over the end of the sequence.</para>
	/// <para>
	/// This operator uses deferred execution and streams its results.</para>
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
	public static IAsyncEnumerable<IList<TSource>> WindowLeft<TSource>(this IAsyncEnumerable<TSource> source, int size)
	{
		Guard.IsNotNull(source);
		Guard.IsGreaterThanOrEqualTo(size, 1);

		return WindowImpl(source, size, WindowType.Left);
	}
}
