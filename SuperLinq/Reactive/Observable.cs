using Delegate = Delegating.Delegate;

namespace SuperLinq.Reactive;

/// <summary>
/// Provides a set of static methods for writing in-memory queries over observable sequences.
/// </summary>
static partial class Observable
{
	/// <summary>
	/// Subscribes an element handler and a completion handler to an
	/// observable sequence.
	/// </summary>
	/// <typeparam name="T">Type of elements in <paramref name="source"/>.</typeparam>
	/// <param name="source">Observable sequence to subscribe to.</param>
	/// <param name="onNext">
	/// Action to invoke for each element in <paramref name="source"/>.</param>
	/// <param name="onError">
	/// Action to invoke upon exceptional termination of the
	/// <paramref name="source"/>.</param>
	/// <param name="onCompleted">
	/// Action to invoke upon graceful termination of <paramref name="source"/>.</param>
	/// <returns>The subscription, which when disposed, will unsubscribe
	/// from <paramref name="source"/>.</returns>

	public static IDisposable Subscribe<T>(this IObservable<T> source, Action<T> onNext, Action<Exception>? onError = null, Action? onCompleted = null) =>
		source == null
		? throw new ArgumentNullException(nameof(source))
		: source.Subscribe(Delegate.Observer(onNext, onError, onCompleted));
}
