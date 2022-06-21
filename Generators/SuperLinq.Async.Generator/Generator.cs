using Microsoft.CodeAnalysis;

namespace SuperLinq;

[Generator]
public class Generator : IIncrementalGenerator
{
	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
			"Fold.g.cs", Fold.Generate()));
	}
}
