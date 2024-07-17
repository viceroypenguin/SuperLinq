using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Running;

BenchmarkSwitcher
	.FromAssembly(typeof(Program).Assembly)
	.Run(
		args,
		DefaultConfig.Instance
			.AddDiagnoser(MemoryDiagnoser.Default)
	);
