namespace SuperLinq.Async;

/// <summary>
/// Provides a set of static methods for querying objects that
/// implement <see cref="IAsyncEnumerable{T}" />.
/// </summary>
public static partial class AsyncSuperEnumerable
{
	internal static ConfiguredCancelableAsyncEnumerable<T>.Enumerator GetConfiguredAsyncEnumerator<T>(this IAsyncEnumerable<T> enumerable, CancellationToken cancellationToken) =>
		enumerable.ConfigureAwait(false).WithCancellation(cancellationToken).GetAsyncEnumerator();

	private static (bool HasValue, T Value) Some<T>(T value) => (true, value);

	private static Func<ValueTask> ToAsync(this Action action)
	{
		return () =>
		{
			action();
			return default;
		};
	}

	private static Func<T, ValueTask> ToAsync<T>(this Action<T> action)
	{
		return p1 =>
		{
			action(p1);
			return default;
		};
	}

	private static Func<ValueTask<TResult>> ToAsync<TResult>(this Func<TResult> func)
	{
		return () =>
		{
			var ret = func();
			return new ValueTask<TResult>(ret);
		};
	}
}
