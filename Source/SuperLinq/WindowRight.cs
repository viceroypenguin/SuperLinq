namespace SuperLinq;

public static partial class SuperEnumerable
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

	public static IEnumerable<IList<TSource>> WindowRight<TSource>(this IEnumerable<TSource> source, int size)
	{
		source.ThrowIfNull();
		size.ThrowIfLessThan(1);

		return source.WindowRightWhile((_, i) => i < size);
	}

	/// <summary>
	/// Creates a right-aligned sliding window over the source sequence
	/// with a predicate function determining the window range.
	/// </summary>

	static IEnumerable<IList<TSource>> WindowRightWhile<TSource>(
		this IEnumerable<TSource> source,
		Func<TSource, int, bool> predicate)
	{
		source.ThrowIfNull();
		predicate.ThrowIfNull();

		return _(); IEnumerable<IList<TSource>> _()
		{
			var window = new List<TSource>();
			foreach (var item in source)
			{
				window.Add(item);

				// prepare next window before exposing data
				var nextWindow = new List<TSource>(predicate(item, window.Count) ? window : window.Skip(1));
				yield return window;
				window = nextWindow;
			}
		}
	}
}
