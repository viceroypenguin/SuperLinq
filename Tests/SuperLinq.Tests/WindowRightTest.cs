namespace SuperLinq.Tests;

public sealed class WindowRightTest
{
	[Test]
	public void WindowRightIsLazy()
	{
		_ = new BreakingSequence<int>().WindowRight(1);
		_ = new BreakingSequence<int>().WindowRight(1, BreakingReadOnlySpanFunc.Of<int, int>());
		_ = new BreakingSequence<int>().WindowRight(new int[3], BreakingReadOnlySpanFunc.Of<int, int>());
		_ = new BreakingSequence<int>().WindowRight(new int[3], 1, BreakingReadOnlySpanFunc.Of<int, int>());
	}

	[Test]
	public void WindowRightNegativeWindowSizeException()
	{
		var sequence = Enumerable.Repeat(1, 10);

		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new BreakingSequence<int>().WindowRight(-5));

		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new BreakingSequence<int>().WindowRight(-5, BreakingReadOnlySpanFunc.Of<int, int>()));

		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new BreakingSequence<int>().WindowRight([], -5, BreakingReadOnlySpanFunc.Of<int, int>()));

		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new BreakingSequence<int>().WindowRight(new int[5], 6, BreakingReadOnlySpanFunc.Of<int, int>()));

		_ = new BreakingSequence<int>()
			.WindowRight(new int[5], 5, BreakingReadOnlySpanFunc.Of<int, int>());
	}

	public static IEnumerable<IDisposableEnumerable<int>> GetThreeElementSequences() =>
		Enumerable.Range(0, 3)
			.GetListSequences();

	[Test]
	[MethodDataSource(nameof(GetThreeElementSequences))]
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

	[Test]
	[MethodDataSource(nameof(GetThreeElementSequences))]
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

	[Test]
	[MethodDataSource(nameof(GetThreeElementSequences))]
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

	public enum WindowMethod
	{
		Traditional,
		BufferSize,
		BufferArray,
		BufferSizeArray,
	}

	private static IEnumerable<(IDisposableEnumerable<int> seq, WindowMethod windowMethod)> GetWindowTestSequences(IEnumerable<int> source)
	{
		foreach (var seq in source.GetListSequences())
			yield return (seq, WindowMethod.Traditional);

		yield return (source.AsTestingSequence(maxEnumerations: 2), WindowMethod.BufferSize);
		yield return (source.AsTestingSequence(maxEnumerations: 2), WindowMethod.BufferArray);
		yield return (source.AsTestingSequence(maxEnumerations: 2), WindowMethod.BufferSizeArray);
	}

	private static IEnumerable<IList<T>> GetWindows<T>(
			IEnumerable<T> seq,
			WindowMethod method,
			int size) =>
		method switch
		{
			WindowMethod.Traditional => seq.WindowRight(size),
			WindowMethod.BufferSize => seq.WindowRight(size, arr => arr.ToArray()),
			WindowMethod.BufferArray => seq.WindowRight(new T[size], arr => arr.ToArray()),
			WindowMethod.BufferSizeArray => seq.WindowRight(new T[size + 10], size, arr => arr.ToArray()),
			_ => throw new NotSupportedException(),
		};

	public static IEnumerable<(IDisposableEnumerable<int> seq, WindowMethod windowMethod)> GetEmptySequences() =>
		GetWindowTestSequences([]);

	[Test]
	[MethodDataSource(nameof(GetEmptySequences))]
	public void WindowRightEmptySequence(IDisposableEnumerable<int> seq, WindowMethod windowMethod)
	{
		using (seq)
		{
			var result = GetWindows(seq, windowMethod, 5);
			result.AssertSequenceEqual();
		}
	}

	public static IEnumerable<(IDisposableEnumerable<int> seq, WindowMethod windowMethod)> GetHundredElementSequences() =>
		GetWindowTestSequences(Enumerable.Range(0, 100));

	[Test]
	[MethodDataSource(nameof(GetHundredElementSequences))]
	public void WindowRightOfSingleElement(IDisposableEnumerable<int> seq, WindowMethod windowMethod)
	{
		using (seq)
		{
			var result = GetWindows(seq, windowMethod, 1);
			foreach (var (actual, expected) in result.Zip(Enumerable.Range(0, 100)))
				Assert.Equal(SuperEnumerable.Return(expected), actual);
		}
	}

	[Test]
	[MethodDataSource(nameof(GetHundredElementSequences))]
	public void WindowRightWithWindowSizeSmallerThanSequence(IDisposableEnumerable<int> seq, WindowMethod windowMethod)
	{
		using (seq)
		{
			var result = GetWindows(seq, windowMethod, 10);
			result.AssertSequenceEqual(
				Enumerable.Range(0, 10)
					.Select(x => Enumerable.Range(0, x + 1))
					.Concat(Enumerable.Range(1, 90)
						.Select(x => Enumerable.Range(x, 10))));
		}
	}

	[Test]
	[MethodDataSource(nameof(GetHundredElementSequences))]
	public void WindowRightWithWindowSizeLargerThanSequence(IDisposableEnumerable<int> seq, WindowMethod windowMethod)
	{
		using (seq)
		{
			var result = GetWindows(seq, windowMethod, 110);
			result.AssertSequenceEqual(
				Enumerable.Range(0, 100)
					.Select(i => Enumerable.Range(0, i + 1)));
		}
	}

	[Test]
	public void WindowRightListBehavior()
	{
		using var seq = Enumerable.Range(0, 10_000).AsBreakingList();

		var result = seq.WindowRight(20);
		result.AssertCollectionErrorChecking(10_000);
		result.AssertListElementChecking(10_000);

		Assert.Equal(Enumerable.Range(0, 10), result.ElementAt(10));
#if !NO_INDEX
		Assert.Equal(Enumerable.Range(9_980, 20), result.ElementAt(^1));
#endif
	}
}
