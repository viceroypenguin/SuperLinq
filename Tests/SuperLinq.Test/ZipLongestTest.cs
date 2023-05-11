namespace Test;
public class ZipLongestTest
{
	[Fact]
	public void ZipLongestIsLazy()
	{
		var bs = new BreakingSequence<int>();
		_ = bs.ZipLongest(bs, BreakingFunc.Of<int, int, int>());
		_ = bs.ZipLongest(bs, bs, BreakingFunc.Of<int, int, int, int>());
		_ = bs.ZipLongest(bs, bs, bs, BreakingFunc.Of<int, int, int, int, int>());
	}

	[Fact]
	public void TwoParamsDisposesInnerSequencesCaseGetEnumeratorThrows()
	{
		using var s1 = TestingSequence.Of(1, 2);

		_ = Assert.Throws<TestException>(() =>
			s1.ZipLongest(new BreakingSequence<int>()).Consume());
	}

	public static IEnumerable<object[]> GetTwoParamSequences()
	{
		var parameters = new List<object[]>
		{
			new object[] { Enumerable.Range(1, 3).AsTestingSequence(maxEnumerations: 2), Enumerable.Range(1, 3).AsTestingSequence(maxEnumerations: 2), 9, },
			new object[] { Enumerable.Range(1, 3).ToList(), Enumerable.Range(1, 3).AsTestingSequence(maxEnumerations: 2), 9, },
			new object[] { Enumerable.Range(1, 3).AsBreakingList(), Enumerable.Range(1, 3).AsBreakingList(), 9, },
		};

		for (var i = 0; i < 2; i++)
		{
			var first = Enumerable.Range(1, 3 - (i == 0 ? 1 : 0));
			var second = Enumerable.Range(1, 3 - (i == 1 ? 1 : 0));
			parameters.Add(
				new object[] { first.AsBreakingList(), second.AsBreakingList(), i, });
			parameters.Add(
				new object[] { first.AsTestingSequence(maxEnumerations: 2), second.AsTestingSequence(maxEnumerations: 2), i, });
		}

		return parameters;
	}

	[Theory]
	[MemberData(nameof(GetTwoParamSequences))]
	public void TwoParamsWorksProperly(IEnumerable<int> seq1, IEnumerable<int> seq2, int shortSeq)
	{
		using (seq1 as IDisposableEnumerable<int>)
		using (seq2 as IDisposableEnumerable<int>)
		{
			var result = seq1.ZipLongest(seq2);
			result.AssertSequenceEqual(
				Enumerable.Range(1, 2)
					.Select(x => (x, x))
					.Append((
						shortSeq == 0 ? 0 : 3,
						shortSeq == 1 ? 0 : 3)));
		}
	}

	[Fact]
	public void TwoParamsListBehavior()
	{
		using var seq1 = Enumerable.Range(0, 10_000).AsBreakingList();
		using var seq2 = Enumerable.Range(0, 5_000).AsBreakingList();

		var result = seq1.ZipLongest(seq2);
		Assert.Equal(10_000, result.Count());
		Assert.Equal((10, 10), result.ElementAt(10));
		Assert.Equal((50, 50), result.ElementAt(50));
		Assert.Equal((9_950, 0), result.ElementAt(^50));

		result = seq2.ZipLongest(seq1);
		Assert.Equal(10_000, result.Count());
		Assert.Equal((0, 9_950), result.ElementAt(^50));
	}

	[Fact]
	public void ThreeParamsDisposesInnerSequencesCaseGetEnumeratorThrows()
	{
		using var s1 = TestingSequence.Of(1, 2);
		using var s2 = TestingSequence.Of(1, 2);

		_ = Assert.Throws<TestException>(() =>
			s1.ZipLongest(s2, new BreakingSequence<int>()).Consume());
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
				9,
			},
			new object[]
			{
				Enumerable.Range(1, 3).ToList(),
				Enumerable.Range(1, 3).AsTestingSequence(maxEnumerations: 2),
				Enumerable.Range(1, 3).AsTestingSequence(maxEnumerations: 2),
				9,
			},
			new object[]
			{
				Enumerable.Range(1, 3).ToList(),
				Enumerable.Range(1, 3).ToList(),
				Enumerable.Range(1, 3).AsTestingSequence(maxEnumerations: 2),
				9,
			},
			new object[]
			{
				Enumerable.Range(1, 3).AsBreakingList(),
				Enumerable.Range(1, 3).AsBreakingList(),
				Enumerable.Range(1, 3).AsBreakingList(),
				9,
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
					i,
				});
			parameters.Add(
				new object[]
				{
					first.AsTestingSequence(maxEnumerations: 2),
					second.AsTestingSequence(maxEnumerations: 2),
					third.AsTestingSequence(maxEnumerations: 2),
					i,
				});
		}

		return parameters;
	}

	[Theory]
	[MemberData(nameof(GetThreeParamSequences))]
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
					.Append((
						shortSeq == 0 ? 0 : 3,
						shortSeq == 1 ? 0 : 3,
						shortSeq == 2 ? 0 : 3)));
		}
	}

	[Fact]
	public void ThreeParamsListBehavior()
	{
		using var seq1 = Enumerable.Range(0, 10_000).AsBreakingList();
		using var seq2 = Enumerable.Range(0, 5_000).AsBreakingList();
		using var seq3 = Enumerable.Range(0, 5_000).AsBreakingList();

		var result = seq1.ZipLongest(seq2, seq3);
		Assert.Equal(10_000, result.Count());
		Assert.Equal((10, 10, 10), result.ElementAt(10));
		Assert.Equal((50, 50, 50), result.ElementAt(50));
		Assert.Equal((9_950, 0, 0), result.ElementAt(^50));

		result = seq2.ZipLongest(seq1, seq3);
		Assert.Equal(10_000, result.Count());
		Assert.Equal((0, 9_950, 0), result.ElementAt(^50));
	}

	[Fact]
	public void FourParamsDisposesInnerSequencesCaseGetEnumeratorThrows()
	{
		using var s1 = TestingSequence.Of(1, 2);
		using var s2 = TestingSequence.Of(1, 2);
		using var s3 = TestingSequence.Of(1, 2);

		_ = Assert.Throws<TestException>(() =>
			s1.ZipLongest(s2, s3, new BreakingSequence<int>()).Consume());
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
				9,
			},
			new object[]
			{
				Enumerable.Range(1, 3).ToList(),
				Enumerable.Range(1, 3).AsTestingSequence(maxEnumerations: 2),
				Enumerable.Range(1, 3).AsTestingSequence(maxEnumerations: 2),
				Enumerable.Range(1, 3).AsTestingSequence(maxEnumerations: 2),
				9,
			},
			new object[]
			{
				Enumerable.Range(1, 3).ToList(),
				Enumerable.Range(1, 3).ToList(),
				Enumerable.Range(1, 3).AsTestingSequence(maxEnumerations: 2),
				Enumerable.Range(1, 3).AsTestingSequence(maxEnumerations: 2),
				9,
			},
			new object[]
			{
				Enumerable.Range(1, 3).ToList(),
				Enumerable.Range(1, 3).ToList(),
				Enumerable.Range(1, 3).ToList(),
				Enumerable.Range(1, 3).AsTestingSequence(maxEnumerations: 2),
				9,
			},
			new object[]
			{
				Enumerable.Range(1, 3).AsBreakingList(),
				Enumerable.Range(1, 3).AsBreakingList(),
				Enumerable.Range(1, 3).AsBreakingList(),
				Enumerable.Range(1, 3).AsBreakingList(),
				9,
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
					i,
				});
			parameters.Add(
				new object[]
				{
					first.AsTestingSequence(maxEnumerations: 2),
					second.AsTestingSequence(maxEnumerations: 2),
					third.AsTestingSequence(maxEnumerations: 2),
					fourth.AsTestingSequence(maxEnumerations: 2),
					i,
				});
		}

		return parameters;
	}

	[Theory]
	[MemberData(nameof(GetFourParamSequences))]
	public void FourParamsWorksProperly(IEnumerable<int> seq1, IEnumerable<int> seq2, IEnumerable<int> seq3, IEnumerable<int> seq4, int shortSeq)
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
					.Append((
						shortSeq == 0 ? 0 : 3,
						shortSeq == 1 ? 0 : 3,
						shortSeq == 2 ? 0 : 3,
						shortSeq == 3 ? 0 : 3)));
		}
	}

	[Fact]
	public void FourParamsListBehavior()
	{
		using var seq1 = Enumerable.Range(0, 10_000).AsBreakingList();
		using var seq2 = Enumerable.Range(0, 5_000).AsBreakingList();
		using var seq3 = Enumerable.Range(0, 5_000).AsBreakingList();
		using var seq4 = Enumerable.Range(0, 5_000).AsBreakingList();

		var result = seq1.ZipLongest(seq2, seq3, seq4);
		Assert.Equal(10_000, result.Count());
		Assert.Equal((10, 10, 10, 10), result.ElementAt(10));
		Assert.Equal((50, 50, 50, 50), result.ElementAt(50));
		Assert.Equal((9_950, 0, 0, 0), result.ElementAt(^50));

		result = seq2.ZipLongest(seq1, seq3, seq4);
		Assert.Equal(10_000, result.Count());
		Assert.Equal((0, 9_950, 0, 0), result.ElementAt(^50));
	}
}
