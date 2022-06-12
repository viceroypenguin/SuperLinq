using NUnit.Framework;

namespace SuperLinq.Test;

[TestFixture]
public class AcquireTest
{
	[Test]
	public void AcquireAll()
	{
		Disposable a = null;
		Disposable b = null;
		Disposable c = null;

		var allocators = SuperEnumerable.From(() => a = new Disposable(),
											 () => b = new Disposable(),
											 () => c = new Disposable());

		var disposables = allocators.Acquire();

		Assert.That(disposables.Length, Is.EqualTo(3));

		foreach (var disposable in disposables.ZipShortest(new[] { a, b, c }, (act, exp) => new { Actual = act, Expected = exp }))
		{
			Assert.That(disposable.Actual, Is.SameAs(disposable.Expected));
			Assert.That(disposable.Actual.Disposed, Is.False);
		}
	}

	[Test]
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

		Assert.That(a, Is.Not.Null);
		Assert.That(a.Disposed, Is.True);
		Assert.That(b, Is.Not.Null);
		Assert.That(b.Disposed, Is.True);
		Assert.That(c, Is.Null);
	}

	class Disposable : IDisposable
	{
		public bool Disposed { get; private set; }
		public void Dispose() { Disposed = true; }
	}
}
