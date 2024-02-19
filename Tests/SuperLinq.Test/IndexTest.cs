namespace Test;

[Obsolete("References `Index` which is obsolete in net9+")]
public class IndexTest
{
	[Fact]
	public void IndexIsLazy()
	{
		_ = SuperEnumerable.Index(new BreakingSequence<object>());
		_ = new BreakingSequence<object>().Index(0);
	}

	private const string One = "one";
	private const string Two = "two";
	private const string Three = "three";

	public static IEnumerable<object[]> GetSequences() =>
		Seq(One, Two, Three)
			.GetAllSequences()
			.Select(x => new object[] { x });

	[Theory]
	[MemberData(nameof(GetSequences))]
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

	[Theory]
	[MemberData(nameof(GetSequences))]
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

	[Fact]
	public void IndexCollectionBehavior()
	{
		using var seq = Enumerable.Range(0, 10_000).AsBreakingCollection();

		var result = seq.Index(10_000);
		result.AssertCollectionErrorChecking(10_000);
	}

	[Fact]
	public void IndexListBehavior()
	{
		using var seq = Enumerable.Range(0, 10_000).AsBreakingList();

		var result = seq.Index(10_000);
		result.AssertCollectionErrorChecking(10_000);
		result.AssertListElementChecking(10_000);

		Assert.Equal((10_200, 200), result.ElementAt(200));
		Assert.Equal((11_200, 1_200), result.ElementAt(1_200));
		Assert.Equal((18_800, 8_800), result.ElementAt(^1_200));
	}
}
