using Microsoft.CodeAnalysis;

namespace SuperLinq;

[Generator]
public class Generator : IIncrementalGenerator
{
	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
			"Aggregate.g.cs", Aggregate.Generate()));
		context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
			"Cartesian.g.cs", Cartesian.Generate()));
	}
}
