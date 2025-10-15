namespace SuperLinq.Tests;

/// <summary>
/// Verify the behavior of the Window operator
/// </summary>
public sealed class WindowTests
{
	[Fact]
	public void TestWindowIsLazy()
	{
		_ = new BreakingSequence<int>().Window(1);
		_ = new BreakingSequence<int>().Window(1, BreakingReadOnlySpanFunc.Of<int, int>());
		_ = new BreakingSequence<int>().Window(new int[3], BreakingReadOnlySpanFunc.Of<int, int>());
		_ = new BreakingSequence<int>().Window(new int[3], 1, BreakingReadOnlySpanFunc.Of<int, int>());
	}

	[Fact]
	public void TestWindowNegativeWindowSizeException()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new BreakingSequence<int>().Window(-5));

		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new BreakingSequence<int>().Window(-5, BreakingReadOnlySpanFunc.Of<int, int>()));

		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new BreakingSequence<int>().Window([], -5, BreakingReadOnlySpanFunc.Of<int, int>()));

		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new BreakingSequence<int>().Window(new int[5], 6, BreakingReadOnlySpanFunc.Of<int, int>()));

		_ = new BreakingSequence<int>()
			.Window(new int[5], 5, BreakingReadOnlySpanFunc.Of<int, int>());
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
			using var e = seq.Window(2).GetEnumerator();

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
			using var e = seq.Window(2).GetEnumerator();

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
			using var e = seq.Window(2).GetEnumerator();

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
			yield return new object[] { seq, WindowMethod.Traditional };

		yield return new object[] { source.AsTestingSequence(maxEnumerations: 2), WindowMethod.BufferSize };
		yield return new object[] { source.AsTestingSequence(maxEnumerations: 2), WindowMethod.BufferArray };
		yield return new object[] { source.AsTestingSequence(maxEnumerations: 2), WindowMethod.BufferSizeArray };
	}

	private static IEnumerable<IList<T>> GetWindows<T>(
			IEnumerable<T> seq,
			WindowMethod method,
			int size) =>
		method switch
		{
			WindowMethod.Traditional => seq.Window(size),
			WindowMethod.BufferSize => seq.Window(size, arr => arr.ToArray()),
			WindowMethod.BufferArray => seq.Window(new T[size], arr => arr.ToArray()),
			WindowMethod.BufferSizeArray => seq.Window(new T[size + 10], size, arr => arr.ToArray()),
			_ => throw new NotSupportedException(),
		};

	public static IEnumerable<object[]> GetEmptySequences() =>
		GetWindowTestSequences([]);

	[Theory]
	[MemberData(nameof(GetEmptySequences))]
	public void TestWindowEmptySequence(IDisposableEnumerable<int> seq, WindowMethod wm)
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
	public void TestWindowOfSingleElement(IDisposableEnumerable<int> seq, WindowMethod wm)
	{
		using (seq)
		{
			var result = GetWindows(seq, wm, 1);
			foreach (var (actual, expected) in result.Zip(Enumerable.Range(0, 100)))
				actual.AssertSequenceEqual(expected);
		}
	}

	[Theory]
	[MemberData(nameof(GetHundredElementSequences))]
	public void TestWindowLargerThanSequence(IDisposableEnumerable<int> seq, WindowMethod wm)
	{
		using (seq)
		{
			var result = GetWindows(seq, wm, 101);
			result.AssertSequenceEqual();
		}
	}

	[Theory]
	[MemberData(nameof(GetHundredElementSequences))]
	public void TestWindowSmallerThanSequence(IDisposableEnumerable<int> seq, WindowMethod wm)
	{
		using (seq)
		{
			var result = GetWindows(seq, wm, 33);
			foreach (var (window, index) in result.Zip(Enumerable.Range(0, 100)))
				window.AssertSequenceEqual(Enumerable.Range(0, 100).Skip(index).Take(33));
		}
	}

	[Fact]
	public void WindowListBehavior()
	{
		using var seq = Enumerable.Range(0, 10_000).AsBreakingList();

		var result = seq.Window(20);
		var length = 10_000 - 20 + 1;
		result.AssertCollectionErrorChecking(length);
		result.AssertListElementChecking(length);

		Assert.Equal(Enumerable.Range(50, 20), result.ElementAt(50));
#if !NO_INDEX
		Assert.Equal(Enumerable.Range(9_980, 20), result.ElementAt(^1));
#endif
	}
}
