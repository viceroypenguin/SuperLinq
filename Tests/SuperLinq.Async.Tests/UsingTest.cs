namespace SuperLinq.Async.Tests;

public sealed class UsingTest
{
	[Fact]
	public void UsingIsLazy()
	{
		_ = AsyncSuperEnumerable.Using(
			BreakingFunc.Of<IAsyncDisposable>(),
			BreakingFunc.Of<IAsyncDisposable, IAsyncEnumerable<int>>());
	}

	[Fact]
	public async Task UsingDisposesCorrectly()
	{
		var starts = 0;
		IEnumerable<int> Test(TestDisposable disposable)
		{
			starts++;
			Assert.False(disposable.IsDisposed);
			yield return 1;
			Assert.False(disposable.IsDisposed);
			yield return 2;
		}

		TestDisposable? dis = null;
		var seq = AsyncSuperEnumerable.Using(
			() => dis = new TestDisposable(),
			d => Test(d).ToAsyncEnumerable());
		Assert.Null(dis);
		Assert.Equal(0, starts);

		await seq.Consume();
		Assert.NotNull(dis);
		Assert.True(dis.IsDisposed);
		Assert.Equal(1, starts);
	}

	[Fact]
	public async Task UsingDisposesOnIterationError()
	{
		var starts = 0;
		IEnumerable<int> Test(TestDisposable disposable)
		{
			starts++;
			Assert.False(disposable.IsDisposed);
			yield return 1;
			Assert.False(disposable.IsDisposed);
			throw new TestException();
		}

		TestDisposable? dis = null;
		var seq = AsyncSuperEnumerable.Using(
			() => dis = new TestDisposable(),
			d => Test(d).ToAsyncEnumerable());
		Assert.Null(dis);
		Assert.Equal(0, starts);

		_ = await Assert.ThrowsAsync<TestException>(async () =>
			await seq.Consume());
		Assert.NotNull(dis);
		Assert.True(dis.IsDisposed);
		Assert.Equal(1, starts);
	}

	[Fact]
	public async Task UsingDisposesOnFunctionError()
	{
		TestDisposable? dis = null;
		var seq = AsyncSuperEnumerable.Using<int, TestDisposable>(
			() => dis = new TestDisposable(),
			_ => throw new TestException());
		Assert.Null(dis);

		_ = await Assert.ThrowsAsync<TestException>(async () =>
			await seq.Consume());
		Assert.NotNull(dis);
		Assert.True(dis.IsDisposed);
	}

	private sealed class TestDisposable : IAsyncDisposable
	{
		public bool IsDisposed { get; set; }
		public ValueTask DisposeAsync()
		{
			IsDisposed = true;
			return default;
		}
	}
}
