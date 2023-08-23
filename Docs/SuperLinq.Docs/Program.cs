using Docfx;
using Docfx.Dotnet;

await DotnetApiCatalog.GenerateManagedReferenceYamlFiles("docfx.json");
await Docset.Build("docfx.json");
