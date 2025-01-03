namespace SuperLinq.Tests;

[Obsolete("References `Index` which is obsolete in net9+")]
public sealed class IndexTest
{
	[Test]
	public void IndexIsLazy()
	{
		_ = SuperEnumerable.Index(new BreakingSequence<object>());
		_ = new BreakingSequence<object>().Index(0);
	}

	private const string One = "one";
	private const string Two = "two";
	private const string Three = "three";

	public static IEnumerable<IDisposableEnumerable<string>> GetSequences() =>
		Seq(One, Two, Three)
			.GetAllSequences();

	[Test]
	[MethodDataSource(nameof(GetSequences))]
	public void IndexSequence(IDisposableEnumerable<string> seq)
	{
		using (seq)
		{
			var result = SuperEnumerable.Index(seq);
			result.AssertSequenceEqual(
				(0, One),
				(1, Two),
				(2, Three));
		}
	}

	[Test]
	[MethodDataSource(nameof(GetSequences))]
	public void IndexSequenceStartIndex(IDisposableEnumerable<string> seq)
	{
		using (seq)
		{
			var result = seq.Index(10);
			result.AssertSequenceEqual(
				(10, One),
				(11, Two),
				(12, Three));
		}
	}

	[Test]
	public void IndexCollectionBehavior()
	{
		using var seq = Enumerable.Range(0, 10_000).AsBreakingCollection();

		var result = seq.Index(10_000);
		result.AssertCollectionErrorChecking(10_000);
	}

	[Test]
	public void IndexListBehavior()
	{
		using var seq = Enumerable.Range(0, 10_000).AsBreakingList();

		var result = seq.Index(10_000);
		result.AssertCollectionErrorChecking(10_000);
		result.AssertListElementChecking(10_000);

		Assert.Equal((10_200, 200), result.ElementAt(200));
		Assert.Equal((11_200, 1_200), result.ElementAt(1_200));
#if !NO_INDEX
		Assert.Equal((18_800, 8_800), result.ElementAt(^1_200));
#endif
	}
}
