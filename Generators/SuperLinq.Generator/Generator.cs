using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Scriban;

namespace SuperLinq;

[Generator]
public class Generator : IIncrementalGenerator
{
	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
			"Aggregate.cs", GenerateArgumentNamesTemplate(ThisAssembly.Resources.Aggregate.Text)));
		context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
			"Cartesian.cs", GenerateArgumentNamesTemplate(ThisAssembly.Resources.Cartesian.Text)));
		context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
			"EquiZip.cs", GenerateArgumentNamesTemplate(ThisAssembly.Resources.EquiZip.Text)));
		context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
			"Fold.cs", GenerateArgumentNamesTemplate(ThisAssembly.Resources.Fold.Text)));
		context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
			"ToDelimitedString.cs", ToDelimitedString.Generate()));
	}

	private static SourceText GenerateArgumentNamesTemplate(string template)
	{
		var output = Template.Parse(template).Render(ArgumentNames.Instance);

		// Apply formatting since indenting isn't that nice in Scriban when rendering nested 
		// structures via functions.
		output = Microsoft.CodeAnalysis.CSharp.SyntaxFactory.ParseCompilationUnit(output)
			.NormalizeWhitespace()
			.GetText()
			.ToString();

		return SourceText.From(output, Encoding.UTF8);
	}
}
