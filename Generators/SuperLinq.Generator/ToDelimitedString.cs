using System.Reflection;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Scriban;

namespace SuperLinq.Generator;

internal static class ToDelimitedString
{
	public static SourceText Generate()
	{
		var types =
			from method in typeof(StringBuilder).GetMethods(BindingFlags.Public | BindingFlags.Instance)
			where string.Equals("Append", method.Name, StringComparison.Ordinal)
			select method.GetParameters() into parameters
			where parameters.Length == 1
			select parameters[0].ParameterType into type
			where !type.IsGenericType // e.g. ReadOnlySpan<>
			   && (type.IsValueType || type == typeof(string))
			orderby type.Name
			select $"global::{type.FullName}";

		var template = Template.Parse(ThisAssembly.Resources.ToDelimitedString.Text);
		var output = template.Render(new { Types = types.ToList(), });

		// Apply formatting since indenting isn't that nice in Scriban when rendering nested 
		// structures via functions.
		output = Microsoft.CodeAnalysis.CSharp.SyntaxFactory.ParseCompilationUnit(output)
			.NormalizeWhitespace()
			.GetText()
			.ToString();

		return SourceText.From(output, Encoding.UTF8);
	}
}
