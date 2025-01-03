using System.Globalization;

namespace SuperLinq.Async.Tests;

public sealed class AggregateRightTest
{
	// Overload 1 Test

	[Test]
	public Task AggregateRightWithEmptySequence()
	{
		return Assert.ThrowsAsync<InvalidOperationException>(
			async () => await AsyncEnumerable.Empty<int>().AggregateRight((a, b) => a + b));
	}

	[Test]
	public async Task AggregateRightFuncIsNotInvokedOnSingleElementSequence()
	{
		await using var enumerable = TestingSequence.Of(1);
		var result = await enumerable.AggregateRight(BreakingFunc.Of<int, int, int>());

		Assert.Equal(1, result);
	}

	[Test]
	public async Task AggregateRight()
	{
		await using var enumerable = AsyncEnumerable.Range(1, 5).Select(x => x.ToString(CultureInfo.InvariantCulture)).AsTestingSequence();
		var result = await enumerable.AggregateRight((a, b) => FormattableString.Invariant($"({a}+{b})"));

		Assert.Equal("(1+(2+(3+(4+5))))", result);
	}

	[Test]
	[Arguments(5)]
	[Arguments("c")]
	[Arguments(true)]
	public async Task AggregateRightSeedWithEmptySequence(object defaultValue)
	{
		await using var enumerable = TestingSequence.Of<int>();
		Assert.Equal(defaultValue, await enumerable.AggregateRight(defaultValue, (a, b) => b));
	}

	[Test]
	public async Task AggregateRightSeedFuncIsNotInvokedOnEmptySequence()
	{
		await using var enumerable = TestingSequence.Of<int>();
		var result = await enumerable.AggregateRight(1, BreakingFunc.Of<int, int, int>());

		Assert.Equal(1, result);
	}

	[Test]
	public async Task AggregateRightSeed()
	{
		await using var enumerable = AsyncEnumerable.Range(1, 4).AsTestingSequence();
		var result = await enumerable
			.AggregateRight("5", (a, b) => FormattableString.Invariant($"({a}+{b})"));

		Assert.Equal("(1+(2+(3+(4+5))))", result);
	}

	[Test]
	[Arguments(5)]
	[Arguments("c")]
	[Arguments(true)]
	public async Task AggregateRightResultorWithEmptySequence(object defaultValue)
	{
		await using var enumerable = TestingSequence.Of<int>();
		var result = await enumerable.AggregateRight(defaultValue, (a, b) => b, a => a == defaultValue);

		Assert.True(result);
	}

	[Test]
	public async Task AggregateRightResultor()
	{
		await using var enumerable = AsyncEnumerable.Range(1, 4).AsTestingSequence();
		var result = await enumerable
			.AggregateRight("5", (a, b) => FormattableString.Invariant($"({a}+{b})"), a => a.Length);

		Assert.Equal("(1+(2+(3+(4+5))))".Length, result);
	}
}
