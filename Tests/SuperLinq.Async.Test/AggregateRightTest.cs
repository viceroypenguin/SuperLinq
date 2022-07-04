namespace Test.Async;

public class AggregateRightTest
{
	// Overload 1 Test

	[Fact]
	public Task AggregateRightWithEmptySequence()
	{
		return Assert.ThrowsAsync<InvalidOperationException>(
			async () => await AsyncEnumerable.Empty<int>().AggregateRight((a, b) => a + b));
	}

	[Fact]
	public async Task AggregateRightFuncIsNotInvokedOnSingleElementSequence()
	{
		const int Value = 1;

		var result = await AsyncSeq(Value).AggregateRight(BreakingFunc.Of<int, int, int>());

		Assert.Equal(Value, result);
	}

	[Fact]
	public async Task AggregateRight()
	{
		var enumerable = AsyncEnumerable.Range(1, 5).Select(x => x.ToString());

		var result = await enumerable.AggregateRight((a, b) => string.Format("({0}+{1})", a, b));

		Assert.Equal("(1+(2+(3+(4+5))))", result);
	}

	[Theory]
	[InlineData(5)]
	[InlineData("c")]
	[InlineData(true)]
	public async Task AggregateRightSeedWithEmptySequence(object defaultValue)
	{
		Assert.Equal(defaultValue, await AsyncEnumerable.Empty<int>().AggregateRight(defaultValue, (a, b) => b));
	}

	[Fact]
	public async Task AggregateRightSeedFuncIsNotInvokedOnEmptySequence()
	{
		const int Value = 1;

		var result = await AsyncEnumerable.Empty<int>().AggregateRight(Value, BreakingFunc.Of<int, int, int>());

		Assert.Equal(Value, result);
	}

	[Fact]
	public async Task AggregateRightSeed()
	{
		var result = await AsyncEnumerable
			.Range(1, 4)
			.AggregateRight("5", (a, b) => string.Format("({0}+{1})", a, b));

		Assert.Equal("(1+(2+(3+(4+5))))", result);
	}

	[Theory]
	[InlineData(5)]
	[InlineData("c")]
	[InlineData(true)]
	public async Task AggregateRightResultorWithEmptySequence(object defaultValue)
	{
		Assert.True(await AsyncEnumerable.Empty<int>().AggregateRight(defaultValue, (a, b) => b, a => a == defaultValue));
	}

	[Fact]
	public async Task AggregateRightResultor()
	{
		var result = await AsyncEnumerable
			.Range(1, 4)
			.AggregateRight("5", (a, b) => string.Format("({0}+{1})", a, b), a => a.Length);
		Assert.Equal("(1+(2+(3+(4+5))))".Length, result);
	}
}
