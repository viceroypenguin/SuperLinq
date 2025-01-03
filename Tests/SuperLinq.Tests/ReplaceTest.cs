#if !NO_INDEX

namespace SuperLinq.Tests;

public sealed class ReplaceTest
{
	[Test]
	public void ReplaceIsLazy()
	{
		_ = new BreakingSequence<int>().Replace(0, 10);
		_ = new BreakingSequence<int>().Replace(new Index(10), 10);
		_ = new BreakingSequence<int>().Replace(^0, 10);
	}

	[Test]
	public void ReplaceEmptySequence()
	{
		using var seq = Enumerable.Empty<int>().AsTestingSequence(maxEnumerations: 6);
		seq.Replace(0, 10).AssertSequenceEqual();
		seq.Replace(10, 10).AssertSequenceEqual();
		seq.Replace(new Index(0), 10).AssertSequenceEqual();
		seq.Replace(new Index(10), 10).AssertSequenceEqual();
		seq.Replace(^0, 10).AssertSequenceEqual();
		seq.Replace(^10, 10).AssertSequenceEqual();
	}

	public static IEnumerable<(int index, IDisposableEnumerable<int> seq)> Indices() =>
		Enumerable.Range(0, 10)
			.SelectMany(
				_ => Enumerable.Range(1, 10).GetAllSequences(),
				(i, s) => (i, s));

	[Test]
	[MethodDataSource(nameof(Indices))]
	public void ReplaceIntIndex(int index, IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.Replace(index, 30);
			result.AssertSequenceEqual(
				Enumerable.Range(1, index)
					.Concat([30])
					.Concat(Enumerable.Range(index + 2, 9 - index)),
				testCollectionEnumerable: true);
		}
	}

	[Test]
	[MethodDataSource(nameof(Indices))]
	public void ReplaceStartIndex(int index, IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.Replace(new Index(index), 30);
			result.AssertSequenceEqual(
				Enumerable.Range(1, index)
					.Concat([30])
					.Concat(Enumerable.Range(index + 2, 9 - index)),
				testCollectionEnumerable: true);
		}
	}

	[Test]
	[MethodDataSource(nameof(Indices))]
	public void ReplaceEndIndex(int index, IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			// offset by one because 0..9 is less useful than 1..10
			var result = seq.Replace(^(index + 1), 30);
			result.AssertSequenceEqual(
				Enumerable.Range(1, 9 - index)
					.Concat([30])
					.Concat(Enumerable.Range(11 - index, index)),
				testCollectionEnumerable: true);
		}
	}

	public static IEnumerable<IDisposableEnumerable<int>> GetSequences() =>
		Enumerable.Range(1, 10)
			.GetListSequences();

	[Test]
	[MethodDataSource(nameof(GetSequences))]
	public void ReplaceIntIndexPastSequenceLength(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.Replace(10, 30);
			result.AssertSequenceEqual(
				Enumerable.Range(1, 10),
				testCollectionEnumerable: true);
		}
	}

	[Test]
	[MethodDataSource(nameof(GetSequences))]
	public void ReplaceStartIndexPastSequenceLength(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.Replace(new Index(10), 30);
			result.AssertSequenceEqual(
				Enumerable.Range(1, 10),
				testCollectionEnumerable: true);
		}
	}

	[Test]
	[MethodDataSource(nameof(GetSequences))]
	public void ReplaceEndIndexPastSequenceLength(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.Replace(^11, 30);
			result.AssertSequenceEqual(
				Enumerable.Range(1, 10),
				testCollectionEnumerable: true);
		}
	}

	[Test]
	public void ReplaceListBehavior()
	{
		using var seq = Enumerable.Range(0, 10_000).AsBreakingList();

		var result = seq.Replace(20, -1);
		result.AssertCollectionErrorChecking(10_000);
		result.AssertListElementChecking(10_000);

		Assert.Equal(10, result.ElementAt(10));
		Assert.Equal(-1, result.ElementAt(20));
		Assert.Equal(9_950, result.ElementAt(^50));

		result.ToList()
			.AssertSequenceEqual(
				Enumerable.Range(0, 20)
					.Concat([-1])
					.Concat(Enumerable.Range(21, 9_979)));
	}
}

#endif
