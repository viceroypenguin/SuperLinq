namespace Test.Async;

public class TrySingleTest
{
	[Fact]
	public async Task TrySingleWithEmptySource()
	{
		var source = AsyncEnumerable.Empty<int?>();

		var (cardinality, value) = await source.TrySingle("zero", "one", "many");

		Assert.Equal("zero", cardinality);
		Assert.Null(value);
	}

	[Fact]
	public async Task TrySingleWithSingleton()
	{
		var source = AsyncSeq<int?>(10);

		var (cardinality, value) = await source.TrySingle("zero", "one", "many");

		Assert.Equal("one", cardinality);
		Assert.Equal(10, value);
	}

	[Fact]
	public async Task TrySingleWithMoreThanOne()
	{
		var source = AsyncSeq<int?>(10, 20);

		var (cardinality, value) = await source.TrySingle("zero", "one", "many");

		Assert.Equal("many", cardinality);
		Assert.Null(value);
	}

	[Fact]
	public async Task TrySingleDoesNotConsumeMoreThanTwoElementsFromTheSequence()
	{
		var seq = AsyncSuperEnumerable.From(
			() => Task.FromResult(1),
			() => Task.FromResult(2),
			AsyncBreakingFunc.Of<int>());

		var (cardinality, value) = await seq.TrySingle("zero", "one", "many");

		Assert.Equal("many", cardinality);
		Assert.Equal(0, value);
	}

	[Theory]
	[InlineData(0, "zero")]
	[InlineData(1, "one")]
	[InlineData(2, "many")]
	public async Task TrySingleEnumeratesOnceOnlyAndDisposes(int numberOfElements, string expectedCardinality)
	{
		await using var seq = AsyncEnumerable.Range(1, numberOfElements).AsTestingSequence();
		var (cardinality, _) = await seq.TrySingle("zero", "one", "many");
		Assert.Equal(expectedCardinality, cardinality);
	}
}
