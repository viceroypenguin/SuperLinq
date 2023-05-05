namespace Test;

public class FillBackwardTest
{
	[Fact]
	public void FillBackwardIsLazy()
	{
		_ = new BreakingSequence<object>().FillBackward();
		_ = new BreakingSequence<object>().FillBackward(BreakingFunc.Of<object, bool>());
		_ = new BreakingSequence<object>().FillBackward(BreakingFunc.Of<object, bool>(), BreakingFunc.Of<object, object, object>());
	}

	public static IEnumerable<object[]> GetIntNullSequences() =>
		Seq<int?>(null, null, 1, 2, null, null, null, 3, 4, null, null)
			.ArrangeCollectionInlineDatas()
			.Select(x => new object[] { x });

	[Theory]
	[MemberData(nameof(GetIntNullSequences))]
	public void FillBackwardBlank(IDisposableEnumerable<int?> seq)
	{
		using (seq)
		{
			seq
				.FillBackward(x => x != 200)
				.AssertSequenceEqual(default(int?), null, 1, 2, null, null, null, 3, 4, null, null);
		}
	}

	[Theory]
	[MemberData(nameof(GetIntNullSequences))]
	public void FillBackward(IDisposableEnumerable<int?> seq)
	{
		using (seq)
		{
			seq
				.FillBackward()
				.AssertSequenceEqual(1, 1, 1, 2, 3, 3, 3, 3, 4, null, null);
		}
	}

	public static IEnumerable<object[]> GetIntSequences() =>
		Enumerable.Range(1, 13)
			.ArrangeCollectionInlineDatas()
			.Select(x => new object[] { x });

	[Theory]
	[MemberData(nameof(GetIntSequences))]
	public void FillBackwardWithFillSelector(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			seq
				.FillBackward(e => e % 5 != 0, (x, y) => x * y)
				.AssertSequenceEqual(5, 10, 15, 20, 5, 60, 70, 80, 90, 10, 11, 12, 13);
		}
	}
}
