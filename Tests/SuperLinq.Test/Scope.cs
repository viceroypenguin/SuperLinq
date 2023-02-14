namespace Test;

internal abstract class Scope<T> : IDisposable
{
	private readonly T _old;

	protected Scope(T current)
	{
		_old = current;
	}

	public virtual void Dispose()
	{
		Restore(_old);
	}

	protected abstract void Restore(T old);
}
