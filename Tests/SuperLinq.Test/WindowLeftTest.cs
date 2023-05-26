namespace Test;

public partial class WindowLeftTest
{
	[Fact]
	public void WindowLeftIsLazy()
	{
		_ = new BreakingSequence<int>().WindowLeft(1);
	}

	[Fact]
	public void WindowLeftNegativeWindowSizeException()
	{
		var sequence = Enumerable.Repeat(1, 10);

		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			sequence.WindowLeft(-5));
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
			using var e = seq.WindowLeft(2).GetEnumerator();

			_ = e.MoveNext();
			var window1 = e.Current;
			window1[1] = -1;
			_ = e.MoveNext();
			var window2 = e.Current;

			Assert.Equal(1, window2[0]);
		}
	}

	[Theory]
	[MemberData(nameof(GetThreeElementSequences))]
	public void WindowModifiedAfterMoveNextDoesNotAffectNextWindow(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			using var e = seq.WindowLeft(2).GetEnumerator();

			_ = e.MoveNext();
			var window1 = e.Current;
			_ = e.MoveNext();
			window1[1] = -1;
			var window2 = e.Current;

			Assert.Equal(1, window2[0]);
		}
	}

	[Theory]
	[MemberData(nameof(GetThreeElementSequences))]
	public void WindowModifiedDoesNotAffectPreviousWindow(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			using var e = seq.WindowLeft(2).GetEnumerator();

			_ = e.MoveNext();
			var window1 = e.Current;
			_ = e.MoveNext();
			var window2 = e.Current;
			window2[0] = -1;

			Assert.Equal(1, window1[1]);
		}
	}

	public static IEnumerable<object[]> GetEmptySequences() =>
		Enumerable.Empty<int>()
			.GetListSequences()
			.Select(x => new object[] { x });

	[Theory]
	[MemberData(nameof(GetEmptySequences))]
	public void WindowLeftEmptySequence(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.WindowLeft(5);
			result.AssertSequenceEqual();
		}
	}

	public static IEnumerable<object[]> GetHundredElementSequences() =>
		Enumerable.Range(0, 100)
			.GetListSequences()
			.Select(x => new object[] { x });

	[Theory]
	[MemberData(nameof(GetHundredElementSequences))]
	public void WindowLeftOfSingleElement(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.WindowLeft(1);

			foreach (var (actual, expected) in result.Zip(Enumerable.Range(0, 100)))
				Assert.Equal(SuperEnumerable.Return(expected), actual);
		}
	}

	[Theory]
	[MemberData(nameof(GetHundredElementSequences))]
	public void WindowLeftWithWindowSizeLargerThanSequence(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.WindowLeft(10);
			result.AssertSequenceEqual(
				Enumerable.Range(0, 100)
					.Select(i => Enumerable.Range(i, Math.Min(10, 100 - i))));
		}
	}

	[Theory]
	[MemberData(nameof(GetHundredElementSequences))]
	public void WindowLeftWithWindowSizeSmallerThanSequence(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.WindowLeft(110);
			result.AssertSequenceEqual(
				Enumerable.Range(0, 100)
					.Select(i => Enumerable.Range(i, 100 - i)));
		}
	}

	[Fact]
	public void WindowLeftListBehavior()
	{
		using var seq = Enumerable.Range(0, 10_000).AsBreakingList();

		var result = seq.WindowLeft(20);
		result.AssertCollectionErrorChecking(10_000);
		result.AssertListElementChecking(10_000);

		Assert.Equal(Enumerable.Range(50, 20), result.ElementAt(50));
		Assert.Equal(Enumerable.Range(9_999, 1), result.ElementAt(^1));
	}
}
