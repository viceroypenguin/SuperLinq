namespace Test.Async;

public sealed class FromTest
{
	[Fact]
	public void TestFromIsLazy()
	{
		var breakingFunc = AsyncBreakingFunc.Of<int>();
		_ = AsyncSuperEnumerable.From(breakingFunc);
		_ = AsyncSuperEnumerable.From(breakingFunc, breakingFunc);
		_ = AsyncSuperEnumerable.From(breakingFunc, breakingFunc, breakingFunc);
		_ = AsyncSuperEnumerable.From(breakingFunc, breakingFunc, breakingFunc, breakingFunc);
	}

	[Theory]
	[InlineData(1)]
	[InlineData(2)]
	[InlineData(3)]
	[InlineData(4)]
	public async Task TestFromInvokesMethods(int numArgs)
	{
		static Task<int> F1() => Task.FromResult(-2);
		static Task<int> F2() => Task.FromResult(4);
		static Task<int> F3() => Task.FromResult(int.MaxValue);
		static Task<int> F4() => Task.FromResult(int.MinValue);

		switch (numArgs)
		{
			case 1:
				await AsyncSuperEnumerable.From(F1)
					.AssertSequenceEqual(await F1());
				break;
			case 2:
				await AsyncSuperEnumerable.From(F1, F2)
					.AssertSequenceEqual(await F1(), await F2());
				break;
			case 3:
				await AsyncSuperEnumerable.From(F1, F2, F3)
					.AssertSequenceEqual(await F1(), await F2(), await F3());
				break;
			case 4:
				await AsyncSuperEnumerable.From(F1, F2, F3, F4)
					.AssertSequenceEqual(await F1(), await F2(), await F3(), await F4());
				break;

			default: throw new ArgumentOutOfRangeException(nameof(numArgs));
		}
	}

	[Theory]
	[InlineData(1)]
	[InlineData(2)]
	[InlineData(3)]
	[InlineData(4)]
	public async Task TestFromInvokesMethodsMultipleTimes(int numArgs)
	{
		var evals = new[] { 0, 0, 0, 0 };
		Task<int> F1() { evals[0]++; return Task.FromResult(-2); }

		Task<int> F2() { evals[1]++; return Task.FromResult(-2); }

		Task<int> F3() { evals[2]++; return Task.FromResult(-2); }

		Task<int> F4() { evals[3]++; return Task.FromResult(-2); }

		var results = numArgs switch
		{
			1 => AsyncSuperEnumerable.From(F1),
			2 => AsyncSuperEnumerable.From(F1, F2),
			3 => AsyncSuperEnumerable.From(F1, F2, F3),
			4 => AsyncSuperEnumerable.From(F1, F2, F3, F4),
			_ => throw new ArgumentOutOfRangeException(nameof(numArgs)),
		};

		await results.Consume();
		await results.Consume();
		await results.Consume();

		// numArgs functions were evaluated...
		evals.Take(numArgs).AssertSequenceEqual(Enumerable.Repeat(3, numArgs));
		// safety check: we didn't evaluate functions past numArgs
		evals.Skip(numArgs).AssertSequenceEqual(Enumerable.Repeat(0, evals.Length - numArgs));
	}
}
