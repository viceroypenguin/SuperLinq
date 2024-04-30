using BenchmarkDotNet.Attributes;

namespace SuperLinq.Benchmarks.Sync;

[BenchmarkCategory("Projection")]
public class ProjectionBenchmarks : TestDataBenchmark
{
	[Benchmark]
	[ArgumentsSource(nameof(ListSpecialization))]
	public void EquiZip(LinqTestData testData) => testData.Collection.EquiZip(testData.Collection);

	[Benchmark]
	[ArgumentsSource(nameof(ListSpecialization))]
	public void ZipShortest(LinqTestData testData) => testData.Collection.ZipShortest(testData.Collection);

	[Benchmark]
	[ArgumentsSource(nameof(ListSpecialization))]
	public void ZipLongest(LinqTestData testData) => testData.Collection.ZipLongest(testData.Collection);

	[Benchmark]
	[ArgumentsSource(nameof(CountSpecialization))]
	public void CountDown(LinqTestData testData) => testData.Collection.CountDown(5);

	[Benchmark]
	[ArgumentsSource(nameof(CountSpecialization))]
	public void TagFirstLast(LinqTestData testData) => testData.Collection.TagFirstLast();

	[Benchmark]
	[ArgumentsSource(nameof(ListSpecialization))]
	public void Index(LinqTestData testData) => testData.Collection.Index();

	[Benchmark]
	[ArgumentsSource(nameof(NoSpecialization))]
	public void IndexBy(LinqTestData testData) => testData.Collection.IndexBy(x => x % 10);

	[Benchmark]
	[ArgumentsSource(nameof(ListSpecialization))]
	public void Lag(LinqTestData testData) => testData.Collection.Lag(1);

	[Benchmark]
	[ArgumentsSource(nameof(ListSpecialization))]
	public void Lead(LinqTestData testData) => testData.Collection.Lead(1);

	[Benchmark]
	[ArgumentsSource(nameof(NoSpecialization))]
	public void Rank(LinqTestData testData) => testData.Collection.Rank();

	[Benchmark]
	[ArgumentsSource(nameof(ListSpecialization))]
	public void ZipMap(LinqTestData testData) => testData.Collection.ZipMap(_ => true);
}
