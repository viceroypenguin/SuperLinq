using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Scriban;

namespace SuperLinq;

internal static class Aggregate
{
	public static SourceText Generate()
	{
		var template = Template.Parse(ThisAssembly.Resources.Aggregate.Text);
		var output = template.Render(new
		{
			Ordinals = new[]
			{
				"zeroth" ,
				"first"  ,
				"second" ,
				"third"  ,
				"fourth" ,
				"fifth"  ,
				"sixth"  ,
				"seventh",
				"eighth" ,
			},
			Arity = new[]
			{
				"zero" ,
				"one"  ,
				"two"  ,
				"three",
				"four" ,
				"five" ,
				"six"  ,
				"seven",
				"eight",
			},
		});

		// Apply formatting since indenting isn't that nice in Scriban when rendering nested 
		// structures via functions.
		output = Microsoft.CodeAnalysis.CSharp.SyntaxFactory.ParseCompilationUnit(output)
			.NormalizeWhitespace()
			.GetText()
			.ToString();

		return SourceText.From(output, Encoding.UTF8);
	}
}
