namespace Test;

public class FromTest
{
	[Fact]
	public void TestFromIsLazy()
	{
		var breakingFunc = BreakingFunc.Of<int>();
		_ = SuperEnumerable.From(breakingFunc);
		_ = SuperEnumerable.From(breakingFunc, breakingFunc);
		_ = SuperEnumerable.From(breakingFunc, breakingFunc, breakingFunc);
		_ = SuperEnumerable.From(breakingFunc, breakingFunc, breakingFunc, breakingFunc);
	}

	[Theory]
	[InlineData(1)]
	[InlineData(2)]
	[InlineData(3)]
	[InlineData(4)]
	public void TestFromInvokesMethods(int numArgs)
	{
		int F1() => -2;
		int F2() => 4;
		int F3() => int.MaxValue;
		int F4() => int.MinValue;

		switch (numArgs)
		{
			case 1: SuperEnumerable.From(F1).AssertSequenceEqual(F1()); break;
			case 2: SuperEnumerable.From(F1, F2).AssertSequenceEqual(F1(), F2()); break;
			case 3: SuperEnumerable.From(F1, F2, F3).AssertSequenceEqual(F1(), F2(), F3()); break;
			case 4: SuperEnumerable.From(F1, F2, F3, F4).AssertSequenceEqual(F1(), F2(), F3(), F4()); break;
			default: throw new ArgumentOutOfRangeException(nameof(numArgs));
		}
	}

	[Theory]
	[InlineData(1)]
	[InlineData(2)]
	[InlineData(3)]
	[InlineData(4)]
	public void TestFromInvokesMethodsMultipleTimes(int numArgs)
	{
		var evals = new[] { 0, 0, 0, 0 };
		int F1() { evals[0]++; return -2; }
		int F2() { evals[1]++; return -2; }
		int F3() { evals[2]++; return -2; }
		int F4() { evals[3]++; return -2; }

		var results = numArgs switch
		{
			1 => SuperEnumerable.From(F1),
			2 => SuperEnumerable.From(F1, F2),
			3 => SuperEnumerable.From(F1, F2, F3),
			4 => SuperEnumerable.From(F1, F2, F3, F4),
			_ => throw new ArgumentOutOfRangeException(nameof(numArgs)),
		};

		results.Consume();
		results.Consume();
		results.Consume();

		// numArgs functions were evaluated...
		evals.Take(numArgs).AssertSequenceEqual(Enumerable.Repeat(3, numArgs));
		// safety check: we didn't evaluate functions past numArgs
		evals.Skip(numArgs).AssertSequenceEqual(Enumerable.Repeat(0, evals.Length - numArgs));
	}
}
