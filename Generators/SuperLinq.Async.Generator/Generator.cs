using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Scriban;
using SuperLinq.Generator;

namespace SuperLinq.Async.Generator;

/// <summary>
/// Uses source generation tools to automate the building of some operators
/// </summary>
[Generator]
public sealed class Generator : IIncrementalGenerator
{
	/// <inheritdoc />
	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
			"EquiZip.g.cs", GenerateArgumentNamesTemplate(ThisAssembly.Resources.EquiZip.Text)));
		context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
			"Fold.g.cs", GenerateArgumentNamesTemplate(ThisAssembly.Resources.Fold.Text)));
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
