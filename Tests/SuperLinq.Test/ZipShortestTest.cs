namespace Test;
public class ZipShortestTest
{
	[Fact]
	public void ZipShortestIsLazy()
	{
		var bs = new BreakingSequence<int>();
		_ = bs.ZipShortest(bs, BreakingFunc.Of<int, int, int>());
		_ = bs.ZipShortest(bs, bs, BreakingFunc.Of<int, int, int, int>());
		_ = bs.ZipShortest(bs, bs, bs, BreakingFunc.Of<int, int, int, int, int>());
	}

	[Fact]
	public void TwoParamsDisposesInnerSequencesCaseGetEnumeratorThrows()
	{
		using var s1 = TestingSequence.Of(1, 2);

		_ = Assert.Throws<TestException>(() =>
			s1.ZipShortest(new BreakingSequence<int>()).Consume());
	}

	public static IEnumerable<object[]> GetTwoParamSequences()
	{
		var parameters = new List<object[]>
		{
			new object[] { Enumerable.Range(1, 3).AsTestingSequence(maxEnumerations: 2), Enumerable.Range(1, 3).AsTestingSequence(maxEnumerations: 2), false, },
			new object[] { Enumerable.Range(1, 3).ToList(), Enumerable.Range(1, 3).AsTestingSequence(maxEnumerations: 2), false, },
			new object[] { Enumerable.Range(1, 3).AsBreakingList(), Enumerable.Range(1, 3).AsBreakingList(), false, },
		};

		for (var i = 0; i < 2; i++)
		{
			var first = Enumerable.Range(1, 3 - (i == 0 ? 1 : 0));
			var second = Enumerable.Range(1, 3 - (i == 1 ? 1 : 0));
			parameters.Add(
				new object[] { first.AsBreakingList(), second.AsBreakingList(), true, });
			parameters.Add(
				new object[] { first.AsTestingSequence(maxEnumerations: 2), second.AsTestingSequence(maxEnumerations: 2), true, });
		}

		return parameters;
	}

	[Theory]
	[MemberData(nameof(GetTwoParamSequences))]
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

	[Fact]
	public void TwoParamsListBehavior()
	{
		using var seq1 = Enumerable.Range(0, 10_000).AsBreakingList();
		using var seq2 = Enumerable.Range(0, 5_000).AsBreakingList();

		var result = seq1.ZipShortest(seq2);
		Assert.Equal(5_000, result.Count());
		Assert.Equal((10, 10), result.ElementAt(10));
		Assert.Equal((50, 50), result.ElementAt(50));
		Assert.Equal((4_950, 4_950), result.ElementAt(^50));

		result = seq2.ZipShortest(seq1);
		Assert.Equal(5_000, result.Count());
		Assert.Equal((4_950, 4_950), result.ElementAt(^50));
	}

	[Fact]
	public void ThreeParamsDisposesInnerSequencesCaseGetEnumeratorThrows()
	{
		using var s1 = TestingSequence.Of(1, 2);
		using var s2 = TestingSequence.Of(1, 2);

		_ = Assert.Throws<TestException>(() =>
			s1.ZipShortest(s2, new BreakingSequence<int>()).Consume());
	}

	public static IEnumerable<object[]> GetThreeParamSequences()
	{
		var parameters = new List<object[]>
		{
			new object[]
			{
				Enumerable.Range(1, 3).AsTestingSequence(maxEnumerations: 2),
				Enumerable.Range(1, 3).AsTestingSequence(maxEnumerations: 2),
				Enumerable.Range(1, 3).AsTestingSequence(maxEnumerations: 2),
				false,
			},
			new object[]
			{
				Enumerable.Range(1, 3).ToList(),
				Enumerable.Range(1, 3).AsTestingSequence(maxEnumerations: 2),
				Enumerable.Range(1, 3).AsTestingSequence(maxEnumerations: 2),
				false,
			},
			new object[]
			{
				Enumerable.Range(1, 3).ToList(),
				Enumerable.Range(1, 3).ToList(),
				Enumerable.Range(1, 3).AsTestingSequence(maxEnumerations: 2),
				false,
			},
			new object[]
			{
				Enumerable.Range(1, 3).AsBreakingList(),
				Enumerable.Range(1, 3).AsBreakingList(),
				Enumerable.Range(1, 3).AsBreakingList(),
				false,
			},
		};

		for (var i = 0; i < 3; i++)
		{
			var first = Enumerable.Range(1, 3 - (i == 0 ? 1 : 0));
			var second = Enumerable.Range(1, 3 - (i == 1 ? 1 : 0));
			var third = Enumerable.Range(1, 3 - (i == 2 ? 1 : 0));
			parameters.Add(
				new object[]
				{
					first.AsBreakingList(),
					second.AsBreakingList(),
					third.AsBreakingList(),
					true,
				});
			parameters.Add(
				new object[]
				{
					first.AsTestingSequence(maxEnumerations: 2),
					second.AsTestingSequence(maxEnumerations: 2),
					third.AsTestingSequence(maxEnumerations: 2),
					true,
				});
		}

		return parameters;
	}

	[Theory]
	[MemberData(nameof(GetThreeParamSequences))]
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

	[Fact]
	public void ThreeParamsListBehavior()
	{
		using var seq1 = Enumerable.Range(0, 10_000).AsBreakingList();
		using var seq2 = Enumerable.Range(0, 5_000).AsBreakingList();
		using var seq3 = Enumerable.Range(0, 5_000).AsBreakingList();

		var result = seq1.ZipShortest(seq2, seq3);
		Assert.Equal(5_000, result.Count());
		Assert.Equal((10, 10, 10), result.ElementAt(10));
		Assert.Equal((50, 50, 50), result.ElementAt(50));
		Assert.Equal((4_950, 4_950, 4_950), result.ElementAt(^50));

		result = seq2.ZipShortest(seq1, seq3);
		Assert.Equal(5_000, result.Count());
		Assert.Equal((4_950, 4_950, 4_950), result.ElementAt(^50));
	}

	[Fact]
	public void FourParamsDisposesInnerSequencesCaseGetEnumeratorThrows()
	{
		using var s1 = TestingSequence.Of(1, 2);
		using var s2 = TestingSequence.Of(1, 2);
		using var s3 = TestingSequence.Of(1, 2);

		_ = Assert.Throws<TestException>(() =>
			s1.ZipShortest(s2, s3, new BreakingSequence<int>()).Consume());
	}

	public static IEnumerable<object[]> GetFourParamSequences()
	{
		var parameters = new List<object[]>
		{
			new object[]
			{
				Enumerable.Range(1, 3).AsTestingSequence(maxEnumerations: 2),
				Enumerable.Range(1, 3).AsTestingSequence(maxEnumerations: 2),
				Enumerable.Range(1, 3).AsTestingSequence(maxEnumerations: 2),
				Enumerable.Range(1, 3).AsTestingSequence(maxEnumerations: 2),
				false,
			},
			new object[]
			{
				Enumerable.Range(1, 3).ToList(),
				Enumerable.Range(1, 3).AsTestingSequence(maxEnumerations: 2),
				Enumerable.Range(1, 3).AsTestingSequence(maxEnumerations: 2),
				Enumerable.Range(1, 3).AsTestingSequence(maxEnumerations: 2),
				false,
			},
			new object[]
			{
				Enumerable.Range(1, 3).ToList(),
				Enumerable.Range(1, 3).ToList(),
				Enumerable.Range(1, 3).AsTestingSequence(maxEnumerations: 2),
				Enumerable.Range(1, 3).AsTestingSequence(maxEnumerations: 2),
				false,
			},
			new object[]
			{
				Enumerable.Range(1, 3).ToList(),
				Enumerable.Range(1, 3).ToList(),
				Enumerable.Range(1, 3).ToList(),
				Enumerable.Range(1, 3).AsTestingSequence(maxEnumerations: 2),
				false,
			},
			new object[]
			{
				Enumerable.Range(1, 3).AsBreakingList(),
				Enumerable.Range(1, 3).AsBreakingList(),
				Enumerable.Range(1, 3).AsBreakingList(),
				Enumerable.Range(1, 3).AsBreakingList(),
				false,
			},
		};

		for (var i = 0; i < 4; i++)
		{
			var first = Enumerable.Range(1, 3 - (i == 0 ? 1 : 0));
			var second = Enumerable.Range(1, 3 - (i == 1 ? 1 : 0));
			var third = Enumerable.Range(1, 3 - (i == 2 ? 1 : 0));
			var fourth = Enumerable.Range(1, 3 - (i == 3 ? 1 : 0));
			parameters.Add(
				new object[]
				{
					first.AsBreakingList(),
					second.AsBreakingList(),
					third.AsBreakingList(),
					fourth.AsBreakingList(),
					true,
				});
			parameters.Add(
				new object[]
				{
					first.AsTestingSequence(maxEnumerations: 2),
					second.AsTestingSequence(maxEnumerations: 2),
					third.AsTestingSequence(maxEnumerations: 2),
					fourth.AsTestingSequence(maxEnumerations: 2),
					true,
				});
		}

		return parameters;
	}

	[Theory]
	[MemberData(nameof(GetFourParamSequences))]
	public void FourParamsWorksProperly(IEnumerable<int> seq1, IEnumerable<int> seq2, IEnumerable<int> seq3, IEnumerable<int> seq4, bool oneShort)
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

	[Fact]
	public void FourParamsListBehavior()
	{
		using var seq1 = Enumerable.Range(0, 10_000).AsBreakingList();
		using var seq2 = Enumerable.Range(0, 5_000).AsBreakingList();
		using var seq3 = Enumerable.Range(0, 5_000).AsBreakingList();
		using var seq4 = Enumerable.Range(0, 5_000).AsBreakingList();

		var result = seq1.ZipShortest(seq2, seq3, seq4);
		Assert.Equal(5_000, result.Count());
		Assert.Equal((10, 10, 10, 10), result.ElementAt(10));
		Assert.Equal((50, 50, 50, 50), result.ElementAt(50));
		Assert.Equal((4_950, 4_950, 4_950, 4_950), result.ElementAt(^50));

		result = seq2.ZipShortest(seq1, seq3, seq4);
		Assert.Equal(5_000, result.Count());
		Assert.Equal((4_950, 4_950, 4_950, 4_950), result.ElementAt(^50));
	}
}
