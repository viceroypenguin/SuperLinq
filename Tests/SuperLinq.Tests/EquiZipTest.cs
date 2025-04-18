using System.Diagnostics.CodeAnalysis;

namespace SuperLinq.Tests;

public sealed class EquiZipTest
{
	private static readonly string[] s_cardinals =
	[
		"Zeroth",
		"First",
		"Second",
		"Third",
		"Fourth",
		"Fifth",
		"Sixth",
		"Seventh",
		"Eighth",
	];

	[Test]
	public void EquiZipIsLazy()
	{
		var bs = new BreakingSequence<int>();
		_ = bs.EquiZip(bs, BreakingFunc.Of<int, int, int>());
		_ = bs.EquiZip(bs, bs, BreakingFunc.Of<int, int, int, int>());
		_ = bs.EquiZip(bs, bs, bs, BreakingFunc.Of<int, int, int, int, int>());
	}

	[Test]
	public void MoveNextIsNotCalledUnnecessarily3()
	{
		using var s1 = TestingSequence.Of(1, 2);
		using var s2 = TestingSequence.Of(1, 2, 3);
		using var s3 = SeqExceptionAt(3).AsTestingSequence();

		// `TestException` from `BreakingFunc` should not be thrown
		_ = Assert.Throws<InvalidOperationException>(() =>
			s1.EquiZip(s2, s3).Consume());
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
		_ = Assert.Throws<InvalidOperationException>(() =>
			s1.EquiZip(s2, s3, s4).Consume());
	}

	#region Two
	[Test]
	public void TwoParamsDisposesInnerSequencesCaseGetEnumeratorThrows()
	{
		using var s1 = TestingSequence.Of(1, 2);

		_ = Assert.Throws<TestException>(() =>
			s1.EquiZip(new BreakingSequence<int>()).Consume());
	}

	[SuppressMessage(
		"Reliability",
		"CA2000:Dispose objects before losing scope",
		Justification = "Will be properly disposed in method"
	)]
	public static IEnumerable<(IEnumerable<int> seq1, IEnumerable<int> seq2)> GetTwoParamEqualSequences() =>
		[
			(Enumerable.Range(1, 3).AsTestingSequence(), Enumerable.Range(1, 3).AsTestingSequence()),
			(Enumerable.Range(1, 3).ToList(), Enumerable.Range(1, 3).AsTestingSequence()),
			(Enumerable.Range(1, 3).AsBreakingList(), Enumerable.Range(1, 3).AsBreakingList()),
		];

	[Test]
	[MethodDataSource(nameof(GetTwoParamEqualSequences))]
	public void TwoParamsWorksProperly(IEnumerable<int> seq1, IEnumerable<int> seq2)
	{
		using (seq1 as IDisposableEnumerable<int>)
		using (seq2 as IDisposableEnumerable<int>)
		{
			var result = seq1.EquiZip(seq2);
			result.AssertSequenceEqual(
				Enumerable.Range(1, 3)
					.Select(x => (x, x)));
		}
	}

	public static IEnumerable<(IEnumerable<int> seq1, IEnumerable<int> seq2, string cardinal)> GetTwoParamInequalSequences()
	{
		var parameters = new List<(IEnumerable<int> seq1, IEnumerable<int> seq2, string cardinal)>();

		for (var i = 0; i < 2; i++)
		{
			var first = Enumerable.Range(1, 3 - (i == 0 ? 1 : 0));
			var second = Enumerable.Range(1, 3 - (i == 1 ? 1 : 0));

			parameters.Add(
				(first.AsBreakingList(), second.AsBreakingList(), s_cardinals[i + 1])
			);

			parameters.Add(
				(first.AsTestingSequence(), second.AsTestingSequence(), s_cardinals[i + 1])
			);
		}

		return parameters;
	}

	[Test]
	[MethodDataSource(nameof(GetTwoParamInequalSequences))]
	public void TwoParamsThrowsProperly(IEnumerable<int> seq1, IEnumerable<int> seq2, string cardinal)
	{
		using (seq1 as IDisposableEnumerable<int>)
		using (seq2 as IDisposableEnumerable<int>)
		{
			var result = seq1.EquiZip(seq2);
			var ex = Assert.Throws<InvalidOperationException>(() =>
				result.Consume());
			Assert.Equal($"{cardinal} sequence too short.", ex.Message);
		}
	}

	[Test]
	public void TwoParamsEqualListBehavior()
	{
		using var seq1 = Enumerable.Range(0, 10_000).AsBreakingList();
		using var seq2 = Enumerable.Range(0, 10_000).AsBreakingList();

		var result = seq1.EquiZip(seq2);
		Assert.Equal(10_000, result.Count());
		Assert.Equal((10, 10), result.ElementAt(10));
		Assert.Equal((50, 50), result.ElementAt(50));
#if !NO_INDEX
		Assert.Equal((9_950, 9_950), result.ElementAt(^50));
#endif
	}

	[Test]
	public void TwoParamsInequalListBehavior()
	{
		using var seq1 = Enumerable.Range(0, 10_000).AsBreakingList();
		using var seq2 = Enumerable.Range(0, 5_000).AsBreakingList();

		var result = seq1.EquiZip(seq2);
		_ = Assert.Throws<InvalidOperationException>(() => result.Count());
		_ = Assert.Throws<InvalidOperationException>(() => result.ElementAt(10));
	}
	#endregion

	#region Three
	[Test]
	public void ThreeParamsDisposesInnerSequencesCaseGetEnumeratorThrows()
	{
		using var s1 = TestingSequence.Of(1, 2);
		using var s2 = TestingSequence.Of(1, 2);

		_ = Assert.Throws<TestException>(() =>
			s1.EquiZip(s2, new BreakingSequence<int>()).Consume());
	}

	[SuppressMessage(
		"Reliability",
		"CA2000:Dispose objects before losing scope",
		Justification = "Will be properly disposed in method"
	)]
	public static IEnumerable<(IEnumerable<int> seq1, IEnumerable<int> seq2, IEnumerable<int> seq3)> GetThreeParamEqualSequences() =>
		[
			(
				Enumerable.Range(1, 3).AsTestingSequence(),
				Enumerable.Range(1, 3).AsTestingSequence(),
				Enumerable.Range(1, 3).AsTestingSequence()
			),
			(
				Enumerable.Range(1, 3).ToList(),
				Enumerable.Range(1, 3).AsTestingSequence(),
				Enumerable.Range(1, 3).AsTestingSequence()
			),
			(
				Enumerable.Range(1, 3).ToList(),
				Enumerable.Range(1, 3).ToList(),
				Enumerable.Range(1, 3).AsTestingSequence()
			),
			(
				Enumerable.Range(1, 3).AsBreakingList(),
				Enumerable.Range(1, 3).AsBreakingList(),
				Enumerable.Range(1, 3).AsBreakingList()
			),
		];

	[Test]
	[MethodDataSource(nameof(GetThreeParamEqualSequences))]
	public void ThreeParamsWorksProperly(IEnumerable<int> seq1, IEnumerable<int> seq2, IEnumerable<int> seq3)
	{
		using (seq1 as IDisposableEnumerable<int>)
		using (seq2 as IDisposableEnumerable<int>)
		using (seq3 as IDisposableEnumerable<int>)
		{
			var result = seq1.EquiZip(seq2, seq3);
			result.AssertSequenceEqual(
				Enumerable.Range(1, 3)
					.Select(x => (x, x, x)));
		}
	}

	public static IEnumerable<(IEnumerable<int> seq1, IEnumerable<int> seq2, IEnumerable<int> seq3, string cardinal)> GetThreeParamInequalSequences()
	{
		var parameters = new List<(IEnumerable<int> seq1, IEnumerable<int> seq2, IEnumerable<int> seq3, string cardinal)>();

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
					s_cardinals[i + 1]
				)
			);

			parameters.Add(
				(
					first.AsTestingSequence(),
					second.AsTestingSequence(),
					third.AsTestingSequence(),
					s_cardinals[i + 1]
				)
			);
		}

		return parameters;
	}

	[Test]
	[MethodDataSource(nameof(GetThreeParamInequalSequences))]
	public void ThreeParamsThrowsProperly(IEnumerable<int> seq1, IEnumerable<int> seq2, IEnumerable<int> seq3, string cardinal)
	{
		using (seq1 as IDisposableEnumerable<int>)
		using (seq2 as IDisposableEnumerable<int>)
		using (seq3 as IDisposableEnumerable<int>)
		{
			var result = seq1.EquiZip(seq2, seq3);
			var ex = Assert.Throws<InvalidOperationException>(() =>
				result.Consume());
			Assert.Equal($"{cardinal} sequence too short.", ex.Message);
		}
	}

	[Test]
	public void ThreeParamsListBehavior()
	{
		using var seq1 = Enumerable.Range(0, 10_000).AsBreakingList();
		using var seq2 = Enumerable.Range(0, 10_000).AsBreakingList();
		using var seq3 = Enumerable.Range(0, 10_000).AsBreakingList();

		var result = seq1.EquiZip(seq2, seq3);
		Assert.Equal(10_000, result.Count());
		Assert.Equal((10, 10, 10), result.ElementAt(10));
		Assert.Equal((50, 50, 50), result.ElementAt(50));
#if !NO_INDEX
		Assert.Equal((9_950, 9_950, 9_950), result.ElementAt(^50));
#endif
	}

	[Test]
	public void ThreeParamsInequalListBehavior()
	{
		using var seq1 = Enumerable.Range(0, 10_000).AsBreakingList();
		using var seq2 = Enumerable.Range(0, 5_000).AsBreakingList();
		using var seq3 = Enumerable.Range(0, 5_000).AsBreakingList();

		var result = seq1.EquiZip(seq2, seq3);
		_ = Assert.Throws<InvalidOperationException>(() => result.Count());
		_ = Assert.Throws<InvalidOperationException>(() => result.ElementAt(10));
	}
	#endregion

	#region Four
	[Test]
	public void FourParamsDisposesInnerSequencesCaseGetEnumeratorThrows()
	{
		using var s1 = TestingSequence.Of(1, 2);
		using var s2 = TestingSequence.Of(1, 2);
		using var s3 = TestingSequence.Of(1, 2);

		_ = Assert.Throws<TestException>(() =>
			s1.EquiZip(s2, s3, new BreakingSequence<int>()).Consume());
	}

	[SuppressMessage(
		"Reliability",
		"CA2000:Dispose objects before losing scope",
		Justification = "Will be properly disposed in method"
	)]
	public static IEnumerable<(
		IEnumerable<int> seq1,
		IEnumerable<int> seq2,
		IEnumerable<int> seq3,
		IEnumerable<int> seq4
	)> GetFourParamEqualSequences() =>
		[
			(
				Enumerable.Range(1, 3).AsTestingSequence(),
				Enumerable.Range(1, 3).AsTestingSequence(),
				Enumerable.Range(1, 3).AsTestingSequence(),
				Enumerable.Range(1, 3).AsTestingSequence()
			),
			(
				Enumerable.Range(1, 3).ToList(),
				Enumerable.Range(1, 3).AsTestingSequence(),
				Enumerable.Range(1, 3).AsTestingSequence(),
				Enumerable.Range(1, 3).AsTestingSequence()
			),
			(
				Enumerable.Range(1, 3).ToList(),
				Enumerable.Range(1, 3).ToList(),
				Enumerable.Range(1, 3).AsTestingSequence(),
				Enumerable.Range(1, 3).AsTestingSequence()
			),
			(
				Enumerable.Range(1, 3).ToList(),
				Enumerable.Range(1, 3).ToList(),
				Enumerable.Range(1, 3).ToList(),
				Enumerable.Range(1, 3).AsTestingSequence()
			),
			(
				Enumerable.Range(1, 3).AsBreakingList(),
				Enumerable.Range(1, 3).AsBreakingList(),
				Enumerable.Range(1, 3).AsBreakingList(),
				Enumerable.Range(1, 3).AsBreakingList()
			),
		];

	[Test]
	[MethodDataSource(nameof(GetFourParamEqualSequences))]
	public void FourParamsWorksProperly(IEnumerable<int> seq1, IEnumerable<int> seq2, IEnumerable<int> seq3, IEnumerable<int> seq4)
	{
		using (seq1 as IDisposableEnumerable<int>)
		using (seq2 as IDisposableEnumerable<int>)
		using (seq3 as IDisposableEnumerable<int>)
		using (seq4 as IDisposableEnumerable<int>)
		{
			var result = seq1.EquiZip(seq2, seq3, seq4);
			result.AssertSequenceEqual(
				Enumerable.Range(1, 3)
					.Select(x => (x, x, x, x)));
		}
	}

	public static IEnumerable<(
		IEnumerable<int> seq1,
		IEnumerable<int> seq2,
		IEnumerable<int> seq3,
		IEnumerable<int> seq4,
		string cardinal
	)> GetFourParamInequalSequences()
	{
		var parameters = new List<(
			IEnumerable<int> seq1,
			IEnumerable<int> seq2,
			IEnumerable<int> seq3,
			IEnumerable<int> seq4,
			string cardinal
		)>();

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
					s_cardinals[i + 1]
				)
			);

			parameters.Add(
				(
					first.AsTestingSequence(),
					second.AsTestingSequence(),
					third.AsTestingSequence(),
					fourth.AsTestingSequence(),
					s_cardinals[i + 1]
				)
			);
		}

		return parameters;
	}

	[Test]
	[MethodDataSource(nameof(GetFourParamInequalSequences))]
	public void FourParamsThrowsProperly(IEnumerable<int> seq1, IEnumerable<int> seq2, IEnumerable<int> seq3, IEnumerable<int> seq4, string cardinal)
	{
		using (seq1 as IDisposableEnumerable<int>)
		using (seq2 as IDisposableEnumerable<int>)
		using (seq3 as IDisposableEnumerable<int>)
		using (seq4 as IDisposableEnumerable<int>)
		{
			var result = seq1.EquiZip(seq2, seq3, seq4);
			var ex = Assert.Throws<InvalidOperationException>(() =>
				result.Consume());
			Assert.Equal($"{cardinal} sequence too short.", ex.Message);
		}
	}

	[Test]
	public void FourParamsListBehavior()
	{
		using var seq1 = Enumerable.Range(0, 10_000).AsBreakingList();
		using var seq2 = Enumerable.Range(0, 10_000).AsBreakingList();
		using var seq3 = Enumerable.Range(0, 10_000).AsBreakingList();
		using var seq4 = Enumerable.Range(0, 10_000).AsBreakingList();

		var result = seq1.EquiZip(seq2, seq3, seq4);
		Assert.Equal(10_000, result.Count());
		Assert.Equal((10, 10, 10, 10), result.ElementAt(10));
		Assert.Equal((50, 50, 50, 50), result.ElementAt(50));
#if !NO_INDEX
		Assert.Equal((9_950, 9_950, 9_950, 9_950), result.ElementAt(^50));
#endif
	}

	[Test]
	public void FourParamsInequalListBehavior()
	{
		using var seq1 = Enumerable.Range(0, 10_000).AsBreakingList();
		using var seq2 = Enumerable.Range(0, 5_000).AsBreakingList();
		using var seq3 = Enumerable.Range(0, 5_000).AsBreakingList();
		using var seq4 = Enumerable.Range(0, 5_000).AsBreakingList();

		var result = seq1.EquiZip(seq2, seq3, seq4);
		_ = Assert.Throws<InvalidOperationException>(() => result.Count());
		_ = Assert.Throws<InvalidOperationException>(() => result.ElementAt(10));
	}
	#endregion
}
