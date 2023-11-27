namespace Test;

public partial class WindowLeftTest
{
	[Fact]
	public void WindowLeftIsLazy()
	{
		_ = new BreakingSequence<int>().WindowLeft(1);
		_ = new BreakingSequence<int>().WindowLeft(1, BreakingFunc.Of<ArraySegment<int>, int>());
		_ = new BreakingSequence<int>().WindowLeft(new int[3], BreakingFunc.Of<ArraySegment<int>, int>());
		_ = new BreakingSequence<int>().WindowLeft(new int[3], 1, BreakingFunc.Of<ArraySegment<int>, int>());
	}

	[Fact]
	public void WindowLeftNegativeWindowSizeException()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new BreakingSequence<int>().WindowLeft(-5));

		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new BreakingSequence<int>().WindowLeft(-5, SuperEnumerable.Identity));

		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new BreakingSequence<int>().WindowLeft([], -5, SuperEnumerable.Identity));

		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new BreakingSequence<int>().WindowLeft(new int[5], 6, SuperEnumerable.Identity));

		_ = new BreakingSequence<int>()
			.WindowLeft(new int[5], 5, SuperEnumerable.Identity);
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

	public enum WindowMethod
	{
		Traditional,
		BufferSize,
		BufferArray,
		BufferSizeArray,
	}

	private static IEnumerable<object[]> GetWindowTestSequences(IEnumerable<int> source)
	{
		foreach (var seq in source.GetListSequences())
			yield return new object[] { seq, WindowMethod.Traditional, };
		yield return new object[] { source.AsTestingSequence(maxEnumerations: 2), WindowMethod.BufferSize, };
		yield return new object[] { source.AsTestingSequence(maxEnumerations: 2), WindowMethod.BufferArray, };
		yield return new object[] { source.AsTestingSequence(maxEnumerations: 2), WindowMethod.BufferSizeArray, };
	}

	private static IEnumerable<IList<T>> GetWindows<T>(
			IEnumerable<T> seq,
			WindowMethod method,
			int size) =>
		method switch
		{
			WindowMethod.Traditional => seq.WindowLeft(size),
			WindowMethod.BufferSize => seq.WindowLeft(size, arr => arr.ToList()),
			WindowMethod.BufferArray => seq.WindowLeft(new T[size], arr => arr.ToList()),
			WindowMethod.BufferSizeArray => seq.WindowLeft(new T[size + 10], size, arr => arr.ToList()),
			_ => throw new NotSupportedException(),
		};

	public static IEnumerable<object[]> GetEmptySequences() =>
		GetWindowTestSequences(Enumerable.Empty<int>());

	[Theory]
	[MemberData(nameof(GetEmptySequences))]
	public void WindowLeftEmptySequence(IDisposableEnumerable<int> seq, WindowMethod wm)
	{
		using (seq)
		{
			var result = GetWindows(seq, wm, 5);
			result.AssertSequenceEqual();
		}
	}

	public static IEnumerable<object[]> GetHundredElementSequences() =>
		GetWindowTestSequences(Enumerable.Range(0, 100));

	[Theory]
	[MemberData(nameof(GetHundredElementSequences))]
	public void WindowLeftOfSingleElement(IDisposableEnumerable<int> seq, WindowMethod wm)
	{
		using (seq)
		{
			var result = GetWindows(seq, wm, 1);
			foreach (var (actual, expected) in result.Zip(Enumerable.Range(0, 100)))
				Assert.Equal(SuperEnumerable.Return(expected), actual);
		}
	}

	[Theory]
	[MemberData(nameof(GetHundredElementSequences))]
	public void WindowLeftWithWindowSizeSmallerThanSequence(IDisposableEnumerable<int> seq, WindowMethod wm)
	{
		using (seq)
		{
			var result = GetWindows(seq, wm, 10);
			result.AssertSequenceEqual(
				Enumerable.Range(0, 100)
					.Select(i => Enumerable.Range(i, Math.Min(10, 100 - i))));
		}
	}

	[Theory]
	[MemberData(nameof(GetHundredElementSequences))]
	public void WindowLeftWithWindowSizeLargerThanSequence(IDisposableEnumerable<int> seq, WindowMethod wm)
	{
		using (seq)
		{
			var result = GetWindows(seq, wm, 110);
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
