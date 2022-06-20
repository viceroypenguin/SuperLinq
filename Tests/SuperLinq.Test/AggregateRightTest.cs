namespace Test;

public class AggregateRightTest
{
	// Overload 1 Test

	[Fact]
	public void AggregateRightWithEmptySequence()
	{
		Assert.Throws<InvalidOperationException>(
			() => Array.Empty<int>().AggregateRight((a, b) => a + b));
	}

	[Fact]
	public void AggregateRightFuncIsNotInvokedOnSingleElementSequence()
	{
		const int Value = 1;

		var result = new[] { Value }.AggregateRight(BreakingFunc.Of<int, int, int>());

		Assert.Equal(Value, result);
	}

	[Fact]
	public void AggregateRight()
	{
		var enumerable = Enumerable.Range(1, 5).Select(x => x.ToString());

		var result = enumerable.AggregateRight((a, b) => string.Format("({0}+{1})", a, b));

		Assert.Equal("(1+(2+(3+(4+5))))", result);
	}

	[Theory]
	[InlineData(5)]
	[InlineData("c")]
	[InlineData(true)]
	public void AggregateRightSeedWithEmptySequence(object defaultValue)
	{
		Assert.Equal(defaultValue, Array.Empty<int>().AggregateRight(defaultValue, (a, b) => b));
	}

	[Fact]
	public void AggregateRightSeedFuncIsNotInvokedOnEmptySequence()
	{
		const int Value = 1;

		var result = Array.Empty<int>().AggregateRight(Value, BreakingFunc.Of<int, int, int>());

		Assert.Equal(Value, result);
	}

	[Fact]
	public void AggregateRightSeed()
	{
		var result = Enumerable.Range(1, 4)
							   .AggregateRight("5", (a, b) => string.Format("({0}+{1})", a, b));

		Assert.Equal("(1+(2+(3+(4+5))))", result);
	}

	[Theory]
	[InlineData(5)]
	[InlineData("c")]
	[InlineData(true)]
	public void AggregateRightResultorWithEmptySequence(object defaultValue)
	{
		Assert.True(Array.Empty<int>().AggregateRight(defaultValue, (a, b) => b, a => a == defaultValue));
	}

	[Fact]
	public void AggregateRightResultor()
	{
		var result = Enumerable.Range(1, 4)
							   .AggregateRight("5", (a, b) => string.Format("({0}+{1})", a, b), a => a.Length);

		Assert.Equal("(1+(2+(3+(4+5))))".Length, result);
	}
}
