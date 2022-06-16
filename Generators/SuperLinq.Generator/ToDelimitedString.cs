using System.Reflection;
using System.Text;
using Microsoft.CodeAnalysis.Text;

namespace SuperLinq;

internal static class ToDelimitedString
{
	public static SourceText Generate()
	{
		var sb = new StringBuilder();
		sb.Append(@"
using System.Text;

namespace SuperLinq;

public static partial class SuperEnumerable
{");

		var types =
			from method in typeof(StringBuilder).GetMethods(BindingFlags.Public | BindingFlags.Instance)
			where string.Equals("Append", method.Name, StringComparison.Ordinal)
			select method.GetParameters() into parameters
			where parameters.Length == 1
			select parameters[0].ParameterType into type
			where !type.IsGenericType // e.g. ReadOnlySpan<>
			   && (type.IsValueType || type == typeof(string))
			orderby type.Name
			select type;

		foreach (var type in types)
		{
			sb.Append($@"
	/// <summary>
	/// Creates a delimited string from a sequence of values and
	/// a given delimiter.
	/// </summary>
	/// <param name=""source"">The sequence of items to delimit. Each is converted to a string using the
	/// simple ToString() conversion.</param>
	/// <param name=""delimiter"">The delimiter to inject between elements.</param>
	/// <returns>
	/// A string that consists of the elements in <paramref name=""source""/>
	/// delimited by <paramref name=""delimiter""/>. If the source sequence
	/// is empty, the method returns an empty string.
	/// </returns>
	/// <exception cref=""ArgumentNullException"">
	/// <paramref name=""source""/> or <paramref name=""delimiter""/> is <c>null</c>.
	/// </exception>
	/// <remarks>
	/// This operator uses immediate execution and effectively buffers the sequence.
	/// </remarks>
	public static string ToDelimitedString(this IEnumerable<global::{type.FullName}> source, string delimiter)
	{{
		source.ThrowIfNull();
		delimiter.ThrowIfNull();
		return ToDelimitedStringImpl(source, delimiter, Builder);

		static StringBuilder Builder(StringBuilder sb, global::{type.FullName} e) => sb.Append(e);
    }}");
		}

		sb.Append(@"
}");

		return SourceText.From(sb.ToString(), Encoding.UTF8);
	}
}
