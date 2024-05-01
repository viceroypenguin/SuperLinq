using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

namespace SuperLinq.Benchmarks.Sync;

public class FilteringBenchmarks : TestDataBenchmark
{
	private readonly Consumer _consumer = new();

	[Benchmark]
	[ArgumentsSource(nameof(NoSpecialization))]
	public void Choose(LinqTestData testData) =>
		testData.Collection
			.Choose(x => (false, x))
			.Consume(_consumer);

	[Benchmark]
	[ArgumentsSource(nameof(NoSpecialization))]
	public void Where(LinqTestData testData) =>
		testData.Collection
			.Where(testData.Collection.Select(x => true))
			.Consume(_consumer);

	[Benchmark]
	[ArgumentsSource(nameof(NoSpecialization))]
	public void WhereLead(LinqTestData testData) =>
		testData.Collection
			.WhereLead(1, (a, b) => true)
			.Consume(_consumer);

	[Benchmark]
	[ArgumentsSource(nameof(NoSpecialization))]
	public void WhereLag(LinqTestData testData) =>
		testData.Collection
			.WhereLag(1, (a, b) => true)
			.Consume(_consumer);
}
