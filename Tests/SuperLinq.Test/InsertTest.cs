#if !NO_INDEX

using System.Diagnostics.CodeAnalysis;

namespace Test;

public sealed class InsertTest
{
	[Fact]
	public void InsertIsLazy()
	{
		_ = new BreakingSequence<int>().Insert(new BreakingSequence<int>(), 0);
	}

	[Fact]
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
	public static IEnumerable<object[]> GetSequences()
	{
		var baseSeq = Enumerable.Range(0, 10);
		var insSeq = Seq(97, 98, 99);

		return
		[
			[baseSeq.AsTestingSequence(), insSeq.AsTestingSequence(),],
			[baseSeq.AsTestingCollection(), insSeq.AsTestingCollection(),],
			[baseSeq.AsBreakingList(), insSeq.AsBreakingList(),],
		];
	}

	[Theory]
	[MemberData(nameof(GetSequences))]
	public void InsertWithIndexGreaterThanSourceLength(
		IDisposableEnumerable<int> seq1,
		IDisposableEnumerable<int> seq2)
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

	[Theory]
	[MemberData(nameof(GetSequences))]
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

	public static IEnumerable<object[]> GetInsertData() =>
		Seq(0, 5, 10, ^0, ^5, ^10)
			.SelectMany(
				_ => GetSequences(),
				(i, x) => new object[] { x[0], x[1], i });

	[Theory]
	[MemberData(nameof(GetInsertData))]
	public void Insert(
		IDisposableEnumerable<int> seq1,
		IDisposableEnumerable<int> seq2,
		Index index)
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
				testCollectionEnumerable: true);
		}
	}

	[Fact]
	public void InsertCollectionBehavior()
	{
		using var seq1 = Enumerable.Range(0, 10_000).AsBreakingCollection();
		using var seq2 = Enumerable.Range(0, 10_000).AsBreakingCollection();

		var result = seq1.Insert(seq2, 5_000);
		result.AssertCollectionErrorChecking(20_000);
	}

	[Fact]
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

	[Fact]
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

	[Fact]
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
