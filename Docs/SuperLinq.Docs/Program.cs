using Microsoft.DocAsCode;
using Microsoft.DocAsCode.Dotnet;

await DotnetApiCatalog.GenerateManagedReferenceYamlFiles("docfx.json");
await Docset.Build("docfx.json");
