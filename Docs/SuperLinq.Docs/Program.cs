using Microsoft.Build.Locator;
using System.Runtime.Loader;
using Microsoft.DocAsCode;

var instance = MSBuildLocator.RegisterDefaults();
AssemblyLoadContext.Default.Resolving += (assemblyLoadContext, assemblyName) =>
{
	var path = Path.Combine(instance.MSBuildPath, assemblyName.Name + ".dll");
	return File.Exists(path)
		? assemblyLoadContext.LoadFromAssemblyPath(path)
		: null;
};

await Docset.Build("docfx.json");
