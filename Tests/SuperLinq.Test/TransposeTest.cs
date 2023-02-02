namespace Test;

public class TransposeTest
{
	[Fact]
	public void TransposeIsLazy()
	{
		new BreakingSequence<BreakingSequence<int>>().Transpose();
	}

	[Fact]
	public void TransposeWithOneNullRow()
	{
		using var seq1 = TestingSequence.Of(10, 11);
		using var seq2 = TestingSequence.Of<int>();
		using var seq3 = TestingSequence.Of(30, 31, 32);
		using var matrix = TestingSequence.Of(seq1, seq2, seq3, null);

		Assert.Throws<NullReferenceException>(() =>
			matrix!.Transpose().FirstOrDefault());
	}

	[Fact]
	public void TransposeWithRowsOfSameLength()
	{
		var expectations = new[]
		{
				new [] { 10, 20, 30 },
				new [] { 11, 21, 31 },
				new [] { 12, 22, 32 },
				new [] { 13, 23, 33 },
			};

		using var row1 = TestingSequence.Of(10, 11, 12, 13);
		using var row2 = TestingSequence.Of(20, 21, 22, 23);
		using var row3 = TestingSequence.Of(30, 31, 32, 33);
		using var matrix = TestingSequence.Of(row1, row2, row3);

		AssertMatrix(expectations, matrix.Transpose());
	}

	[Fact]
	public void TransposeWithRowsOfDifferentLengths()
	{
		var expectations = new[]
		{
			new[] { 10, 20, 30 },
			new[] { 11, 31 },
			new[] { 32 },
		};

		using var row1 = TestingSequence.Of(10, 11);
		using var row2 = TestingSequence.Of(20);
		using var row3 = TestingSequence.Of<int>();
		using var row4 = TestingSequence.Of(30, 31, 32);
		using var matrix = TestingSequence.Of(row1, row2, row3, row4);

		AssertMatrix(expectations, matrix.Transpose());
	}

	[Fact]
	public void TransposeMaintainsCornerElements()
	{
		var sequences = new List<TestingSequence<int>>()
		{
			TestingSequence.Of<int>(10, 11),
			TestingSequence.Of<int>(20),
			TestingSequence.Of<int>(),
			TestingSequence.Of<int>(30, 31, 32),
		};
		using var matrix = sequences.AsTestingSequence();

		var transpose = matrix.Transpose().ToList();

		Assert.Equal(10, transpose.First().First());
		Assert.Equal(32, transpose.Last().Last());
		sequences.VerifySequences();
	}

	[Fact]
	public void TransposeWithAllRowsAsInfiniteSequences()
	{
		var sequences = new List<TestingSequence<int>>();
		using var matrix = SuperEnumerable.Generate(1, x => x + 1)
			.Where(IsPrime)
			.Take(3)
			.Select(x => SuperEnumerable.Generate(x, n => n * x)
				.AddTestingSequenceToList(sequences))
			.AsTestingSequence();

		var result = matrix.Transpose().Take(5);

		var expectations = new[]
		{
				new[] { 2,    3,    5 },
				new[] { 4,    9,   25 },
				new[] { 8,   27,  125 },
				new[] { 16,  81,  625 },
				new[] { 32, 243, 3125 }
			};

		AssertMatrix(expectations, result);
		sequences.VerifySequences();
	}

	[Fact]
	public void TransposeWithSomeRowsAsInfiniteSequences()
	{
		var sequences = new List<TestingSequence<int>>();
		using var matrix = SuperEnumerable.Generate(1, x => x + 1)
			.Where(IsPrime)
			.Take(3)
			.Select((x, i) => i == 1
				? SuperEnumerable.Generate(x, n => n * x).Take(2).AddTestingSequenceToList(sequences)
				: SuperEnumerable.Generate(x, n => n * x).AddTestingSequenceToList(sequences))
			.AsTestingSequence();

		var result = matrix.Transpose().Take(5);

		var expectations = new[]
		{
				new[] { 2,    3,    5 },
				new[] { 4,    9,   25 },
				new[] { 8,        125 },
				new[] { 16,       625 },
				new[] { 32,      3125 }
			};

		AssertMatrix(expectations, result);
		sequences.VerifySequences();
	}

	[Fact]
	public void TransposeColumnTraversalOrderIsIrrelevant()
	{
		var sequences = new List<TestingSequence<int>>()
		{
			TestingSequence.Of<int>(10, 11),
			TestingSequence.Of<int>(20),
			TestingSequence.Of<int>(),
			TestingSequence.Of<int>(30, 31, 32),
		};
		using var matrix = sequences.AsTestingSequence();

		var transpose = matrix.Transpose().ToList();

		transpose[0].AssertSequenceEqual(10, 20, 30);
		transpose[1].AssertSequenceEqual(11, 31);
		transpose[2].AssertSequenceEqual(32);
		sequences.VerifySequences();
	}

	[Fact]
	public void TransposeConsumesRowsLazily()
	{
		var sequences = new List<TestingSequence<int>>()
		{
			Enumerable.Range(10, 3).AsTestingSequence(maxEnumerations: 2),
			Enumerable.Range(20, 3).AsTestingSequence(maxEnumerations: 2),
			SeqExceptionAt(2).Select(x => x + 29).AsTestingSequence(maxEnumerations: 2),
		};
		using var matrix = sequences.AsTestingSequence(maxEnumerations: 2);

		var result = matrix.Transpose();

		result.ElementAt(0).AssertSequenceEqual(10, 20, 30);

		Assert.Throws<TestException>(() =>
			result.ElementAt(1));
		sequences.VerifySequences();
	}

	[Fact]
	public void TransposeWithErroneousRowDisposesRowIterators()
	{
		using var row1 = TestingSequence.Of(10, 11);
		using var row2 = SeqExceptionAt(2).Select(x => x + 20).AsTestingSequence();
		using var row3 = TestingSequence.Of(30, 32);
		using var matrix = TestingSequence.Of(row1, row2, row3);

		Assert.Throws<TestException>(() =>
			matrix.Transpose().Consume());
	}

	private static bool IsPrime(int number)
	{
		if (number == 1) return false;
		if (number == 2) return true;

		var boundary = (int)Math.Floor(Math.Sqrt(number));

		for (var i = 2; i <= boundary; ++i)
		{
			if (number % i == 0)
				return false;
		}

		return true;
	}

	private static void AssertMatrix<T>(IEnumerable<IEnumerable<T>> expectation, IEnumerable<IEnumerable<T>> result)
	{
		var resultList = result.ToList();
		var expectationList = expectation.ToList();

		Assert.Equal(expectationList.Count, resultList.Count);

		expectationList
			.Zip(resultList)
			.ForEach(t => t.First.AssertSequenceEqual(t.Second));
	}
}
