await Bootstrapper
	.Factory
	.CreateDocs(args)
	.AddSetting("LinkRoot", "/SuperLinq/")
	.AddSourceFiles(
		"../../../Source/SuperLinq/{!bin,!obj,}/**/*.cs",
		"../../../Source/SuperLinq.Async/{!bin,!obj,}/**/*.cs")
	.RunAsync();
