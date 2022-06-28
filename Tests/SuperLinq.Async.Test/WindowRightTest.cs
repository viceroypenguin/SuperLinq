using SuperLinq;

namespace Test.Async;

public class WindowRightTest
{
	[Fact]
	public void WindowRightIsLazy()
	{
		new AsyncBreakingSequence<int>().WindowRight(1);
	}

	[Fact]
	public async Task WindowModifiedBeforeMoveNextDoesNotAffectNextWindow()
	{
		var sequence = AsyncEnumerable.Range(0, 3);
		await using var e = sequence.WindowRight(2).GetAsyncEnumerator();

		await e.MoveNextAsync();
		var window1 = e.Current;
		window1[0] = -1;
		await e.MoveNextAsync();
		var window2 = e.Current;

		Assert.Equal(0, window2[0]);
	}

	[Fact]
	public async Task WindowModifiedAfterMoveNextDoesNotAffectNextWindow()
	{
		var sequence = AsyncEnumerable.Range(0, 3);
		await using var e = sequence.WindowRight(2).GetAsyncEnumerator();

		await e.MoveNextAsync();
		var window1 = e.Current;
		await e.MoveNextAsync();
		window1[0] = -1;
		var window2 = e.Current;

		Assert.Equal(0, window2[0]);
	}

	[Fact]
	public async Task WindowModifiedDoesNotAffectPreviousWindow()
	{
		var sequence = AsyncEnumerable.Range(0, 3);
		await using var e = sequence.WindowRight(2).GetAsyncEnumerator();

		await e.MoveNextAsync();
		var window1 = e.Current;
		await e.MoveNextAsync();
		var window2 = e.Current;
		window2[0] = -1;

		Assert.Equal(0, window1[0]);
	}

	[Fact]
	public void WindowRightWithNegativeWindowSize()
	{
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			AsyncEnumerable.Repeat(1, 10).WindowRight(-5));
	}

	[Fact]
	public async Task WindowRightWithEmptySequence()
	{
		await using var xs = Enumerable.Empty<int>().AsTestingSequence();

		var result = xs.WindowRight(5);

		await result.AssertEmpty();
	}

	[Fact]
	public async Task  WindowRightWithSingleElement()
	{
		const int count = 100;
		var sequence = Enumerable.Range(1, count).ToArray();

		IList<int>[] result;
		await using (var ts = sequence.AsTestingSequence())
			result = await ts.WindowRight(1).ToArrayAsync();

		// number of windows should be equal to the source sequence length
		Assert.Equal(count, result.Length);

		// each window should contain single item consistent of element at that offset
		foreach (var (index, item) in result.Index())
			Assert.Equal(item.Single(), sequence[index]);
	}

	[Fact]
	public async Task WindowRightWithWindowSizeLargerThanSequence()
	{
		await using var sequence = AsyncEnumerable.Range(1, 5).AsTestingSequence();

		await using var reader = sequence.WindowRight(10).Read();
		(await reader.Read()).AssertSequenceEqual(1);
		(await reader.Read()).AssertSequenceEqual(1, 2);
		(await reader.Read()).AssertSequenceEqual(1, 2, 3);
		(await reader.Read()).AssertSequenceEqual(1, 2, 3, 4);
		(await reader.Read()).AssertSequenceEqual(1, 2, 3, 4, 5);
		await reader.ReadEnd();
	}

	[Fact]
	public async Task WindowRightWithWindowSizeSmallerThanSequence()
	{
		await using var sequence = AsyncEnumerable.Range(1, 5).AsTestingSequence();

		await using var reader = sequence.WindowRight(3).Read();
		(await reader.Read()).AssertSequenceEqual(1);
		(await reader.Read()).AssertSequenceEqual(1, 2);
		(await reader.Read()).AssertSequenceEqual(1, 2, 3);
		(await reader.Read()).AssertSequenceEqual(2, 3, 4);
		(await reader.Read()).AssertSequenceEqual(3, 4, 5);
		await reader.ReadEnd();
	}
}
