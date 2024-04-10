using System.Globalization;

namespace Test;

public sealed class AggregateRightTest
{
	[Fact]
	public void AggregateRightWithEmptySequence()
	{
		_ = Assert.Throws<InvalidOperationException>(
			() => Array.Empty<int>().AggregateRight((a, b) => a + b));
	}

	[Fact]
	public void AggregateRightFuncIsNotInvokedOnSingleElementSequence()
	{
		using var enumerable = TestingSequence.Of(1);
		var result = enumerable.AggregateRight(BreakingFunc.Of<int, int, int>());

		Assert.Equal(1, result);
	}

	[Fact]
	public void AggregateRight()
	{
		using var enumerable = Enumerable.Range(1, 5).Select(x => x.ToString(CultureInfo.InvariantCulture)).AsTestingSequence();
		var result = enumerable.AggregateRight((a, b) => FormattableString.Invariant($"({a}+{b})"));

		Assert.Equal("(1+(2+(3+(4+5))))", result);
	}

	[Fact]
	public void AggregateRightWithList()
	{
		var list = Enumerable.Range(1, 5).Select(x => x.ToString(CultureInfo.InvariantCulture)).ToList();
		var result = list.AggregateRight((a, b) => FormattableString.Invariant($"({a}+{b})"));

		Assert.Equal("(1+(2+(3+(4+5))))", result);
	}

	[Theory]
	[InlineData(5)]
	[InlineData("c")]
	[InlineData(true)]
	public void AggregateRightSeedWithEmptySequence(object defaultValue)
	{
		using var enumerable = TestingSequence.Of<int>();
		Assert.Equal(defaultValue, enumerable.AggregateRight(defaultValue, (a, b) => b));
	}

	[Fact]
	public void AggregateRightSeedFuncIsNotInvokedOnEmptySequence()
	{
		using var enumerable = TestingSequence.Of<int>();
		var result = enumerable.AggregateRight(1, BreakingFunc.Of<int, int, int>());

		Assert.Equal(1, result);
	}

	[Fact]
	public void AggregateRightSeed()
	{
		using var enumerable = Enumerable.Range(1, 4).AsTestingSequence();
		var result = enumerable.AggregateRight("5", (a, b) => FormattableString.Invariant($"({a}+{b})"));

		Assert.Equal("(1+(2+(3+(4+5))))", result);
	}

	[Theory]
	[InlineData(5)]
	[InlineData("c")]
	[InlineData(true)]
	public void AggregateRightResultorWithEmptySequence(object defaultValue)
	{
		using var enumerable = TestingSequence.Of<int>();
		var result = enumerable.AggregateRight(defaultValue, (a, b) => b, a => a == defaultValue);

		Assert.True(result);
	}

	[Fact]
	public void AggregateRightResultor()
	{
		using var enumerable = Enumerable.Range(1, 4).AsTestingSequence();
		var result = enumerable.AggregateRight("5", (a, b) => FormattableString.Invariant($"({a}+{b})"), a => a.Length);

		Assert.Equal("(1+(2+(3+(4+5))))".Length, result);
	}

	[Fact]
	public void AggregateRightResultorWithList()
	{
		var list = Enumerable.Range(1, 4).ToList();
		var result = list.AggregateRight("5", (a, b) => FormattableString.Invariant($"({a}+{b})"), a => a.Length);

		Assert.Equal("(1+(2+(3+(4+5))))".Length, result);
	}
}
