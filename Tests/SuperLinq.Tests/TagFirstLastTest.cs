namespace SuperLinq.Tests;

public sealed class TagFirstLastTest
{
	[Test]
	public void TagFirstLastIsLazy()
	{
		_ = new BreakingSequence<object>().TagFirstLast();
		_ = new BreakingSequence<object>().TagFirstLast(BreakingFunc.Of<object, bool, bool, object>());
	}

	public static IEnumerable<(IDisposableEnumerable<int> seq, int n)> GetSourceSequences() =>
		Enumerable.Range(0, 4)
			.SelectMany(n =>
				Enumerable.Range(0, n)
					.GetListSequences()
					.Select(x => (x, n)));

	[Test]
	[MethodDataSource(nameof(GetSourceSequences))]
	public void TagFirstLastVaryingLengths(IDisposableEnumerable<int> seq, int n)
	{
		using (seq)
		{
			var result = seq.TagFirstLast();
			result.AssertSequenceEqual(
				Enumerable.Range(0, n)
					.Select(x => (x, x == 0, x == n - 1)));
		}
	}

	[Test]
	public void TagFirstLastListBehavior()
	{
		using var seq = Enumerable.Range(0, 10_000).AsBreakingList();

		var result = seq.TagFirstLast();
		result.AssertCollectionErrorChecking(10_000);
		result.AssertListElementChecking(10_000);

		Assert.Equal((0, true, false), result.ElementAt(0));
		Assert.Equal((30, false, false), result.ElementAt(30));
#if !NO_INDEX
		Assert.Equal((9_999, false, true), result.ElementAt(^1));
#endif
	}
}
