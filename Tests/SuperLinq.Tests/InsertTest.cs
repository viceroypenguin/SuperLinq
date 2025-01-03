#if !NO_INDEX

using System.Diagnostics.CodeAnalysis;

namespace SuperLinq.Tests;

public sealed class InsertTest
{
	[Test]
	public void InsertIsLazy()
	{
		_ = new BreakingSequence<int>().Insert(new BreakingSequence<int>(), 0);
	}

	[Test]
	public void InsertWithNegativeIndex()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			 new BreakingSequence<int>().Insert([97, 98, 99], -1));
	}

	[SuppressMessage(
		"Reliability",
		"CA2000:Dispose objects before losing scope",
		Justification = "Will be properly disposed in method"
	)]
	public static IEnumerable<(
		IDisposableEnumerable<int> seq1,
		IDisposableEnumerable<int> seq2
	)> GetSequences()
	{
		var baseSeq = Enumerable.Range(0, 10);
		var insSeq = Seq(97, 98, 99);

		return
		[
			(baseSeq.AsTestingSequence(), insSeq.AsTestingSequence()),
			(baseSeq.AsTestingCollection(), insSeq.AsTestingCollection()),
			(baseSeq.AsBreakingList(), insSeq.AsBreakingList()),
		];
	}

	[Test]
	[MethodDataSource(nameof(GetSequences))]
	public void InsertWithIndexGreaterThanSourceLength(
		IDisposableEnumerable<int> seq1,
		IDisposableEnumerable<int> seq2
	)
	{
		using (seq1)
		using (seq2)
		{
			var result = seq1.Insert(seq2, 11);

			_ = Assert.Throws<ArgumentOutOfRangeException>(delegate
			{
				result.Consume();
			});
		}
	}

	[Test]
	[MethodDataSource(nameof(GetSequences))]
	public void InsertWithEndIndexGreaterThanSourceLength(
		IDisposableEnumerable<int> seq1,
		IDisposableEnumerable<int> seq2)
	{
		using (seq1)
		using (seq2)
		{
			var result = seq1.Insert(seq2, ^11);

			_ = Assert.Throws<ArgumentOutOfRangeException>(delegate
			{
				result.Consume();
			});
		}
	}

	public static IEnumerable<(
		IDisposableEnumerable<int> seq1,
		IDisposableEnumerable<int> seq2,
		Index index
	)> GetInsertData() =>
		Seq(0, 5, 10, ^0, ^5, ^10)
			.SelectMany(
				_ => GetSequences(),
				(i, x) => (x.seq1, x.seq2, i)
			);

	[Test]
	[MethodDataSource(nameof(GetInsertData))]
	public void Insert(
		IDisposableEnumerable<int> seq1,
		IDisposableEnumerable<int> seq2,
		Index index
	)
	{
		using (seq1)
		using (seq2)
		{
			var result = seq1.Insert(seq2, index);
			var idx = index.GetOffset(10);

			result.AssertSequenceEqual(
				Enumerable.Range(0, idx)
					.Concat(Seq(97, 98, 99))
					.Concat(Enumerable.Range(idx, 10 - idx)),
				testCollectionEnumerable: true
			);
		}
	}

	[Test]
	public void InsertCollectionBehavior()
	{
		using var seq1 = Enumerable.Range(0, 10_000).AsBreakingCollection();
		using var seq2 = Enumerable.Range(0, 10_000).AsBreakingCollection();

		var result = seq1.Insert(seq2, 5_000);
		result.AssertCollectionErrorChecking(20_000);
	}

	[Test]
	public void InsertListFromStartBehavior()
	{
		using var seq1 = Enumerable.Range(0, 10_000).AsBreakingList();
		using var seq2 = Enumerable.Range(0, 10_000).AsBreakingList();

		var result = seq1.Insert(seq2, 5_000);
		result.AssertCollectionErrorChecking(20_000);
		result.AssertListElementChecking(20_000);

		Assert.Equal(1_200, result.ElementAt(1_200));
		Assert.Equal(6_200, result.ElementAt(11_200));
		Assert.Equal(8_800, result.ElementAt(^1_200));
	}

	[Test]
	public void InsertListFromEndBehavior()
	{
		using var seq1 = Enumerable.Range(0, 10_000).AsBreakingList();
		using var seq2 = Enumerable.Range(0, 10_000).AsBreakingList();

		var result = seq1.Insert(seq2, ^5_000);
		result.AssertCollectionErrorChecking(20_000);
		result.AssertListElementChecking(20_000);

		Assert.Equal(1_200, result.ElementAt(1_200));
		Assert.Equal(6_200, result.ElementAt(11_200));
		Assert.Equal(8_800, result.ElementAt(^1_200));
	}

	[Test]
	public void InsertListAtEndBehavior()
	{
		using var seq1 = Enumerable.Range(0, 10_000).AsBreakingList();
		using var seq2 = Enumerable.Range(0, 10_000).AsBreakingList();

		var result = seq1.Insert(seq2, ^0);
		result.AssertCollectionErrorChecking(20_000);
		result.AssertListElementChecking(20_000);

		Assert.Equal(1_200, result.ElementAt(1_200));
		Assert.Equal(1_200, result.ElementAt(11_200));
		Assert.Equal(8_800, result.ElementAt(^1_200));
	}
}

#endif
