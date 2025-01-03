namespace SuperLinq.Tests;

public sealed class UsingTest
{
	[Test]
	public void UsingIsLazy()
	{
		_ = SuperEnumerable.Using(
			BreakingFunc.Of<IDisposable>(),
			BreakingFunc.Of<IDisposable, IEnumerable<int>>());
	}

	[Test]
	public void UsingDisposesCorrectly()
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
		var seq = SuperEnumerable.Using(
			() => dis = new TestDisposable(),
			Test);
		Assert.Null(dis);
		Assert.Equal(0, starts);

		seq.Consume();
		Assert.NotNull(dis);
		Assert.True(dis.IsDisposed);
		Assert.Equal(1, starts);
	}

	[Test]
	public void UsingDisposesOnIterationError()
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
		var seq = SuperEnumerable.Using(
			() => dis = new TestDisposable(),
			Test);
		Assert.Null(dis);
		Assert.Equal(0, starts);

		_ = Assert.Throws<TestException>(() => seq.Consume());
		Assert.NotNull(dis);
		Assert.True(dis.IsDisposed);
		Assert.Equal(1, starts);
	}

	[Test]
	public void UsingDisposesOnFunctionError()
	{
		TestDisposable? dis = null;
		var seq = SuperEnumerable.Using<int, TestDisposable>(
			() => dis = new TestDisposable(),
			_ => throw new TestException());
		Assert.Null(dis);

		_ = Assert.Throws<TestException>(() => seq.Consume());
		Assert.NotNull(dis);
		Assert.True(dis.IsDisposed);
	}

	private sealed class TestDisposable : IDisposable
	{
		public bool IsDisposed { get; set; }
		public void Dispose() => IsDisposed = true;
	}
}
