using System.Globalization;

namespace SuperLinq.Tests;

public sealed class AggregateRightTest
{
	[Test]
	public void AggregateRightWithEmptySequence()
	{
		_ = Assert.Throws<InvalidOperationException>(
			() => Array.Empty<int>().AggregateRight((a, b) => a + b));
	}

	[Test]
	public void AggregateRightFuncIsNotInvokedOnSingleElementSequence()
	{
		using var enumerable = TestingSequence.Of(1);
		var result = enumerable.AggregateRight(BreakingFunc.Of<int, int, int>());

		Assert.Equal(1, result);
	}

	[Test]
	public void AggregateRight()
	{
		using var enumerable = Enumerable.Range(1, 5).Select(x => x.ToString(CultureInfo.InvariantCulture)).AsTestingSequence();
		var result = enumerable.AggregateRight((a, b) => FormattableString.Invariant($"({a}+{b})"));

		Assert.Equal("(1+(2+(3+(4+5))))", result);
	}

	[Test]
	public void AggregateRightWithList()
	{
		var list = Enumerable.Range(1, 5).Select(x => x.ToString(CultureInfo.InvariantCulture)).ToList();
		var result = list.AggregateRight((a, b) => FormattableString.Invariant($"({a}+{b})"));

		Assert.Equal("(1+(2+(3+(4+5))))", result);
	}

	[Test]
	[Arguments(5)]
	[Arguments("c")]
	[Arguments(true)]
	public void AggregateRightSeedWithEmptySequence(object defaultValue)
	{
		using var enumerable = TestingSequence.Of<int>();
		Assert.Equal(defaultValue, enumerable.AggregateRight(defaultValue, (a, b) => b));
	}

	[Test]
	public void AggregateRightSeedFuncIsNotInvokedOnEmptySequence()
	{
		using var enumerable = TestingSequence.Of<int>();
		var result = enumerable.AggregateRight(1, BreakingFunc.Of<int, int, int>());

		Assert.Equal(1, result);
	}

	[Test]
	public void AggregateRightSeed()
	{
		using var enumerable = Enumerable.Range(1, 4).AsTestingSequence();
		var result = enumerable.AggregateRight("5", (a, b) => FormattableString.Invariant($"({a}+{b})"));

		Assert.Equal("(1+(2+(3+(4+5))))", result);
	}

	[Test]
	[Arguments(5)]
	[Arguments("c")]
	[Arguments(true)]
	public void AggregateRightResultorWithEmptySequence(object defaultValue)
	{
		using var enumerable = TestingSequence.Of<int>();
		var result = enumerable.AggregateRight(defaultValue, (a, b) => b, a => a == defaultValue);

		Assert.True(result);
	}

	[Test]
	public void AggregateRightResultor()
	{
		using var enumerable = Enumerable.Range(1, 4).AsTestingSequence();
		var result = enumerable.AggregateRight("5", (a, b) => FormattableString.Invariant($"({a}+{b})"), a => a.Length);

		Assert.Equal("(1+(2+(3+(4+5))))".Length, result);
	}

	[Test]
	public void AggregateRightResultorWithList()
	{
		var list = Enumerable.Range(1, 4).ToList();
		var result = list.AggregateRight("5", (a, b) => FormattableString.Invariant($"({a}+{b})"), a => a.Length);

		Assert.Equal("(1+(2+(3+(4+5))))".Length, result);
	}
}
