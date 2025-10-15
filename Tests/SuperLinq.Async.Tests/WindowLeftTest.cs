namespace SuperLinq.Async.Tests;

public sealed class WindowLeftTest
{
	[Fact]
	public void WindowLeftIsLazy()
	{
		_ = new AsyncBreakingSequence<int>().WindowLeft(1);
	}

	[Fact]
	public async Task WindowModifiedBeforeMoveNextDoesNotAffectNextWindow()
	{
		var sequence = AsyncEnumerable.Range(0, 3);
		await using var e = sequence.WindowLeft(2).GetAsyncEnumerator();

		_ = await e.MoveNextAsync();
		var window1 = e.Current;
		window1[1] = -1;
		_ = await e.MoveNextAsync();
		var window2 = e.Current;

		Assert.Equal(1, window2[0]);
	}

	[Fact]
	public async Task WindowModifiedAfterMoveNextDoesNotAffectNextWindow()
	{
		await using var sequence = AsyncEnumerable.Range(0, 3).AsTestingSequence();
		await using var e = sequence.WindowLeft(2).GetAsyncEnumerator();

		_ = await e.MoveNextAsync();
		var window1 = e.Current;
		_ = await e.MoveNextAsync();
		window1[1] = -1;
		var window2 = e.Current;

		Assert.Equal(1, window2[0]);
	}

	[Fact]
	public async Task WindowModifiedDoesNotAffectPreviousWindow()
	{
		await using var sequence = AsyncEnumerable.Range(0, 3).AsTestingSequence();
		await using var e = sequence.WindowLeft(2).GetAsyncEnumerator();

		_ = await e.MoveNextAsync();
		var window1 = e.Current;
		_ = await e.MoveNextAsync();
		var window2 = e.Current;
		window2[0] = -1;

		Assert.Equal(1, window1[1]);
	}

	[Fact]
	public void WindowLeftWithNegativeWindowSize()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() =>
			AsyncEnumerable.Repeat(1, 10).WindowLeft(-5));
	}

	[Fact]
	public async Task WindowLeftWithEmptySequence()
	{
		await using var xs = AsyncEnumerable.Empty<int>().AsTestingSequence();

		var result = xs.WindowLeft(5);

		await result.AssertEmpty();
	}

	[Fact]
	public async Task WindowLeftWithSingleElement()
	{
		var sequence = Enumerable.Range(1, 100).ToArray();

		IList<int>[] result;
		await using (var ts = sequence.AsTestingSequence())
			result = await ts.WindowLeft(1).ToArrayAsync();

		// number of windows should be equal to the source sequence length
		Assert.Equal(100, result.Length);

		// each window should contain single item consistent of element at that offset
		foreach (var (index, item) in result.Index())
			Assert.Equal(item.Single(), sequence[index]);
	}

	[Fact]
	public async Task WindowLeftWithWindowSizeLargerThanSequence()
	{
		await using var sequence = AsyncEnumerable.Range(1, 5).AsTestingSequence();

		await using var reader = sequence.WindowLeft(10).Read();

		(await reader.Read()).AssertSequenceEqual(1, 2, 3, 4, 5);
		(await reader.Read()).AssertSequenceEqual(2, 3, 4, 5);
		(await reader.Read()).AssertSequenceEqual(3, 4, 5);
		(await reader.Read()).AssertSequenceEqual(4, 5);
		(await reader.Read()).AssertSequenceEqual(5);
		await reader.ReadEnd();
	}

	[Fact]
	public async Task WindowLeftWithWindowSizeSmallerThanSequence()
	{
		await using var sequence = AsyncEnumerable.Range(1, 5).AsTestingSequence();

		await using var reader = sequence.WindowLeft(3).Read();

		(await reader.Read()).AssertSequenceEqual(1, 2, 3);
		(await reader.Read()).AssertSequenceEqual(2, 3, 4);
		(await reader.Read()).AssertSequenceEqual(3, 4, 5);
		(await reader.Read()).AssertSequenceEqual(4, 5);
		(await reader.Read()).AssertSequenceEqual(5);
		await reader.ReadEnd();
	}
}
