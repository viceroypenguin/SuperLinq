namespace SuperLinq.Tests;
public sealed class ZipLongestTest
{
	[Test]
	public void ZipLongestIsLazy()
	{
		var bs = new BreakingSequence<int>();
		_ = bs.ZipLongest(bs, BreakingFunc.Of<int, int, int>());
		_ = bs.ZipLongest(bs, bs, BreakingFunc.Of<int, int, int, int>());
		_ = bs.ZipLongest(bs, bs, bs, BreakingFunc.Of<int, int, int, int, int>());
	}

	[Test]
	public void TwoParamsDisposesInnerSequencesCaseGetEnumeratorThrows()
	{
		using var s1 = TestingSequence.Of(1, 2);

		_ = Assert.Throws<TestException>(() =>
			s1.ZipLongest(new BreakingSequence<int>()).Consume());
	}

	public static IEnumerable<(IEnumerable<int> seq1, IEnumerable<int> seq2, int shortSeq)> GetTwoParamSequences()
	{
		var parameters = new List<(IEnumerable<int> seq1, IEnumerable<int> seq2, int shortSeq)>
		{
			(Enumerable.Range(1, 3).AsTestingSequence(), Enumerable.Range(1, 3).AsTestingSequence(), 9),
			(Enumerable.Range(1, 3).ToList(), Enumerable.Range(1, 3).AsTestingSequence(), 9),
			(Enumerable.Range(1, 3).AsBreakingList(), Enumerable.Range(1, 3).AsBreakingList(), 9),
		};

		for (var i = 0; i < 2; i++)
		{
			var first = Enumerable.Range(1, 3 - (i == 0 ? 1 : 0));
			var second = Enumerable.Range(1, 3 - (i == 1 ? 1 : 0));

			parameters.Add(
				(first.AsBreakingList(), second.AsBreakingList(), i)
			);

			parameters.Add(
				(first.AsTestingSequence(), second.AsTestingSequence(), i)
			);
		}

		return parameters;
	}

	[Test]
	[MethodDataSource(nameof(GetTwoParamSequences))]
	public void TwoParamsWorksProperly(IEnumerable<int> seq1, IEnumerable<int> seq2, int shortSeq)
	{
		using (seq1 as IDisposableEnumerable<int>)
		using (seq2 as IDisposableEnumerable<int>)
		{
			var result = seq1.ZipLongest(seq2);
			result.AssertSequenceEqual(
				Enumerable.Range(1, 2)
					.Select(x => (x, x))
					.Concat([
						(
							shortSeq == 0 ? 0 : 3,
							shortSeq == 1 ? 0 : 3
						),
					]));
		}
	}

	[Test]
	public void TwoParamsListBehavior()
	{
		using var seq1 = Enumerable.Range(0, 10_000).AsBreakingList();
		using var seq2 = Enumerable.Range(0, 5_000).AsBreakingList();

		var result = seq1.ZipLongest(seq2);
		result.AssertCollectionErrorChecking(10_000);
		result.AssertListElementChecking(10_000);

		Assert.Equal((10, 10), result.ElementAt(10));
		Assert.Equal((50, 50), result.ElementAt(50));
#if !NO_INDEX
		Assert.Equal((9_950, 0), result.ElementAt(^50));
#endif

		result = seq2.ZipLongest(seq1);
		result.AssertCollectionErrorChecking(10_000);
		result.AssertListElementChecking(10_000);

#if !NO_INDEX
		Assert.Equal((0, 9_950), result.ElementAt(^50));
#endif
	}

	[Test]
	public void ThreeParamsDisposesInnerSequencesCaseGetEnumeratorThrows()
	{
		using var s1 = TestingSequence.Of(1, 2);
		using var s2 = TestingSequence.Of(1, 2);

		_ = Assert.Throws<TestException>(() =>
			s1.ZipLongest(s2, new BreakingSequence<int>()).Consume());
	}

	public static IEnumerable<(IEnumerable<int> seq1, IEnumerable<int> seq2, IEnumerable<int> seq3, int shortSeq)> GetThreeParamSequences()
	{
		var parameters = new List<(IEnumerable<int> seq1, IEnumerable<int> seq2, IEnumerable<int> seq3, int shortSeq)>
		{
			(
				Enumerable.Range(1, 3).AsTestingSequence(),
				Enumerable.Range(1, 3).AsTestingSequence(),
				Enumerable.Range(1, 3).AsTestingSequence(),
				9
			),
			(
				Enumerable.Range(1, 3).ToList(),
				Enumerable.Range(1, 3).AsTestingSequence(),
				Enumerable.Range(1, 3).AsTestingSequence(),
				9
			),
			(
				Enumerable.Range(1, 3).ToList(),
				Enumerable.Range(1, 3).ToList(),
				Enumerable.Range(1, 3).AsTestingSequence(),
				9
			),
			(
				Enumerable.Range(1, 3).AsBreakingList(),
				Enumerable.Range(1, 3).AsBreakingList(),
				Enumerable.Range(1, 3).AsBreakingList(),
				9
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
					i
				)
			);

			parameters.Add(
				(
					first.AsTestingSequence(),
					second.AsTestingSequence(),
					third.AsTestingSequence(),
					i
				)
			);
		}

		return parameters;
	}

	[Test]
	[MethodDataSource(nameof(GetThreeParamSequences))]
	public void ThreeParamsWorksProperly(IEnumerable<int> seq1, IEnumerable<int> seq2, IEnumerable<int> seq3, int shortSeq)
	{
		using (seq1 as IDisposableEnumerable<int>)
		using (seq2 as IDisposableEnumerable<int>)
		using (seq3 as IDisposableEnumerable<int>)
		{
			var result = seq1.ZipLongest(seq2, seq3);
			result.AssertSequenceEqual(
				Enumerable.Range(1, 2)
					.Select(x => (x, x, x))
					.Concat([
						(
							shortSeq == 0 ? 0 : 3,
							shortSeq == 1 ? 0 : 3,
							shortSeq == 2 ? 0 : 3
						),
					]));
		}
	}

	[Test]
	public void ThreeParamsListBehavior()
	{
		using var seq1 = Enumerable.Range(0, 10_000).AsBreakingList();
		using var seq2 = Enumerable.Range(0, 5_000).AsBreakingList();
		using var seq3 = Enumerable.Range(0, 5_000).AsBreakingList();

		var result = seq1.ZipLongest(seq2, seq3);
		result.AssertCollectionErrorChecking(10_000);
		result.AssertListElementChecking(10_000);

		Assert.Equal((10, 10, 10), result.ElementAt(10));
		Assert.Equal((50, 50, 50), result.ElementAt(50));
#if !NO_INDEX
		Assert.Equal((9_950, 0, 0), result.ElementAt(^50));
#endif
		result = seq2.ZipLongest(seq1, seq3);
		result.AssertCollectionErrorChecking(10_000);
		result.AssertListElementChecking(10_000);

#if !NO_INDEX
		Assert.Equal((0, 9_950, 0), result.ElementAt(^50));
#endif
	}

	[Test]
	public void FourParamsDisposesInnerSequencesCaseGetEnumeratorThrows()
	{
		using var s1 = TestingSequence.Of(1, 2);
		using var s2 = TestingSequence.Of(1, 2);
		using var s3 = TestingSequence.Of(1, 2);

		_ = Assert.Throws<TestException>(() =>
			s1.ZipLongest(s2, s3, new BreakingSequence<int>()).Consume());
	}

	public static IEnumerable<(
		IEnumerable<int> seq1,
		IEnumerable<int> seq2,
		IEnumerable<int> seq3,
		IEnumerable<int> seq4,
		int shortSeq
	)> GetFourParamSequences()
	{
		var parameters = new List<(
			IEnumerable<int> seq1,
			IEnumerable<int> seq2,
			IEnumerable<int> seq3,
			IEnumerable<int> seq4,
			int shortSeq
		)>
		{
			(
				Enumerable.Range(1, 3).AsTestingSequence(),
				Enumerable.Range(1, 3).AsTestingSequence(),
				Enumerable.Range(1, 3).AsTestingSequence(),
				Enumerable.Range(1, 3).AsTestingSequence(),
				9
			),
			(
				Enumerable.Range(1, 3).ToList(),
				Enumerable.Range(1, 3).AsTestingSequence(),
				Enumerable.Range(1, 3).AsTestingSequence(),
				Enumerable.Range(1, 3).AsTestingSequence(),
				9
			),
			(
				Enumerable.Range(1, 3).ToList(),
				Enumerable.Range(1, 3).ToList(),
				Enumerable.Range(1, 3).AsTestingSequence(),
				Enumerable.Range(1, 3).AsTestingSequence(),
				9
			),
			(
				Enumerable.Range(1, 3).ToList(),
				Enumerable.Range(1, 3).ToList(),
				Enumerable.Range(1, 3).ToList(),
				Enumerable.Range(1, 3).AsTestingSequence(),
				9
			),
			(
				Enumerable.Range(1, 3).AsBreakingList(),
				Enumerable.Range(1, 3).AsBreakingList(),
				Enumerable.Range(1, 3).AsBreakingList(),
				Enumerable.Range(1, 3).AsBreakingList(),
				9
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
					i
				));
			parameters.Add(
				(
					first.AsTestingSequence(),
					second.AsTestingSequence(),
					third.AsTestingSequence(),
					fourth.AsTestingSequence(),
					i
				));
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
		int shortSeq
	)
	{
		using (seq1 as IDisposableEnumerable<int>)
		using (seq2 as IDisposableEnumerable<int>)
		using (seq3 as IDisposableEnumerable<int>)
		using (seq4 as IDisposableEnumerable<int>)
		{
			var result = seq1.ZipLongest(seq2, seq3, seq4);
			result.AssertSequenceEqual(
				Enumerable.Range(1, 2)
					.Select(x => (x, x, x, x))
					.Concat([
						(
							shortSeq == 0 ? 0 : 3,
							shortSeq == 1 ? 0 : 3,
							shortSeq == 2 ? 0 : 3,
							shortSeq == 3 ? 0 : 3
						),
					]));
		}
	}

	[Test]
	public void FourParamsListBehavior()
	{
		using var seq1 = Enumerable.Range(0, 10_000).AsBreakingList();
		using var seq2 = Enumerable.Range(0, 5_000).AsBreakingList();
		using var seq3 = Enumerable.Range(0, 5_000).AsBreakingList();
		using var seq4 = Enumerable.Range(0, 5_000).AsBreakingList();

		var result = seq1.ZipLongest(seq2, seq3, seq4);
		result.AssertCollectionErrorChecking(10_000);
		result.AssertListElementChecking(10_000);

		Assert.Equal((10, 10, 10, 10), result.ElementAt(10));
		Assert.Equal((50, 50, 50, 50), result.ElementAt(50));
#if !NO_INDEX
		Assert.Equal((9_950, 0, 0, 0), result.ElementAt(^50));
#endif

		result = seq2.ZipLongest(seq1, seq3, seq4);
		result.AssertCollectionErrorChecking(10_000);
		result.AssertListElementChecking(10_000);

#if !NO_INDEX
		Assert.Equal((0, 9_950, 0, 0), result.ElementAt(^50));
#endif
	}
}
