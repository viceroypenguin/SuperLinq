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
		var types = typeof(StringBuilder).GetMethods(BindingFlags.Public | BindingFlags.Instance)
			.Where(m => m.Name is "Append")
			.Select(m => m.GetParameters())
			.Where(p => p.Length == 1)
			.Select(p => p[0].ParameterType)
			.Where(t =>
				!t.IsGenericType // e.g. ReadOnlySpan<>
			   && (t.IsValueType || t == typeof(string)))
			.OrderBy(t => t.Name, StringComparer.Ordinal)
			.Select(t => $"global::{t.FullName}");

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
