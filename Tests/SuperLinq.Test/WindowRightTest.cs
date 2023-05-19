namespace Test;

public partial class WindowRightTest
{
	[Fact]
	public void WindowRightIsLazy()
	{
		_ = new BreakingSequence<int>().WindowRight(1);
	}

	[Fact]
	public void WindowRightNegativeWindowSizeException()
	{
		var sequence = Enumerable.Repeat(1, 10);

		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			sequence.WindowRight(-5));
	}

	public static IEnumerable<object[]> GetThreeElementSequences() =>
		Enumerable.Range(0, 3)
			.GetListSequences()
			.Select(x => new object[] { x });

	[Theory]
	[MemberData(nameof(GetThreeElementSequences))]
	public void WindowModifiedBeforeMoveNextDoesNotAffectNextWindow(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			using var e = seq.WindowRight(2).GetEnumerator();

			_ = e.MoveNext();
			var window1 = e.Current;
			window1[0] = -1;
			_ = e.MoveNext();
			var window2 = e.Current;

			Assert.Equal(0, window2[0]);
		}
	}

	[Theory]
	[MemberData(nameof(GetThreeElementSequences))]
	public void WindowModifiedAfterMoveNextDoesNotAffectNextWindow(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			using var e = seq.WindowRight(2).GetEnumerator();

			_ = e.MoveNext();
			var window1 = e.Current;
			_ = e.MoveNext();
			window1[0] = -1;
			var window2 = e.Current;

			Assert.Equal(0, window2[0]);
		}
	}

	[Theory]
	[MemberData(nameof(GetThreeElementSequences))]
	public void WindowModifiedDoesNotAffectPreviousWindow(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			using var e = seq.WindowRight(2).GetEnumerator();

			_ = e.MoveNext();
			var window1 = e.Current;
			_ = e.MoveNext();
			var window2 = e.Current;
			window2[0] = -1;

			Assert.Equal(0, window1[0]);
		}
	}

	public static IEnumerable<object[]> GetEmptySequences() =>
		Enumerable.Empty<int>()
			.GetListSequences()
			.Select(x => new object[] { x });

	[Theory]
	[MemberData(nameof(GetEmptySequences))]
	public void WindowRightEmptySequence(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.WindowRight(5);
			result.AssertSequenceEqual();
		}
	}

	public static IEnumerable<object[]> GetHundredElementSequences() =>
		Enumerable.Range(0, 100)
			.GetListSequences()
			.Select(x => new object[] { x });

	[Theory]
	[MemberData(nameof(GetHundredElementSequences))]
	public void WindowRightOfSingleElement(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.WindowRight(1);

			foreach (var (actual, expected) in result.Zip(Enumerable.Range(0, 100)))
				Assert.Equal(SuperEnumerable.Return(expected), actual);
		}
	}

	[Theory]
	[MemberData(nameof(GetHundredElementSequences))]
	public void WindowRightWithWindowSizeLargerThanSequence(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.WindowRight(10);
			result.AssertSequenceEqual(
				Enumerable.Range(0, 10)
					.Select(x => Enumerable.Range(0, x + 1))
					.Concat(Enumerable.Range(1, 90)
						.Select(x => Enumerable.Range(x, 10))));
		}
	}

	[Theory]
	[MemberData(nameof(GetHundredElementSequences))]
	public void WindowRightWithWindowSizeSmallerThanSequence(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.WindowRight(110);
			result.AssertSequenceEqual(
				Enumerable.Range(0, 100)
					.Select(i => Enumerable.Range(0, i + 1)));
		}
	}

	[Fact]
	public void WindowRightListBehavior()
	{
		using var seq = Enumerable.Range(0, 10_000).AsBreakingList();

		var result = seq.WindowRight(20);
		Assert.Equal(10_000, result.Count());
		Assert.Equal(Enumerable.Range(0, 10), result.ElementAt(10));
		Assert.Equal(Enumerable.Range(9_980, 20), result.ElementAt(^1));
	}
}
