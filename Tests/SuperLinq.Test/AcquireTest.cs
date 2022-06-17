namespace Test;

public class AcquireTest
{
	[Fact]
	public void AcquireAll()
	{
		Disposable a = null;
		Disposable b = null;
		Disposable c = null;

		var allocators = SuperEnumerable.From(() => a = new Disposable(),
											 () => b = new Disposable(),
											 () => c = new Disposable());

		var disposables = allocators.Acquire();

		Assert.Equal(3, disposables.Length);

		foreach (var disposable in disposables.ZipShortest(new[] { a, b, c }, (act, exp) => new { Actual = act, Expected = exp }))
		{
			Assert.Equal(disposable.Expected, disposable.Actual);
			Assert.False(disposable.Actual.Disposed);
		}
	}

	[Fact]
	public void AcquireSome()
	{
		Disposable a = null;
		Disposable b = null;
		Disposable c = null;

		var allocators = SuperEnumerable.From(() => a = new Disposable(),
											 () => b = new Disposable(),
											 () => throw new TestException(),
											 () => c = new Disposable());

		Assert.Throws<TestException>(() => allocators.Acquire());

		Assert.NotNull(a);
		Assert.True(a.Disposed);
		Assert.NotNull(b);
		Assert.True(b.Disposed);
		Assert.Null(c);
	}

	class Disposable : IDisposable
	{
		public bool Disposed { get; private set; }
		public void Dispose() { Disposed = true; }
	}
}
