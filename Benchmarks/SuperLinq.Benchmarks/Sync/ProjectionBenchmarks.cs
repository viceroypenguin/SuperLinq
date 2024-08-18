using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

namespace SuperLinq.Benchmarks.Sync;

[BenchmarkCategory("Projection")]
public class ProjectionBenchmarks : TestDataBenchmark
{
	private readonly Consumer _consumer = new();

	[Benchmark]
	[ArgumentsSource(nameof(ListSpecialization))]
	public void EquiZip(LinqTestData testData) =>
		testData.Collection
			.EquiZip(testData.Collection)
			.Consume(_consumer);

	[Benchmark]
	[ArgumentsSource(nameof(ListSpecialization))]
	public void ZipShortest(LinqTestData testData) =>
		testData.Collection
			.ZipShortest(testData.Collection)
			.Consume(_consumer);

	[Benchmark]
	[ArgumentsSource(nameof(ListSpecialization))]
	public void ZipLongest(LinqTestData testData) =>
		testData.Collection
			.ZipLongest(testData.Collection)
			.Consume(_consumer);

	[Benchmark]
	[ArgumentsSource(nameof(CountSpecialization))]
	public void CountDown(LinqTestData testData) =>
		testData.Collection
			.CountDown(5)
			.Consume(_consumer);

	[Benchmark]
	[ArgumentsSource(nameof(CountSpecialization))]
	public void TagFirstLast(LinqTestData testData) =>
		testData.Collection
			.TagFirstLast()
			.Consume(_consumer);

	[Benchmark]
	[ArgumentsSource(nameof(ListSpecialization))]
	public void Index(LinqTestData testData) =>
		testData.Collection
			.Index()
			.Consume(_consumer);

	[Benchmark]
	[ArgumentsSource(nameof(NoSpecialization))]
	public void IndexBy(LinqTestData testData) =>
		testData.Collection
			.IndexBy(x => x % 10)
			.Consume(_consumer);

	[Benchmark]
	[ArgumentsSource(nameof(ListSpecialization))]
	public void Lag(LinqTestData testData) =>
		testData.Collection
			.Lag(1)
			.Consume(_consumer);

	[Benchmark]
	[ArgumentsSource(nameof(ListSpecialization))]
	public void Lead(LinqTestData testData) =>
		testData.Collection
			.Lead(1)
			.Consume(_consumer);

	[Benchmark]
	[ArgumentsSource(nameof(NoSpecialization))]
	public void Rank(LinqTestData testData) =>
		testData.Collection
			.Rank()
			.Consume(_consumer);

	[Benchmark]
	[ArgumentsSource(nameof(ListSpecialization))]
	public void ZipMap(LinqTestData testData) =>
		testData.Collection
			.ZipMap(_ => true)
			.Consume(_consumer);
}
