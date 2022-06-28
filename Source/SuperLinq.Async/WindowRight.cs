namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Creates a right-aligned sliding window over the source sequence
	/// of a given size.
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
	/// especially as it slides over the start of the sequence.</para>
	/// <para>
	/// This operator uses deferred execution and streams its results.</para>
	/// </remarks>
	/// <example>
	/// <code><![CDATA[
	/// Console.WriteLine(
	///     Enumerable
	///         .Range(1, 5)
	///         .WindowRight(3)
	///         .Select(w => "AVG(" + w.ToDelimitedString(",") + ") = " + w.Average())
	///         .ToDelimitedString(Environment.NewLine));
	///
	/// // Output:
	/// // AVG(1) = 1
	/// // AVG(1,2) = 1.5
	/// // AVG(1,2,3) = 2
	/// // AVG(2,3,4) = 3
	/// // AVG(3,4,5) = 4
	/// ]]></code>
	/// </example>

	public static IAsyncEnumerable<IList<TSource>> WindowRight<TSource>(this IAsyncEnumerable<TSource> source, int size)
	{
		source.ThrowIfNull();
		size.ThrowIfLessThan(1);

		return WindowImpl(source, size, WindowType.Right);
	}
}
