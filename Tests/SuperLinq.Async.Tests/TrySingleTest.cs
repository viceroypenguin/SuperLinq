namespace SuperLinq.Async.Tests;

public sealed class TrySingleTest
{
	[Test]
	public async Task TrySingleWithEmptySource()
	{
		await using var source = AsyncEnumerable.Empty<int?>().AsTestingSequence();

		var (cardinality, value) = await source.TrySingle("zero", "one", "many");

		Assert.Equal("zero", cardinality);
		Assert.Null(value);
	}

	[Test]
	public async Task TrySingleWithSingleton()
	{
		await using var source = AsyncSeq<int?>(10).AsTestingSequence();

		var (cardinality, value) = await source.TrySingle("zero", "one", "many");

		Assert.Equal("one", cardinality);
		Assert.Equal(10, value);
	}

	[Test]
	public async Task TrySingleWithMoreThanOne()
	{
		await using var source = AsyncSeq<int?>(10, 20).AsTestingSequence();

		var (cardinality, value) = await source.TrySingle("zero", "one", "many");

		Assert.Equal("many", cardinality);
		Assert.Null(value);
	}

	[Test]
	public async Task TrySingleDoesNotConsumeMoreThanTwoElementsFromTheSequence()
	{
		await using var seq = AsyncSuperEnumerable
			.From(
				() => Task.FromResult(1),
				() => Task.FromResult(2),
				AsyncBreakingFunc.Of<int>())
			.AsTestingSequence();

		var (cardinality, value) = await seq.TrySingle("zero", "one", "many");

		Assert.Equal("many", cardinality);
		Assert.Equal(0, value);
	}

	[Test]
	[Arguments(0, "zero")]
	[Arguments(1, "one")]
	[Arguments(2, "many")]
	public async Task TrySingleEnumeratesOnceOnlyAndDisposes(int numberOfElements, string expectedCardinality)
	{
		await using var seq = AsyncEnumerable.Range(1, numberOfElements).AsTestingSequence();
		var (cardinality, _) = await seq.TrySingle("zero", "one", "many");
		Assert.Equal(expectedCardinality, cardinality);
	}

	[Test]
	[Arguments(0, 0)]
	[Arguments(1, 1)]
	[Arguments(2, 0)]
	public async Task TrySingleShouldReturnDefaultOrSingleValue(int numberOfElements, int expectedResult)
	{
		await using var seq = AsyncEnumerable.Range(1, numberOfElements).AsTestingSequence();
		var result = await seq.TrySingle();
		Assert.Equal(expectedResult, result);
	}
}
