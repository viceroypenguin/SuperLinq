namespace SuperLinq.Tests;

public sealed class EvaluateTest
{
	[Test]
	public void TestEvaluateIsLazy()
	{
		_ = new BreakingSequence<Func<int>>().Evaluate();
	}

	[Test]
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

	[Test]
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
