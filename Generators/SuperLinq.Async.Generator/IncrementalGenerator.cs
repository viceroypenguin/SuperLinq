using System.Reflection;
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
public sealed class IncrementalGenerator : IIncrementalGenerator
{
	/// <inheritdoc />
	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
			"EquiZip.g.cs", GenerateArgumentNamesTemplate(GetTemplate("EquiZip"))));
		context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
			"Fold.g.cs", GenerateArgumentNamesTemplate(GetTemplate("Fold"))));
		context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
			"ZipLongest.g.cs", GenerateArgumentNamesTemplate(GetTemplate("ZipLongest"))));
		context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
			"ZipShortest.g.cs", GenerateArgumentNamesTemplate(GetTemplate("ZipShortest"))));
	}

	private static SourceText GenerateArgumentNamesTemplate(Template template)
	{
		var output = template.Render(ArgumentNames.Instance);
		return SourceText.From(output, Encoding.UTF8);
	}

	private static Template GetTemplate(string name)
	{
		using var stream = Assembly
			.GetExecutingAssembly()
			.GetManifestResourceStream(
				$"SuperLinq.Async.Generator.{name}.sbntxt"
			)!;

		using var reader = new StreamReader(stream);
		return Template.Parse(reader.ReadToEnd());
	}
}
