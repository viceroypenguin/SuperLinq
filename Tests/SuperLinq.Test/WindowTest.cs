namespace Test;

/// <summary>
/// Verify the behavior of the Window operator
/// </summary>
public partial class WindowTests
{
	/// <summary>
	/// Verify that Window behaves in a lazy manner
	/// </summary>
	[Fact]
	public void TestWindowIsLazy()
	{
		_ = new BreakingSequence<int>().Window(1);
	}

	/// <summary>
	/// Verify that a negative window size results in an exception
	/// </summary>
	[Fact]
	public void TestWindowNegativeWindowSizeException()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new BreakingSequence<int>().Window(-5));
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

	public static IEnumerable<object[]> GetEmptySequences() =>
		Enumerable.Empty<int>()
			.GetListSequences()
			.Select(x => new object[] { x });

	[Theory]
	[MemberData(nameof(GetEmptySequences))]
	public void TestWindowEmptySequence(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.Window(5);
			result.AssertSequenceEqual();
		}
	}

	public static IEnumerable<object[]> GetHundredElementSequences() =>
		Enumerable.Range(0, 100)
			.GetListSequences()
			.Select(x => new object[] { x });

	[Theory]
	[MemberData(nameof(GetHundredElementSequences))]
	public void TestWindowOfSingleElement(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.Window(1);

			foreach (var (actual, expected) in result.Zip(Enumerable.Range(0, 100)))
				Assert.Equal(SuperEnumerable.Return(expected), actual);
		}
	}

	[Theory]
	[MemberData(nameof(GetHundredElementSequences))]
	public void TestWindowLargerThanSequence(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.Window(101);
			result.AssertSequenceEqual();
		}
	}

	[Theory]
	[MemberData(nameof(GetHundredElementSequences))]
	public void TestWindowSmallerThanSequence(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			var result = seq.Window(33);

			foreach (var (window, index) in result.Zip(Enumerable.Range(0, 100)))
				window.AssertSequenceEqual(Enumerable.Range(0, 100).Skip(index).Take(33));
		}
	}

	[Fact]
	public void WindowListBehavior()
	{
		using var seq = Enumerable.Range(0, 10_000).AsBreakingList();

		var result = seq.Window(20);
		Assert.Equal(10_000 - 20 + 1, result.Count());
		Assert.Equal(Enumerable.Range(50, 20), result.ElementAt(50));
		Assert.Equal(Enumerable.Range(9_980, 20), result.ElementAt(^1));
	}
}
