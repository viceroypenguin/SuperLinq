using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Scriban;

namespace SuperLinq.Generator;

/// <summary>
/// Uses source generation tools to automate the building of some operators
/// </summary>
[Generator]
public class Generator : IIncrementalGenerator
{
	/// <inheritdoc />
	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
			"Aggregate.g.cs", GenerateArgumentNamesTemplate(ThisAssembly.Resources.Aggregate.Text)));
		context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
			"Cartesian.g.cs", GenerateArgumentNamesTemplate(ThisAssembly.Resources.Cartesian.Text)));
		context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
			"EquiZip.g.cs", GenerateArgumentNamesTemplate(ThisAssembly.Resources.EquiZip.Text)));
		context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
			"Fold.g.cs", GenerateArgumentNamesTemplate(ThisAssembly.Resources.Fold.Text)));
		context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
			"ToDelimitedString.g.cs", ToDelimitedString.Generate()));
		context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
			"ZipLongest.g.cs", GenerateArgumentNamesTemplate(ThisAssembly.Resources.ZipLongest.Text)));
		context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
			"ZipShortest.g.cs", GenerateArgumentNamesTemplate(ThisAssembly.Resources.ZipShortest.Text)));
	}

	private static SourceText GenerateArgumentNamesTemplate(string template)
	{
		var output = Template.Parse(template).Render(ArgumentNames.Instance);
		return SourceText.From(output, Encoding.UTF8);
	}
}
