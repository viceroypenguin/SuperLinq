await Bootstrapper
	.Factory
	.CreateDocs(args)
	.AddSourceFiles(
		"../../../Source/SuperLinq/{!bin,!obj,}/**/*.cs",
		"../../../Source/SuperLinq.Async/{!bin,!obj,}/**/*.cs")
	.RunAsync();
