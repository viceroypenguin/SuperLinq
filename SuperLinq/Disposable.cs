using Delegate = Delegating.Delegate;

namespace SuperLinq;

static class Disposable
{
	public static readonly IDisposable Nop = Delegate.Disposable(delegate { });
}
