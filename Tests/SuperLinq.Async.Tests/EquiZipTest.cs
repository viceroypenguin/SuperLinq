namespace SuperLinq.Async.Tests;

public sealed class EquiZipTest
{
	[Fact]
	public void EquiZipIsLazy()
	{
		var bs = new AsyncBreakingSequence<int>();
		_ = bs.EquiZip(bs, BreakingFunc.Of<int, int, int>());
		_ = bs.EquiZip(bs, bs, BreakingFunc.Of<int, int, int, int>());
		_ = bs.EquiZip(bs, bs, bs, BreakingFunc.Of<int, int, int, int, int>());
	}

	[Fact]
	public async Task MoveNextIsNotCalledUnnecessarily3()
	{
		await using var s1 = TestingSequence.Of(1, 2);
		await using var s2 = TestingSequence.Of(1, 2, 3);
		await using var s3 = AsyncSuperEnumerable
			.From(
				() => Task.FromResult(1),
				() => Task.FromResult(2),
				AsyncBreakingFunc.Of<int>())
			.AsTestingSequence();

		// `TestException` from `BreakingFunc` should not be thrown
		_ = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
			await s1.EquiZip(s2, s3).Consume());
	}

	[Fact]
	public async Task MoveNextIsNotCalledUnnecessarily4()
	{
		await using var s1 = TestingSequence.Of(1, 2);
		await using var s2 = TestingSequence.Of(1, 2, 3);
		await using var s3 = AsyncSuperEnumerable
			.From(
				() => Task.FromResult(1),
				() => Task.FromResult(2),
				AsyncBreakingFunc.Of<int>())
			.AsTestingSequence();
		await using var s4 = TestingSequence.Of(1, 2, 3);

		// `TestException` from `BreakingFunc` should not be thrown
		_ = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
			await s1.EquiZip(s2, s3, s4).Consume());
	}

	[Fact]
	public async Task TwoParamsWorksCorrectly()
	{
		await using var seq1 = Enumerable.Range(1, 3).AsTestingSequence();
		await using var seq2 = Enumerable.Range(1, 3).AsTestingSequence();

		await seq1.EquiZip(seq2)
			.AssertSequenceEqual(
				Enumerable.Range(1, 3).Select(x => (x, x)));
	}

	[Theory]
	[InlineData(1, "First")]
	[InlineData(2, "Second")]
	public async Task TwoParamsThrowsOnCorrectSequence(int shortSequence, string cardinal)
	{
		await using var seq1 = Enumerable.Range(1, shortSequence == 1 ? 2 : 3).AsTestingSequence();
		await using var seq2 = Enumerable.Range(1, shortSequence == 2 ? 2 : 3).AsTestingSequence();

		var ex = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
			await seq1.EquiZip(seq2).Consume());
		Assert.Equal($"{cardinal} sequence too short.", ex.Message);
	}

	[Fact]
	public async Task ThreeParamsWorksCorrectly()
	{
		await using var seq1 = Enumerable.Range(1, 3).AsTestingSequence();
		await using var seq2 = Enumerable.Range(1, 3).AsTestingSequence();
		await using var seq3 = Enumerable.Range(1, 3).AsTestingSequence();

		await seq1.EquiZip(seq2, seq3)
			.AssertSequenceEqual(
				Enumerable.Range(1, 3).Select(x => (x, x, x)));
	}

	[Theory]
	[InlineData(1, "First")]
	[InlineData(2, "Second")]
	[InlineData(3, "Third")]
	public async Task ThreeParamsThrowsOnCorrectSequence(int shortSequence, string cardinal)
	{
		await using var seq1 = Enumerable.Range(1, shortSequence == 1 ? 2 : 3).AsTestingSequence();
		await using var seq2 = Enumerable.Range(1, shortSequence == 2 ? 2 : 3).AsTestingSequence();
		await using var seq3 = Enumerable.Range(1, shortSequence == 3 ? 2 : 3).AsTestingSequence();

		var ex = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
			await seq1.EquiZip(seq2, seq3).Consume());
		Assert.Equal($"{cardinal} sequence too short.", ex.Message);
	}

	[Fact]
	public async Task FourParamsWorksCorrectly()
	{
		await using var seq1 = Enumerable.Range(1, 3).AsTestingSequence();
		await using var seq2 = Enumerable.Range(1, 3).AsTestingSequence();
		await using var seq3 = Enumerable.Range(1, 3).AsTestingSequence();
		await using var seq4 = Enumerable.Range(1, 3).AsTestingSequence();

		await seq1.EquiZip(seq2, seq3, seq4)
			.AssertSequenceEqual(
				Enumerable.Range(1, 3).Select(x => (x, x, x, x)));
	}

	[Theory]
	[InlineData(1, "First")]
	[InlineData(2, "Second")]
	[InlineData(3, "Third")]
	[InlineData(4, "Fourth")]
	public async Task FourParamsThrowsOnCorrectSequence(int shortSequence, string cardinal)
	{
		await using var seq1 = Enumerable.Range(1, shortSequence == 1 ? 2 : 3).AsTestingSequence();
		await using var seq2 = Enumerable.Range(1, shortSequence == 2 ? 2 : 3).AsTestingSequence();
		await using var seq3 = Enumerable.Range(1, shortSequence == 3 ? 2 : 3).AsTestingSequence();
		await using var seq4 = Enumerable.Range(1, shortSequence == 4 ? 2 : 3).AsTestingSequence();

		var ex = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
			await seq1.EquiZip(seq2, seq3, seq4).Consume());
		Assert.Equal($"{cardinal} sequence too short.", ex.Message);
	}

	[Fact]
	public async Task ZipDisposesInnerSequencesCaseGetEnumeratorThrows()
	{
		await using var s1 = TestingSequence.Of(1, 2);

		_ = await Assert.ThrowsAsync<TestException>(async () =>
			await s1.EquiZip(new AsyncBreakingSequence<int>(), Tuple.Create).Consume());
	}
}
