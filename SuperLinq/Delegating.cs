// https://github.com/atifaziz/Delegating

namespace Delegating;

static class Delegate
{
	public static IDisposable Disposable(Action delegatee) =>
		new DelegatingDisposable(delegatee);

	public static IObserver<T> Observer<T>(Action<T> onNext,
										   Action<Exception>? onError = null,
										   Action? onCompleted = null) =>
		new DelegatingObserver<T>(onNext, onError, onCompleted);
}

sealed class DelegatingDisposable : IDisposable
{
	Action? _delegatee;

	public DelegatingDisposable(Action delegatee) =>
		_delegatee = delegatee ?? throw new ArgumentNullException(nameof(delegatee));

	public void Dispose()
	{
		var delegatee = _delegatee;
		if (delegatee == null || Interlocked.CompareExchange(ref _delegatee, null, delegatee) != delegatee)
			return;
		delegatee();
	}
}

sealed class DelegatingObserver<T> : IObserver<T>
{
	readonly Action<T> _onNext;
	readonly Action<Exception>? _onError;
	readonly Action? _onCompleted;

	public DelegatingObserver(Action<T> onNext,
							  Action<Exception>? onError = null,
							  Action? onCompleted = null)
	{
		_onNext = onNext ?? throw new ArgumentNullException(nameof(onNext));
		_onError = onError;
		_onCompleted = onCompleted;
	}

	public void OnCompleted() => _onCompleted?.Invoke();
	public void OnError(Exception error) => _onError?.Invoke(error);
	public void OnNext(T value) => _onNext(value);
}
