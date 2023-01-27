namespace Test;

public class EquiZipTest
{
	[Fact]
	public void EquiZipIsLazy()
	{
		var bs = new BreakingSequence<int>();
		_ = bs.EquiZip(bs, BreakingFunc.Of<int, int, int>());
		_ = bs.EquiZip(bs, bs, BreakingFunc.Of<int, int, int, int>());
		_ = bs.EquiZip(bs, bs, bs, BreakingFunc.Of<int, int, int, int, int>());
	}

	[Fact]
	public void MoveNextIsNotCalledUnnecessarily3()
	{
		using var s1 = TestingSequence.Of(1, 2);
		using var s2 = TestingSequence.Of(1, 2, 3);
		using var s3 = SuperEnumerable
			.From(
				() => 1,
				() => 2,
				BreakingFunc.Of<int>())
			.AsTestingSequence();

		// `TestException` from `BreakingFunc` should not be thrown
		Assert.Throws<InvalidOperationException>(() =>
			s1.EquiZip(s2, s3).Consume());
	}

	[Fact]
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
		Assert.Throws<InvalidOperationException>(() =>
			s1.EquiZip(s2, s3, s4).Consume());
	}

	[Fact]
	public void TwoParamsWorksCorrectly()
	{
		using var seq1 = Enumerable.Range(1, 3).AsTestingSequence();
		using var seq2 = Enumerable.Range(1, 3).AsTestingSequence();

		seq1.EquiZip(seq2)
			.AssertSequenceEqual(
				Enumerable.Range(1, 3).Select(x => (x, x)));
	}

	[Theory]
	[InlineData(1, "First")]
	[InlineData(2, "Second")]
	public void TwoParamsThrowsOnCorrectSequence(int shortSequence, string cardinal)
	{
		using var seq1 = Enumerable.Range(1, shortSequence == 1 ? 2 : 3).AsTestingSequence();
		using var seq2 = Enumerable.Range(1, shortSequence == 2 ? 2 : 3).AsTestingSequence();

		var ex = Assert.Throws<InvalidOperationException>(() =>
			seq1.EquiZip(seq2).Consume());
		Assert.Equal($"{cardinal} sequence too short.", ex.Message);
	}

	[Fact]
	public void ThreeParamsWorksCorrectly()
	{
		using var seq1 = Enumerable.Range(1, 3).AsTestingSequence();
		using var seq2 = Enumerable.Range(1, 3).AsTestingSequence();
		using var seq3 = Enumerable.Range(1, 3).AsTestingSequence();

		seq1.EquiZip(seq2, seq3)
			.AssertSequenceEqual(
				Enumerable.Range(1, 3).Select(x => (x, x, x)));
	}

	[Theory]
	[InlineData(1, "First")]
	[InlineData(2, "Second")]
	[InlineData(3, "Third")]
	public void ThreeParamsThrowsOnCorrectSequence(int shortSequence, string cardinal)
	{
		using var seq1 = Enumerable.Range(1, shortSequence == 1 ? 2 : 3).AsTestingSequence();
		using var seq2 = Enumerable.Range(1, shortSequence == 2 ? 2 : 3).AsTestingSequence();
		using var seq3 = Enumerable.Range(1, shortSequence == 3 ? 2 : 3).AsTestingSequence();

		var ex = Assert.Throws<InvalidOperationException>(() =>
			seq1.EquiZip(seq2, seq3).Consume());
		Assert.Equal($"{cardinal} sequence too short.", ex.Message);
	}

	[Fact]
	public void FourParamsWorksCorrectly()
	{
		using var seq1 = Enumerable.Range(1, 3).AsTestingSequence();
		using var seq2 = Enumerable.Range(1, 3).AsTestingSequence();
		using var seq3 = Enumerable.Range(1, 3).AsTestingSequence();
		using var seq4 = Enumerable.Range(1, 3).AsTestingSequence();

		seq1.EquiZip(seq2, seq3, seq4)
			.AssertSequenceEqual(
				Enumerable.Range(1, 3).Select(x => (x, x, x, x)));
	}

	[Theory]
	[InlineData(1, "First")]
	[InlineData(2, "Second")]
	[InlineData(3, "Third")]
	[InlineData(4, "Fourth")]
	public void FourParamsThrowsOnCorrectSequence(int shortSequence, string cardinal)
	{
		using var seq1 = Enumerable.Range(1, shortSequence == 1 ? 2 : 3).AsTestingSequence();
		using var seq2 = Enumerable.Range(1, shortSequence == 2 ? 2 : 3).AsTestingSequence();
		using var seq3 = Enumerable.Range(1, shortSequence == 3 ? 2 : 3).AsTestingSequence();
		using var seq4 = Enumerable.Range(1, shortSequence == 4 ? 2 : 3).AsTestingSequence();

		var ex = Assert.Throws<InvalidOperationException>(() =>
			seq1.EquiZip(seq2, seq3, seq4).Consume());
		Assert.Equal($"{cardinal} sequence too short.", ex.Message);
	}

	[Fact]
	public void ZipDisposesInnerSequencesCaseGetEnumeratorThrows()
	{
		using var s1 = TestingSequence.Of(1, 2);

		Assert.Throws<TestException>(() =>
			s1.EquiZip(new BreakingSequence<int>(), Tuple.Create).Consume());
	}
}
