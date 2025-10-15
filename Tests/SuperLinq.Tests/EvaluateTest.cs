namespace SuperLinq.Tests;

public sealed class EvaluateTest
{
	[Fact]
	public void TestEvaluateIsLazy()
	{
		_ = new BreakingSequence<Func<int>>().Evaluate();
	}

	[Fact]
	public void TestEvaluateInvokesMethods()
	{
		var factories = new Func<int>[]
		{
				() => -2,
				() => 4,
				() => int.MaxValue,
				() => int.MinValue,
		};
		var results = factories.Evaluate();

		results.AssertSequenceEqual(-2, 4, int.MaxValue, int.MinValue);
	}

	[Fact]
	public void TestEvaluateInvokesMethodsMultipleTimes()
	{
		var evals = 0;
		var factories = new Func<int>[]
		{
			() => { evals++; return -2; },
		};
		var results = factories.Evaluate();

		results.Consume();
		results.Consume();
		results.Consume();

		Assert.Equal(3, evals);
	}
}
