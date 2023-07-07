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

	private static Func<T, ValueTask<TResult>> ToAsync<T, TResult>(this Func<T, TResult> func)
	{
		return arg1 =>
		{
			var ret = func(arg1);
			return new ValueTask<TResult>(ret);
		};
	}

	private static Func<T1, T2, ValueTask<TResult>> ToAsync<T1, T2, TResult>(this Func<T1, T2, TResult> func)
	{
		return (arg1, arg2) =>
		{
			var ret = func(arg1, arg2);
			return new ValueTask<TResult>(ret);
		};
	}
}
