namespace SuperLinq.Tests;

public sealed class ZipShortestTest
{
	[Test]
	public void ZipShortestIsLazy()
	{
		var bs = new BreakingSequence<int>();
		_ = bs.ZipShortest(bs, BreakingFunc.Of<int, int, int>());
		_ = bs.ZipShortest(bs, bs, BreakingFunc.Of<int, int, int, int>());
		_ = bs.ZipShortest(bs, bs, bs, BreakingFunc.Of<int, int, int, int, int>());
	}

	[Test]
	public void MoveNextIsNotCalledUnnecessarily3()
	{
		using var s1 = TestingSequence.Of(1, 2);
		using var s2 = TestingSequence.Of(1, 2, 3);
		using var s3 = SeqExceptionAt(3).AsTestingSequence();

		// `TestException` from `BreakingFunc` should not be thrown
		s1.ZipShortest(s2, s3).Consume();
	}

	[Test]
	public void MoveNextIsNotCalledUnnecessarily4()
	{
		using var s1 = TestingSequence.Of(1, 2);
		using var s2 = TestingSequence.Of(1, 2, 3);
		using var s3 = SuperEnumerable
			.From(
				() => 1,
				() => 2,
				BreakingFunc.Of<int>())
			.AsTestingSequence();
		using var s4 = TestingSequence.Of(1, 2, 3);

		// `TestException` from `BreakingFunc` should not be thrown
		s1.ZipShortest(s2, s3, s4).Consume();
	}

	[Test]
	public void TwoParamsDisposesInnerSequencesCaseGetEnumeratorThrows()
	{
		using var s1 = TestingSequence.Of(1, 2);

		_ = Assert.Throws<TestException>(() =>
			s1.ZipShortest(new BreakingSequence<int>()).Consume());
	}

	public static IEnumerable<(IEnumerable<int> seq1, IEnumerable<int> seq2, bool oneShort)> GetTwoParamSequences()
	{
		var parameters = new List<(IEnumerable<int> seq1, IEnumerable<int> seq2, bool oneShort)>
		{
			(Enumerable.Range(1, 3).AsTestingSequence(), Enumerable.Range(1, 3).AsTestingSequence(), false),
			(Enumerable.Range(1, 3).ToList(), Enumerable.Range(1, 3).AsTestingSequence(), false),
			(Enumerable.Range(1, 3).AsBreakingList(), Enumerable.Range(1, 3).AsBreakingList(), false),
		};

		for (var i = 0; i < 2; i++)
		{
			var first = Enumerable.Range(1, 3 - (i == 0 ? 1 : 0));
			var second = Enumerable.Range(1, 3 - (i == 1 ? 1 : 0));

			parameters.Add(
				(first.AsBreakingList(), second.AsBreakingList(), true)
			);

			parameters.Add(
				(first.AsTestingSequence(), second.AsTestingSequence(), true)
			);
		}

		return parameters;
	}

	[Test]
	[MethodDataSource(nameof(GetTwoParamSequences))]
	public void TwoParamsWorksProperly(IEnumerable<int> seq1, IEnumerable<int> seq2, bool oneShort)
	{
		using (seq1 as IDisposableEnumerable<int>)
		using (seq2 as IDisposableEnumerable<int>)
		{
			var result = seq1.ZipShortest(seq2);
			result.AssertSequenceEqual(
				Enumerable.Range(1, oneShort ? 2 : 3)
					.Select(x => (x, x)));
		}
	}

	[Test]
	public void TwoParamsListBehavior()
	{
		using var seq1 = Enumerable.Range(0, 10_000).AsBreakingList();
		using var seq2 = Enumerable.Range(0, 5_000).AsBreakingList();

		var result = seq1.ZipShortest(seq2);
		result.AssertCollectionErrorChecking(5_000);
		result.AssertListElementChecking(5_000);

		Assert.Equal((10, 10), result.ElementAt(10));
		Assert.Equal((50, 50), result.ElementAt(50));
#if !NO_INDEX
		Assert.Equal((4_950, 4_950), result.ElementAt(^50));
#endif

		result = seq2.ZipShortest(seq1);
		result.AssertCollectionErrorChecking(5_000);
		result.AssertListElementChecking(5_000);

#if !NO_INDEX
		Assert.Equal((4_950, 4_950), result.ElementAt(^50));
#endif
	}

	[Test]
	public void ThreeParamsDisposesInnerSequencesCaseGetEnumeratorThrows()
	{
		using var s1 = TestingSequence.Of(1, 2);
		using var s2 = TestingSequence.Of(1, 2);

		_ = Assert.Throws<TestException>(() =>
			s1.ZipShortest(s2, new BreakingSequence<int>()).Consume());
	}

	public static IEnumerable<(IEnumerable<int> seq1, IEnumerable<int> seq2, IEnumerable<int> seq3, bool oneShort)> GetThreeParamSequences()
	{
		var parameters = new List<(IEnumerable<int> seq1, IEnumerable<int> seq2, IEnumerable<int> seq3, bool oneShort)>
		{
			(
				Enumerable.Range(1, 3).AsTestingSequence(),
				Enumerable.Range(1, 3).AsTestingSequence(),
				Enumerable.Range(1, 3).AsTestingSequence(),
				false
			),
			(
				Enumerable.Range(1, 3).ToList(),
				Enumerable.Range(1, 3).AsTestingSequence(),
				Enumerable.Range(1, 3).AsTestingSequence(),
				false
			),
			(
				Enumerable.Range(1, 3).ToList(),
				Enumerable.Range(1, 3).ToList(),
				Enumerable.Range(1, 3).AsTestingSequence(),
				false
			),
			(
				Enumerable.Range(1, 3).AsBreakingList(),
				Enumerable.Range(1, 3).AsBreakingList(),
				Enumerable.Range(1, 3).AsBreakingList(),
				false
			),
		};

		for (var i = 0; i < 3; i++)
		{
			var first = Enumerable.Range(1, 3 - (i == 0 ? 1 : 0));
			var second = Enumerable.Range(1, 3 - (i == 1 ? 1 : 0));
			var third = Enumerable.Range(1, 3 - (i == 2 ? 1 : 0));

			parameters.Add(
				(
					first.AsBreakingList(),
					second.AsBreakingList(),
					third.AsBreakingList(),
					true
				)
			);

			parameters.Add(
				(
					first.AsTestingSequence(),
					second.AsTestingSequence(),
					third.AsTestingSequence(),
					true
				)
			);
		}

		return parameters;
	}

	[Test]
	[MethodDataSource(nameof(GetThreeParamSequences))]
	public void ThreeParamsWorksProperly(IEnumerable<int> seq1, IEnumerable<int> seq2, IEnumerable<int> seq3, bool oneShort)
	{
		using (seq1 as IDisposableEnumerable<int>)
		using (seq2 as IDisposableEnumerable<int>)
		using (seq3 as IDisposableEnumerable<int>)
		{
			var result = seq1.ZipShortest(seq2, seq3);
			result.AssertSequenceEqual(
				Enumerable.Range(1, oneShort ? 2 : 3)
					.Select(x => (x, x, x)));
		}
	}

	[Test]
	public void ThreeParamsListBehavior()
	{
		using var seq1 = Enumerable.Range(0, 10_000).AsBreakingList();
		using var seq2 = Enumerable.Range(0, 5_000).AsBreakingList();
		using var seq3 = Enumerable.Range(0, 5_000).AsBreakingList();

		var result = seq1.ZipShortest(seq2, seq3);
		result.AssertCollectionErrorChecking(5_000);
		result.AssertListElementChecking(5_000);

		Assert.Equal((10, 10, 10), result.ElementAt(10));
		Assert.Equal((50, 50, 50), result.ElementAt(50));
#if !NO_INDEX
		Assert.Equal((4_950, 4_950, 4_950), result.ElementAt(^50));
#endif

		result = seq2.ZipShortest(seq1, seq3);
		result.AssertCollectionErrorChecking(5_000);
		result.AssertListElementChecking(5_000);

#if !NO_INDEX
		Assert.Equal((4_950, 4_950, 4_950), result.ElementAt(^50));
#endif
	}

	[Test]
	public void FourParamsDisposesInnerSequencesCaseGetEnumeratorThrows()
	{
		using var s1 = TestingSequence.Of(1, 2);
		using var s2 = TestingSequence.Of(1, 2);
		using var s3 = TestingSequence.Of(1, 2);

		_ = Assert.Throws<TestException>(() =>
			s1.ZipShortest(s2, s3, new BreakingSequence<int>()).Consume());
	}

	public static IEnumerable<(
		IEnumerable<int> seq1,
		IEnumerable<int> seq2,
		IEnumerable<int> seq3,
		IEnumerable<int> seq4,
		bool oneShort
	)> GetFourParamSequences()
	{
		var parameters = new List<(
			IEnumerable<int> seq1,
			IEnumerable<int> seq2,
			IEnumerable<int> seq3,
			IEnumerable<int> seq4,
			bool oneShort
		)>
		{
			(
				Enumerable.Range(1, 3).AsTestingSequence(),
				Enumerable.Range(1, 3).AsTestingSequence(),
				Enumerable.Range(1, 3).AsTestingSequence(),
				Enumerable.Range(1, 3).AsTestingSequence(),
				false
			),
			(
				Enumerable.Range(1, 3).ToList(),
				Enumerable.Range(1, 3).AsTestingSequence(),
				Enumerable.Range(1, 3).AsTestingSequence(),
				Enumerable.Range(1, 3).AsTestingSequence(),
				false
			),
			(
				Enumerable.Range(1, 3).ToList(),
				Enumerable.Range(1, 3).ToList(),
				Enumerable.Range(1, 3).AsTestingSequence(),
				Enumerable.Range(1, 3).AsTestingSequence(),
				false
			),
			(
				Enumerable.Range(1, 3).ToList(),
				Enumerable.Range(1, 3).ToList(),
				Enumerable.Range(1, 3).ToList(),
				Enumerable.Range(1, 3).AsTestingSequence(),
				false
			),
			(
				Enumerable.Range(1, 3).AsBreakingList(),
				Enumerable.Range(1, 3).AsBreakingList(),
				Enumerable.Range(1, 3).AsBreakingList(),
				Enumerable.Range(1, 3).AsBreakingList(),
				false
			),
		};

		for (var i = 0; i < 4; i++)
		{
			var first = Enumerable.Range(1, 3 - (i == 0 ? 1 : 0));
			var second = Enumerable.Range(1, 3 - (i == 1 ? 1 : 0));
			var third = Enumerable.Range(1, 3 - (i == 2 ? 1 : 0));
			var fourth = Enumerable.Range(1, 3 - (i == 3 ? 1 : 0));

			parameters.Add(
				(
					first.AsBreakingList(),
					second.AsBreakingList(),
					third.AsBreakingList(),
					fourth.AsBreakingList(),
					true
				)
			);

			parameters.Add(
				(
					first.AsTestingSequence(),
					second.AsTestingSequence(),
					third.AsTestingSequence(),
					fourth.AsTestingSequence(),
					true
				)
			);
		}

		return parameters;
	}

	[Test]
	[MethodDataSource(nameof(GetFourParamSequences))]
	public void FourParamsWorksProperly(
		IEnumerable<int> seq1,
		IEnumerable<int> seq2,
		IEnumerable<int> seq3,
		IEnumerable<int> seq4,
		bool oneShort
	)
	{
		using (seq1 as IDisposableEnumerable<int>)
		using (seq2 as IDisposableEnumerable<int>)
		using (seq3 as IDisposableEnumerable<int>)
		using (seq4 as IDisposableEnumerable<int>)
		{
			var result = seq1.ZipShortest(seq2, seq3, seq4);
			result.AssertSequenceEqual(
				Enumerable.Range(1, oneShort ? 2 : 3)
					.Select(x => (x, x, x, x)));
		}
	}

	[Test]
	public void FourParamsListBehavior()
	{
		using var seq1 = Enumerable.Range(0, 10_000).AsBreakingList();
		using var seq2 = Enumerable.Range(0, 5_000).AsBreakingList();
		using var seq3 = Enumerable.Range(0, 5_000).AsBreakingList();
		using var seq4 = Enumerable.Range(0, 5_000).AsBreakingList();

		var result = seq1.ZipShortest(seq2, seq3, seq4);
		result.AssertCollectionErrorChecking(5_000);
		result.AssertListElementChecking(5_000);

		Assert.Equal((10, 10, 10, 10), result.ElementAt(10));
		Assert.Equal((50, 50, 50, 50), result.ElementAt(50));
#if !NO_INDEX
		Assert.Equal((4_950, 4_950, 4_950, 4_950), result.ElementAt(^50));
#endif

		result = seq2.ZipShortest(seq1, seq3, seq4);
		result.AssertCollectionErrorChecking(5_000);
		result.AssertListElementChecking(5_000);

#if !NO_INDEX
		Assert.Equal((4_950, 4_950, 4_950, 4_950), result.ElementAt(^50));
#endif
	}
}
