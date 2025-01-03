namespace SuperLinq.Tests;

public sealed class WindowLeftTest
{
	[Test]
	public void WindowLeftIsLazy()
	{
		_ = new BreakingSequence<int>().WindowLeft(1);
		_ = new BreakingSequence<int>().WindowLeft(1, BreakingReadOnlySpanFunc.Of<int, int>());
		_ = new BreakingSequence<int>().WindowLeft(new int[3], BreakingReadOnlySpanFunc.Of<int, int>());
		_ = new BreakingSequence<int>().WindowLeft(new int[3], 1, BreakingReadOnlySpanFunc.Of<int, int>());
	}

	[Test]
	public void WindowLeftNegativeWindowSizeException()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new BreakingSequence<int>().WindowLeft(-5));

		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new BreakingSequence<int>().WindowLeft(-5, BreakingReadOnlySpanFunc.Of<int, int>()));

		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new BreakingSequence<int>().WindowLeft([], -5, BreakingReadOnlySpanFunc.Of<int, int>()));

		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new BreakingSequence<int>().WindowLeft(new int[5], 6, BreakingReadOnlySpanFunc.Of<int, int>()));

		_ = new BreakingSequence<int>()
			.WindowLeft(new int[5], 5, BreakingReadOnlySpanFunc.Of<int, int>());
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
			using var e = seq.WindowLeft(2).GetEnumerator();

			_ = e.MoveNext();
			var window1 = e.Current;
			window1[1] = -1;
			_ = e.MoveNext();
			var window2 = e.Current;

			Assert.Equal(1, window2[0]);
		}
	}

	[Test]
	[MethodDataSource(nameof(GetThreeElementSequences))]
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

	[Test]
	[MethodDataSource(nameof(GetThreeElementSequences))]
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

	private static IEnumerable<(IDisposableEnumerable<int> seq, WindowMethod windowMethod)> GetWindowTestSequences(IEnumerable<int> source)
	{
		foreach (var seq in source.GetListSequences())
			yield return (seq, WindowMethod.Traditional);

		yield return (source.AsTestingSequence(), WindowMethod.BufferSize);
		yield return (source.AsTestingSequence(), WindowMethod.BufferArray);
		yield return (source.AsTestingSequence(), WindowMethod.BufferSizeArray);
	}

	private static IEnumerable<IList<T>> GetWindows<T>(
			IEnumerable<T> seq,
			WindowMethod method,
			int size) =>
		method switch
		{
			WindowMethod.Traditional => seq.WindowLeft(size),
			WindowMethod.BufferSize => seq.WindowLeft(size, arr => arr.ToArray()),
			WindowMethod.BufferArray => seq.WindowLeft(new T[size], arr => arr.ToArray()),
			WindowMethod.BufferSizeArray => seq.WindowLeft(new T[size + 10], size, arr => arr.ToArray()),
			_ => throw new NotSupportedException(),
		};

	public static IEnumerable<(IDisposableEnumerable<int> seq, WindowMethod windowMethod)> GetEmptySequences() =>
		GetWindowTestSequences([]);

	[Test]
	[MethodDataSource(nameof(GetEmptySequences))]
	public void WindowLeftEmptySequence(IDisposableEnumerable<int> seq, WindowMethod windowMethod)
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
	public void WindowLeftOfSingleElement(IDisposableEnumerable<int> seq, WindowMethod windowMethod)
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
	public void WindowLeftWithWindowSizeSmallerThanSequence(IDisposableEnumerable<int> seq, WindowMethod windowMethod)
	{
		using (seq)
		{
			var result = GetWindows(seq, windowMethod, 10);
			result.AssertSequenceEqual(
				Enumerable.Range(0, 100)
					.Select(i => Enumerable.Range(i, Math.Min(10, 100 - i))));
		}
	}

	[Test]
	[MethodDataSource(nameof(GetHundredElementSequences))]
	public void WindowLeftWithWindowSizeLargerThanSequence(IDisposableEnumerable<int> seq, WindowMethod windowMethod)
	{
		using (seq)
		{
			var result = GetWindows(seq, windowMethod, 110);
			result.AssertSequenceEqual(
				Enumerable.Range(0, 100)
					.Select(i => Enumerable.Range(i, 100 - i)));
		}
	}

	[Test]
	public void WindowLeftListBehavior()
	{
		using var seq = Enumerable.Range(0, 10_000).AsBreakingList();

		var result = seq.WindowLeft(20);
		result.AssertCollectionErrorChecking(10_000);
		result.AssertListElementChecking(10_000);

		Assert.Equal(Enumerable.Range(50, 20), result.ElementAt(50));
#if !NO_INDEX
		Assert.Equal(Enumerable.Range(9_999, 1), result.ElementAt(^1));
#endif
	}
}
