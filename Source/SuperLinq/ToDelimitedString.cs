using System.Text;

namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Creates a delimited string from a sequence of values and
	/// a given delimiter.
	/// </summary>
	/// <typeparam name="TSource">Type of element in the source sequence</typeparam>
	/// <param name="source">The sequence of items to delimit. Each is converted to a string using the
	/// simple ToString() conversion.</param>
	/// <param name="delimiter">The delimiter to inject between elements.</param>
	/// <returns>
	/// A string that consists of the elements in <paramref name="source"/>
	/// delimited by <paramref name="delimiter"/>. If the source sequence
	/// is empty, the method returns an empty string.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	/// <paramref name="source"/> or <paramref name="delimiter"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	/// This operator uses immediate execution and effectively buffers the sequence.
	/// </remarks>

	public static string ToDelimitedString<TSource>(this IEnumerable<TSource> source, string delimiter)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(delimiter);
		return ToDelimitedStringImpl(source, delimiter, (sb, e) => sb.Append(e));
	}

	static string ToDelimitedStringImpl<T>(IEnumerable<T> source, string delimiter, Func<StringBuilder, T, StringBuilder> append)
	{
		var sb = new StringBuilder();
		var i = 0;

		foreach (var value in source)
		{
			if (i++ > 0) sb.Append(delimiter);
			append(sb, value);
		}

		return sb.ToString();
	}
}
